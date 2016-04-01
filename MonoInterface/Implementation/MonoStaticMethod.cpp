#include "stdafx.h"

#include "MonoStaticMethod.h"

#if 0
#define StaticMethodMessage CryLogAlways
#else
#define StaticMethodMessage(...) void(0)
#endif

mono::object MonoStaticMethod::Invoke(mono::exception *exc /*= nullptr*/) const
{
	StaticMethodMessage("Invoking the static method with no parameters.");

	return MonoEnv->Functions->InternalInvoke(this->wrappedMethod, nullptr, nullptr, exc, false);
}

mono::object MonoStaticMethod::Invoke(IMonoArray<> &params, mono::exception *exc /*= nullptr*/) const
{
	StaticMethodMessage("Invoking the static method with Mono array parameters.");

	return MonoEnv->Functions->InternalInvokeArray(this->wrappedMethod, nullptr, params, exc, false);
}

mono::object MonoStaticMethod::Invoke(void **params, mono::exception *exc /*= nullptr*/) const
{
	StaticMethodMessage("Invoking the static method with simple array parameters.");

	return MonoEnv->Functions->InternalInvoke(this->wrappedMethod, nullptr, params, exc, false);
}