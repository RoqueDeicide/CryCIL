#include "stdafx.h"

#include "AudioSystem.h"

void AudioSystemInterop::InitializeInterops()
{
	REGISTER_METHOD(CreateNativeImplementationObject);
	REGISTER_METHOD(GetPreloadRequestId);
	REGISTER_METHOD(GetAudioTriggerID);
	REGISTER_METHOD(GetAudioRtpcID);
	REGISTER_METHOD(GetAudioSwitchID);
	REGISTER_METHOD(GetAudioSwitchStateID);
	REGISTER_METHOD(GetAudioEnvironmentID);
	REGISTER_METHOD(GetInfo);
	REGISTER_METHOD(GetConfigPath);
	REGISTER_METHOD(GetFreeAudioProxy);
	REGISTER_METHOD(GetAudioControlNameInternal);
	REGISTER_METHOD(RequestSetImpl);
	REGISTER_METHOD(RequestReserveAudioId);
	REGISTER_METHOD(RequestPreloadAudioRequest);
	REGISTER_METHOD(RequestUnloadAudioRequest);
	REGISTER_METHOD(RequestSetRtpcValue);
	REGISTER_METHOD(RequestSetSwitchState);
	REGISTER_METHOD(RequestExecuteTrigger);
	REGISTER_METHOD(RequestStopTrigger);
	REGISTER_METHOD(RequestStopAllTriggers);
	REGISTER_METHOD(RequestSetPosition);
	REGISTER_METHOD(RequestSetVolume);
	REGISTER_METHOD(RequestSetEnvironmentAmount);
	REGISTER_METHOD(RequestResetEnvironments);
}

IAudioSystemImplementation *AudioSystemInterop::CreateNativeImplementationObject(mono::object managedObject)
{
	// TODO: Implement custom audio system implementations.
	NotImplementedException().Throw();
	return nullptr;
}

bool AudioSystemInterop::GetPreloadRequestId(mono::string name, uint32 &id)
{
	return gEnv->pAudioSystem->GetAudioPreloadRequestID(NtText(name), id);
}

bool AudioSystemInterop::GetAudioTriggerID(mono::string sAudioTriggerName, uint32 &rAudioTriggerID)
{
	return gEnv->pAudioSystem->GetAudioTriggerID(NtText(sAudioTriggerName), rAudioTriggerID);
}

bool AudioSystemInterop::GetAudioRtpcID(mono::string audioRtpcName, uint32 &audioRtpcId)
{
	return gEnv->pAudioSystem->GetAudioRtpcID(NtText(audioRtpcName), audioRtpcId);
}

bool AudioSystemInterop::GetAudioSwitchID(mono::string audioSwitchName, uint32 &audioSwitchId)
{
	return gEnv->pAudioSystem->GetAudioSwitchID(NtText(audioSwitchName), audioSwitchId);
}

bool AudioSystemInterop::GetAudioSwitchStateID(uint32 switchID, mono::string audioTriggerName, uint32 &audioStateId)
{
	return gEnv->pAudioSystem->GetAudioSwitchStateID(switchID, NtText(audioTriggerName), audioStateId);
}

bool AudioSystemInterop::GetAudioEnvironmentID(mono::string sAudioEnvironmentName, uint32 &rAudioEnvironmentID)
{
	return gEnv->pAudioSystem->GetAudioEnvironmentID(NtText(sAudioEnvironmentName), rAudioEnvironmentID);
}

void AudioSystemInterop::GetInfo(SAudioSystemInfo &rAudioSystemInfo)
{
	gEnv->pAudioSystem->GetInfo(rAudioSystemInfo);
}

mono::string AudioSystemInterop::GetConfigPath()
{
	return ToMonoString(gEnv->pAudioSystem->GetConfigPath());
}

IAudioProxy *AudioSystemInterop::GetFreeAudioProxy()
{
	return gEnv->pAudioSystem->GetFreeAudioProxy();
}

mono::string AudioSystemInterop::GetAudioControlNameInternal(EAudioControlType eAudioEntityType, uint32 nAudioEntityID)
{
	return ToMonoString(gEnv->pAudioSystem->GetAudioControlName(eAudioEntityType, nAudioEntityID));
}

void AudioSystemInterop::RequestSetImpl(IAudioSystemImplementation *implHandle)
{
	// TODO: Implement custom audio system implementations.
	NotImplementedException().Throw();

	SAudioRequest request;

	SAudioManagerRequestData<eAMRT_SET_AUDIO_IMPL> data(implHandle);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestReserveAudioId(uint32 *const id, mono::string name)
{
	SAudioRequest request;

	SAudioManagerRequestData<eAMRT_RESERVE_AUDIO_OBJECT_ID> data(id, NtText(name));

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestPreloadAudioRequest(uint32 id)
{
	SAudioRequest request;

	SAudioManagerRequestData<eAMRT_PRELOAD_SINGLE_REQUEST> data(id);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestUnloadAudioRequest(uint32 id)
{
	SAudioRequest request;

	SAudioManagerRequestData<eAMRT_UNLOAD_SINGLE_REQUEST> data(id);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestSetRtpcValue(uint32 id, float value)
{
	SAudioRequest request;

	SAudioObjectRequestData<eAORT_SET_RTPC_VALUE> data(id, value);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestSetSwitchState(uint32 switchId, uint32 stateId)
{
	SAudioRequest request;

	SAudioObjectRequestData<eAORT_SET_SWITCH_STATE> data(switchId, stateId);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestExecuteTrigger(uint32 id, float timeout)
{
	SAudioRequest request;

	SAudioObjectRequestData<eAORT_EXECUTE_TRIGGER> data(id, timeout);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestStopTrigger(uint32 id)
{
	SAudioRequest request;

	SAudioObjectRequestData<eAORT_STOP_TRIGGER> data(id);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestStopAllTriggers()
{
	SAudioRequest request;

	SAudioObjectRequestData<eAORT_STOP_ALL_TRIGGERS> data;

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestSetPosition(const Matrix34 &tm)
{
	SAudioRequest request;

	SAudioObjectRequestData<eAORT_SET_POSITION> data(tm);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestSetVolume(float volume)
{
	SAudioRequest request;

	SAudioObjectRequestData<eAORT_SET_VOLUME> data(volume);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestSetEnvironmentAmount(uint32 id, float amount)
{
	SAudioRequest request;

	SAudioObjectRequestData<eAORT_SET_ENVIRONMENT_AMOUNT> data(id, amount);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestResetEnvironments()
{
	SAudioRequest request;

	SAudioObjectRequestData<eAORT_RESET_ENVIRONMENTS> data;

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}
