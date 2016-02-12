#include "stdafx.h"
#include "LogPosting.h"

LogPostingInterop::LogPostingInterop()
{
	REGISTER_METHOD(Post);
	REGISTER_METHOD(get_VerbosityLevel);
	REGISTER_METHOD(set_VerbosityLevel);

	auto func = this->GetInteropClass(MonoEnv->Cryambly)->GetFunction("RedirectStdOutput", -1);
	void(*thunk)() = static_cast<void(*)()>(func->RawThunk);

	thunk();
}

void LogPostingInterop::Post(IMiniLog::ELogType postType, mono::string text)
{
	NtText message(text);

	//CryLogAlways("Posting a message: %s", message);

	gEnv->pLog->LogWithType(postType, message);
}
int LogPostingInterop::get_VerbosityLevel()
{
	return gEnv->pLog->GetVerbosityLevel();
}
void LogPostingInterop::set_VerbosityLevel(int level)
{
	gEnv->pLog->SetVerbosity(level);
}