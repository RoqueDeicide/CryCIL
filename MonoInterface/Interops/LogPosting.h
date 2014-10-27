#pragma once

#include "IMonoInterface.h"

struct LogPostingInterop : public IDefaultMonoInterop
{

	virtual const char *GetName();

	virtual void OnRunTimeInitialized();
	static void Post(int postType, mono::string text);
	static int GetVerboxity();
	static void SetVerbosity(int level);
};