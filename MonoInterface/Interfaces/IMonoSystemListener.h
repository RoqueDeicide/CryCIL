#pragma once

#include "IMonoAliases.h"

//! CryCIL uses initialization stages to allow various systems perform various initialization
//! tasks in a very specific order.
//!
//! This define marks indices used by CryCIL.
#define DEFAULT_INITIALIZATION_STAGE

//! Index of the initialization stage during which entities defined in CryCIL are registered.
#define ENTITY_REGISTRATION_STAGE DEFAULT_INITIALIZATION_STAGE 1000000
//! Index of the initialization stage during which actors defined in CryCIL are registered.
#define ACTORS_REGISTRATION_STAGE DEFAULT_INITIALIZATION_STAGE 2000000
//! Index of the initialization stage during which game modes defined in CryCIL are registered.
#define GAME_MODE_REGISTRATION_STAGE DEFAULT_INITIALIZATION_STAGE 3000000
//! Index of the initialization stage during which data required to register CryCIL flow nodes
//! is gathered.
#define FLOWNODE_RECOGNITION_STAGE DEFAULT_INITIALIZATION_STAGE 4000000

//! Base interface for objects that subscribe to the events raised by IMonoInterface.
//!
//! Listeners receive events in the order of registration. Internal listeners are always
//! registered first.
//!
//! @example DoxygenExampleFiles\ListenerExample.h
struct IMonoSystemListener
{
protected:
	IMonoInterface *monoInterface;
public:
	//! Allows IMonoInterface implementation let the listener access it before global
	//! variable MonoEnv is initialized.
	//!
	//! @param handle Pointer to IMonoInterface implementation that can be used.
	virtual void SetInterface(IMonoInterface *handle)
	{
		this->monoInterface = handle;
	}
	//! Invoked before Mono interface is initialized.
	//!
	//! IMonoInterface object is not usable at this stage.
	virtual void OnPreInitialization() = 0;
	//! Invoked before Mono run-time initialization begins.
	//!
	//! CryCIL Mono run-time API is not usable at this stage.
	virtual void OnRunTimeInitializing() = 0;
	//! Invoked after Mono run-time is initialized.
	//!
	//! Cryambly is loaded at this point and Mono is running: CryCIL API can be used now.
	virtual void OnRunTimeInitialized() = 0;
	//! Invoked before MonoInterface object defined in Cryambly is initialized.
	virtual void OnCryamblyInitilizing() = 0;
	//! Invoked before Cryambly attempts to compile game code.
	virtual void OnCompilationStarting() = 0;
	//! Invoked after Cryambly finishes compilation of game code.
	//!
	//! @param success Indicates whether compilation was successful.
	virtual void OnCompilationComplete(bool success) = 0;
	//! Invoked when this listener is registered to get indices of initialization stages
	//! this listener would like to subscribe to.
	//!
	//! Resultant list will be deleted after use, so return a deep copy, if you want it
	//! to be used somewhere else.
	//!
	//! @returns A pointer to a list of integer numbers that represent indices of
	//!          initialization stages this listener wants to subscribe to.
	//!          Null can be return if the listener doesn't want to subscribe to anything.
	virtual List<int> *GetSubscribedStages() = 0;
	//! Invoked when one of initialization stages this listener has subscribed to begins.
	//!
	//! @param stageIndex Zero-based index of the stage.
	virtual void OnInitializationStage(int stageIndex) = 0;
	//! Invoked after MonoInterface object defined in Cryambly is initialized.
	virtual void OnCryamblyInitilized() = 0;
	//! Invoked after all initialization of CryCIL is complete.
	virtual void OnPostInitialization() = 0;
	//! Invoked when logical frame of CryCIL subsystem starts.
	virtual void Update() = 0;
	//! Invoked when logical frame of CryCIL subsystem ends.
	virtual void PostUpdate() = 0;
	//! Invoked when CryCIL shuts down.
	virtual void Shutdown() = 0;
};