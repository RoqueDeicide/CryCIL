#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"
#include "MonoMethod.h"
#include "MonoStaticMethod.h"

struct MonoPropertyWrapper : public IMonoProperty
{
private:
	MonoProperty *prop;
	IMonoClass *klass;
	IMonoFunction *getter;
	IMonoFunction *setter;
public:
	MonoPropertyWrapper(MonoProperty *prop, IMonoClass *klass = nullptr)
		: getter(nullptr)
		, setter(nullptr)
	{
		this->prop = prop;
		this->klass = klass;
		
		MonoMethod *getterMethod = mono_property_get_get_method(prop);
		MonoMethod *setterMethod = mono_property_get_set_method(prop);

		bool isStatic =
			mono_signature_is_instance(mono_method_signature(getterMethod ? getterMethod : setterMethod)) != 0;

		if (getterMethod)
		{
			if (isStatic)
			{
				this->getter = new MonoStaticMethod(getterMethod, klass);
			}
			else
			{
				this->getter = new MonoMethodWrapper(getterMethod, klass);
			}
		}
		if (setterMethod)
		{
			if (isStatic)
			{
				this->setter = new MonoStaticMethod(setterMethod, klass);
			}
			else
			{
				this->setter = new MonoMethodWrapper(setterMethod, klass);
			}
		}
	}
	~MonoPropertyWrapper()
	{
		if (this->getter) delete this->getter; this->getter = nullptr;
		if (this->setter) delete this->setter; this->setter = nullptr;
	}

	virtual IMonoFunction *GetGetter();
	virtual IMonoFunction *GetSetter();
	virtual void          *GetWrappedPointer();
	virtual const char    *GetName();
	virtual IMonoClass    *GetDeclaringClass();
	virtual IMonoFunction *GetIdentifier();
	virtual int            GetParameterCount();
};