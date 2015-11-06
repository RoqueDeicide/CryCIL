#include "stdafx.h"

#include "FacialAnimationSkeletonAnimationEntry.h"
#include <IFacialAnimation.h>

void FacialAnimationSkeletonAnimationEntryInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(SetName);
	REGISTER_METHOD(GetName);
	REGISTER_METHOD(SetStartTime);
	REGISTER_METHOD(GetStartTime);
	REGISTER_METHOD(SetEndTime);
	REGISTER_METHOD(GetEndTime);
}

void FacialAnimationSkeletonAnimationEntryInterop::SetName(IFacialAnimSkeletonAnimationEntry *handle,
														   mono::string skeletonAnimationFile)
{
	handle->SetName(NtText(skeletonAnimationFile));
}

mono::string FacialAnimationSkeletonAnimationEntryInterop::GetName(IFacialAnimSkeletonAnimationEntry *handle)
{
	return ToMonoString(handle->GetName());
}

void FacialAnimationSkeletonAnimationEntryInterop::SetStartTime(IFacialAnimSkeletonAnimationEntry *handle, float time)
{
	handle->SetStartTime(time);
}

float FacialAnimationSkeletonAnimationEntryInterop::GetStartTime(IFacialAnimSkeletonAnimationEntry *handle)
{
	return handle->GetStartTime();
}

void FacialAnimationSkeletonAnimationEntryInterop::SetEndTime(IFacialAnimSkeletonAnimationEntry *handle, float time)
{
	handle->SetEndTime(time);
}

float FacialAnimationSkeletonAnimationEntryInterop::GetEndTime(IFacialAnimSkeletonAnimationEntry *handle)
{
	return handle->GetEndTime();
}
