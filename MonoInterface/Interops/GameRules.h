#pragma once

#include "IMonoInterface.h"

struct GameRulesInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "GameRules"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Logic"; }

	virtual void OnRunTimeInitialized() override;

	static void         RegisterGameRules(mono::string name, mono::string typeName, mono::Array aliases,
										  mono::Array paths, bool _default);
	static void         AddGameRulesAlias(mono::string gamerules, mono::string alias);
	static void         AddGameRulesLevelLocation(mono::string gamerules, mono::string mapLocation);
	static mono::string GetGameRulesLevelLocation(mono::string gamerules, int i);
	static mono::Array  GetGameRulesLevelLocations(mono::string gamerules);
	static mono::string GetGameRulesName(mono::string alias);
	static bool         HaveGameRules(mono::string pRulesName);
	static void         SetCurrentGameRules(mono::string pGameRules);
	static mono::string GetCurrentGameRules();
	static mono::object GetCurrentGameRulesObject();

	static List<NtText> monoGameRules;
};