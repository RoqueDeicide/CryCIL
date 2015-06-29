#pragma once

#include "IMonoInterface.h"

struct CryPakInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() override { return "CryPacks"; }
	virtual const char *GetNameSpace() override { return "CryCil.Engine.Files"; }

	virtual void OnRunTimeInitialized() override;

	static mono::string OpenPack       (mono::string name, ICryPak::EPathResolutionRules rules);
	static mono::string OpenPackRooted (mono::string root, mono::string name, ICryPak::EPathResolutionRules rules);
	
	static bool         OpenPacks      (mono::string wildcard, mono::Array *paths, ICryPak::EPathResolutionRules rules);
	static bool         OpenPacksRooted(mono::string root, mono::string wildcard, mono::Array *paths, ICryPak::EPathResolutionRules rules);
	
	static bool         ClosePack      (mono::string name, bool closeMany, ICryPak::EPathResolutionRules rules);

	static bool         Exist          (mono::string wildcard);

	static bool SetPacksAccessible(bool bAccessible, mono::string pWildcard);
	static bool SetPackAccessible (bool bAccessible, mono::string pName);
};