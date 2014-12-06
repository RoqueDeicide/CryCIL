#pragma once

#include "IMonoInterface.h"
#include "Wrappers/MonoClass.h"

#include <mono/metadata/object.h>
#include <mono/metadata/appdomain.h>

#define GetWrappedArray (MonoArray *)this->GetWrappedPointer()
//! Implements most of the functionality of IMonoArray.
struct MonoArrayWrapper : public IMonoArray
{
private:
	int elementSize;
	int size;
	IMonoClass *klass;
	MonoArray *arrayPtr;
public:
	MonoArrayWrapper()
		: elementSize(-1)
		, size(-1)
		, klass(nullptr)
		, arrayPtr(nullptr)
	{

	}
	MonoArrayWrapper(MonoArray *arrayPtr)
		: elementSize(-1)
		, size(-1)
		, klass(nullptr)
	{
		this->arrayPtr = arrayPtr;
	}
	MonoArrayWrapper(IMonoClass *elementClass, int size)
	{
		this->size = size;
		this->arrayPtr =
			mono_array_new(mono_domain_get(), (MonoClass *)elementClass->GetWrappedPointer(), this->size);
	}
	MonoArrayWrapper(int size)
	{
		this->size = size;

		this->arrayPtr = mono_array_new(mono_domain_get(), mono_get_object_class(), this->size);
	}

	virtual void *Item(int index);

	//! Returns number of elements in the array.
	virtual int GetSize();
	//! Gets the wrapper for class that represents elements of the array.
	virtual IMonoClass *GetElementClass();
	//! Returns the size of one element in the memory.
	int GetElementSize();
	virtual void *GetWrappedPointer();
};