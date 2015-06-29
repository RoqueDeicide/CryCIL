#pragma once

#include "IMonoInterface.h"

#pragma warning(push)
#pragma warning(disable : 4250)

struct MonoConstructor : public IMonoConstructor
{
	MonoConstructor(MonoMethod *method, IMonoClass *klass = nullptr)
		: IMonoConstructor(method, klass)
	{}

	virtual mono::object Create(mono::exception *ex = nullptr) override;
	virtual mono::object Create(IMonoArray<> &args, mono::exception *ex = nullptr) override;
	virtual mono::object Create(void **args, mono::exception *ex = nullptr) override;

	virtual void Initialize(void *obj, mono::exception *ex = nullptr) override;
	virtual void Initialize(void *obj, IMonoArray<> &args, mono::exception *ex = nullptr) override;
	virtual void Initialize(void *obj, void **args, mono::exception *ex = nullptr) override;
};

#pragma warning(pop)