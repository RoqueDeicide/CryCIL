#pragma once

#include "IMonoAliases.h"

//! Represents a wrapper for a Mono property.
struct IMonoProperty : public IMonoFunctionalityWrapper
{
	//! Gets IMonoMethod that allows you to invoke a getter of the property.
	__declspec(property(get = GetGetter)) IMonoMethod *Getter;
	//! Gets IMonoMethod that allows you to invoke a setter of the property.
	__declspec(property(get = GetSetter)) IMonoMethod *Setter;
	//! Gets name of the property.
	__declspec(property(get = GetName)) const char *Name;

	VIRTUAL_API virtual IMonoMethod *GetGetter() = 0;
	VIRTUAL_API virtual IMonoMethod *GetSetter() = 0;
	VIRTUAL_API virtual const char *GetName() = 0;
};