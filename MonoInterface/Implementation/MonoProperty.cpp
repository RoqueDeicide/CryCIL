#include "stdafx.h"
#include "MonoProperty.h"
#include "MonoMethod.h"

IMonoFunction *MonoPropertyWrapper::GetGetter()
{
	return this->getter;
}

IMonoFunction *MonoPropertyWrapper::GetSetter()
{
	return this->setter;
}

void *MonoPropertyWrapper::GetWrappedPointer()
{
	return this->prop;
}

const char *MonoPropertyWrapper::GetName()
{
	return mono_property_get_name(this->prop);
}

IMonoClass *MonoPropertyWrapper::GetDeclaringClass()
{
	return this->klass;
}

IMonoFunction *MonoPropertyWrapper::GetIdentifier()
{
	if (this->getter)
	{
		return this->getter;
	}
	else
	{
		return this->setter;
	}
}

