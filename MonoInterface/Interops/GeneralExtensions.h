#pragma once

#include "IMonoInterface.h"

struct GeneralExtensionsInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() { return "GeneralExtensions"; }
	virtual const char *GetNameSpace() { return "CryCil"; }

	virtual void OnRunTimeInitialized();

	static void CopyToBuffer(mono::string str, wchar_t *chars, int start, int count);
};