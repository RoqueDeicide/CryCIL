#include "stdafx.h"

#include "AudioSystem.h"

void AudioSystemInterop::InitializeInterops()
{
	REGISTER_METHOD(CreateNativeImplementationObject);
	REGISTER_METHOD(GetPreloadRequestId);
	REGISTER_METHOD(GetAudioTriggerId);
	REGISTER_METHOD(GetAudioRtpcId);
	REGISTER_METHOD(GetAudioSwitchId);
	REGISTER_METHOD(GetAudioSwitchStateId);
	REGISTER_METHOD(GetAudioEnvironmentId);
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

CryAudio::Impl::IAudioImpl *AudioSystemInterop::CreateNativeImplementationObject(mono::object)
{
	// TODO: Implement custom audio system implementations.
	NotImplementedException().Throw();
	return nullptr;
}

bool AudioSystemInterop::GetPreloadRequestId(mono::string name, uint32 &id)
{
	return gEnv->pAudioSystem->GetAudioPreloadRequestId(NtText(name), id);
}

bool AudioSystemInterop::GetAudioTriggerId(mono::string sAudioTriggerName, uint32 &rAudioTriggerId)
{
	return gEnv->pAudioSystem->GetAudioTriggerId(NtText(sAudioTriggerName), rAudioTriggerId);
}

bool AudioSystemInterop::GetAudioRtpcId(mono::string audioRtpcName, uint32 &audioRtpcId)
{
	return gEnv->pAudioSystem->GetAudioRtpcId(NtText(audioRtpcName), audioRtpcId);
}

bool AudioSystemInterop::GetAudioSwitchId(mono::string audioSwitchName, uint32 &audioSwitchId)
{
	return gEnv->pAudioSystem->GetAudioSwitchId(NtText(audioSwitchName), audioSwitchId);
}

bool AudioSystemInterop::GetAudioSwitchStateId(uint32 switchId, mono::string audioTriggerName, uint32 &audioStateId)
{
	return gEnv->pAudioSystem->GetAudioSwitchStateId(switchId, NtText(audioTriggerName), audioStateId);
}

bool AudioSystemInterop::GetAudioEnvironmentId(mono::string sAudioEnvironmentName, uint32 &rAudioEnvironmentId)
{
	return gEnv->pAudioSystem->GetAudioEnvironmentId(NtText(sAudioEnvironmentName), rAudioEnvironmentId);
}

mono::string AudioSystemInterop::GetConfigPath()
{
	return ToMonoString(gEnv->pAudioSystem->GetConfigPath());
}

IAudioProxy *AudioSystemInterop::GetFreeAudioProxy()
{
	return gEnv->pAudioSystem->GetFreeAudioProxy();
}

mono::string AudioSystemInterop::GetAudioControlNameInternal(EAudioControlType eAudioEntityType, uint32 nAudioEntityId)
{
	return ToMonoString(gEnv->pAudioSystem->GetAudioControlName(eAudioEntityType, nAudioEntityId));
}

void AudioSystemInterop::RequestSetImpl(CryAudio::Impl::IAudioImpl *implHandle)
{
	// TODO: Implement custom audio system implementations.
	NotImplementedException().Throw();

	SAudioRequest request;

	SAudioManagerRequestData<eAudioManagerRequestType_SetAudioImpl> data(implHandle);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestReserveAudioId(uint32 *const id, mono::string name)
{
	SAudioRequest request;

	SAudioManagerRequestData<eAudioManagerRequestType_ReserveAudioObjectId> data(id, NtText(name));

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestPreloadAudioRequest(uint32 id)
{
	SAudioRequest request;

	SAudioManagerRequestData<eAudioManagerRequestType_PreloadSingleRequest> data(id, true);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestUnloadAudioRequest(uint32 id)
{
	SAudioRequest request;

	SAudioManagerRequestData<eAudioManagerRequestType_UnloadSingleRequest> data(id);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestSetRtpcValue(uint32 id, float value)
{
	SAudioRequest request;

	SAudioObjectRequestData<eAudioObjectRequestType_SetRtpcValue> data(id, value);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestSetSwitchState(uint32 switchId, uint32 stateId)
{
	SAudioRequest request;

	SAudioObjectRequestData<eAudioObjectRequestType_SetSwitchState> data(switchId, stateId);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestExecuteTrigger(uint32 id, float timeout)
{
	SAudioRequest request;

	SAudioObjectRequestData<eAudioObjectRequestType_ExecuteTrigger> data(id, timeout);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestStopTrigger(uint32 id)
{
	SAudioRequest request;

	SAudioObjectRequestData<eAudioObjectRequestType_StopTrigger> data(id);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestStopAllTriggers()
{
	SAudioRequest request;

	SAudioObjectRequestData<eAudioObjectRequestType_StopAllTriggers> data;

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestSetPosition(const CAudioObjectTransformation &tm)
{
	SAudioRequest request;

	SAudioObjectRequestData<EAudioObjectRequestType::eAudioObjectRequestType_SetTransformation> data(tm);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestSetVolume(float volume)
{
	SAudioRequest request;

	SAudioObjectRequestData<eAudioObjectRequestType_SetVolume> data(volume);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestSetEnvironmentAmount(uint32 id, float amount)
{
	SAudioRequest request;

	SAudioObjectRequestData<eAudioObjectRequestType_SetEnvironmentAmount> data(id, amount);

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}

void AudioSystemInterop::RequestResetEnvironments()
{
	SAudioRequest request;

	SAudioObjectRequestData<eAudioObjectRequestType_ResetEnvironments> data;

	request.pData = &data;

	gEnv->pAudioSystem->PushRequest(request);
}
