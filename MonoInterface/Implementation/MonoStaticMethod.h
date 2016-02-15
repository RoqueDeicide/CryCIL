#pragma once

#include "IMonoInterface.h"

#pragma warning(push)
#pragma warning(disable : 4250)

struct MonoStaticMethod : public IMonoStaticMethod
{
	explicit MonoStaticMethod(MonoMethod *method, IMonoClass *klass = nullptr)
		: IMonoStaticMethod(method, klass) {}

	virtual mono::object Invoke(mono::exception *exc = nullptr) override;
	virtual mono::object Invoke(IMonoArray<> &params, mono::exception *exc = nullptr) override;
	virtual mono::object Invoke(void **params, mono::exception *exc = nullptr) override;
};

#pragma warning(pop)