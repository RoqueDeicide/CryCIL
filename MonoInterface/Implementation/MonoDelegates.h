#pragma once

#include "IMonoInterface.h"

typedef mono::delegat(__stdcall *CreateStaticDelegate)(mono::type, mono::object, mono::exception *);
typedef mono::delegat(__stdcall *CreateInstanceDelegate)(mono::type, mono::object, mono::object,
														 mono::exception *);
typedef mono::delegat(__stdcall *CreateDelegateForFunctionPointer)(void *, mono::type, mono::exception *);

struct MonoDelegates : public IMonoDelegates
{
private:
	static CreateStaticDelegate             createStaticDelegate;
	static CreateInstanceDelegate           createInstanceDelegate;
	static CreateDelegateForFunctionPointer createDelegateForFunctionPointer;
public:
	virtual mono::delegat Create(IMonoClass *delegateType, IMonoStaticMethod *method) override;
	virtual mono::delegat Create(IMonoClass *delegateType, IMonoMethod *method, mono::object target) override;
	virtual mono::delegat Create(IMonoClass *delegateType, void *functionPointer) override;
};