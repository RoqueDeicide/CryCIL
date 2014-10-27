#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"

struct MonoMethodWrapper : public IMonoMethod
{
private:
	MonoMethod *wrappedMethod;
	MonoMethodSignature *signature;
	int paramCount;
	const char *name;
public:
	MonoMethodWrapper(MonoMethod *method);
	//! Invokes this method.
	//!
	//! @remark Since extension methods are static by their internal nature,
	//!         you can pass null as object parameter, and that can work,
	//!         if extension method is not using the instance. It's up to
	//!         you to find uses for that minor detail.
	//!
	//! @param object    Pointer to the instance to use, if this method is not
	//!                  static, it can be null otherwise.
	//! @param params    Pointer to the mono array of parameters to pass to the method.
	//!                  Pass null, if method can accept no arguments.
	//! @param polymorph Indicates whether we need to invoke a virtual method,
	//!                  that is specific to the instance.
	virtual mono::object Invoke(mono::object object, IMonoArray *params = nullptr, bool polymorph = false);
	//! Invokes this method.
	//!
	//! @remark Since extension methods are static by their internal nature,
	//!         you can pass null as object parameter, and that can work,
	//!         if extension method is not using the instance. It's up to
	//!         you to find uses for that minor detail.
	//!
	//! @param object     Pointer to the instance to use, if this method is not
	//!                   static, it can be null otherwise.
	//! @param params     Pointer to the array of parameters to pass to the method.
	//!                   Pass null, if method can accept no arguments.
	//! @param polymorph  Indicates whether we need to invoke a virtual method,
	//!                   that is specific to the instance.
	virtual mono::object Invoke(mono::object object, void **params = nullptr, bool polymorph = false);

	virtual void * GetThunk();

	virtual const char * GetName();

	virtual int GetParameterCount();

	virtual void * GetWrappedPointer();
};