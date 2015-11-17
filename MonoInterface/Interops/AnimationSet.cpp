#include "stdafx.h"

#include "AnimationSet.h"

void AnimationSetInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetAnimationCount);
	REGISTER_METHOD(GetAnimIDByName);
	REGISTER_METHOD(GetNameByAnimID);
	REGISTER_METHOD(GetAnimIDByCRC);
	REGISTER_METHOD(GetCRCByAnimID);
	REGISTER_METHOD(GetFilePathCRCByAnimID);
	REGISTER_METHOD(GetFilePathByID);
	REGISTER_METHOD(GetDuration_sec);
	REGISTER_METHOD(GetAnimationFlags);
	REGISTER_METHOD(GetAnimationSize);
	REGISTER_METHOD(IsAnimLoaded);
	REGISTER_METHOD(AddRef);
	REGISTER_METHOD(Release);
	REGISTER_METHOD(GetAnimationDCCWorldSpaceLocationName);
	REGISTER_METHOD(GetAnimationDCCWorldSpaceLocationId);
	REGISTER_METHOD(GetAnimationDCCWorldSpaceLocationObject);
	REGISTER_METHOD(SampleAnimation);
}

uint AnimationSetInterop::GetAnimationCount(IAnimationSet *handle)
{
	return handle->GetAnimationCount();
}

int AnimationSetInterop::GetAnimIDByName(IAnimationSet *handle, mono::string szAnimationName)
{
	return handle->GetAnimIDByName(NtText(szAnimationName));
}

mono::string AnimationSetInterop::GetNameByAnimID(IAnimationSet *handle, int nAnimationId)
{
	return ToMonoString(handle->GetNameByAnimID(nAnimationId));
}

int AnimationSetInterop::GetAnimIDByCRC(IAnimationSet *handle, uint animationCRC)
{
	return handle->GetAnimIDByCRC(animationCRC);
}

uint AnimationSetInterop::GetCRCByAnimID(IAnimationSet *handle, int nAnimationId)
{
	return handle->GetCRCByAnimID(nAnimationId);
}

uint AnimationSetInterop::GetFilePathCRCByAnimID(IAnimationSet *handle, int nAnimationId)
{
	return handle->GetFilePathCRCByAnimID(nAnimationId);
}

mono::string AnimationSetInterop::GetFilePathByID(IAnimationSet *handle, int nAnimationId)
{
	return ToMonoString(handle->GetFilePathByID(nAnimationId));
}

float AnimationSetInterop::GetDuration_sec(IAnimationSet *handle, int nAnimationId)
{
	return handle->GetDuration_sec(nAnimationId);
}

uint AnimationSetInterop::GetAnimationFlags(IAnimationSet *handle, int nAnimationId)
{
	return handle->GetAnimationFlags(nAnimationId);
}

uint AnimationSetInterop::GetAnimationSize(IAnimationSet *handle, uint nAnimationId)
{
	return handle->GetAnimationSize(nAnimationId);
}

bool AnimationSetInterop::IsAnimLoaded(IAnimationSet *handle, int nAnimationId)
{
	return handle->IsAnimLoaded(nAnimationId);
}

void AnimationSetInterop::AddRef(IAnimationSet *handle, int nAnimationId)
{
	handle->AddRef(nAnimationId);
}

void AnimationSetInterop::Release(IAnimationSet *handle, int nAnimationId)
{
	handle->Release(nAnimationId);
}

bool AnimationSetInterop::GetAnimationDCCWorldSpaceLocationName(IAnimationSet *handle, mono::string szAnimationName, QuatT &startLocation)
{
	return handle->GetAnimationDCCWorldSpaceLocation(NtText(szAnimationName), startLocation);
}

bool AnimationSetInterop::GetAnimationDCCWorldSpaceLocationId(IAnimationSet *handle, int animId, QuatT &startLocation)
{
	return handle->GetAnimationDCCWorldSpaceLocation(animId, startLocation);
}

bool AnimationSetInterop::GetAnimationDCCWorldSpaceLocationObject(IAnimationSet *handle, const CAnimation* pAnim, QuatT &startLocation, uint controllerId)
{
	return handle->GetAnimationDCCWorldSpaceLocation(pAnim, startLocation, controllerId);
}

IAnimationSet::ESampleResult AnimationSetInterop::SampleAnimation(IAnimationSet *handle, int animationId, float animationNormalizedTime, uint controllerId, QuatT &relativeLocationOutput)
{
	return handle->SampleAnimation(animationId, animationNormalizedTime, controllerId, relativeLocationOutput);
}
