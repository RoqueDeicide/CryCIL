// All functions that are exported to be used by CryEngine Game Dll.

#include "stdafx.h"

#include "RunTime/MonoInterface.h"

extern "C"	// Mark exported functions as C code, so the compiler keeps function names as they are.
{
	//! Invoked from Game dll to initialize this module.
	//!
	//! @remarks This function must be called before anything can be done with this module.
	//!
	//! @param framework     Pointer to IGameFramework object that will allow us to initialize
	//!                      everything.
	//! @param listeners     Pointer to an array of listeners to register before initialization.
	//! @param listenerCount Number of listeners in the above array.
	MONOINTERFACE_API IMonoInterface *InitializeCryCilSubsystem
	(
		IGameFramework *framework,
		IMonoSystemListener **listeners,
		int listenerCount
	)
	{
		// Initializes gEnv variable, registers some objects.
		// Fun fact: Module name is only used for Unit Tests.
		ModuleInitISystem(framework->GetISystem(), "MonoInterface");
		// Save IGameFramework pointer now.
		Framework = framework;
		// I'm not sure, if I can use "new" operator to create a new instance of MonoRunTime.
		static char buff[sizeof(MonoInterface)];
		return new (buff)MonoInterface(framework, listeners, listenerCount);
	}
}