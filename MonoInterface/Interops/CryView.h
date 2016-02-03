#pragma once

#include "IMonoInterface.h"
#include <IViewSystem.h>

struct CryViewInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryView"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Rendering.Views"; }

	virtual void OnRunTimeInitialized() override;

	static void UpdateInternal(IView *handle, float frameTime, bool isActive);
	static void LinkToInternal(IView *handle, IEntity *follow, bool gameObject);
	static EntityId GetLinkedId(IView *handle);
	static void SetCurrentParams(IView *handle, SViewParams &parameters);
	static void GetCurrentParams(IView *handle, SViewParams &parameters);
	static void SetViewShake(IView *handle, Ang3 shakeAngle, Vec3 shakeShift, float duration, float frequency, float randomness, int shakeID, bool bFlipVec, bool bUpdateOnly, bool bGroundOnly);
	static void SetViewShakeEx(IView *handle, const IView::SShakeParams &parameters);
	static void StopShakeInternal(IView *handle, int shakeId);
	static void ResetShakingInternal(IView *handle);
	static void ResetBlendingInternal(IView *handle);
	static void SetFrameAdditiveCameraAngles(IView *handle, const Ang3 &addFrameAngles);
	static void SetScale(IView *handle, float scale);
	static void SetZoomedScale(IView *handle, float scale);

	static IView *CreateView();
	static void RemoveView(IView *pView);
	static void RemoveViewId(uint32 viewId);
	static void SetActiveView(IView *pView);
	static void SetActiveViewId(uint32 viewId);
	static IView *GetViewInternal(uint32 viewId);
	static IView *GetActiveViewInternal();
	static uint32 GetViewId(IView *pView);
	static uint32 GetActiveViewIdInternal();
	static IView *GetViewByEntityId(EntityId id, bool forceCreate);
	static float GetDefaultZNear();
	static bool IsPlayingCutScene();
};