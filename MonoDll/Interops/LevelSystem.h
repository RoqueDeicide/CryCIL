/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// Level System scriptbind for loading levels etc.
//////////////////////////////////////////////////////////////////////////
// 16/05/2012 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/

#include <IMonoScriptBind.h>

#include "MonoCommon.h"

struct IMonoScript;

struct ILevelInfo;

class CScriptbind_LevelSystem
	: public IMonoScriptBind
{
public:
	CScriptbind_LevelSystem();
	~CScriptbind_LevelSystem() {}

	// IMonoScriptBind
	virtual const char *GetClassName() { return "LevelInterop"; }
	// ~IMonoScriptBind

private:

	// Skip passing ILevel, since it only contains one relevant method. (GetLevelInfo)
	static ILevelInfo *GetCurrentLevel();
	static ILevelInfo *LoadLevel(mono::string name);

	static bool IsLevelLoaded();
	static void UnloadLevel();

	static mono::string GetName(ILevelInfo *pLevelInfo);
	static mono::string GetPath(ILevelInfo *pLevelInfo);
	static mono::string GetPaks(ILevelInfo *pLevelInfo);
	static mono::string GetDisplayName(ILevelInfo *pLevelInfo);

	static int GetHeightmapSize(ILevelInfo *pLevelInfo);

	static int GetGameTypeCount(ILevelInfo *pLevelInfo);
	static mono::string GetGameType(ILevelInfo *pLevelInfo, int gameType);
	static bool SupportsGameType(ILevelInfo *pLevelInfo, mono::string gameTypeName);
	static mono::string GetDefaultGameType(ILevelInfo *pLevelInfo);

	static bool HasGameRules(ILevelInfo *pLevelInfo);
};
