#pragma once

#include "Wrappers/MonoArrayWrapper.h"
//! Represents an array of Object instances that is tracked by GC.
//!
//! @remark This type of array can be around for any amount of time.
struct MonoArrayPersistent : MonoArrayWrapper
{
private:
	unsigned int wrappedArrayHandle;
public:
	//! Creates a new simple array.
	MonoArrayPersistent(int size)
	{
		this->size = size;
		// Create an array.
		MonoArray *wrappedArray =
			mono_array_new((MonoDomain *)MonoEnv->AppDomain, mono_get_object_class(), this->size);
		// Hold it.
		this->wrappedArrayHandle = mono_gchandle_new((MonoObject *)wrappedArray, false);
	}
	//! Wraps an existing Mono array.
	MonoArrayPersistent(mono::object arrayHandle)
	{
		MonoArray *handle = (MonoArray *)arrayHandle;
		this->size = mono_array_length(handle);
		// Hold it.
		this->wrappedArrayHandle = mono_gchandle_new((MonoObject *)handle, false);
	}
	//! Returns a pointer to the wrapped array.
	virtual void * GetWrappedPointer()
	{
		if (this->wrappedArrayHandle == -1)
		{
			return nullptr;
		}
		return mono_gchandle_get_target(this->wrappedArrayHandle);
	}
	//! Removes GC tracking from this array, allowing it to be collected.
	virtual void Release()
	{
		mono_gchandle_free(this->wrappedArrayHandle);
		this->wrappedArrayHandle = -1;
	}
};