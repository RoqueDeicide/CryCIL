#pragma once

#include "IMonoInterface.h"

struct SurfaceTypeInterop : public IMonoInterop < true, true >
{
	virtual const char *GetName() { return "SurfaceType"; }
	virtual const char *GetNameSpace() { return "CryCil.Engine.Physics"; }

	virtual void OnRunTimeInitialized();

	static ISurfaceType                     *Get(mono::string name);
	static ISurfaceType                     *GetInt(int id);
	static bool                              Register(ISurfaceType *type, bool isDefault = false);
	static void                              Unregister(ISurfaceType *type);
	static ushort                            GetId(ISurfaceType *handle);
	static mono::string                      GetSurfaceTypeName(ISurfaceType *handle);
	static mono::string                      GetTypeName(ISurfaceType *handle);
	static ESurfaceTypeFlags                 GetFlags(ISurfaceType *handle);
	static int                               GetBreakability(ISurfaceType *handle);
	static float                             GetBreakEnergy(ISurfaceType *handle);
	static int                               GetHitpoints(ISurfaceType *handle);
	static ISurfaceType::SPhysicalParams    *GetPhyscalParams(ISurfaceType *handle);
	static ISurfaceType::SBreakable2DParams *GetBreakable2DParams(ISurfaceType *handle);
	static ISurfaceType::SBreakageParticles *GetBreakageParticles(ISurfaceType *handle, const char *sType,
																			 bool bLookInDefault = true);
};

struct SurfaceTypeEnumeratorInterop : public IMonoInterop < true, true >
{
	virtual const char *GetName() { return "SurfaceTypeEnumerator"; }
	virtual const char *GetNameSpace() { return "CryCil.Engine.Physics"; }

	virtual void OnRunTimeInitialized();

	static ISurfaceTypeEnumerator *Init();
	static ISurfaceType *GetFirst(ISurfaceTypeEnumerator *handle);
	static ISurfaceType *GetNext(ISurfaceTypeEnumerator *handle);
	static void          Release(ISurfaceTypeEnumerator *handle);
};