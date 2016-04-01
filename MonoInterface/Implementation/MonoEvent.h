#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"
#include "MonoStaticMethod.h"

struct MonoEventWrapper : public IMonoEvent
{
private:
	MonoEvent *_event;
	IMonoClass *klass;

	const IMonoFunction *add;
	const IMonoFunction *remove;
	const IMonoFunction *raise;

	int raiseDefined;			//!< -1 - unchecked; 0 - raise method is not defined; 1 - defined.
public:
	explicit MonoEventWrapper(MonoEvent *_event, IMonoClass *klass = nullptr);

	virtual const IMonoFunction *GetAdd() const override;
	virtual const IMonoFunction *GetRemove() const override;
	virtual const IMonoFunction *GetRaise() const override;

	virtual const char *GetName() const override;
	virtual void       *GetWrappedPointer() const override;
	virtual IMonoClass *GetDeclaringClass() const override;
};