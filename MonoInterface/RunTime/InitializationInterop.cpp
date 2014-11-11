#include "stdafx.h"
#include "InitializationInterop.h"
#include "RunTime/MonoInterface.h"

MonoInterface *InitializationInterop::monoEnv = nullptr;

const char *InitializationInterop::GetName()
{
	return "Initialization";
}

void InitializationInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(OnCompilationStartingBind);
	REGISTER_METHOD(OnCompilationCompleteBind);
	REGISTER_METHOD(GetSubscribedStagesBind);
	REGISTER_METHOD(OnInitializationStageBind);
}

void InitializationInterop::OnCompilationStartingBind()
{
	monoEnv->broadcaster->OnCompilationStarting();
}
void InitializationInterop::OnCompilationCompleteBind(bool success)
{
	monoEnv->broadcaster->OnCompilationComplete(success);
}
mono::Array InitializationInterop::GetSubscribedStagesBind()
{
	int stagesCount;
	int *indices = monoEnv->broadcaster->GetSubscribedStagesInfo(stagesCount);
	IMonoClass *SystemInt32 = MonoClassCache::Wrap(mono_get_int32_class());
	IMonoArray *result = MonoEnv->CreateArray(SystemInt32, stagesCount, false);
	for (int i = 0; i < stagesCount; i++)
	{
		mono::int32 boxedIndex = Box(indices[i]);
		result->SetItem(i, boxedIndex);
	}
	return (mono::Array)result->GetWrappedPointer();
}
void InitializationInterop::OnInitializationStageBind(int stageIndex)
{
	monoEnv->broadcaster->OnInitializationStage(stageIndex);
}

void InitializationInterop::SetInterface(IMonoInterface *handle)
{
	this->monoInterface = handle;
	monoEnv = (MonoInterface *)this->monoInterface;
}
