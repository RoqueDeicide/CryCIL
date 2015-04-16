#pragma once

#include "IMonoAliases.h"
#include "IMonoSystemListener.h"

//! Base class for a template class that defines interface for Mono interops.
//!
//! This class is here to reduce amount of code that has to be duplicated when
//! specializing aforementioned template.
struct IMonoInteropBase : public IMonoSystemListener
{
	//! Registers an interop method.
	//!
	//! @param methodName      Name of the method to register.
	//! @param functionPointer Pointer to the method that will be called internally.
	virtual void RegisterInteropMethod(const char *methodName, void *functionPointer)
	{
		this->monoInterface->Functions->AddInternalCall
			(this->GetNameSpace(), this->GetName(), methodName, functionPointer);
	}
	//! Returns the name of the class that will declare managed counter-parts
	//! of the internal calls.
	virtual const char *GetName() = 0;
	//! Returns the name space where the class that will declare managed counter-parts
	//! of the internal calls is declared.
	virtual const char *GetNameSpace() = 0;
	//! Unnecessary for most interops.
	virtual void OnPreInitialization()
	{}
	//! Unnecessary for most interops.
	virtual void OnRunTimeInitializing() {}
	//! Unnecessary for most interops.
	virtual void OnCryamblyInitilizing() {}
	//! Unnecessary for most interops.
	virtual void OnCompilationStarting()
	{}
	//! Unnecessary for most interops.
	virtual void OnCompilationComplete(bool success)
	{}
	//! Unnecessary for most interops.
	virtual List<int> *GetSubscribedStages()
	{
		return nullptr;
	}
	//! Unnecessary for most interops.
	virtual void OnInitializationStage(int stageIndex)
	{}
	//! Unnecessary for most interops.
	virtual void OnCryamblyInitilized()
	{}
	//! Unnecessary for most interops.
	virtual void OnPostInitialization()
	{}
	//! Unnecessary for most interops.
	virtual void Update()
	{}
	//! Unnecessary for most interops.
	virtual void PostUpdate()
	{}
	//! Unnecessary for most interops.
	virtual void Shutdown()
	{}
};

//! Interface of objects that specialize on setting up interops between C++ and Mono.
//!
//! The earliest time for registration of internal calls is during invocation
//! of OnRunTimeInitialized, which is why it is kept abstract.
//!
//! @typeparam callRegistrationOnly Indicates whether this interop object will unregister
//!                                 and destroy itself after adding internal calls to Mono.
//! @typeparam useMonoEnv Indicates whether this interop object will override SetInterface
//!                       to not save IMonoInterface implementation to the internal field.
template<bool callRegistrationOnly, bool useMonoEnv = false>
struct IMonoInterop : public IMonoInteropBase
{};
//! Specialization of IMonoInterop<,> template that behaves in a default manner.
template<> struct IMonoInterop<false, false> : public IMonoInteropBase
{
};
//! Specialization of IMonoInterop<,> template that unregisters and destroys itself
//! after registration of internal calls.
template<> struct IMonoInterop<true, false> : public IMonoInteropBase
{
	//! Unregisters itself and commits suicide.
	virtual void OnCryamblyInitilizing()
	{
		this->monoInterface->RemoveListener(this);
		delete this;
	}
};
//! Specialization of IMonoInterop<,> template that relies on using MonoEnv variable
//! instead of internal field.
template<> struct IMonoInterop < false, true > : public IMonoInteropBase
{
	//! No saving.
	virtual void SetInterface(IMonoInterface *handle) {}
	//! Registers internal calls through MonoEnv, since internal field is a null pointer.
	virtual void RegisterInteropMethod(const char *methodName, void *functionPointer)
	{
		MonoEnv->Functions->AddInternalCall
			(this->GetNameSpace(), this->GetName(), methodName, functionPointer);
	}
};

#define REGISTER_METHOD(method) this->RegisterInteropMethod(#method, method)
#define REGISTER_CTOR(method) this->RegisterInteropMethod(".ctor", method)

//! Interface of interops that use classes within CryCil.Interops name space.
template<bool callRegistrationOnly>
struct IDefaultMonoInterop : public IMonoInterop < callRegistrationOnly, true >
{
	virtual const char *GetNameSpace() { return "CryCil.Interops"; }
};


//! Specialization of IMonoInterop<,> template that relies on using MonoEnv variable
//! instead of internal field and unregisters and destroys itself after registration
//! of internal calls.
template<> struct IMonoInterop < true, true > : public IMonoInteropBase
{
	//! No saving.
	virtual void SetInterface(IMonoInterface *handle) {}
	//! Unregisters itself and commits suicide.
	virtual void OnCryamblyInitilizing()
	{
		MonoEnv->RemoveListener(this);
		delete this;
	}
	//! Registers internal calls through MonoEnv, since internal field is a null pointer.
	virtual void RegisterInteropMethod(const char *methodName, void *functionPointer)
	{
		MonoEnv->Functions->AddInternalCall
			(this->GetNameSpace(), this->GetName(), methodName, functionPointer);
	}
};