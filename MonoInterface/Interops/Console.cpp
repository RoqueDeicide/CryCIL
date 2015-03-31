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

	MonoEnv->RemoveListener(this);	// No need to listen anymore.
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