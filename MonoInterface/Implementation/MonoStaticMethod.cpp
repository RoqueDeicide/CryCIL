#include "stdafx.h"

#include "MonoStaticMethod.h"

mono::object MonoStaticMethod::Invoke(mono::exception *exc /*= nullptr*/)
{
	return MonoEnv->Functions->InternalInvoke(this->wrappedMethod, nullptr, nullptr, exc, false);
}

mono::object MonoStaticMethod::Invoke(IMonoArray<> &params, mono::exception *exc /*= nullptr*/)
{
	return MonoEnv->Functions->InternalInvokeArray(this->wrappedMethod, nullptr, params, exc, false);
}

mono::object MonoStaticMethod::Invoke(void **params, mono::exception *exc /*= nullptr*/)
{
	return MonoEnv->Functions->InternalInvoke(this->wrappedMethod, nullptr, params, exc, false);
}