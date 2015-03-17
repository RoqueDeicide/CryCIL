#pragma once

#include "IMonoAliases.h"

//! Wraps an object that represents a Mono delegate.
struct IMonoDelegate : public IMonoObject
{
private:
	IMonoFunction *func;
public:
	//! Creates new wrapper for given delegate.
	IMonoDelegate(mono::delegat d)
		: IMonoObject(d)
		, func(nullptr)
	{
	}
	//! Creates new wrapper for given delegate.
	IMonoDelegate(MonoGCHandle &handle)
		: IMonoObject(handle)
		, func(nullptr)
	{
	}
	~IMonoDelegate()
	{
		if (this->func)
		{
			delete this->func;
		}
	}
	//! This operator does the same thing as one for base class, but it also releases cached IMonoFunction
	//! wrapper.
	IMonoDelegate &operator=(mono::delegat del)
	{
		if (this->obj != del)
		{
			if (this->func)
			{
				delete this->func;
			}
			this->obj = del;
		}
	}
	//! Gets a wrapper for a Mono function that will be invoked by this delegate.
	__declspec(property(get = GetFunction)) IMonoFunction *Function;
	//! Gets an object that will be used when invoking a method if the latter is an instance method.
	__declspec(property(get = GetTarget)) mono::object Target;
	//! Gets a raw function pointer that can be used to invoke this delegate.
	//!
	//! Returned function pointer ceases to exist after the delegate is GCed.
	__declspec(property(get = GetTrampoline)) void *Trampoline;

	//! Invokes this delegate.
	//!
	//! @param params A pointer to an array of pointers to the arguments to pass to method(s) represented by
	//!               this delegate. Pass null, if delegate accepts no arguments.
	//! @param ex     A pointer to object reference that will be set to the reference to the exception
	//!               object that represents unhandled exception if it was thrown during delegate execution.
	//!               If set to null, then exception will caught and handled by CryCIL.
	mono::object Invoke(void **params, mono::exception *ex = nullptr)
	{
		if (!ex)
		{
			mono::exception exo;

			mono::object obj = MonoEnv->Objects->InvokeDelegate(this->obj, params, &exo);

			if (exo)
			{
				MonoEnv->HandleException(exo);
			}
		}
		return MonoEnv->Objects->InvokeDelegate(this->obj, params, ex);
	}
	//! Adds invocation list of given delegate to this one's.
	//!
	//! @param del Another delegate.
	//!
	//! @returns A new delegate that encapsulates invocation lists of this and another delegates.
	mono::delegat operator +(mono::delegat del)
	{
		if (strcmp(this->klass->Base->Name, "MulticastDelegate") != 0)
		{
			return;
		}
		void *param = del;
		return this->klass->GetFunction("CombineImpl", -1)->ToInstance()->Invoke(this->obj, &param, nullptr, true);
	}
	//! Removes invocation list of given delegate to this one's.
	//!
	//! @param del Another delegate.
	//!
	//! @returns A new delegate that represents invocation list of this delegate with another delegate's
	//!          one removed from it.
	mono::delegat operator -(mono::delegat del)
	{
		if (strcmp(this->klass->Base->Name, "MulticastDelegate") != 0)
		{
			return;
		}
		void *param = del;
		return this->klass->GetFunction("RemoveImpl", -1)->ToInstance()->Invoke(this->obj, &param, nullptr, true);
	}
	//! Combines assignment and addition.
	IMonoDelegate &operator +=(mono::delegat del)
	{
		*this = *this + del;
		return *this;
	}
	//! Combines assignment and subtraction.
	IMonoDelegate &operator -=(mono::delegat del)
	{
		*this = *this - del;
		return *this;
	}

	IMonoFunction *GetFunction()
	{
		if (!this->func)
		{
			this->func = MonoEnv->Objects->GetDelegateFunction(this->obj);
		}
		return this->func;
	}
	mono::object GetTarget()
	{
		return MonoEnv->Objects->GetDelegateTarget(this->obj);
	}
	void *GetTrampoline()
	{
		return MonoEnv->Objects->GetDelegateTrampoline(this->obj);
	}
};