#pragma once

#include "IMonoInterface.h"

struct ProfilingInterop : public IMonoInterop<false, true>
{
	virtual const char *GetName() { return ""; }
	virtual const char *GetNameSpace() { return "CryCil.Engine.DebugServices"; }

	virtual void OnRunTimeInitialized();

	virtual void Shutdown();

	static void constructor(mono::object profiler, mono::string name);
	static CFrameProfilerSection *StartSection(mono::object profiler);

	static void FinishSection(CFrameProfilerSection **sectionPtr);

	static List<CFrameProfiler *> cryCilProfilers;
};