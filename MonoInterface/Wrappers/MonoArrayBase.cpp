#include "stdafx.h"
#include "API_ImplementationHeaders.h"

mono::object MonoArrayBase::GetItem(int index)
{
	return *(mono::object *)mono_array_addr_with_size
		(GetWrappedArray, this->GetElementSize(), index);
}
//! Sets an element at specified position.
void MonoArrayBase::SetItem(int index, mono::object value)
{
	*(mono::object *)mono_array_addr_with_size(GetWrappedArray, this->GetElementSize(), index) =
		value;
}

//! Returns number of elements in the array.
int MonoArrayBase::GetSize()
{
	if (this->size == -1)
	{
		this->size = (int)mono_array_length(GetWrappedArray);
	}
	return this->size;
}
//! Returns the size of one element in the memory.
int MonoArrayBase::GetElementSize()
{
	if (this->elementSize == -1)
	{
		this->elementSize =
			mono_array_element_size(mono_object_get_class((MonoObject *)this->GetWrappedPointer()));
	}
	return this->elementSize;
}
//! Gets the wrapper for class that represents elements of the array.
IMonoClass *MonoArrayBase::GetElementClass()
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