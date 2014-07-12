#pragma once

#include <CryLibrary.h>

#include <IMonoScriptSystem.h>

static HMODULE InitCryMono(ISystem *pSystem, IGameFramework *pGameFramework)
{
	HMODULE hCryMonoDll = CryLoadLibrary(CRYMONO_LIBRARY);
	if (!hCryMonoDll)
	{
		CryFatalError("Could not locate CryMono library!");
		return false;
	}

	auto InitMonoFunc = (IMonoScriptSystem::TEntryFunction)CryGetProcAddress(hCryMonoDll, "InitCryMono");
	if (!InitMonoFunc)
	{
		CryFatalError("Specified CryMono library is not valid!");
		return false;
	}

	IMonoScriptSystem::g_pThis = InitMonoFunc(pSystem, pGameFramework);

	return hCryMonoDll;
}