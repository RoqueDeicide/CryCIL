#include "stdafx.h"

#include "FacialAnimationSoundEntry.h"
#include <IFacialAnimation.h>

void FacialAnimationSoundEntryInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(SetSoundFile);
	REGISTER_METHOD(GetSoundFile);
	REGISTER_METHOD(GetSentence);
	REGISTER_METHOD(GetStartTime);
	REGISTER_METHOD(SetStartTime);
}

void FacialAnimationSoundEntryInterop::SetSoundFile(IFacialAnimSoundEntry *handle, mono::string sSoundFile)
{
	handle->SetSoundFile(NtText(sSoundFile));
}

mono::string FacialAnimationSoundEntryInterop::GetSoundFile(IFacialAnimSoundEntry *handle)
{
	return ToMonoString(handle->GetSoundFile());
}

IFacialSentence *FacialAnimationSoundEntryInterop::GetSentence(IFacialAnimSoundEntry *handle)
{
	return handle->GetSentence();
}

float FacialAnimationSoundEntryInterop::GetStartTime(IFacialAnimSoundEntry *handle)
{
	return handle->GetStartTime();
}

void FacialAnimationSoundEntryInterop::SetStartTime(IFacialAnimSoundEntry *handle, float time)
{
	handle->SetStartTime(time);
}
