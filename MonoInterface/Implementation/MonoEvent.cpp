#include "stdafx.h"
#include "MonoEvent.h"
#include "MonoClass.h"

#if 1
#define EventMessage CryLogAlways
#else
#define EventMessage(...) void(0)
#endif

MonoEventWrapper::MonoEventWrapper(MonoEvent *_event, IMonoClass *klass /*= nullptr*/)
	: _event(nullptr)
	, klass(nullptr)
	, add(nullptr)
	, remove(nullptr)
	, raise(nullptr)
	, raiseDefined(-1)
{
	this->_event = _event;
	this->klass = klass;
}

IMonoFunction *MonoEventWrapper::GetAdd()
{
	if (!this->add)
	{
		const char *eventName = mono_event_get_name(this->_event);

		this->add = this->klass->GetFunction(NtText({ "add_", eventName }), -1);
	}
	return this->add;
}

IMonoFunction *MonoEventWrapper::GetRemove()
{
	if (!this->remove)
	{
		const char *eventName = mono_event_get_name(this->_event);

		this->remove = this->klass->GetFunction(NtText({ "remove_", eventName }), -1);
	}
	return this->remove;
}

IMonoFunction *MonoEventWrapper::GetRaise()
{
	if (!this->raise && this->raiseDefined != 0)
	{
		const char *eventName = mono_event_get_name(this->_event);

		this->raise = this->klass->GetFunction(NtText({ "raise_", eventName }), -1);

		if (!this->raise)
		{
			this->raise = this->klass->GetFunction(NtText({ "On", eventName }), -1);
		}

		this->raiseDefined = this->raise ? 1 : 0;
	}
	return this->raise;
}

const char *MonoEventWrapper::GetName()
{
	return mono_event_get_name(this->_event);
}

void *MonoEventWrapper::GetWrappedPointer()
{
	return this->_event;
}

IMonoClass *MonoEventWrapper::GetDeclaringClass()
{
	return this->klass;
}
