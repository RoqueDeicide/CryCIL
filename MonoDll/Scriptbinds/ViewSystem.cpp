#include "StdAfx.h"
#include "ViewSystem.h"

#include "MonoScriptSystem.h"

#include <IViewSystem.h>
#include <IGameFramework.h>

#include <IEntitySystem.h>

CScriptbind_ViewSystem::CScriptbind_ViewSystem()
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

unsigned int CScriptbind_ViewSystem::GetView(EntityId id, bool forceCreate)
{
	if (IViewSystem *pViewSystem = static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetIViewSystem())
	{
		if (IView *pView = pViewSystem->GetViewByEntityId(id, forceCreate))
			return pViewSystem->GetViewId(pView);
	}

	return 0;
}

void CScriptbind_ViewSystem::RemoveView(unsigned int viewId)
{
	if (IViewSystem *pViewSystem = static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetIViewSystem())
	{
		if (IView *pView = pViewSystem->GetView(viewId))
			pView->Release();
	}
}

unsigned int CScriptbind_ViewSystem::GetActiveView()
{
	if (IViewSystem *pViewSystem = static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetIViewSystem())
		return pViewSystem->GetActiveViewId();

	return 0;
}

void CScriptbind_ViewSystem::SetActiveView(unsigned int viewId)
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

Vec3 CScriptbind_ViewSystem::GetViewPosition(unsigned int viewId)
{
	return GetViewParams(viewId).position;
}

Quat CScriptbind_ViewSystem::GetViewRotation(unsigned int viewId)
{
	return GetViewParams(viewId).rotation;
}

float CScriptbind_ViewSystem::GetViewNearPlane(unsigned int viewId)
{
	return GetViewParams(viewId).nearplane;
}

float CScriptbind_ViewSystem::GetViewFieldOfView(unsigned int viewId)
{
	return GetViewParams(viewId).fov;
}

void CScriptbind_ViewSystem::SetViewPosition(unsigned int viewId, Vec3 pos)
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

void CScriptbind_ViewSystem::SetViewRotation(unsigned int viewId, Quat rot)
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

void CScriptbind_ViewSystem::SetViewNearPlane(unsigned int viewId, float nearPlane)
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

void CScriptbind_ViewSystem::SetViewFieldOfView(unsigned int viewId, float fov)
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