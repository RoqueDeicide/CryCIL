#include "StdAfx.h"
#include "Debug.h"

#include "MonoScriptSystem.h"

#include <IGameFramework.h>

std::vector<CFrameProfiler *> CScriptbind_Debug::m_frameProfilers = std::vector<CFrameProfiler *>();

CScriptbind_Debug::CScriptbind_Debug()
{
	REGISTER_METHOD(AddPersistentSphere);
	REGISTER_METHOD(AddDirection);
	REGISTER_METHOD(AddPersistentText2D);
	REGISTER_METHOD(AddPersistentLine);

	REGISTER_METHOD(AddAABB);

	REGISTER_METHOD(CreateFrameProfiler);
	REGISTER_METHOD(CreateFrameProfilerSection);
	REGISTER_METHOD(DeleteFrameProfilerSection);
}

CScriptbind_Debug::~CScriptbind_Debug()
{
	for each(auto pFrameProfiler in m_frameProfilers)
		delete pFrameProfiler;

	m_frameProfilers.clear();
}

void CScriptbind_Debug::AddPersistentSphere(Vec3 pos, float radius, ColorF color, float timeout)
{
	// TODO: Find a pretty way to do Begin in C#.
	GetIPersistentDebug()->Begin("TestAddPersistentSphere", false);
	GetIPersistentDebug()->AddSphere(pos, radius, color, timeout);
}

void CScriptbind_Debug::AddDirection(Vec3 pos, float radius, Vec3 dir, ColorF color, float timeout)
{
	GetIPersistentDebug()->Begin("TestAddDirection", false);
	GetIPersistentDebug()->AddDirection(pos, radius, dir, color, timeout);
}

void CScriptbind_Debug::AddPersistentText2D(mono::string text, float size, ColorF color, float timeout)
{
	GetIPersistentDebug()->Begin("TestAddPersistentText2D", false);
	GetIPersistentDebug()->Add2DText(ToCryString(text), size, color, timeout);
}

void CScriptbind_Debug::AddPersistentLine(Vec3 pos, Vec3 end, ColorF clr, float timeout)
{
	GetIPersistentDebug()->Begin("TestAddPersistentLine", false);
	GetIPersistentDebug()->AddLine(pos, end, clr, timeout);
}

void CScriptbind_Debug::AddAABB(Vec3 pos, AABB aabb, ColorF clr, float timeout)
{
	GetIPersistentDebug()->Begin("TestAddAABB", false);
	GetIPersistentDebug()->AddAABB(aabb.min + pos, aabb.max + pos, clr, timeout);
}

IPersistantDebug *CScriptbind_Debug::GetIPersistentDebug()
{
	return static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetIPersistantDebug();
}

CFrameProfiler *CScriptbind_Debug::CreateFrameProfiler(mono::string methodName)
{
	CFrameProfiler *pFrameProfiler = new CFrameProfiler(GetISystem(), ToCryString(methodName), PROFILE_SCRIPT);

	m_frameProfilers.push_back(pFrameProfiler);

	return pFrameProfiler;
}

CFrameProfilerSection *CScriptbind_Debug::CreateFrameProfilerSection(CFrameProfiler *pProfiler)
{
	return new CFrameProfilerSection(pProfiler);
}

void CScriptbind_Debug::DeleteFrameProfilerSection(CFrameProfilerSection *pSection)
{
	delete pSection;
}

extern "C"
{
	_declspec(dllexport) void __cdecl Log(const char *msg, const IMiniLog::ELogType nType)
	{
		va_list args;
		gEnv->pLog->LogV(nType, msg, args);
	}

	_declspec(dllexport) void __cdecl Warning(const char *msg)
	{
		MonoWarning(msg); 
	}
}