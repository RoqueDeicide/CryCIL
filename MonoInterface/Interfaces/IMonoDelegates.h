#pragma once

#include "IMonoAliases.h"

//! Provides access to functions that create and wrap Mono delegates.
struct IMonoDelegates
{
	virtual ~IMonoDelegates() {}

	//! Creates a new delegate for a static method.
	//!
	//! @param delegateType Type that represents the delegate that needs to be created.
	//! @param name         Name of the delegate type.
	//! @param method       Method for which the delegate is made.
	VIRTUAL_API virtual mono::delegat Create(IMonoClass *delegateType, IMonoStaticMethod *method) = 0;
	//! Creates a new delegate for an instance method.
	//!
	//! @param delegateType Type that represents the delegate that needs to be created.
	//! @param method       Method for which the delegate is made.
	//! @param target       Target of invocation.
	VIRTUAL_API virtual mono::delegat Create(IMonoClass *delegateType, IMonoMethod *method,
											 mono::object target) = 0;
	//! Creates a delegate that wraps an unmanaged function pointer.
	//!
	//! There is a number of restrictions on this function:
	//! 1) Generics are not supported in interop scenarios.
	//!
	//! 2) You cannot pass an invalid function pointer to this method.
	//!
	//! 3) You can use this method only for pure unmanaged function pointers.
	//!
	//! 4) You cannot use this method with function pointers obtained through C++ or from the
	//!    GetFunctionPointer method.
	//!
	//! 5) You cannot use this method to create a delegate from a function pointer to another managed
	//!    delegate.
	//!
	//! 6) The function pointer will be invoked via Platform Invoke mechanism, which means that,
	//!    if your delegate accepts or returns any managed objects those must be made marshalable.
	//!    You cannot create delegates from function pointers using most delegate types that are
	//!    defined in .Net/Mono assemblies since they lack any marshaling information.
	//!
	//! @param delegateType    Type of the delegate to use. It needs to match the signature of unmanaged
	//!                        method.
	//! @param functionPointer Pointer to the function that will be wrapped by the delegate.
	VIRTUAL_API virtual mono::delegat Create(IMonoClass *delegateType, void *functionPointer) = 0;
};