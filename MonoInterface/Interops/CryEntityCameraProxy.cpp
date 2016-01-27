#include "stdafx.h"

#include "CryEntityCameraProxy.h"

void CryEntityCameraProxyInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(SetCamera);
	REGISTER_METHOD(GetCamera);
}

void CryEntityCameraProxyInterop::SetCamera(IEntityCameraProxy *handle, CCamera &cam)
{
	handle->SetCamera(cam);
}

void CryEntityCameraProxyInterop::GetCamera(IEntityCameraProxy *handle, CCamera &cam)
{
	cam = handle->GetCamera();
}
