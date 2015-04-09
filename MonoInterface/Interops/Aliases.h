#pragma once

#include "IMonoInterface.h"

struct AliasesInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() { return "PathAliases"; }
	virtual const char *GetNameSpace() { return "CryCil.Engine.Files"; }

	virtual void OnRunTimeInitialized();

	static mono::string Get(mono::string alias, bool returnName);
	static void         Set(mono::string alias, mono::string value);
};