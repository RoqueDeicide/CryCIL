// All functions that are exported to be used by CryEngine Game Dll.

#include "stdafx.h"

extern "C"	// Mark exported functions as C code, so the compiler keeps function names as they are.
{
	//! Invoked from Game dll to initialize this module.
	//!
	//! @remarks This function must be called before anything can be done with this module.
	//!
	//! @param system Pointer to ISystem object that will provide us with pointer to gEnv.
	MONOINTERFACE_API void InitializeModule(ISystem *system)
	{
		// Initializes gEnv variable, registers some objects.
		// Fun fact: Module name is only used for Unit Tests.
		ModuleInitISystem(system, "MonoInterface");
	}
}