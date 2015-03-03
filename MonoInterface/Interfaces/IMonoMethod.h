#pragma once

#include "IMonoAliases.h"

//! Defines interface of objects that wrap functionality of MonoMethod type.
//!
//! WARNING: Due to virtual inheritance if you want to cast IMonoFunction to IMonoMethod, don't use
//!          C-style downcasts, use dynamic_cast operator instead. (/GR compiler option is necessary).
struct IMonoMethod : public virtual IMonoFunction
{
	//! Invokes this method without any parameters.
	//!
	//! Since extension methods are static by their internal nature, you can pass null
	//! as object parameter, and that can work, if extension method is not using the
	//! instance. It's up to you to find uses for that minor detail.
	//!
	//! @param object    Pointer to the instance to use, if this method is not static, it can be null
	//!                  otherwise. If you want to invoke this method on an instance of value type, you
	//!                  should either pass the pointer to it in unmanaged memory, or unbox it and pass
	//!                  the returned pointer.
	//! @param exc       An optional pointer to the exception object that can hold a reference
	//!                  the unhandled exception that could have been raised during method's
	//!                  execution.
	//! @param polymorph Indicates whether we need to invoke a virtual method,
	//!                  that is specific to the instance.
	//!
	//! @returns A reference to the result of execution, if no unhandled exception was thrown.
	//!          If result is of value-type, it's boxed.
	//! @example DoxygenExampleFiles\MonoMethodInvocations.h
	VIRTUAL_API virtual mono::object Invoke
		(void *object, mono::exception *exc = nullptr, bool polymorph = false) = 0;
	//! Invokes this method.
	//!
	//! Since extension methods are static by their internal nature, you can pass null
	//! as object parameter, and that can work, if extension method is not using the
	//! instance. It's up to you to find uses for that minor detail.
	//!
	//! @param object    Pointer to the instance to use, if this method is not static, it can be null
	//!                  otherwise. If you want to invoke this method on an instance of value type, you
	//!                  should either pass the pointer to it in unmanaged memory, or unbox it and pass
	//!                  the returned pointer.
	//! @param params    Pointer to the mono array of parameters to pass to the method.
	//!                  Pass null, if method can accept no arguments.
	//! @param exc       An optional pointer to the exception object that can hold a reference the unhandled
	//!                  exception that could have been raised during method's execution.
	//! @param polymorph Indicates whether we need to invoke a virtual method, that is specific to the
	//!                  instance.
	//!
	//! @returns A reference to the result of execution, if no unhandled exception was thrown.
	//!          If result is of value-type, it's boxed.
	//! @example DoxygenExampleFiles\MonoMethodInvocations.h
	VIRTUAL_API virtual mono::object Invoke
		(void *object, IMonoArray *params, mono::exception *exc = nullptr, bool polymorph = false) = 0;
	//! Invokes this method.
	//!
	//! Since extension methods are static by their internal nature, you can pass null
	//! as object parameter, and that can work, if extension method is not using the
	//! instance. It's up to you to find uses for that minor detail.
	//!
	//! @param object    Pointer to the instance to use, if this method is not static, it can be null
	//!                  otherwise. If you want to invoke this method on an instance of value type, you
	//!                  should either pass the pointer to it in unmanaged memory, or unbox it and pass
	//!                  the returned pointer.
	//! @param params    Pointer to the array of parameters to pass to the method. Pass null, if method can
	//!                  accept no arguments.
	//! @param exc       An optional pointer to the exception object that can hold a reference the unhandled
	//!                  exception that could have been raised during method's execution.
	//! @param polymorph Indicates whether we need to invoke a virtual method, that is specific to the
	//!                  instance.
	//!
	//! @returns A reference to the result of execution, if no unhandled exception was thrown.
	//!          If result is of value-type, it's boxed.
	//! @example DoxygenExampleFiles\MonoMethodInvocations.h
	VIRTUAL_API virtual mono::object Invoke
		(void *object, void **params, mono::exception *exc = nullptr, bool polymorph = false) = 0;
};

__forceinline IMonoMethod *IMonoFunction::ToInstance()
{
#ifdef _CPPRTTI
	return dynamic_cast<IMonoMethod *>(this);
#else
	return this->DynamicCastToInstance();
#endif //_CPPRTTI
}