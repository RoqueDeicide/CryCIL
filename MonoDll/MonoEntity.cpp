#include "StdAfx.h"
#include "MonoEntity.h"
#include "MonoEntityPropertyHandler.h"

#include "MonoRunTime.h"
#include "Interops/Entities/Entity.h"

#include "EntityEventHandling.h"

#include "MonoCVars.h"

#include <IEntityClass.h>

#include <IMonoAssembly.h>
#include <IMonoClass.h>

#include <MonoCommon.h>

CMonoEntityExtension::CMonoEntityExtension()
	: m_pScript(nullptr)
	, m_pManagedObject(nullptr)
	, m_bInitialized(false)
	, m_pAnimatedCharacter(nullptr)
	, m_bDestroyed(false)
{}

CMonoEntityExtension::~CMonoEntityExtension()
{
	if (m_pAnimatedCharacter)
	{
		IGameObject *pGameObject = GetGameObject();
		pGameObject->ReleaseExtension("AnimatedCharacter");
	}
}

bool CMonoEntityExtension::Init(IGameObject *pGameObject)
{
	SetGameObject(pGameObject);

	pGameObject->EnablePhysicsEvent(true, eEPE_OnPostStepImmediate);

	if (!GetGameObject()->BindToNetwork() && g_pMonoCVars->mono_entityDeleteExtensionOnNetworkBindFailure == 1)
		return false;

	IEntity *pEntity = GetEntity();
	IEntityClass *pEntityClass = pEntity->GetClass();

	IMonoClass *pEntityInfoClass =
		GetMonoRunTime()->CryBrary->GetClass("EntityInitializationParams", "CryEngine.Native");

	SMonoEntityInfo entityInfo(pEntity);

	IMonoArray *pArgs = CreateMonoArray(2);
	pArgs->InsertMonoString(ToMonoString(pEntityClass->GetName()));
	pArgs->InsertMonoObject(pEntityInfoClass->BoxObject(&entityInfo));
	// Create the wrapper and save it.
	m_pManagedObject =
		GetMonoRunTime()->CryBrary->
		GetClass("Entity", "CryEngine.Entities")->
		GetMethod("Create")->InvokeArray(nullptr, pArgs);
	pArgs->Release();

	int numProperties;
	auto pProperties = static_cast<CEntityPropertyHandler *>(pEntityClass->GetPropertyHandler())->GetQueuedProperties(pEntity->GetId(), numProperties);

	if (pProperties)
	{
		for (int i = 0; i < numProperties; i++)
		{
			auto queuedProperty = pProperties[i];

			SetPropertyValue(queuedProperty.propertyInfo, queuedProperty.value.c_str());
		}
	}

	m_bInitialized = true;

	return true;
}

void CMonoEntityExtension::PostInit(IGameObject *pGameObject)
{
	Reset(false);
}

void CMonoEntityExtension::Reset(bool enteringGamemode)
{
	if (m_pAnimatedCharacter)
		m_pAnimatedCharacter->ResetState();
	else if (m_pAnimatedCharacter = static_cast<IAnimatedCharacter *>(GetGameObject()->QueryExtension("AnimatedCharacter")))
		m_pAnimatedCharacter->ResetState();
}

void CMonoEntityExtension::ProcessEvent(SEntityEvent &event)
{
	// Don't attempt to send any events to managed code when the entity has been destroyed.
	if (m_bDestroyed)
		return;

	CEntityEventHandler::HandleEntityEvent(CEntityEventHandler::Entity, event, GetEntity(), m_pManagedObject);

	switch (event.event)
	{
	case ENTITY_EVENT_RESET:
		Reset(event.nParam[0] == 1);
		break;
	case ENTITY_EVENT_DONE:
		m_bDestroyed = true;
		break;
	}
}

void CMonoEntityExtension::PostUpdate(float frameTime)
{
	this->m_pManagedObject->CallMethod("OnPostUpdate", frameTime);
}

