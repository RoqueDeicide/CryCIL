#pragma once

//! Base interface for Mono static functions.
struct IMonoStaticMethod : public IMonoFunction
{
protected:
	IMonoStaticMethod(_MonoMethod *method, IMonoClass *klass = nullptr) : IMonoFunction(method, klass) {}
public:
	//! Invokes this method without any parameters.
	//!
	//! Since extension methods are static by their internal nature, you can pass null
	//! as object parameter, and that can work, if extension method is not using the
	//! instance. It's up to you to find uses for that minor detail.
	//!
	//! @param exc       An optional pointer to the exception object that can hold a reference
	//!                  the unhandled exception that could have been raised during method's
	//!                  execution.
	//!
	//! @returns A reference to the result of execution, if no unhandled exception was thrown.
	//!          If result is of value-type, it's boxed.
	//! @example DoxygenExampleFiles\MonoMethodInvocations.h
	VIRTUAL_API virtual mono::object Invoke(mono::exception *exc = nullptr) const = 0;
	//! Invokes this method.
	//!
	//! Since extension methods are static by their internal nature, you can pass null
	//! as object parameter, and that can work, if extension method is not using the
	//! instance. It's up to you to find uses for that minor detail.
	//!
	//! @param params    Pointer to the mono array of parameters to pass to the method.
	//!                  Pass null, if method can accept no arguments.
	//! @param exc       An optional pointer to the exception object that can hold a reference the unhandled
	//!                  exception that could have been raised during method's execution.
	//!
	//! @returns A reference to the result of execution, if no unhandled exception was thrown.
	//!          If result is of value-type, it's boxed.
	//! @example DoxygenExampleFiles\MonoMethodInvocations.h
	VIRTUAL_API virtual mono::object Invoke(IMonoArray<> &params, mono::exception *exc = nullptr) const = 0;
	//! Invokes this method.
	//!
	//! Since extension methods are static by their internal nature, you can pass null
	//! as object parameter, and that can work, if extension method is not using the
	//! instance. It's up to you to find uses for that minor detail.
	//!
	//! @param params    Pointer to the array of parameters to pass to the method. Pass null, if method can
	//!                  accept no arguments.
	//! @param exc       An optional pointer to the exception object that can hold a reference the unhandled
	//!                  exception that could have been raised during method's execution.
	//!
	//! @returns A reference to the result of execution, if no unhandled exception was thrown.
	//!          If result is of value-type, it's boxed.
	//! @example DoxygenExampleFiles\MonoMethodInvocations.h
	VIRTUAL_API virtual mono::object Invoke(void **params, mono::exception *exc = nullptr) const = 0;
};

__forceinline const IMonoStaticMethod *IMonoFunction::ToStatic() const
{
	return static_cast<const IMonoStaticMethod *>(this);
}