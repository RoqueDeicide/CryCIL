#pragma once

#include "IMonoInterface.h"

struct SystemEventsInterop : IMonoSystemListener, ISystemEventListener
{
	virtual void OnSystemEvent(ESystemEvent _event, UINT_PTR wparam, UINT_PTR lparam) override;

	virtual void OnPreInitialization() override {}
	virtual void OnRunTimeInitializing() override {}
	virtual void OnRunTimeInitialized() override;
	virtual void OnCryamblyInitilizing() override {}
	virtual void OnCompilationStarting() override {}
	virtual void OnCompilationComplete(bool) override {}
	virtual List<int> *GetSubscribedStages() override { return nullptr; }
	virtual void OnInitializationStage(int) override {}
	virtual void OnCryamblyInitilized() override {}
	virtual void OnPostInitialization() override {}
	virtual void Update() override {}
	virtual void PostUpdate() override {}
	virtual void Shutdown() override {}

	static SystemEventsInterop g_this;
};