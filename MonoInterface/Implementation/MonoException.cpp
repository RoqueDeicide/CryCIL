#include "stdafx.h"

#include "MonoHandle.h"
#include "MonoException.h"
#include "MonoClass.h"

MonoExceptionWrapper::MonoExceptionWrapper(MonoException *monoEx)
{
	this->monoEx = monoEx;
	this->klass = MonoClassCache::Wrap(mono_object_get_class(this->obj));
}

MonoExceptionWrapper::MonoExceptionWrapper(mono::exception ex)
{
	this->ex = ex;
	this->klass = MonoClassCache::Wrap(mono_object_get_class(this->obj));
}

void MonoExceptionWrapper::Throw()
{
	mono_raise_exception(this->monoEx);
}

const char *MonoExceptionWrapper::GetErrorMessage()
{
	return ToNativeString(this->GetProperty("Message")->Getter->ToInstance()->Invoke(this->monoEx));
}

const char *MonoExceptionWrapper::GetStackTrace()
{
	return ToNativeString(this->GetProperty("StackTrace")->Getter->ToInstance()->Invoke(this->monoEx));
}

IMonoException *MonoExceptionWrapper::GetInnerException()
{
	return new MonoExceptionWrapper(this->GetProperty("InnerException")->Getter->ToInstance()->Invoke(this->monoEx));
}

mono::object MonoExceptionWrapper::Get()
{
	return this->ex;
}

void MonoExceptionWrapper::GetField(const char *name, void *value)
{
	this->klass->GetField(this->ex, name, value);
}

void MonoExceptionWrapper::SetField(const char *name, void *value)
{
	this->klass->SetField(this->ex, name, value);
}

IMonoProperty *MonoExceptionWrapper::GetProperty(const char *name)
{
	return this->klass->GetProperty(name);
}

IMonoEvent *MonoExceptionWrapper::GetEvent(const char *name)
{
	return this->klass->GetEvent(name);
}

IMonoClass *MonoExceptionWrapper::GetClass()
{
	return this->klass;
}

void *MonoExceptionWrapper::GetWrappedPointer()
{
	return this->ex;
}

void MonoExceptionWrapper::Update(mono::object newLocation)
{
	this->ex = newLocation;
}
