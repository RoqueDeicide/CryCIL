#include "stdafx.h"

#include "FacialEffectorController.h"

void FacialEffectorControllerInterop::InitializeInterops()
{
	REGISTER_METHOD(GetControlType);
	REGISTER_METHOD(SetControlType);
	REGISTER_METHOD(GetEffector);
	REGISTER_METHOD(GetConstantWeight);
	REGISTER_METHOD(SetConstantWeight);
	REGISTER_METHOD(GetConstantBalance);
	REGISTER_METHOD(SetConstantBalance);
	REGISTER_METHOD(GetSpline);
	REGISTER_METHOD(EvaluateInternal);
	REGISTER_METHOD(GetFlags);
	REGISTER_METHOD(SetFlags);
}

IFacialEffCtrl::ControlType FacialEffectorControllerInterop::GetControlType(IFacialEffCtrl *handle)
{
	return handle->GetType();
}

void FacialEffectorControllerInterop::SetControlType(IFacialEffCtrl *handle, IFacialEffCtrl::ControlType t)
{
	handle->SetType(t);
}

IFacialEffector *FacialEffectorControllerInterop::GetEffector(IFacialEffCtrl *handle)
{
	return handle->GetEffector();
}

float FacialEffectorControllerInterop::GetConstantWeight(IFacialEffCtrl *handle)
{
	return handle->GetConstantWeight();
}

void FacialEffectorControllerInterop::SetConstantWeight(IFacialEffCtrl *handle, float fWeight)
{
	handle->SetConstantWeight(fWeight);
}

float FacialEffectorControllerInterop::GetConstantBalance(IFacialEffCtrl *handle)
{
	return handle->GetConstantBalance();
}

void FacialEffectorControllerInterop::SetConstantBalance(IFacialEffCtrl *handle, float fBalance)
{
	handle->SetConstantBalance(fBalance);
}

ISplineInterpolator *FacialEffectorControllerInterop::GetSpline(IFacialEffCtrl *handle)
{
	return handle->GetSpline();
}

float FacialEffectorControllerInterop::EvaluateInternal(IFacialEffCtrl *handle, float fInput)
{
	return handle->Evaluate(fInput);
}

IFacialEffCtrl::ControlFlags FacialEffectorControllerInterop::GetFlags(IFacialEffCtrl *handle)
{
	return IFacialEffCtrl::ControlFlags(handle->GetFlags());
}

void FacialEffectorControllerInterop::SetFlags(IFacialEffCtrl *handle, IFacialEffCtrl::ControlFlags nFlags)
{
	handle->SetFlags(nFlags);
}
