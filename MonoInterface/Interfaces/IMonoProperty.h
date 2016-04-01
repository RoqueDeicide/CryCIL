#pragma once

#include "IMonoAliases.h"

//! Represents a wrapper for a Mono property.
struct IMonoProperty : public IMonoMember
{
	//! Gets IMonoMethod that allows you to invoke a getter of the property.
	__declspec(property(get = GetGetter)) IMonoFunction *Getter;
	//! Gets IMonoMethod that allows you to invoke a setter of the property.
	__declspec(property(get = GetSetter)) IMonoFunction *Setter;
	//! Gets IMonoMethod that allows you to identify the signature of this property.
	__declspec(property(get = GetIdentifier)) IMonoFunction *Identifier;
	//! Gets the number of parameters this property uses.
	__declspec(property(get = GetParameterCount)) int ParameterCount;

	VIRTUAL_API virtual const IMonoFunction *GetGetter() const = 0;
	VIRTUAL_API virtual const IMonoFunction *GetSetter() const = 0;
	VIRTUAL_API virtual const IMonoFunction *GetIdentifier() const = 0;
	VIRTUAL_API virtual int GetParameterCount() const = 0;

	// Internal method, just ignore it.
	__forceinline const IMonoFunction *GetFunc() const
	{
		return this->Identifier;
	}
};