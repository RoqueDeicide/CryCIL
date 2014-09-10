///////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// CryENGINE particle system scriptbind
//////////////////////////////////////////////////////////////////////////
// 17/03/2012 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __SCRIPTBIND_VIEW_SYSTEM__
#define __SCRIPTBIND_VIEW_SYSTEM__

#include <MonoCommon.h>
#include <IMonoScriptBind.h>

struct SViewParams;

class CScriptbind_ViewSystem : public IMonoScriptBind
{
public:
	CScriptbind_ViewSystem();
	~CScriptbind_ViewSystem() {}

protected:
	// IMonoScriptBind
	virtual const char *GetClassName() override { return "ViewInterop"; }
	// ~IMonoScriptBind

	// Externals
	static unsigned int GetView(unsigned int linkedId, bool forceCreate = false);
	static void RemoveView(unsigned int viewId);

	static EntityId GetActiveView();
	static void SetActiveView(unsigned int viewId);

	static Vec3 GetViewPosition(unsigned int viewId);
	static Quat GetViewRotation(unsigned int viewId);
	static float GetViewNearPlane(unsigned int viewId);
	static float GetViewFieldOfView(unsigned int viewId);
	static void SetViewPosition(unsigned int viewId, Vec3 pos);
	static void SetViewRotation(unsigned int viewId, Quat rot);
	static void SetViewNearPlane(unsigned int viewId, float nearPlane);
	static void SetViewFieldOfView(unsigned int viewId, float fov);
	// ~Externals
};

#endif //__SCRIPTBIND_VIEW_SYSTEM__