#pragma once

#include "IMonoInterface.h"
#include "List.h"
//! Broadcasts events to listeners.
struct EventBroadcaster
{
	List<IMonoSystemListener *> listeners;
	// Map is basically a SortedList.
	std::map<int, List<IMonoSystemListener *> *> stageMap;
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
};