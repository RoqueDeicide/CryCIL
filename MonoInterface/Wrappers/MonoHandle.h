#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"

#include "Wrappers/MonoClass.h"

#include <ISystem.h>

//! Represents a wrapper for managed objects.
struct MonoHandle : public IMonoHandle
{
private:
	MonoClass *monoClass;
	IMonoClass *type;
	mono::object obj;
public:
	MonoHandle()
		: type(nullptr)
		, monoClass(nullptr)
		, obj(nullptr)
	{

	}
	MonoHandle(mono::object obj)
		: type(nullptr)
		, monoClass(nullptr)
	{
		this->obj = obj;
	}
	//! Calls a Mono method associated with this object.
	virtual mono::object CallMethod(const char *name, IMonoArray *args);
	//! Gets the value of the object's field.
	virtual void GetField(const char *name, void *value);
	//! Sets the value of the object's field.
	virtual void SetField(const char *name, void *value);

	virtual IMonoProperty *GetProperty(const char *name);
	virtual IMonoEvent *GetEvent(const char *name);
	//! Gets the wrapper for the class of this object.
	virtual struct IMonoClass *GetClass();

	virtual void *UnboxObject();

	virtual void *GetWrappedPointer();
private:
	MonoClass *getMonoClass()
	{
		if (!this->monoClass)
		{
			this->monoClass = mono_object_get_class((MonoObject *)this->Get());
		}
		return this->monoClass;
	}

	VIRTUAL_API virtual mono::object Get() { return this->obj; }
};