#pragma once

#include "IMonoInterface.h"
#include <IAudioSystem.h>

struct AudioSystemInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "AudioSystem"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Audio"; }

	virtual void InitializeInterops() override;

	static IAudioSystemImplementation *CreateNativeImplementationObject(mono::object managedObject);
	static bool GetPreloadRequestId(mono::string name, uint32 &id);
	static bool GetAudioTriggerID(mono::string sAudioTriggerName, uint32 &rAudioTriggerID);
	static bool GetAudioRtpcID(mono::string audioRtpcName, uint32 &audioRtpcId);
	static bool GetAudioSwitchID(mono::string audioSwitchName, uint32 &audioSwitchId);
	static bool GetAudioSwitchStateID(uint32 switchID, mono::string audioTriggerName, uint32 &audioStateId);
	static bool GetAudioEnvironmentID(mono::string sAudioEnvironmentName, uint32 &rAudioEnvironmentID);
	static void GetInfo(SAudioSystemInfo &rAudioSystemInfo);
	static mono::string GetConfigPath();
	static IAudioProxy *GetFreeAudioProxy();
	static mono::string GetAudioControlNameInternal(EAudioControlType eAudioEntityType, uint32 nAudioEntityID);
	static void RequestSetImpl(IAudioSystemImplementation *implHandle);
	static void RequestReserveAudioId(uint32 *const id, mono::string name);
	static void RequestPreloadAudioRequest(uint32 id);
	static void RequestUnloadAudioRequest(uint32 id);
	static void RequestSetRtpcValue(uint32 id, float value);
	static void RequestSetSwitchState(uint32 switchId, uint32 stateId);
	static void RequestExecuteTrigger(uint32 id, float timeout);
	static void RequestStopTrigger(uint32 id);
	static void RequestStopAllTriggers();
	static void RequestSetPosition(const Matrix34 &tm);
	static void RequestSetVolume(float volume);
	static void RequestSetEnvironmentAmount(uint32 id, float amount);
	static void RequestResetEnvironments();
};