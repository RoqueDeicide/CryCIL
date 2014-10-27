#pragma once

#include "IMonoInterface.h"
//! Reports events that occur within CryCIL.
struct DebugEventReporter : public IMonoSystemListener
{
	virtual void SetInterface(IMonoInterface *handle);

	virtual void OnPreInitialization();

	virtual void OnRunTimeInitializing();

	virtual void OnRunTimeInitialized();

	virtual void OnCryamblyInitilizing();

	virtual void OnCompilationStarting();

	virtual void OnCompilationComplete(bool success);

	virtual int * GetSubscribedStages(int &stageCount);

	virtual void OnInitializationStage(int stageIndex);

	virtual void OnCryamblyInitilized();

	virtual void OnPostInitialization();

	virtual void Update();

	virtual void PostUpdate();

	virtual void Shutdown();
};