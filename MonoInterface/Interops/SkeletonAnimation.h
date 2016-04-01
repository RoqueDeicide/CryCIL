#pragma once

#include "IMonoInterface.h"

enum EMotionParamID;
struct ISkeletonAnim;
class CAnimation;
struct CryCharAnimationParams;

struct SkeletonAnimationInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "SkeletonAnimation"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters"; }

	virtual void InitializeInterops() override;

	static void SetDebugging(ISkeletonAnim *handle, bool flags);
	static void SetAnimationDrivenMotion(ISkeletonAnim *handle, bool ts);
	static bool GetAnimationDrivenMotion(ISkeletonAnim *handle);
	static bool GetTrackViewStatus(ISkeletonAnim *handle);
	static bool StartAnimation(ISkeletonAnim *handle, mono::string szAnimName0, CryCharAnimationParams &parameters);
	static bool StartAnimationById(ISkeletonAnim *handle, int id, CryCharAnimationParams &Params);
	static bool StopAnimationInLayer(ISkeletonAnim *handle, int nLayer, float blendOutTime);
	static bool StopAnimationsAllLayers(ISkeletonAnim *handle);
	static CAnimation *FindAnimInFifo(ISkeletonAnim *handle, uint nUserToken, int nLayer);
	static bool RemoveAnimFromFifo(ISkeletonAnim *handle, uint nLayer, uint num, bool forceRemove);
	static int GetNumAnimsInFifo(ISkeletonAnim *handle, uint nLayer);
	static void ClearFifoLayer(ISkeletonAnim *handle, uint nLayer);
	static CAnimation *GetAnimFromFifo(ISkeletonAnim *handle, uint nLayer, uint num);
	static void ManualSeekAnimationInFifo(ISkeletonAnim *handle, uint nLayer, uint num, float time,
										  bool triggerAnimEvents);
	static void RemoveTransitionDelayConditions(ISkeletonAnim *handle, uint nLayer);
	static void SetLayerBlendWeight(ISkeletonAnim *handle, int nLayer, float fMult);
	static void SetLayerPlaybackScale(ISkeletonAnim *handle, int nLayer, float fSpeed);
	static float GetLayerPlaybackScale(ISkeletonAnim *handle, uint nLayer);
	static void SetDesiredMotionParam(ISkeletonAnim *handle, EMotionParamID id, float value, float frametime);
	static bool GetDesiredMotionParam(ISkeletonAnim *handle, EMotionParamID id, float &value);
	static void SetLayerNormalizedTime(ISkeletonAnim *handle, uint layer, float normalizedTime);
	static float GetLayerNormalizedTime(ISkeletonAnim *handle, uint layer);
	static Vec3 GetCurrentVelocity(ISkeletonAnim *handle);
	static void GetRelMovement(ISkeletonAnim *handle, QuatT &movement);
};