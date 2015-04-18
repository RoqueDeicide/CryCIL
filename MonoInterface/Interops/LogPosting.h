#pragma once

#include "IMonoInterface.h"

struct LogPostingInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() { return "Log"; }
	virtual const char *GetNameSpace() { return "CryCil.Engine.DebugServices"; }

	virtual void OnRunTimeInitialized();
	
	static void Post(IMiniLog::ELogType postType, mono::string text);
	static int  get_VerbosityLevel();
	static void set_VerbosityLevel(int level);
};