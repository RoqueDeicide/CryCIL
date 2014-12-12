// All functions that are exported to be used by CryEngine Game Dll.

#include "stdafx.h"

#include "RunTime/MonoInterface.h"

extern "C"	// Mark exported functions as C code, so the compiler keeps function names as they are.
{
	//! Invoked from Game dll to initialize this module.
	//!
	//! This function must be called before anything can be done with this module.
	//!
	//! @param framework Pointer to IGameFramework object that will allow us to initialize
	//!                  everything.
	//! @param listeners Pointer to a list of listeners to register before initialization.
	//!                  Can be null if there are no listeners to register. All listeners
	//!                  must be persistent.
	MONOINTERFACE_API IMonoInterface *InitializeCryCilSubsystem
	(
		IGameFramework *framework,
		List<IMonoSystemListener *> *listeners
	)
	{
		// Initializes gEnv variable, registers some objects.
		// Fun fact: Module name is only used for Unit Tests.
		ModuleInitISystem(framework->GetISystem(), "CryCIL");

		// Use static allocation. Allows to not have to call a destructor.
		static char buff[sizeof(MonoInterface)];
		return new (buff)MonoInterface(framework, listeners);
	}
}