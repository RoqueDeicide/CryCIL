#pragma once

#include "IMonoInterface.h"

struct MonoFunctions : public IMonoFunctions
{
	virtual IMonoClass *GetDeclaringClass(_MonoMethod *method) override;
	virtual const char *GetName(_MonoMethod *method) override;

	virtual void  AddInternalCall(const char *nameSpace, const char *className, const char *name,
								  void *functionPointer) override;
	virtual void *LookupInternalCall(const IMonoFunction *func) override;

	virtual mono::object InternalInvoke(_MonoMethod *func, void *object, void **args, mono::exception *ex,
										bool polymorph) override;
	virtual mono::object InternalInvokeArray(_MonoMethod *func, void *object, IMonoArray<> &args,
											 mono::exception *ex, bool polymorph) override;

	virtual void *GetUnmanagedThunk(_MonoMethod *func) override;
	virtual void *GetRawThunk(_MonoMethod *func) override;
	virtual int   ParseSignature(_MonoMethod *func, List<const char *> &names, const char *&params) override;
	virtual void  GetParameterClasses(_MonoMethod *func, List<IMonoClass *> &classes) override;
	
	virtual mono::object GetReflectionObject(_MonoMethod *func) override;
};