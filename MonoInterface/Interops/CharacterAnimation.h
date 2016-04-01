#pragma once

#include "IMonoInterface.h"

class CAnimation;
struct SParametricSampler;

struct CharacterAnimationInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CharacterAnimation"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters"; }

	virtual void InitializeInterops() override;

	static void Serialize(CAnimation *handle, ISerialize *ser);
	static SParametricSampler *GetParametricSampler(CAnimation *handle);
	static int16 GetAnimationId(CAnimation *handle);
	static byte GetCurrentSegmentIndex(CAnimation *handle);
	static bool  HasStaticFlagInternal(CAnimation *handle, uint32 animationFlag);
	static void  SetStaticFlagInternal(CAnimation *handle, uint32 nStaticFlags);
	static void  ClearStaticFlagInternal(CAnimation *handle, uint32 nStaticFlags);
	static float GetCurrentSegmentNormalizedTime(CAnimation *handle);
	static void  SetCurrentSegmentNormalizedTime(CAnimation *handle, float normalizedSegmentTime);
	static float GetTransitionPriority(CAnimation *handle);
	static void  SetTransitionPriority(CAnimation *handle, float transitionPriority);
	static float GetTransitionWeight(CAnimation *handle);
	static void  SetTransitionWeight(CAnimation *handle, float transitionWeight);
	static float GetTransitionTime(CAnimation *handle);
	static void  SetTransitionTime(CAnimation *handle, float transitionTime);
	static float GetPlaybackWeight(CAnimation *handle);
	static void  SetPlaybackWeight(CAnimation *handle, float playbackWeight);
	static float GetPlaybackScale(CAnimation *handle);
	static void  SetPlaybackScale(CAnimation *handle, float playbackScale);
	static uint  GetUserToken(CAnimation *handle);
	static void  SetUserToken(CAnimation *handle, uint nUserToken);
	static float GetExpectedTotalDurationSeconds(CAnimation *handle);
	static void  SetExpectedTotalDurationSeconds(CAnimation *handle, float expectedDurationSeconds);
	static uint  IsActivated(CAnimation *handle);
	static uint  GetLoop(CAnimation *handle);
	static uint  GetEndOfCycle(CAnimation *handle);
	static uint  GetUseTimeWarping(CAnimation *handle);
};