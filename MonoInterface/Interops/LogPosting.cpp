#include "stdafx.h"
#include "LogPosting.h"

const char *LogPostingInterop::GetName()
{
	return "LogPosting";
}

void LogPostingInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(Post);
	REGISTER_METHOD(GetVerboxity);
	REGISTER_METHOD(SetVerbosity);
}
void LogPostingInterop::Post(int postType, mono::string text)
{
	gEnv->pLog->LogV((IMiniLog::ELogType)postType, ToNativeString(text), 0);
}
int LogPostingInterop::GetVerboxity()
{
	return gEnv->pLog->GetVerbosityLevel();
}
void LogPostingInterop::SetVerbosity(int level)
{
	gEnv->pLog->SetVerbosity(level);
}