#include "stdafx.h"
#include "EntityEventHandling.h"

#include "MonoScriptSystem.h"

#include "MonoEntity.h"

IMonoMethod *CEntityEventHandler::m_pOnEditorReset[2];

IMonoMethod *CEntityEventHandler::m_pOnCollision[2];

IMonoMethod *CEntityEventHandler::m_pOnStartGame[2];
IMonoMethod *CEntityEventHandler::m_pOnStartLevel[2];
IMonoMethod *CEntityEventHandler::m_pOnLevelLoaded[2];

IMonoMethod *CEntityEventHandler::m_pOnEnterArea[2];
IMonoMethod *CEntityEventHandler::m_pOnMoveInsideArea[2];
IMonoMethod *CEntityEventHandler::m_pOnLeaveArea[2];
IMonoMethod *CEntityEventHandler::m_pOnEnterNearArea[2];
IMonoMethod *CEntityEventHandler::m_pOnMoveNearArea[2];
IMonoMethod *CEntityEventHandler::m_pOnLeaveNearArea[2];

IMonoMethod *CEntityEventHandler::m_pOnMove[2];

IMonoMethod *CEntityEventHandler::m_pOnAttach[2];
IMonoMethod *CEntityEventHandler::m_pOnDetach[2];
IMonoMethod *CEntityEventHandler::m_pOnDetachThis[2];

IMonoMethod *CEntityEventHandler::m_pOnAnimEvent[2];

IMonoMethod *CEntityEventHandler::m_pPrePhysicsUpdate[2];

void CEntityEventHandler::CacheMethods()
{
	IMonoClass *pEntityClass = g_pScriptSystem->GetCryBraryAssembly()->GetClass("Entity");
	CRY_ASSERT(pEntityClass);

	IMonoClass *pActorClass = g_pScriptSystem->GetCryBraryAssembly()->GetClass("Actor");
	CRY_ASSERT(pActorClass);

#define GET_METHOD(function, name, paramCount) \
	{ \
		function[Entity] = pEntityClass->GetMethod(name, paramCount); \
		function[Actor] = pActorClass->GetMethod(name, paramCount); \
	}

	GET_METHOD(m_pOnEditorReset, "OnEditorReset", 1);

	GET_METHOD(m_pOnCollision, "OnCollision", 6);

	GET_METHOD(m_pOnStartGame, "OnStartGame", 0);
	GET_METHOD(m_pOnStartLevel, "OnStartLevel", 0);
	GET_METHOD(m_pOnLevelLoaded, "OnLevelLoaded", 0);

	GET_METHOD(m_pOnEnterArea, "OnEnterArea", 3);
	GET_METHOD(m_pOnMoveInsideArea, "OnMoveInsideArea", 3);
	GET_METHOD(m_pOnLeaveArea, "OnLeaveArea", 3);
	GET_METHOD(m_pOnEnterNearArea, "OnEnterNearArea", 3);
	GET_METHOD(m_pOnMoveNearArea, "OnMoveNearArea", 4);
	GET_METHOD(m_pOnLeaveNearArea, "OnLeaveNearArea", 3);

	GET_METHOD(m_pOnMove, "OnMove", 1);

	GET_METHOD(m_pOnAttach, "OnAttach", 1);
	GET_METHOD(m_pOnDetach, "OnDetach", 1);
	GET_METHOD(m_pOnDetachThis, "OnDetachThis", 1);

	GET_METHOD(m_pOnAnimEvent, "OnAnimationEvent", 1);

	GET_METHOD(m_pPrePhysicsUpdate, "OnPrePhysicsUpdate", 0);

#undef GET_METHOD
}

