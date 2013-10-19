#include "stdafx.h"
#include "EntityEventHandling.h"

#include "MonoScriptSystem.h"

#include "MonoEntity.h"

IMonoClass *CEntityEventHandler::m_pClass[2];

void CEntityEventHandler::HandleEntityEvent(EEntityType type, SEntityEvent &event, IEntity *pEntity, mono::object managedObject)
{
	switch(event.event)
	{
	case ENTITY_EVENT_RESET:
		{
			bool enterGamemode = event.nParam[0]==1;

			if(!enterGamemode && pEntity->GetFlags() & ENTITY_FLAG_NO_SAVE)
			{
				gEnv->pEntitySystem->RemoveEntity(pEntity->GetId());
				return;
			}

			IMonoArray *pParams = CreateMonoArray(1);
			pParams->Insert(enterGamemode);
			m_pClass[type]->GetMethod("OnEditorReset", 1)->InvokeArray(managedObject, pParams);

			SAFE_RELEASE(pParams);
		}
		break;
	case ENTITY_EVENT_COLLISION:
		{
			EventPhysCollision *pCollision = (EventPhysCollision *)event.nParam[0];

			SMonoColliderInfo source = SMonoColliderInfo(pCollision, 0);
			SMonoColliderInfo target = SMonoColliderInfo(pCollision, 1);

			IMonoClass *pColliderInfoClass = g_pScriptSystem->GetCryBraryAssembly()->GetClass("ColliderInfo");

			IMonoArray *pArgs = CreateMonoArray(6);

			pArgs->InsertMonoObject(pColliderInfoClass->BoxObject(&source));
			pArgs->InsertMonoObject(pColliderInfoClass->BoxObject(&target));

			pArgs->Insert(pCollision->pt);
			pArgs->Insert(pCollision->n);

			pArgs->Insert(pCollision->penetration);
			pArgs->Insert(pCollision->radius);

			m_pClass[type]->GetMethod("OnCollision", 6)->InvokeArray(managedObject, pArgs);
			SAFE_RELEASE(pArgs);
		}
		break;
	case ENTITY_EVENT_START_GAME:
		m_pClass[type]->GetMethod("OnStartGame")->Invoke(managedObject);
		break;
	case ENTITY_EVENT_START_LEVEL:
		m_pClass[type]->GetMethod("OnStartLevel")->Invoke(managedObject);
		break;
	case ENTITY_EVENT_LEVEL_LOADED:
		m_pClass[type]->GetMethod("OnLevelLoaded")->Invoke(managedObject);
		break;
	case ENTITY_EVENT_ENTERAREA:
		m_pClass[type]->GetMethod("OnEnterArea", 3)->Call(managedObject, (EntityId)event.nParam[0], (int)event.nParam[1], (EntityId)event.nParam[2]);
		break;
	case ENTITY_EVENT_MOVEINSIDEAREA:
		m_pClass[type]->GetMethod("OnMoveInsideArea", 3)->Call(managedObject, (EntityId)event.nParam[0], (int)event.nParam[1], (EntityId)event.nParam[2]);
		break;
	case ENTITY_EVENT_LEAVEAREA:
		m_pClass[type]->GetMethod("OnLeaveArea", 3)->Call(managedObject, (EntityId)event.nParam[0], (int)event.nParam[1], (EntityId)event.nParam[2]);
		break;
	case ENTITY_EVENT_ENTERNEARAREA:
		m_pClass[type]->GetMethod("OnEnterNearArea", 3)->Call(managedObject, (EntityId)event.nParam[0], (int)event.nParam[1], (EntityId)event.nParam[2]);
		break;
	case ENTITY_EVENT_MOVENEARAREA:
		m_pClass[type]->GetMethod("OnMoveNearArea", 4)->Call(managedObject, (EntityId)event.nParam[0], (int)event.nParam[1], (EntityId)event.nParam[2], event.fParam[0]);
		break;
	case ENTITY_EVENT_LEAVENEARAREA:
		m_pClass[type]->GetMethod("OnLeaveNearArea", 3)->Call(managedObject, (EntityId)event.nParam[0], (int)event.nParam[1], (EntityId)event.nParam[2]);
		break;
	case ENTITY_EVENT_XFORM:
		m_pClass[type]->GetMethod("OnMove", 1)->Call(managedObject, (EEntityXFormFlags)event.nParam[0]);
		break;
	case ENTITY_EVENT_ATTACH:
		m_pClass[type]->GetMethod("OnAttach", 1)->Call(managedObject, (EntityId)event.nParam[0]);
		break;
	case ENTITY_EVENT_DETACH:
		m_pClass[type]->GetMethod("OnDetach", 1)->Call(managedObject, (EntityId)event.nParam[0]);
		break;
	case ENTITY_EVENT_DETACH_THIS:
		m_pClass[type]->GetMethod("OnDetachThis", 1)->Call(managedObject, (EntityId)event.nParam[0]);
		break;
	case ENTITY_EVENT_ANIM_EVENT:
		{
			const AnimEventInstance* pAnimEvent = reinterpret_cast<const AnimEventInstance*>(event.nParam[0]);
			ICharacterInstance* pCharacter = reinterpret_cast<ICharacterInstance*>(event.nParam[1]);

			IMonoClass *pAnimationEventClass = g_pScriptSystem->GetCryBraryAssembly()->GetClass("AnimationEvent");

			SMonoAnimationEvent animEvent(pAnimEvent);

			IMonoArray *pArgs = CreateMonoArray(1);

			pArgs->InsertMonoObject(pAnimationEventClass->BoxObject(&animEvent));

			m_pClass[type]->GetMethod("OnAnimEvent", 1)->InvokeArray(managedObject, pArgs);
			SAFE_RELEASE(pArgs);
		}
		break;
	case ENTITY_EVENT_PREPHYSICSUPDATE:
		{
			m_pClass[type]->GetMethod("OnPrePhysicsUpdate")->Invoke(managedObject);
		}
		break;
	}
}

void CEntityEventHandler::CacheManagedResources()
{
	m_pClass[Entity] = g_pScriptSystem->GetCryBraryAssembly()->GetClass("Entity");
	m_pClass[Actor] = g_pScriptSystem->GetCryBraryAssembly()->GetClass("Actor");
}