#include "stdafx.h"
#include "MonoInterface.h"


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

IMonoAssembly *MonoInterface::GetCryambly()
{
	return this->cryambly;
}

IMonoAssembly *MonoInterface::GetPdbMdbAssembly()
{
	return this->pdb2mdb;
}

IMonoAssembly *MonoInterface::GetCoreLibrary()
{
	return this->corlib;
}

bool MonoInterface::GetInitializedIndication()
{
	return this->running;
}

IDefaultBoxinator *MonoInterface::GetDefaultBoxer()
{
	return &this->boxer;
}

IGameFramework *MonoInterface::GetGameFramework()
{
	return this->framework;
}
#pragma endregion
#pragma region Construction
//! Initializes Mono run-time environment.
MonoInterface::MonoInterface(IGameFramework *framework, List<IMonoSystemListener *> *listeners)
	: running(false)
	, appDomain(nullptr)
	, cryambly(nullptr)
	, assemblies(10)
	, broadcaster(nullptr)
	, gc()
	, framework(framework)
{
	broadcaster = new EventBroadcaster();
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
		DirectoryStructure::GetMonoBinariesFolder(),
		DirectoryStructure::GetMonoAppDomainConfigurationFile()
	);
	this->running = true;
#ifdef _DEBUG
	ReportComment("Loading pdb2mdb.");
	// Load Pdb2Mdb.dll before everything else.
	this->pdb2mdb = this->LoadAssembly(DirectoryStructure::GetPdb2MdbFile());
	// Initialize conversion thunk immediately.
	Pdb2MdbThunks::Convert = (this->pdb2mdb)
		? this->GetMethodThunk<ConvertPdbThunk>
		(this->pdb2mdb, "", "Driver", "Convert", "string")
		: nullptr;
