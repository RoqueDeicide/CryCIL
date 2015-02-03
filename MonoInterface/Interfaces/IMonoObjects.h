#pragma once

#include "IMonoAliases.h"

#include "IMonoArrays.h"
#include "IMonoTexts.h"
#include "IMonoExceptions.h"
#include "IMonoDelegates.h"
#include "IMonoThreads.h"

//! Provides access to functions that create and wrap various Mono objects.
struct IMonoObjects
{
	virtual ~IMonoObjects() {}

	//! Gets the object that allows creation and wrapping of Mono arrays.
	__declspec(property(get = GetArrays))     IMonoArrays       *Arrays;
	//! Gets the object that allows creation of general Mono strings.
	__declspec(property(get = GetTexts))      IMonoTexts        *Texts;
	//! Gets the object that allows creation and wrapping of Mono exceptions.
	__declspec(property(get = GetExceptions)) IMonoExceptions   *Exceptions;
	//! Gets the object that allows creation and wrapping of Mono delegates.
	__declspec(property(get = GetDelegates))  IMonoDelegates    *Delegates;
	//! Gets the object that allows boxing of number of built-in types.
	__declspec(property(get = GetBoxinator))  IDefaultBoxinator *Boxer;
	//! Gets the object that provides access to Mono threads interface.
	__declspec(property(get = GetThreads))    IMonoThreads      *Threads;

	//! Creates a new wrapper for given MonoObject.
	//!
	//! @param obj An object to wrap.
	VIRTUAL_API virtual IMonoHandle *Wrap(mono::object obj) = 0;
	//! Unboxes managed value-type object.
	//!
	//! @param value Value-type object to unbox.
	VIRTUAL_API virtual void *Unbox(mono::object value) = 0;

	VIRTUAL_API virtual IMonoArrays       *GetArrays() = 0;
	VIRTUAL_API virtual IMonoTexts        *GetTexts() = 0;
	VIRTUAL_API virtual IMonoExceptions   *GetExceptions() = 0;
	VIRTUAL_API virtual IMonoDelegates    *GetDelegates() = 0;
	VIRTUAL_API virtual IDefaultBoxinator *GetBoxinator() = 0;
	VIRTUAL_API virtual IMonoThreads      *GetThreads() = 0;
};