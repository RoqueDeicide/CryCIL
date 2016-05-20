#include "stdafx.h"

#include "CryAudioProxy.h"

void CryAudioProxyInterop::InitializeInterops()
{
	REGISTER_METHOD(Init);
	REGISTER_METHOD(ReleaseInternal);
	REGISTER_METHOD(ResetInternal);
	REGISTER_METHOD(PlayFileInternal);
	REGISTER_METHOD(StopFileInternal);
	REGISTER_METHOD(ExecuteTriggerInternal);
	REGISTER_METHOD(StopTriggerInternal);
	REGISTER_METHOD(SetSwitchStateInternal);
	REGISTER_METHOD(SetRtpcValueInternal);
	REGISTER_METHOD(SetObstructionCalcTypeInternal);
	REGISTER_METHOD(SetTransformation);
	REGISTER_METHOD(SetPosition);
	REGISTER_METHOD(SetEnvironmentAmountInternal);
	REGISTER_METHOD(SetCurrentEnvironmentsInternal);
	REGISTER_METHOD(GetAudioObjectId);
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

void CryAudioProxyInterop::PlayFileInternal(IAudioProxy *handle, mono::string file)
{
	handle->PlayFile(NtText(file));
}

void CryAudioProxyInterop::StopFileInternal(IAudioProxy *handle, mono::string file)
{
	handle->StopFile(NtText(file));
}

void CryAudioProxyInterop::ExecuteTriggerInternal(IAudioProxy *handle, uint nTriggerID)
{
	handle->ExecuteTrigger(nTriggerID);
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

void CryAudioProxyInterop::SetObstructionCalcTypeInternal(IAudioProxy *handle, EAudioOcclusionType eOcclusionType)
{
	handle->SetOcclusionType(eOcclusionType);
}

void CryAudioProxyInterop::SetTransformation(IAudioProxy *handle, const Matrix34 &rPosition)
{
	handle->SetTransformation(rPosition);
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

uint CryAudioProxyInterop::GetAudioObjectId(IAudioProxy *handle)
{
	return handle->GetAudioObjectId();
}
