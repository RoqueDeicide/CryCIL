#pragma once

#include "IMonoInterface.h"

struct LogPostingInterop : public IDefaultMonoInterop<true>
{
	virtual const char *GetName();

	virtual void OnRunTimeInitialized();
	
	static int  GetVerboxity();
	static void SetVerbosity(int level);
	static void Post(IMiniLog::ELogType postType, mono::string text);
};