#include "stdafx.h"

#include "MonoConstructor.h"
#include "MonoClass.h"

mono::object MonoConstructor::Create(mono::exception *ex /*= nullptr*/)
{
	return MonoEnv->Functions->InternalInvoke(this->wrappedMethod, mono_object_new(mono_domain_get(),
		mono_method_get_class(this->wrappedMethod)), nullptr, ex, false);
}

mono::object MonoConstructor::Create(IMonoArray<> &args, mono::exception *ex /*= nullptr*/)
{
	return MonoEnv->Functions->InternalInvokeArray(this->wrappedMethod, mono_object_new(mono_domain_get(),
		mono_method_get_class(this->wrappedMethod)), args, ex, false);
}

mono::object MonoConstructor::Create(void **args, mono::exception *ex /*= nullptr*/)
{
	return MonoEnv->Functions->InternalInvoke(this->wrappedMethod, mono_object_new(mono_domain_get(),
		mono_method_get_class(this->wrappedMethod)), args, ex, false);
}

void MonoConstructor::Initialize(void *obj, mono::exception *ex /*= nullptr*/)
{
	MonoEnv->Functions->InternalInvoke(this->wrappedMethod, obj, nullptr, ex, false);
}

void MonoConstructor::Initialize(void *obj, IMonoArray<> &args, mono::exception *ex /*= nullptr*/)
{
	MonoEnv->Functions->InternalInvokeArray(this->wrappedMethod, obj, args, ex, false);
}

void MonoConstructor::Initialize(void *obj, void **args, mono::exception *ex /*= nullptr*/)
{
	MonoEnv->Functions->InternalInvoke(this->wrappedMethod, obj, args, ex, false);
}
