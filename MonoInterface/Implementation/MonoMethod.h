#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"

#pragma warning(push)
#pragma warning(disable : 4250)

struct MonoMethodWrapper : public IMonoMethod
{
	explicit MonoMethodWrapper(MonoMethod *method, IMonoClass *klass = nullptr)
		: IMonoMethod(method, klass) {}
	
	virtual mono::object Invoke(void *object, mono::exception *exc = nullptr,
								bool polymorph = false) const override;
	virtual mono::object Invoke(void *object, IMonoArray<> &params, mono::exception *exc = nullptr,
								bool polymorph = false) const override;
	virtual mono::object Invoke(void *object, void **params, mono::exception *exc = nullptr,
								bool polymorph = false) const override;
};

#pragma warning(pop)