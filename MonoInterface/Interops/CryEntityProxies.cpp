#include "stdafx.h"

#include "CryEntityProxies.h"

void CryEntityProxiesInterop::InitializeInterops()
{
	REGISTER_METHOD(GetAreaProxy);
	REGISTER_METHOD(GetAudioProxy);
	REGISTER_METHOD(GetCameraProxy);
	REGISTER_METHOD(GetPhysicalProxy);
	REGISTER_METHOD(GetRenderProxy);
	REGISTER_METHOD(GetRopeProxy);
	REGISTER_METHOD(GetSubstitutionProxy);
	REGISTER_METHOD(GetTriggerProxy);
	REGISTER_METHOD(CreateAreaProxy);
	REGISTER_METHOD(CreateAudioProxy);
	REGISTER_METHOD(CreateCameraProxy);
	REGISTER_METHOD(CreatePhysicalProxy);
	REGISTER_METHOD(CreateRenderProxy);
	REGISTER_METHOD(CreateRopeProxy);
	REGISTER_METHOD(CreateSubstitutionProxy);
	REGISTER_METHOD(CreateTriggerProxy);
}

IEntityAreaProxy *CryEntityProxiesInterop::GetAreaProxy(IEntity *handle)
{
	return static_cast<IEntityAreaProxy *>(handle->GetProxy(EEntityProxy::ENTITY_PROXY_AREA));
}

IEntityAudioProxy *CryEntityProxiesInterop::GetAudioProxy(IEntity *handle)
{
	return static_cast<IEntityAudioProxy *>(handle->GetProxy(EEntityProxy::ENTITY_PROXY_AUDIO));
}

IEntityCameraProxy *CryEntityProxiesInterop::GetCameraProxy(IEntity *handle)
{
	return static_cast<IEntityCameraProxy *>(handle->GetProxy(EEntityProxy::ENTITY_PROXY_CAMERA));
}

IEntityPhysicalProxy *CryEntityProxiesInterop::GetPhysicalProxy(IEntity *handle)
{
	return static_cast<IEntityPhysicalProxy *>(handle->GetProxy(EEntityProxy::ENTITY_PROXY_PHYSICS));
}

IEntityRenderProxy *CryEntityProxiesInterop::GetRenderProxy(IEntity *handle)
{
	return static_cast<IEntityRenderProxy *>(handle->GetProxy(EEntityProxy::ENTITY_PROXY_RENDER));
}

IEntityRopeProxy *CryEntityProxiesInterop::GetRopeProxy(IEntity *handle)
{
	return static_cast<IEntityRopeProxy *>(handle->GetProxy(EEntityProxy::ENTITY_PROXY_ROPE));
}

IEntitySubstitutionProxy *CryEntityProxiesInterop::GetSubstitutionProxy(IEntity *handle)
{
	return static_cast<IEntitySubstitutionProxy *>(handle->GetProxy(EEntityProxy::ENTITY_PROXY_SUBSTITUTION));
}

IEntityTriggerProxy *CryEntityProxiesInterop::GetTriggerProxy(IEntity *handle)
{
	return static_cast<IEntityTriggerProxy *>(handle->GetProxy(EEntityProxy::ENTITY_PROXY_TRIGGER));
}

IEntityAreaProxy *CryEntityProxiesInterop::CreateAreaProxy(IEntity *handle)
{
	return static_cast<IEntityAreaProxy *>(handle->CreateProxy(EEntityProxy::ENTITY_PROXY_AREA).get());
}

IEntityAudioProxy *CryEntityProxiesInterop::CreateAudioProxy(IEntity *handle)
{
	return static_cast<IEntityAudioProxy *>(handle->CreateProxy(EEntityProxy::ENTITY_PROXY_AUDIO).get());
}

IEntityCameraProxy *CryEntityProxiesInterop::CreateCameraProxy(IEntity *handle)
{
	return static_cast<IEntityCameraProxy *>(handle->CreateProxy(EEntityProxy::ENTITY_PROXY_CAMERA).get());
}

IEntityPhysicalProxy *CryEntityProxiesInterop::CreatePhysicalProxy(IEntity *handle)
{
	return static_cast<IEntityPhysicalProxy *>(handle->CreateProxy(EEntityProxy::ENTITY_PROXY_PHYSICS).get());
}

IEntityRenderProxy *CryEntityProxiesInterop::CreateRenderProxy(IEntity *handle)
{
	return static_cast<IEntityRenderProxy *>(handle->CreateProxy(EEntityProxy::ENTITY_PROXY_RENDER).get());
}

IEntityRopeProxy *CryEntityProxiesInterop::CreateRopeProxy(IEntity *handle)
{
	return static_cast<IEntityRopeProxy *>(handle->CreateProxy(EEntityProxy::ENTITY_PROXY_ROPE).get());
}

IEntitySubstitutionProxy *CryEntityProxiesInterop::CreateSubstitutionProxy(IEntity *handle)
{
	return static_cast<IEntitySubstitutionProxy *>(handle->CreateProxy(EEntityProxy::ENTITY_PROXY_SUBSTITUTION).get());
}

IEntityTriggerProxy *CryEntityProxiesInterop::CreateTriggerProxy(IEntity *handle)
{
	return static_cast<IEntityTriggerProxy *>(handle->CreateProxy(EEntityProxy::ENTITY_PROXY_TRIGGER).get());
}
