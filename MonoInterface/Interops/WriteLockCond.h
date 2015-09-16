#pragma once

#include "IMonoInterface.h"

struct WriteLockCondInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "WriteLockCond"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Utilities"; }

	virtual void OnRunTimeInitialized() override;

	static WriteLockCond *CreateLock();
	static void ReleaseLock(WriteLockCond *handle);
};