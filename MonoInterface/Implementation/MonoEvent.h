#pragma once

#include "IMonoInterface.h"

struct MonoEventWrapper : public IMonoEvent
{
private:
	MonoEvent *_event;
	IMonoClass *klass;
public:
	MonoEventWrapper(MonoEvent *_event, IMonoClass *klass = nullptr)
	{
		this->_event = _event;
		this->klass = klass;
	}

	virtual IMonoMethod *GetAdd();

	virtual IMonoMethod *GetRemove();

	virtual IMonoMethod *GetRaise();

	virtual const char *GetName();

	virtual void *GetWrappedPointer();

	virtual IMonoClass *GetDeclaringClass();

};