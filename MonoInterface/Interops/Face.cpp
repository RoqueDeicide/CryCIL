#include "stdafx.h"

#include "Face.h"
#include <IFacialAnimation.h>

void FaceInterop::InitializeInterops()
{
	REGISTER_METHOD(GetFacialModel);
	REGISTER_METHOD(GetFaceState);
	REGISTER_METHOD(StartEffectorChannel);
	REGISTER_METHOD(StopEffectorChannel);
	REGISTER_METHOD(LoadSequence);
	REGISTER_METHOD(PrecacheFacialExpression);
	REGISTER_METHOD(PlaySequence);
	REGISTER_METHOD(StopSequence);
	REGISTER_METHOD(IsPlaySequence);
	REGISTER_METHOD(PauseSequence);
	REGISTER_METHOD(SeekSequence);
	REGISTER_METHOD(LipSyncWithSound);
	REGISTER_METHOD(EnableProceduralFacialAnimation);
	REGISTER_METHOD(IsProceduralFacialAnimationEnabled);
	REGISTER_METHOD(SetForcedRotations);
	REGISTER_METHOD(SetMasterCharacter);
	REGISTER_METHOD(StopAllSequencesAndChannels);
}

IFacialModel *FaceInterop::GetFacialModel(IFacialInstance *handle)
{
	return handle->GetFacialModel();
}

IFaceState *FaceInterop::GetFaceState(IFacialInstance *handle)
{
	return handle->GetFaceState();
}

uint FaceInterop::StartEffectorChannel(IFacialInstance *handle, IFacialEffector *pEffector, float fWeight, float fFadeTime,
									   float fLifeTime, int nRepeatCount)
{
	return handle->StartEffectorChannel(pEffector, fWeight, fFadeTime, fLifeTime, nRepeatCount);
}

void FaceInterop::StopEffectorChannel(IFacialInstance *handle, uint nChannelID, float fFadeOutTime)
{
	handle->StopEffectorChannel(nChannelID, fFadeOutTime);
}

IFacialAnimSequence *FaceInterop::LoadSequence(IFacialInstance *handle, mono::string sSequenceName, bool addToCache)
{
	return handle->LoadSequence(NtText(sSequenceName), addToCache);
}

void FaceInterop::PrecacheFacialExpression(IFacialInstance *handle, mono::string sSequenceName)
{
	handle->PrecacheFacialExpression(NtText(sSequenceName));
}

void FaceInterop::PlaySequence(IFacialInstance *handle, IFacialAnimSequence *pSequence, EFacialSequenceLayer layer,
							   bool bExclusive, bool bLooping)
{
	handle->PlaySequence(pSequence, layer, bExclusive, bLooping);
}

void FaceInterop::StopSequence(IFacialInstance *handle, EFacialSequenceLayer layer)
{
	handle->StopSequence(layer);
}

bool FaceInterop::IsPlaySequence(IFacialInstance *handle, IFacialAnimSequence *pSequence, EFacialSequenceLayer layer)
{
	return handle->IsPlaySequence(pSequence, layer);
}

void FaceInterop::PauseSequence(IFacialInstance *handle, EFacialSequenceLayer layer, bool bPaused)
{
	handle->PauseSequence(layer, bPaused);
}

void FaceInterop::SeekSequence(IFacialInstance *handle, EFacialSequenceLayer layer, float fTime)
{
	handle->SeekSequence(layer, fTime);
}

void FaceInterop::LipSyncWithSound(IFacialInstance *handle, uint nSoundId, bool bStop)
{
	handle->LipSyncWithSound(nSoundId, bStop);
}

void FaceInterop::EnableProceduralFacialAnimation(IFacialInstance *handle, bool bEnable)
{
	handle->EnableProceduralFacialAnimation(bEnable);
}

bool FaceInterop::IsProceduralFacialAnimationEnabled(IFacialInstance *handle)
{
	return handle->IsProceduralFacialAnimationEnabled();
}

void FaceInterop::SetForcedRotations(IFacialInstance *handle, int numForcedRotations,
									 CFacialAnimForcedRotationEntry* forcedRotations)
{
	handle->SetForcedRotations(numForcedRotations, forcedRotations);
}

void FaceInterop::SetMasterCharacter(IFacialInstance *handle, ICharacterInstance *pMasterInstance)
{
	handle->SetMasterCharacter(pMasterInstance);
}

void FaceInterop::StopAllSequencesAndChannels(IFacialInstance *handle)
{
	handle->StopAllSequencesAndChannels();
}
