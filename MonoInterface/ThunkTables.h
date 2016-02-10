#pragma once

#include "IMonoInterface.h"

//! Signature of the method MonoInterface.DisplayException();
typedef mono::nothing(__stdcall *DisplayExceptionThunk)(mono::object, mono::exception *);
//! Signature of the method MonoInterface.Initialize();
typedef mono::object (__stdcall *InitializeThunk)(mono::exception *);
//! Signature of the method MonoInterface.RegisterFlowGraphNodeTypes();
typedef mono::nothing(__stdcall *RegisterFlowNodesThunk)(mono::exception *);
//! Signature of the method MonoInterface.Update();
typedef mono::nothing(__stdcall *UpdateThunk)(mono::exception *);
//! Signature of the method MonoInterface.Shutdown();
typedef mono::nothing(__stdcall *ShutDownThunk)(mono::exception *);

struct MonoInterfaceThunks
{
	static DisplayExceptionThunk DisplayException;
	static InitializeThunk Initialize;
	static RegisterFlowNodesThunk TriggerFlowNodesRegistration;
	static ShutDownThunk Shutdown;
	static UpdateThunk Update;
};

//! Signature of the method Driver.Convert(string);
typedef mono::string(__stdcall *LookUpAssemblyThunk)(mono::string, mono::exception *);

struct AssemblyCollectionThunks
{
	static LookUpAssemblyThunk LookUpAssembly;
};