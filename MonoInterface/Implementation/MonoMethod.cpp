#include "stdafx.h"
#include "API_ImplementationHeaders.h"

mono::object MonoMethodWrapper::Invoke
(
	void *object,
	IMonoArray<> &params,
	mono::exception *exc,
	bool polymorph
)
{
	return this->InternalInvokeArray(object, params, exc, polymorph);
}

mono::object MonoMethodWrapper::Invoke
(
	void *object,
	void **params,
	mono::exception *exc,
	bool polymorph
)
{
	return this->InternalInvoke(object, params, exc, polymorph);
}

mono::object MonoMethodWrapper::Invoke(void *object, mono::exception *exc /*= nullptr*/, bool polymorph /*= false */)
{
	return this->InternalInvoke(object, nullptr, exc, polymorph);
}
