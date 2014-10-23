#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"

struct MonoMethodWrapper : IMonoMethod
{
private:
	MonoMethod *wrappedMethod;
	MonoMethodSignature *signature;
	int paramCount;
	const char *name;
public:
	MonoMethodWrapper(MonoMethod *method)
	{
		this->wrappedMethod = method;
		this->signature = mono_method_signature(this->wrappedMethod);
		this->paramCount = mono_signature_get_param_count(this->signature);
		this->name = mono_method_get_name(this->wrappedMethod);
	}
	//! Invokes this method.
	//!
	//! @remark Since extension methods are static by their internal nature,
	//!         you can pass null as object parameter, and that can work,
	//!         if extension method is not using the instance. It's up to
	//!         you to find uses for that minor detail.
	//!
	//! @param object    Pointer to the instance to use, if this method is not
	//!                  static, it can be null otherwise.
	//! @param params    Pointer to the mono array of parameters to pass to the method.
	//!                  Pass null, if method can accept no arguments.
	//! @param polymorph Indicates whether we need to invoke a virtual method,
	//!                  that is specific to the instance.
	virtual mono::object Invoke(mono::object object, IMonoArray *params = nullptr, bool polymorph = false)
	{
		void **pars = new void*[params->Length];
		for (int i = 0; i < params->Length; i++)
		{
			pars[i] = params->GetItem(i);
		}
		mono::object result = this->Invoke(object, pars, polymorph);
		delete pars;
		return result;
	}
	//! Invokes this method.
	//!
	//! @remark Since extension methods are static by their internal nature,
	//!         you can pass null as object parameter, and that can work,
	//!         if extension method is not using the instance. It's up to
	//!         you to find uses for that minor detail.
	//!
	//! @param object     Pointer to the instance to use, if this method is not
	//!                   static, it can be null otherwise.
	//! @param params     Pointer to the array of parameters to pass to the method.
	//!                   Pass null, if method can accept no arguments.
	//! @param polymorph  Indicates whether we need to invoke a virtual method,
	//!                   that is specific to the instance.
	virtual mono::object Invoke(mono::object object, void **params = nullptr, bool polymorph = false)
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

	virtual void * GetThunk()
	{
		mono_method_get_unmanaged_thunk(this->wrappedMethod);
	}

	virtual const char * GetName()
	{
		return this->name;
	}

	virtual int GetParameterCount()
	{
		return this->paramCount;
	}

	virtual void * GetWrappedPointer()
	{
		return this->wrappedMethod;
	}

};