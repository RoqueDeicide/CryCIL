#include "stdafx.h"

#include "StaticSubObject.h"

void StaticSubObjectInterop::InitializeInterops()
{
	REGISTER_METHOD(GetName);
	REGISTER_METHOD(SetName);
	REGISTER_METHOD(GetProperties);
	REGISTER_METHOD(SetProperties);
}

mono::string StaticSubObjectInterop::GetName(IStatObj::SSubObject *obj)
{
	return ToMonoString(obj->name);
}

void StaticSubObjectInterop::SetName(IStatObj::SSubObject *obj, mono::string name)
{
	obj->name = string(NtText(name));
}

mono::string StaticSubObjectInterop::GetProperties(IStatObj::SSubObject *obj)
{
	return ToMonoString(obj->properties);
}

void StaticSubObjectInterop::SetProperties(IStatObj::SSubObject *obj, mono::string props)
{
	obj->properties = string(NtText(props));
}
