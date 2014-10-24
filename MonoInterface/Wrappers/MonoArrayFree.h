#pragma once

#include "Wrappers/MonoArrayWrapper.h"
//! Represents an array of Object instances that is not tracked by GC.
//!
//! @remark This type of array should not be around for too long,
//!         because it is in danger of being GC-ed.
struct MonoArrayFree : MonoArrayWrapper
{
private:
	int size;
	MonoArray *wrappedArray;
public:
	//! Creates a new simple array.
	MonoArrayFree(int size)
	{
		this->size = size;

		this->wrappedArray =
			mono_array_new((MonoDomain *)MonoEnv->AppDomain, mono_get_object_class(), this->size);
	}
	//! Creates a new array of elements of specific type.
	MonoArrayFree(IMonoClass *elementClass, int size)
	{
		this->size = size;

		this->wrappedArray = mono_array_new((MonoDomain *)MonoEnv->AppDomain,
								(MonoClass *)elementClass->GetWrappedPointer(), this->size);
	}
	//! Wraps an existing Mono array.
	MonoArrayFree(mono::object arrayHandle)
	{
		this->wrappedArray = (MonoArray *)arrayHandle;
		this->size = mono_array_length(this->wrappedArray);
	}
	//! Returns a pointer to the wrapped array.
	virtual void * GetWrappedPointer()
	{
		return this->wrappedArray;
	}
	//! Does nothing since there is nothing to release.
	virtual void Release()
	{
	}
};