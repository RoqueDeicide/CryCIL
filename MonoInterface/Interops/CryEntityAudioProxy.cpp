#include "stdafx.h"

#include "CryEntityAudioProxy.h"

void CryEntityAudioProxyInterop::InitializeInterops()
{
	REGISTER_METHOD(SetFadeDistance);
	REGISTER_METHOD(GetFadeDistance);
	REGISTER_METHOD(SetEnvironmentFadeDistance);
	REGISTER_METHOD(GetEnvironmentFadeDistance);
	REGISTER_METHOD(SetEnvironmentId);
	REGISTER_METHOD(GetEnvironmentId);
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

void CryEntityAudioProxyInterop::SetEnvironmentFadeDistance(IEntityAudioProxy *handle,
															float fEnvironmentFadeDistance)
{
	handle->SetEnvironmentFadeDistance(fEnvironmentFadeDistance);
}

float CryEntityAudioProxyInterop::GetEnvironmentFadeDistance(IEntityAudioProxy *handle)
{
	return handle->GetEnvironmentFadeDistance();
}

void CryEntityAudioProxyInterop::SetEnvironmentId(IEntityAudioProxy *handle, AudioIdType nEnvironmentID)
{
	handle->SetEnvironmentId(nEnvironmentID);
}

AudioIdType CryEntityAudioProxyInterop::GetEnvironmentId(IEntityAudioProxy *handle)
{
	return handle->GetEnvironmentID();
}

AudioIdType CryEntityAudioProxyInterop::CreateAuxAudioProxy(IEntityAudioProxy *handle)
{
	return handle->CreateAuxAudioProxy();
}

bool CryEntityAudioProxyInterop::RemoveAuxAudioProxy(IEntityAudioProxy *handle,
													 AudioIdType nAudioProxyLocalID)
{
	return handle->RemoveAuxAudioProxy(nAudioProxyLocalID);
}

void CryEntityAudioProxyInterop::SetAuxAudioProxyOffset(IEntityAudioProxy *handle, const Matrix34 &rOffset,
														AudioIdType nAudioProxyLocalID)
{
	handle->SetAuxAudioProxyOffset(rOffset, nAudioProxyLocalID);
}

void CryEntityAudioProxyInterop::GetAuxAudioProxyOffset(IEntityAudioProxy *handle, Matrix34 &offset,
														AudioIdType nAudioProxyLocalID)
{
	offset = handle->GetAuxAudioProxyOffset(nAudioProxyLocalID);
}

void CryEntityAudioProxyInterop::PlayFileInternal(IEntityAudioProxy *handle, mono::string _szFile, AudioIdType _audioProxyId /* = AudioIdType.Default*/)
{
	handle->PlayFile(NtText(_szFile), _audioProxyId);
}

void CryEntityAudioProxyInterop::StopFileInternal(IEntityAudioProxy *handle, mono::string _szFile, AudioIdType _audioProxyId /* = AudioIdType.Default*/)
{
	handle->StopFile(NtText(_szFile), _audioProxyId);
}

bool CryEntityAudioProxyInterop::ExecuteTriggerInternal(IEntityAudioProxy *handle, AudioIdType nTriggerID,
														AudioIdType nAudioProxyLocalID)
{
	return handle->ExecuteTrigger(nTriggerID, nAudioProxyLocalID);
}

void CryEntityAudioProxyInterop::StopTriggerInternal(IEntityAudioProxy *handle, AudioIdType nTriggerID,
													 AudioIdType nAudioProxyLocalID)
{
	handle->StopTrigger(nTriggerID, nAudioProxyLocalID);
}

void CryEntityAudioProxyInterop::SetSwitchStateInternal(IEntityAudioProxy *handle, AudioIdType nSwitchID,
														AudioIdType nStateID, AudioIdType nAudioProxyLocalID)
{
	handle->SetSwitchState(nSwitchID, nStateID, nAudioProxyLocalID);
}

void CryEntityAudioProxyInterop::SetRtpcValueInternal(IEntityAudioProxy *handle, AudioIdType nRtpcID,
													  float fValue, AudioIdType nAudioProxyLocalID)
{
	handle->SetRtpcValue(nRtpcID, fValue, nAudioProxyLocalID);
}

void CryEntityAudioProxyInterop::SetObstructionCalcTypeInternal(IEntityAudioProxy *handle,
																EAudioOcclusionType eObstructionType,
																AudioIdType nAudioProxyLocalID)
{
	handle->SetObstructionCalcType(eObstructionType, nAudioProxyLocalID);
}

void CryEntityAudioProxyInterop::SetEnvironmentAmountInternal(IEntityAudioProxy *handle,
															  AudioIdType nEnvironmentID, float fAmount,
															  AudioIdType nAudioProxyLocalID)
{
	handle->SetEnvironmentAmount(nEnvironmentID, fAmount, nAudioProxyLocalID);
}

void CryEntityAudioProxyInterop::SetCurrentEnvironmentsInternal(IEntityAudioProxy *handle,
																AudioIdType nAudioProxyLocalID)
{
	handle->SetCurrentEnvironments(nAudioProxyLocalID);
}

void CryEntityAudioProxyInterop::AuxAudioProxiesMoveWithEntity(IEntityAudioProxy *handle,
															   bool bCanMoveWithEntity)
{
	handle->AuxAudioProxiesMoveWithEntity(bCanMoveWithEntity);
}
