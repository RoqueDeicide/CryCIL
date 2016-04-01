#include "stdafx.h"

#include "CryView.h"

void CryViewInterop::InitializeInterops()
{
	REGISTER_METHOD(UpdateInternal);
	REGISTER_METHOD(LinkToInternal);
	REGISTER_METHOD(GetLinkedId);
	REGISTER_METHOD(SetCurrentParams);
	REGISTER_METHOD(GetCurrentParams);
	REGISTER_METHOD(SetViewShake);
	REGISTER_METHOD(SetViewShakeEx);
	REGISTER_METHOD(StopShakeInternal);
	REGISTER_METHOD(ResetShakingInternal);
	REGISTER_METHOD(ResetBlendingInternal);
	REGISTER_METHOD(SetFrameAdditiveCameraAngles);
	REGISTER_METHOD(SetScale);
	REGISTER_METHOD(SetZoomedScale);
	REGISTER_METHOD(CreateView);
	REGISTER_METHOD(RemoveView);
	REGISTER_METHOD(RemoveViewId);
	REGISTER_METHOD(SetActiveView);
	REGISTER_METHOD(SetActiveViewId);
	REGISTER_METHOD(GetViewInternal);
	REGISTER_METHOD(GetActiveViewInternal);
	REGISTER_METHOD(GetViewId);
	REGISTER_METHOD(GetActiveViewIdInternal);
	REGISTER_METHOD(GetViewByEntityId);
	REGISTER_METHOD(GetDefaultZNear);
	REGISTER_METHOD(IsPlayingCutScene);
}

void CryViewInterop::UpdateInternal(IView *handle, float frameTime, bool isActive)
{
	handle->Update(frameTime, isActive);
}

void CryViewInterop::LinkToInternal(IView *handle, IEntity *follow, bool gameObject)
{
	if (gameObject)
	{
		auto obj = MonoEnv->CryAction->GetGameObject(follow->GetId());

		handle->LinkTo(obj);

		return;
	}

	handle->LinkTo(follow);
}

EntityId CryViewInterop::GetLinkedId(IView *handle)
{
	return handle->GetLinkedId();
}

void CryViewInterop::SetCurrentParams(IView *handle, SViewParams &parameters)
{
	handle->SetCurrentParams(parameters);
}

void CryViewInterop::GetCurrentParams(IView *handle, SViewParams &parameters)
{
	parameters = const_cast<SViewParams &>(*handle->GetCurrentParams());
}

void CryViewInterop::SetViewShake(IView *handle, Ang3 shakeAngle, Vec3 shakeShift, float duration, float frequency, float randomness, int shakeID, bool bFlipVec, bool bUpdateOnly, bool bGroundOnly)
{
	handle->SetViewShake(shakeAngle, shakeShift, duration, frequency, randomness, shakeID, bFlipVec, bUpdateOnly, bGroundOnly);
}

void CryViewInterop::SetViewShakeEx(IView *handle, const IView::SShakeParams &parameters)
{
	handle->SetViewShakeEx(parameters);
}

void CryViewInterop::StopShakeInternal(IView *handle, int shakeId)
{
	handle->StopShake(shakeId);
}

void CryViewInterop::ResetShakingInternal(IView *handle)
{
	handle->ResetShaking();
}

void CryViewInterop::ResetBlendingInternal(IView *handle)
{
	handle->ResetBlending();
}

void CryViewInterop::SetFrameAdditiveCameraAngles(IView *handle, const Ang3 &addFrameAngles)
{
	handle->SetFrameAdditiveCameraAngles(addFrameAngles);
}

void CryViewInterop::SetScale(IView *handle, float scale)
{
	handle->SetScale(scale);
}

void CryViewInterop::SetZoomedScale(IView *handle, float scale)
{
	handle->SetZoomedScale(scale);
}

IView *CryViewInterop::CreateView()
{
	return MonoEnv->CryAction->GetIViewSystem()->CreateView();
}

void CryViewInterop::RemoveView(IView *pView)
{
	MonoEnv->CryAction->GetIViewSystem()->RemoveView(pView);
}

void CryViewInterop::RemoveViewId(uint32 viewId)
{
	MonoEnv->CryAction->GetIViewSystem()->RemoveView(viewId);
}

void CryViewInterop::SetActiveView(IView *pView)
{
	MonoEnv->CryAction->GetIViewSystem()->SetActiveView(pView);
}

void CryViewInterop::SetActiveViewId(uint32 viewId)
{
	MonoEnv->CryAction->GetIViewSystem()->SetActiveView(viewId);
}

IView *CryViewInterop::GetViewInternal(uint32 viewId)
{
	return MonoEnv->CryAction->GetIViewSystem()->GetView(viewId);
}

IView *CryViewInterop::GetActiveViewInternal()
{
	return MonoEnv->CryAction->GetIViewSystem()->GetActiveView();
}

uint32 CryViewInterop::GetViewId(IView *pView)
{
	return MonoEnv->CryAction->GetIViewSystem()->GetViewId(pView);
}

uint32 CryViewInterop::GetActiveViewIdInternal()
{
	return MonoEnv->CryAction->GetIViewSystem()->GetActiveViewId();
}

IView *CryViewInterop::GetViewByEntityId(EntityId id, bool forceCreate)
{
	return MonoEnv->CryAction->GetIViewSystem()->GetViewByEntityId(id, forceCreate);
}

float CryViewInterop::GetDefaultZNear()
{
	return MonoEnv->CryAction->GetIViewSystem()->GetDefaultZNear();
}

bool CryViewInterop::IsPlayingCutScene()
{
	return MonoEnv->CryAction->GetIViewSystem()->IsPlayingCutScene();
}
