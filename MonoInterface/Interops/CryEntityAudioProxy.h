#pragma once

#include "IMonoInterface.h"

struct CryEntityAudioProxyInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryEntityAudioProxy"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Logic.EntityProxies"; }

	virtual void InitializeInterops() override;

	static void SetFadeDistance(IEntityAudioProxy *handle, float fFadeDistance);
	static float GetFadeDistance(IEntityAudioProxy *handle);
	static void SetEnvironmentFadeDistance(IEntityAudioProxy *handle, float fEnvironmentFadeDistance);
	static float GetEnvironmentFadeDistance(IEntityAudioProxy *handle);
	static void SetEnvironmentId(IEntityAudioProxy *handle, AudioIdType nEnvironmentID);
	static AudioIdType GetEnvironmentId(IEntityAudioProxy *handle);
	static AudioIdType CreateAuxAudioProxy(IEntityAudioProxy *handle);
	static bool RemoveAuxAudioProxy(IEntityAudioProxy *handle, AudioIdType nAudioProxyLocalID);
	static void SetAuxAudioProxyOffset(IEntityAudioProxy *handle, const Matrix34 &rOffset,
									   AudioIdType nAudioProxyLocalID);
	static void GetAuxAudioProxyOffset(IEntityAudioProxy *handle, Matrix34 &offset,
									   AudioIdType nAudioProxyLocalID);
	static void PlayFileInternal(IEntityAudioProxy *handle, mono::string _szFile,
								 AudioIdType _audioProxyId /* = AudioIdType.Default*/);
	static void StopFileInternal(IEntityAudioProxy *handle, mono::string _szFile,
								 AudioIdType _audioProxyId /* = AudioIdType.Default*/);
	static bool ExecuteTriggerInternal(IEntityAudioProxy *handle, AudioIdType nTriggerID,
									   AudioIdType nAudioProxyLocalID);
	static void StopTriggerInternal(IEntityAudioProxy *handle, AudioIdType nTriggerID,
									AudioIdType nAudioProxyLocalID);
	static void SetSwitchStateInternal(IEntityAudioProxy *handle, AudioIdType nSwitchID,
									   AudioIdType nStateID, AudioIdType nAudioProxyLocalID);
	static void SetRtpcValueInternal(IEntityAudioProxy *handle, AudioIdType nRtpcID, float fValue,
									 AudioIdType nAudioProxyLocalID);
	static void SetObstructionCalcTypeInternal(IEntityAudioProxy *handle,
											   EAudioOcclusionType eObstructionType,
											   AudioIdType nAudioProxyLocalID);
	static void SetEnvironmentAmountInternal(IEntityAudioProxy *handle, AudioIdType nEnvironmentID,
											 float fAmount, AudioIdType nAudioProxyLocalID);
	static void SetCurrentEnvironmentsInternal(IEntityAudioProxy *handle, AudioIdType nAudioProxyLocalID);
	static void AuxAudioProxiesMoveWithEntity(IEntityAudioProxy *handle, bool bCanMoveWithEntity);
};