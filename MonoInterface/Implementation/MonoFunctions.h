#pragma once

#include "IMonoInterface.h"

struct MonoFunctions : public IMonoFunctions
{


	virtual IMonoClass *GetDeclaringClass(_MonoMethod *method);
	virtual const char *GetName(_MonoMethod *method);

	virtual void  AddInternalCall(const char *nameSpace, const char *className, const char *name, void *functionPointer);
	virtual void *LookupInternalCall(IMonoFunction *func);

	virtual mono::object InternalInvoke(_MonoMethod *func, void *object, void **args, mono::exception *ex, bool polymorph);
	virtual mono::object InternalInvokeArray(_MonoMethod *func, void *object, IMonoArray<> &args, mono::exception *ex, bool polymorph);

	virtual void *GetThunk(_MonoMethod *func);
	virtual void *GetFunctionPointer(_MonoMethod *func);
	virtual int   ParseSignature(_MonoMethod *func, List<const char *> &names, const char *&params);
	virtual void  GetParameterClasses(_MonoMethod *func, List<IMonoClass *> &classes);
	
	virtual mono::object GetReflectionObject(_MonoMethod *func);
};