#pragma once

#include "IMonoInterface.h"
//! Broadcasts events to listeners.
struct EventBroadcaster
{
	std::vector<IMonoSystemListener *> listeners;
	// Map is basically a SortedList.
	std::map<int, std::vector<IMonoSystemListener *>> stageMap;

	void OnPreInitialization()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->OnPreInitialization();
		}
	}
	void OnRunTimeInitializing()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->OnRunTimeInitializing();
		}
	}
	void OnRunTimeInitialized()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->OnRunTimeInitialized();
		}
	}
	void OnCryamblyInitilizing()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->OnCryamblyInitilizing();
		}
	}
	void OnCompilationStarting()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->OnCompilationStarting();
		}
	}
	void OnCompilationComplete(bool success)
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->OnCompilationComplete(success);
		}
	}
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
	void OnInitializationStage(int stageIndex)
	{
		IMonoSystemListener **listeners = &this->stageMap[stageIndex][0];
		int listenerCount = this->stageMap[stageIndex].size();

		for (int i = 0; i < listenerCount; i++)
		{
			listeners[i]->OnInitializationStage(stageIndex);
		}
	}
	void OnCryamblyInitilized()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->OnCryamblyInitilized();
		}
	}
	void OnPostInitialization()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->OnPostInitialization();
		}
	}
	void Update()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->Update();
		}
	}
	void PostUpdate()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->PostUpdate();
		}
	}
	void Shutdown()
	{
		for (int i = 0; i < this->listeners.size(); i++)
		{
			this->listeners[i]->Shutdown();
		}
	}
};