#pragma once

#include "IMonoAliases.h"

//! Wraps a Mono exception object.
//!
//! It is highly recommended to pin mono::exception objects using Mono GC handles.
struct IMonoException : public IMonoHandle
{
	//! Gets the message provided with this exception object.
	//!
	//! Result needs to be deleted after use.
	__declspec(property(get = GetMessage)) const char *Message;
	//! Gets a string representation of the immediate frames on the call stack.
	//!
	//! Result needs to be deleted after use.
	__declspec(property(get = GetStackTrace)) const char *StackTrace;
	//! Gets the exception object that is a cause of this one.
	//!
	//! Result needs to be deleted after use.
	__declspec(property(get = GetInnerException)) IMonoException *InnerException;
	//! Gets the wrapped exception object.
	__declspec(property(get = GetExceptionObject)) mono::exception WrappedException;

	//! Raises this exception in the Mono run-time.
	VIRTUAL_API virtual void Throw() = 0;

	VIRTUAL_API virtual const char *GetErrorMessage() = 0;
	VIRTUAL_API virtual const char *GetStackTrace() = 0;
	VIRTUAL_API virtual IMonoException *GetInnerException() = 0;
	VIRTUAL_API virtual mono::exception GetExceptionObject() = 0;
};