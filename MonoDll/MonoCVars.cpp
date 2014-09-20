#include "StdAfx.h"
#include "MonoCVars.h"

#include "MonoScriptSystem.h"

void SCVars::InitCVars(IConsole *pConsole)
{
	EVarFlags nullOrCheatFlag = VF_NULL;
#ifdef RELEASE
	nullOrCheatFlag = VF_CHEAT;
#endif

	REGISTER_CVAR(mono_exceptionsTriggerMessageBoxes, 1, VF_NULL, "If true, exceptions will trigger a message box to appear");
	REGISTER_CVAR(mono_exceptionsTriggerFatalErrors, 0, VF_NULL, "If true, exceptions will trigger a fatal error");

	REGISTER_CVAR(mono_generateMdbIfPdbIsPresent, 1, VF_NULL, "Toggles on mono debug database (.mdb) generation, if a pdb file is present");

	REGISTER_CVAR(mono_entityDeleteExtensionOnNetworkBindFailure, 1, nullOrCheatFlag, "If set, the game object extension will delete itself if IGameObject::BindToNetwork returns false in the IGameObjectExtension::Init function");

	REGISTER_CVAR(mono_log, 0, VF_CHEAT, "");
}

//------------------------------------------------------------------------
void SCVars::ReleaseCVars()
{
	IConsole *pConsole = gEnv->pConsole;

	pConsole->UnregisterVariable("mono_exceptionsTriggerMessageBoxes", true);
	pConsole->UnregisterVariable("mono_exceptionsTriggerFatalErrors", true);
}