#pragma once

#include "IMonoInterface.h"

struct ILevelInfo;

struct LevelsInterop : public IMonoInterop < true, true >
{
	virtual const char *GetInteropClassName() override { return "Levels"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.CryAction"; }

	virtual void OnRunTimeInitialized() override;

	static int         GetCount();
	static ILevelInfo *GetItemInt(int index, bool &outOfRange);
	static ILevelInfo *GetItem(mono::string name);
};