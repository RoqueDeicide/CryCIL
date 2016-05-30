#pragma once

#include "IMonoInterface.h"
#include <CryAnimation/IFacialAnimation.h>

struct FacialEffectorControllerInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "FacialEffectorController"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Faces"; }

	virtual void InitializeInterops() override;

	static IFacialEffCtrl::ControlType  GetControlType(IFacialEffCtrl *handle);
	static void                         SetControlType(IFacialEffCtrl *handle, IFacialEffCtrl::ControlType t);
	static IFacialEffector             *GetEffector(IFacialEffCtrl *handle);
	static float                        GetConstantWeight(IFacialEffCtrl *handle);
	static void                         SetConstantWeight(IFacialEffCtrl *handle, float fWeight);
	static float                        GetConstantBalance(IFacialEffCtrl *handle);
	static void                         SetConstantBalance(IFacialEffCtrl *handle, float fBalance);
	static ISplineInterpolator         *GetSpline(IFacialEffCtrl *handle);
	static float                        EvaluateInternal(IFacialEffCtrl *handle, float fInput);
	static IFacialEffCtrl::ControlFlags GetFlags(IFacialEffCtrl *handle);
	static void                         SetFlags(IFacialEffCtrl *handle, IFacialEffCtrl::ControlFlags nFlags);
};