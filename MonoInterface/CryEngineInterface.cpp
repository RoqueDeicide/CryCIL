// All functions that are exported to be used by CryEngine Game Dll.

#include "stdafx.h"

extern "C"	// Mark exported functions as C code, so the compiler keeps function names as they are.
{
	//! Invoked from Game dll to initialize this module.
	//!
	//! @remarks This function must be called before anything can be done with this module.
	//!
	//! @param framework Pointer to IGameFramework object that will allow us to initialize everything.
	MONOINTERFACE_API IMonoInterface *InitializeModule(IGameFramework *framework, IMonoSystemListener **, int listenerCount)
	{
		// Initializes gEnv variable, registers some objects.
		// Fun fact: Module name is only used for Unit Tests.
		ModuleInitISystem(framework->GetISystem(), "MonoInterface");
	}
}