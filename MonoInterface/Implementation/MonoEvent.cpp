#include "stdafx.h"
#include "MonoEvent.h"
#include "MonoClass.h"


IMonoFunction *MonoEventWrapper::GetAdd()
{
	if (!this->add)
	{
		MonoMethod *addMethod = mono_event_get_add_method(_event);

		if (isStatic)
		{
			this->add = new MonoStaticMethod(addMethod, klass);
		}
		else
		{
			this->add = new MonoMethodWrapper(addMethod, klass);
		}
	}
	return this->add;
}

IMonoFunction *MonoEventWrapper::GetRemove()
{
	if (!this->remove)
	{
		MonoMethod *removeMethod = mono_event_get_remove_method(_event);

		if (isStatic)
		{
			this->remove = new MonoStaticMethod(removeMethod, klass);
		}
		else
		{
			this->remove = new MonoMethodWrapper(removeMethod, klass);
		}
	}
	return this->remove;
}

IMonoFunction *MonoEventWrapper::GetRaise()
{
	if (!this->raise && this->raiseDefined != 0)
	{
		MonoMethod *raiseMethod = mono_event_get_raise_method(_event);

		if (isStatic)
		{
			this->raise = raiseMethod ? new MonoStaticMethod(raiseMethod, klass) : nullptr;
		}
		else
		{
			this->raise = raiseMethod ? new MonoMethodWrapper(raiseMethod, klass) : nullptr;
		}

		if (!this->raise)
		{
			auto methodName = NtText(2, "On", mono_event_get_name(this->_event));

			auto overloads = klass->GetFunctions(methodName);
			if (overloads)
			{
				this->raise = overloads->At(0);
			}
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

