#include "stdafx.h"

#include "MonoRunTime.h"
#include "MonoRunTime.h"

#ifdef PLUGIN_SDK
#include <IPluginManager.h>
#include "CPluginCryMono.h"

PluginManager::IPluginManager* gPluginManager = NULL; //!< pointer to plugin manager

extern "C"
{
	// For PluginSDK.
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
	// Must be called from GameStartup.cpp to initialize CryMono run-time.
	CRYMONO_API IMonoRunTime *InitCryMono(ISystem *pSystem, IGameFramework *pGameFramework)
	{
		// Tell System, that we have another subsystem here.
		ModuleInitISystem(pSystem, "CryMono");
		// Initialize that subsystem.
		return new MonoRunTime(pGameFramework);
	}
}
#endif
// DLL entry point, does not do anything, just says: "we are good to go".
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