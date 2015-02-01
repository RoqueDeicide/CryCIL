#pragma once

#include "IMonoInterface.h"
#include "Implementation/MonoClass.h"

#include <mono/metadata/object.h>
#include <mono/metadata/appdomain.h>

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
	MonoArrayWrapper(int dimCount, unsigned int *lengths, IMonoClass *klass = nullptr, int *lowerBounds = nullptr)
	{
		bool lowerBoundsAllocated = false;
		if (!lowerBounds)
		{
			lowerBounds = new int[dimCount];
			memset(lowerBounds, 0, dimCount * sizeof(int));
			lowerBoundsAllocated = true;
		}
		MonoClass *arrayClass = mono_array_class_get((klass) ? (MonoClass *)klass->GetWrappedPointer() : mono_get_object_class(), dimCount);
		this->arrayPtr =
			mono_array_new_full(mono_domain_get(), arrayClass, lengths, lowerBounds);
	}

	virtual void *Item(int index);

	virtual int GetLength(int dimensionIndex);

	virtual int GetLowerBound(int dimensionIndex);

	//! Returns number of elements in the array.
	virtual int GetSize();
	//! Gets the wrapper for class that represents elements of the array.
	virtual IMonoClass *GetElementClass();
	//! Returns the size of one element in the memory.
	int GetElementSize();
	virtual void *GetWrappedPointer();

	virtual int GetRank();

	virtual mono::object Get();

	virtual mono::object CallMethod(const char *name, IMonoArray *args);

	virtual void GetField(const char *name, void *value);

	virtual void SetField(const char *name, void *value);

	virtual IMonoProperty *GetProperty(const char *name);

	virtual IMonoEvent *GetEvent(const char *name);

	virtual IMonoClass *GetClass();

	virtual void *UnboxObject();

};