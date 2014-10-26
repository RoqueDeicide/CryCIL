#pragma once

#include "IMonoInterface.h"

//! Signature of the method Activator.CreateInstance(Type,object[]);
typedef mono::object(*CreateInstanceThunk)(mono::type, mono::Array, mono::exception*);
//! Signature of the method 
typedef mono::boolean(*StaticEqualsThunk)(mono::object, mono::object, mono::exception*);

struct MonoClassThunks
{
	static CreateInstanceThunk CreateInstance;
	static StaticEqualsThunk StaticEquals;
};

//! Signature of the method MonoInterface.DisplayException();
typedef mono::nothing(*DisplayExceptionThunk)(mono::object, mono::exception *);
//! Signature of the method MonoInterface.Initialize();
typedef mono::object(*InitializeThunk)(mono::exception *);
//! Signature of the method MonoInterface.RegisterFlowGraphNodeTypes();
typedef mono::nothing(*RegisterFlowNodesThunk)(mono::object monoInterface, mono::exception *);
//! Signature of the method MonoInterface.Update();
typedef mono::nothing(*UpdateThunk)(mono::object monoInterface, mono::exception *);
//! Signature of the method MonoInterface.Shutdown();
typedef mono::nothing(*ShutDownThunk)(mono::object monoInterface, mono::exception *);

struct MonoInterfaceThunks
{
	static DisplayExceptionThunk DisplayException;
	static InitializeThunk Initialize;
	static RegisterFlowNodesThunk TriggerFlowNodesRegistration;
	static ShutDownThunk Shutdown;
	static UpdateThunk Update;
};

//! Signature of the method Driver.Convert(string);
typedef mono::nothing(*ConvertPdbThunk)(mono::string, mono::exception *);

struct Pdb2MdbThunks
{
	static ConvertPdbThunk Convert;
};