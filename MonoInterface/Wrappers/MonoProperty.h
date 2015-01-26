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

	VIRTUAL_API virtual IMonoMethod *GetGetter();

	VIRTUAL_API virtual IMonoMethod *GetSetter();

	VIRTUAL_API virtual void *GetWrappedPointer();
};