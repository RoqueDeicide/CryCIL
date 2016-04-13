#include "stdafx.h"
#include "EventBroadcaster.h"

#if 0
  #define EventMessage CryLogAlways
#else
  #define EventMessage(...) void(0)
#endif

EventBroadcaster::EventBroadcaster()
	: stages(false)
	, index(-1)
	, listeners(20)
	, stageMap(50)
{}


void EventBroadcaster::RemoveListener(IMonoSystemListener *listener)
{
	EventMessage("Removing a listener.");
	// Delete the listener from the main list.
	int listenerIndex = -1;
	for (int i = 0; i < this->listeners.Length; i++)
	{
		if (this->listeners[i] == listener)
		{
			listenerIndex = i;
			break;
		}
	}

	if (listenerIndex == -1)
	{
		return;
	}

	if (this->stages)
	{
		CryFatalError("Cannot remove a listener #%d during the initialization stage.", listenerIndex);
	}

	if (this->index <= listenerIndex && this->index != -1)
	{
		// Adjust the current index.
		this->index--;
	}

	this->listeners.Erase(listenerIndex);

	// Remove listener from stage map.
	this->stageMap.ForEach
	(
		[&listener](int stageIndex, List<IMonoSystemListener *> &subscribers)
		{
			for (int i = 0; i < subscribers.Length; i++)
			{
				if (subscribers[i] == listener)
				{
					subscribers.Erase(i);
					break;      // No need to proceed as there can only be one unique listener per stage.
				}
			}
		}
	);
}

void EventBroadcaster::SetInterface(IMonoInterface *inter)
{
	for (this->index = 0; this->index < this->listeners.Length; this->index++)
	{
		this->listeners[this->index]->SetInterface(inter);
	}
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
	for (this->index = 0; this->index < this->listeners.Length; this->index++)
	{
		this->listeners[this->index]->OnCompilationComplete(success);
	}
}
//! Gathers initialization stages data for sending it to managed code.
int *EventBroadcaster::GetSubscribedStagesInfo(int &stageCount)
{
	EventMessage("Going through listeners.");
	// Gather information about all stages.
	for (this->index = 0; this->index < this->listeners.Length; this->index++)
	{
		EventMessage("Getting stage indices for the listener #%d.", this->index + 1);
		// Get stages.
		List<int> *stages = this->listeners[this->index]->GetSubscribedStages();
		if (stages)
		{
			// Put the listeners into the map.
			for (int j = 0; j < stages->Length; j++)
			{
				int currentStageIndex = (*stages)[j];
				if (!this->stageMap.Contains(currentStageIndex))
				{
					this->stageMap.Add(currentStageIndex, List<IMonoSystemListener *>(10));
				}
				this->stageMap[currentStageIndex].Add(this->listeners[this->index]);
			}
			delete stages;
		}
		else
		{
			EventMessage("Listener #%d is not subscribed to any stages.", this->index + 1);
		}
	}
	// Get the stage indices.
	stageCount = this->stageMap.Length;
	int *stageIndices = new int[stageCount];
	int  i            = 0;
	this->stageMap.ForEach([stageIndices, &i](int stageIndex, List<IMonoSystemListener *> *subscribers)
						   {
							   stageIndices[i++] = stageIndex;
						   });
	return stageIndices;
}
//! Broadcasts InitializationStage event.
//!
//! @param stageIndex Index of initialization stage that is happening.
void EventBroadcaster::OnInitializationStage(int stageIndex)
{
	CryLogAlways("Commencing initialization stage #%d", stageIndex);

	this->stages = true;

	List<IMonoSystemListener *> stageList;
	if (this->stageMap.TryGet(stageIndex, stageList))
	{
		for (int i = 0; i < stageList.Length; i++)
		{
			IMonoSystemListener *stageListener = stageList[i];
			stageListener->OnInitializationStage(stageIndex);
		}
	}

	this->stages = false;
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

void EventBroadcaster::SendSimpleEvent(SimpleEventHandler handler)
{
	EventMessage("Broadcasting the event.");

	const int printEvery = 1;

	for (this->index = 0; this->index < this->listeners.Length; this->index++)
	{
#if 1
		if (this->index % printEvery == 0)
		{
			EventMessage("Sending the event to the listener #%d.", this->index);
		}
#endif
		IMonoSystemListener *listener = this->listeners[this->index];
		(listener->*handler)();
#if 1
		if (this->index % printEvery == 0)
		{
			EventMessage("Sent the event to the listener #%d.", this->index);
		}
#endif
	}

	this->index = -1;
}

void EventBroadcaster::SendUpdateEvent(SimpleEventHandler handler)
{
	for (this->index = 0; this->index < this->listeners.Length; this->index++)
	{
		IMonoSystemListener *listener = this->listeners[this->index];
		(listener->*handler)();
	}

	this->index = -1;
}