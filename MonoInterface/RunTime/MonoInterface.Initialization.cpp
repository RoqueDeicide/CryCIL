#include "stdafx.h"
#include "MonoInterface.h"
#include "Implementation/Cryambly.h"
#include "Implementation/MonoCoreLibrary.h"
#include "Implementation/MonoAssemblies.h"

#if 1
  #define InitializationMessage CryLogAlways
#else
  #define InitializationMessage(...) void(0)
#endif

#include <Testing/TestStart.h>
#include <mono/utils/mono-logger.h>
#include <Interops/LogPosting.h>

void HandleSignalAbort(int error)
{
	CryLogAlways("Aborted %i", error);
}

void TestFramework()
{
	BeginTheTest();
}

//! Initializes Mono run-time environment.
MonoInterface::MonoInterface(IGameFramework *framework, List<IMonoSystemListener *> *listeners,
							 MonoLog::Level logLevel, const SSystemInitParams &startupParams)
	: broadcaster(nullptr)
	, appDomain(nullptr)
{
	_this = this;

	this->running = false;

	this->cryambly   = nullptr;
	this->assemblies = nullptr;
	this->framework  = framework;
	this->funcs      = new MonoFunctions();

	this->assemblies  = new MonoAssemblies();
	this->broadcaster = new EventBroadcaster();

	// Initialize paths. This must be done before DirectoryStructure class can be used.
	char pathArray[2080];
	CryGetExecutableFolder(2080, pathArray);
	this->executablePath = pathArray;
	this->projectPath    = startupParams.szProjectDllDir;

	// Register all initial listeners.
	this->RegisterDefaultListeners();
	if (listeners)
	{
		this->broadcaster->listeners.AddRange(*listeners);
		listeners->Clear();
	}

	// Set global variables.
	MonoEnv = this;

	// Let listeners know about this object.
	this->broadcaster->SetInterface(this);

	this->broadcaster->OnPreInitialization();

	InitializationMessage("Sending RunTimeInitializing event");

	this->broadcaster->OnRunTimeInitializing();

	InitializationMessage("Setting mono directories.");

	// Tell Mono, where to look for the libraries and configuration files.
	const char *assembly_dir = DirectoryStructure::GetMonoLibraryFolder();
	const char *config_dir   = DirectoryStructure::GetMonoConfigurationFolder();
	mono_set_dirs(assembly_dir, config_dir);

	InitializeDebugEnvironment();

	InitializationMessage("Registering HandeSignalAbort.");

	// Register HandleSignalAbort function as a handler for SIGABRT.
	signal(SIGABRT, HandleSignalAbort);

	this->RegisterHooks(logLevel);

	InitializationMessage("Initializing the domain.");

	// Initialize the AppDomain.
	this->appDomain = mono_jit_init_version("CryCIL", "v4.0.30319");
	if (!this->appDomain)
	{
		CryFatalError("Unable to initialize Mono AppDomain.");
	}

	InitializationMessage("Setting the config for domain.");

	// Manually tell AppDomain where to find the configuration for itself.
	// Not calling this function results in exception when trying to get CodeDomProvider for C#.
	mono_domain_set_config(this->appDomain,
						   DirectoryStructure::GetCryCilBinariesFolder(),
						   DirectoryStructure::GetMonoAppDomainConfigurationFile());

	this->running = true;

	this->gc   = new MonoGC();
	this->objs = new MonoObjects();

	InitializationMessage("Loading Cryambly.");

	// Load Cryambly.
	const char *cryamblyFile = DirectoryStructure::GetCryamblyFile();

	InitializationMessage("Creating a Cryambly object.");

	this->cryambly = new CryamblyWrapper(cryamblyFile);

	InitializationMessage("Creating a CoreLibrary object.");

	this->corlib = new MonoCoreLibrary();

	InitializationMessage("Initializing main thunks.");

	this->InitializeThunks();

	InitializationMessage("Main thunks initialized.");

	const char *ns = "CryCil.RunTime";
	const char *cn = "MonoInterface";

	this->funcs->AddInternalCall(ns, cn, "OnCompilationStartingBind", OnCompilationStartingBind);
	this->funcs->AddInternalCall(ns, cn, "OnCompilationCompleteBind", OnCompilationCompleteBind);
	this->funcs->AddInternalCall(ns, cn, "GetSubscribedStagesBind", GetSubscribedStagesBind);
	this->funcs->AddInternalCall(ns, cn, "OnInitializationStageBind", OnInitializationStageBind);

	// Redirect console output to the CryEngine log.
	LogPostingInterop();

	// Initiate testing.
	this->funcs->AddInternalCall(ns, "TestLauncher", "Test", TestFramework);
	const IMonoClass    *testLauncher = this->cryambly->GetClass(ns, "TestLauncher");
	const IMonoFunction *testFunc     = testLauncher->GetFunction("StartTesting");
	void                *testThunk    = testFunc->RawThunk;
	static_cast<void (*)()>(testThunk)();

	this->broadcaster->OnRunTimeInitialized();

	// Initialize the folder paths.
	void *thunk = this->Cryambly->GetClass(ns, "DirectoryStructure")->GetFunction("InitializeFolderPaths", -1)->RawThunk;
	mono::string ef = ToMonoString(this->executablePath);
	mono::string pf = ToMonoString(this->projectPath);
	mono::string cf = ToMonoString(gEnv->pCryPak->GetGameFolder());
	static_cast<void(*)(mono::string, mono::string, mono::string)>(thunk)(ef, pf, cf);

	// Initialize an instance of type MonoInterface.
	mono::exception ex;
	MonoInterfaceThunks::Initialize(&ex);
	if (ex)
	{
		mono::exception eX;
		MonoInterfaceThunks::DisplayException(ex, &eX);
		CryFatalError("CryCil.RunTime.MonoInterface object was not initialized. Cannot continue.");
	}

	this->framework->RegisterListener(this, "CryCIL", FRAMEWORKLISTENERPRIORITY_GAME);
	gEnv->pSystem->GetISystemEventDispatcher()->RegisterListener(this);

	this->broadcaster->OnPostInitialization();

	// Uncomment next 2 lines, if there is a need to crash the game when debugging initialization.

	//  int *crash = nullptr;
	//  *crash = 101;
}

void MonoInterface::InitializeDebugEnvironment()
{
#ifndef _RELEASE
	InitializationMessage("Setting up signal chaining.");

	// Tell Mono to crash the game if there is an exception that wasn't handled.
	mono_set_signal_chaining(true);
	mono_set_crash_chaining(true);

	InitializationMessage("Initializing Mono debugging.");

	// Enable debug data support for debuggers.
	mono_debug_init(MONO_DEBUG_FORMAT_MONO);

	char *debugOptions[] =
	{
		"--soft-breakpoints",
		"--debugger-agent=transport=dt_socket,address=127.0.0.1:17615,embedding=1,server=y,suspend=n"
	};

	mono_jit_parse_options(sizeof debugOptions / sizeof(char *), debugOptions);
#endif // _RELEASE
}
