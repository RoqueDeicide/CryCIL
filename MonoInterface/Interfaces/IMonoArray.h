#pragma once

#include "IMonoAliases.h"

//! Defines interface of objects that wrap functionality of MonoArray type.
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