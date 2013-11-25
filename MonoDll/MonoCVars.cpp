#include "StdAfx.h"
#include "MonoCVars.h"

#include "MonoScriptSystem.h"

void CmdReload(IConsoleCmdArgs *pCmdArgs)
{
	GetMonoScriptSystem()->Reload();
}

void SCVars::InitCVars(IConsole *pConsole)
{
	EVarFlags nullOrCheatFlag = VF_NULL;
#ifdef RELEASE
	nullOrCheatFlag = VF_CHEAT;
#endif

	REGISTER_CVAR(mono_exceptionsTriggerMessageBoxes, 1, VF_NULL, "If true, exceptions will trigger a message box to appear");
	REGISTER_CVAR(mono_exceptionsTriggerFatalErrors, 0, VF_NULL, "If true, exceptions will trigger a fatal error");

	REGISTER_CVAR(mono_realtimeScripting, 1, nullOrCheatFlag, "Enables / Disables Realtime Scripting functionality. (Editor-only)");
	REGISTER_CVAR(mono_realtimeScriptingDebug, 0, nullOrCheatFlag, "Toggles on realtime scripting debug, useful for finding serialization bugs");
	REGISTER_CVAR(mono_realtimeScriptingDetectChanges, 1, nullOrCheatFlag, "Toggles whether realtime scripting should automatically reload scripts when any *.cs file is modified in the scripts directory.");

	REGISTER_CVAR(mono_softBreakpoints, 1, VF_REQUIRE_APP_RESTART, "[Performance Warning] Enables / Disables soft breakpoints, preventing managed null reference exceptions causing crashes in unmanaged code. \n Not supported along with -DEBUG command line option");

	REGISTER_CVAR(mono_generateMdbIfPdbIsPresent, 1, VF_NULL, "Toggles on mono debug database (.mdb) generation, if a pdb file is present");

	REGISTER_CVAR(mono_compileScripts, 1, nullOrCheatFlag, "Determines whether source files found in the Scripts directory will be compiled and loaded");
	REGISTER_CVAR(mono_scriptDirectory, "", nullOrCheatFlag, "If set, CryMono will attempt to load its script files (e.g. *.cs) from this directory. Full path only.");

	REGISTER_CVAR(mono_entityDeleteExtensionOnNetworkBindFailure, 1, nullOrCheatFlag, "If set, the game object extension will delete itself if IGameObject::BindToNetwork returns false in the IGameObjectExtension::Init function");

	REGISTER_CVAR(mono_log, 0, VF_CHEAT, "");

	REGISTER_COMMAND("mono_reload", CmdReload, nullOrCheatFlag, "[Realtime Scripting] Reloads / recompiles all scripts");
}

//------------------------------------------------------------------------
void SCVars::ReleaseCVars()
{
	IConsole *pConsole = gEnv->pConsole;

	pConsole->UnregisterVariable("mono_exceptionsTriggerMessageBoxes", true);
	pConsole->UnregisterVariable("mono_exceptionsTriggerFatalErrors", true);

	pConsole->UnregisterVariable("mono_realtimeScripting", true);

#ifndef RELEASE
	pConsole->UnregisterVariable("mono_realtimeScriptingDebug", true);
#endif

	pConsole->UnregisterVariable("mono_realtimeScriptingDetectChanges", true);
	pConsole->RemoveCommand("mono_reload");

	pConsole->UnregisterVariable("mono_softBreakpoints", true);

	pConsole->UnregisterVariable("mono_scriptDirectory", true);
}