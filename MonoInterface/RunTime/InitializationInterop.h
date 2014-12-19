#pragma once

#include "IMonoInterface.h"
//! Broadcasts CryCIL events to listeners from managed code.
struct InitializationInterop : public IDefaultMonoInterop
{
	virtual const char *GetName();

	virtual void OnRunTimeInitialized();

	virtual void SetInterface(IMonoInterface *handle);
	virtual void OnPostInitialization()
	{
		MonoEnv->RemoveListener(this);
		delete this;
	}

	static void OnCompilationStartingBind();
	static void OnCompilationCompleteBind(bool success);
	static mono::Array GetSubscribedStagesBind();
	static void OnInitializationStageBind(int stageIndex);
};