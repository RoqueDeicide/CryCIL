#pragma once

#include "IMonoInterface.h"
#include <ILevelSystem.h>

struct LevelSystemInterop : public IMonoInterop < false, true >, public ILevelSystemListener
{
	virtual ~LevelSystemInterop();

	virtual const char *GetInteropClassName() override { return "LevelSystem"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.CryAction"; }

	virtual void OnRunTimeInitialized() override;

	virtual void OnLevelNotFound(const char *levelName) override;
	virtual void OnLoadingStart(ILevelInfo *pLevel) override;
	virtual void OnLoadingLevelEntitiesStart(ILevelInfo* pLevel) override;
	virtual void OnLoadingComplete(ILevel *pLevel) override;
	virtual void OnLoadingError(ILevelInfo *pLevel, const char *error) override;
	virtual void OnLoadingProgress(ILevelInfo *pLevel, int progressAmount) override;
	virtual void OnUnloadComplete(ILevel* pLevel) override;

	static mono::object get_Current();
	static bool         get_Loaded();
	static uint64       get_LastLoadTime();

	static void         Unload();
	static mono::object Load(mono::string name);
	static void         Prepare(mono::string name);
};