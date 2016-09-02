#pragma once

#include "IMonoInterface.h"

#include "Engine/DirectoryStructure.h"
#include "MonoHeaders.h"

#include <cstdlib>
#include <csignal>

#include "ThunkTables.h"

#include "API_ImplementationHeaders.h"

#include "RunTime/DebugEventReporter.h"
#include "RunTime/EventBroadcaster.h"

#include "List.hpp"

static const char* monoLogLevels[] =
{
	nullptr,
	"error",
	"critical",
	"warning",
	"message",
	"info",
	"debug"
};

//! Handles interface between CryEngine and Mono.
class MonoInterface
	: public IMonoInterface
	, public IGameFrameworkListener
	, public ISystemEventListener
{
	#pragma region Fields
	EventBroadcaster *broadcaster;		//!< An object that broadcasts CryCIL events.
	MonoDomain *appDomain;				//!< Pointer to the Mono AppDomain where all managed code is executed.
	Text executablePath;				//!< Path to the directory that contains the Launcher file.
	Text projectPath;					//!< Path to the directory that contains the game content.

	static MonoInterface *_this;
	#pragma endregion
public:
	#pragma region Property Methods
	//! Returns a pointer to app domain.
	void       *GetAppDomain() override;
	const Text &GetBasePath() override;
	const Text &GetProjectPath() override;
	void        SetMonoLogLevel(MonoLog::Level logLevel) override;
	#pragma endregion
	#pragma region Construction
	//! Initializes Mono run-time environment.
	//!
	//! @param framework Pointer to IGameFramework object that cannot be obtained in any other way.
	MonoInterface(IGameFramework *framework, List<IMonoSystemListener *> *listeners,
				  MonoLog::Level logLevel, const SSystemInitParams &startupParams);
	#pragma endregion
	#pragma region External Triggers
	//! Triggers registration of FlowGraph nodes.
	void RegisterFlowGraphNodes() override;
	//! Shuts down Mono run-time environment.
	void Shutdown() override;
	#pragma endregion
	#pragma region Interaction with Run-Time
	//! Handles exception that occurred during managed method invocation.
	void HandleException(mono::exception exception) override;
	#pragma endregion
	#pragma region Listeners
	//! Registers new object that receives notifications about CryCIL events.
	void AddListener(IMonoSystemListener *listener) override;
	//! Unregisters an object that receives notifications about CryCIL events.
	void RemoveListener(IMonoSystemListener *listener) override;
	#pragma endregion
	#pragma region IGameFrameworkListener Implementation.
	//! Triggers Update event in MonoInterface object in Cryambly.
	void OnPostUpdate(float fDeltaTime) override;
	//! Not used.
	void OnSaveGame(ISaveGame *pSaveGame) override;
	//! Not used.
	void OnLoadGame(ILoadGame *pLoadGame) override;
	//! Not used.
	void OnLevelEnd(const char *nextLevel) override;
	//! Not used.
	void OnActionEvent(const SActionEvent &event) override;
	#pragma endregion
	#pragma region ISystemEventListener Implementation
	//! Reacts to system events.
	//!
	//! @param event  Identifier of the event.
	//! @param wparam First parameter that can supply extra information about the event.
	//! @param lparam Second parameter that can supply extra information about the event.
	void OnSystemEvent(ESystemEvent event, UINT_PTR wparam, UINT_PTR lparam) override;
	#pragma endregion
private:
	#pragma region Default Listeners
	void RegisterDefaultListeners() const;
	#pragma endregion
	#pragma region Thunks Initialization
	void InitializeThunks();
	void InitializeMonoInterfaceThunks();
	void InitializeAssemblyCollectionThunks();
	template<typename MethodSignature>
	MethodSignature GetMethodThunk(IMonoAssembly *assembly, const char *nameSpace, const char *className, const char *methodName, const char *params);
	#pragma endregion
	#pragma region Internal Calls
	static void        OnCompilationStartingBind();
	static void        OnCompilationCompleteBind(bool success);
	static mono::Array GetSubscribedStagesBind();
	static void        OnInitializationStageBind(int stageIndex);
	#pragma endregion
#pragma region Mono Hooks
	//! Invoked by Mono to allow CryCIL to print the Mono log message into CryEngine log.
	//!
	//! @param log_domain Name of the AppDomain that originated the message.
	//! @param log_level  Name of the logging level.
	//! @param message    Message to post into the log.
	//! @param fatal      Not used.
	//! @param user_data  Not used.
	static void MonoLogCallback(const char *log_domain, const char *log_level, const char *message, mono_bool, void *);
	//! Invoked by Mono to allow CryCIL to print the Mono message into CryEngine log.
	//!
	//! @param message   Message to post into the log.
	//! @param is_stdout Not used.
	static void MonoPrintCallback(const char *string, mono_bool);
	//! Invoked by Mono to allow CryCIL to print the Mono error message into CryEngine log.
	//!
	//! @param message   Error to post into the log.
	//! @param is_stdout Not used.
	static void MonoPrintErrorCallback(const char *string, mono_bool);
	void RegisterHooks(MonoLog::Level logLevel);
#pragma endregion

	static void InitializeDebugEnvironment();
};