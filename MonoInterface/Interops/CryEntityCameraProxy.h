#pragma once

#include "IMonoInterface.h"

struct CryEntityCameraProxyInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryEntityCameraProxy"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Logic.EntityProxies"; }

	virtual void OnRunTimeInitialized() override;

	static void SetCamera(IEntityCameraProxy *handle, CCamera &cam);
	static void GetCamera(IEntityCameraProxy *handle, CCamera &cam);
};