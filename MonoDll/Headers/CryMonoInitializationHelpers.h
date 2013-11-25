#pragma once

#include <CryLibrary.h>

#include <IMonoScriptSystem.h>

static HMODULE InitCryMono(ISystem *pSystem, IGameFramework *pGameFramework)
{
	HMODULE hCryMonoDll = CryLoadLibrary("CryMono.dll");
	if (!hCryMonoDll)
	{
		CryFatalError("Could not locate CryMono DLL!");
		return false;
	}

	IMonoScriptSystem::TEntryFunction InitMonoFunc = (IMonoScriptSystem::TEntryFunction)CryGetProcAddress(hCryMonoDll, "InitCryMono");
	if (!InitMonoFunc)
	{
		CryFatalError("Specified CryMono DLL is not valid!");
		return false;
	}

	IMonoScriptSystem::g_pThis = InitMonoFunc(pSystem, pGameFramework);

	return hCryMonoDll;
}