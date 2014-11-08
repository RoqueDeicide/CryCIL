#include "stdafx.h"
#include "API_ImplementationHeaders.h"


MonoArrayFree::MonoArrayFree(int size)
{
	this->size = size;

	this->wrappedArray =
		mono_array_new((MonoDomain *)MonoEnv->AppDomain, mono_get_object_class(), this->size);
}
//! Creates a new array of elements of specific type.
MonoArrayFree::MonoArrayFree(IMonoClass *elementClass, int size)
{
	this->size = size;

	this->wrappedArray = mono_array_new((MonoDomain *)MonoEnv->AppDomain,
		(MonoClass *)elementClass->GetWrappedPointer(), this->size);
}
//! Wraps an existing Mono array.
MonoArrayFree::MonoArrayFree(mono::object arrayHandle)
{
	this->wrappedArray = (MonoArray *)arrayHandle;
	this->size = mono_array_length(this->wrappedArray);
}
//! Returns a pointer to the wrapped array.
void *MonoArrayFree::GetWrappedPointer()
{
	return this->wrappedArray;
}
//! Does nothing since there is nothing to release.
void MonoArrayFree::Release()
{}