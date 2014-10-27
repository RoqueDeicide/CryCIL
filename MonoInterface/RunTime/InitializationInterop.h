#pragma once

#include "IMonoInterface.h"
//! Broadcasts CryCIL events to listeners from managed code.
struct InitializationInterop : public IDefaultMonoInterop
{
	static class MonoInterface *monoEnv;	//!< Just so I don't have to cast MonoEnv to MonoInterface every time.
	virtual const char *GetName();

	virtual void OnRunTimeInitialized();

	static void OnCompilationStartingBind();
	static void OnCompilationCompleteBind(bool success);
	static mono::Array GetSubscribedStagesBind();
	static void OnInitializationStageBind(int stageIndex);
};