// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
//! Dll entry point function.
//!
//! @param hModule            Pointer to this dll in virtual memory space.
//! @param ul_reason_for_call Identifier of the reason this function was invoked.
//! @param lpReserved         Extra markers for cases when dll is (de/a)ttached to/from the process.
BOOL APIENTRY DllMain(HMODULE hModule,
					   DWORD  ul_reason_for_call,
					   LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}