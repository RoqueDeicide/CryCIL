#pragma once

#include "IMonoInterface.h"

struct CryEntityProxiesInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryEntityProxies"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Logic.EntityProxies"; }

	virtual void OnRunTimeInitialized() override;

	static IEntityAreaProxy *GetAreaProxy(IEntity *handle);
	static IEntityAudioProxy *GetAudioProxy(IEntity *handle);
	static IEntityCameraProxy *GetCameraProxy(IEntity *handle);
	static IEntityPhysicalProxy *GetPhysicalProxy(IEntity *handle);
	static IEntityRenderProxy *GetRenderProxy(IEntity *handle);
	static IEntityRopeProxy *GetRopeProxy(IEntity *handle);
	static IEntitySubstitutionProxy *GetSubstitutionProxy(IEntity *handle);
	static IEntityTriggerProxy *GetTriggerProxy(IEntity *handle);
	static IEntityAreaProxy *CreateAreaProxy(IEntity *handle);
	static IEntityAudioProxy *CreateAudioProxy(IEntity *handle);
	static IEntityCameraProxy *CreateCameraProxy(IEntity *handle);
	static IEntityPhysicalProxy *CreatePhysicalProxy(IEntity *handle);
	static IEntityRenderProxy *CreateRenderProxy(IEntity *handle);
	static IEntityRopeProxy *CreateRopeProxy(IEntity *handle);
	static IEntitySubstitutionProxy *CreateSubstitutionProxy(IEntity *handle);
	static IEntityTriggerProxy *CreateTriggerProxy(IEntity *handle);
};