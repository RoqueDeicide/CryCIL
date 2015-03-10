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