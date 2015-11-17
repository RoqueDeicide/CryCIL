#include "stdafx.h"

#include "CharacterAnimation.h"
#include <CryCharAnimationParams.h>

void CharacterAnimationInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(Serialize);
	REGISTER_METHOD(GetParametricSampler);
	REGISTER_METHOD(GetAnimationId);
	REGISTER_METHOD(GetCurrentSegmentIndex);
	REGISTER_METHOD(HasStaticFlagInternal);
	REGISTER_METHOD(SetStaticFlagInternal);
	REGISTER_METHOD(ClearStaticFlagInternal);
	REGISTER_METHOD(GetCurrentSegmentNormalizedTime);
	REGISTER_METHOD(SetCurrentSegmentNormalizedTime);
	REGISTER_METHOD(GetTransitionPriority);
	REGISTER_METHOD(SetTransitionPriority);
	REGISTER_METHOD(GetTransitionWeight);
	REGISTER_METHOD(SetTransitionWeight);
	REGISTER_METHOD(GetTransitionTime);
	REGISTER_METHOD(SetTransitionTime);
	REGISTER_METHOD(GetPlaybackWeight);
	REGISTER_METHOD(SetPlaybackWeight);
	REGISTER_METHOD(GetPlaybackScale);
	REGISTER_METHOD(SetPlaybackScale);
	REGISTER_METHOD(GetUserToken);
	REGISTER_METHOD(SetUserToken);
	REGISTER_METHOD(GetExpectedTotalDurationSeconds);
	REGISTER_METHOD(SetExpectedTotalDurationSeconds);
	REGISTER_METHOD(IsActivated);
	REGISTER_METHOD(GetLoop);
	REGISTER_METHOD(GetEndOfCycle);
	REGISTER_METHOD(GetUseTimeWarping);
}

void CharacterAnimationInterop::Serialize(CAnimation *handle, ISerialize *ser)
{
	handle->Serialize(ser);
}

SParametricSampler *CharacterAnimationInterop::GetParametricSampler(CAnimation *handle)
{
	return handle->GetParametricSampler();
}

int16 CharacterAnimationInterop::GetAnimationId(CAnimation *handle)
{
	return handle->GetAnimationId();
}

byte CharacterAnimationInterop::GetCurrentSegmentIndex(CAnimation *handle)
{
	return handle->GetCurrentSegmentIndex();
}

bool CharacterAnimationInterop::HasStaticFlagInternal(CAnimation *handle, uint32 animationFlag)
{
	return handle->HasStaticFlag(animationFlag);
}

void CharacterAnimationInterop::SetStaticFlagInternal(CAnimation *handle, uint32 nStaticFlags)
{
	handle->SetStaticFlag(nStaticFlags);
}

void CharacterAnimationInterop::ClearStaticFlagInternal(CAnimation *handle, uint32 nStaticFlags)
{
	handle->ClearStaticFlag(nStaticFlags);
}

float CharacterAnimationInterop::GetCurrentSegmentNormalizedTime(CAnimation *handle)
{
	return handle->GetCurrentSegmentNormalizedTime();
}

void CharacterAnimationInterop::SetCurrentSegmentNormalizedTime(CAnimation *handle, float normalizedSegmentTime)
{
	handle->SetCurrentSegmentNormalizedTime(normalizedSegmentTime);
}

float CharacterAnimationInterop::GetTransitionPriority(CAnimation *handle)
{
	return handle->GetTransitionPriority();
}

void CharacterAnimationInterop::SetTransitionPriority(CAnimation *handle, float transitionPriority)
{
	handle->SetTransitionPriority(transitionPriority);
}

float CharacterAnimationInterop::GetTransitionWeight(CAnimation *handle)
{
	return handle->GetTransitionWeight();
}

void CharacterAnimationInterop::SetTransitionWeight(CAnimation *handle, float transitionWeight)
{
	handle->SetTransitionWeight(transitionWeight);
}

float CharacterAnimationInterop::GetTransitionTime(CAnimation *handle)
{
	return handle->GetTransitionTime();
}

void CharacterAnimationInterop::SetTransitionTime(CAnimation *handle, float transitionTime)
{
	handle->SetTransitionTime(transitionTime);
}

float CharacterAnimationInterop::GetPlaybackWeight(CAnimation *handle)
{
	return handle->GetPlaybackWeight();
}

void CharacterAnimationInterop::SetPlaybackWeight(CAnimation *handle, float playbackWeight)
{
	handle->SetPlaybackWeight(playbackWeight);
}

float CharacterAnimationInterop::GetPlaybackScale(CAnimation *handle)
{
	return handle->GetPlaybackScale();
}

void CharacterAnimationInterop::SetPlaybackScale(CAnimation *handle, float playbackScale)
{
	handle->SetPlaybackScale(playbackScale);
}

uint CharacterAnimationInterop::GetUserToken(CAnimation *handle)
{
	return handle->GetUserToken();
}

void CharacterAnimationInterop::SetUserToken(CAnimation *handle, uint nUserToken)
{
	handle->SetUserToken(nUserToken);
}

float CharacterAnimationInterop::GetExpectedTotalDurationSeconds(CAnimation *handle)
{
	return handle->GetExpectedTotalDurationSeconds();
}

void CharacterAnimationInterop::SetExpectedTotalDurationSeconds(CAnimation *handle, float expectedDurationSeconds)
{
	handle->SetExpectedTotalDurationSeconds(expectedDurationSeconds);
}

uint CharacterAnimationInterop::IsActivated(CAnimation *handle)
{
	return handle->IsActivated();
}

uint CharacterAnimationInterop::GetLoop(CAnimation *handle)
{
	return handle->GetLoop();
}

uint CharacterAnimationInterop::GetEndOfCycle(CAnimation *handle)
{
	return handle->GetEndOfCycle();
}

uint CharacterAnimationInterop::GetUseTimeWarping(CAnimation *handle)
{
	return handle->GetUseTimeWarping();
}
