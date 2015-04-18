#include "stdafx.h"
#include "LogPosting.h"

void LogPostingInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(Post);
	REGISTER_METHOD(get_VerbosityLevel);
	REGISTER_METHOD(set_VerbosityLevel);
}

void LogPostingInterop::Post(IMiniLog::ELogType postType, mono::string text)
{
	gEnv->pLog->LogV(postType, NtText(text), 0);
}
int LogPostingInterop::get_VerbosityLevel()
{
	return gEnv->pLog->GetVerbosityLevel();
}
void LogPostingInterop::set_VerbosityLevel(int level)
{
	gEnv->pLog->SetVerbosity(level);
}