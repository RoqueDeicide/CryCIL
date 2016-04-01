#include "stdafx.h"

#include "WriteLockCond.h"

void WriteLockCondInterop::InitializeInterops()
{
	REGISTER_METHOD(CreateLock);
	REGISTER_METHOD(ReleaseLock);
}

WriteLockCond *WriteLockCondInterop::CreateLock()
{
	return new WriteLockCond();
}

void WriteLockCondInterop::ReleaseLock(WriteLockCond *handle)
{
	delete handle;
}
