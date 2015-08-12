#pragma once

#include "IMonoInterface.h"

struct ILevelInfo;

struct LevelMission
{
	mono::string name;
	mono::string xmlPath;
	int cgfCount;
};

struct LevelMissionsInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "LevelMissions"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.CryAction"; }

	virtual void OnRunTimeInitialized() override;

	static bool Supports(ILevelInfo **info, mono::string name);
	static LevelMission GetDefault(ILevelInfo **info);
	static LevelMission GetItem(ILevelInfo **info, int index);
	static int GetCount(ILevelInfo **info);
};