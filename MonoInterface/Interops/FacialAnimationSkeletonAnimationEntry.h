#pragma once

#include "IMonoInterface.h"

struct IFacialAnimSkeletonAnimationEntry;

struct FacialAnimationSkeletonAnimationEntryInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "FacialAnimationSkeletonAnimationEntry"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Faces"; }

	virtual void InitializeInterops() override;

	static void         SetName(IFacialAnimSkeletonAnimationEntry *handle, mono::string skeletonAnimationFile);
	static mono::string GetName(IFacialAnimSkeletonAnimationEntry *handle);
	static void         SetStartTime(IFacialAnimSkeletonAnimationEntry *handle, float time);
	static float        GetStartTime(IFacialAnimSkeletonAnimationEntry *handle);
	static void         SetEndTime(IFacialAnimSkeletonAnimationEntry *handle, float time);
	static float        GetEndTime(IFacialAnimSkeletonAnimationEntry *handle);
};