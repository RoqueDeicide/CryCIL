#pragma once

#include "IMonoInterface.h"

typedef mono::object(*CreateInstanceThunk)(mono::type, mono::Array, mono::exception*);

typedef mono::boolean(*StaticEqualsThunk)(mono::object, mono::object, mono::exception*);

struct MonoClassThunks
{
	static CreateInstanceThunk CreateInstance;
	static StaticEqualsThunk StaticEquals;
};

typedef mono::nothing(*DisplayExceptionThunk)(mono::object, mono::exception *);
typedef mono::object(*InitializeThunk)(mono::exception *);
typedef mono::nothing(*RegisterFlowNodesThunk)(mono::object monoInterface, mono::exception *);
typedef mono::nothing(*UpdateThunk)(mono::object monoInterface, mono::exception *);
typedef mono::nothing(*ShutDownThunk)(mono::object monoInterface, mono::exception *);

struct MonoInterfaceThunks
{
	static DisplayExceptionThunk DisplayException;
	static InitializeThunk Initialize;
	static RegisterFlowNodesThunk TriggerFlowNodesRegistration;
	static ShutDownThunk Shutdown;
	static UpdateThunk Update;
};

typedef mono::nothing(*ConvertPdbThunk)(mono::string, mono::exception *);

struct Pdb2MdbThunks
{
	static ConvertPdbThunk Convert;
};