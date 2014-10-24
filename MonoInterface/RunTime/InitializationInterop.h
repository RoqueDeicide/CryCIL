#pragma once

#include "RunTime/MonoInterface.h"
//! Broadcasts CryCIL events to listeners from managed code.
struct InitializationInterop : public IDefaultMonoInterop
{
	static MonoInterface *monoEnv;	//!< Just so I don't have to cast MonoEnv to MonoInterface every time.
	virtual const char * GetName()
	{
		return "Initialization";
	}

	virtual void OnRunTimeInitialized()
	{
		monoEnv = (MonoInterface *)MonoEnv;

		REGISTER_METHOD(OnCompilationStarting);
		REGISTER_METHOD(OnCompilationComplete);
		REGISTER_METHOD(GetSubscribedStages);
		REGISTER_METHOD(OnInitializationStage);
	}

	static void OnCompilationStarting()
	{
		monoEnv->broadcaster->OnCompilationStarting();
	}
	static void OnCompilationComplete(bool success)
	{
		monoEnv->broadcaster->OnCompilationComplete(success);
	}
	static mono::Array GetSubscribedStages()
	{
		int stagesCount;
		int *indices = monoEnv->broadcaster->GetSubscribedStagesInfo(stagesCount);
		IMonoArray *result =
			MonoEnv->CreateArray(MonoClassCache::Wrap(mono_get_int32_class()), stagesCount, false);
		for (int i = 0; i < stagesCount; i++)
		{
			result->SetItem(i, Box(indices[i]));
		}
		return (mono::Array)result->GetWrappedPointer();
	}
	static void OnInitializationStage(int stageIndex)
	{
		monoEnv->broadcaster->OnInitializationStage(stageIndex);
	}
};