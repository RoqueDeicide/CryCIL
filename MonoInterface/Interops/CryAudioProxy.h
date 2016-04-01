#pragma once

#include "IMonoInterface.h"

struct CryAudioProxyInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryAudioProxy"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Audio"; }

	virtual void InitializeInterops() override;

	static void Init(IAudioProxy *handle, mono::string sObjectName, bool bInitAsync);
	static void ReleaseInternal(IAudioProxy *handle);
	static void ResetInternal(IAudioProxy *handle);
	static void ExecuteTriggerInternal(IAudioProxy *handle, uint nTriggerID, ELipSyncMethod eLipSyncMethod);
	static void StopTriggerInternal(IAudioProxy *handle, uint nTriggerID);
	static void SetSwitchStateInternal(IAudioProxy *handle, uint nSwitchID, uint nStateID);
	static void SetRtpcValueInternal(IAudioProxy *handle, uint nRtpcID, float fValue);
	static void SetObstructionCalcTypeInternal(IAudioProxy *handle, EAudioObjectObstructionCalcType eObstructionType);
	static void SetTransformation(IAudioProxy *handle, const Matrix34 &rPosition);
	static void SetPosition(IAudioProxy *handle, const Vec3 &rPosition);
	static void SetEnvironmentAmountInternal(IAudioProxy *handle, uint nEnvironmentID, float fAmount);
	static void SetCurrentEnvironmentsInternal(IAudioProxy *handle, EntityId nEntityToIgnore);
	static uint GetAudioObjectID(IAudioProxy *handle);
};