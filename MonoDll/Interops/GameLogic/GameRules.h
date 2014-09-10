/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// GameRules scriptbind, handles GameRulesSystem tasks which shouldn't
// be in the GameDll. (As these should always be present)
//////////////////////////////////////////////////////////////////////////
// 13/01/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __SCRIPTBIND_GAMERULES_SYSTEM__
#define __SCRIPTBIND_GAMERULES_SYSTEM__

#include <MonoCommon.h>
#include <IMonoScriptBind.h>

class CScriptbind_GameRules : public IMonoScriptBind
{
public:
	CScriptbind_GameRules();
	~CScriptbind_GameRules() {}

	// IMonoScriptBind
	virtual const char *GetClassName() { return "GameRulesInterop"; }
	// ~IMonoScriptBind

	static void RegisterGameMode(mono::string);
	static void AddGameModeAlias(mono::string, mono::string);
	static void AddGameModeLevelLocation(mono::string, mono::string);
	static void SetDefaultGameMode(mono::string);
	static EntityId GetPlayer();
};

#endif //__SCRIPTBIND_GAMERULES_SYSTEM__