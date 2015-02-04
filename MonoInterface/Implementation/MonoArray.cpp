#include "stdafx.h"
#include "API_ImplementationHeaders.h"

#include "MonoDefinitionFiles/MonoArray.h"

void *MonoArrayWrapper::Item(int index)
{
	return mono_array_addr_with_size(this->arrayPtr, this->GetElementSize(), index);
}

int MonoArrayWrapper::GetLength(int dimensionIndex)
{
	if (!this->arrayPtr->bounds)
	{
		return this->arrayPtr->max_length;
	}
	return this->arrayPtr->bounds[dimensionIndex].length;
}

int MonoArrayWrapper::GetLowerBound(int dimensionIndex)
{
	if (!this->arrayPtr->bounds)
	{
		return 0;
	}
	return this->arrayPtr->bounds[dimensionIndex].lower_bound;
}

int MonoArrayWrapper::GetRank()
{
	return mono_class_get_rank(mono_object_get_class(this->obj));
}


//! Returns number of elements in the array.
int MonoArrayWrapper::GetSize()
{
	return this->arrayPtr->max_length;
}
//! Returns the size of one element in the memory.
int MonoArrayWrapper::GetElementSize()
{
	if (this->elementSize == -1)
	{
		this->elementSize =
			mono_array_element_size(mono_object_get_class(this->obj));
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
				mono_object_get_class(this->obj)
			)
		);
	}
	return this->klass;
}

void *MonoArrayWrapper::GetWrappedPointer()
{
	return this->arrayPtr;
}

mono::object MonoArrayWrapper::Get()
{
	return this->mArray;
}

void MonoArrayWrapper::GetField(const char *name, void *value)
{
	return this->GetClass()->GetField(this->mArray, name, value);
}

void MonoArrayWrapper::SetField(const char *name, void *value)
{
	this->GetClass()->SetField(this->mArray, name, value);
}

IMonoProperty *MonoArrayWrapper::GetProperty(const char *name)
{
	return this->GetClass()->GetProperty(name);
}

IMonoEvent *MonoArrayWrapper::GetEvent(const char *name)
{
	return this->GetClass()->GetEvent(name);
}

IMonoClass *MonoArrayWrapper::GetClass()
{
	return MonoClassCache::Wrap(mono_object_get_class(this->obj));
}

