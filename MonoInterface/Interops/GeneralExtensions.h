#pragma once

#include "IMonoInterface.h"

struct GeneralExtensionsInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return ""; }
	virtual const char *GetInteropNameSpace() override { return "CryCil"; }

	virtual void InitializeInterops() override;

	static void CopyToBuffer(mono::string str, wchar_t *chars, int start, int count);
};