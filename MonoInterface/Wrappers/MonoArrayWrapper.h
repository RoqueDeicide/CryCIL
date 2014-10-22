#pragma once

#include "IMonoInterface.h"

#include <mono/metadata/object.h>
#include <mono/metadata/appdomain.h>

#define GetWrappedArray (MonoArray *)this->GetWrappedPointer()
//! Implements most of the functionality of IMonoArray.
struct MonoArrayWrapper : IMonoArray
{
	//! Returns an element at specified position.
	virtual mono::object GetItem(int index)
	{
		return *(mono::object *)mono_array_addr_with_size
			(GetWrappedArray, this->GetElementSize(), index);
	}
	//! Sets an element at specified position.
	virtual void SetItem(int index, mono::object value)
	{
		*(mono::object *)mono_array_addr_with_size(GetWrappedArray, this->GetElementSize(), index) =
			value;
	}
protected:
	int elementSize = -1;
	int size = -1;
	IMonoClass *klass = nullptr;
	//! Returns number of elements in the array.
	virtual int GetSize()
	{
		if (this->size == -1)
		{
			this->size = (int)mono_array_length(GetWrappedArray);
		}
		return this->size;
	}

	int GetElementSize()
	{
		if (this->elementSize == -1)
		{
			this->elementSize =
				mono_array_element_size(mono_object_get_class((MonoObject *)this->GetWrappedPointer()));
		}
		return this->elementSize;
	}

	virtual IMonoClass * GetElementClass()
	{
		return MonoClassCache::Wrap
		(
			mono_class_get_element_class
			(
				mono_object_get_class((MonoObject *)this->GetWrappedPointer())
			)
		);
	}

};