#pragma once

#include "IMonoAliases.h"

//! Base class for objects that represent members of the class.
struct IMonoMember : public IMonoFunctionalityWrapper
{
	//! Gets the name of the method.
	__declspec(property(get = GetName)) const char *Name;
	//! Gets a pointer to the class where this method is declared.
	__declspec(property(get = GetDeclaringClass)) IMonoClass *DeclaringClass;

	VIRTUAL_API virtual const char *GetName() = 0;
	VIRTUAL_API virtual IMonoClass *GetDeclaringClass() = 0;
};