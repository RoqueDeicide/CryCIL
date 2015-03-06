#pragma once

#include "IMonoInterface.h"
#include "MonoDelegate.h"

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
	virtual IMonoDelegate *Wrap(mono::delegat delegat);
	virtual IMonoDelegate *Create(IMonoClass *delegateType, IMonoMethod *method);
	virtual IMonoDelegate *Create(IMonoClass *delegateType, IMonoMethod *method, mono::object target);
	virtual IMonoDelegate *Create(IMonoClass *delegateType, void *functionPointer);
};