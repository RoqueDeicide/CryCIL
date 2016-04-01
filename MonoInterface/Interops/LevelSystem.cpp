#include "stdafx.h"

#include "LevelSystem.h"
#include "TimeUtilities.h"

IMonoClass *LevelSystemInterop::GetMonoClass()
{
	return this->GetInteropClass(MonoEnv->Cryambly);
}

LevelSystemInterop::~LevelSystemInterop()
{
	MonoEnv->CryAction->GetILevelSystem()->RemoveListener(this);
}

void LevelSystemInterop::InitializeInterops()
{
	MonoEnv->CryAction->GetILevelSystem()->AddListener(this);

	REGISTER_METHOD(get_Current);
	REGISTER_METHOD(get_Loaded);
	REGISTER_METHOD(get_LastLoadTime);
	REGISTER_METHOD(Unload);
	REGISTER_METHOD(LoadInternal);
	REGISTER_METHOD(PrepareInternal);
}

void LevelSystemInterop::OnLevelNotFound(const char *levelName)
{
	static IMonoStaticMethod *raise = this->GetMonoClass()->GetEvent("LevelNotFound")->Raise->ToStatic();
	
	void *params[1];
	params[0] = ToMonoString(levelName);
	raise->Invoke(params);
}

void LevelSystemInterop::OnLoadingStart(ILevelInfo *pLevel)
{
	static IMonoStaticMethod *raise = this->GetMonoClass()->GetEvent("LoadingStart")->Raise->ToStatic();

	void *params[1];
	params[0] = pLevel;
	raise->Invoke(params);
}

void LevelSystemInterop::OnLoadingLevelEntitiesStart(ILevelInfo* pLevel)
{
	static IMonoStaticMethod *raise = this->GetMonoClass()->GetEvent("LoadingEntitiesStart")->Raise->ToStatic();

	void *params[1];
	params[0] = pLevel;
	raise->Invoke(params);
}

void LevelSystemInterop::OnLoadingComplete(ILevel *pLevel)
{
	static IMonoStaticMethod *raise = this->GetMonoClass()->GetEvent("LoadingComplete")->Raise->ToStatic();

	void *params[1];
	params[0] = pLevel->GetLevelInfo();
	raise->Invoke(params);
}

void LevelSystemInterop::OnLoadingError(ILevelInfo *pLevel, const char *)
{
	static IMonoStaticMethod *raise = this->GetMonoClass()->GetEvent("LoadingError")->Raise->ToStatic();

	void *params[1];
	params[0] = pLevel;
	raise->Invoke(params);
}

void LevelSystemInterop::OnLoadingProgress(ILevelInfo *pLevel, int progressAmount)
{
	static IMonoStaticMethod *raise = this->GetMonoClass()->GetEvent("LoadingProgress")->Raise->ToStatic();

	void *params[2];
	params[0] = pLevel;
	params[1] = &progressAmount;
	raise->Invoke(params);
}

void LevelSystemInterop::OnUnloadComplete(ILevel* pLevel)
{
	static IMonoStaticMethod *raise = this->GetMonoClass()->GetEvent("UnloadComplete")->Raise->ToStatic();

	void *params[1];
	params[0] = pLevel->GetLevelInfo();
	raise->Invoke(params);
}

ILevelInfo *LevelSystemInterop::get_Current()
{
	return MonoEnv->CryAction->GetILevelSystem()->GetCurrentLevel()->GetLevelInfo();
}

bool LevelSystemInterop::get_Loaded()
{
	return MonoEnv->CryAction->GetILevelSystem()->IsLevelLoaded();
}

uint64 LevelSystemInterop::get_LastLoadTime()
{
	return TimeUtilities::SecondsToMonoTicks(MonoEnv->CryAction->GetILevelSystem()->GetLastLevelLoadTime());
}

void LevelSystemInterop::Unload()
{
	MonoEnv->CryAction->GetILevelSystem()->UnLoadLevel();
}

ILevelInfo *LevelSystemInterop::LoadInternal(mono::string name)
{
	return MonoEnv->CryAction->GetILevelSystem()->LoadLevel(NtText(name))->GetLevelInfo();
}

void LevelSystemInterop::PrepareInternal(mono::string name)
{
	MonoEnv->CryAction->GetILevelSystem()->PrepareNextLevel(NtText(name));
}