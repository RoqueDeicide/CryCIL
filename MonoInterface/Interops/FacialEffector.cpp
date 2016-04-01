#include "stdafx.h"

#include "FacialEffector.h"
#include <IFacialAnimation.h>

void FacialEffectorInterop::InitializeInterops()
{
	REGISTER_METHOD(SetIdentifier);
	REGISTER_METHOD(GetIdentifier);
	REGISTER_METHOD(GetEffectorType);
	REGISTER_METHOD(SetFlags);
	REGISTER_METHOD(GetFlags);
	REGISTER_METHOD(GetIndexInState);
	REGISTER_METHOD(SetParamString);
	REGISTER_METHOD(GetParamString);
	REGISTER_METHOD(SetParamVec3);
	REGISTER_METHOD(GetParamVec3);
	REGISTER_METHOD(SetParamInt);
	REGISTER_METHOD(GetParamInt);
	REGISTER_METHOD(GetSubEffectorCount);
	REGISTER_METHOD(GetSubEffector);
	REGISTER_METHOD(GetSubEffCtrl);
	REGISTER_METHOD(GetSubEffCtrlByName);
	REGISTER_METHOD(AddSubEffector);
	REGISTER_METHOD(RemoveSubEffector);
	REGISTER_METHOD(RemoveAllSubEffectors);
}

void FacialEffectorInterop::SetIdentifier(IFacialEffector *handle, CFaceIdentifierHandle ident)
{
	handle->SetIdentifier(ident);
}

CFaceIdentifierHandle FacialEffectorInterop::GetIdentifier(IFacialEffector *handle)
{
	return handle->GetIdentifier();
}

EFacialEffectorType FacialEffectorInterop::GetEffectorType(IFacialEffector *handle)
{
	return handle->GetType();
}

void FacialEffectorInterop::SetFlags(IFacialEffector *handle, uint32 nFlags)
{
	handle->SetFlags(nFlags);
}

uint32 FacialEffectorInterop::GetFlags(IFacialEffector *handle)
{
	return handle->GetFlags();
}

int FacialEffectorInterop::GetIndexInState(IFacialEffector *handle)
{
	return handle->GetIndexInState();
}

void FacialEffectorInterop::SetParamString(IFacialEffector *handle, EFacialEffectorParam param, mono::string str)
{
	handle->SetParamString(param, NtText(str));
}

mono::string FacialEffectorInterop::GetParamString(IFacialEffector *handle, EFacialEffectorParam param)
{
	return ToMonoString(handle->GetParamString(param));
}

void FacialEffectorInterop::SetParamVec3(IFacialEffector *handle, EFacialEffectorParam param, Vec3 vValue)
{
	handle->SetParamVec3(param, vValue);
}

Vec3 FacialEffectorInterop::GetParamVec3(IFacialEffector *handle, EFacialEffectorParam param)
{
	return handle->GetParamVec3(param);
}

void FacialEffectorInterop::SetParamInt(IFacialEffector *handle, EFacialEffectorParam param, int nValue)
{
	handle->SetParamInt(param, nValue);
}

int FacialEffectorInterop::GetParamInt(IFacialEffector *handle, EFacialEffectorParam param)
{
	return handle->GetParamInt(param);
}

int FacialEffectorInterop::GetSubEffectorCount(IFacialEffector *handle)
{
	return handle->GetSubEffectorCount();
}

IFacialEffector *FacialEffectorInterop::GetSubEffector(IFacialEffector *handle, int nIndex)
{
	return handle->GetSubEffector(nIndex);
}

IFacialEffCtrl *FacialEffectorInterop::GetSubEffCtrl(IFacialEffector *handle, int nIndex)
{
	return handle->GetSubEffCtrl(nIndex);
}

IFacialEffCtrl *FacialEffectorInterop::GetSubEffCtrlByName(IFacialEffector *handle, mono::string effectorName)
{
	return handle->GetSubEffCtrlByName(NtText(effectorName));
}

IFacialEffCtrl *FacialEffectorInterop::AddSubEffector(IFacialEffector *handle, IFacialEffector *pEffector)
{
	return handle->AddSubEffector(pEffector);
}

void FacialEffectorInterop::RemoveSubEffector(IFacialEffector *handle, IFacialEffector *pEffector)
{
	handle->RemoveSubEffector(pEffector);
}

void FacialEffectorInterop::RemoveAllSubEffectors(IFacialEffector *handle)
{
	handle->RemoveAllSubEffectors();
}
