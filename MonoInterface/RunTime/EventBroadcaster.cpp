#include "stdafx.h"
#include "EventBroadcaster.h"

#if 0
  #define EventMessage CryLogAlways
#else
  #define EventMessage(...) void(0)
#endif

EventBroadcaster::EventBroadcaster()
	: listeners(20)
	, listenersToRemove(5)
{}


void EventBroadcaster::RemoveListener(IMonoSystemListener *listener)
{
	EventMessage("Scheduling removal of a listener.");

	this->listenersToRemove.Add(listener);
}

void EventBroadcaster::SetInterface(IMonoInterface *inter)
{
	for (auto currentListener : this->listeners)
	{
		currentListener->SetInterface(inter);
	}

	this->ClearRemovedListeners();
}

//! Broadcasts PreInitialization event.
void EventBroadcaster::OnPreInitialization()
{
	this->SendSimpleEvent(&IMonoSystemListener::OnPreInitialization);
}
//! Broadcasts RunTimeInitializing event.
void EventBroadcaster::OnRunTimeInitializing()
{
	this->SendSimpleEvent(&IMonoSystemListener::OnRunTimeInitializing);
}
//! Broadcasts RunTimeInitialized event.
void EventBroadcaster::OnRunTimeInitialized()
{
	this->SendSimpleEvent(&IMonoSystemListener::OnRunTimeInitialized);
}
//! Broadcasts CryamblyInitilizing event.
void EventBroadcaster::OnCryamblyInitilizing()
{
	this->SendSimpleEvent(&IMonoSystemListener::OnCryamblyInitilizing);
}
//! Broadcasts CompilationStarting event.
void EventBroadcaster::OnCompilationStarting()
{
	this->SendSimpleEvent(&IMonoSystemListener::OnCompilationStarting);
}
//! Broadcasts CompilationComplete event.
void EventBroadcaster::OnCompilationComplete(bool success)
{
	for (auto currentListener : this->listeners)
	{
		currentListener->OnCompilationComplete(success);
	}

	this->ClearRemovedListeners();
}
//! Gathers initialization stages data for sending it to managed code.
int *EventBroadcaster::GetSubscribedStagesInfo(int &stageCount)
{
	EventMessage("Gathering information about the native initialization stages.");

	// Gather information about all stages.
	for (auto currentListener : this->listeners)
	{
		// Get the stages.
		List<int> stages = currentListener->GetSubscribedStages();

		// Put the stages into the map.
		for (auto currentStageIndex : stages)
		{
			auto &stageList = this->stageMap.Ensure(currentStageIndex, List<IMonoSystemListener *>(10));

			stageList.Add(currentListener);
		}
	}

	this->ClearRemovedListeners();

	// Get the array of stage indices to pass to managed code that is going to invoke the stages.
	stageCount = this->stageMap.Length;

	int *stageIndices = new int[stageCount];
	int  i            = 0;

	for (auto current = this->stageMap.ascend(); current != this->stageMap.top(); ++current)
	{
		stageIndices[i++] = current.Key();
	}

	return stageIndices;
}
//! Broadcasts InitializationStage event.
//!
//! @param stageIndex Index of initialization stage that is happening.
void EventBroadcaster::OnInitializationStage(int stageIndex)
{
	CryLogAlways("Commencing initialization stage #%d", stageIndex);

	List<IMonoSystemListener *> stageList;
	if (this->stageMap.TryGet(stageIndex, stageList))
	{
		for (auto stageListener : stageList)
		{
			stageListener->OnInitializationStage(stageIndex);
		}
	}

	this->ClearRemovedListeners();
}
//! Broadcasts CryamblyInitilized event.
void EventBroadcaster::OnCryamblyInitilized()
{
	this->SendSimpleEvent(&IMonoSystemListener::OnCryamblyInitilized);
}
//! Broadcasts PostInitialization event.
void EventBroadcaster::OnPostInitialization()
{
	this->SendSimpleEvent(&IMonoSystemListener::OnPostInitialization);
}
//! Broadcasts Update event.
void EventBroadcaster::Update()
{
	this->SendUpdateEvent(&IMonoSystemListener::Update);
}
//! Broadcasts PostUpdate event.
void EventBroadcaster::PostUpdate()
{
	this->SendUpdateEvent(&IMonoSystemListener::PostUpdate);
}
//! Broadcasts Shutdown event.
void EventBroadcaster::Shutdown()
{
	this->SendSimpleEvent(&IMonoSystemListener::Shutdown);
}

#if 1
#define DebugSimpleEvents
#endif

void EventBroadcaster::SendSimpleEvent(SimpleEventHandler handler)
{
	EventMessage("Broadcasting the event.");

	const int printEvery = 1;
	size_t current = 0;

	for (auto currentListener : this->listeners)
	{
#ifdef DebugSimpleEvents
		if (current % printEvery == 0)
		{
			EventMessage("Sending the event to the listener #%d.", current);
		}
#endif

		(currentListener->*handler)();

#ifdef DebugSimpleEvents
		if (current % printEvery == 0)
		{
			EventMessage("Sent the event to the listener #%d.", current);
		}
#endif

		current++;
	}

	this->ClearRemovedListeners();
}

void EventBroadcaster::SendUpdateEvent(SimpleEventHandler handler)
{
	for (auto currentListener : this->listeners)
	{
		(currentListener->*handler)();
	}

	this->ClearRemovedListeners();
}

void EventBroadcaster::ClearRemovedListeners()
{
	EventMessage("Removing listeners that were scheduled for deletion.");
	
	for (const auto &currentListenerToRemove : this->listenersToRemove)
	{
		// Delete the listener from the main list.
		if (auto listenerToRemove = this->listeners.Find(currentListenerToRemove))
		{
			this->listeners.Erase(listenerToRemove);
		}

		// Clear it from the stage map.
		for (auto current = this->stageMap.ascend(); current != this->stageMap.top(); ++current)
		{
			auto &currentListOfSubs = current.Value();
			if (auto currentSubToRemove = currentListOfSubs.Find(currentListenerToRemove))
			{
				currentListOfSubs.Erase(currentSubToRemove);
			}

			if (currentListOfSubs.IsEmpty())
			{
				this->stageMap.Remove(current--.Key());
			}
		}
	}
}
