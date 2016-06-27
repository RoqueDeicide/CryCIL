#pragma once

#include "IMonoInterface.h"

#pragma warning(push)
#pragma warning(disable : 4250)

struct MonoConstructor : public IMonoConstructor
{
	explicit MonoConstructor(MonoMethod *method, IMonoClass *klass = nullptr)
		: IMonoConstructor(method, klass)
	{}

	mono::object Create(mono::exception *ex = nullptr) const override;
	mono::object Create(IMonoArray<> &args, mono::exception *ex = nullptr) const override;
	mono::object Create(void **args, mono::exception *ex = nullptr) const override;

	void Initialize(void *obj, mono::exception *ex = nullptr) const override;
	void Initialize(void *obj, IMonoArray<> &args, mono::exception *ex = nullptr) const override;
	void Initialize(void *obj, void **args, mono::exception *ex = nullptr) const override;
};

#pragma warning(pop)