#include "StdAfx.h"
#include "GameRules.h"

#include "MonoScriptSystem.h"

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
	if (IGameFramework *pGameFramework = static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework())
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
	static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetIGameRulesSystem()->AddGameRulesAlias(ToCryString(gamemode), ToCryString(alias));
}

//-----------------------------------------------------------------------------
void GameRulesInterop::AddGameModeLevelLocation(mono::string gamemode, mono::string location)
{
	static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetIGameRulesSystem()->AddGameRulesLevelLocation(ToCryString(gamemode), ToCryString(location));
}

//-----------------------------------------------------------------------------
void GameRulesInterop::SetDefaultGameMode(mono::string gamemode)
{
	gEnv->pConsole->GetCVar("sv_gamerulesdefault")->Set(ToCryString(gamemode));
}

//-----------------------------------------------------------------------------
EntityId GameRulesInterop::GetPlayer()
{
	return static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetClientActorId();
}