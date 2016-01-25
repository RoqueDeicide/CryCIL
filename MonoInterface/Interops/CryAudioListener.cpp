#include "stdafx.h"

#include "CryAudioListener.h"

void CryAudioListenerInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetId);
	REGISTER_METHOD(GetActive);
	REGISTER_METHOD(SetActive);
	REGISTER_METHOD(GetMoved);
	REGISTER_METHOD(GetInside);
	REGISTER_METHOD(SetInside);
	REGISTER_METHOD(SetRecordLevel);
	REGISTER_METHOD(GetRecordLevel);
	REGISTER_METHOD(GetPosition);
	REGISTER_METHOD(SetPosition);
	REGISTER_METHOD(GetForward);
	REGISTER_METHOD(GetTop);
	REGISTER_METHOD(GetVelocity);
	REGISTER_METHOD(SetVelocity);
	REGISTER_METHOD(SetMatrix);
	REGISTER_METHOD(GetMatrix);
	REGISTER_METHOD(GetUnderwater);
	REGISTER_METHOD(SetUnderwater);
}

EntityId CryAudioListenerInterop::GetId(IAudioListener *handle)
{
	return handle->GetID();
}

bool CryAudioListenerInterop::GetActive(IAudioListener *handle)
{
	return handle->GetActive();
}

void CryAudioListenerInterop::SetActive(IAudioListener *handle, bool bActive)
{
	handle->SetActive(bActive);
}

bool CryAudioListenerInterop::GetMoved(IAudioListener *handle)
{
	return handle->GetMoved();
}

bool CryAudioListenerInterop::GetInside(IAudioListener *handle)
{
	return handle->GetInside();
}

void CryAudioListenerInterop::SetInside(IAudioListener *handle, bool bInside)
{
	handle->SetInside(bInside);
}

void CryAudioListenerInterop::SetRecordLevel(IAudioListener *handle, float fRecordLevel)
{
	handle->SetRecordLevel(fRecordLevel);
}

float CryAudioListenerInterop::GetRecordLevel(IAudioListener *handle)
{
	return handle->GetRecordLevel();
}

Vec3 CryAudioListenerInterop::GetPosition(IAudioListener *handle)
{
	return handle->GetPosition();
}

void CryAudioListenerInterop::SetPosition(IAudioListener *handle, const Vec3 &rPosition)
{
	handle->SetPosition(rPosition);
}

void CryAudioListenerInterop::GetForward(IAudioListener *handle, Vec3 &forward)
{
	forward = const_cast<Vec3 &>(handle->GetForward());
}

Vec3 CryAudioListenerInterop::GetTop(IAudioListener *handle)
{
	return handle->GetTop();
}

void CryAudioListenerInterop::GetVelocity(IAudioListener *handle, Vec3 &velocity)
{
	velocity = handle->GetVelocity();
}

void CryAudioListenerInterop::SetVelocity(IAudioListener *handle, const Vec3 &vVel)
{
	handle->SetVelocity(vVel);
}

void CryAudioListenerInterop::SetMatrix(IAudioListener *handle, const Matrix34 &newTransformation)
{
	handle->SetMatrix(newTransformation);
}

void CryAudioListenerInterop::GetMatrix(IAudioListener *handle, Matrix34 &matrix)
{
	matrix = handle->GetMatrix();
}

float CryAudioListenerInterop::GetUnderwater(IAudioListener *handle)
{
	return handle->GetUnderwater();
}

void CryAudioListenerInterop::SetUnderwater(IAudioListener *handle, float fUnder)
{
	handle->SetUnderwater(fUnder);
}
