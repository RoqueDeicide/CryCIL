#pragma once

#include "IMonoInterface.h"

#include <mono/metadata/object.h>
#include <mono/metadata/mono-gc.h>

//! Implementation for IMonoGC.
struct MonoGC : public IMonoGC
{
	MonoGC() {}
	
	virtual void Collect(int generation = -1)
	{
		mono_gc_collect(generation);
	}

	virtual unsigned int Hold(mono::object obj)
	{
		if (obj)
		{
			mono_gchandle_new_weakref((MonoObject *)obj, false);
		}
		ReportError("Attempted to create weak GC handle for null pointer.");
		return -1;
	}

	virtual unsigned int HoldWithHope(mono::object obj)
	{
		if (obj)
		{
			return mono_gchandle_new_weakref((MonoObject *)obj, true);
		}
		ReportError("Attempted to create weak GC handle for null pointer.");
		return -1;
	}

	virtual unsigned int Keep(mono::object obj)
	{
		if (obj)
		{
			return mono_gchandle_new((MonoObject *)obj, false);
		}
		ReportError("Attempted to create strong GC handle for null pointer.");
		return -1;
	}

	virtual unsigned int Pin(mono::object obj)
	{
		if (obj)
		{
			return mono_gchandle_new((MonoObject *)obj, true);
		}
		ReportError("Attempted to create pinning GC handle for null pointer.");
		return -1;
	}

	virtual int GetMaxGeneration()
	{
		return mono_gc_max_generation();
	}

	virtual __int64 GetHeapSize()
	{
		return mono_gc_get_heap_size();
	}

	virtual void ReleaseGCHandle(unsigned int handle)
	{
		if (handle == -1)
		{
			return;
		}
		mono_gchandle_free(handle);
	}

	virtual mono::object GetGCHandleTarget(unsigned int handle)
	{
		if (handle == -1)
		{
			return nullptr;
		}
		return (mono::object)mono_gchandle_get_target(handle);
	}
};