#pragma once

#include "IMonoInterface.h"

struct MonoDelegateWrapper : public IMonoDelegate
{
private:
	union
	{
		MonoDelegate *delegat;
		mono::delegat mDelegate;
	};
public:
	MonoDelegateWrapper(mono::delegat delegat)
	{
		this->mDelegate = delegat;
	}

	virtual IMonoMethod *GetMethod();

	virtual mono::object GetTarget();

	virtual void *GetFunctionPointer();

	virtual void *GetWrappedPointer();
};