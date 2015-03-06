#pragma once

#include "IMonoInterface.h"
#include "MonoDelegate.h"

typedef mono::delegat(__stdcall *CreateStaticDelegate)(mono::type, mono::object, mono::exception *);
typedef mono::delegat(__stdcall *CreateInstanceDelegate)(mono::type, mono::object, mono::object, mono::exception *);

struct MonoDelegates : public IMonoDelegates
{
private:
	static CreateStaticDelegate             createStaticDelegate;
	static CreateInstanceDelegate           createInstanceDelegate;
public:
	virtual IMonoDelegate *Wrap(mono::delegat delegat);

	virtual IMonoDelegate *Create(IMonoClass *delegateType, IMonoMethod *method, IMonoClass *klass);

	virtual IMonoDelegate *Create(IMonoClass *delegateType, IMonoMethod *method, mono::object target);

};