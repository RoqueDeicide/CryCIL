/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// CryConsole scriptbind; used for logging and CVars / CCommands
//////////////////////////////////////////////////////////////////////////
// 20/11/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __LOGGING_BINDING_H__
#define __LOGGING_BINDING_H__

#include <MonoCommon.h>
#include <IMonoInterop.h>

class ConsoleInterop : public IMonoInterop
{
public:
	ConsoleInterop();
	virtual ~ConsoleInterop() {}

protected:
	// IMonoScriptBind
	virtual const char *GetClassName() { return "ConsoleInterop"; }
	// ~IMonoScriptBind

	static void HandleException(mono::object exception);

	static void Execute(mono::string string, bool silent);

	// Console commands
	static void OnMonoCmd(IConsoleCmdArgs *);
	static void RegisterCommand(mono::string, mono::string, EVarFlags);

	// CVars
	static void RegisterCVarFloat(mono::string, float&, float, EVarFlags, mono::string);
	static void RegisterCVarInt(mono::string, int&, int, EVarFlags, mono::string);
	static void RegisterCVarString(mono::string, mono::string &, mono::string, EVarFlags, mono::string);

	static void UnregisterCVar(mono::string, bool bDelete);
	static void UnregisterCCommand(mono::string);

	static bool HasCVar(mono::string);

	static float GetCVarFloat(mono::string);
	static int GetCVarInt(mono::string);
	static mono::string GetCVarString(mono::string);

	static void SetCVarFloat(mono::string, float);
	static void SetCVarInt(mono::string, int);
	static void SetCVarString(mono::string, mono::string);

	static mono::string GetCmdArg(mono::string argName, int type);
};

#endif //__LOGGING_BINDING_H__