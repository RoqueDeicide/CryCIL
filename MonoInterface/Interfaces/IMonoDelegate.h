#pragma once

#include "IMonoAliases.h"

//! Wraps an object that represents a Mono delegate.
struct IMonoDelegate : public IMonoObject
{
	//! Creates new wrapper for given delegate.
	IMonoDelegate(mono::delegat d)
		: IMonoObject(d)
	{
	}
	//! Creates new wrapper for given delegate.
	IMonoDelegate(MonoGCHandle &handle)
		: IMonoObject(handle)
	{
	}
	//! Gets a wrapper for a Mono function that will be invoked by this delegate.
	__declspec(property(get = GetMethod)) IMonoFunction *Function;
	//! Gets an object that will be used when invoking a method if the latter is an instance method.
	__declspec(property(get = GetTarget)) mono::object Target;
	//! Gets a raw function pointer that can be used to invoke this delegate.
	//!
	//! Returned function pointer may cease to exist after the delegate is GCed.
	__declspec(property(get = GetFunctionPointer)) void *FunctionPointer;

	IMonoFunction *GetFunction()
	{
		return MonoEnv->Objects->GetDelegateFunction(this->obj);
	}
	mono::object GetTarget()
	{
		return MonoEnv->Objects->GetDelegateTarget(this->obj);
	}
	void *GetFunctionPointer()
	{
		return MonoEnv->Objects->GetDelegateFunctionPointer(this->obj);
	}
};