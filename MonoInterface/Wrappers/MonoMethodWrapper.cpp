#include "stdafx.h"
#include "API_ImplementationHeaders.h"

MonoMethodWrapper::MonoMethodWrapper(MonoMethod *method)
{
	this->wrappedMethod = method;
	this->signature = mono_method_signature(this->wrappedMethod);
	this->paramCount = mono_signature_get_param_count(this->signature);
	this->name = mono_method_get_name(this->wrappedMethod);
}

mono::object MonoMethodWrapper::Invoke(mono::object object, IMonoArray *params, bool polymorph)
{
	void **pars = new void*[params->Length];
	for (int i = 0; i < params->Length; i++)
	{
		pars[i] = params->Item(i);
	}
	mono::object result = this->Invoke(object, pars, polymorph);
	delete pars;
	return result;
}

mono::object MonoMethodWrapper::Invoke(mono::object object, void **params, bool polymorph)
{
	MonoMethod *methodToInvoke;
	if (polymorph)
	{
		methodToInvoke =
			mono_object_get_virtual_method((MonoObject *)object, this->wrappedMethod);
	}
	else
	{
		methodToInvoke = this->wrappedMethod;
	}
	mono::object exception;
	mono::object result = (mono::object)
		mono_runtime_invoke(methodToInvoke, object, params, (MonoObject **)&exception);
	if (exception)
	{
		MonoEnv->HandleException(exception);
		return nullptr;
	}
	return result;
}

void *MonoMethodWrapper::GetThunk()
{
	return mono_method_get_unmanaged_thunk(this->wrappedMethod);
}

const char *MonoMethodWrapper::GetName()
{
	return this->name;
}

int MonoMethodWrapper::GetParameterCount()
{
	return this->paramCount;
}

void *MonoMethodWrapper::GetWrappedPointer()
{
	return this->wrappedMethod;
}