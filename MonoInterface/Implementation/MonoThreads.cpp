#include "stdafx.h"

#include "MonoThreads.h"


mono::Thread MonoThreads::Attach()
{
	return (mono::Thread)mono_thread_attach(mono_domain_get());
}

mono::Thread MonoThreads::Create(mono::delegat method)
{
	void *param = method;
	return MonoEnv->CoreLibrary->Thread->GetConstructor("System.Threading.ThreadStart")->Create(&param);
}

mono::Thread MonoThreads::CreateParametrized(mono::delegat method)
{
	void *param = method;
	return MonoEnv->CoreLibrary->Thread->GetConstructor("System.Threading.ParameterizedThreadStart")->Create(&param);
}

void MonoThreads::Sleep(int timeSpan)
{
	void *param = &timeSpan;
	MonoEnv->CoreLibrary->Thread->GetFunction("System.Threading.ThreadStart")->ToStatic()->Invoke(&param);
}