#include "stdafx.h"
#include "EventBroadcaster.h"

EventBroadcaster::EventBroadcaster()
	: latestRemovedListeners(20)
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
	// Delete the listener from the main list.
	int index = -1;
	this->listeners->ThroughEach
	(
		[&index, &listener](IMonoSystemListener *l, int i) { if (l == listener) index = i; }
	);
	if (index != -1)
	{
		this->latestRemovedListeners.Add(Pair<int, IMonoSystemListener *>(index, this->listeners->At(index)));
		this->listeners->RemoveAt(index);

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
}

void EventBroadcaster::SetInterface(IMonoInterface *inter)
{
	for (int i = 0; i < this->listeners->Length; i++)
	{
		this->listeners->At(i)->SetInterface(inter);
	}
}

//! Broadcasts PreInitialization event.
void EventBroadcaster::OnPreInitialization()
{
	this->SendSimpleEvent(IMonoSystemListener::OnPreInitialization);
}
//! Broadcasts RunTimeInitializing event.
void EventBroadcaster::OnRunTimeInitializing()
{
	this->SendSimpleEvent(IMonoSystemListener::OnRunTimeInitializing);
}
//! Broadcasts RunTimeInitialized event.
void EventBroadcaster::OnRunTimeInitialized()
{
	this->SendSimpleEvent(IMonoSystemListener::OnRunTimeInitialized);
}
//! Broadcasts CryamblyInitilizing event.
void EventBroadcaster::OnCryamblyInitilizing()
{
	this->SendSimpleEvent(IMonoSystemListener::OnCryamblyInitilizing);
}
//! Broadcasts CompilationStarting event.
void EventBroadcaster::OnCompilationStarting()
{
	this->SendSimpleEvent(IMonoSystemListener::OnCompilationStarting);
}
//! Broadcasts CompilationComplete event.
void EventBroadcaster::OnCompilationComplete(bool success)
{
	for (int i = 0; i < this->listeners->Length; i++)
	{
		this->listeners->At(i)->OnCompilationComplete(success);
		this->CorrectIndex(i);
	}
}
//! Gathers initialization stages data for sending it to managed code.
int *EventBroadcaster::GetSubscribedStagesInfo(int &stageCount)
{
	ReportComment("Going through listeners.");
	// Gather information about all stages.
	for (int i = 0; i < this->listeners->Length; i++)
	{
		ReportComment("Getting stage indices for the listener #%d.", i + 1);
		// Get stages.
		List<int> *stages = this->listeners->At(i)->GetSubscribedStages();
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
				this->stageMap->At(currentStageIndex)->Add(this->listeners->At(i));
			}
			delete stages;
		}
		else
		{
			ReportComment("Listener #%d is not subscribed to any stages.", i + 1);
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

			if (this->latestRemovedListeners.Length > 0)
			{
				int decrement = 0;
				// We need to find number of listeners that were placed in the stage list not after current one that were
				// unregistered.
				for (int j = 0; j < this->latestRemovedListeners.Length; j++)
				{
					auto removedListener = this->latestRemovedListeners[j].Value2;
					// Lambda function that checks whether one of the stage listeners was removed.
					auto indexOfLookUp = [removedListener](int stageIndex, IMonoSystemListener *listener)
					{
						return listener == removedListener;
					};
					if (stageList->IndexOf(indexOfLookUp) <= i)
					{
						decrement++;
					}
				}

				i -= decrement;

				this->latestRemovedListeners.Clear();
			}
		}
	}
}
//! Broadcasts CryamblyInitilized event.
void EventBroadcaster::OnCryamblyInitilized()
{
	this->SendSimpleEvent(IMonoSystemListener::OnCryamblyInitilized);
}
//! Broadcasts PostInitialization event.
void EventBroadcaster::OnPostInitialization()
{
	this->SendSimpleEvent(IMonoSystemListener::OnPostInitialization);
}
//! Broadcasts Update event.
void EventBroadcaster::Update()
{
	this->SendSimpleEvent(IMonoSystemListener::Update);
}
//! Broadcasts PostUpdate event.
void EventBroadcaster::PostUpdate()
{
	this->SendSimpleEvent(IMonoSystemListener::PostUpdate);
}
//! Broadcasts Shutdown event.
void EventBroadcaster::Shutdown()
{
	this->SendSimpleEvent(IMonoSystemListener::Shutdown);
}

void EventBroadcaster::SendSimpleEvent(SimpleEventHandler handler)
{
	for (int i = 0; i < this->listeners->Length; i++)
	{
		IMonoSystemListener *listener = this->listeners->At(i);
		(listener->*handler)();
		this->CorrectIndex(i);
	}
}

void EventBroadcaster::CorrectIndex(int &index)
{
	if (this->latestRemovedListeners.Length > 0)
	{
		int decrement = 0;		// This value will be subtracted from the counter to make sure we land on correct
								// listener on the next loop frame.

		for (int i = 0; i < this->latestRemovedListeners.Length; i++)
		{
			if (this->latestRemovedListeners[i].Value1 <= index)
			{
				decrement++;
			}
		}

		index -= decrement;

		this->latestRemovedListeners.Clear();
	}
}
