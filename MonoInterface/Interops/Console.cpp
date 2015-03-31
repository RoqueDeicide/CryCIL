#include "stdafx.h"

#include "Console.h"

#undef GetCommandLine

void ConsoleInterop::OnRunTimeInitialized()
{
	auto klass = MonoEnv->Cryambly->GetClass(this->GetNameSpace(), this->GetName());
	executeCommand = (ExecuteCommandThunk)klass->GetFunction("ExecuteMonoCommand", -1)->UnmanagedThunk;

	REGISTER_METHOD(RegisterCommandInternal);
	REGISTER_METHOD(UnregisterCommandInternal);
	MonoEnv->Functions->AddInternalCall
		(this->GetNameSpace(), this->GetName(), "ExecuteCommand(string,bool,bool)", ExecuteCommand);

	MonoEnv->Functions->AddInternalCall
		(this->GetNameSpace(), this->GetName(),
		"RegisterVariable(string,float&,float,CryCil.Engine.ConsoleFlags,string)", RegisterVariable);
	MonoEnv->Functions->AddInternalCall
		(this->GetNameSpace(), this->GetName(),
		"RegisterVariable(string,int&,int,CryCil.Engine.ConsoleFlags,string)", RegisterVariableIntRef);

	MonoEnv->Functions->AddInternalCall
		(this->GetNameSpace(), this->GetName(),
		"RegisterVariableInternal(string,float,CryCil.Engine.ConsoleFlags,intptr,string)",
		RegisterVariableFloat);
	MonoEnv->Functions->AddInternalCall
		(this->GetNameSpace(), this->GetName(),
		"RegisterVariableInternal(string,int,CryCil.Engine.ConsoleFlags,intptr,string)",
		RegisterVariableInt);
	MonoEnv->Functions->AddInternalCall
		(this->GetNameSpace(), this->GetName(),
		"RegisterVariableInternal(string,string,CryCil.Engine.ConsoleFlags,intptr,string)",
		RegisterVariableString);

	REGISTER_METHOD(GetVariable);
}

void ConsoleInterop::MonoCommand(IConsoleCmdArgs *args)
{
	mono::exception ex;
	executeCommand(ToMonoString(args->GetCommandLine()), &ex);
}

void ConsoleInterop::RegisterCommandInternal(mono::string name, mono::string help, EVarFlags flags)
{
	if (gEnv && gEnv->pConsole && name)
	{
		gEnv->pConsole->AddCommand(ToNativeString(name), MonoCommand, flags, help ? ToNativeString(help) : nullptr);
	}
}

void ConsoleInterop::UnregisterCommandInternal(mono::string name)
{
	if (gEnv && gEnv->pConsole)
	{
		gEnv->pConsole->RemoveCommand(NtText(ToNativeString(name)));
	}
}

void ConsoleInterop::ExecuteCommand(mono::string command, bool silent, bool deferExecution)
{
	if (gEnv && gEnv->pConsole)
	{
		gEnv->pConsole->ExecuteString(NtText(ToNativeString(command)), silent, deferExecution);
	}
}

ExecuteCommandThunk ConsoleInterop::executeCommand;

#define GetCommandLine GetCommandLineA

ICVar *ConsoleInterop::RegisterVariableFloat(mono::string name, float value, EVarFlags flags, ConsoleVarFunc thunk, mono::string help)
{
	if (gEnv && gEnv->pConsole && name)
	{
		return gEnv->pConsole->RegisterFloat
			(ToNativeString(name), value, flags, help ? ToNativeString(help) : nullptr, thunk);
	}
	else
	{
		return nullptr;
	}
}

ICVar *ConsoleInterop::RegisterVariableInt(mono::string name, int value, EVarFlags flags, ConsoleVarFunc thunk, mono::string help)
{
	if (gEnv && gEnv->pConsole && name)
	{
		return gEnv->pConsole->RegisterInt
			(ToNativeString(name), value, flags, help ? ToNativeString(help) : nullptr, thunk);
	}
	else
	{
		return nullptr;
	}
}

ICVar *ConsoleInterop::RegisterVariableString(mono::string name, mono::string value, EVarFlags flags, ConsoleVarFunc thunk, mono::string help)
{
	if (gEnv && gEnv->pConsole && name)
	{
		const char *val = value ? ToNativeString(value) : nullptr;
		const char *h = help ? ToNativeString(help) : nullptr;
		return gEnv->pConsole->RegisterString(ToNativeString(name), val, flags, h, thunk);
	}
	else
	{
		return nullptr;
	}
}

ICVar *ConsoleInterop::RegisterVariable(mono::string name, float *field, float value, EVarFlags flags, mono::string help)
{
	if (gEnv && gEnv->pConsole && name)
	{
		return gEnv->pConsole->Register
			(ToNativeString(name), field, value, flags, help ? ToNativeString(help) : nullptr);
	}
	else
	{
		return nullptr;
	}
}

ICVar *ConsoleInterop::RegisterVariableIntRef(mono::string name, int *field, int value, EVarFlags flags, mono::string help)
{
	if (gEnv && gEnv->pConsole && name)
	{
		return gEnv->pConsole->Register
			(ToNativeString(name), field, value, flags, help ? ToNativeString(help) : nullptr);
	}
	else
	{
		return nullptr;
	}
}

void ConsoleInterop::UnregisterVariable(mono::string name, bool _delete)
{
	if (gEnv && gEnv->pConsole && name)
	{
		gEnv->pConsole->UnregisterVariable(ToNativeString(name), _delete);
	}
}

ICVar *ConsoleInterop::GetVariable(mono::string name)
{
	if (gEnv && gEnv->pConsole && name)
	{
		return gEnv->pConsole->GetCVar(NtText(ToNativeString(name)));
	}
	else
	{
		return nullptr;
	}
}
