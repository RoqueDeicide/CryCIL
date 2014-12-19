#pragma once

// Only include this header into one cpp file, or face linker complaining about symbols
// being defined in too many places.

#include <CryLibrary.h>
#include "IMonoInterface.h"
// Define the external variable that was declared in IMonoInterface.h
IMonoInterface *MonoEnv = nullptr;
//! Represents an event listener that initializes MonoEnv, when needed.
struct EarlyInitializer : IMonoSystemListener
{
	//! Sets MonoEnv and removes itself.
	virtual void SetInterface(IMonoInterface *handle)
	{
		MonoEnv = handle;
		MonoEnv->RemoveListener(this);
		delete this;
	}
	//! Not used since this listener is set to unregister itself after MonoEnv is set.
	virtual void OnPreInitialization() {}
	//! Not used since this listener is set to unregister itself after MonoEnv is set.
	virtual void OnRunTimeInitializing() {}
	//! Not used since this listener is set to unregister itself after MonoEnv is set.
	virtual void OnRunTimeInitialized() {}
	//! Not used since this listener is set to unregister itself after MonoEnv is set.
	virtual void OnCryamblyInitilizing() {}
	//! Not used since this listener is set to unregister itself after MonoEnv is set.
	virtual void OnCompilationStarting() {}
	//! Not used since this listener is set to unregister itself after MonoEnv is set.
	virtual void OnCompilationComplete(bool success) {}
	//! Not used since this listener is set to unregister itself after MonoEnv is set.
	virtual List<int> *GetSubscribedStages() {}
	//! Not used since this listener is set to unregister itself after MonoEnv is set.
	virtual void OnInitializationStage(int stageIndex) {}
	//! Not used since this listener is set to unregister itself after MonoEnv is set.
	virtual void OnCryamblyInitilized() {}
	//! Not used since this listener is set to unregister itself after MonoEnv is set.
	virtual void OnPostInitialization() {}
	//! Not used since this listener is set to unregister itself after MonoEnv is set.
	virtual void Update() {}
	//! Not used since this listener is set to unregister itself after MonoEnv is set.
	virtual void PostUpdate() {}
	//! Not used since this listener is set to unregister itself after MonoEnv is set.
	virtual void Shutdown() {}
};
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
//! @param framework        Pointer to implementation of IGameFramework.
//! @param listeners        Pointer to the array of persistent listeners to use.
//!                         Can be null, if you don't want to register any.
//! @param earlyMonoEnvInit Indicates whether MonoEnv should be set to the appropriate value
//!                         before initialization is complete. Setting this to false
//!                         necessitates setting up some extra way of accessing uninitialized
//!                         IMonoInterface implementation for interops that register internal
//!                         calls that require MonoEnv and can be invoked before it's done.
//!
//! @return A Dll handle that represents CryCil library.
HMODULE InitializeCryCIL
(
	IGameFramework *framework,
	List<IMonoSystemListener *> *listeners,
	bool earlyMonoEnvInit = true
)
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
	if (earlyMonoEnvInit)
	{
		if (!listeners)
		{
			listeners = new List<IMonoSystemListener *>(1);
		}
		listeners->Add(new EarlyInitializer());
	}
	MonoEnv = initFunc(framework, listeners);
	return monoInterfaceDll;
}