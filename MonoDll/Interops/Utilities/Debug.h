///////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// Scriptbind to hold various debugging methods
//////////////////////////////////////////////////////////////////////////
// 01/03/2012 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/

#ifndef __SCRIPTBIND_DEBUG__
#define __SCRIPTBIND_DEBUG__

#include <MonoCommon.h>
#include <IMonoInterop.h>

struct IPersistantDebug;

class DebugInterop : public IMonoInterop
{
public:
	DebugInterop();
	~DebugInterop();

protected:

	// Externals

	// IPersistentDebug
	static void AddPersistentSphere(Vec3 pos, float radius, ColorF color, float timeout);
	static void AddDirection(Vec3 pos, float radius, Vec3 dir, ColorF color, float timeout);
	static void AddPersistentText2D(mono::string text, float size, ColorF color, float timeout);
	static void AddPersistentLine(Vec3 pos, Vec3 end, ColorF clr, float timeout);

	static void AddAABB(Vec3 pos, AABB aabb, ColorF clr, float timeout);
	//static void
	// ~IPersistentDebug

	// Profiling
	static CFrameProfiler *CreateFrameProfiler(mono::string methodName);
	static CFrameProfilerSection *CreateFrameProfilerSection(CFrameProfiler *pProfiler);
	static void DeleteFrameProfilerSection(CFrameProfilerSection *pSection);
	// ~Profiling

	// ~Externals

	static IPersistantDebug *GetIPersistentDebug();

	// IMonoScriptBind
	virtual const char *GetClassName() override { return "DebugInterop"; }
	// ~IMonoScriptBind

protected:
	static std::vector<CFrameProfiler *> m_frameProfilers;
};

#endif