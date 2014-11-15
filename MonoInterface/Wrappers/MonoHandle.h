#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"

#include "Wrappers/MonoClass.h"

#include <ISystem.h>

//! Extra base class for mono handles.
struct MonoHandleBase : public IMonoHandle
{
protected:
	MonoClass *monoClass;
	IMonoClass *type;
public:
	MonoHandleBase()
		: type(nullptr)
		, monoClass(nullptr)
	{

	}
	//! Calls a Mono method associated with this object.
	virtual mono::object CallMethod(const char *name, IMonoArray *args);
	//! Gets the value of the object's field.
	virtual mono::object GetField(const char *name);
	//! Sets the value of the object's field.
	virtual void SetField(const char *name, mono::object value);
	//! Gets the value of the object's property.
	virtual mono::object GetProperty(const char *name);
	//! Sets the value of the object's property.
	virtual void SetProperty(const char *name, mono::object value);
	//! Gets the wrapper for the class of this object.
	virtual struct IMonoClass * GetClass();

	virtual void * UnboxObject();

	virtual void * GetWrappedPointer();
private:
	MonoClass *getMonoClass()
	{
		if (!this->monoClass)
		{
			this->monoClass = mono_object_get_class((MonoObject *)this->Get());
		}
		return this->monoClass;
	}
};

template<bool pinned>
struct MonoGCHandle : public MonoHandleBase
{
private:
	mono::object obj;
	unsigned int gc_handle;
public:
	MonoGCHandle(mono::object obj)
	{
		this->obj = obj;
		this->gc_handle = mono_gchandle_new((MonoObject *)obj, pinned);
	}

	virtual void Hold(mono::object object)
	{
		this->Release();
		this->obj = obj;
		this->gc_handle = mono_gchandle_new((MonoObject *)obj, pinned);
	}

	virtual void Release()
	{
		if (this->obj)
		{
			mono_gchandle_free(this->gc_handle);
			this->obj = nullptr;
		}
	}

	virtual mono::object Get()
	{
		if (pinned)
		{
			return this->obj;
		}
		return (mono::object)mono_gchandle_get_target(this->gc_handle);
	}
};
//! Represents persistent object wrapper.
struct MonoHandlePersistent : public MonoGCHandle < false >
{
	MonoHandlePersistent(mono::object obj) : MonoGCHandle(obj) {}
};
//! Represents pinned object wrapper.
struct MonoHandlePinned : public MonoGCHandle < true >
{
	MonoHandlePinned(mono::object obj) : MonoGCHandle(obj) {}
};

//! Represents a wrapper for MonoObject that doesn't pin the object in the heap.
//!
//! Never use objects of this type for longer then execution of one short method,
//! because there is a high chance that the object will be either moved or
//! destroyed on the next GC session.
struct MonoHandleFree : public MonoHandleBase
{
private:
	mono::object handle;
public:
	MonoHandleFree(mono::object obj)
	{
		this->handle = obj;
	}
	//! Encapsulates a different managed object into this wrapper.
	virtual void Hold(mono::object object) override
	{
		this->handle = object;
	}

	virtual void Release() override
	{
		this->handle = nullptr;
	}

	virtual mono::object Get() override
	{
		return this->handle;
	}

	virtual void * GetWrappedPointer() override
	{
		return this->handle;
	}

};