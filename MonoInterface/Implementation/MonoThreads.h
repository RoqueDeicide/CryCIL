#pragma once

#include "IMonoInterface.h"

struct MonoThreads : public IMonoThreads
{
	virtual mono::Thread Attach();

	virtual mono::Thread Create(mono::delegat method);
	virtual mono::Thread CreateParametrized(mono::delegat method);

	virtual void Sleep(int timeSpan);
};