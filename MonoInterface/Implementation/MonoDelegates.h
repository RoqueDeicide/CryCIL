#pragma once

#include "IMonoInterface.h"

typedef mono::delegat(__stdcall *CreateStaticDelegate)(mono::type, mono::object, mono::exception *);
typedef mono::delegat(__stdcall *CreateInstanceDelegate)(mono::type, mono::object, mono::object, mono::exception *);
typedef mono::delegat(__stdcall *CreateDelegateForFunctionPointer)(void *, mono::type, mono::exception *);

struct MonoDelegates : public IMonoDelegates
{
private:
	static CreateStaticDelegate             createStaticDelegate;
	static CreateInstanceDelegate           createInstanceDelegate;
	static CreateDelegateForFunctionPointer createDelegateForFunctionPointer;
public:
	mono::delegat Create(IMonoClass *delegateType, const IMonoStaticMethod *method) override;
	mono::delegat Create(IMonoClass *delegateType, const IMonoMethod *method, mono::object target) override;
	mono::delegat Create(IMonoClass *delegateType, void *functionPointer) override;
};