#endif // _DEBUG

	ReportComment("Loading Cryambly.");
	// Load Cryambly.
	const char *cryamblyFile = DirectoryStructure::GetCryamblyFile();
	this->cryambly = this->LoadAssembly(cryamblyFile);
	this->corlib = this->WrapAssembly(mono_image_get_assembly(mono_get_corlib()));
	ReportComment("Initializing main thunks.");
	this->InitializeThunks();
	ReportComment("Main thunks initialized.");
	this->broadcaster->OnRunTimeInitialized();
	// Initialize an instance of type MonoInterface.
	mono::exception ex;
	this->managedInterface = this->gc->Keep(MonoInterfaceThunks::Initialize(&ex));
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
	MonoInterfaceThunks::TriggerFlowNodesRegistration(this->managedInterface->ObjectPointer, &ex);
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
	MonoInterfaceThunks::Shutdown(this->managedInterface->ObjectPointer, &ex);
	this->framework->UnregisterListener(this);
	gEnv->pSystem->GetISystemEventDispatcher()->RemoveListener(this);
	CryLogAlways("Shutting down jit.");
	mono_jit_cleanup(this->appDomain);
	CryLogAlways("No more running.");
	this->running = false;
	CryLogAlways("Deleting broadcaster.");
	delete this->broadcaster;
}
#pragma endregion
#pragma region String Conversions
//! Converts given null-terminated string to Mono managed object.
mono::string MonoInterface::ToManagedString(const char *text)
{
	if (!this->running)
	{
		return nullptr;
	}
	return (mono::string)mono_string_new(this->appDomain, text);
}
//! Converts given managed string to null-terminated one.
const char *MonoInterface::ToNativeString(mono::string text)
{
	if (!this->running)
	{
		return nullptr;
	}
	const char *monoNativeText = mono_string_to_utf8((MonoString *)text);
	ConstructiveText nativeText(monoNativeText);
	mono_free((void *)monoNativeText);
	return nativeText.ToNTString();
}
#pragma endregion
#pragma region Objects and Arrays
//! Creates a new wrapped MonoObject using constructor with specific parameters.
mono::object MonoInterface::CreateObject(IMonoAssembly *assembly, const char *name_space, const char *class_name, IMonoArray *params)
{
	if (!this->running)
	{
		return nullptr;
	}
	return assembly->GetClass(class_name, name_space)->CreateInstance(params);

}
//! Creates a new Mono handle wrapper for given MonoObject.
IMonoHandle *MonoInterface::WrapObject(mono::object obj)
{
	if (!this->running)
	{
		return nullptr;
	}
	return new MonoHandle(obj);
}
//! Creates object of type object[] with specified capacity.
IMonoArray *MonoInterface::CreateArray(int capacity)
{
	if (!this->running)
	{
		return nullptr;
	}
	return new MonoArrayWrapper(capacity);
}
//! Creates object of specified type with specified capacity.
IMonoArray *MonoInterface::CreateArray(IMonoClass *klass, int capacity)
{
	if (!this->running)
	{
		return nullptr;
	}
	return new MonoArrayWrapper(klass, capacity);
}
//! Wraps already existing Mono array.
IMonoArray *MonoInterface::WrapArray(mono::Array arrayHandle)
{
	if (!this->running)
	{
		return nullptr;
	}
	return new MonoArrayWrapper((MonoArray *)arrayHandle);
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
//! Registers a new internal call.
void MonoInterface::AddInternalCall(const char *nameSpace, const char *className, const char *name, void *functionPointer)
{
	if (!this->running)
	{
		return;
	}
	ConstructiveText fullName(30);
	fullName << nameSpace << '.' << className << "::" << name;
	mono_add_internal_call(fullName.ToNTString(), functionPointer);
}
#pragma endregion
#pragma region Assemblies
//! Loads a Mono assembly into memory.
//!
//! @param moduleFileName Name of the file inside Modules folder.
IMonoAssembly *MonoInterface::LoadAssembly(const char *moduleFileName)
{
	if (!this->running)
	{
		return nullptr;
	}
	bool failed;
	MonoAssemblyWrapper *wrapper = new MonoAssemblyWrapper(moduleFileName, failed);
	if (failed)
	{
		return nullptr;
	}
	this->assemblies.Add(wrapper);
	return wrapper;
}
//! Wraps assembly pointer.
IMonoAssembly *MonoInterface::WrapAssembly(void *assemblyHandle)
{
	if (!this->running)
	{
		return nullptr;
	}
	for (int i = 0; i < this->assemblies.Length; i++)
	{
		if (this->assemblies[i]->GetWrappedPointer() == assemblyHandle)
		{
			return this->assemblies[i];
		}
	}
	MonoAssemblyWrapper *wrapper = new MonoAssemblyWrapper((MonoAssembly *)assemblyHandle);
	this->assemblies.Add(wrapper);
	return wrapper;
}
//! Wraps an assembly.
IMonoAssembly *MonoInterface::WrapAssembly(const char *fullAssemblyName)
{
	if (!this->running)
	{
		return nullptr;
	}
	return this->WrapAssembly(mono_assembly_loaded(mono_assembly_name_new(fullAssemblyName)));
}
#pragma endregion
#pragma region Unboxing
//! Unboxes managed value-type object.
void *MonoInterface::Unbox(mono::object value)
{
	if (!this->running)
	{
		return nullptr;
	}
	return mono_object_unbox((MonoObject *)value);
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
		MonoInterfaceThunks::Update(this->managedInterface->ObjectPointer, &ex);
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
#ifdef _DEBUG
	this->broadcaster->listeners->Add(new DebugEventReporter());
#endif // _DEBUG
	this->broadcaster->listeners->Add(new InitializationInterop());
	this->broadcaster->listeners->Add(new LogPostingInterop());
}
#pragma endregion
#pragma region Thunks Initialization
	void MonoInterface::InitializeThunks()
	{
		this->InitializeMonoInterfaceThunks();
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
	template<typename MethodSignature>
	MethodSignature MonoInterface::GetMethodThunk(IMonoAssembly *assembly, const char *nameSpace, const char *className, const char *methodName, const char *params)
	{
		return (MethodSignature)assembly->Classes[nameSpace][className]
										->GetMethod(methodName, params)
										->UnmanagedThunk;
	}
#pragma endregion

