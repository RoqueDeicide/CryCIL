#pragma once

#include <mono/metadata/object.h>

#include "IMonoInterface.h"

#include "Wrappers/MonoHandle.h"
struct MonoGCHandle : public IMonoGCHandle
{
protected:
	unsigned int handle;
public:
	VIRTUAL_API virtual void Release()
	{
		if (this->handle == -1)
		{
			return;
		}
		mono_gchandle_free(this->handle);
		this->handle = -1;
	}

	VIRTUAL_API virtual IMonoHandle *GetObjectHandle()
	{
		if (this->handle == -1)
		{
			return nullptr;
		}
		return new MonoHandle((mono::object)mono_gchandle_get_target(this->handle));
	}

	VIRTUAL_API virtual mono::object GetObjectPointer()
	{
		if (this->handle == -1)
		{
			return nullptr;
		}
		return (mono::object)mono_gchandle_get_target(this->handle);
	}
};

struct MonoGCHandleKeeper : public MonoGCHandle
{
	MonoGCHandleKeeper()
	{
		this->handle = -1;
	}
	MonoGCHandleKeeper(mono::object obj)
	{
		mono_gchandle_new((MonoObject *)obj, false);
	}
};
struct MonoGCHandleWeak : public MonoGCHandle
{
	MonoGCHandleWeak()
	{
		this->handle = -1;
	}
	MonoGCHandleWeak(mono::object obj)
	{
		mono_gchandle_new_weakref((MonoObject *)obj, false);
	}
};
struct MonoGCHandlePin : public MonoGCHandle
{
	MonoGCHandlePin()
	{
		this->handle = -1;
	}
	MonoGCHandlePin(mono::object obj)
	{
		mono_gchandle_new((MonoObject *)obj, true);
	}
};