void CMonoEntityExtension::FullSerialize(TSerialize ser)
{
	IEntity *pEntity = GetEntity();

	ser.BeginGroup("Properties");
	auto pPropertyHandler = static_cast<CEntityPropertyHandler *>(pEntity->GetClass()->GetPropertyHandler());
	for (int i = 0; i < pPropertyHandler->GetPropertyCount(); i++)
	{
		if (ser.IsWriting())
		{
			IEntityPropertyHandler::SPropertyInfo propertyInfo;
			pPropertyHandler->GetPropertyInfo(i, propertyInfo);

			ser.Value(propertyInfo.name, pPropertyHandler->GetProperty(pEntity, i));
		}
		else
		{
			IEntityPropertyHandler::SPropertyInfo propertyInfo;
			pPropertyHandler->GetPropertyInfo(i, propertyInfo);

			char *propertyValue = nullptr;
			ser.ValueChar(propertyInfo.name, propertyValue, 0);

			pPropertyHandler->SetProperty(pEntity, i, propertyValue);
		}
	}

	ser.EndGroup();

	ser.BeginGroup("ManagedEntity");

	IMonoArray *pArgs = CreateMonoArray(1);
	pArgs->InsertNativePointer(&ser);

	this->m_pManagedObject->GetClass()->
		GetMethod("InternalFullSerialize", 1)->InvokeArray(m_pManagedObject, pArgs);
	pArgs->Release();

	ser.EndGroup();
}

bool CMonoEntityExtension::NetSerialize(TSerialize ser, EEntityAspects aspect, uint8 profile, int flags)
{
	ser.BeginGroup("ManagedEntity");

	void *params[4];
	params[0] = &ser;
	params[1] = &aspect;
	params[2] = &profile;
	params[3] = &flags;

	this->m_pManagedObject->GetClass()->GetMethod("InternalNetSerialize", 4)->Invoke(m_pManagedObject, params);

	ser.EndGroup();

	return true;
}

void CMonoEntityExtension::PostSerialize()
{
	this->m_pManagedObject->CallMethod("PostSerialize");
}

void CMonoEntityExtension::SetPropertyValue(IEntityPropertyHandler::SPropertyInfo propertyInfo, const char *value)
{
	if (value != nullptr)
		this->m_pManagedObject->CallMethod("SetPropertyValue", propertyInfo.name, propertyInfo.type, value);
}

///////////////////////////////////////////////////
// Entity RMI's
///////////////////////////////////////////////////
CMonoEntityExtension::RMIParams::RMIParams(IMonoObject *_args, const char *funcName, EntityId target)
	: methodName(funcName)
	, targetId(target)
	, args(_args)
{}

void CMonoEntityExtension::RMIParams::SerializeWith(TSerialize ser)
{
	IMonoArray *pArgs;
	int length;

	if (this->args != nullptr)
	{
		pArgs = GetMonoRunTime()->ToArray(this->args);
		length = pArgs->GetSize();
	}
	else
		length = 0;

	ser.Value("length", length);

	ser.Value("methodName", methodName);
	ser.Value("targetId", targetId, 'eid');

	if (length > 0)
	{
		if (ser.IsWriting())
		{
			for (int i = 0; i < length; i++)
			{
				IMonoObject *pItem = pArgs->GetItem(i);
				pItem->GetAnyValue().SerializeWith(ser);
				SAFE_RELEASE(pItem);
			}
		}
		else
		{
			pArgs = GetMonoScriptSystem()->AppDomain->CreateArray(length);

			for (int i = 0; i < length; i++)
			{
				MonoAnyValue value;
				value.SerializeWith(ser);
				pArgs->InsertAny(value, i);
			}

			args = pArgs->GetManagedObject();
		}

		pArgs->Release();
	}
}

IMPLEMENT_RMI(CMonoEntityExtension, SvScriptRMI)
{
	IMonoClass *pEntityClass = GetMonoRunTime()->CryBrary->GetClass("Entity");

	IMonoArray *pNetworkArgs = CreateMonoArray(3);
	pNetworkArgs->Insert(ToMonoString(params.methodName.c_str()));
	pNetworkArgs->InsertMonoObject(params.args);
	pNetworkArgs->Insert(params.targetId);

	pEntityClass->GetMethod("OnRemoteInvocation", 3)->InvokeArray(nullptr, pNetworkArgs);
	pNetworkArgs->Release();

	return true;
}

IMPLEMENT_RMI(CMonoEntityExtension, ClScriptRMI)
{
	IMonoClass *pEntityClass = GetMonoRunTime()->CryBrary->GetClass("Entity");

	IMonoArray *pNetworkArgs = CreateMonoArray(3);
	pNetworkArgs->Insert(ToMonoString(params.methodName.c_str()));
	pNetworkArgs->InsertMonoObject(params.args);
	pNetworkArgs->Insert(params.targetId);

	pEntityClass->GetMethod("OnRemoteInvocation", 3)->InvokeArray(nullptr, pNetworkArgs);
	pNetworkArgs->Release();

	return true;
}