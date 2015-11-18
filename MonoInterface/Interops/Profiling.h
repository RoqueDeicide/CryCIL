#pragma once

#include "IMonoInterface.h"

struct ProfilingInterop : public IMonoInterop<false, true>
{
	virtual const char *GetInteropClassName() override { return ""; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.DebugServices"; }

	virtual void OnRunTimeInitialized() override;

	virtual void Shutdown() override;

	static void FinishSection(CFrameProfilerSection *sectionPtr);
	static CFrameProfiler *CreateProfiler(mono::string name);
	static CFrameProfilerSection *StartSection(CFrameProfiler *handle);

	static List<CFrameProfiler *> cryCilProfilers;
};