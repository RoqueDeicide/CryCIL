#pragma once

#include "IMonoInterface.h"
#include "MonoFunction.h"

#pragma warning(push)
#pragma warning(disable : 4250)

struct MonoConstructor : public IMonoConstructor, public MonoFunction
{
	MonoConstructor(MonoMethod *method, IMonoClass *klass = nullptr)
		: MonoFunction(method, klass) {}

	virtual mono::object Create(mono::exception *ex = nullptr);
	virtual mono::object Create(IMonoArray *args, mono::exception *ex = nullptr);
	virtual mono::object Create(void **args, mono::exception *ex = nullptr);

	virtual void Initialize(void *obj, mono::exception *ex = nullptr);
	virtual void Initialize(void *obj, IMonoArray *args, mono::exception *ex = nullptr);
	virtual void Initialize(void *obj, void **args, mono::exception *ex = nullptr);
};

#pragma warning(pop)