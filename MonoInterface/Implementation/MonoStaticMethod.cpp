#include "stdafx.h"

#include "MonoStaticMethod.h"

mono::object MonoStaticMethod::Invoke(mono::exception *exc /*= nullptr*/)
{
	return this->InternalInvoke(nullptr, nullptr, exc, false);
}

mono::object MonoStaticMethod::Invoke(IMonoArray<> &params, mono::exception *exc /*= nullptr*/)
{
	return this->InternalInvokeArray(nullptr, params, exc, false);
}

mono::object MonoStaticMethod::Invoke(void **params, mono::exception *exc /*= nullptr*/)
{
	return this->InternalInvoke(nullptr, params, exc, false);
}
