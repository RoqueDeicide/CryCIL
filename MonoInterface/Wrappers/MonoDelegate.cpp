#include "stdafx.h"

#include "MonoDelegate.h"
#include "MonoMethod.h"

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