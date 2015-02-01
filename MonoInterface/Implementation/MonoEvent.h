#pragma once

#include "IMonoInterface.h"

struct MonoEventWrapper : public IMonoEvent
{
private:
	MonoEvent *_event;
public:
	MonoEventWrapper(MonoEvent *_event)
	{
		this->_event = _event;
	}

	virtual IMonoMethod *GetAdd();

	virtual IMonoMethod *GetRemove();

	virtual IMonoMethod *GetRaise();

	virtual const char *GetName();

	virtual void *GetWrappedPointer();

};