#include "StdAfx.h"
#include "GameRules.h"

#include "MonoRunTime.h"

#include <IGameRulesSystem.h>
#include <IActorSystem.h>

GameRulesInterop::GameRulesInterop()
{
	REGISTER_METHOD(RegisterGameMode);
	REGISTER_METHOD(AddGameModeAlias);
	REGISTER_METHOD(AddGameModeLevelLocation);
	REGISTER_METHOD(SetDefaultGameMode);
}

//-----------------------------------------------------------------------------
void GameRulesInterop::RegisterGameMode(mono::string gamemode)
{
	if (IGameFramework *pGameFramework = GetMonoRunTime()->GameFramework)
	{
		if (IGameRulesSystem *pGameRulesSystem = pGameFramework->GetIGameRulesSystem())
		{
			const char *gameModeStr = ToCryString(gamemode);

			if (!pGameRulesSystem->HaveGameRules(gameModeStr))
				pGameRulesSystem->RegisterGameRules(gameModeStr, "GameRules");
		}
	}
}

//-----------------------------------------------------------------------------
void GameRulesInterop::AddGameModeAlias(mono::string gamemode, mono::string alias)
{
	GetMonoRunTime()->GameFramework->GetIGameRulesSystem()->AddGameRulesAlias(ToCryString(gamemode), ToCryString(alias));
}

//-----------------------------------------------------------------------------
void GameRulesInterop::AddGameModeLevelLocation(mono::string gamemode, mono::string location)
{
	GetMonoRunTime()->GameFramework->GetIGameRulesSystem()->AddGameRulesLevelLocation(ToCryString(gamemode), ToCryString(location));
}

//-----------------------------------------------------------------------------
void GameRulesInterop::SetDefaultGameMode(mono::string gamemode)
{
	gEnv->pConsole->GetCVar("sv_gamerulesdefault")->Set(ToCryString(gamemode));
}

//-----------------------------------------------------------------------------
EntityId GameRulesInterop::GetPlayer()
{
	return GetMonoRunTime()->GameFramework->GetClientActorId();
}