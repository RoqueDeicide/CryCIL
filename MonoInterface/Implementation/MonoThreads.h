#pragma once

#include "IMonoInterface.h"

struct MonoThreads : public IMonoThreads
{
	virtual mono::Thread Attach() override;

	virtual mono::Thread Create(mono::delegat method) override;
	virtual mono::Thread CreateParametrized(mono::delegat method) override;

	virtual void Sleep(int timeSpan) override;
};