void CEntityEventHandler::HandleEntityEvent(EEntityType type, SEntityEvent &event, IEntity *pEntity, IMonoObject *pScript)
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
			m_pOnEditorReset[type]->InvokeArray(pScript->GetManagedObject(), pParams);

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

			m_pOnCollision[type]->InvokeArray(pScript->GetManagedObject(), pArgs);
			SAFE_RELEASE(pArgs);
		}
		break;
	case ENTITY_EVENT_START_GAME:
		m_pOnStartGame[type]->Invoke(pScript->GetManagedObject());
		break;
	case ENTITY_EVENT_START_LEVEL:
		m_pOnStartLevel[type]->Invoke(pScript->GetManagedObject());
		break;
	case ENTITY_EVENT_LEVEL_LOADED:
		m_pOnLevelLoaded[type]->Invoke(pScript->GetManagedObject());
		break;
	case ENTITY_EVENT_ENTERAREA:
		m_pOnEnterArea[type]->Call(pScript->GetManagedObject(), (EntityId)event.nParam[0], (int)event.nParam[1], (EntityId)event.nParam[2]);
		break;
	case ENTITY_EVENT_MOVEINSIDEAREA:
		m_pOnMoveInsideArea[type]->Call(pScript->GetManagedObject(), (EntityId)event.nParam[0], (int)event.nParam[1], (EntityId)event.nParam[2]);
		break;
	case ENTITY_EVENT_LEAVEAREA:
		m_pOnLeaveArea[type]->Call(pScript->GetManagedObject(), (EntityId)event.nParam[0], (int)event.nParam[1], (EntityId)event.nParam[2]);
		break;
	case ENTITY_EVENT_ENTERNEARAREA:
		m_pOnEnterNearArea[type]->Call(pScript->GetManagedObject(), (EntityId)event.nParam[0], (int)event.nParam[1], (EntityId)event.nParam[2]);
		break;
	case ENTITY_EVENT_MOVENEARAREA:
		m_pOnMoveNearArea[type]->Call(pScript->GetManagedObject(), (EntityId)event.nParam[0], (int)event.nParam[1], (EntityId)event.nParam[2], event.fParam[0]);
		break;
	case ENTITY_EVENT_LEAVENEARAREA:
		m_pOnLeaveNearArea[type]->Call(pScript->GetManagedObject(), (EntityId)event.nParam[0], (int)event.nParam[1], (EntityId)event.nParam[2]);
		break;
	case ENTITY_EVENT_XFORM:
		m_pOnMove[type]->Call(pScript->GetManagedObject(), (EEntityXFormFlags)event.nParam[0]);
		break;
	case ENTITY_EVENT_ATTACH:
		m_pOnAttach[type]->Call(pScript->GetManagedObject(), (EntityId)event.nParam[0]);
		break;
	case ENTITY_EVENT_DETACH:
		m_pOnDetach[type]->Call(pScript->GetManagedObject(), (EntityId)event.nParam[0]);
		break;
	case ENTITY_EVENT_DETACH_THIS:
		m_pOnDetachThis[type]->Call(pScript->GetManagedObject(), (EntityId)event.nParam[0]);
		break;
	case ENTITY_EVENT_ANIM_EVENT:
		{
			const AnimEventInstance* pAnimEvent = reinterpret_cast<const AnimEventInstance*>(event.nParam[0]);
			ICharacterInstance* pCharacter = reinterpret_cast<ICharacterInstance*>(event.nParam[1]);

			IMonoClass *pAnimationEventClass = g_pScriptSystem->GetCryBraryAssembly()->GetClass("AnimationEvent");

			SMonoAnimationEvent animEvent(pAnimEvent);

			IMonoArray *pArgs = CreateMonoArray(1);

			pArgs->InsertMonoObject(pAnimationEventClass->BoxObject(&animEvent));

			m_pOnAnimEvent[type]->InvokeArray(pScript->GetManagedObject(), pArgs);
			SAFE_RELEASE(pArgs);
		}
		break;
	case ENTITY_EVENT_PREPHYSICSUPDATE:
		{
			m_pPrePhysicsUpdate[type]->Invoke(pScript->GetManagedObject());
		}
		break;
	}
}