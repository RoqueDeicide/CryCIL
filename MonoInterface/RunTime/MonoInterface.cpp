#include "stdafx.h"
#include "MonoInterface.h"
#include "Implementation/Cryambly.h"
#include "Implementation/MonoCoreLibrary.h"
#include "Implementation/MonoAssemblies.h"


void HandleSignalAbort(int error)
{
	CryLogAlways("Aborted %i", error);
}


#pragma region Property Methods
//! Returns a pointer to app domain.
void *MonoInterface::GetAppDomain()
{
	return this->appDomain;
}
#pragma endregion
#pragma region Construction
//! Initializes Mono run-time environment.
MonoInterface::MonoInterface(IGameFramework *framework, List<IMonoSystemListener *> *listeners)
	: appDomain(nullptr)
	, broadcaster(nullptr)
{
	this->running = false;

	this->cryambly = nullptr;
	this->assemblies = nullptr;
	this->framework = framework;

	this->assemblies = new MonoAssemblies();
	this->broadcaster = new EventBroadcaster();
	// Register all initial listeners.
	this->RegisterDefaultListeners();
	if (listeners)
	{
		this->broadcaster->listeners->AddRange(listeners);
	}
	// Set global variables.
	MonoEnv = this;

	// Let listeners know about this object.
	this->broadcaster->SetInterface(this);

	this->broadcaster->OnPreInitialization();

	this->broadcaster->OnRunTimeInitializing();
	
	ReportComment("Setting mono directories.");
	
	// Tell Mono, where to look for the libraries and configuration files.
	const char *assembly_dir = DirectoryStructure::GetMonoLibraryFolder();
	const char *config_dir = DirectoryStructure::GetMonoConfigurationFolder();
	mono_set_dirs(assembly_dir, config_dir);

#ifdef _DEBUG
	ReportComment("Setting up signal chaining.");
	
	// Tell Mono to crash the game if there is an exception that wasn't handled.
	mono_set_signal_chaining(true);
	
	ReportComment("Initializing Mono debugging.");
	
	// Load up mdb files.
	mono_debug_init(MONO_DEBUG_FORMAT_MONO);
#endif // _DEBUG
	
	ReportComment("Registering HandeSignalAbort.");
	
	// Register HandleSignalAbort function as a handler for SIGABRT.
	signal(SIGABRT, HandleSignalAbort);
	
	ReportComment("Initializing the domain.");
	
	// Initialize the AppDomain.
	this->appDomain = mono_jit_init_version("CryCIL", "v4.0.30319");
	if (!this->appDomain)
	{
		CryFatalError("Unable to initialize Mono AppDomain.");
	}
	
	ReportComment("Setting the config for domain.");
	
	// Manually tell AppDomain where to find the configuration for itself.
	// Not calling this function results in exception when trying to get CodeDomProvider for C#.
	mono_domain_set_config
	(
		this->appDomain,
		DirectoryStructure::GetCryCilBinariesFolder(),
		DirectoryStructure::GetMonoAppDomainConfigurationFile()
	);
	
	this->running = true;

	this->gc = new MonoGC();
	this->objs = new MonoObjects();

#ifdef _DEBUG
	
	ReportComment("Loading pdb2mdb.");
	
	// Load Pdb2Mdb.dll before everything else.
	this->pdb2mdb = this->assemblies->Load(DirectoryStructure::GetPdb2MdbFile());
	
	// Initialize conversion thunk immediately.
	Pdb2MdbThunks::Convert = (this->pdb2mdb)
		? this->GetMethodThunk<ConvertPdbThunk>
		(this->pdb2mdb, "", "Driver", "Convert", "string")
		: nullptr;
#endif // _DEBUG

	ReportComment("Loading Cryambly.");
	
	// Load Cryambly.
	const char *cryamblyFile = DirectoryStructure::GetCryamblyFile();
	this->cryambly = new CryamblyWrapper(cryamblyFile);
	this->corlib = new MonoCoreLibrary();
	
	ReportComment("Initializing main thunks.");
	
	this->InitializeThunks();
	
	ReportComment("Main thunks initialized.");
	
	this->broadcaster->OnRunTimeInitialized();
	
	// Initialize an instance of type MonoInterface.
	mono::exception ex;
	MonoInterfaceThunks::Initialize(&ex);
	if (ex)
	{
		this->HandleException(ex);
		CryFatalError("CryCil.RunTime.MonoInterface object was not initialized. Cannot continue.");
	}
	
	this->framework->RegisterListener(this, "CryCIL", FRAMEWORKLISTENERPRIORITY_GAME);
	gEnv->pSystem->GetISystemEventDispatcher()->RegisterListener(this);
	
	this->broadcaster->OnPostInitialization();
	
	// Uncomment next 2 lines, if there is a need to crash the game when debugging initialization.

// 	int *crash = nullptr;
// 	*crash = 101;
}
#pragma endregion
#pragma region External Triggers
//! Triggers registration of FlowGraph nodes.
void MonoInterface::RegisterFlowGraphNodes()
{
	if (!this->running)
	{
		return;
	}
	mono::exception ex;
	MonoInterfaceThunks::TriggerFlowNodesRegistration(&ex);
}
//! Shuts down Mono run-time environment.
void MonoInterface::Shutdown()
{
	CryLogAlways("Checking activity before shutdown.");
	
	if (!this->running)
	{
		return;
	}
	
	CryLogAlways("About to broadcast shutdown event.");
	
	this->broadcaster->Shutdown();
	
	CryLogAlways("About to send shutdown event to Cryambly.");
	
	mono::exception ex;
	MonoInterfaceThunks::Shutdown(&ex);
	
	this->framework->UnregisterListener(this);
	gEnv->pSystem->GetISystemEventDispatcher()->RemoveListener(this);
	
	delete this->assemblies;
	delete this->gc;
	delete this->objs;
	
	CryLogAlways("Shutting down jit.");
	
	mono_jit_cleanup(this->appDomain);
	
	CryLogAlways("No more running.");
	
	this->running = false;
	
	CryLogAlways("Deleting broadcaster.");
	
	delete this->broadcaster;
}
#pragma endregion
#pragma region Interaction with Run-Time
//! Handles exception that occurred during managed method invocation.
void MonoInterface::HandleException(mono::exception exception)
{
	if (!this->running)
	{
		return;
	}
	mono::exception ex;
	MonoInterfaceThunks::DisplayException(exception, &ex);
}
#pragma endregion
#pragma region Listeners
//! Registers new object that receives notifications about CryCIL events.
void MonoInterface::AddListener(IMonoSystemListener *listener)
{
	if (!this->running)
	{
		return;
	}
	this->broadcaster->listeners->Add(listener);
}
//! Unregisters an object that receives notifications about CryCIL events.
void MonoInterface::RemoveListener(IMonoSystemListener *listener)
{
	if (!this->running)
	{
		return;
	}
	this->broadcaster->RemoveListener(listener);
}
#pragma endregion
#pragma region IGameFrameworkListener Implementation.
//! Triggers Update event in MonoInterface object in Cryambly.
void MonoInterface::OnPostUpdate(float fDeltaTime)
{
	// Notify everything about the update.
	if (this->running)
	{
		this->broadcaster->Update();
		
		mono::exception ex;
		MonoInterfaceThunks::Update(&ex);
		
		this->broadcaster->PostUpdate();
	}
}
//! Not used.
void MonoInterface::OnSaveGame(ISaveGame* pSaveGame) {}
//! Not used.
void MonoInterface::OnLoadGame(ILoadGame* pLoadGame) {}
//! Not used.
void MonoInterface::OnLevelEnd(const char* nextLevel) {}
//! Not used.
void MonoInterface::OnActionEvent(const SActionEvent& event) {}
#pragma endregion
#pragma region ISystemEventListener Implementation
//! Reacts to system events.
//!
//! @param event  Identifier of the event.
//! @param wparam First parameter that can supply extra information about the event.
//! @param lparam Second parameter that can supply extra information about the event.
void MonoInterface::OnSystemEvent(ESystemEvent event, UINT_PTR wparam, UINT_PTR lparam)
{
// 	switch (event)
// 	{
// 		case ESYSTEM_EVENT_CHANGE_FOCUS:
// 		{
// 			CryLogAlways("The window has lost/gained focus.");
// 		}
// 		break;
// 	default:
// 		break;
// 	}
}
#pragma endregion
#pragma region Default Listeners
void MonoInterface::RegisterDefaultListeners()
{
	this->broadcaster->listeners->Add(new TimingInterop());			// This interop has to be first to make
																	// sure that all timings are up-to-date.
#ifdef _DEBUG
	this->broadcaster->listeners->Add(new DebugEventReporter());
#endif // _DEBUG
	this->broadcaster->listeners->Add(new InitializationInterop());
	this->broadcaster->listeners->Add(new LogPostingInterop());
	this->broadcaster->listeners->Add(new CryMarshalInterop());
	this->broadcaster->listeners->Add(new MeshOpsInterop());
	this->broadcaster->listeners->Add(new BatchOps());
	this->broadcaster->listeners->Add(new MouseInterop());
	this->broadcaster->listeners->Add(new InputInterop());
	this->broadcaster->listeners->Add(new ConsoleInterop());
	this->broadcaster->listeners->Add(new ConsoleVariableInterop());
	this->broadcaster->listeners->Add(new DebugDrawInterop());
	this->broadcaster->listeners->Add(new ProfilingInterop());
	this->broadcaster->listeners->Add(new ArchiveStreamInterop());
	this->broadcaster->listeners->Add(new CryArchiveInterop());
	this->broadcaster->listeners->Add(new CryFilesInterop());
	this->broadcaster->listeners->Add(new AliasesInterop());
	this->broadcaster->listeners->Add(new CryPakInterop());
	this->broadcaster->listeners->Add(new AuxiliaryGeometryInterop());
	this->broadcaster->listeners->Add(new RendererInterop());
	this->broadcaster->listeners->Add(new TextureInterop());
}
#pragma endregion
#pragma region Thunks Initialization
	void MonoInterface::InitializeThunks()
	{
		this->InitializeMonoInterfaceThunks();
		this->InitializeAssemblyCollectionThunks();
	}
	void MonoInterface::InitializeMonoInterfaceThunks()
	{
		CryLogAlways("Initializing mono interface thunks.");
		MonoInterfaceThunks::DisplayException =
			this->GetMethodThunk<DisplayExceptionThunk>
			(this->cryambly, "CryCil.RunTime", "MonoInterface", "DisplayException", "System.Object");
		MonoInterfaceThunks::Initialize =
			this->GetMethodThunk<InitializeThunk>
			(this->cryambly, "CryCil.RunTime", "MonoInterface", "Initialize", nullptr);
		MonoInterfaceThunks::TriggerFlowNodesRegistration =
			this->GetMethodThunk<RegisterFlowNodesThunk>
			(this->cryambly, "CryCil.RunTime", "MonoInterface", "RegisterFlowGraphNodeTypes", nullptr);
		MonoInterfaceThunks::Shutdown =
			this->GetMethodThunk<ShutDownThunk>
			(this->cryambly, "CryCil.RunTime", "MonoInterface", "Shutdown", nullptr);
		MonoInterfaceThunks::Update =
			this->GetMethodThunk<UpdateThunk>
			(this->cryambly, "CryCil.RunTime", "MonoInterface", "Update", nullptr);
	}
	void MonoInterface::InitializeAssemblyCollectionThunks()
	{
		AssemblyCollectionThunks::LookUpAssembly =
			this->GetMethodThunk<LookUpAssemblyThunk>
			(this->cryambly, "CryCil.RunTime", "AssemblyLookUp", "LookUpAssembly", "System.String");
	}
	template<typename MethodSignature>
	MethodSignature MonoInterface::GetMethodThunk(IMonoAssembly *assembly, const char *nameSpace, const char *className, const char *methodName, const char *params)
	{
		return (MethodSignature)assembly->GetClass(nameSpace, className)
										->GetFunction(methodName, params)
										->UnmanagedThunk;
	}
#pragma endregion

