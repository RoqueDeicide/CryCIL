#include "stdafx.h"

#include "SkeletonAnimation.h"
#include <ICryAnimation.h>

void SkeletonAnimationInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(SetDebugging);
	REGISTER_METHOD(SetAnimationDrivenMotion);
	REGISTER_METHOD(GetAnimationDrivenMotion);
	REGISTER_METHOD(GetTrackViewStatus);
	REGISTER_METHOD(StartAnimation);
	REGISTER_METHOD(StartAnimationById);
	REGISTER_METHOD(StopAnimationInLayer);
	REGISTER_METHOD(StopAnimationsAllLayers);
	REGISTER_METHOD(FindAnimInFifo);
	REGISTER_METHOD(RemoveAnimFromFifo);
	REGISTER_METHOD(GetNumAnimsInFifo);
	REGISTER_METHOD(ClearFifoLayer);
	REGISTER_METHOD(GetAnimFromFifo);
	REGISTER_METHOD(ManualSeekAnimationInFifo);
	REGISTER_METHOD(RemoveTransitionDelayConditions);
	REGISTER_METHOD(SetLayerBlendWeight);
	REGISTER_METHOD(SetLayerPlaybackScale);
	REGISTER_METHOD(GetLayerPlaybackScale);
	REGISTER_METHOD(SetDesiredMotionParam);
	REGISTER_METHOD(GetDesiredMotionParam);
	REGISTER_METHOD(SetLayerNormalizedTime);
	REGISTER_METHOD(GetLayerNormalizedTime);
	REGISTER_METHOD(GetCurrentVelocity);
	REGISTER_METHOD(GetRelMovement);
}

void SkeletonAnimationInterop::SetDebugging(ISkeletonAnim *handle, bool flags)
{
	handle->SetDebugging(flags ? 1 : 0);
}

void SkeletonAnimationInterop::SetAnimationDrivenMotion(ISkeletonAnim *handle, bool ts)
{
	handle->SetAnimationDrivenMotion(ts ? 1 : 0);
}

bool SkeletonAnimationInterop::GetAnimationDrivenMotion(ISkeletonAnim *handle)
{
	return handle->GetAnimationDrivenMotion() != 0;
}

bool SkeletonAnimationInterop::GetTrackViewStatus(ISkeletonAnim *handle)
{
	return handle->GetTrackViewStatus() != 0;
}

bool SkeletonAnimationInterop::StartAnimation(ISkeletonAnim *handle, mono::string szAnimName0, CryCharAnimationParams &parameters)
{
	return handle->StartAnimation(NtText(szAnimName0), parameters);
}

bool SkeletonAnimationInterop::StartAnimationById(ISkeletonAnim *handle, int id, CryCharAnimationParams &Params)
{
	return handle->StartAnimationById(id, Params);
}

bool SkeletonAnimationInterop::StopAnimationInLayer(ISkeletonAnim *handle, int nLayer, float blendOutTime)
{
	return handle->StopAnimationInLayer(nLayer, blendOutTime);
}

bool SkeletonAnimationInterop::StopAnimationsAllLayers(ISkeletonAnim *handle)
{
	return handle->StopAnimationsAllLayers();
}

CAnimation *SkeletonAnimationInterop::FindAnimInFifo(ISkeletonAnim *handle, uint nUserToken, int nLayer)
{
	return handle->FindAnimInFIFO(nUserToken, nLayer);
}

bool SkeletonAnimationInterop::RemoveAnimFromFifo(ISkeletonAnim *handle, uint nLayer, uint num, bool forceRemove)
{
	return handle->RemoveAnimFromFIFO(nLayer, num, forceRemove);
}

int SkeletonAnimationInterop::GetNumAnimsInFifo(ISkeletonAnim *handle, uint nLayer)
{
	return handle->GetNumAnimsInFIFO(nLayer);
}

void SkeletonAnimationInterop::ClearFifoLayer(ISkeletonAnim *handle, uint nLayer)
{
	handle->ClearFIFOLayer(nLayer);
}

CAnimation *SkeletonAnimationInterop::GetAnimFromFifo(ISkeletonAnim *handle, uint nLayer, uint num)
{
	return &handle->GetAnimFromFIFO(nLayer, num);
}

void SkeletonAnimationInterop::ManualSeekAnimationInFifo(ISkeletonAnim *handle, uint nLayer, uint num, float time,
														 bool triggerAnimEvents)
{
	handle->ManualSeekAnimationInFIFO(nLayer, num, time, triggerAnimEvents);
}

void SkeletonAnimationInterop::RemoveTransitionDelayConditions(ISkeletonAnim *handle, uint nLayer)
{
	handle->RemoveTransitionDelayConditions(nLayer);
}

void SkeletonAnimationInterop::SetLayerBlendWeight(ISkeletonAnim *handle, int nLayer, float fMult)
{
	handle->SetLayerBlendWeight(nLayer, fMult);
}

void SkeletonAnimationInterop::SetLayerPlaybackScale(ISkeletonAnim *handle, int nLayer, float fSpeed)
{
	handle->SetLayerPlaybackScale(nLayer, fSpeed);
}

float SkeletonAnimationInterop::GetLayerPlaybackScale(ISkeletonAnim *handle, uint nLayer)
{
	return handle->GetLayerPlaybackScale(nLayer);
}

void SkeletonAnimationInterop::SetDesiredMotionParam(ISkeletonAnim *handle, EMotionParamID id, float value, float frametime)
{
	handle->SetDesiredMotionParam(id, value, frametime);
}

bool SkeletonAnimationInterop::GetDesiredMotionParam(ISkeletonAnim *handle, EMotionParamID id, float &value)
{
	return handle->GetDesiredMotionParam(id, value);
}

void SkeletonAnimationInterop::SetLayerNormalizedTime(ISkeletonAnim *handle, uint layer, float normalizedTime)
{
	handle->SetLayerNormalizedTime(layer, normalizedTime);
}

float SkeletonAnimationInterop::GetLayerNormalizedTime(ISkeletonAnim *handle, uint layer)
{
	return handle->GetLayerNormalizedTime(layer);
}

Vec3 SkeletonAnimationInterop::GetCurrentVelocity(ISkeletonAnim *handle)
{
	return handle->GetCurrentVelocity();
}

void SkeletonAnimationInterop::GetRelMovement(ISkeletonAnim *handle, QuatT &movement)
{
	movement = handle->GetRelMovement();
}
