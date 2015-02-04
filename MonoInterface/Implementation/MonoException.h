#pragma once

#include "IMonoInterface.h"

struct MonoExceptionWrapper : IMonoException
{
private:
	union
	{
		MonoException *monoEx;
		mono::exception ex;
	};
	IMonoHandle *handle;
public:

	MonoExceptionWrapper(MonoException *monoEx);
	MonoExceptionWrapper(mono::exception ex);

	virtual void Throw();

	virtual const char *GetErrorMessage();

	virtual const char *GetStackTrace();

	virtual IMonoException *GetInnerException();

	virtual mono::exception GetExceptionObject();

	virtual mono::object Get();

	virtual void GetField(const char *name, void *value);

	virtual void SetField(const char *name, void *value);

	virtual IMonoProperty *GetProperty(const char *name);

	virtual IMonoEvent *GetEvent(const char *name);

	virtual IMonoClass *GetClass();

	virtual void *GetWrappedPointer();

};