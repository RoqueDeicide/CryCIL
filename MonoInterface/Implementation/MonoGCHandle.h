#pragma once

#include <mono/metadata/object.h>

#include "IMonoInterface.h"

#include "Implementation/MonoHandle.h"
struct MonoGCHandle : public IMonoGCHandle
{
protected:
	unsigned int handle;
public:
	virtual ~MonoGCHandle()
	{
		this->Release();
	}
	virtual void Release()
	{
		if (this->handle == -1)
		{
			return;
		}
		mono_gchandle_free(this->handle);
		this->handle = -1;
	}

	virtual mono::object GetObjectPointer()
	{
		if (this->handle == -1)
		{
			return nullptr;
		}
		return (mono::object)mono_gchandle_get_target(this->handle);
	}
};

struct MonoGCHandleStrong : public MonoGCHandle
{
	MonoGCHandleStrong()
	{
		this->handle = -1;
	}
	MonoGCHandleStrong(mono::object obj, bool pin)
	{
		this->handle = mono_gchandle_new((MonoObject *)obj, pin);
	}
};
struct MonoGCHandleWeak : public MonoGCHandle
{
	MonoGCHandleWeak()
	{
		this->handle = -1;
	}
	MonoGCHandleWeak(mono::object obj, bool trackResurrection)
	{
		this->handle = mono_gchandle_new_weakref((MonoObject *)obj, trackResurrection);
	}
};