#pragma once

#include "IMonoInterface.h"
#include "MonoFunction.h"

#pragma warning(push)
#pragma warning(disable : 4250)

struct MonoStaticMethod : public IMonoStaticMethod, public MonoFunction
{
	MonoStaticMethod(MonoMethod *method, IMonoClass *klass = nullptr) : MonoFunction(method, klass) {}

	virtual mono::object Invoke(mono::exception *exc = nullptr);
	virtual mono::object Invoke(IMonoArray<> &params, mono::exception *exc = nullptr);
	virtual mono::object Invoke(void **params, mono::exception *exc = nullptr);
};

#pragma warning(pop)