#pragma once

#include "IMonoInterface.h"

struct MonoThreads : public IMonoThreads
{
	mono::Thread Attach() override;

	mono::Thread Create(mono::delegat method) override;
	mono::Thread CreateParametrized(mono::delegat method) override;

	void Sleep(int timeSpan) override;
};