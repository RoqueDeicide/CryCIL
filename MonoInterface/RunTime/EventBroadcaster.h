#pragma once

#include "IMonoInterface.h"
//! Broadcasts events to listeners.
struct EventBroadcaster
{
	std::vector<IMonoSystemListener *> listeners;
	// Map is basically a SortedList.
	std::map<int, std::vector<IMonoSystemListener *>> stageMap;
	//! Removes a listener from broadcasting list and stages map.
	void RemoveListener(IMonoSystemListener *listener)
	{
		// Delete the listener from the main list.
		for (auto i = this->listeners.begin(); i != this->listeners.end(); i++)
		{
			if (*i == listener)
			{
				this->listeners.erase(i);
			}
		}
		// Remove listener from stage map.
		for each (auto var in this->stageMap)
		{
			for (auto i = var.second.begin(); i != var.second.end(); i++)
			{
				if (*i == listener)
				{
					var.second.erase(i);
				}
			}
		}
	}
	//! Broadcasts PreInitialization event.
	void OnPreInitialization()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->OnPreInitialization();
		}
	}
	//! Broadcasts RunTimeInitializing event.
	void OnRunTimeInitializing()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->OnRunTimeInitializing();
		}
	}
	//! Broadcasts RunTimeInitialized event.
	void OnRunTimeInitialized()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->OnRunTimeInitialized();
		}
	}
	//! Broadcasts CryamblyInitilizing event.
	void OnCryamblyInitilizing()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->OnCryamblyInitilizing();
		}
	}
	//! Broadcasts CompilationStarting event.
	void OnCompilationStarting()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->OnCompilationStarting();
		}
	}
	//! Broadcasts CompilationComplete event.
	void OnCompilationComplete(bool success)
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->OnCompilationComplete(success);
		}
	}
	//! Gathers initialization stages data for sending it to managed code.
	int *GetSubscribedStagesInfo(int &stageCount)
	{
		// Gather information about all stages.
		for each (IMonoSystemListener *listener in this->listeners)
		{
			// Get stages.
			int stagesCount;
			int *stages = listener->GetSubscribedStages(stagesCount);
			if (!stages)
			{
				continue;
			}
			// Put the listeners into the map.
			for (int i = 0; i < stagesCount; i++)
			{
				this->stageMap[stages[i]].push_back(listener);
			}
			delete stages;
		}
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
	void OnInitializationStage(int stageIndex)
	{
		IMonoSystemListener **listeners = &this->stageMap[stageIndex][0];
		int listenerCount = this->stageMap[stageIndex].size();

		for (int i = 0; i < listenerCount; i++)
		{
			listeners[i]->OnInitializationStage(stageIndex);
		}
	}
	//! Broadcasts CryamblyInitilized event.
	void OnCryamblyInitilized()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->OnCryamblyInitilized();
		}
	}
	//! Broadcasts PostInitialization event.
	void OnPostInitialization()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->OnPostInitialization();
		}
	}
	//! Broadcasts Update event.
	void Update()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->Update();
		}
	}
	//! Broadcasts PostUpdate event.
	void PostUpdate()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->PostUpdate();
		}
	}
	//! Broadcasts Shutdown event.
	void Shutdown()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->Shutdown();
		}
	}
};