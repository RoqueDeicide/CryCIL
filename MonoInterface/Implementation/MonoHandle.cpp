#include "stdafx.h"
#include "API_ImplementationHeaders.h"

//! Gets the value of the object's field.
void MonoHandle::GetField(const char *name, void *value)
{
	return this->klass->GetField(this->mObj, name, value);
}
//! Sets the value of the object's field.
void MonoHandle::SetField(const char *name, void *value)
{
	this->klass->SetField(this->mObj, name, value);
}

IMonoProperty *MonoHandle::GetProperty(const char *name)
{
	return this->klass->GetProperty(name);
}

IMonoEvent *MonoHandle::GetEvent(const char *name)
{
	return this->klass->GetEvent(name);
}
//! Gets the wrapper for the class of this object.
IMonoClass *MonoHandle::GetClass()
{
	return this->klass;
}

void *MonoHandle::GetWrappedPointer()
{
	return this->obj;
}

void MonoHandle::Update(mono::object newLocation)
{
	this->mObj = newLocation;
}
