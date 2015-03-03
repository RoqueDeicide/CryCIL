#include "stdafx.h"
#include "MonoProperty.h"
#include "MonoMethod.h"

IMonoMethod *MonoPropertyWrapper::GetGetter()
{
	return this->getter;
}

IMonoMethod *MonoPropertyWrapper::GetSetter()
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

IMonoMethod *MonoPropertyWrapper::GetIdentifier()
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

