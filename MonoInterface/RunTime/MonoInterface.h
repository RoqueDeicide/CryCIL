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

//! Handles interface between CryEngine and Mono.
class MonoInterface
	: public IMonoInterface
	, public IGameFrameworkListener
	, public ISystemEventListener
{
private:
#pragma region Fields
	EventBroadcaster *broadcaster;
	MonoDomain *appDomain;

	static MonoInterface *_this;
#pragma endregion
public:
#pragma region Property Methods
	//! Returns a pointer to app domain.
	virtual void *GetAppDomain() override;
#pragma endregion
#pragma region Construction
	//! Initializes Mono run-time environment.
	//!
	//! @param framework Pointer to IGameFramework object that cannot be obtained in any other way.
	MonoInterface(IGameFramework *framework, List<IMonoSystemListener *> *listeners);
#pragma endregion
#pragma region External Triggers
	//! Triggers registration of FlowGraph nodes.
	virtual void RegisterFlowGraphNodes() override;
	//! Shuts down Mono run-time environment.
	virtual void Shutdown() override;
#pragma endregion
#pragma region Interaction with Run-Time
	//! Handles exception that occurred during managed method invocation.
	virtual void HandleException(mono::exception exception) override;
#pragma endregion
#pragma region Listeners
	//! Registers new object that receives notifications about CryCIL events.
	virtual void AddListener(IMonoSystemListener *listener) override;
	//! Unregisters an object that receives notifications about CryCIL events.
	virtual void RemoveListener(IMonoSystemListener *listener) override;
#pragma endregion
#pragma region IGameFrameworkListener Implementation.
	//! Triggers Update event in MonoInterface object in Cryambly.
	virtual void OnPostUpdate(float fDeltaTime) override;
	//! Not used.
	virtual void OnSaveGame(ISaveGame* pSaveGame) override;
	//! Not used.
	virtual void OnLoadGame(ILoadGame* pLoadGame) override;
	//! Not used.
	virtual void OnLevelEnd(const char* nextLevel) override;
	//! Not used.
	virtual void OnActionEvent(const SActionEvent& event) override;
#pragma endregion
#pragma region ISystemEventListener Implementation
	//! Reacts to system events.
	//!
	//! @param event  Identifier of the event.
	//! @param wparam First parameter that can supply extra information about the event.
	//! @param lparam Second parameter that can supply extra information about the event.
	virtual void OnSystemEvent(ESystemEvent event, UINT_PTR wparam, UINT_PTR lparam) override;
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
	static void OnCompilationStartingBind();
	static void OnCompilationCompleteBind(bool success);
	static mono::Array GetSubscribedStagesBind();
	static void OnInitializationStageBind(int stageIndex);
#pragma endregion
};