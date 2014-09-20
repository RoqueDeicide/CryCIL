#include "StdAfx.h"
#include "Console.h"

#include <IMonoAssembly.h>
#include <MonoClass.h>
#include <MonoArray.h>
#include <MonoCommon.h>

#include "MonoRunTime.h"

ConsoleInterop::ConsoleInterop()
{
	REGISTER_METHOD(HandleException);

	// Console commands
	REGISTER_METHOD(RegisterCommand);

	// CVars
	REGISTER_METHOD(RegisterCVarFloat);
	REGISTER_METHOD(RegisterCVarInt);
	REGISTER_METHOD(RegisterCVarString);

	REGISTER_METHOD(UnregisterCVar);
	REGISTER_METHOD(UnregisterCCommand);

	REGISTER_METHOD(HasCVar);

	REGISTER_METHOD(GetCVarFloat);
	REGISTER_METHOD(GetCVarInt);
	REGISTER_METHOD(GetCVarString);

	REGISTER_METHOD(SetCVarFloat);
	REGISTER_METHOD(SetCVarInt);
	REGISTER_METHOD(SetCVarString);

	REGISTER_METHOD(Execute);

	REGISTER_METHOD(GetCmdArg);
}

void ConsoleInterop::HandleException(IMonoObject *exception)
{
	CScriptObject::HandleException((MonoObject *)exception);
}

void ConsoleInterop::Execute(mono::string string, bool silent)
{
	gEnv->pConsole->ExecuteString(ToCryString(string), silent);
}

#undef GetCommandLine
void ConsoleInterop::OnMonoCmd(IConsoleCmdArgs *cmdArgs)
{
	IMonoArray *pArgs = CreateMonoArray(1);
	pArgs->Insert(cmdArgs->GetCommandLine());

	GetMonoRunTime()->CryBrary->GetClass("ConsoleCommand")->GetMethod("OnCommand", 1)->InvokeArray(NULL, pArgs);
	pArgs->Release();
}

void ConsoleInterop::RegisterCommand(mono::string cmd, mono::string desc, EVarFlags flags)
{
	REGISTER_COMMAND(ToCryString(cmd), OnMonoCmd, flags, ToCryString(desc));
}

void ConsoleInterop::RegisterCVarFloat(mono::string name, float &val, float defaultVal, EVarFlags flags, mono::string description)
{
	gEnv->pConsole->Register(ToCryString(name), &val, defaultVal, flags, ToCryString(description));
}

void ConsoleInterop::RegisterCVarInt(mono::string name, int &val, int defaultVal, EVarFlags flags, mono::string description)
{
	gEnv->pConsole->Register(ToCryString(name), &val, defaultVal, flags, ToCryString(description));
}

void ConsoleInterop::RegisterCVarString(mono::string name, mono::string &val, mono::string defaultVal, EVarFlags flags, mono::string description)
{
	//gEnv->pConsole->Register(ToCryString(name), &val, ToCryString(defaultVal), flags, ToCryString(description));
}

void ConsoleInterop::UnregisterCVar(mono::string name, bool bDelete)
{
	gEnv->pConsole->UnregisterVariable(ToCryString(name), bDelete);
}

void ConsoleInterop::UnregisterCCommand(mono::string name)
{
	gEnv->pConsole->RemoveCommand(ToCryString(name));
}

bool ConsoleInterop::HasCVar(mono::string name)
{
	if (ICVar *pCVar = gEnv->pConsole->GetCVar(ToCryString(name)))
		return true;

	return false;
}

float ConsoleInterop::GetCVarFloat(mono::string name)
{
	if (ICVar *pCVar = gEnv->pConsole->GetCVar(ToCryString(name)))
		return pCVar->GetFVal();

	return 0.0f;
}

int ConsoleInterop::GetCVarInt(mono::string name)
{
	if (ICVar *pCVar = gEnv->pConsole->GetCVar(ToCryString(name)))
		return pCVar->GetIVal();

	return 0;
}

mono::string ConsoleInterop::GetCVarString(mono::string name)
{
	if (ICVar *pCVar = gEnv->pConsole->GetCVar(ToCryString(name)))
		return (mono::string)ToMonoString(pCVar->GetString());

	return (mono::string)ToMonoString("");
}

void ConsoleInterop::SetCVarFloat(mono::string name, float val)
{
	if (ICVar *pCVar = gEnv->pConsole->GetCVar(ToCryString(name)))
		pCVar->Set(val);
}

void ConsoleInterop::SetCVarInt(mono::string name, int val)
{
	if (ICVar *pCVar = gEnv->pConsole->GetCVar(ToCryString(name)))
		pCVar->Set(val);
}

void ConsoleInterop::SetCVarString(mono::string name, mono::string val)
{
	if (ICVar *pCVar = gEnv->pConsole->GetCVar(ToCryString(name)))
		pCVar->Set(ToCryString(val));
}

mono::string ConsoleInterop::GetCmdArg(mono::string argName, int type)
{
	auto cmdline = gEnv->pSystem->GetICmdLine();
	auto arg = cmdline->FindArg((ECmdLineArgType)type, ToCryString(argName));

	return ToMonoString(arg ? arg->GetValue() : "");
}