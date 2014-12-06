#pragma once

#include "IMonoInterface.h"
#include "Wrappers/MonoGCHandle.h"

#include <mono/metadata/object.h>
#include <mono/metadata/mono-gc.h>

struct MonoGC : public IMonoGC
{
	MonoGC() {}
	VIRTUAL_API virtual void Collect(int generation = -1)
	{
		mono_gc_collect(generation);
	}

	VIRTUAL_API virtual IMonoGCHandle *Hold(mono::object obj)
	{
		if (obj)
		{
			return new MonoGCHandleWeak(obj);
		}
	}

	VIRTUAL_API virtual IMonoGCHandle *Keep(mono::object obj)
	{
		if (obj)
		{
			return new MonoGCHandleKeeper(obj);
		}
	}

	VIRTUAL_API virtual IMonoGCHandle *Pin(mono::object obj)
	{
		if (obj)
		{
			return new MonoGCHandlePin(obj);
		}
	}

	VIRTUAL_API virtual int GetMaxGeneration()
	{
		return mono_gc_max_generation();
	}

	VIRTUAL_API virtual __int64 GetHeapSize()
	{
		return mono_gc_get_heap_size();
	}

};