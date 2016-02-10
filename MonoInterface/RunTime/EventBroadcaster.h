#pragma once

#include "IMonoInterface.h"
#include "SortedList.h"

//! Represents a signature of most member functions in IMonoSystemListener struct.
typedef void(IMonoSystemListener::*SimpleEventHandler)();

//! Broadcasts events to listeners.
struct EventBroadcaster
{
private:
	//! Indicates whether we are going through initialization stages. During such stages listeners cannot be removed.
	bool stages;
	//! An index that is used when going through the listeners in order to be able to adjust it when listener
	//! is removed.
	int index;
public:
	List<IMonoSystemListener *> *listeners;
	SortedList<int, List<IMonoSystemListener *> *> *stageMap;
	//! Initializes event broadcaster.
	EventBroadcaster();
	~EventBroadcaster();
	//! Removes a listener from broadcasting list and stages map.
	void RemoveListener(IMonoSystemListener *listener);
	//! Gives listeners a pointer to IMonoInterface.
	void SetInterface(IMonoInterface *inter);
	//! Broadcasts PreInitialization event.
	void OnPreInitialization();
	//! Broadcasts RunTimeInitializing event.
	void OnRunTimeInitializing();
	//! Broadcasts RunTimeInitialized event.
	void OnRunTimeInitialized();
	//! Broadcasts CryamblyInitilizing event.
	void OnCryamblyInitilizing();
	//! Broadcasts CompilationStarting event.
	void OnCompilationStarting();
	//! Broadcasts CompilationComplete event.
	void OnCompilationComplete(bool success);
	//! Gathers initialization stages data for sending it to managed code.
	int *GetSubscribedStagesInfo(int &stageCount);
	//! Broadcasts InitializationStage event.
	//!
	//! @param stageIndex Index of initialization stage that is happening.
	void OnInitializationStage(int stageIndex);
	//! Broadcasts CryamblyInitilized event.
	void OnCryamblyInitilized();
	//! Broadcasts PostInitialization event.
	void OnPostInitialization();
	//! Broadcasts Update event.
	void Update();
	//! Broadcasts PostUpdate event.
	void PostUpdate();
	//! Broadcasts Shutdown event.
	void Shutdown();
private:
	// Used for propagating events that don't have any extra data.
	void SendSimpleEvent(SimpleEventHandler handler);
	// Used for propagating events Update and PostUpdate. This function is the same as SendSimpleEvent
	// but it doesn't have as much debug code.
	void SendUpdateEvent(SimpleEventHandler handler);
};