#include "stdafx.h"
#include "ThunkTables.h"

CreateInstanceThunk MonoClassThunks::CreateInstance = nullptr;
StaticEqualsThunk MonoClassThunks::StaticEquals = nullptr;

DisplayExceptionThunk MonoInterfaceThunks::DisplayException = nullptr;
InitializeThunk MonoInterfaceThunks::Initialize = nullptr;
RegisterFlowNodesThunk MonoInterfaceThunks::TriggerFlowNodesRegistration = nullptr;
ShutDownThunk MonoInterfaceThunks::Shutdown = nullptr;
UpdateThunk MonoInterfaceThunks::Update = nullptr;

ConvertPdbThunk Pdb2MdbThunks::Convert = nullptr;
