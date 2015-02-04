#pragma once

#include "IMonoInterface.h"

#include "MonoClass.h"

struct MonoDelegateWrapper : public IMonoDelegate
{
private:
	union
	{
		MonoObject *obj;
		MonoDelegate *delegat;
		mono::delegat mDelegate;
	};
	IMonoClass *klass;
public:
	MonoDelegateWrapper(mono::delegat delegat)
	{
		this->mDelegate = delegat;
		klass = MonoClassCache::Wrap(mono_object_get_class(this->obj));
	}

	virtual IMonoMethod *GetMethod();

	virtual mono::object GetTarget();

	virtual void *GetFunctionPointer();

	virtual void *GetWrappedPointer();

	virtual mono::object Get();

	virtual void GetField(const char *name, void *value);

	virtual void SetField(const char *name, void *value);

	virtual IMonoProperty *GetProperty(const char *name);

	virtual IMonoEvent *GetEvent(const char *name);

	virtual IMonoClass *GetClass();

	virtual void Update(mono::object newLocation);

};