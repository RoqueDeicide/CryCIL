#include "stdafx.h"
#include "API_ImplementationHeaders.h"

mono::object MonoMethodWrapper::Invoke(void *object, mono::exception *exc, bool polymorph)
{
	return MonoEnv->Functions->InternalInvoke(this->wrappedMethod, object, nullptr, exc, polymorph);
}

mono::object MonoMethodWrapper::Invoke
(void *object, IMonoArray<> &params, mono::exception *exc, bool polymorph)
{
	return MonoEnv->Functions->InternalInvokeArray(this->wrappedMethod, object, params, exc, polymorph);
}

mono::object MonoMethodWrapper::Invoke
(void *object, void **params, mono::exception *exc, bool polymorph)
{
	return MonoEnv->Functions->InternalInvoke(this->wrappedMethod, object, params, exc, polymorph);
}