#pragma once

#include "IMonoInterface.h"

struct CustomMarshalingInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CustomMarshaling"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Utilities"; }

	virtual void OnRunTimeInitialized() override;

	static mono::string GetUtf8String(const char *stringHandle);
};