#include "stdafx.h"

#include "Profiling.h"

void ProfilingInterop::OnRunTimeInitialized()
{
	MonoEnv->Functions->AddInternalCall(this->GetNameSpace(), "Profiler", ".ctor", constructor);
	MonoEnv->Functions->AddInternalCall(this->GetNameSpace(), "Profiler", "Start", StartSection);

	MonoEnv->Functions->AddInternalCall(this->GetNameSpace(), "ProfilingSection", "Dispose", FinishSection);
}

void ProfilingInterop::constructor(mono::object profiler, mono::string name)
{
	auto frameProfiler = new CFrameProfiler(gEnv->pSystem, ToNativeString(name), PROFILE_SYSTEM);

	*GET_BOXED_OBJECT_DATA(CFrameProfiler *, profiler) = frameProfiler;
	
	cryCilProfilers.Add(frameProfiler);
}

CFrameProfilerSection *ProfilingInterop::StartSection(mono::object profiler)
{
	return new CFrameProfilerSection(*GET_BOXED_OBJECT_DATA(CFrameProfiler *, profiler));
}

void ProfilingInterop::FinishSection(CFrameProfilerSection **sectionPtr)
{
	delete *sectionPtr;
}

void ProfilingInterop::Shutdown()
{
	for (int i = 0; i < cryCilProfilers.Length; i++)
	{
		delete cryCilProfilers[i];
	}
	cryCilProfilers.Clear();
}

List<CFrameProfiler *> ProfilingInterop::cryCilProfilers(10);
