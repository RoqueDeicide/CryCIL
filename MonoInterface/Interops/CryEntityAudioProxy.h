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
	static void SetEnvironmentID(IEntityAudioProxy *handle, TATLIDType nEnvironmentID);
	static TATLIDType GetEnvironmentID(IEntityAudioProxy *handle);
	static TATLIDType CreateAuxAudioProxy(IEntityAudioProxy *handle);
	static bool RemoveAuxAudioProxy(IEntityAudioProxy *handle, TATLIDType nAudioProxyLocalID);
	static void SetAuxAudioProxyOffset(IEntityAudioProxy *handle, const Matrix34 &rOffset, TATLIDType nAudioProxyLocalID);
	static void GetAuxAudioProxyOffset(IEntityAudioProxy *handle, Matrix34 &offset, TATLIDType nAudioProxyLocalID);
	static bool ExecuteTriggerInternal(IEntityAudioProxy *handle, TATLIDType nTriggerID, ELipSyncMethod eLipSyncMethod, TATLIDType nAudioProxyLocalID);
	static void StopTriggerInternal(IEntityAudioProxy *handle, TATLIDType nTriggerID, TATLIDType nAudioProxyLocalID);
	static void SetSwitchStateInternal(IEntityAudioProxy *handle, TATLIDType nSwitchID, TATLIDType nStateID, TATLIDType nAudioProxyLocalID);
	static void SetRtpcValueInternal(IEntityAudioProxy *handle, TATLIDType nRtpcID, float fValue, TATLIDType nAudioProxyLocalID);
	static void SetObstructionCalcTypeInternal(IEntityAudioProxy *handle, EAudioObjectObstructionCalcType eObstructionType, TATLIDType nAudioProxyLocalID);
	static void SetEnvironmentAmountInternal(IEntityAudioProxy *handle, TATLIDType nEnvironmentID, float fAmount, TATLIDType nAudioProxyLocalID);
	static void SetCurrentEnvironmentsInternal(IEntityAudioProxy *handle, TATLIDType nAudioProxyLocalID);
	static void AuxAudioProxiesMoveWithEntity(IEntityAudioProxy *handle, bool bCanMoveWithEntity);
};