#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"

struct MonoPropertyWrapper : public IMonoProperty
{
private:
	MonoProperty *prop;
public:
	MonoPropertyWrapper(MonoProperty *prop)
	{
		this->prop = prop;
	}

	virtual IMonoMethod *GetGetter();

	virtual IMonoMethod *GetSetter();

	virtual void *GetWrappedPointer();
};