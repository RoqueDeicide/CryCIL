#pragma once

#include "IMonoInterface.h"
#include <ILevelSystem.h>

struct LevelSystemInterop : public IMonoInterop < false, true >, public ILevelSystemListener
{
	virtual ~LevelSystemInterop();

	virtual const char *GetName() { return "LevelSystem"; }
	virtual const char *GetNameSpace() { return "CryCil.Engine.CryAction"; }

	virtual void OnRunTimeInitialized();

	virtual void OnLevelNotFound(const char *levelName);
	virtual void OnLoadingStart(ILevelInfo *pLevel);
	virtual void OnLoadingComplete(ILevel *pLevel);
	virtual void OnLoadingError(ILevelInfo *pLevel, const char *error);
	virtual void OnLoadingProgress(ILevelInfo *pLevel, int progressAmount);
	virtual void OnUnloadComplete(ILevel* pLevel);

	static mono::object get_Current();
	static bool         get_Loaded();
	static uint64       get_LastLoadTime();

	static void         Unload();
	static mono::object Load(mono::string name);
	static void         Prepare(mono::string name);
};