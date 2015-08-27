#pragma once

#include "IMonoAliases.h"

typedef struct
{
	unsigned int length;
	int lower_bound;
} MonoArrayBounds;

struct _MonoArray
{
	MonoObject obj;
	/* bounds is NULL for szarrays */
	MonoArrayBounds *bounds;
	/* total number of elements of the array */
	unsigned int max_length;
	/* we use double to ensure proper alignment on platforms that need it */
	double vector[MONO_ZERO_LEN_ARRAY];
};

//! Defines interface of objects that wrap functionality of MonoArray type.
//!
//! All arrays wrapped with this type are the same as ordinary Mono arrays.
//! Their size is fixed, and they can only hold objects of specific type. (Although it is possible
//! to screw with them through pointers).
//!
//! Examples:
//!
//! We can simply create and fill an array of integers:
//!
//! @code{.cpp}
//!
//! // Create the array. Specify the class and length
//! IMonoArray<int> array = MonoEnv->Objects->Arrays->Create(MonoEnv->CoreLibrary->Int32, 10);
//!
//! // Fill it.
//! for (int i = 0; i < array.Length; i++)
//! {
//!     array[i] = i * 2;
//! }
//!
//! @endcode
//!
//! We can also wrap existing array that may be returned by other functions.
//!
//! @code{.cpp}
//!
//! // Get the array and immediately wrap it.
//! IMonoArray<mono::string> array = GetArray();
//!
//! // Let's say, we've got the array of text information. Specifically, with 3 objects in it.
//! CryLogAlways(array.ElementClass->Name);		// Prints "String".
//! CryLogAlways(array.Length);					// Prints "3".
//!
//! // Print the contents.
//! for (int i = 0; i < array.Length; i++)
//! {
//!     CryLogAlways(NtText(array[i]));
//! }
//!
//! @endcode
//!
//! We can also create object[] arrays and use them to pass arguments to functions.
//!
//! @code{.cpp}
//!
//! // When creating object[] array, we don't need to specify the class.
//! IMonoArray<mono::object> pars = MonoEnv->Objects->Arrays->Create(4);
//!
//! // We can put anything into those things, however any value-type objects must be boxed
//! // manually prior to insertion.
//! pars[0] = ToMonoString("Some text");
//! pars[1] = Box(Vec2(10, 20));
//! pars[2] = Box(Vec2(20, 10));
//! pars[3] = SomeAssembly->GetClass("Boo", "Foo")->GetConstructor()->Create();
//!
//! // Invoke the method using this array.
//! // Method's parameters: System.String, CryCil.Vector2, CryCil.Vector2, Boo.Foo.
//! IMonoStaticMethod *method =
//!     SomeAssembly->GetClass("BumBum", "Blabla")
//!                 ->GetMethod
//!                 (
//!                     "Meth",
//!                      "System.String, CryCil.Vector2, CryCil.Vector2, Boo.Foo"
//!                 )
//!                 ->ToStatic();
//!
//! method->Invoke(pars);
//!
//! @endcode
//!
//! We can also work with multi-dimensional arrays.
//!
//! @code{.cpp}
//!
//! // Get the array and immediately wrap it.
//! IMonoArray<double> array = GetArray();
//!
//! // Print the contents.
//! for (int i = array.GetLowerBound(0); i < array.GetLength(0); i++)
//! {
//!     CryLogAlways("{ %f, %f, %f }",
//!                  array[List<int>(2).Add(i, 1)],
//!                  array[List<int>(2).Add(i, 2)],
//!                  array[List<int>(2).Add(i, 3)]);
//! }
//!
//! @endcode
template<typename ElementType>
struct IMonoArray : public IMonoObject
{
protected:
	int         elementSize;			//!< Size of elements of this array.
	IMonoClass *elementClass;			//!< Class that represents elements of this array.
	int         rank;					//!< Number of dimensions this array has.
public:
	//! Gets the length of the array.
	__declspec(property(get = GetSize)) int Length;
	//! Gets the length of the array.
	__declspec(property(get = GetElementSize)) int ElementSize;
	//! Gets number of dimensions this array has.
	__declspec(property(get = GetRank)) int Rank;
	//! Gets the type of the elements of the array.
	__declspec(property(get = GetElementClass)) IMonoClass *ElementClass;

