#pragma once

#include "IMonoAliases.h"

//! Wraps an object that represents a Mono delegate.
struct IMonoDelegate : public IMonoHandle
{
	//! Gets a wrapper for a method that will be invoked by this delegate.
	__declspec(property(get = GetMethod)) IMonoMethod *Method;
	//! Gets an object that will be used when invoking a method if the latter is an instance method.
	__declspec(property(get = GetTarget)) mono::object Target;
	//! Gets a raw function pointer that can be used to invoke this delegate.
	//!
	//! Returned function pointer may cease to exist after the delegate is GCed.
	__declspec(property(get = GetFunctionPointer)) void *FunctionPointer;

	VIRTUAL_API virtual IMonoMethod *GetMethod() = 0;
	VIRTUAL_API virtual mono::object GetTarget() = 0;
	VIRTUAL_API virtual void *GetFunctionPointer() = 0;
};