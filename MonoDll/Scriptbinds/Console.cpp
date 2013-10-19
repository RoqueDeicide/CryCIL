#include "StdAfx.h"
#include "Console.h"

#include <IMonoAssembly.h>
#include <MonoClass.h>
#include <MonoArray.h>
#include <MonoCommon.h>

#include "MonoScriptSystem.h"

CScriptbind_Console::CScriptbind_Console()
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

void CScriptbind_Console::HandleException(mono::object exception)
{
	CScriptObject::HandleException((MonoObject *)exception);
}

void CScriptbind_Console::Execute(mono::string string, bool silent)
{
	gEnv->pConsole->ExecuteString(ToCryString(string), silent); 
}

#undef GetCommandLine
void CScriptbind_Console::OnMonoCmd(IConsoleCmdArgs *cmdArgs)
{
	IMonoArray *pArgs = CreateMonoArray(1);
	pArgs->Insert(cmdArgs->GetCommandLine());

	g_pScriptSystem->GetCryBraryAssembly()->GetClass("ConsoleCommand")->GetMethod("OnCommand", 1)->InvokeArray(NULL, pArgs);
	pArgs->Release();
}

void CScriptbind_Console::RegisterCommand(mono::string cmd, mono::string desc, EVarFlags flags)
{
	REGISTER_COMMAND(ToCryString(cmd), OnMonoCmd, flags, ToCryString(desc));
}

void CScriptbind_Console::RegisterCVarFloat(mono::string name, float &val, float defaultVal, EVarFlags flags, mono::string description)
{
	gEnv->pConsole->Register(ToCryString(name), &val, defaultVal, flags, ToCryString(description));
}

void CScriptbind_Console::RegisterCVarInt(mono::string name, int &val, int defaultVal, EVarFlags flags, mono::string description)
{
	gEnv->pConsole->Register(ToCryString(name), &val, defaultVal, flags, ToCryString(description));
}

void CScriptbind_Console::RegisterCVarString(mono::string name, mono::string &val, mono::string defaultVal, EVarFlags flags, mono::string description)
{
	//gEnv->pConsole->Register(ToCryString(name), &val, ToCryString(defaultVal), flags, ToCryString(description));
}

void CScriptbind_Console::UnregisterCVar(mono::string name, bool bDelete)
{
	gEnv->pConsole->UnregisterVariable(ToCryString(name), bDelete); 
}

void CScriptbind_Console::UnregisterCCommand(mono::string name)
{
	gEnv->pConsole->RemoveCommand(ToCryString(name));
}

bool CScriptbind_Console::HasCVar(mono::string name)
{
	if(ICVar *pCVar = gEnv->pConsole->GetCVar(ToCryString(name)))
		return true;

	return false;
}

float CScriptbind_Console::GetCVarFloat(mono::string name)
{
	if(ICVar *pCVar = gEnv->pConsole->GetCVar(ToCryString(name)))
		return pCVar->GetFVal();

	return 0.0f;
}

int CScriptbind_Console::GetCVarInt(mono::string name)
{
	if(ICVar *pCVar = gEnv->pConsole->GetCVar(ToCryString(name)))
		return pCVar->GetIVal();

	return 0;
}

mono::string CScriptbind_Console::GetCVarString(mono::string name)
{
	if(ICVar *pCVar = gEnv->pConsole->GetCVar(ToCryString(name)))
		return (mono::string )ToMonoString(pCVar->GetString());

	return (mono::string )ToMonoString("");
}

void CScriptbind_Console::SetCVarFloat(mono::string name, float val)
{
	if(ICVar *pCVar = gEnv->pConsole->GetCVar(ToCryString(name)))
		pCVar->Set(val);
}

void CScriptbind_Console::SetCVarInt(mono::string name, int val)
{
	if(ICVar *pCVar = gEnv->pConsole->GetCVar(ToCryString(name)))
		pCVar->Set(val);
}

void CScriptbind_Console::SetCVarString(mono::string name, mono::string val)
{
	if(ICVar *pCVar = gEnv->pConsole->GetCVar(ToCryString(name)))
		pCVar->Set(ToCryString(val));
}

mono::string CScriptbind_Console::GetCmdArg(mono::string argName, int type)
{
	auto cmdline = gEnv->pSystem->GetICmdLine();
	auto arg = cmdline->FindArg((ECmdLineArgType)type, ToCryString(argName));

	return ToMonoString(arg ? arg->GetValue() : "");
}