#pragma once

#include "IMonoInterface.h"
#include <ILevelSystem.h>

struct LevelInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() override { return "Level"; }
	virtual const char *GetNameSpace() override { return "CryCil.Engine.CryAction"; }

	virtual void OnRunTimeInitialized() override;

	static mono::string get_Name(mono::object levelObj);
	static mono::string get_DisplayName(mono::object levelObj);
	static mono::string get_Path(mono::object levelObj);
	static mono::string get_Paks(mono::object levelObj);
	static bool         get_IsFromMod(mono::object levelObj);
	static mono::string get_PreviewPath(mono::object levelObj);
	static mono::string get_BackgroundPath(mono::object levelObj);
	static mono::string get_MinimapPath(mono::object levelObj);
	static ILevelInfo::SMinimapInfo get_Minimap(mono::object levelObj);

	static bool IsOfType(mono::object levelObj, mono::string type);
};