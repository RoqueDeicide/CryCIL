#include "stdafx.h"

#include "GameRules.h"
#include <IGameRulesSystem.h>
#include "MonoGameRules.h"

void GameRulesInterop::InitializeInterops()
{
	REGISTER_METHOD(RegisterGameRules);
	REGISTER_METHOD(AddGameRulesAlias);
	REGISTER_METHOD(AddGameRulesLevelLocation);
	REGISTER_METHOD(GetGameRulesLevelLocation);
	REGISTER_METHOD(GetGameRulesLevelLocations);
	REGISTER_METHOD(GetGameRulesName);
	REGISTER_METHOD(HaveGameRules);
	REGISTER_METHOD(SetCurrentGameRules);
	REGISTER_METHOD(GetCurrentGameRules);
	REGISTER_METHOD(GetCurrentGameRulesObject);
}

extern bool RegisterGameRulesHacked(const char *name, const char *typeName);

void GameRulesInterop::RegisterGameRules(mono::string name, mono::string typeName, mono::Array aliases,
										 mono::Array paths, bool _default)
{
	MonoGCHandle aliasesHandle = MonoEnv->GC->Pin(aliases);

	NtText gameRulesName(name);
	NtText gameRulesTypeName(typeName);

	List<NtText> aliasesList(10);
	List<NtText> pathsList(10);
	if (aliases)
	{
		IMonoArray<mono::string> aliasesArray(aliases);
		for (int i = 0; i < aliasesArray.Length; i++)
		{
			aliasesList.Add(aliasesArray[i]);
		}
	}
	if (paths)
	{
		IMonoArray<mono::string> pathsArray(paths);
		for (int i = 0; i < pathsArray.Length; i++)
		{
			pathsList.Add(pathsArray[i]);
		}
	}

	RegisterGameRulesHacked(gameRulesName, gameRulesTypeName);

	auto gameRulesSystem = MonoEnv->CryAction->GetIGameRulesSystem();
	for (int i = 0; i < aliasesList.Length; i++)
	{
		gameRulesSystem->AddGameRulesAlias(gameRulesName, aliasesList[i]);
	}
	for (int i = 0; i < pathsList.Length; i++)
	{
		gameRulesSystem->AddGameRulesLevelLocation(gameRulesName, pathsList[i]);
	}

	if (_default)
	{
		gEnv->pConsole->GetCVar("sv_gamerulesdefault")->Set(gameRulesName);
	}
}

void GameRulesInterop::AddGameRulesAlias(mono::string gamerules, mono::string alias)
{
	MonoEnv->CryAction->GetIGameRulesSystem()->AddGameRulesAlias(NtText(gamerules), NtText(alias));
}

void GameRulesInterop::AddGameRulesLevelLocation(mono::string gamerules, mono::string mapLocation)
{
	MonoEnv->CryAction->GetIGameRulesSystem()->AddGameRulesLevelLocation(NtText(gamerules),
																		 NtText(mapLocation));
}

mono::string GameRulesInterop::GetGameRulesLevelLocation(mono::string gamerules, int i)
{
	return ToMonoString(MonoEnv->CryAction->GetIGameRulesSystem()->GetGameRulesLevelLocation(NtText(gamerules), i));
}

mono::Array GameRulesInterop::GetGameRulesLevelLocations(mono::string gamerules)
{
	NtText gameRulesName(gamerules);
	List<const char *> aliasesList(10);
	IGameRulesSystem *gameRulesSystem = MonoEnv->CryAction->GetIGameRulesSystem();
	
	int i = 0;
	while (const char *alias = gameRulesSystem->GetGameRulesLevelLocation(gameRulesName, i++))
	{
		aliasesList.Add(alias);
	}
	
	if (aliasesList.Length == 0)
	{
		return nullptr;
	}

	mono::Array _array = MonoEnv->Objects->Arrays->Create(aliasesList.Length, MonoEnv->CoreLibrary->String);
	MonoGCHandle _arrayHandle = MonoEnv->GC->Pin(_array);
	List<MonoGCHandle> handles(aliasesList.Length);
	IMonoArray<mono::string> aliasesArray(_array);

	for (i = 0; i < aliasesList.Length; i++)
	{
		mono::string alias = ToMonoString(aliasesList[i]);
		handles.Add(MonoEnv->GC->Pin(alias));
		aliasesArray[i] = alias;
	}

	delete aliasesList.Detach(i);

	return _array;
}

mono::string GameRulesInterop::GetGameRulesName(mono::string alias)
{
	return ToMonoString(MonoEnv->CryAction->GetIGameRulesSystem()->GetGameRulesName(NtText(alias)));
}

bool GameRulesInterop::HaveGameRules(mono::string pRulesName)
{
	return MonoEnv->CryAction->GetIGameRulesSystem()->HaveGameRules(NtText(pRulesName));
}

void GameRulesInterop::SetCurrentGameRules(mono::string pGameRules)
{
	NtText gameRulesName(pGameRules);
	IGameRulesSystem *system = MonoEnv->CryAction->GetIGameRulesSystem();

	if (gameRulesName.CompareTo(system->GetCurrentGameRulesEntity()->GetClass()->GetName()) == 0)
	{
		// No need to change game rules, one we need is already active.
		return;
	}

	system->DestroyGameRules();

	system->CreateGameRules(gameRulesName);
}

mono::string GameRulesInterop::GetCurrentGameRules()
{
	return ToMonoString(MonoEnv->CryAction->GetIGameRulesSystem()->GetCurrentGameRulesEntity()->GetClass()->GetName());
}

mono::object GameRulesInterop::GetCurrentGameRulesObject()
{
	IEntity *rulesEntity = MonoEnv->CryAction->GetIGameRulesSystem()->GetCurrentGameRulesEntity();
	const char *rulesName = rulesEntity->GetClass()->GetName();

	bool isMono = false;
	for (int i = 0; i < monoGameRules.Length; i++)
	{
		if (monoGameRules[i].CompareTo(rulesName) == 0)
		{
			isMono = true;
		}
	}

	if (isMono)
	{
		IGameObject *gameObject = MonoEnv->CryAction->GetGameObject(rulesEntity->GetId());
		MonoGameRules *monoRules = static_cast<MonoGameRules *>(gameObject->GetUserData());
		return monoRules->MonoWrapper;
	}
	return nullptr;
}

List<NtText> GameRulesInterop::monoGameRules = List<NtText>(10);
