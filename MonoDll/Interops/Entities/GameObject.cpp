#include "StdAfx.h"
#include "GameObject.h"

#include "MonoScriptSystem.h"

GameObjectInterop::GameObjectInterop()
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

IGameObject *GameObjectInterop::GetGameObject(EntityId id)
{
	return static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetIGameFramework()->GetGameObject(id);
}

void GameObjectInterop::EnablePostUpdates(IGameObject *pGameObject, IGameObjectExtension *pExtension, bool enable)
{
	if (enable)
		pGameObject->EnablePostUpdates(pExtension);
	else
		pGameObject->DisablePostUpdates(pExtension);
}

void GameObjectInterop::EnablePrePhysicsUpdates(IGameObject *pGameObject, EPrePhysicsUpdate rule)
{
	pGameObject->EnablePrePhysicsUpdate(rule);
}

IGameObjectExtension *GameObjectInterop::QueryExtension(IGameObject *pGameObject, mono::string name)
{
	return pGameObject->QueryExtension(ToCryString(name));
}

IGameObjectExtension *GameObjectInterop::AcquireExtension(IGameObject *pGameObject, mono::string name)
{
	return pGameObject->AcquireExtension(ToCryString(name));
}

void GameObjectInterop::ReleaseExtension(IGameObject *pGameObject, mono::string name)
{
	pGameObject->ReleaseExtension(ToCryString(name));
}

bool GameObjectInterop::ActivateExtension(IGameObject *pGameObject, mono::string name)
{
	return pGameObject->ActivateExtension(ToCryString(name));
}

void GameObjectInterop::DeactivateExtension(IGameObject *pGameObject, mono::string name)
{
	pGameObject->DeactivateExtension(ToCryString(name));
}

void GameObjectInterop::ChangedNetworkState(IGameObject *pGameObject, int aspect)
{
	pGameObject->ChangedNetworkState((NetworkAspectType)aspect);
}

bool GameObjectInterop::SetAspectProfile(IGameObject *pGameObject, EEntityAspects aspect, uint8 profile, bool fromNetwork)
{
	return pGameObject->SetAspectProfile(aspect, profile, fromNetwork);
}

void GameObjectInterop::EnablePhysicsEvent(IGameObject *pGameObject, bool enable, int event)
{
	pGameObject->EnablePhysicsEvent(enable, event);
}

bool GameObjectInterop::WantsPhysicsEvent(IGameObject *pGameObject, int event)
{
	return pGameObject->WantsPhysicsEvent(event);
}

bool GameObjectInterop::BindToNetwork(IGameObject *pGameObject, EBindToNetworkMode mode)
{
	return pGameObject->BindToNetwork(mode);
}