#pragma once

#include "IMonoInterface.h"

struct LevelsInterop : public IMonoInterop < true, true >
{
	virtual const char *GetName() { return "Levels"; }
	virtual const char *GetNameSpace() { return "CryCil.Engine.CryAction"; }

	virtual void OnRunTimeInitialized();

	static int          get_Count(mono::object obj);
	static mono::object get_ItemInt(mono::object obj, int index);
	static mono::object get_Item(mono::object obj, mono::string name);

	static ILevelSystem     *system;
	static IMonoConstructor *levelCtor;
};