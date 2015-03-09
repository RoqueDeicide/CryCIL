#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"
#include "MonoFunction.h"

#pragma warning(push)
#pragma warning(disable : 4250)

struct MonoMethodWrapper : public IMonoMethod, public MonoFunction
{
	MonoMethodWrapper(MonoMethod *method, IMonoClass *klass = nullptr)
		: MonoFunction(method, klass) {}
	//! Invokes this method.
	virtual mono::object Invoke
	(
		void *object,
		mono::exception *exc = nullptr,
		bool polymorph = false
	);
	//! Invokes this method.
	virtual mono::object Invoke
	(
		void *object,
		IMonoArray<> &params,
		mono::exception *exc = nullptr,
		bool polymorph = false
	);
	//! Invokes this method.
	virtual mono::object Invoke
	(
		void *object,
		void **params = nullptr,
		mono::exception *exc = nullptr,
		bool polymorph = false
	);

};

#pragma warning(pop)