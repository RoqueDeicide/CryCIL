#pragma once

#include "IMonoAliases.h"

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
//! IMonoArray *array = MonoEnv->CreateArray(MonoEnv->CoreLibrary->GetClass("System", "Int32"), 10);
//!
//! // Fill it.
//! for (int i = 0; i < array->Length; i++)
//! {
//!     array->At<int>(i) = i * 2;
//! }
//!
//! @endcode
//!
//! We can also wrap existing array that may be returned by other functions.
//!
//! @code{.cpp}
//!
//! // Get the array and immediately wrap it.
//! IMonoArray *array = MonoEnv->WrapArray(GetArray());
//!
//! // Let's say, we've got the array of text information. Specifically, with 3 objects in it.
//! CryLogAlways(array->ElementClass->Name);		// Prints "String".
//! CryLogAlways(array->Length);					// Prints "3".
//!
//! // Print the contents.
//! for (int i = 0; i < array->Length; i++)
//! {
//!     const char *content = ToNativeString(array->At<mono::string>(i));
//!     CryLogAlways(content);
//!     delete content;				// Always clean-up afterwards.
//! }
//!
//! @endcode
//!
//! We can also create object[] arrays and use them to pass arguments to functions.
//!
//! @code{.cpp}
//!
//! // When creating object[] array, we don't need to specify the class.
//! IMonoArray *pars = MonoEnv->CreateArray(4);
//!
//! // We can put anything into those things, however any value-type objects must be boxed
//! // manually prior to insertion.
//! pars->At<mono::string>(0) = ToMonoString("Some text");
//! pars->At<mono::vector2>(1) = Box(Vec2(10, 20));
//! pars->At<mono::vector2>(2) = Box(Vec2(20, 10));
//! pars->At<mono::object>(3) = MonoEnv->CreateObject(SomeAssembly, "Boo", "Foo");
//!
//! // Invoke the method using this array.
//! // Method's parameters: System.String, CryCil.Geometry.Vector2, CryCil.Geometry.Vector2, Boo.Foo.
//! IMonoMethod *method =
//!     SomeAssembly->GetClass("BumBum", "Blabla")
//!                 ->GetMethod
//!                 (
//!                     "Meth",
//!                      "System.String, CryCil.Geometry.Vector2, CryCil.Geometry.Vector2, Boo.Foo"
//!                 );
//!
//! method->Invoke(nullptr, pars);
//!
//! @endcode
struct IMonoArray : public IMonoFunctionalityWrapper
{
	//! Gets the length of the array.
	__declspec(property(get = GetSize)) int Length;
	//! Gets the type of the elements of the array.
	__declspec(property(get = GetElementClass)) IMonoClass *ElementClass;
	//! Provides access to the item.
	//!
	//! Don't hesitate on dereferencing returned pointer: Mono arrays have tendency of
	//! being moved around the memory.
	//!
	//! @param index Zero-based index of the item to access.
	//!
	//! @returns Pointer to the item. The pointer is either mono::object, if this is an
	//!          array of reference types, or a pointer to a struct that can be easily
	//!          dereferenced, if this is an array of value types.
	VIRTUAL_API virtual void *Item(int index) = 0;
	template<typename T> T& At(int index)
	{
		return *(T *)this->Item(index);
	}

	VIRTUAL_API virtual int GetSize() = 0;
	VIRTUAL_API virtual IMonoClass *GetElementClass() = 0;
};