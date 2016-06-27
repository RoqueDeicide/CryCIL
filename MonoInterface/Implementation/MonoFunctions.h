#pragma once

#include "IMonoInterface.h"

struct MonoFunctions : public IMonoFunctions
{
	IMonoClass  *GetDeclaringClass(_MonoMethod *method) override;
	const char  *GetName(_MonoMethod *method) override;
	void         AddInternalCall(const char *nameSpace, const char *className, const char *name, void *functionPointer) override;
	void        *LookupInternalCall(const IMonoFunction *func) override;
	mono::object InternalInvoke(_MonoMethod *func, void *object, void **args, mono::exception *ex, bool polymorph) override;
	mono::object InternalInvokeArray(_MonoMethod *func, void *object, IMonoArray<> &args, mono::exception *ex, bool polymorph) override;
	void        *GetUnmanagedThunk(_MonoMethod *func) override;
	void        *GetRawThunk(_MonoMethod *func) override;
	int          ParseSignature(_MonoMethod *func, List<Text> &names, Text &params) override;
	void         GetParameterClasses(_MonoMethod *func, List<IMonoClass *> &classes) override;
	mono::object GetReflectionObject(_MonoMethod *func) override;
};