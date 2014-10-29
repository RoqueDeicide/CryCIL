#pragma once

// Only include this header into one cpp file, or face linker complaining about symbols
// being defined in too many places.

#include <CryLibrary.h>
#include "IMonoInterface.h"
// Define the external variable that was declared in IMonoInterface.h
IMonoInterface *MonoEnv = nullptr;
//! Loads up and initializes CryCIL.
//!
//! Invoke this function before using CryCil and after CryAction.dll is loaded
//! and its subsystem is initialized.
//!
//! Preferable place of invocation is CGameStartup::Init function.
//! It is best to release MonoEnv and returned handle from ~CGameStartup.
//!
//! @param framework     Pointer to implementation of IGameFramework.
//! @param listeners     Pointer to the array of persistent listeners to use.
//!                      Can be null, if you don't want to register any.
//! @param listenerCount Number of listeners in the array.
//!
//! @return A Dll handle that will allow you to release CryCil library.
HMODULE InitializeCryCIL
(
	IGameFramework *framework,
	IMonoSystemListener **listeners,
	int listenerCount
)
{
	HMODULE monoInterfaceDll = CryLoadLibrary(MONOINTERFACE_LIBRARY);
	if (!monoInterfaceDll)
	{
		CryFatalError("Could not locate %s.", MONOINTERFACE_LIBRARY);
	}
	InitializeMonoInterface initFunc =
		(InitializeMonoInterface)CryGetProcAddress(monoInterfaceDll, MONO_INTERFACE_INIT);
	if (!initFunc)
	{
		CryFatalError("Could not locate %s function within %s.", MONO_INTERFACE_INIT, MONOINTERFACE_LIBRARY);
	}
	MonoEnv = initFunc(framework, listeners, listenerCount);
	return monoInterfaceDll;
}