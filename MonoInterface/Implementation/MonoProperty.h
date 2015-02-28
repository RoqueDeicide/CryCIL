#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"

struct MonoPropertyWrapper : public IMonoProperty
{
private:
	MonoProperty *prop;
	IMonoClass *klass;
public:
	MonoPropertyWrapper(MonoProperty *prop, IMonoClass *klass = nullptr)
	{
		this->prop = prop;
		this->klass = klass;
	}

	virtual IMonoMethod *GetGetter();

	virtual IMonoMethod *GetSetter();

	virtual void *GetWrappedPointer();

	virtual const char *GetName();

	virtual IMonoClass *GetDeclaringClass();

};