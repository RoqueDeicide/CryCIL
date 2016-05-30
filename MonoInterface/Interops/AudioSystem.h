#pragma once

#include "IMonoInterface.h"
#include <CryAudio/IAudioSystem.h>

struct AudioSystemInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "AudioSystem"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Audio"; }

	virtual void InitializeInterops() override;

	static CryAudio::Impl::IAudioImpl *CreateNativeImplementationObject(mono::object managedObject);
	static bool                        GetPreloadRequestId(mono::string name, uint32 &id);
	static bool                        GetAudioTriggerId(mono::string sAudioTriggerName, uint32 &rAudioTriggerId);
	static bool                        GetAudioRtpcId(mono::string audioRtpcName, uint32 &audioRtpcId);
	static bool                        GetAudioSwitchId(mono::string audioSwitchName, uint32 &audioSwitchId);
	static bool                        GetAudioSwitchStateId(uint32 switchId, mono::string audioTriggerName, uint32 &audioStateId);
	static bool                        GetAudioEnvironmentId(mono::string sAudioEnvironmentName, uint32 &rAudioEnvironmentId);
	static mono::string                GetConfigPath();
	static IAudioProxy                *GetFreeAudioProxy();
	static mono::string                GetAudioControlNameInternal(EAudioControlType eAudioEntityType, uint32 nAudioEntityId);
	static void                        RequestSetImpl(CryAudio::Impl::IAudioImpl *implHandle);
	static void                        RequestReserveAudioId(uint32 *const id, mono::string name);
	static void                        RequestPreloadAudioRequest(uint32 id);
	static void                        RequestUnloadAudioRequest(uint32 id);
	static void                        RequestSetRtpcValue(uint32 id, float value);
	static void                        RequestSetSwitchState(uint32 switchId, uint32 stateId);
	static void                        RequestExecuteTrigger(uint32 id, float timeout);
	static void                        RequestStopTrigger(uint32 id);
	static void                        RequestStopAllTriggers();
	static void                        RequestSetPosition(const CAudioObjectTransformation &tm);
	static void                        RequestSetVolume(float volume);
	static void                        RequestSetEnvironmentAmount(uint32 id, float amount);
	static void                        RequestResetEnvironments();
};