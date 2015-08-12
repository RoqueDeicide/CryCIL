#pragma once

#include "IMonoInterface.h"

struct ILevelInfo;

struct LevelGameRulesInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "LevelGameRules"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.CryAction"; }
	
	virtual void OnRunTimeInitialized() override;

	static mono::string GetDefault(ILevelInfo **info);
	static mono::string GetItem(ILevelInfo **info, int index);
	static int GetCount(ILevelInfo **info);
};