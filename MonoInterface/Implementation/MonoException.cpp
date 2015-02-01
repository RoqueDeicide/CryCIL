#include "stdafx.h"

#include "MonoHandle.h"
#include "MonoException.h"
#include "MonoClass.h"

MonoExceptionWrapper::MonoExceptionWrapper(MonoException *monoEx)
{
	this->monoEx = monoEx;
	this->handle = new MonoHandle(this->ex);
}

MonoExceptionWrapper::MonoExceptionWrapper(mono::exception ex)
{
	this->ex = ex;
	this->handle = new MonoHandle(this->ex);
}

void MonoExceptionWrapper::Throw()
{
	mono_raise_exception(this->monoEx);
}

const char *MonoExceptionWrapper::GetErrorMessage()
{
	return ToNativeString(this->GetProperty("Message")->Getter->Invoke(this->monoEx));
}

const char *MonoExceptionWrapper::GetStackTrace()
{
	return ToNativeString(this->GetProperty("StackTrace")->Getter->Invoke(this->monoEx));
}

IMonoException *MonoExceptionWrapper::GetInnerException()
{
	return new MonoExceptionWrapper(this->GetProperty("InnerException")->Getter->Invoke(this->monoEx));
}

mono::exception MonoExceptionWrapper::GetExceptionObject()
{
	return this->ex;
}

mono::object MonoExceptionWrapper::Get()
{
	return this->ex;
}

mono::object MonoExceptionWrapper::CallMethod(const char *name, IMonoArray *args)
{
	return this->handle->CallMethod(name, args);
}

void MonoExceptionWrapper::GetField(const char *name, void *value)
{
	this->handle->GetField(name, value);
}

void MonoExceptionWrapper::SetField(const char *name, void *value)
{
	this->handle->SetField(name, value);
}

IMonoProperty *MonoExceptionWrapper::GetProperty(const char *name)
{
	return this->handle->GetProperty(name);
}

IMonoEvent *MonoExceptionWrapper::GetEvent(const char *name)
{
	return this->handle->GetEvent(name);
}

IMonoClass *MonoExceptionWrapper::GetClass()
{
	return this->handle->GetClass();
}

void *MonoExceptionWrapper::UnboxObject()
{
	gEnv->pLog->LogError("Attempt to unbox an exception object was made.");
	return nullptr;
}

void *MonoExceptionWrapper::GetWrappedPointer()
{
	return this->ex;
}