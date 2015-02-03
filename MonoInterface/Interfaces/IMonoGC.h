#pragma once

#include "IMonoAliases.h"

//! Represents an interface with Mono Garbage Collector (GC).
struct IMonoGC
{
	virtual ~IMonoGC() {}

	//! Gets index of the oldest generation that is used by GC.
	__declspec(property(get = GetMaxGeneration)) int MaxGeneration;
	//! Gets number of bytes that are currently allocated by this GC.
	__declspec(property(get = GetHeapSize)) __int64 HeapSize;

	//! Triggers garbage collection.
	//!
	//! @param generation Index of generation to clean up. Bigger number means older generation
	//!                   to collect garbage in. Clean up of older generations is always preceded
	//!                   by GCion of younger ones. Invocation with -1 or @see IMonoGC::MaxGeneration
	//!                   will trigger full scale garbage collection.
	VIRTUAL_API virtual void Collect(int generation = -1) = 0;
	//! Holds given managed object allowing to get the reference to it wherever it can be after heap
	//! compression. The object will still be eligible for collection if it becomes unreachable from
	//! the rest of the managed code.
	//!
	//! You have to use @see IMonoGCHandle::Object property to get the pointer to the object.
	//!
	//! @param obj Pointer to managed object to hold.
	VIRTUAL_API virtual IMonoGCHandle *Hold(mono::object obj) = 0;
	//! Keeps given managed object from being collected by GC.
	//!
	//! You will have to use @see IMonoGCHandle::Object property to get the valid pointer
	//! to the given object before using it.
	//!
	//! @param obj Pointer to managed object to keep.
	VIRTUAL_API virtual IMonoGCHandle *Keep(mono::object obj) = 0;
	//! Pins given managed object by prohibiting GC from collecting or moving it.
	//!
	//! As long as returned object is not released, it will be perfectly safe to access
	//! given object through its current pointer, as it will never be deleted or moved.
	//!
	//! @param obj Pointer to managed object to pin.
	VIRTUAL_API virtual IMonoGCHandle *Pin(mono::object obj) = 0;

	VIRTUAL_API virtual int GetMaxGeneration() = 0;
	VIRTUAL_API virtual __int64 GetHeapSize() = 0;
};