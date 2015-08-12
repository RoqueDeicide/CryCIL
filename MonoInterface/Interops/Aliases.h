#pragma once

#include "IMonoInterface.h"

struct AliasesInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "PathAliases"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Files"; }

	virtual void OnRunTimeInitialized() override;

	static mono::string Get(mono::string alias, bool returnName);
	static void         Set(mono::string alias, mono::string value);
};