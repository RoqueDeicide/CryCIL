#include "stdafx.h"
#include "API_ImplementationHeaders.h"

MonoMethodWrapper::MonoMethodWrapper(MonoMethod *method)
{
	this->wrappedMethod = method;
	this->signature = mono_method_signature(this->wrappedMethod);
	this->paramCount = mono_signature_get_param_count(this->signature);
	this->name = mono_method_get_name(this->wrappedMethod);
}

mono::object MonoMethodWrapper::Invoke(void *object, IMonoArray *params, bool polymorph)
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
	MonoArray *paramsArray = (MonoArray *)params->GetWrappedPointer();
	MonoObject *exception;
	MonoObject *result = mono_runtime_invoke_array(methodToInvoke, object, paramsArray, &exception);
	if (exception)
	{
		MonoEnv->HandleException((mono::exception)exception);
		return nullptr;
	}
	return (mono::object)result;
}

mono::object MonoMethodWrapper::Invoke(void *object, void **params, bool polymorph)
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
	MonoObject *exception;
	MonoObject *result = mono_runtime_invoke(methodToInvoke, object, params, &exception);
	if (exception)
	{
		MonoEnv->HandleException((mono::exception)exception);
		return nullptr;
	}
	return (mono::object)result;
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