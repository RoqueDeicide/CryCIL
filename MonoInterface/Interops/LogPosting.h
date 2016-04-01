#pragma once

#include "IMonoInterface.h"

struct LogPostingInterop : public IMonoInterop<false, true>
{
	LogPostingInterop();

	virtual const char *GetInteropClassName() override { return "Log"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.DebugServices"; }

	virtual void InitializeInterops() override {}
	
	static void Post(IMiniLog::ELogType postType, mono::string text);
	static int  get_VerbosityLevel();
	static void set_VerbosityLevel(int level);
};