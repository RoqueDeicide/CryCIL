#pragma once

#include "IMonoAliases.h"

//! Provides access to functions that create and wrap Mono delegates.
struct IMonoDelegates
{
	//! Wraps a pointer to the delegate.
	//!
	//! Wrapper needs to be deleted when not used anymore.
	VIRTUAL_API virtual IMonoDelegate *Wrap(mono::delegat delegat) = 0;
	//! Creates a new delegate for a static method.
	//!
	//! Wrapper needs to be deleted when not used anymore.
	//!
	//! @param assembly     Assembly where the delegate type is defined.
	//! @param delegateType Type that represents the delegate that needs to be created.
	//! @param name         Name of the delegate type.
	//! @param method       Method for which the delegate is made.
	//! @param klass        Type where static method is defined.
	VIRTUAL_API virtual IMonoDelegate *Create
		(IMonoClass *delegateType, IMonoMethod *method, IMonoClass *klass) = 0;
	//! Creates a new delegate for an instance method.
	//!
	//! Wrapper needs to be deleted when not used anymore.
	//!
	//! @param assembly     Assembly where the delegate type is defined.
	//! @param delegateType Type that represents the delegate that needs to be created.
	//! @param method       Method for which the delegate is made.
	//! @param target       Target of invocation.
	VIRTUAL_API virtual IMonoDelegate *Create
		(IMonoClass *delegateType, IMonoMethod *method, mono::object target) = 0;
};