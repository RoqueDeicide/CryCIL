#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"
#include "MonoStaticMethod.h"

struct MonoEventWrapper : public IMonoEvent
{
private:
	MonoEvent *_event;
	IMonoClass *klass;

	IMonoFunction *add;
	IMonoFunction *remove;
	IMonoFunction *raise;

	bool isStatic;
	int raiseDefined;			//!< -1 - unchecked; 0 - raise method is not defined; 1 - defined.
public:
	MonoEventWrapper(MonoEvent *_event, IMonoClass *klass = nullptr)
		: raiseDefined(-1)
	{
		this->_event = _event;
		this->klass = klass;
	}
	~MonoEventWrapper()
	{
		if (this->add)    { delete this->add;		this->add    = nullptr; }
		if (this->remove) { delete this->remove;	this->remove = nullptr; }
		if (this->raise)  { delete this->raise;		this->raise  = nullptr;}
	}

	virtual IMonoFunction *GetAdd();
	virtual IMonoFunction *GetRemove();
	virtual IMonoFunction *GetRaise();

	virtual const char *GetName();
	virtual void       *GetWrappedPointer();
	virtual IMonoClass *GetDeclaringClass();
};