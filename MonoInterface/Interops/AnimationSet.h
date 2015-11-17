#pragma once

#include "IMonoInterface.h"
#include <ICryAnimation.h>

struct AnimationSetInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "AnimationSet"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters"; }

	virtual void OnRunTimeInitialized() override;

	static uint         GetAnimationCount(IAnimationSet *handle);
	static int          GetAnimIDByName(IAnimationSet *handle, mono::string szAnimationName);
	static mono::string GetNameByAnimID(IAnimationSet *handle, int nAnimationId);
	static int          GetAnimIDByCRC(IAnimationSet *handle, uint animationCRC);
	static uint         GetCRCByAnimID(IAnimationSet *handle, int nAnimationId);
	static uint         GetFilePathCRCByAnimID(IAnimationSet *handle, int nAnimationId);
	static mono::string GetFilePathByID(IAnimationSet *handle, int nAnimationId);
	static float        GetDuration_sec(IAnimationSet *handle, int nAnimationId);
	static uint         GetAnimationFlags(IAnimationSet *handle, int nAnimationId);
	static uint         GetAnimationSize(IAnimationSet *handle, uint nAnimationId);
	static bool         IsAnimLoaded(IAnimationSet *handle, int nAnimationId);
	static void         AddRef(IAnimationSet *handle, int nAnimationId);
	static void         Release(IAnimationSet *handle, int nAnimationId);
	static bool         GetAnimationDCCWorldSpaceLocationName(IAnimationSet *handle, mono::string szAnimationName, QuatT &startLocation);
	static bool         GetAnimationDCCWorldSpaceLocationId(IAnimationSet *handle, int animId, QuatT &startLocation);
	static bool         GetAnimationDCCWorldSpaceLocationObject(IAnimationSet *handle, const CAnimation* pAnim, QuatT &startLocation, uint controllerId);
	static IAnimationSet::ESampleResult SampleAnimation(IAnimationSet *handle, int animationId, float animationNormalizedTime, uint controllerId, QuatT &relativeLocationOutput);
};