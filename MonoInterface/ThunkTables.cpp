#include "stdafx.h"
#include "ThunkTables.h"

DisplayExceptionThunk  MonoInterfaceThunks::DisplayException = nullptr;
InitializeThunk        MonoInterfaceThunks::Initialize = nullptr;
RegisterFlowNodesThunk MonoInterfaceThunks::TriggerFlowNodesRegistration = nullptr;
ShutDownThunk          MonoInterfaceThunks::Shutdown = nullptr;
UpdateThunk            MonoInterfaceThunks::Update = nullptr;

LookUpAssemblyThunk AssemblyCollectionThunks::LookUpAssembly = nullptr;
