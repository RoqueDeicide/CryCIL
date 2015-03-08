#pragma once

#include "IMonoAliases.h"

//! Represents an interface with Mono Garbage Collector (GC).
//!
//! Visit http://www.mono-project.com/docs/advanced/garbage-collector/sgen/ for detailed information
//! about GC used in Mono.
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
	//! @seealso HoldWithHope()
	//!
	//! @returns An identifier of the GC handle that can be implicitly converted to MonoGCHandle object.
	VIRTUAL_API virtual unsigned int Hold(mono::object obj) = 0;
	//! Holds given managed object allowing to get the reference to it wherever it can be after heap
	//! compression. The object will still be eligible for collection if it becomes unreachable from
	//! the rest of the managed code.
	//!
	//! Unlike Hold() this method creates a GC handle that can keep access to the collected object in
	//! case it was resurrected by the Finalize method (.Net/Mono equivalent of destructor) by means of
	//! creating a new strong reference to it.
	//!
	//! You have to use @see IMonoGCHandle::Object property to get the pointer to the object.
	//!
	//! @param obj Pointer to managed object to hold.
	//!
	//! @returns An identifier of the GC handle that can be implicitly converted to MonoGCHandle object.
	VIRTUAL_API virtual unsigned int HoldWithHope(mono::object obj) = 0;
	//! Keeps given managed object from being collected by GC.
	//!
	//! You will have to use @see IMonoGCHandle::Object property to get the valid pointer
	//! to the given object before using it.
	//!
	//! @param obj Pointer to managed object to keep.
	//!
	//! @returns An identifier of the GC handle that can be implicitly converted to MonoGCHandle object.
	VIRTUAL_API virtual unsigned int Keep(mono::object obj) = 0;
	//! Pins given managed object by prohibiting GC from collecting or moving it.
	//!
	//! As long as returned object is not released, it will be perfectly safe to access
	//! given object through its current pointer, as it will never be deleted or moved.
	//!
	//! @param obj Pointer to managed object to pin.
	//!
	//! @returns An identifier of the GC handle that can be implicitly converted to MonoGCHandle object.
	VIRTUAL_API virtual unsigned int Pin(mono::object obj) = 0;
	//! Releases the GC handle.
	//!
	//! @param handle Identifier of the handle that needs to be released.
	VIRTUAL_API virtual void ReleaseGCHandle(unsigned int handle) = 0;
	//! Acquires current position of managed object that is being held by the GC handle.
	//!
	//! @param handle Identifier of the handle.
	VIRTUAL_API virtual mono::object GetGCHandleTarget(unsigned int handle) = 0;

	VIRTUAL_API virtual int GetMaxGeneration() = 0;
	VIRTUAL_API virtual __int64 GetHeapSize() = 0;
};