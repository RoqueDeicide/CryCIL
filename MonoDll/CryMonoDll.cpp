#include "stdafx.h"

#include "MonoScriptSystem.h"

#ifdef PLUGIN_SDK
#include <IPluginManager.h>
#include "CPluginCryMono.h"

PluginManager::IPluginManager* gPluginManager = NULL; //!< pointer to plugin manager

extern "C"
{
	CRYMONO_API PluginManager::IPluginBase *GetPluginInterface(const char *sInterfaceVersion)
	{
		// This function should not create a new interface class each call.
		static CryMonoPlugin::CPluginCryMono modulePlugin;
		CryMonoPlugin::gPlugin = &modulePlugin;
		return static_cast<PluginManager::IPluginBase *>(CryMonoPlugin::gPlugin);
	}
}
#else
#include <Windows.h>

extern "C"
{
	CRYMONO_API IMonoScriptSystem *InitCryMono(ISystem *pSystem, IGameFramework *pGameFramework)
	{
		ModuleInitISystem(pSystem, "CryMono");

		return new CScriptSystem(pGameFramework);
	}
}
#endif

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		break;

	case DLL_THREAD_ATTACH:
		break;

	case DLL_THREAD_DETACH:
		break;

	case DLL_PROCESS_DETACH:
		break;
	}

	return TRUE;
}