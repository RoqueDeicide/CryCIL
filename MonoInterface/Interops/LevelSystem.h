#pragma once

#include "IMonoInterface.h"
#include <ILevelSystem.h>

struct LevelSystemInterop : public IMonoInterop < false, true >, public ILevelSystemListener
{
	virtual ~LevelSystemInterop();

	virtual const char *GetInteropClassName() override { return "LevelSystem"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.CryAction"; }

	IMonoClass *GetMonoClass();

	virtual void InitializeInterops() override;

	virtual void OnLevelNotFound(const char *levelName) override;
	virtual void OnLoadingStart(ILevelInfo *pLevel) override;
	virtual void OnLoadingLevelEntitiesStart(ILevelInfo* pLevel) override;
	virtual void OnLoadingComplete(ILevelInfo *pLevel) override;
	virtual void OnLoadingError(ILevelInfo *pLevel, const char *error) override;
	virtual void OnLoadingProgress(ILevelInfo *pLevel, int progressAmount) override;
	virtual void OnUnloadComplete(ILevelInfo* pLevel) override;

	static ILevelInfo *get_Current();
	static bool        get_Loaded();
	static uint64      get_LastLoadTime();

	static void        Unload();
	static ILevelInfo *LoadInternal(mono::string name);
	static void        PrepareInternal(mono::string name);
};