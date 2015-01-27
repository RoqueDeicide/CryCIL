#pragma once

#include "IMonoAliases.h"

//! Represents a GC handle.
struct IMonoGCHandle
{
	//! Gets the wrapper for managed object that is being held by this GC handle.
	//!
	//! @returns A pointer that provides access to object's API, or null if this is a weak
	//!          handle and held object has been collected.
	__declspec(property(get = GetObjectHandle)) IMonoHandle *ObjectHandle;
	//! Gets the pointer to managed object that is being held by this GC handle.
	//!
	//! @returns A pointer that can be passed directly to methods and thunks, or null if
	//!          this is a weak handle and held object has been collected.
	__declspec(property(get = GetObjectPointer)) mono::object ObjectPointer;

	//! Releases this GC handle.
	VIRTUAL_API virtual void Release() = 0;

	VIRTUAL_API virtual IMonoHandle *GetObjectHandle() = 0;
	VIRTUAL_API virtual mono::object GetObjectPointer() = 0;
};