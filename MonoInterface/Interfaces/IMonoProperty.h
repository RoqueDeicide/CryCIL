#pragma once

#include "IMonoAliases.h"

//! Represents a wrapper for a Mono property.
struct IMonoProperty : public IMonoMember
{
	//! Gets IMonoMethod that allows you to invoke a getter of the property.
	__declspec(property(get = GetGetter)) IMonoMethod *Getter;
	//! Gets IMonoMethod that allows you to invoke a setter of the property.
	__declspec(property(get = GetSetter)) IMonoMethod *Setter;
	//! Gets IMonoMethod that allows you to identify the signature of this property.
	__declspec(property(get = GetIdentifier)) IMonoMethod *Identifier;

	VIRTUAL_API virtual IMonoMethod *GetGetter() = 0;
	VIRTUAL_API virtual IMonoMethod *GetSetter() = 0;
	VIRTUAL_API virtual IMonoMethod *GetIdentifier() = 0;
};