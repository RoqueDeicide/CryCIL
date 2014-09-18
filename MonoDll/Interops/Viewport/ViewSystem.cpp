#include "StdAfx.h"
#include "ViewSystem.h"

#include "MonoScriptSystem.h"

#include <IViewSystem.h>
#include <IGameFramework.h>

#include <IEntitySystem.h>

ViewSystemInterop::ViewSystemInterop()
{
	REGISTER_METHOD(GetView);
	REGISTER_METHOD(RemoveView);

	REGISTER_METHOD(GetActiveView);
	REGISTER_METHOD(SetActiveView);

	REGISTER_METHOD(GetViewPosition);
	REGISTER_METHOD(GetViewRotation);
	REGISTER_METHOD(GetViewNearPlane);
	REGISTER_METHOD(GetViewFieldOfView);
	REGISTER_METHOD(SetViewPosition);
	REGISTER_METHOD(SetViewRotation);
	REGISTER_METHOD(SetViewNearPlane);
	REGISTER_METHOD(SetViewFieldOfView);
}

unsigned int ViewSystemInterop::GetView(EntityId id, bool forceCreate)
{
	if (IViewSystem *pViewSystem = static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetIViewSystem())
	{
		if (IView *pView = pViewSystem->GetViewByEntityId(id, forceCreate))
			return pViewSystem->GetViewId(pView);
	}

	return 0;
}

void ViewSystemInterop::RemoveView(unsigned int viewId)
{
	if (IViewSystem *pViewSystem = static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetIViewSystem())
	{
		if (IView *pView = pViewSystem->GetView(viewId))
			pView->Release();
	}
}

unsigned int ViewSystemInterop::GetActiveView()
{
	if (IViewSystem *pViewSystem = static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetIViewSystem())
		return pViewSystem->GetActiveViewId();

	return 0;
}

void ViewSystemInterop::SetActiveView(unsigned int viewId)
{
	if (IViewSystem *pViewSystem = static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetIViewSystem())
	{
		if (pViewSystem->GetView(viewId))
			pViewSystem->SetActiveView(viewId);
	}
}

SViewParams GetViewParams(unsigned int viewId)
{
	if (IViewSystem *pViewSystem = static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetIViewSystem())
	{
		if (IView *pView = pViewSystem->GetView(viewId))
			return *pView->GetCurrentParams();
	}

	return SViewParams();
}

void SetViewParams(EntityId viewId, SViewParams &viewParams)
{
	if (IViewSystem *pViewSystem = static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetIViewSystem())
	{
		if (IView *pView = pViewSystem->GetView(viewId))
			pView->SetCurrentParams(viewParams);
	}
}

Vec3 ViewSystemInterop::GetViewPosition(unsigned int viewId)
{
	return GetViewParams(viewId).position;
}

Quat ViewSystemInterop::GetViewRotation(unsigned int viewId)
{
	return GetViewParams(viewId).rotation;
}

float ViewSystemInterop::GetViewNearPlane(unsigned int viewId)
{
	return GetViewParams(viewId).nearplane;
}

float ViewSystemInterop::GetViewFieldOfView(unsigned int viewId)
{
	return GetViewParams(viewId).fov;
}

void ViewSystemInterop::SetViewPosition(unsigned int viewId, Vec3 pos)
{
	if (IViewSystem *pViewSystem = static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetIViewSystem())
	{
		if (IView *pView = pViewSystem->GetView(viewId))
		{
			SViewParams params = *pView->GetCurrentParams();
			params.position = pos;
			pView->SetCurrentParams(params);
		}
	}
}

void ViewSystemInterop::SetViewRotation(unsigned int viewId, Quat rot)
{
	if (IViewSystem *pViewSystem = static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetIViewSystem())
	{
		if (IView *pView = pViewSystem->GetView(viewId))
		{
			SViewParams params = *pView->GetCurrentParams();
			params.rotation = rot;
			pView->SetCurrentParams(params);
		}
	}
}

void ViewSystemInterop::SetViewNearPlane(unsigned int viewId, float nearPlane)
{
	if (IViewSystem *pViewSystem = static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetIViewSystem())
	{
		if (IView *pView = pViewSystem->GetView(viewId))
		{
			SViewParams params = *pView->GetCurrentParams();
			params.nearplane = nearPlane;
			pView->SetCurrentParams(params);
		}
	}
}

void ViewSystemInterop::SetViewFieldOfView(unsigned int viewId, float fov)
{
	if (IViewSystem *pViewSystem = static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetIViewSystem())
	{
		if (IView *pView = pViewSystem->GetView(viewId))
		{
			SViewParams params = *pView->GetCurrentParams();
			params.fov = fov;
			pView->SetCurrentParams(params);
		}
	}
}