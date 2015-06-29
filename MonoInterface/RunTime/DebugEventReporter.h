#pragma once

#include "IMonoInterface.h"
//! Reports events that occur within CryCIL.
struct DebugEventReporter : public IMonoSystemListener
{
	virtual void       OnPreInitialization() override;
	virtual void       OnRunTimeInitializing() override;
	virtual void       OnRunTimeInitialized() override;
	virtual void       OnCryamblyInitilizing() override;
	virtual void       OnCompilationStarting() override;
	virtual void       OnCompilationComplete(bool success) override;
	virtual List<int> *GetSubscribedStages() override;
	virtual void       OnInitializationStage(int stageIndex) override;
	virtual void       OnCryamblyInitilized() override;
	virtual void       OnPostInitialization() override;
	virtual void       Update() override;
	virtual void       PostUpdate() override;
	virtual void       Shutdown() override;
};