#pragma once

#include "IMonoInterface.h"

struct LogPostingInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() override { return "Log"; }
	virtual const char *GetNameSpace() override { return "CryCil.Engine.DebugServices"; }

	virtual void OnRunTimeInitialized() override;
	
	static void Post(IMiniLog::ELogType postType, mono::string text);
	static int  get_VerbosityLevel();
	static void set_VerbosityLevel(int level);
};