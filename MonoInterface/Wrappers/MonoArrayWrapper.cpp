#include "stdafx.h"
#include "API_ImplementationHeaders.h"

void *MonoArrayWrapper::Item(int index)
{
	return mono_array_addr_with_size(GetWrappedArray, this->GetElementSize(), index);
}

//! Returns number of elements in the array.
int MonoArrayWrapper::GetSize()
{
	if (this->size == -1)
	{
		this->size = (int)mono_array_length(GetWrappedArray);
	}
	return this->size;
}
//! Returns the size of one element in the memory.
int MonoArrayWrapper::GetElementSize()
{
	if (this->elementSize == -1)
	{
		this->elementSize =
			mono_array_element_size(mono_object_get_class((MonoObject *)this->GetWrappedPointer()));
	}
	return this->elementSize;
}
//! Gets the wrapper for class that represents elements of the array.
IMonoClass *MonoArrayWrapper::GetElementClass()
{
	if (this->klass == nullptr)
	{
		this->klass = MonoClassCache::Wrap
		(
			mono_class_get_element_class
			(
				mono_object_get_class((MonoObject *)this->GetWrappedPointer())
			)
		);
	}
	return this->klass;
}

void *MonoArrayWrapper::GetWrappedPointer()
{
	return this->arrayPtr;
}