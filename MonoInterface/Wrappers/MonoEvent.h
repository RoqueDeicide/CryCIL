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

	VIRTUAL_API virtual IMonoMethod *GetAdd();

	VIRTUAL_API virtual IMonoMethod *GetRemove();

	VIRTUAL_API virtual IMonoMethod *GetRaise();

	VIRTUAL_API virtual const char *GetName();

	VIRTUAL_API virtual void *GetWrappedPointer();

};