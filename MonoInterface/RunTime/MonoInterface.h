#pragma once

#include "stdafx.h"

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
	List<MonoAssemblyWrapper *> assemblies;
	EventBroadcaster *broadcaster;

	bool running;

	MonoDomain *appDomain;
	IMonoAssembly *cryambly;					//! Extra pointer for Cryambly.
	IMonoAssembly *corlib;						//! Extra pointer for mscorlib.
	IMonoAssembly *pdb2mdb;
	DefaultBoxinator boxer;

	IMonoHandle *managedInterface;
#pragma endregion
public:
#pragma region Property Methods
	//! Returns a pointer to app domain.
	virtual void *GetAppDomain();

	virtual IMonoAssembly *GetCryambly();

	virtual IMonoAssembly *GetPdbMdbAssembly();

	virtual IMonoAssembly *GetCoreLibrary();

	virtual bool GetInitializedIndication();

	virtual IDefaultBoxinator *GetDefaultBoxer();
#pragma endregion
#pragma region Construction
	//! Initializes Mono run-time environment.
	//!
	//! @param framework Pointer to IGameFramework object that cannot be obtained in any other way.
	MonoInterface(IGameFramework *framework, IMonoSystemListener **listeners, int listenerCount);
	~MonoInterface();
#pragma endregion
#pragma region External Triggers
	//! Triggers registration of FlowGraph nodes.
	//!
	//! @remark Call this method from Game::RegisterGameFlowNodes function.
	virtual void RegisterFlowGraphNodes();
	//! Shuts down Mono run-time environment.
	//!
	//! @remark Call this method from GameStartup destructor.
	virtual void Shutdown();
#pragma endregion
#pragma region String Conversions
	//! Converts given null-terminated string to Mono managed object.
	virtual mono::string ToManagedString(const char *text);
	//! Converts given managed string to null-terminated one.
	virtual const char *ToNativeString(mono::string text);
#pragma endregion
#pragma region Objects and Arrays
	//! Creates a new wrapped MonoObject using constructor with specific parameters.
	//!
	//! @param assembly   Assembly where the type of the object is defined.
	//! @param name_space Name space that contains the type of the object.
	//! @param class_name Name of the type to use.
	//! @param persistent Indicates whether handle should keep the object away from GC.
	//! @param pinned     Indicates whether the object's location
	//!                   in the managed heap must be kept constant.
	//! @param params     An array of parameters to pass to the constructor.
	//!                   If null, default constructor will be used.
	virtual IMonoHandle *CreateObject(IMonoAssembly *assembly, const char *name_space, const char *class_name, bool persistent = false, bool pinned = false, IMonoArray *params = nullptr);
	//! Creates a new Mono handle wrapper for given MonoObject.
	//!
	//! @param obj        An object to make persistent.
	//! @param persistent Indicates whether handle should keep the object away from GC.
	//! @param pinned     Indicates whether the object's location
	//!                   in the managed heap must be kept constant.
	virtual IMonoHandle *WrapObject(mono::object obj, bool persistent = false, bool pinned = false);
	//! Creates object of type object[] with specified capacity.
	//!
	//! @param capacity   Number of elements that can be held by the array.
	//! @param persistent Indicates whether the array must be safe to
	//!                   keep a reference to for prolonged periods of time.
	virtual IMonoArray *CreateArray(int capacity, bool persistent);
	//! Creates object of specified type with specified capacity.
	//!
	//! @param klass      Pointer to the class that will represent objects
	//!                   within the array.
	//! @param capacity   Number of elements that can be held by the array.
	//! @param persistent Indicates whether the array must be safe to
	//!                   keep a reference to for prolonged periods of time.
	virtual IMonoArray *CreateArray(IMonoClass *klass, int capacity, bool persistent);
	//! Wraps already existing Mono array.
	//!
	//! @param arrayHandle Pointer to the array that needs to be wrapped.
	//! @param persistent  Indicates whether the array wrapping must be safe to
	//!                    keep a reference to for prolonged periods of time.
	virtual IMonoArray * WrapArray(mono::Array arrayHandle, bool persistent);
#pragma endregion
#pragma region Interaction with Run-Time
	//! Handles exception that occurred during managed method invocation.
	//!
	//! @param exception Exception object to handle.
	virtual void HandleException(mono::exception exception);
	//! Registers a new internal call.
	//!
	//! @param nameSpace       Name space where the class is located.
	//! @param className       Name of the class where managed method is declared.
	//! @param name            Name of the managed method.
	//! @param functionPointer Pointer to unmanaged thunk that needs to be exposed to Mono code.
	virtual void AddInternalCall(const char *name, const char *className, const char *nameSpace, void *functionPointer);
#pragma endregion
#pragma region Assemblies
	//! Loads a Mono assembly into memory.
	//!
	//! @param moduleFileName Name of the file inside Modules folder.
	virtual IMonoAssembly *LoadAssembly(const char *moduleFileName);
	//! Wraps assembly pointer.
	//!
	//! @param assemblyHandle Pointer to MonoAssembly to wrap.
	virtual IMonoAssembly *WrapAssembly(void *assemblyHandle);
#pragma endregion
#pragma region Unboxing
	//! Unboxes managed value-type object.
	//!
	//! @param value Value-type object to unbox.
	virtual void *Unbox(mono::object value);
#pragma endregion
#pragma region Listeners
	//! Registers new object that receives notifications about CryCIL events.
	//!
	//! @param listener Pointer to the object that implements IMonoSystemListener.
	virtual void AddListener(IMonoSystemListener *listener);
	//! Unregisters an object that receives notifications about CryCIL events.
	//!
	//! @param listener Pointer to the object that implements IMonoSystemListener.
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
	void InitializeClassThunks();
	void InitializeMonoInterfaceThunks();
	template<typename MethodSignature>
	MethodSignature GetMethodThunk(IMonoAssembly *assembly, const char *nameSpace, const char *className, const char *methodName, const char *params);
#pragma endregion
};