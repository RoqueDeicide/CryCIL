#include "stdafx.h"

#include "MonoThreads.h"


mono::Thread MonoThreads::Attach()
{
	return (mono::Thread)mono_thread_attach(mono_domain_get());
}

mono::Thread MonoThreads::Create(mono::delegat method)
{
	MonoObject *obj = mono_object_new(mono_domain_get(), (MonoClass *)MonoThreadWrapper::SystemThreadingThread->GetWrappedPointer());

	mono::exception ex;
	threadCtor(obj, method, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
		return nullptr;
	}
	return (mono::Thread)obj;
}

mono::Thread MonoThreads::CreateParametrized(mono::delegat method)
{
	MonoObject *obj = mono_object_new(mono_domain_get(), (MonoClass *)MonoThreadWrapper::SystemThreadingThread->GetWrappedPointer());

	mono::exception ex;
	threadCtorParam(obj, method, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
		return nullptr;
	}
	return (mono::Thread)obj;
}

void MonoThreads::Sleep(int timeSpan)
{
	mono::exception ex;
	sleep(timeSpan, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

IMonoThread *MonoThreads::Wrap(mono::Thread _thread)
{
	return new MonoThreadWrapper(_thread);
}

bool MonoThreads::staticsInitialized = false;

SleepThunk MonoThreads::sleep;

ThreadCtorParamThunk MonoThreads::threadCtorParam;

ThreadCtorThunk MonoThreads::threadCtor;