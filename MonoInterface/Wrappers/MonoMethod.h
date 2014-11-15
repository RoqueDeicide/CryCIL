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
	virtual mono::object Invoke(mono::object object, IMonoArray *params = nullptr, bool polymorph = false);
	//! Invokes this method.
	virtual mono::object Invoke(mono::object object, void **params = nullptr, bool polymorph = false);

	virtual void * GetThunk();

	virtual const char * GetName();

	virtual int GetParameterCount();

	virtual void * GetWrappedPointer();
};