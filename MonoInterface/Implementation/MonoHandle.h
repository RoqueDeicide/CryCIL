#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"

#include "Implementation/MonoClass.h"

#include <ISystem.h>

//! Represents a wrapper for managed objects.
struct MonoHandle : public IMonoHandle
{
private:
	IMonoClass *klass;
	union
	{
		mono::object mObj;
		MonoObject *obj;
	};
public:
	MonoHandle(mono::object obj)
	{
		this->mObj = obj;
		this->klass = MonoClassCache::Wrap(mono_object_get_class(this->obj));
	}
	//! Gets the value of the object's field.
	virtual void GetField(const char *name, void *value);
	//! Sets the value of the object's field.
	virtual void SetField(const char *name, void *value);

	virtual IMonoProperty *GetProperty(const char *name);
	virtual IMonoEvent *GetEvent(const char *name);
	//! Gets the wrapper for the class of this object.
	virtual struct IMonoClass *GetClass();

	virtual void *GetWrappedPointer();

	virtual mono::object Get() { return this->mObj; }
};