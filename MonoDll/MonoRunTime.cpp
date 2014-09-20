#include "stdafx.h"
#include "MonoRunTime.h"
// Initialize singleton instance to null, before run-time is initialized properly.
#ifndef PLUGIN_SDK
IMonoRunTime *IMonoRunTime::instance = nullptr;
#else
IMonoRunTime *MonoRunTime::instance = nullptr;
#endif

/**
* Initializes Mono run-time.
*/
MonoRunTime::MonoRunTime(IGameFramework *pGameFramework)
	:mainAppDomain(nullptr),
	 cryBraryAssembly(nullptr),
	 pdb2MdbAssembly(nullptr),
	 monoInterface(nullptr),
	 gameFramework(pGameFramework),
	 quitting(false)
{
	//
	// Setup run-time environment.
	//
	CryLogAlways("    Initializing CryMono run-time environment.");
	// This constructor is supposed to be called only once, so lets initialize the singleton, while we are at it.
	instance = this;
	// Prepare the console variables.
	this->consoleVariables = new SCVars();
	g_pMonoCVars = this->consoleVariables;
	// Configure Mono to use appropriate directories for looking up the libraries and their configuration.
	mono_set_dirs(PathUtils::GetMonoLibPath(), PathUtils::GetMonoConfigPath());
#ifndef _RELEASE
	// Enable proper exception chaining, so if Mono exception is not handled by anything, it crashes CryEngine.
	mono_set_signal_chaining(true);
	// Load up mdb file.
	mono_debug_init(MONO_DEBUG_FORMAT_MONO);
#endif // !_RELEASE
	//
	// Setup CryMono.
	//
	CryLogAlways("    Initializing CryMono subsystems.");
	// Register HandleSignalAbort function as a handler for SIGABRT.
	signal(SIGABRT, HandleSignalAbort);
	// Initialize app domain. Use 4.5 .NET libs.
	this->mainAppDomain = new CScriptDomain(ERuntimeVersion::eRV_4_30319);
	// Register System.Object as default object type in ScriptArray.
	CScriptArray::m_pDefaultElementClass = mono_get_object_class();
#ifndef _RELEASE
	// Try loading assembly that provides methods for converting .pdb files to .mdb files.
	this->pdb2MdbAssembly = this->mainAppDomain->LoadAssembly(PathUtils::GetMonoPath() + "bin\\pdb2mdb.dll");
#endif // !_RELEASE
	// Load CryBrary.
	this->cryBraryAssembly = this->mainAppDomain->LoadAssembly(PathUtils::GetBinaryPath() + "CryBrary.dll");
	// Register interops.
	this->RegisterMainInterops();
	this->RegisterSecondaryInterops();
	//
	// Initialize CryBrary subsystems.
	//
	// Setup information to pass to constructor.
	IMonoArray *initializationParams = CreateMonoArray(1);
	initializationParams->InsertMonoString(ToMonoString(PathUtils::GetConfigPath()));
	// Initialize.
	this->monoInterface =
		*this->cryBraryAssembly->GetClass("MonoInterface", "CryEngine.RunTime")->CreateInstance(initializationParams);
	// Register event listeners.
	this->gameFramework->RegisterListener(this, "CryMono", FRAMEWORKLISTENERPRIORITY_GAME);			// Game events.
	gEnv->pSystem->GetISystemEventDispatcher()->RegisterListener(&g_systemEventListener_CryMono);	// System events.
}
/**
* Destroys Mono run-time.
*/
MonoRunTime::~MonoRunTime()
{
	this->quitting = true;
	// Dispose of all the interops.
	for (auto it = this->interops.begin(); it != this->interops.end(); ++it)
		delete (*it);
	this->interops.clear();
	// Delete MonoInterface object.
	SAFE_RELEASE(this->monoInterface);
	// Unload the AppDomain.
	SAFE_RELEASE(this->mainAppDomain);
	// Let the game know about our disposal.
	this->gameFramework->UnregisterListener(this);
	// Clear the method interops.
	this->interopsMethods.clear();
	// Clear the console variables.
	SAFE_DELETE(this->consoleVariables);
	// Log the shutdown.
	CryLogAlways("CryMono shutting down.");

	instance = nullptr;
}
