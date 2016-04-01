#pragma once

#include "IMonoAliases.h"

//! Wraps a Mono exception object.
//!
//! It is highly recommended to pin mono::exception objects using Mono GC handles.
struct IMonoException : public IMonoObject
{
	//! Creates new wrapper for given exception.
	IMonoException(mono::exception ex) : IMonoObject(ex) {}
	//! Creates new wrapper for given exception.
	IMonoException(MonoGCHandle &handle) : IMonoObject(handle) {}
	
	//! Gets the message provided with this exception object.
	//!
	//! Result needs to be deleted after use.
	__declspec(property(get = GetErrorMessage)) const char *Message;
	//! Gets a string representation of the immediate frames on the call stack.
	//!
	//! Result needs to be deleted after use.
	__declspec(property(get = GetStackTrace)) const char *StackTrace;
	//! Gets the exception object that is a cause of this one.
	__declspec(property(get = GetInnerException)) mono::exception InnerException;

	//! Raises this exception in the Mono run-time.
	void Throw() const
	{
		MonoEnv->Objects->ThrowException(this->obj);
	}

	const char *GetErrorMessage() const
	{
		return ToNativeString(this->klass->GetProperty("Message")->Getter->ToInstance()->Invoke(this->obj, nullptr, true));
	}
	const char *GetStackTrace() const
	{
		return ToNativeString(this->klass->GetProperty("StackTrace")->Getter->ToInstance()->Invoke(this->obj, nullptr, true));
	}
	mono::exception GetInnerException() const
	{
		return this->GetProperty("InnerException")->Getter->ToInstance()->Invoke(this->obj);
	}
};