#include "stdafx.h"

#include "CryEntityAudioProxy.h"

void CryEntityAudioProxyInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(SetFadeDistance);
	REGISTER_METHOD(GetFadeDistance);
	REGISTER_METHOD(SetEnvironmentFadeDistance);
	REGISTER_METHOD(GetEnvironmentFadeDistance);
	REGISTER_METHOD(SetEnvironmentID);
	REGISTER_METHOD(GetEnvironmentID);
	REGISTER_METHOD(CreateAuxAudioProxy);
	REGISTER_METHOD(RemoveAuxAudioProxy);
	REGISTER_METHOD(SetAuxAudioProxyOffset);
	REGISTER_METHOD(GetAuxAudioProxyOffset);
	REGISTER_METHOD(ExecuteTriggerInternal);
	REGISTER_METHOD(StopTriggerInternal);
	REGISTER_METHOD(SetSwitchStateInternal);
	REGISTER_METHOD(SetRtpcValueInternal);
	REGISTER_METHOD(SetObstructionCalcTypeInternal);
	REGISTER_METHOD(SetEnvironmentAmountInternal);
	REGISTER_METHOD(SetCurrentEnvironmentsInternal);
	REGISTER_METHOD(AuxAudioProxiesMoveWithEntity);
}

void CryEntityAudioProxyInterop::SetFadeDistance(IEntityAudioProxy *handle, float fFadeDistance)
{
	handle->SetFadeDistance(fFadeDistance);
}

float CryEntityAudioProxyInterop::GetFadeDistance(IEntityAudioProxy *handle)
{
	return handle->GetFadeDistance();
}

void CryEntityAudioProxyInterop::SetEnvironmentFadeDistance(IEntityAudioProxy *handle, float fEnvironmentFadeDistance)
{
	handle->SetEnvironmentFadeDistance(fEnvironmentFadeDistance);
}

float CryEntityAudioProxyInterop::GetEnvironmentFadeDistance(IEntityAudioProxy *handle)
{
	return handle->GetEnvironmentFadeDistance();
}

void CryEntityAudioProxyInterop::SetEnvironmentID(IEntityAudioProxy *handle, TATLIDType nEnvironmentID)
{
	handle->SetEnvironmentID(nEnvironmentID);
}

TATLIDType CryEntityAudioProxyInterop::GetEnvironmentID(IEntityAudioProxy *handle)
{
	return handle->GetEnvironmentID();
}

TATLIDType CryEntityAudioProxyInterop::CreateAuxAudioProxy(IEntityAudioProxy *handle)
{
	return handle->CreateAuxAudioProxy();
}

bool CryEntityAudioProxyInterop::RemoveAuxAudioProxy(IEntityAudioProxy *handle, TATLIDType nAudioProxyLocalID)
{
	return handle->RemoveAuxAudioProxy(nAudioProxyLocalID);
}

void CryEntityAudioProxyInterop::SetAuxAudioProxyOffset(IEntityAudioProxy *handle, const Matrix34 &rOffset, TATLIDType nAudioProxyLocalID)
{
	handle->SetAuxAudioProxyOffset(rOffset, nAudioProxyLocalID);
}

void CryEntityAudioProxyInterop::GetAuxAudioProxyOffset(IEntityAudioProxy *handle, Matrix34 &offset, TATLIDType nAudioProxyLocalID)
{
	offset = handle->GetAuxAudioProxyOffset(nAudioProxyLocalID).mPosition;
}

bool CryEntityAudioProxyInterop::ExecuteTriggerInternal(IEntityAudioProxy *handle, TATLIDType nTriggerID, ELipSyncMethod eLipSyncMethod, TATLIDType nAudioProxyLocalID)
{
	return handle->ExecuteTrigger(nTriggerID, eLipSyncMethod, nAudioProxyLocalID);
}

void CryEntityAudioProxyInterop::StopTriggerInternal(IEntityAudioProxy *handle, TATLIDType nTriggerID, TATLIDType nAudioProxyLocalID)
{
	handle->StopTrigger(nTriggerID, nAudioProxyLocalID);
}

void CryEntityAudioProxyInterop::SetSwitchStateInternal(IEntityAudioProxy *handle, TATLIDType nSwitchID, TATLIDType nStateID, TATLIDType nAudioProxyLocalID)
{
	handle->SetSwitchState(nSwitchID, nStateID, nAudioProxyLocalID);
}

void CryEntityAudioProxyInterop::SetRtpcValueInternal(IEntityAudioProxy *handle, TATLIDType nRtpcID, float fValue, TATLIDType nAudioProxyLocalID)
{
	handle->SetRtpcValue(nRtpcID, fValue, nAudioProxyLocalID);
}

void CryEntityAudioProxyInterop::SetObstructionCalcTypeInternal(IEntityAudioProxy *handle, EAudioObjectObstructionCalcType eObstructionType, TATLIDType nAudioProxyLocalID)
{
	handle->SetObstructionCalcType(eObstructionType, nAudioProxyLocalID);
}

void CryEntityAudioProxyInterop::SetEnvironmentAmountInternal(IEntityAudioProxy *handle, TATLIDType nEnvironmentID, float fAmount, TATLIDType nAudioProxyLocalID)
{
	handle->SetEnvironmentAmount(nEnvironmentID, fAmount, nAudioProxyLocalID);
}

void CryEntityAudioProxyInterop::SetCurrentEnvironmentsInternal(IEntityAudioProxy *handle, TATLIDType nAudioProxyLocalID)
{
	handle->SetCurrentEnvironments(nAudioProxyLocalID);
}

void CryEntityAudioProxyInterop::AuxAudioProxiesMoveWithEntity(IEntityAudioProxy *handle, bool bCanMoveWithEntity)
{
	handle->AuxAudioProxiesMoveWithEntity(bCanMoveWithEntity);
}
