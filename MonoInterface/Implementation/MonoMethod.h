#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"

#pragma warning(push)
#pragma warning(disable : 4250)

struct MonoMethodWrapper : public IMonoMethod
{
	explicit MonoMethodWrapper(MonoMethod *method, IMonoClass *klass = nullptr)
		: IMonoMethod(method, klass) {}
	
	mono::object Invoke(void *object, mono::exception *exc, bool polymorph) const override;
	mono::object Invoke(void *object, IMonoArray<> &params, mono::exception *exc, bool polymorph) const override;
	mono::object Invoke(void *object, void **params, mono::exception *exc, bool polymorph) const override;
};

#pragma warning(pop)