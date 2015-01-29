#pragma once

#include "IMonoAliases.h"

#include "IMonoArrays.h"
#include "IMonoTexts.h"
#include "IMonoExceptions.h"

//! Provides access to functions that create and wrap various Mono objects.
struct IMonoObjects
{
	//! Gets the object that allows creation and wrapping of Mono arrays.
	__declspec(property(get = GetArrays)) IMonoArrays *Arrays;
	//! Gets the object that allows creation of general Mono strings.
	__declspec(property(get = GetTexts)) IMonoTexts *Texts;
	//! Gets the object that allows creation and wrapping of Mono arrays.
	__declspec(property(get = GetExceptions)) IMonoExceptions *Exceptions;

	//! Creates a new MonoObject using constructor with specific parameters.
	//!
	//! @param assembly   Assembly where the type of the object is defined.
	//! @param name_space Name space that contains the type of the object.
	//! @param class_name Name of the type to use.
	//! @param params     An array of parameters to pass to the constructor.
	//!                   If null, default constructor will be used.
	VIRTUAL_API virtual mono::object Create
		(IMonoAssembly *assembly,
		const char *name_space, const char *class_name,
		IMonoArray *params = nullptr) = 0;
	//! Creates a new wrapper for given MonoObject.
	//!
	//! @param obj An object to wrap.
	VIRTUAL_API virtual IMonoHandle *Wrap(mono::object obj) = 0;
	//! Unboxes managed value-type object.
	//!
	//! @param value Value-type object to unbox.
	VIRTUAL_API virtual void *Unbox(mono::object value) = 0;

	VIRTUAL_API virtual IMonoArrays *GetArrays() = 0;
	VIRTUAL_API virtual IMonoTexts *GetTexts() = 0;
	VIRTUAL_API virtual IMonoExceptions *GetExceptions() = 0;
};