#pragma once

#include "IMonoInterface.h"
#include "SortedList.h"

//! Represents a signature of most member functions in IMonoSystemListener struct.
typedef void(IMonoSystemListener::*SimpleEventHandler)();

//! Broadcasts events to listeners.
struct EventBroadcaster
{
private:
	//! Value1 in each pair is an index of the removed listener in the main list. Value2 is a pointer itself and it's used
	//! to skip removed listeners during initialization stages.
	List<Pair<int, IMonoSystemListener *>> latestRemovedListeners;
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
	// Corrects the given index to account for listeners that were unregistered during the event.
	void CorrectIndex(int &index);
};