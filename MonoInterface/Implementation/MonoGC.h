#pragma once

#include "IMonoInterface.h"

#include <mono/metadata/object.h>
#include <mono/metadata/mono-gc.h>

#if 1
#define GcMessage CryLogAlways
#else
#define GcMessage(...) void(0)
#endif

//! Implementation for IMonoGC.
struct MonoGC : public IMonoGC
{
	MonoGC() {}
	
	virtual void Collect(int generation = -1) override
	{
		GcMessage("Triggering the garbage collection.");

		mono_gc_collect(generation == -1 ? mono_gc_max_generation() : generation);

		GcMessage("Triggered the garbage collection.");
	}

	virtual unsigned int Hold(mono::object obj) override
	{
		if (obj)
		{
			mono_gchandle_new_weakref(reinterpret_cast<MonoObject *>(obj), false);
		}
		//ReportError("Attempted to create weak GC handle for null pointer.");
		return -1;
	}

	virtual unsigned int HoldWithHope(mono::object obj) override
	{
		if (obj)
		{
			return mono_gchandle_new_weakref(reinterpret_cast<MonoObject *>(obj), true);
		}
		//ReportError("Attempted to create weak GC handle for null pointer.");
		return -1;
	}

	virtual unsigned int Keep(mono::object obj) override
	{
		if (obj)
		{
			return mono_gchandle_new(reinterpret_cast<MonoObject *>(obj), false);
		}
		//ReportError("Attempted to create strong GC handle for null pointer.");
		return -1;
	}

	virtual unsigned int Pin(mono::object obj) override
	{
		if (obj)
		{
			return mono_gchandle_new(reinterpret_cast<MonoObject *>(obj), true);
		}
		//ReportError("Attempted to create pinning GC handle for null pointer.");
		return -1;
	}

	virtual int GetMaxGeneration() override
	{
		return mono_gc_max_generation();
	}

	virtual __int64 GetHeapSize() override
	{
		return mono_gc_get_heap_size();
	}

	virtual void ReleaseGCHandle(unsigned int handle) override
	{
		if (handle == -1)
		{
			return;
		}
		mono_gchandle_free(handle);
	}

	virtual mono::object GetGCHandleTarget(unsigned int handle) override
	{
		if (handle == -1)
		{
			return nullptr;
		}
		return mono::object(mono_gchandle_get_target(handle));
	}
};