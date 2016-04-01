#pragma once

#include "IMonoInterface.h"
#include <Range.h>
#include <IFacialAnimation.h>

struct FacialAnimationSequenceInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "FacialAnimationSequence"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Faces"; }

	virtual void InitializeInterops() override;

	static void                 AddRef(IFacialAnimSequence *handle);
	static void                 Release(IFacialAnimSequence *handle);
	static bool                 StartStreamingInternal(IFacialAnimSequence *handle, mono::string sFilename);
	static void                 SetName(IFacialAnimSequence *handle, mono::string sNewName);
	static mono::string         GetName(IFacialAnimSequence *handle);
	static void                 SetFlags(IFacialAnimSequence *handle, int nFlags);
	static int                  GetFlags(IFacialAnimSequence *handle);
	static Range                GetTimeRange(IFacialAnimSequence *handle);
	static void                 SetTimeRange(IFacialAnimSequence *handle, Range range);
	static int                  GetChannelCount(IFacialAnimSequence *handle);
	static IFacialAnimChannel  *GetChannel(IFacialAnimSequence *handle, int nIndex);
	static IFacialAnimChannel  *CreateChannel(IFacialAnimSequence *handle);
	static IFacialAnimChannel  *CreateChannelGroup(IFacialAnimSequence *handle);
	static void                 RemoveChannel(IFacialAnimSequence *handle, IFacialAnimChannel *pChannel);
	static int                  GetSoundEntryCount(IFacialAnimSequence *handle);
	static void                 InsertSoundEntry(IFacialAnimSequence *handle, int index);
	static void                 DeleteSoundEntry(IFacialAnimSequence *handle, int index);
	static IFacialAnimSoundEntry *GetSoundEntry(IFacialAnimSequence *handle, int index);
	static int                  GetSkeletonAnimationEntryCount(IFacialAnimSequence *handle);
	static void                 InsertSkeletonAnimationEntry(IFacialAnimSequence *handle, int index);
	static void                 DeleteSkeletonAnimationEntry(IFacialAnimSequence *handle, int index);
	static IFacialAnimSkeletonAnimationEntry *GetSkeletonAnimationEntry(IFacialAnimSequence *handle, int index);
	static void                 SetJoystickFile(IFacialAnimSequence *handle, mono::string joystickFile);
	static mono::string         GetJoystickFile(IFacialAnimSequence *handle);
	static void                 Serialize(IFacialAnimSequence *handle, IXmlNode *xmlNode, bool bLoading, IFacialAnimSequence::ESerializationFlags flags);
	static void                 MergeSequence(IFacialAnimSequence *handle, IFacialAnimSequence *pMergeSequence, bool overwrite);
	static ISplineInterpolator *GetCameraPathPosition(IFacialAnimSequence *handle);
	static ISplineInterpolator *GetCameraPathOrientation(IFacialAnimSequence *handle);
	static ISplineInterpolator *GetCameraPathFOV(IFacialAnimSequence *handle);
	static bool                 IsInMemory(IFacialAnimSequence *handle);
	static void                 SetInMemory(IFacialAnimSequence *handle, bool bInMemory);
};