#pragma once

#include "IMonoInterface.h"
#include "MonoDelegate.h"

struct MonoDelegates : public IMonoDelegates
{


	virtual IMonoDelegate *Wrap(mono::delegat delegat);

	virtual IMonoDelegate *Create(IMonoClass *delegateType, IMonoMethod *method, IMonoClass *klass);

	virtual IMonoDelegate *Create(IMonoClass *delegateType, IMonoMethod *method, mono::object target);

};