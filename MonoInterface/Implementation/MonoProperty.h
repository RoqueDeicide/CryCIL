#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"

struct MonoPropertyWrapper : public IMonoProperty
{
private:
	MonoProperty *prop;
	IMonoClass *klass;
	IMonoMethod *getter;
	IMonoMethod *setter;
public:
	MonoPropertyWrapper(MonoProperty *prop, IMonoClass *klass = nullptr)
		: getter(nullptr)
		, setter(nullptr)
	{
		this->prop = prop;
		this->klass = klass;

		MonoMethod *getterMethod = mono_property_get_get_method(prop);
		MonoMethod *setterMethod = mono_property_get_set_method(prop);
		if (getterMethod)
		{
			this->getter = new MonoMethodWrapper(getterMethod, klass);
		}
		if (setterMethod)
		{
			this->setter = new MonoMethodWrapper(setterMethod, klass);
		}
	}
	~MonoPropertyWrapper()
	{
		if (this->getter) delete this->getter; this->getter = nullptr;
		if (this->setter) delete this->setter; this->setter = nullptr;
	}

	virtual IMonoMethod *GetGetter();

	virtual IMonoMethod *GetSetter();

	virtual void *GetWrappedPointer();

	virtual const char *GetName();

	virtual IMonoClass *GetDeclaringClass();

};