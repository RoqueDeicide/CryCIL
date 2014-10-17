// All functions that are exported to be used by CryEngine Game Dll.

#include "stdafx.h"

extern "C"	// Mark exported functions as C code, so the compiler keeps function names as they are.
{
	MONOINTERFACE_API void Test()
	{
		if (gEnv)
		{
			gEnv->pLog->LogAlways("Hello World!");
		}
	}
	MONOINTERFACE_API void TestText(const char* arg)
	{
		if (gEnv)
		{
			gEnv->pLog->LogAlways(arg);
		}
	}
}