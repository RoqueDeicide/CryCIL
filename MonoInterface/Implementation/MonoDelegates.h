#pragma once

#include "IMonoInterface.h"

typedef mono::delegat(__stdcall *CreateStaticDelegate)(mono::type, mono::object, mono::exception *);
typedef mono::delegat(__stdcall *CreateInstanceDelegate)(mono::type, mono::object, mono::object, mono::exception *);
typedef mono::delegat(__stdcall *CreateDelegateForFunctionPointer)(mono::type, void *, mono::exception *);

struct MonoDelegates : public IMonoDelegates
{
private:
	static CreateStaticDelegate             createStaticDelegate;
	static CreateInstanceDelegate           createInstanceDelegate;
	static CreateDelegateForFunctionPointer createDelegateForFunctionPointer;
public:
	virtual mono::delegat Create(IMonoClass *delegateType, IMonoMethod *method);
	virtual mono::delegat Create(IMonoClass *delegateType, IMonoMethod *method, mono::object target);
	virtual mono::delegat Create(IMonoClass *delegateType, void *functionPointer);
};