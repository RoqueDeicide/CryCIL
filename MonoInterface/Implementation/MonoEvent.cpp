#include "stdafx.h"
#include "MonoEvent.h"
#include "MonoClass.h"


IMonoMethod *MonoEventWrapper::GetAdd()
{
	return new MonoMethodWrapper(mono_event_get_add_method(this->_event));
}

IMonoMethod *MonoEventWrapper::GetRemove()
{
	return new MonoMethodWrapper(mono_event_get_remove_method(this->_event));
}

IMonoMethod *MonoEventWrapper::GetRaise()
{
	MonoMethod *method = mono_event_get_raise_method(this->_event);
	if (!method)
	{
		ConstructiveText raiseMethodName(strlen(this->Name) + 2);
		raiseMethodName << "On" << this->Name;
		const char *name = raiseMethodName.ToNTString();
		MonoClass *klass = mono_event_get_parent(this->_event);
		for (int i = 0; i < 5; i++)
		{
			method = mono_class_get_method_from_name(klass, name, i);
			if (method)
			{
				break;
			}
		}
	}
	if (!method)
	{
		return nullptr;
	}
	return new MonoMethodWrapper(method);
}

const char *MonoEventWrapper::GetName()
{
	return mono_event_get_name(this->_event);
}

void *MonoEventWrapper::GetWrappedPointer()
{
	return this->_event;
}

