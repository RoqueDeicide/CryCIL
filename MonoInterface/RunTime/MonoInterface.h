#pragma once

#include "stdafx.h"

#include "Engine/DirectoryStructure.h"
#include "MonoHeaders.h"

#include <cstdlib>
#include <csignal>

#include "ThunkTables.h"

#include "Wrappers/MonoArrayFree.h"
#include "Wrappers/MonoArrayPersistent.h"

#include "Wrappers/MonoHandleFree.h"

#include "Wrappers/MonoAssemblyWrapper.h"

#include "Wrappers/DefaultBoxinator.h"

#include "RunTime/DebugEventReporter.h"
#include "RunTime/EventBroadcaster.h"
#include "RunTime/AllInterops.h"

void HandleSignalAbort(int error)
{
	CryLogAlways("Aborted %i", error);
}

//! Handles interface between CryEngine and Mono.
class MonoInterface
	: public IMonoInterface
	, public IGameFrameworkListener
	, public ISystemEventListener
{
	friend InitializationInterop;
private:
	std::vector<MonoAssemblyWrapper *> assemblies;
	EventBroadcaster *broadcaster;

	bool running;

	MonoDomain *appDomain;
	IMonoAssembly *cryambly;					//! Extra pointer for Cryambly.
	IMonoAssembly *corlib;						//! Extra pointer for mscorlib.
	IMonoAssembly *pdb2mdb;
	DefaultBoxinator boxer;

	IMonoHandle *managedInterface;
public:
	//! Initializes Mono run-time environment.
	//!
	//! @param framework Pointer to IGameFramework object that cannot be obtained in any other way.
	MonoInterface(IGameFramework *framework, IMonoSystemListener **listeners, int listenerCount)
		: running(false)
		, appDomain(nullptr)
		, cryambly(nullptr)
		, assemblies(10)
		, broadcaster(nullptr)
	{
		broadcaster = new EventBroadcaster();
		// Register all initial listeners.
		this->RegisterDefaultListeners();
		if (listeners)
		{
			for (int i = 0; i < listenerCount; i++)
			{
				this->broadcaster->listeners.push_back(listeners[i]);
			}
		}
		// Set global variables.
		Framework = framework;
		MonoEnv = this;
		
		this->broadcaster->OnPreInitialization();

		this->broadcaster->OnRunTimeInitializing();
		// Tell Mono, where to look for the libraries and configuration files.
		mono_set_dirs
			(DirectoryStructure::GetMonoLibraryFolder(),
			DirectoryStructure::GetMonoConfigurationFolder());
#ifdef _DEBUG
		// Tell Mono to crash the game if there is an exception that wasn't handled.
		mono_set_signal_chaining(true);
		// Load up mdb files.
		mono_debug_init(MONO_DEBUG_FORMAT_MONO);
#endif // _DEBUG
		// Register HandleSignalAbort function as a handler for SIGABRT.
		signal(SIGABRT, HandleSignalAbort);
		// Initialize the AppDomain.
		this->appDomain = mono_jit_init_version("CryCIL", "v4.0.30319");
		if (!this->appDomain)
		{
			CryFatalError("Unable to initialize Mono AppDomain.");
		}
		// Manually tell AppDomain where to find the configuration for itself.
		// Not calling this function results in exception when trying to get CodeDomProvider for C#.
		mono_domain_set_config
		(
			this->appDomain,
			DirectoryStructure::GetMonoBinariesFolder(),
			DirectoryStructure::GetMonoAppDomainConfigurationFile()
		);
#ifdef _DEBUG
		// Load Pdb2Mdb.dll before everything else.
		this->pdb2mdb = this->LoadAssembly(DirectoryStructure::GetPdb2MdbFile());
		// Initialize conversion thunk immediately.
		Pdb2MdbThunks::Convert = (this->pdb2mdb)
			? this->GetMethodThunk<ConvertPdbThunk>
			(this->pdb2mdb, "", "Driver", "Convert", "string")
			: nullptr;
#endif // _DEBUG

		// Load Cryambly.
		this->cryambly = this->LoadAssembly(DirectoryStructure::GetCryamblyFile());
		this->corlib = this->WrapAssembly(mono_get_corlib());

		this->InitializeThunks();
		
		this->broadcaster->OnRunTimeInitialized();
		// Initialize an instance of type MonoInterface.
		mono::exception ex;
		this->managedInterface = new MonoHandlePersistent(MonoInterfaceThunks::Initialize(&ex));
		if (ex)
		{
			this->HandleException(ex);
			CryFatalError("CryCil.RunTime.MonoInterface object was not initialized. Cannot continue.");
		}
		Framework->RegisterListener(this, "CryCIL", FRAMEWORKLISTENERPRIORITY_GAME);
		gEnv->pSystem->GetISystemEventDispatcher()->RegisterListener(this);
		this->running = true;
		this->broadcaster->OnPostInitialization();
	}
	~MonoInterface()
	{
		if (running)
		{
			mono_jit_cleanup(this->appDomain);
			this->running = false;
		}
	}

	// IGameFrameworkListener overrides.
	
	//! Triggers Update event in MonoInterface object in Cryambly.
	virtual void OnPostUpdate(float fDeltaTime)
	{
		// Notify everything about the update.
		if (this->monoInterface)
		{
			this->monoInterface->ToWrapper()->CallMethod
			(
				"Think",
				fDeltaTime,
				gEnv->pTimer->GetFrameStartTime().GetMilliSeconds(),
				gEnv->pTimer->GetAsyncTime().GetMilliSeconds(),
				gEnv->pTimer->GetFrameRate(),
				gEnv->pTimer->GetTimeScale()
			);
		}
	}
	//! Not used.
	virtual void OnSaveGame(ISaveGame* pSaveGame) {}
	//! Not used.
	virtual void OnLoadGame(ILoadGame* pLoadGame) {}
	//! Not used.
	virtual void OnLevelEnd(const char* nextLevel) {}
	//! Not used.
	virtual void OnActionEvent(const SActionEvent& event) {}

	virtual void RegisterFlowGraphNodes()
	{
		mono::exception ex;
		MonoInterfaceThunks::TriggerFlowNodesRegistration(this->managedInterface->Get(), &ex);
	}

	virtual void Shutdown()
	{
		this->broadcaster->Shutdown();
		mono::exception ex;
		MonoInterfaceThunks::Shutdown(this->managedInterface->Get(), &ex);
		// Invoke destructor.
		delete this;
	}
	//! Converts given null-terminated string to Mono managed object.
	virtual mono::string ToManagedString(const char *text)
	{
		return (mono::string)mono_string_new(this->appDomain, text);
	}
	//! Converts given managed string to null-terminated one.
	virtual const char *ToNativeString(mono::string text)
	{
		return mono_string_to_utf8((MonoString *)text);
	}
	//! Creates a new wrapped MonoObject using constructor with specific parameters.
	//!
	//! @remark Object that is created by this method can made to be safely keepable by anything.
	//!         Pinning the object in place allows its pointer to be safe to use at any
	//!         time, however it makes GC sessions longer and decreases memory usage efficiency.
	//!
	//! @param assembly   Assembly where the type of the object is defined.
	//! @param name_space Name space that contains the type of the object.
	//! @param class_name Name of the type to use.
	//! @param persistent Indicates whether handle should keep the object away from GC.
	//! @param pinned     Indicates whether the object's location
	//!                   in the managed heap must be kept constant.
	//! @param params     An array of parameters to pass to the constructor.
	//!                   If null, default constructor will be used.
	virtual IMonoHandle *CreateObject(IMonoAssembly *assembly, const char *name_space, const char *class_name, bool persistent = false, bool pinned = false, IMonoArray *params = nullptr)
	{
		return this->WrapObject(assembly->GetClass(class_name, name_space)->CreateInstance(params), persistent, pinned);
		
	}
	//! Creates a new Mono handle wrapper for given MonoObject.
	//!
	//! @remark Making the object persistent allows the object to be accessible from
	//!         native code for prolonged periods of time.
	//!         Pinning the object in place allows its pointer to be safe to use at any
	//!         time, however it makes GC sessions longer and decreases memory usage efficiency.
	//!         Use this method to choose, what to do with results of invoking Mono methods.
	//!
	//! @param obj        An object to make persistent.
	//! @param persistent Indicates whether handle should keep the object away from GC.
	//! @param pinned     Indicates whether the object's location
	//!                   in the managed heap must be kept constant.
	virtual IMonoHandle * WrapObject(mono::object obj, bool persistent = false, bool pinned = false)
	{
		if (pinned)
		{
			return new MonoHandlePinned(obj);
		}
		if (persistent)
		{
			return new MonoHandlePersistent(obj);
		}
		return new MonoHandleFree(obj);
	}
	//! Creates object of type object[] with specified capacity.
	//!
	//! @param capacity   Number of elements that can be held by the array.
	//! @param persistent Indicates whether the array must be safe to
	//!                   keep a reference to for prolonged periods of time.
	virtual IMonoArray * CreateArray(int capacity, bool persistent)
	{
		if (persistent)
		{
			return new MonoArrayPersistent(capacity);
		}
		return new MonoArrayFree(capacity);
	}

	virtual IMonoArray * CreateArray(IMonoClass *klass, int capacity, bool persistent)
	{
		if (persistent)
		{
			return new MonoArrayPersistent(klass, capacity);
		}
		return new MonoArrayFree(klass, capacity);
	}
	//! Wraps already existing Mono array.
	//!
	//! @param arrayHandle Pointer to the array that needs to be wrapped.
	//! @param persistent  Indicates whether the array wrapping must be safe to
	//!                    keep a reference to for prolonged periods of time.
	virtual IMonoArray * WrapArray(mono::Array arrayHandle, bool persistent)
	{
		if (persistent)
		{
			return new MonoArrayPersistent(arrayHandle);
		}
		return new MonoArrayFree(arrayHandle);
	}

	virtual void HandleException(mono::exception exception)
	{
		mono::exception ex;
		MonoInterfaceThunks::DisplayException(exception, &ex);
	}
	//! Registers a method as internal call.
	virtual void AddInternalCall(const char *name, const char *className, const char *nameSpace, void *functionPointer)
	{
		mono_add_internal_call
			(std::string(nameSpace).append(className).append(name).c_str(), functionPointer);
	}

	virtual IMonoAssembly *LoadAssembly(const char *moduleFileName)
	{
		bool failed;
		MonoAssemblyWrapper *wrapper = new MonoAssemblyWrapper(moduleFileName, failed);
		if (failed)
		{
			return nullptr;
		}
		this->assemblies.push_back(wrapper);
		return wrapper;
	}
	//! Wraps assembly pointer.
	virtual IMonoAssembly *WrapAssembly(void *assemblyHandle)
	{
		for each (MonoAssemblyWrapper *wrapper in this->assemblies)
		{
			if (wrapper->GetWrappedPointer() == assemblyHandle)
			{
				return wrapper;
			}
		}
		MonoAssemblyWrapper *wrapper = new MonoAssemblyWrapper((MonoAssembly *)assemblyHandle);
		this->assemblies.push_back(wrapper);
		return wrapper;
	}
	//! Returns a pointer to app domain.
	virtual void *GetAppDomain()
	{
		return this->appDomain;
	}

	virtual IMonoAssembly *GetCryambly()
	{
		return this->cryambly;
	}

	virtual IMonoAssembly *GetPdbMdbAssembly()
	{
		return this->pdb2mdb;
	}

	virtual IMonoAssembly *GetCoreLibrary()
	{
		return this->corlib;
	}

	virtual bool GetInitializedIndication()
	{
		return this->running;
	}

	virtual IDefaultBoxinator *GetDefaultBoxer()
	{
		return &this->boxer;
	}

	virtual void *Unbox(mono::object value)
	{
		return mono_object_unbox((MonoObject *)value);
	}

	virtual void AddListener(IMonoSystemListener *listener)
	{
		this->broadcaster->listeners.push_back(listener);
	}

	virtual void RemoveListener(IMonoSystemListener *listener)
	{
		this->broadcaster->RemoveListener(listener);
	}
private:
	void RegisterDefaultListeners()
	{
#ifdef _DEBUG
		this->broadcaster->listeners.push_back(new DebugEventReporter());
#endif // _DEBUG
		this->broadcaster->listeners.push_back(new InitializationInterop());
	}
	void InitializeThunks()
	{
		this->InitializeClassThunks();
		this->InitializeMonoInterfaceThunks();
		this->InitializeDebugThunks();
	}
	void InitializeClassThunks()
	{
		MonoClassThunks::CreateInstance =
			this->GetMethodThunk<CreateInstanceThunk>
			(this->corlib, "System", "Activator", "CreateInstance", "Type,object[]");
		MonoClassThunks::StaticEquals =
			this->GetMethodThunk<StaticEqualsThunk>
			(this->corlib, "System", "Object", "Equals", "object,object");
	}
	void InitializeMonoInterfaceThunks()
	{
		MonoInterfaceThunks::DisplayException =
			this->GetMethodThunk<DisplayExceptionThunk>
			(this->cryambly, "CryCil.RunTime", "MonoInterface", "DisplayException", "object");
		MonoInterfaceThunks::Initialize =
			this->GetMethodThunk<InitializeThunk>
			(this->cryambly, "CryCil.RunTime", "MonoInterface", "Initialize", nullptr);
		MonoInterfaceThunks::TriggerFlowNodesRegistration =
			this->GetMethodThunk<RegisterFlowNodesThunk>
			(this->cryambly, "CryCil.RunTime", "MonoInterface", "RegisterFlowGraphNodeTypes", nullptr);
		MonoInterfaceThunks::Shutdown =
			this->GetMethodThunk<ShutDownThunk>
			(this->cryambly, "CryCil.RunTime", "MonoInterface", "Shutdown", nullptr);
	}
	void InitializeDebugThunks()
	{
	}
	template<typename MethodSignature>
	MethodSignature GetMethodThunk(IMonoAssembly *assembly, const char *nameSpace, const char *className, const char *methodName, const char *params)
	{
		return (MethodSignature)assembly->MethodFromDescription
			(nameSpace, className, methodName, params)->UnmanagedThunk;
	}

	virtual void OnSystemEvent(ESystemEvent event, UINT_PTR wparam, UINT_PTR lparam)
	{
		switch (event)
		{
		case ESYSTEM_EVENT_SHUTDOWN:
			this->Shutdown();
			break;
		default:
			break;
		}
	}
};