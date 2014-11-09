#include "stdafx.h"
#include "EventBroadcaster.h"

void EventBroadcaster::RemoveListener(IMonoSystemListener *listener)
{
	// Delete the listener from the main list.
	int index = -1;
	this->listeners.ThroughEach
	(
		[&index, &listener](IMonoSystemListener *l, int i) { if (l == listener) index = i; }
	);
	if (index != -1)
	{
		this->listeners.RemoveAt(index);
	}
	// Remove listener from stage map.
	for each (auto var in this->stageMap)
	{
		auto stageList = var.second;
		for (int i = 0; i < stageList->Length; i++)
		{
			if (stageList->At(i) == listener)
			{
				stageList->RemoveAt(i);
				i--;
			}
		}
	}
}
//! Broadcasts PreInitialization event.
void EventBroadcaster::OnPreInitialization()
{
	for (int i = 0; i < this->listeners.Length; i++)
	{
		this->listeners[i]->OnPreInitialization();
	}
}
//! Broadcasts RunTimeInitializing event.
void EventBroadcaster::OnRunTimeInitializing()
{
	for (int i = 0; i < this->listeners.Length; i++)
	{
		this->listeners[i]->OnRunTimeInitializing();
	}
}
//! Broadcasts RunTimeInitialized event.
void EventBroadcaster::OnRunTimeInitialized()
{
	for (int i = 0; i < this->listeners.Length; i++)
	{
		this->listeners[i]->OnRunTimeInitialized();
	}
}
//! Broadcasts CryamblyInitilizing event.
void EventBroadcaster::OnCryamblyInitilizing()
{
	for (int i = 0; i < this->listeners.Length; i++)
	{
		this->listeners[i]->OnCryamblyInitilizing();
	}
}
//! Broadcasts CompilationStarting event.
void EventBroadcaster::OnCompilationStarting()
{
	for (int i = 0; i < this->listeners.Length; i++)
	{
		this->listeners[i]->OnCompilationStarting();
	}
}
//! Broadcasts CompilationComplete event.
void EventBroadcaster::OnCompilationComplete(bool success)
{
	for (int i = 0; i < this->listeners.Length; i++)
	{
		this->listeners[i]->OnCompilationComplete(success);
	}
}
//! Gathers initialization stages data for sending it to managed code.
int *EventBroadcaster::GetSubscribedStagesInfo(int &stageCount)
{
	// Gather information about all stages.
	auto processor = [&stageCount, this](IMonoSystemListener *listener)
	{
		// Get stages.
		int stagesCount;
		int *stages = listener->GetSubscribedStages(stagesCount);
		if (stages)
		{
			// Put the listeners into the map.
			for (int i = 0; i < stagesCount; i++)
			{
				this->stageMap[stages[i]]->Add(listener);
			}
			delete stages;
		}
	};
	this->listeners.ForEach(processor);
	// Get the stage indices.
	stageCount = this->stageMap.size();
	int *stageIndices = new int[stageCount];
	int i = 0;
	for (auto it = this->stageMap.begin(); it != this->stageMap.end(); it++, i++)
	{
		stageIndices[i] = it->first;
	}
	return stageIndices;
}
//! Broadcasts InitializationStage event.
//!
//! @param stageIndex Index of initialization stage that is happening.
void EventBroadcaster::OnInitializationStage(int stageIndex)
{
	auto stageList = this->stageMap[stageIndex];

	for (int i = 0; i < stageList->Length; i++)
	{
		stageList->At(i)->OnInitializationStage(stageIndex);
	}
}
//! Broadcasts CryamblyInitilized event.
void EventBroadcaster::OnCryamblyInitilized()
{
	for (int i = 0; i < this->listeners.Length; i++)
	{
		this->listeners[i]->OnCryamblyInitilized();
	}
}
//! Broadcasts PostInitialization event.
void EventBroadcaster::OnPostInitialization()
{
	for (int i = 0; i < this->listeners.Length; i++)
	{
		this->listeners[i]->OnPostInitialization();
	}
}
//! Broadcasts Update event.
void EventBroadcaster::Update()
{
	for (int i = 0; i < this->listeners.Length; i++)
	{
		this->listeners[i]->Update();
	}
}
//! Broadcasts PostUpdate event.
void EventBroadcaster::PostUpdate()
{
	for (int i = 0; i < this->listeners.Length; i++)
	{
		this->listeners[i]->PostUpdate();
	}
}
//! Broadcasts Shutdown event.
void EventBroadcaster::Shutdown()
{
	for (int i = 0; i < this->listeners.Length; i++)
	{
		this->listeners[i]->Shutdown();
	}
}