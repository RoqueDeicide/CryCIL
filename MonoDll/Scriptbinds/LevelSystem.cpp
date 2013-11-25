#include "StdAfx.h"
#include "LevelSystem.h"

#include "MonoScriptSystem.h"

#include <ILevelSystem.h>
#include <IGameFramework.h>

CScriptbind_LevelSystem::CScriptbind_LevelSystem()
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

ILevelInfo *CScriptbind_LevelSystem::GetCurrentLevel()
{
	if(ILevel *pLevel = static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetILevelSystem()->GetCurrentLevel())
		return pLevel->GetLevelInfo();

	return nullptr;
}

ILevelInfo *CScriptbind_LevelSystem::LoadLevel(mono::string name)
{
	if(ILevel *pLevel = static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetILevelSystem()->LoadLevel(ToCryString(name)))
		return pLevel->GetLevelInfo();

	return nullptr;
}

bool CScriptbind_LevelSystem::IsLevelLoaded()
{
	return static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetILevelSystem()->IsLevelLoaded();
}

void CScriptbind_LevelSystem::UnloadLevel()
{
	return static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetILevelSystem()->UnLoadLevel();
}

mono::string CScriptbind_LevelSystem::GetName(ILevelInfo *pLevelInfo)
{
	return ToMonoString(pLevelInfo->GetName());
}

mono::string CScriptbind_LevelSystem::GetPath(ILevelInfo *pLevelInfo)
{
	return ToMonoString(pLevelInfo->GetPath());
}

mono::string CScriptbind_LevelSystem::GetPaks(ILevelInfo *pLevelInfo)
{
	return ToMonoString(pLevelInfo->GetPaks());
}

mono::string CScriptbind_LevelSystem::GetDisplayName(ILevelInfo *pLevelInfo)
{
	return ToMonoString(pLevelInfo->GetDisplayName());
}

int CScriptbind_LevelSystem::GetHeightmapSize(ILevelInfo *pLevelInfo)
{
	return pLevelInfo->GetHeightmapSize();
}
	
int CScriptbind_LevelSystem::GetGameTypeCount(ILevelInfo *pLevelInfo)
{
	return pLevelInfo->GetGameTypeCount();
}

mono::string CScriptbind_LevelSystem::GetGameType(ILevelInfo *pLevelInfo, int gameType)
{
	if(auto pGameType = pLevelInfo->GetGameType(gameType))
		return ToMonoString(pGameType->name);

	return ToMonoString("");
}

bool CScriptbind_LevelSystem::SupportsGameType(ILevelInfo *pLevelInfo, mono::string gameTypeName)
{
	return pLevelInfo->SupportsGameType(ToCryString(gameTypeName));
}

mono::string CScriptbind_LevelSystem::GetDefaultGameType(ILevelInfo *pLevelInfo)
{
	if(auto pGameType = pLevelInfo->GetDefaultGameType())
		return ToMonoString(pGameType->name);

	return ToMonoString("");
}

bool CScriptbind_LevelSystem::HasGameRules(ILevelInfo *pLevelInfo)
{
	return pLevelInfo->HasGameRules();
}