#include "stdafx.h"

#include "Profiling.h"

void ProfilingInterop::InitializeInterops()
{
	const char *nameSpace = this->GetInteropNameSpace();
	REGISTER_METHOD_NCN(nameSpace, "Profiler", "CreateProfiler", CreateProfiler);
	REGISTER_METHOD_NCN(nameSpace, "Profiler", "StartSection", StartSection);

	REGISTER_METHOD_NCN(nameSpace, "ProfilingSection", "FinishSection", FinishSection);
}

CFrameProfiler *ProfilingInterop::CreateProfiler(EProfileDescription desc, mono::string name,
												 mono::string file, uint32 line)
{
	auto frameProfiler = new CFrameProfiler(PROFILE_SYSTEM, desc, ToNativeString(name),
											ToNativeString(file), line);

	cryCilProfilers.Add(frameProfiler);

	return frameProfiler;
}

CFrameProfilerSection *ProfilingInterop::StartSection(CFrameProfiler *handle)
{
	return new CFrameProfilerSection(handle, nullptr, nullptr, EProfileDescription::REGION);
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
