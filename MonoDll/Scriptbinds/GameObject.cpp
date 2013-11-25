#include "StdAfx.h"
#include "GameObject.h"

#include "MonoScriptSystem.h"

CScriptbind_GameObject::CScriptbind_GameObject()
{
	REGISTER_METHOD(GetGameObject);

	REGISTER_METHOD(EnablePostUpdates);
	REGISTER_METHOD(EnablePrePhysicsUpdates);

	REGISTER_METHOD(QueryExtension);
	REGISTER_METHOD(AcquireExtension);

	REGISTER_METHOD(ReleaseExtension);

	REGISTER_METHOD(ActivateExtension);
	REGISTER_METHOD(DeactivateExtension);

	REGISTER_METHOD(ChangedNetworkState);

	REGISTER_METHOD(SetAspectProfile);

	REGISTER_METHOD(EnablePhysicsEvent);
	REGISTER_METHOD(WantsPhysicsEvent);

	REGISTER_METHOD(BindToNetwork);
}

IGameObject *CScriptbind_GameObject::GetGameObject(EntityId id)
{
	return static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetGameObject(id);
}

void CScriptbind_GameObject::EnablePostUpdates(IGameObject *pGameObject, IGameObjectExtension *pExtension, bool enable)
{
	if(enable)
		pGameObject->EnablePostUpdates(pExtension);
	else
		pGameObject->DisablePostUpdates(pExtension);
}

void CScriptbind_GameObject::EnablePrePhysicsUpdates(IGameObject *pGameObject, EPrePhysicsUpdate rule)
{
	pGameObject->EnablePrePhysicsUpdate(rule);
}

IGameObjectExtension *CScriptbind_GameObject::QueryExtension(IGameObject *pGameObject, mono::string name)
{
	return pGameObject->QueryExtension(ToCryString(name));
}

IGameObjectExtension *CScriptbind_GameObject::AcquireExtension(IGameObject *pGameObject, mono::string name)
{
	return pGameObject->AcquireExtension(ToCryString(name));
}

void CScriptbind_GameObject::ReleaseExtension(IGameObject *pGameObject, mono::string name)
{
	pGameObject->ReleaseExtension(ToCryString(name));
}

bool CScriptbind_GameObject::ActivateExtension(IGameObject *pGameObject, mono::string name)
{
	return pGameObject->ActivateExtension(ToCryString(name));
}

void CScriptbind_GameObject::DeactivateExtension(IGameObject *pGameObject, mono::string name)
{
	pGameObject->DeactivateExtension(ToCryString(name));
}

void CScriptbind_GameObject::ChangedNetworkState(IGameObject *pGameObject, int aspect)
{
	pGameObject->ChangedNetworkState((NetworkAspectType)aspect);
}

bool CScriptbind_GameObject::SetAspectProfile(IGameObject *pGameObject, EEntityAspects aspect, uint8 profile, bool fromNetwork)
{
	return pGameObject->SetAspectProfile(aspect, profile, fromNetwork);
}

void CScriptbind_GameObject::EnablePhysicsEvent(IGameObject *pGameObject, bool enable, int event)
{
	pGameObject->EnablePhysicsEvent(enable, event);
}

bool CScriptbind_GameObject::WantsPhysicsEvent(IGameObject *pGameObject, int event)
{
	return pGameObject->WantsPhysicsEvent(event);
}

bool CScriptbind_GameObject::BindToNetwork(IGameObject *pGameObject, EBindToNetworkMode mode)
{
	return pGameObject->BindToNetwork(mode);
}