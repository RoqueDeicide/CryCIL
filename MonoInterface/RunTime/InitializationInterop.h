#pragma once

#include "IMonoInterface.h"
//! Broadcasts CryCIL events to listeners from managed code.
struct InitializationInterop : public IDefaultMonoInterop
{
	static class MonoInterface *monoEnv;	//!< For static functions.
	virtual const char *GetName();

	virtual void OnRunTimeInitialized();

	virtual void SetInterface(IMonoInterface *handle);

	static void OnCompilationStartingBind();
	static void OnCompilationCompleteBind(bool success);
	static mono::Array GetSubscribedStagesBind();
	static void OnInitializationStageBind(int stageIndex);
};