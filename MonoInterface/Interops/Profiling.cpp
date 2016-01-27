#include "stdafx.h"

#include "Profiling.h"

void ProfilingInterop::OnRunTimeInitialized()
{
	const char *nameSpace = this->GetInteropNameSpace();
	REGISTER_METHOD_NCN(nameSpace, "Profiler", "CreateProfiler", CreateProfiler);
	REGISTER_METHOD_NCN(nameSpace, "Profiler", "StartSection", StartSection);

	REGISTER_METHOD_NCN(nameSpace, "ProfilingSection", "FinishSection", FinishSection);
}

CFrameProfiler *ProfilingInterop::CreateProfiler(mono::string name)
{
	auto frameProfiler = new CFrameProfiler(gEnv->pSystem, ToNativeString(name), PROFILE_SYSTEM);
	
	cryCilProfilers.Add(frameProfiler);

	return frameProfiler;
}

CFrameProfilerSection *ProfilingInterop::StartSection(CFrameProfiler *handle)
{
	return new CFrameProfilerSection(handle);
}

void ProfilingInterop::FinishSection(CFrameProfilerSection *sectionPtr)
{
	delete sectionPtr;
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
