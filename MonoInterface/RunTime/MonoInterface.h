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
{
	friend InitializationInterop;
private:
	std::vector<MonoAssemblyWrapper *> assemblies;
	EventBroadcaster *broadcaster;

	bool running;

	MonoDomain *appDomain;
	IMonoAssembly *cryambly;					//! Extra pointer for Cryambly.
	IMonoAssembly *corlib;						//! Extra pointer for mscorlib.

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
		// Load Cryambly.
		this->cryambly = this->LoadAssembly(DirectoryStructure::GetCryamblyFile());
		this->corlib = this->WrapAssembly(mono_get_corlib());

		this->InitializeThunks();
		
		this->broadcaster->OnRunTimeInitialized();
		// Initialize an instance of type MonoInterface.
		this->managedInterface =
			this->CreateObject(this->cryambly, "CryCil.RunTime", "MonoInterface", true, false);

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
		throw std::logic_error("The method or operation is not implemented.");
	}

	virtual void Shutdown()
	{
		throw std::logic_error("The method or operation is not implemented.");
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
	//! Wraps already existing Mono array.
	//!
	//! @param arrayHandle Pointer to the array that needs to be wrapped.
	//! @param persistent  Indicates whether the array wrapping must be safe to
	//!                    keep a reference to for prolonged periods of time.
	virtual IMonoArray *WrapArray(mono::object arrayHandle, bool persistent)
	{
		if (persistent)
		{
			return new MonoArrayPersistent(arrayHandle);
		}
		return new MonoArrayFree(arrayHandle);
	}

	virtual void HandleException(mono::object exception)
	{
		throw std::logic_error("The method or operation is not implemented.");
	}
	//! Registers a method as internal call.
	virtual void AddInternalCall(const char *name, void *functionPointer)
	{
		if (this->running)
		{
			mono_add_internal_call(name, functionPointer);
		}
	}
	virtual IMonoAssembly *LoadAssembly(const char *moduleFileName)
	{
		MonoAssembly *assembly = mono_domain_assembly_open(this->appDomain, moduleFileName);
		MonoAssemblyWrapper *wrapper = new MonoAssemblyWrapper(assembly);
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
		MonoAssemblyWrapper *wrapper = new MonoAssemblyWrapper(assemblyHandle);
		this->assemblies.push_back(wrapper);
		return wrapper;
	}
	//! Returns a pointer to app domain.
	virtual void * GetAppDomain()
	{
		return this->appDomain;
	}

	virtual IMonoAssembly * GetCryambly()
	{
		throw std::logic_error("The method or operation is not implemented.");
	}

	virtual IMonoAssembly * GetPdbMdbAssembly()
	{
		throw std::logic_error("The method or operation is not implemented.");
	}

	virtual IMonoAssembly * GetCoreLibrary()
	{

	}

	virtual bool GetInitializedIndication()
	{
		return this->running;
	}

	virtual IDefaultBoxinator * GetDefaultBoxer()
	{
		throw std::logic_error("The method or operation is not implemented.");
	}
private:
	void RegisterDefaultListeners()
	{
#ifdef _DEBUG
		this->listeners.push_back(new DebugEventReporter());
#endif // _DEBUG

	}
	void InitializeThunks()
	{
		this->InitializeClassThunks();
	}
	void InitializeClassThunks()
	{
		MonoImage *coreImage =
			mono_assembly_get_image((MonoAssembly *)this->CoreLibrary->GetWrappedPointer());
		// Register Activator.CreateInstance thunk.
		MonoMethodDesc *createInstanceDesc =
			mono_method_desc_new("System.Activator:CreateInstance(System.Type)", true);
		MonoMethod *createInstanceMethod =
			mono_method_desc_search_in_image(createInstanceDesc, coreImage);
		MonoClassThunks::CreateInstance =
			(CreateInstanceThunk)mono_method_get_unmanaged_thunk(createInstanceMethod);
		// Register Object.Equals thunk.
		MonoMethodDesc *staticEqualsDesc =
			mono_method_desc_new("System.Object:Equals(System.Object,System.Object)", true);
		MonoMethod *staticEqualsMethod =
			mono_method_desc_search_in_image(staticEqualsDesc, coreImage);
		MonoClassThunks::StaticEquals =
			(StaticEqualsThunk)mono_method_get_unmanaged_thunk(staticEqualsMethod);
	}
};