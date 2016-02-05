#include "stdafx.h"
#include "EventBroadcaster.h"

EventBroadcaster::EventBroadcaster()
	: stages(false)
	, index(-1)
{
	this->listeners = new List<IMonoSystemListener *>(20);
	this->stageMap = new SortedList<int, List<IMonoSystemListener *> *>(50);
}

EventBroadcaster::~EventBroadcaster()
{
	ReportComment("Deleting stage map.");
	if (this->stageMap) { delete this->stageMap; this->stageMap = nullptr; }
	ReportComment("Deleting listeners.");
	if (this->listeners) { delete this->listeners; this->listeners = nullptr; }
}


void EventBroadcaster::RemoveListener(IMonoSystemListener *listener)
{
	ReportComment("Removing a listener.");
	// Delete the listener from the main list.
	int listenerIndex = -1;
	for (int i = 0; i < this->listeners->Length; i++)
	{
		if (this->listeners->At(i) == listener)
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

	this->listeners->RemoveAt(listenerIndex);

	// Remove listener from stage map.
	this->stageMap->ForEach
	(
		[&listener](int stageIndex, List<IMonoSystemListener *> *subscribers)
		{
			for (int i = 0; i < subscribers->Length; i++)
			{
				if (subscribers->At(i) == listener)
				{
					subscribers->RemoveAt(i);
					break;		// No need to proceed as there can only be one unique listener per stage.
				}
			}
		}
	);
}

void EventBroadcaster::SetInterface(IMonoInterface *inter)
{
	for (this->index = 0; this->index < this->listeners->Length; this->index++)
	{
		this->listeners->At(this->index)->SetInterface(inter);
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
	for (this->index = 0; this->index < this->listeners->Length; this->index++)
	{
		this->listeners->At(this->index)->OnCompilationComplete(success);
	}
}
//! Gathers initialization stages data for sending it to managed code.
int *EventBroadcaster::GetSubscribedStagesInfo(int &stageCount)
{
	ReportComment("Going through listeners.");
	// Gather information about all stages.
	for (this->index = 0; this->index < this->listeners->Length; this->index++)
	{
		ReportComment("Getting stage indices for the listener #%d.", this->index + 1);
		// Get stages.
		List<int> *stages = this->listeners->At(this->index)->GetSubscribedStages();
		if (stages)
		{
			// Put the listeners into the map.
			for (int j = 0; j < stages->Length; j++)
			{
				int currentStageIndex = stages->At(j);
				if (!this->stageMap->Contains(currentStageIndex))
				{
					this->stageMap->Add(currentStageIndex, new List<IMonoSystemListener *>(10));
				}
				this->stageMap->At(currentStageIndex)->Add(this->listeners->At(this->index));
			}
			delete stages;
		}
		else
		{
			ReportComment("Listener #%d is not subscribed to any stages.", this->index + 1);
		}
	}
	// Get the stage indices.
	stageCount = this->stageMap->Length;
	int *stageIndices = new int[stageCount];
	int i = 0;
	this->stageMap->ForEach([stageIndices, &i](int stageIndex, List<IMonoSystemListener *> *subscribers)
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

	List<IMonoSystemListener *> *stageList;
	if (this->stageMap->TryGet(stageIndex, stageList))
	{
		if (!stageList)
		{
			return;
		}

		for (int i = 0; i < stageList->Length; i++)
		{
			IMonoSystemListener *stageListener = stageList->At(i);
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
	this->SendSimpleEvent(&IMonoSystemListener::Update);
}
//! Broadcasts PostUpdate event.
void EventBroadcaster::PostUpdate()
{
	this->SendSimpleEvent(&IMonoSystemListener::PostUpdate);
}
//! Broadcasts Shutdown event.
void EventBroadcaster::Shutdown()
{
	this->SendSimpleEvent(&IMonoSystemListener::Shutdown);
}

void EventBroadcaster::SendSimpleEvent(SimpleEventHandler handler)
{
	for (this->index = 0; this->index < this->listeners->Length; this->index++)
	{
		IMonoSystemListener *listener = this->listeners->At(this->index);
		(listener->*handler)();
	}

	this->index = -1;
}