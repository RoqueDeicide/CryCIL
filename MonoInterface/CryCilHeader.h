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
//! It is best to invoke MonoEnv->Shutdown() from ~CGameStartup.
//!
//! Attempting to free library represented by the returned handle is likely to cause BEX.
//!
//! @param framework Pointer to implementation of IGameFramework.
//! @param listeners Pointer to the array of persistent listeners to use.
//!                  Can be null, if you don't want to register any.
//!
//! @return A Dll handle that represents CryCil library.
HMODULE InitializeCryCIL(IGameFramework *framework, List<IMonoSystemListener *> *listeners)
{
	HMODULE monoInterfaceDll = CryLoadLibrary(MONOINTERFACE_LIBRARY);
	if (!monoInterfaceDll)
	{
		CryFatalError("Could not locate %s.", MONOINTERFACE_LIBRARY);
	}
	CryLogAlways("Loaded CryCIL interface library.");
	InitializeMonoInterface initFunc =
		(InitializeMonoInterface)CryGetProcAddress(monoInterfaceDll, MONO_INTERFACE_INIT);
	if (!initFunc)
	{
		CryFatalError("Could not locate %s function within %s.", MONO_INTERFACE_INIT, MONOINTERFACE_LIBRARY);
	}
	CryLogAlways("Acquired a pointer to initializer function.");
	MonoEnv = initFunc(framework, listeners);
	return monoInterfaceDll;
}