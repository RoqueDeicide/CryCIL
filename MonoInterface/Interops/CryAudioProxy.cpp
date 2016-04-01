#include "stdafx.h"

#include "CryAudioProxy.h"

void CryAudioProxyInterop::InitializeInterops()
{
	REGISTER_METHOD(Init);
	REGISTER_METHOD(ReleaseInternal);
	REGISTER_METHOD(ResetInternal);
	REGISTER_METHOD(ExecuteTriggerInternal);
	REGISTER_METHOD(StopTriggerInternal);
	REGISTER_METHOD(SetSwitchStateInternal);
	REGISTER_METHOD(SetRtpcValueInternal);
	REGISTER_METHOD(SetObstructionCalcTypeInternal);
	REGISTER_METHOD(SetTransformation);
	REGISTER_METHOD(SetPosition);
	REGISTER_METHOD(SetEnvironmentAmountInternal);
	REGISTER_METHOD(SetCurrentEnvironmentsInternal);
	REGISTER_METHOD(GetAudioObjectID);
}

void CryAudioProxyInterop::Init(IAudioProxy *handle, mono::string sObjectName, bool bInitAsync)
{
	handle->Initialize(NtText(sObjectName), bInitAsync);
}

void CryAudioProxyInterop::ReleaseInternal(IAudioProxy *handle)
{
	handle->Release();
}

void CryAudioProxyInterop::ResetInternal(IAudioProxy *handle)
{
	handle->Reset();
}

void CryAudioProxyInterop::ExecuteTriggerInternal(IAudioProxy *handle, uint nTriggerID, ELipSyncMethod eLipSyncMethod)
{
	handle->ExecuteTrigger(nTriggerID, eLipSyncMethod);
}

void CryAudioProxyInterop::StopTriggerInternal(IAudioProxy *handle, uint nTriggerID)
{
	handle->StopTrigger(nTriggerID);
}

void CryAudioProxyInterop::SetSwitchStateInternal(IAudioProxy *handle, uint nSwitchID, uint nStateID)
{
	handle->SetSwitchState(nSwitchID, nStateID);
}

void CryAudioProxyInterop::SetRtpcValueInternal(IAudioProxy *handle, uint nRtpcID, float fValue)
{
	handle->SetRtpcValue(nRtpcID, fValue);
}

void CryAudioProxyInterop::SetObstructionCalcTypeInternal(IAudioProxy *handle, EAudioObjectObstructionCalcType eObstructionType)
{
	handle->SetObstructionCalcType(eObstructionType);
}

void CryAudioProxyInterop::SetTransformation(IAudioProxy *handle, const Matrix34 &rPosition)
{
	handle->SetPosition(rPosition);
}

void CryAudioProxyInterop::SetPosition(IAudioProxy *handle, const Vec3 &rPosition)
{
	handle->SetPosition(rPosition);
}

void CryAudioProxyInterop::SetEnvironmentAmountInternal(IAudioProxy *handle, uint nEnvironmentID, float fAmount)
{
	handle->SetEnvironmentAmount(nEnvironmentID, fAmount);
}

void CryAudioProxyInterop::SetCurrentEnvironmentsInternal(IAudioProxy *handle, EntityId nEntityToIgnore)
{
	handle->SetCurrentEnvironments(nEntityToIgnore);
}

uint CryAudioProxyInterop::GetAudioObjectID(IAudioProxy *handle)
{
	return handle->GetAudioObjectID();
}
