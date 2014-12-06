#include "StdAfx.h"
#include "LevelSystem.h"

#include "MonoRunTime.h"

#include <ILevelSystem.h>
#include <IGameFramework.h>

LevelSystemInterop::LevelSystemInterop()
{
	REGISTER_METHOD(GetCurrentLevel);
	REGISTER_METHOD(LoadLevel);
	REGISTER_METHOD(IsLevelLoaded);
	REGISTER_METHOD(UnloadLevel);

	REGISTER_METHOD(GetName);
	REGISTER_METHOD(GetPath);
	REGISTER_METHOD(GetPaks);
	REGISTER_METHOD(GetDisplayName);

	REGISTER_METHOD(GetHeightmapSize);

	REGISTER_METHOD(GetGameTypeCount);
	REGISTER_METHOD(GetGameType);
	REGISTER_METHOD(SupportsGameType);
	REGISTER_METHOD(GetDefaultGameType);

	REGISTER_METHOD(HasGameRules);
}

ILevelInfo *LevelSystemInterop::GetCurrentLevel()
{
	if (ILevel *pLevel = GetMonoRunTime()->GameFramework->GetILevelSystem()->GetCurrentLevel())
		return pLevel->GetLevelInfo();

	return nullptr;
}

ILevelInfo *LevelSystemInterop::LoadLevel(mono::string name)
{
	if (ILevel *pLevel = GetMonoRunTime()->GameFramework->GetILevelSystem()->LoadLevel(ToCryString(name)))
		return pLevel->GetLevelInfo();

	return nullptr;
}

bool LevelSystemInterop::IsLevelLoaded()
{
	return GetMonoRunTime()->GameFramework->GetILevelSystem()->IsLevelLoaded();
}

void LevelSystemInterop::UnloadLevel()
{
	return GetMonoRunTime()->GameFramework->GetILevelSystem()->UnLoadLevel();
}

mono::string LevelSystemInterop::GetName(ILevelInfo *pLevelInfo)
{
	return ToMonoString(pLevelInfo->GetName());
}

mono::string LevelSystemInterop::GetPath(ILevelInfo *pLevelInfo)
{
	return ToMonoString(pLevelInfo->GetPath());
}

mono::string LevelSystemInterop::GetPaks(ILevelInfo *pLevelInfo)
{
	return ToMonoString(pLevelInfo->GetPaks());
}

mono::string LevelSystemInterop::GetDisplayName(ILevelInfo *pLevelInfo)
{
	return ToMonoString(pLevelInfo->GetDisplayName());
}

int LevelSystemInterop::GetHeightmapSize(ILevelInfo *pLevelInfo)
{
	return pLevelInfo->GetHeightmapSize();
}

int LevelSystemInterop::GetGameTypeCount(ILevelInfo *pLevelInfo)
{
	return pLevelInfo->GetGameTypeCount();
}

mono::string LevelSystemInterop::GetGameType(ILevelInfo *pLevelInfo, int gameType)
{
	if (auto pGameType = pLevelInfo->GetGameType(gameType))
		return ToMonoString(pGameType->name);

	return ToMonoString("");
}

bool LevelSystemInterop::SupportsGameType(ILevelInfo *pLevelInfo, mono::string gameTypeName)
{
	return pLevelInfo->SupportsGameType(ToCryString(gameTypeName));
}

mono::string LevelSystemInterop::GetDefaultGameType(ILevelInfo *pLevelInfo)
{
	if (auto pGameType = pLevelInfo->GetDefaultGameType())
		return ToMonoString(pGameType->name);

	return ToMonoString("");
}

bool LevelSystemInterop::HasGameRules(ILevelInfo *pLevelInfo)
{
	return pLevelInfo->HasGameRules();
}