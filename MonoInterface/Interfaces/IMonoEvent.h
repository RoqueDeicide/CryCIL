#pragma once

#include "IMonoAliases.h"

//! Represents a wrapper for a Mono event.
struct IMonoEvent : public IMonoMember
{
	//! Gets IMonoMethod that allows you to subscribe to the event.
	__declspec(property(get = GetAdd)) IMonoFunction *Add;
	//! Gets IMonoMethod that allows you to unsubscribe from the event.
	__declspec(property(get = GetRemove)) IMonoFunction *Remove;
	//! Gets IMonoMethod that allows you to raise the event.
	//!
	//! Since C# and Visual Basic compilers do not generate such method by default, after failing
	//! to get it normally this property will try looking for the method which name is
	//! On<Name of the event>. For instance if the event is called Closed, then raise method would be
	//! OnClosed.
	__declspec(property(get = GetRaise)) IMonoFunction *Raise;

	VIRTUAL_API virtual const IMonoFunction *GetAdd() const = 0;
	VIRTUAL_API virtual const IMonoFunction *GetRemove() const = 0;
	VIRTUAL_API virtual const IMonoFunction *GetRaise() const = 0;
};