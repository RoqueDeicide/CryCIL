#include "stdafx.h"

#include "FacialAnimationSequence.h"
#include "MergingUtility.h"

void FacialAnimationSequenceInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(AddRef);
	REGISTER_METHOD(Release);
	REGISTER_METHOD(StartStreamingInternal);
	REGISTER_METHOD(SetName);
	REGISTER_METHOD(GetName);
	REGISTER_METHOD(SetFlags);
	REGISTER_METHOD(GetFlags);
	REGISTER_METHOD(GetTimeRange);
	REGISTER_METHOD(SetTimeRange);
	REGISTER_METHOD(GetChannelCount);
	REGISTER_METHOD(GetChannel);
	REGISTER_METHOD(CreateChannel);
	REGISTER_METHOD(CreateChannelGroup);
	REGISTER_METHOD(RemoveChannel);
	REGISTER_METHOD(GetSoundEntryCount);
	REGISTER_METHOD(InsertSoundEntry);
	REGISTER_METHOD(DeleteSoundEntry);
	REGISTER_METHOD(GetSoundEntry);
	REGISTER_METHOD(GetSkeletonAnimationEntryCount);
	REGISTER_METHOD(InsertSkeletonAnimationEntry);
	REGISTER_METHOD(DeleteSkeletonAnimationEntry);
	REGISTER_METHOD(GetSkeletonAnimationEntry);
	REGISTER_METHOD(SetJoystickFile);
	REGISTER_METHOD(GetJoystickFile);
	REGISTER_METHOD(Serialize);
	REGISTER_METHOD(MergeSequence);
	REGISTER_METHOD(GetCameraPathPosition);
	REGISTER_METHOD(GetCameraPathOrientation);
	REGISTER_METHOD(GetCameraPathFOV);
	REGISTER_METHOD(IsInMemory);
	REGISTER_METHOD(SetInMemory);
}

void FacialAnimationSequenceInterop::AddRef(IFacialAnimSequence *handle)
{
	handle->AddRef();
}

void FacialAnimationSequenceInterop::Release(IFacialAnimSequence *handle)
{
	handle->Release();
}

bool FacialAnimationSequenceInterop::StartStreamingInternal(IFacialAnimSequence *handle, mono::string sFilename)
{
	return handle->StartStreaming(NtText(sFilename));
}

void FacialAnimationSequenceInterop::SetName(IFacialAnimSequence *handle, mono::string sNewName)
{
	handle->SetName(NtText(sNewName));
}

mono::string FacialAnimationSequenceInterop::GetName(IFacialAnimSequence *handle)
{
	return ToMonoString(handle->GetName());
}

void FacialAnimationSequenceInterop::SetFlags(IFacialAnimSequence *handle, int nFlags)
{
	handle->SetFlags(nFlags);
}

int FacialAnimationSequenceInterop::GetFlags(IFacialAnimSequence *handle)
{
	return handle->GetFlags();
}

Range FacialAnimationSequenceInterop::GetTimeRange(IFacialAnimSequence *handle)
{
	return handle->GetTimeRange();
}

void FacialAnimationSequenceInterop::SetTimeRange(IFacialAnimSequence *handle, Range range)
{
	handle->SetTimeRange(range);
}

int FacialAnimationSequenceInterop::GetChannelCount(IFacialAnimSequence *handle)
{
	return handle->GetChannelCount();
}

IFacialAnimChannel *FacialAnimationSequenceInterop::GetChannel(IFacialAnimSequence *handle, int nIndex)
{
	return handle->GetChannel(nIndex);
}

IFacialAnimChannel *FacialAnimationSequenceInterop::CreateChannel(IFacialAnimSequence *handle)
{
	return handle->CreateChannel();
}

IFacialAnimChannel *FacialAnimationSequenceInterop::CreateChannelGroup(IFacialAnimSequence *handle)
{
	return handle->CreateChannelGroup();
}

void FacialAnimationSequenceInterop::RemoveChannel(IFacialAnimSequence *handle, IFacialAnimChannel *pChannel)
{
	handle->RemoveChannel(pChannel);
}

int FacialAnimationSequenceInterop::GetSoundEntryCount(IFacialAnimSequence *handle)
{
	return handle->GetSoundEntryCount();
}

void FacialAnimationSequenceInterop::InsertSoundEntry(IFacialAnimSequence *handle, int index)
{
	handle->InsertSoundEntry(index);
}

void FacialAnimationSequenceInterop::DeleteSoundEntry(IFacialAnimSequence *handle, int index)
{
	handle->DeleteSoundEntry(index);
}

IFacialAnimSoundEntry *FacialAnimationSequenceInterop::GetSoundEntry(IFacialAnimSequence *handle, int index)
{
	return handle->GetSoundEntry(index);
}

int FacialAnimationSequenceInterop::GetSkeletonAnimationEntryCount(IFacialAnimSequence *handle)
{
	return handle->GetSkeletonAnimationEntryCount();
}

void FacialAnimationSequenceInterop::InsertSkeletonAnimationEntry(IFacialAnimSequence *handle, int index)
{
	handle->InsertSkeletonAnimationEntry(index);
}

void FacialAnimationSequenceInterop::DeleteSkeletonAnimationEntry(IFacialAnimSequence *handle, int index)
{
	handle->DeleteSkeletonAnimationEntry(index);
}

IFacialAnimSkeletonAnimationEntry *FacialAnimationSequenceInterop::GetSkeletonAnimationEntry(IFacialAnimSequence *handle,
																							 int index)
{
	return handle->GetSkeletonAnimationEntry(index);
}

void FacialAnimationSequenceInterop::SetJoystickFile(IFacialAnimSequence *handle, mono::string joystickFile)
{
	handle->SetJoystickFile(NtText(joystickFile));
}

mono::string FacialAnimationSequenceInterop::GetJoystickFile(IFacialAnimSequence *handle)
{
	return ToMonoString(handle->GetJoystickFile());
}

void FacialAnimationSequenceInterop::Serialize(IFacialAnimSequence *handle, IXmlNode *xmlNode, bool bLoading,
											   IFacialAnimSequence::ESerializationFlags flags)
{
	XmlNodeRef nodeRef(xmlNode);
	handle->Serialize(nodeRef, bLoading, flags);
}

void FacialAnimationSequenceInterop::MergeSequence(IFacialAnimSequence *handle, IFacialAnimSequence *pMergeSequence,
												   bool overwrite)
{

	handle->MergeSequence(pMergeSequence, CreateMergingFunctor(overwrite));
}

ISplineInterpolator *FacialAnimationSequenceInterop::GetCameraPathPosition(IFacialAnimSequence *handle)
{
	return handle->GetCameraPathPosition();
}

ISplineInterpolator *FacialAnimationSequenceInterop::GetCameraPathOrientation(IFacialAnimSequence *handle)
{
	return handle->GetCameraPathOrientation();
}

ISplineInterpolator *FacialAnimationSequenceInterop::GetCameraPathFOV(IFacialAnimSequence *handle)
{
	return handle->GetCameraPathFOV();
}

bool FacialAnimationSequenceInterop::IsInMemory(IFacialAnimSequence *handle)
{
	return handle->IsInMemory();
}

void FacialAnimationSequenceInterop::SetInMemory(IFacialAnimSequence *handle, bool bInMemory)
{
	handle->SetInMemory(bInMemory);
}
