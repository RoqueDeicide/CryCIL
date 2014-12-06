#include "stdafx.h"
#include "API_ImplementationHeaders.h"

MonoArrayPersistent::MonoArrayPersistent(int size)
{
	this->size = size;
	// Create an array.
	MonoArray *wrappedArray =
		mono_array_new((MonoDomain *)MonoEnv->AppDomain, mono_get_object_class(), this->size);
	// Hold it.
	this->wrappedArrayHandle = mono_gchandle_new((MonoObject *)wrappedArray, false);
}
//! Creates a new array of elements of specific type.
MonoArrayPersistent::MonoArrayPersistent(IMonoClass *elementClass, int size)
{
	this->size = size;
	// Create an array.
	MonoArray *wrappedArray = mono_array_new((MonoDomain *)MonoEnv->AppDomain,
		(MonoClass *)elementClass->GetWrappedPointer(), this->size);
	// Hold it.
	this->wrappedArrayHandle = mono_gchandle_new((MonoObject *)wrappedArray, false);
}
//! Wraps an existing Mono array.
MonoArrayPersistent::MonoArrayPersistent(mono::object arrayHandle)
{
	MonoArray *handle = (MonoArray *)arrayHandle;
	this->size = mono_array_length(handle);
	// Hold it.
	this->wrappedArrayHandle = mono_gchandle_new((MonoObject *)handle, false);
}
//! Returns a pointer to the wrapped array.
void *MonoArrayPersistent::GetWrappedPointer()
{
	if (this->wrappedArrayHandle == -1)
	{
		return nullptr;
	}
	return mono_gchandle_get_target(this->wrappedArrayHandle);
}
//! Removes GC tracking from this array, allowing it to be collected.
void MonoArrayPersistent::Release()
{
	mono_gchandle_free(this->wrappedArrayHandle);
	this->wrappedArrayHandle = -1;
}