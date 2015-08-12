#pragma once

#include "IMonoInterface.h"

typedef void(__stdcall *ExecuteCommandThunk)(mono::string, mono::exception *);

//! Interops with CryEngine Console API.
struct ConsoleInterop : public IMonoInterop<true, true>
{


	virtual const char *GetInteropClassName() override { return "CryConsole"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine"; }

	virtual void OnRunTimeInitialized() override;

	static void RegisterCommandInternal(mono::string name, mono::string help, EVarFlags flags);
	static void UnregisterCommandInternal(mono::string name);
	static void ExecuteCommand(mono::string command, bool silent, bool deferExecution);

	static void MonoCommand(IConsoleCmdArgs *args);

	static ExecuteCommandThunk executeCommand;

	static ICVar *RegisterVariable(mono::string name, float *field, float value, EVarFlags flags, mono::string help);
	static ICVar *RegisterVariableIntRef(mono::string name, int *field, int value, EVarFlags flags, mono::string help);

	static ICVar *RegisterVariableFloat(mono::string name, float value, EVarFlags flags,
										   ConsoleVarFunc thunk, mono::string help);
	static ICVar *RegisterVariableInt(mono::string name, int value, EVarFlags flags,
										   ConsoleVarFunc thunk, mono::string help);
	static ICVar *RegisterVariableString(mono::string name, mono::string value, EVarFlags flags,
										   ConsoleVarFunc thunk, mono::string help);

	static void UnregisterVariable(mono::string name, bool _delete);

	static ICVar *GetVariable(mono::string name);
};