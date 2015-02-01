#include "stdafx.h"
#include "MonoProperty.h"
#include "MonoMethod.h"

IMonoMethod *MonoPropertyWrapper::GetGetter()
{
	return new MonoMethodWrapper(mono_property_get_get_method(this->prop));
}

IMonoMethod *MonoPropertyWrapper::GetSetter()
{
	return new MonoMethodWrapper(mono_property_get_set_method(this->prop));
}

void *MonoPropertyWrapper::GetWrappedPointer()
{
	return this->prop;
}

const char *MonoPropertyWrapper::GetName()
{
	return mono_property_get_name(this->prop);
}

