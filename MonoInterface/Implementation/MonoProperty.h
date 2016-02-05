#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"
#include "MonoMethod.h"
#include "MonoStaticMethod.h"

#if 0
#define PropertyMessage CryLogAlways
#else
#define PropertyMessage(...) void(0)
#endif

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
		PropertyMessage("Started creation of the property wrapper.");

		this->prop = prop;
		this->klass = klass;
		PropertyMessage("Stored pointers to the property and class.");
		
		MonoMethod *getterMethod = mono_property_get_get_method(prop);
		MonoMethod *setterMethod = mono_property_get_set_method(prop);
		PropertyMessage("Got the getter and setter methods.");

		bool isStatic =
			mono_signature_is_instance(mono_method_signature(getterMethod ? getterMethod : setterMethod)) != 0;
		PropertyMessage("Checked whether this property is static.");

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
		PropertyMessage("Created wrapper for getter.");
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
		PropertyMessage("Created wrapper for setter.");
	}
	~MonoPropertyWrapper()
	{
		if (this->getter) delete this->getter; this->getter = nullptr;
		if (this->setter) delete this->setter; this->setter = nullptr;
	}

	virtual IMonoFunction *GetGetter() override;
	virtual IMonoFunction *GetSetter() override;
	virtual void          *GetWrappedPointer() override;
	virtual const char    *GetName() override;
	virtual IMonoClass    *GetDeclaringClass() override;
	virtual IMonoFunction *GetIdentifier() override;
	virtual int            GetParameterCount() override;
};