#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"
#include "MonoStaticMethod.h"

struct MonoEventWrapper : public IMonoEvent
{
private:
	MonoEvent *_event;
	IMonoClass *klass;

	IMonoFunction *add;
	IMonoFunction *remove;
	IMonoFunction *raise;

	int raiseDefined;			//!< -1 - unchecked; 0 - raise method is not defined; 1 - defined.
public:
	explicit MonoEventWrapper(MonoEvent *_event, IMonoClass *klass = nullptr);

	virtual IMonoFunction *GetAdd() override;
	virtual IMonoFunction *GetRemove() override;
	virtual IMonoFunction *GetRaise() override;

	virtual const char *GetName() override;
	virtual void       *GetWrappedPointer() override;
	virtual IMonoClass *GetDeclaringClass() override;
};