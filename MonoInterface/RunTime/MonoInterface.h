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
#include "RunTime/AllInterops.h"

#include "List.h"

//! Handles interface between CryEngine and Mono.
class MonoInterface
	: public IMonoInterface
	, public IGameFrameworkListener
	, public ISystemEventListener
{
	friend InitializationInterop;
private:
#pragma region Fields
	EventBroadcaster *broadcaster;

	bool running;

	MonoDomain *appDomain;
	IMonoAssemblyCollection *assemblies;
	ICryambly *cryambly;					//! Extra pointer for Cryambly.
	IMonoCoreLibrary *corlib;				//! Extra pointer for mscorlib.
	IMonoAssembly *pdb2mdb;

	IMonoGCHandle *managedInterface;
	IMonoGC *gc;
	IGameFramework *framework;
	IMonoObjects *objs;
#pragma endregion
public:
#pragma region Property Methods
	//! Returns a pointer to app domain.
	virtual void *GetAppDomain();

	virtual IMonoAssemblyCollection *GetAssemblyCollection();

	virtual ICryambly *GetCryambly();

	virtual IMonoAssembly *GetPdbMdbAssembly();

	virtual IMonoCoreLibrary *GetCoreLibrary();

	virtual bool GetInitializedIndication();

	virtual IGameFramework *GetGameFramework();

	virtual IMonoObjects *GetObjects();

	virtual IMonoGC *GetGC()
	{
		return this->gc;
	}
#pragma endregion
#pragma region Construction
	//! Initializes Mono run-time environment.
	//!
	//! @param framework Pointer to IGameFramework object that cannot be obtained in any other way.
	MonoInterface(IGameFramework *framework, List<IMonoSystemListener *> *listeners);
#pragma endregion
#pragma region External Triggers
	//! Triggers registration of FlowGraph nodes.
	virtual void RegisterFlowGraphNodes();
	//! Shuts down Mono run-time environment.
	virtual void Shutdown();
#pragma endregion
#pragma region Interaction with Run-Time
	//! Handles exception that occurred during managed method invocation.
	virtual void HandleException(mono::exception exception);
	//! Registers a new internal call.
	virtual void AddInternalCall(const char *name, const char *className, const char *nameSpace, void *functionPointer);
#pragma endregion
#pragma region Listeners
	//! Registers new object that receives notifications about CryCIL events.
	virtual void AddListener(IMonoSystemListener *listener);
	//! Unregisters an object that receives notifications about CryCIL events.
	virtual void RemoveListener(IMonoSystemListener *listener);
#pragma endregion
#pragma region IGameFrameworkListener Implementation.
	//! Triggers Update event in MonoInterface object in Cryambly.
	virtual void OnPostUpdate(float fDeltaTime);
	//! Not used.
	virtual void OnSaveGame(ISaveGame* pSaveGame);
	//! Not used.
	virtual void OnLoadGame(ILoadGame* pLoadGame);
	//! Not used.
	virtual void OnLevelEnd(const char* nextLevel);
	//! Not used.
	virtual void OnActionEvent(const SActionEvent& event);
#pragma endregion
#pragma region ISystemEventListener Implementation
	//! Reacts to system events.
	//!
	//! @param event  Identifier of the event.
	//! @param wparam First parameter that can supply extra information about the event.
	//! @param lparam Second parameter that can supply extra information about the event.
	virtual void OnSystemEvent(ESystemEvent event, UINT_PTR wparam, UINT_PTR lparam);
#pragma endregion
private:
#pragma region Default Listeners
	void RegisterDefaultListeners();
#pragma endregion
#pragma region Thunks Initialization
	void InitializeThunks();
	void InitializeMonoInterfaceThunks();
	void InitializeAssemblyCollectionThunks();
	template<typename MethodSignature>
	MethodSignature GetMethodThunk(IMonoAssembly *assembly, const char *nameSpace, const char *className, const char *methodName, const char *params);
#pragma endregion
};