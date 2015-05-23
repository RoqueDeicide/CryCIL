#include "stdafx.h"

#include "LevelSystem.h"
#include "TimeUtilities.h"

LevelSystemInterop::~LevelSystemInterop()
{
	MonoEnv->CryAction->GetILevelSystem()->RemoveListener(this);
}

void LevelSystemInterop::OnRunTimeInitialized()
{
	MonoEnv->CryAction->GetILevelSystem()->AddListener(this);
	MonoEnv->RemoveListener(this);

	REGISTER_METHOD(get_Current);
	REGISTER_METHOD(get_Loaded);
	REGISTER_METHOD(get_LastLoadTime);
	REGISTER_METHOD(Unload);
	REGISTER_METHOD(Load);
	REGISTER_METHOD(Prepare);
}

void LevelSystemInterop::OnLevelNotFound(const char *levelName)
{
	static IMonoStaticMethod *raise =
		MonoEnv->Cryambly->GetClass(this->GetNameSpace(), this->GetName())->GetEvent("LevelNotFound")->Raise->ToStatic();
	
	void *params[1];
	params[0] = ToMonoString(levelName);
	raise->Invoke(params);
}

void LevelSystemInterop::OnLoadingStart(ILevelInfo *pLevel)
{
	static IMonoStaticMethod *raise =
		MonoEnv->Cryambly->GetClass(this->GetNameSpace(), this->GetName())->GetEvent("LoadingStart")->Raise->ToStatic();

	void *params[1];
	params[0] = pLevel;
	raise->Invoke(params);
}

void LevelSystemInterop::OnLoadingLevelEntitiesStart(ILevelInfo* pLevel)
{
	static IMonoStaticMethod *raise =
		MonoEnv->Cryambly->GetClass(this->GetNameSpace(), this->GetName())->GetEvent("LoadingEntitiesStart")->Raise->ToStatic();

	void *params[1];
	params[0] = pLevel;
	raise->Invoke(params);
}

void LevelSystemInterop::OnLoadingComplete(ILevel *pLevel)
{
	static IMonoStaticMethod *raise =
		MonoEnv->Cryambly->GetClass(this->GetNameSpace(), this->GetName())->GetEvent("LoadingComplete")->Raise->ToStatic();

	void *params[1];
	params[0] = pLevel;
	raise->Invoke(params);
}

void LevelSystemInterop::OnLoadingError(ILevelInfo *pLevel, const char *error)
{
	static IMonoStaticMethod *raise =
		MonoEnv->Cryambly->GetClass(this->GetNameSpace(), this->GetName())->GetEvent("LoadingError")->Raise->ToStatic();

	void *params[1];
	params[0] = pLevel;
	raise->Invoke(params);
}

void LevelSystemInterop::OnLoadingProgress(ILevelInfo *pLevel, int progressAmount)
{
	static IMonoStaticMethod *raise =
		MonoEnv->Cryambly->GetClass(this->GetNameSpace(), this->GetName())->GetEvent("LoadingProgress")->Raise->ToStatic();

	void *params[2];
	params[0] = pLevel;
	params[1] = &progressAmount;
	raise->Invoke(params);
}

void LevelSystemInterop::OnUnloadComplete(ILevel* pLevel)
{
	static IMonoStaticMethod *raise =
		MonoEnv->Cryambly->GetClass(this->GetNameSpace(), this->GetName())->GetEvent("UnloadComplete")->Raise->ToStatic();

	void *params[1];
	params[0] = pLevel;
	raise->Invoke(params);
}

mono::object LevelSystemInterop::get_Current()
{
	static IMonoConstructor *ctor =
		MonoEnv->Cryambly->GetClass("CryCil.Engine.CryAction", "Level")->GetConstructor(-1);

	ILevelInfo *info = MonoEnv->CryAction->GetILevelSystem()->GetCurrentLevel()->GetLevelInfo();
	void *param = &info;
	return ctor->Create(&param);
}

bool LevelSystemInterop::get_Loaded()
{
	return MonoEnv->CryAction->GetILevelSystem()->IsLevelLoaded();
}

uint64 LevelSystemInterop::get_LastLoadTime()
{
	return TimeUtilities::TicksToMonoSeconds(MonoEnv->CryAction->GetILevelSystem()->GetLastLevelLoadTime());
}

void LevelSystemInterop::Unload()
{
	MonoEnv->CryAction->GetILevelSystem()->UnLoadLevel();
}

mono::object LevelSystemInterop::Load(mono::string name)
{
	static IMonoConstructor *ctor =
		MonoEnv->Cryambly->GetClass("CryCil.Engine.CryAction", "Level")->GetConstructor(-1);

	if (!name)
	{
		ArgumentNullException("Name of the level to load cannot be null.").Throw();
	}

	ILevelInfo *info = MonoEnv->CryAction->GetILevelSystem()->LoadLevel(NtText(name))->GetLevelInfo();
	void *param = &info;
	return ctor->Create(&param);
}

void LevelSystemInterop::Prepare(mono::string name)
{
	if (!name)
	{
		ArgumentNullException("Name of the level to prepare cannot be null.").Throw();
	}

	MonoEnv->CryAction->GetILevelSystem()->PrepareNextLevel(NtText(name));
}