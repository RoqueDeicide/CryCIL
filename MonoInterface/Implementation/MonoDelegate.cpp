#include "stdafx.h"

#include "MonoDelegate.h"
#include "MonoMethod.h"

#include "MonoDefinitionFiles/MonoDelegate.h"

IMonoMethod *MonoDelegateWrapper::GetMethod()
{
	return new MonoMethodWrapper(this->delegat->method);
}

mono::object MonoDelegateWrapper::GetTarget()
{
	return (mono::object)this->delegat->target;
}

void *MonoDelegateWrapper::GetFunctionPointer()
{
	static IMonoMethod *Marshal_getFuntionPointer = nullptr;

	if (!Marshal_getFuntionPointer)
	{
		Marshal_getFuntionPointer =
			MonoEnv->CoreLibrary->GetClass("System.Runtime.InteropServices", "Marshal")
								->GetMethod("GetFunctionPointerForDelegateInternal", 1);
	}

	void *param = this->delegat;
	
	return Unbox<void *>(Marshal_getFuntionPointer->Invoke(nullptr, &param));
}

void *MonoDelegateWrapper::GetWrappedPointer()
{
	return this->delegat;
}

mono::object MonoDelegateWrapper::Get()
{
	return this->mDelegate;
}

void MonoDelegateWrapper::GetField(const char *name, void *value)
{
	this->klass->GetField(this->mDelegate, name, value);
}

void MonoDelegateWrapper::SetField(const char *name, void *value)
{
	this->klass->SetField(this->mDelegate, name, value);
}

IMonoProperty *MonoDelegateWrapper::GetProperty(const char *name)
{
	return this->klass->GetProperty(name);
}

IMonoEvent *MonoDelegateWrapper::GetEvent(const char *name)
{
	return this->klass->GetEvent(name);
}

IMonoClass *MonoDelegateWrapper::GetClass()
{
	return this->klass;
}

void MonoDelegateWrapper::Update(mono::object newLocation)
{
	this->mDelegate = newLocation;
}
