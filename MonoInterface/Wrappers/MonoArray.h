#pragma once

#include "IMonoInterface.h"
#include "Wrappers/MonoClass.h"

#include <mono/metadata/object.h>
#include <mono/metadata/appdomain.h>

#define GetWrappedArray (MonoArray *)this->GetWrappedPointer()
//! Implements most of the functionality of IMonoArray.
struct MonoArrayBase : public IMonoArray
{
protected:
	int elementSize;
	int size;
	IMonoClass *klass;
public:
	MonoArrayBase()
		: elementSize(-1)
		, size(-1)
		, klass(nullptr)
	{

	}

	//! Returns an element at specified position.
	virtual mono::object GetItem(int index);
	//! Sets an element at specified position.
	virtual void SetItem(int index, mono::object value);

	//! Returns number of elements in the array.
	virtual int GetSize();
	//! Returns the size of one element in the memory.
	int GetElementSize();
	//! Gets the wrapper for class that represents elements of the array.
	virtual IMonoClass *GetElementClass();
};

//! Represents an array of Object instances that is not tracked by GC.
//!
//! This type of array should not be around for too long,
//! because it is in danger of being GC-ed.
struct MonoArrayFree : public MonoArrayBase
{
private:
	int size;
	MonoArray *wrappedArray;
public:
	//! Creates a new simple array.
	MonoArrayFree(int size);
	//! Creates a new array of elements of specific type.
	MonoArrayFree(IMonoClass *elementClass, int size);
	//! Wraps an existing Mono array.
	MonoArrayFree(mono::object arrayHandle);
	//! Returns a pointer to the wrapped array.
	virtual void * GetWrappedPointer();
	//! Does nothing since there is nothing to release.
	virtual void Release();
};
//! Represents an array of Object instances that is tracked by GC.
//!
//! @remark This type of array can be around for any amount of time.
struct MonoArrayPersistent : public MonoArrayBase
{
private:
	unsigned int wrappedArrayHandle;
public:
	//! Creates a new simple array.
	MonoArrayPersistent(int size);
	//! Creates a new array of elements of specific type.
	MonoArrayPersistent(IMonoClass *elementClass, int size);
	//! Wraps an existing Mono array.
	MonoArrayPersistent(mono::object arrayHandle);
	//! Returns a pointer to the wrapped array.
	virtual void * GetWrappedPointer();
	//! Removes GC tracking from this array, allowing it to be collected.
	virtual void Release();
};