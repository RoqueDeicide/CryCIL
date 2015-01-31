#pragma once

#include "IMonoInterface.h"
#include "MonoThread.h"

typedef void(__stdcall *ThreadCtorThunk)(MonoObject *, mono::delegat, mono::exception *);
typedef void(__stdcall *ThreadCtorParamThunk)(MonoObject *, mono::delegat, mono::exception *);
typedef void(__stdcall *SleepThunk)(int, mono::exception *);

struct MonoThreads : public IMonoThreads
{
	static ThreadCtorThunk threadCtor;
	static ThreadCtorParamThunk threadCtorParam;
	static SleepThunk sleep;
	static bool staticsInitialized;

	MonoThreads()
	{
		if (!MonoThreadWrapper::StaticsAreInitialized)
		{
			MonoThreadWrapper::InitializeStatics();
		}
		if (!staticsInitialized)
		{
			threadCtor =
				(ThreadCtorThunk)
					MonoThreadWrapper::SystemThreadingThread
						->GetMethod(".ctor", "System.Threading.ThreadStart")
						->UnmanagedThunk;
			threadCtorParam =
				(ThreadCtorParamThunk)
					MonoThreadWrapper::SystemThreadingThread
						->GetMethod(".ctor", "System.Threading.ParameterizedThreadStart")
						->UnmanagedThunk;
			sleep = (SleepThunk)MonoThreadWrapper::SystemThreadingThread->GetMethod("Abort", 0)
																		->UnmanagedThunk;
		
			staticsInitialized = true;
		}
	}

	virtual mono::Thread Attach();

	virtual mono::Thread Create(mono::delegat method);

	virtual mono::Thread CreateParametrized(mono::delegat method);

	virtual void Sleep(int timeSpan);

	virtual IMonoThread *Wrap(mono::Thread _thread);
};