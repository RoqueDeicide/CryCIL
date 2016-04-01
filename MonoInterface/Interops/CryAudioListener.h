#pragma once

#include "IMonoInterface.h"

struct CryAudioListenerInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryAudioListener"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Audio"; }

	virtual void InitializeInterops() override;

	static EntityId GetId(IAudioListener *handle);
	static bool GetActive(IAudioListener *handle);
	static void SetActive(IAudioListener *handle, bool bActive);
	static bool GetMoved(IAudioListener *handle);
	static bool GetInside(IAudioListener *handle);
	static void SetInside(IAudioListener *handle, bool bInside);
	static void SetRecordLevel(IAudioListener *handle, float fRecordLevel);
	static float GetRecordLevel(IAudioListener *handle);
	static Vec3 GetPosition(IAudioListener *handle);
	static void SetPosition(IAudioListener *handle, const Vec3 &rPosition);
	static void GetForward(IAudioListener *handle, Vec3 &forward);
	static Vec3 GetTop(IAudioListener *handle);
	static void GetVelocity(IAudioListener *handle, Vec3 &velocity);
	static void SetVelocity(IAudioListener *handle, const Vec3 &vVel);
	static void SetMatrix(IAudioListener *handle, const Matrix34 &newTransformation);
	static void GetMatrix(IAudioListener *handle, Matrix34 &matrix);
	static float GetUnderwater(IAudioListener *handle);
	static void SetUnderwater(IAudioListener *handle, float fUnder);
};