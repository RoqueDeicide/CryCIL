/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// IMonoArray interface for external projects, i.e. CryGame.
//////////////////////////////////////////////////////////////////////////
// 18/12/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __I_MONO_ARRAY_H__
#define __I_MONO_ARRAY_H__

#include <IMonoObject.h>

namespace mono
{
	class _string; typedef _string* string;
};

struct IMonoObject;

/// <summary>
/// Used to wrap arrays sent from C#, and also used when passing arrays of elements from C++.
///
/// Creating an IMonoArray: IMonoArray *pMyArray = CreateMonoArray(arraySize);
/// Converting an mono::array: IMonoArray *pConvertedArray = *(mono::array)monoArray;
/// </summary>
struct IMonoArray : public IMonoObject
{
public:
	/// <summary>
	/// Clears the array of all its elements.
	/// Note that the pre-determined size set when the array was created remains.
	/// </summary>
	virtual void Clear() = 0;

	/// <summary>
	/// Removes the object at the specified index.
	/// </summary>
	virtual void Remove(int index) = 0;

	/// <summary>
	/// Resizes the array.
	/// </summary>
	virtual void Resize(int size) = 0;
	/// <summary>
	/// Retrieves the size of the array.
	/// </summary>
	virtual int GetSize() const = 0;

	/// <summary>
	/// Clones the array.
	/// </summary>
	virtual IMonoArray *Clone() = 0;

	/// <summary>
	/// Gets the type of objects stored in this array.
	/// </summary>
	virtual IMonoClass *GetElementClass() = 0;

	/// <summary>
	/// Gets the default element class, used when a specific element types isn't specified.
	/// </summary>
	virtual IMonoClass *GetDefaultElementClass() = 0;

	/// <summary>
	/// Retrieves an IMonoObject at the selected index of the array.
	/// </summary>
	virtual IMonoObject *GetItem(int index) = 0;
	/// <summary>
	/// Retrieves an unboxed IMonoObject at the selected index of the array.
	/// </summary>
	template <class T>
	T GetItemUnboxed(int index)
	{
		IMonoObject *result = GetItem(index);
		if (result)
		{
			IMonoObject *pResult = *result;
			T tResult = pResult->Unbox<T>();
			SAFE_RELEASE(pResult);

			return tResult;
		}

		return nullptr;
	}

	template <typename T>
	inline void Insert(T value, int index = -1);

	template<>
	inline void Insert(mono::string monoString, int index);

	/// <summary>
	/// Inserts a native pointer into the array at the specified index. (-1 = back)
	/// Note: This method is currently very unreliable.
	/// </summary>
	virtual void InsertNativePointer(void *ptr, int index = -1) = 0;

	/// <summary>
	/// Inserts an MonoAnyValue object into the array at the specified index. (-1 = back)
	/// </summary>
	virtual void InsertAny(MonoAnyValue value, int index = -1) = 0;

	/// <summary>
	/// Inserts a managed object into the array at the specified index. (-1 = back)
	/// </summary>
	virtual void InsertMonoObject(IMonoObject *object, int index = -1) = 0;

	/// <summary>
	/// Inserts an mono string into the array at the specified index. (-1 = back)
	//// C# equivalent type: string
	/// </summary>
	virtual void InsertMonoString(mono::string string, int index = -1) = 0;
};

template <typename T>
inline void IMonoArray::Insert(T value, int index) { InsertAny(MonoAnyValue((T)value), index); }

template<>
inline void IMonoArray::Insert(mono::string monoString, int index) { InsertMonoString(monoString, index); }

#endif //__I_MONO_ARRAY_H__