	//! Creates new wrapper for given array.
	IMonoArray(mono::Array ar)
		: elementSize(0)
		, elementClass(nullptr)
		, rank(0)
	{
		this->Update(ar);
	}
	//! Creates new wrapper for given array.
	IMonoArray(MonoGCHandle &handle)
		: elementSize(0)
		, elementClass(nullptr)
		, rank(0)
	{
		this->Update(handle.Object);
	}
private:
	void Update(mono::Array ar)
	{
		if (ar)
		{
			this->obj = ar;
			IMonoObjects *objs = MonoEnv->Objects;
			this->klass = objs->GetObjectClass(ar);
			this->elementClass = objs->GetArrayElementClass(ar);
			this->elementSize = objs->GetArrayElementSize(ar);
			this->rank = objs->GetArrayRank(ar);
		}
	}
public:
	//! Updates the array data.
	IMonoObject &operator=(mono::object obj)
	{
		this->Update(obj);
		return *this;
	}
	//! Updates the array data.
	IMonoObject &operator=(const MonoGCHandle &handle)
	{
		this->Update(handle.Object);
		return *this;
	}
	//! Provides read/write access to the element of the array.
	//!
	//! @param index Zero-based index of the item to access.
	//!
	//! @returns Reference to the element of the array.
	ElementType& operator[](int index)
	{
		if (!this->obj)
		{
			return nullptr;
		}

		_MonoArray *a = reinterpret_cast<_MonoArray *>(this->obj);
		return *reinterpret_cast<ElementType *>(reinterpret_cast<char*>((a)->vector) + this->elementSize * index);
	}
	//! Provides read/write access to the element of the array.
	//!
	//! @param indices A list of indices that identify location of the element on the array.
	//!
	//! @returns Reference to the element of the array.
	ElementType& operator[](List<int> &indices)
	{
		_MonoArray *a = reinterpret_cast<_MonoArray *>(this->obj);
		// Checking everything.
		if (indices.Length == 0)
		{
			CryFatalError("Unable to access an element of the array using no indices.");
		}
		if (!a->bounds)
		{
			if (indices.Length == 1)
			{
				return this->operator[](indices[0]);
			}
			CryFatalError("Attempt was made to access an element of 1D array via more then 1 index.");
		}
		if (indices.Length != this->rank)
		{
			CryFatalError("Unable to access an element of the array using invalid number of indices.");
		}
		int position = 0;
		for (int i = 0; i < this->rank; i++)
		{
			MonoArrayBounds bounds = a->bounds[i];
			int index = indices[i];
			if (index < bounds.lower_bound || index - bounds.lower_bound >= bounds.length)
			{
				CryFatalError("The index #%d is out of range of the Mono array's dimension.", i);
			}
			position *= bounds.length;
			position += index - bounds.lower_bound;
		}
		return this->operator[](position);
	}
	//! Gets the length of the dimension of the array.
	//!
	//! @param dimensionIndex Zero-based index of the dimension which length to get.
	int GetLength(int dimensionIndex) const
	{
		MonoArray *arrayPtr = reinterpret_cast<MonoArray *>(this->obj);
		if (!arrayPtr->bounds)
		{
			return 0;
		}
		return arrayPtr->bounds[dimensionIndex].length;
	}
	//! Gets the lower bound of the dimension of the array.
	//!
	//! @param dimensionIndex Zero-based index of the dimension which lower bound to get.
	int GetLowerBound(int dimensionIndex) const
	{
		MonoArray *arrayPtr = reinterpret_cast<MonoArray *>(this->obj);
		if (!arrayPtr->bounds)
		{
			return 0;
		}
		return arrayPtr->bounds[dimensionIndex].lower_bound;
	}

	int GetSize() const
	{
		MonoArray *arrayPtr = reinterpret_cast<MonoArray *>(this->obj);
		return arrayPtr->max_length;
	}
	int GetElementSize() const
	{
		return this->elementSize;
	}
	int GetRank() const
	{
		return this->rank;
	}
	IMonoClass *GetElementClass() const
	{
		return this->elementSize;
	}
};