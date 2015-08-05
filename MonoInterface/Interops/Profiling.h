#pragma once

#include "IMonoInterface.h"

struct ProfilingInterop : public IMonoInterop<false, true>
{
	virtual const char *GetName() override { return ""; }
	virtual const char *GetNameSpace() override { return "CryCil.Engine.DebugServices"; }

	virtual void OnRunTimeInitialized() override;

	virtual void Shutdown() override;

	static void constructor(mono::object profiler, mono::string name);
	static CFrameProfilerSection *StartSection(mono::object profiler);

	static void FinishSection(CFrameProfilerSection **sectionPtr);

	static List<CFrameProfiler *> cryCilProfilers;
};