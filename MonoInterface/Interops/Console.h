#pragma once

#include "IMonoInterface.h"

typedef void(__stdcall *ExecuteCommandThunk)(mono::string, mono::exception *);

//! Interops with CryEngine Console API.
struct ConsoleInterop : public IMonoInterop<true, true>
{


	virtual const char *GetName() { return "CryConsole"; }
	virtual const char *GetNameSpace() { return "CryCil.Engine"; }

	virtual void OnRunTimeInitialized();

	static void RegisterCommandInternal(mono::string name, mono::string help, EVarFlags flags);
	static void UnregisterCommandInternal(mono::string name);
	static void ExecuteCommand(mono::string command, bool silent, bool deferExecution);

	static void MonoCommand(IConsoleCmdArgs *args);

	static ExecuteCommandThunk executeCommand;
};