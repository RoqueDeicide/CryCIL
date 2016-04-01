#include "stdafx.h"

#include "MonoConstructor.h"
#include "MonoClass.h"

mono::object MonoConstructor::Create(mono::exception *ex /*= nullptr*/) const
{
	MonoObject *obj = mono_object_new(mono_domain_get(), mono_method_get_class(this->wrappedMethod));

	MonoEnv->Functions->InternalInvoke(this->wrappedMethod, obj, nullptr, ex, false);

	return mono::object(obj);
}

mono::object MonoConstructor::Create(IMonoArray<> &args, mono::exception *ex /*= nullptr*/) const
{
	MonoObject *obj = mono_object_new(mono_domain_get(), mono_method_get_class(this->wrappedMethod));

	MonoEnv->Functions->InternalInvokeArray(this->wrappedMethod, obj, args, ex, false);

	return mono::object(obj);
}

mono::object MonoConstructor::Create(void **args, mono::exception *ex /*= nullptr*/) const
{
	MonoObject *obj = mono_object_new(mono_domain_get(), mono_method_get_class(this->wrappedMethod));

	MonoEnv->Functions->InternalInvoke(this->wrappedMethod, obj, args, ex, false);

	return mono::object(obj);
}

void MonoConstructor::Initialize(void *obj, mono::exception *ex /*= nullptr*/) const
{
	MonoEnv->Functions->InternalInvoke(this->wrappedMethod, obj, nullptr, ex, false);
}

void MonoConstructor::Initialize(void *obj, IMonoArray<> &args, mono::exception *ex /*= nullptr*/) const
{
	MonoEnv->Functions->InternalInvokeArray(this->wrappedMethod, obj, args, ex, false);
}

void MonoConstructor::Initialize(void *obj, void **args, mono::exception *ex /*= nullptr*/) const
{
	MonoEnv->Functions->InternalInvoke(this->wrappedMethod, obj, args, ex, false);
}
