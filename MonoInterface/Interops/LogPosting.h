#pragma once

#include "IMonoInterface.h"

struct LogPostingInterop : IDefaultMonoInterop
{

	virtual const char * GetName()
	{
		return "LogPosting";
	}

	virtual void OnRunTimeInitialized()
	{
		REGISTER_METHOD(Post);
		REGISTER_METHOD(GetVerboxity);
		REGISTER_METHOD(SetVerbosity);
	}
	static void Post(int postType, mono::string text)
	{
		gEnv->pLog->LogV((IMiniLog::ELogType)postType, ToNativeString(text), 0);
	}
	static int GetVerboxity()
	{
		return gEnv->pLog->GetVerbosityLevel();
	}
	static void SetVerbosity(int level)
	{
		gEnv->pLog->SetVerbosity(level);
	}
};