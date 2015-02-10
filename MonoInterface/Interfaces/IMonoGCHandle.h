#pragma once

#include "IMonoAliases.h"

//! Represents a GC handle.
//!
//! GC handles are used to inform Mono run-time environment about references to managed objects from
//! unmanaged memory.
//!
//! There are 4 types of GC handles, all to be used in various situations:
//!
//!     1) Weak - weak GC handles allow you to track position of the object in managed memory as it
//!        gets moved around by the garbage collector, however, it doesn't count as a reliable reference
//!        to the object, which means that, if there are no more reliable references (references in
//!        managed memory, strong GC handles), the object will become eligible for the garbage collection.
//!        Bear in mind that this type of GC handle loses access to the managed object that
//!        is set for collection, even it it gets resurrected by the Finalize() method of that object.
//!     2) Weak with resurrection tracking - very similar to the previous type of GC handle, except this
//!        one will not lose access to the object that was resurrected by the destructor (Finalize method).
//!     3) Strong - strong GC handles work as weak ones but they count as reliable references to the object
//!        preventing garbage collection of the object when only references to it are in unmanaged memory.
//!     4) Pin - pins are special strong GC handles that pin location of the managed object in place,
//!        preventing GC from collecting it or moving to a different location.
struct IMonoGCHandle
{
	virtual ~IMonoGCHandle() {}

	//! Gets the pointer to managed object that is being held by this GC handle.
	//!
	//! @returns A pointer that can be passed directly to methods and thunks, or null if
	//!          this is a weak handle and held object has been collected.
	__declspec(property(get = GetObjectPointer)) mono::object Object;

	//! Releases this GC handle.
	VIRTUAL_API virtual void Release() = 0;

	VIRTUAL_API virtual mono::object GetObjectPointer() = 0;
};