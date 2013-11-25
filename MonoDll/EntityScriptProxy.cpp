#include "StdAfx.h"
#include "EntityScriptProxy.h"
#include "EntityManager.h"

#include <IEntityClass.h>

#include <IMonoScriptSystem.h>

CEntityScriptProxy::CEntityScriptProxy(IEntity *pEntity, SEntitySpawnParams &params)
	: m_pEntity(pEntity)
	, m_currentState(0)
{
	m_pScript = gEnv->pMonoScriptSystem->InstantiateScript(params.pClass->GetName(), eScriptType_Entity);

	IMonoClass *pEntityInfoClass = gEnv->pMonoScriptSystem->GetCryBraryAssembly()->GetCustomClass("EntityInfo");

	CallMonoScript<void>(m_pScript, "InternalSpawn", gEnv->pMonoScriptSystem->GetConverter()->ToManagedType(pEntityInfoClass, &SMonoEntityInfo(pEntity, params.id)));

	Init(pEntity, params);
}

CEntityScriptProxy::~CEntityScriptProxy()
{
	SAFE_RELEASE(m_pScript);
}

void CEntityScriptProxy::Release()
{
	CryLogAlways("release");

	delete this;
}

bool CEntityScriptProxy::Init( IEntity *pEntity,SEntitySpawnParams &params )
{
	m_pScript->CallMethod("OnInit");

	return true;
}

void CEntityScriptProxy::Reload(IEntity *pEntity,SEntitySpawnParams &params)
{
}

void CEntityScriptProxy::Done()
{
	m_pScript->CallMethod("OnRemove");
}

void CEntityScriptProxy::ProcessEvent( SEntityEvent &event )
{
	switch (event.event)
	{
	case ENTITY_EVENT_RESET:
		// OnReset()
		CallMonoScript<void>(m_pScript, "OnReset", event.nParam[0]==1);
		break;
	case ENTITY_EVENT_INIT:
		m_pScript->CallMethod("OnInit");
		break;
	case ENTITY_EVENT_TIMER:
		CallMonoScript<void>(m_pScript, "OnTimer", (int)event.nParam[0], (int)event.nParam[1]);
		break;
	case ENTITY_EVENT_XFORM:
		m_pScript->CallMethod("OnMove");
		break;
	case ENTITY_EVENT_ATTACH:
		CallMonoScript<void>(m_pScript, "OnAttach", (EntityId)event.nParam[0]);
		break;
	case ENTITY_EVENT_DETACH:
	case ENTITY_EVENT_DETACH_THIS:
		CallMonoScript<void>(m_pScript, "OnDetach", (EntityId)event.nParam[0]);
		break;
	case ENTITY_EVENT_ENTERAREA:
		CallMonoScript<void>(m_pScript, "OnEnterArea", (EntityId)event.nParam[2], (EntityId)event.nParam[1], event.fParam[0]);
		break;
	case ENTITY_EVENT_MOVEINSIDEAREA:
		CallMonoScript<void>(m_pScript, "OnMoveInsideArea", (EntityId)event.nParam[0], (int)event.nParam[1], event.fParam[0]);
		break;	
	case ENTITY_EVENT_LEAVEAREA:
		CallMonoScript<void>(m_pScript, "OnLeaveArea", (EntityId)event.nParam[2], (EntityId)event.nParam[1], event.fParam[0]);
		break;
	case ENTITY_EVENT_ENTERNEARAREA:
		CallMonoScript<void>(m_pScript, "OnEnterNearArea", (EntityId)event.nParam[0], (int)event.nParam[1], event.fParam[0]);
		break;
	case ENTITY_EVENT_LEAVENEARAREA:
		CallMonoScript<void>(m_pScript, "OnLeaveNearArea", (EntityId)event.nParam[0], (int)event.nParam[1], event.fParam[0]);
		break;
	case ENTITY_EVENT_MOVENEARAREA:
		CallMonoScript<void>(m_pScript, "OnMoveNearArea", (EntityId)event.nParam[0], (int)event.nParam[1], event.fParam[0], event.fParam[1]);
		break;
	case ENTITY_EVENT_PHYS_BREAK:
		{
			EventPhysJointBroken *pBreakEvent = (EventPhysJointBroken*)event.nParam[0];
			Vec3 breakPos = pBreakEvent->pt;
			int nBreakPartId = pBreakEvent->partid[0];
			int nBreakOtherEntityPartId = pBreakEvent->partid[1];

			CallMonoScript<void>(m_pScript, "OnBreak", breakPos, nBreakPartId, nBreakOtherEntityPartId);
		}
		break;
	case ENTITY_EVENT_COLLISION:
		{
			EventPhysCollision *pCollision = (EventPhysCollision *)event.nParam[0];

			EntityId targetId = 0;

			IEntity *pTarget = pCollision->iForeignData[0]==PHYS_FOREIGN_ID_ENTITY ? (IEntity*)pCollision->pForeignData[0]:0;
			if(pTarget)
				targetId = pTarget->GetId();

			CallMonoScript<void>(m_pScript, "OnCollision", targetId, pCollision->pt, pCollision->vloc[0].GetNormalizedSafe(), pCollision->idmat[0], pCollision->n);
		}
		break;
	case ENTITY_EVENT_SOUND_DONE:
		CallMonoScript<void>(m_pScript, "OnSoundDone");
		break;
	case ENTITY_EVENT_LEVEL_LOADED:
		CallMonoScript<void>(m_pScript, "OnLevelLoaded");
		break;
	case ENTITY_EVENT_START_LEVEL:
		CallMonoScript<void>(m_pScript, "OnStartLevel");
		break;
	case ENTITY_EVENT_START_GAME:
		CallMonoScript<void>(m_pScript, "OnStartGame");
		break;
	
	case ENTITY_EVENT_PRE_SERIALIZE:
		{
			m_currentState = 0;

			m_pEntity->KillTimer(-1);
		}
		break;
	};
}

void CEntityScriptProxy::GetMemoryUsage( ICrySizer *pSizer ) const
{
	pSizer->Add(*this);
}

void CEntityScriptProxy::Serialize( TSerialize ser )
{
}

void CEntityScriptProxy::SerializeXML(XmlNodeRef &entityNode,bool bLoading)
{
	if(bLoading)
	{
		// Deserialize properties
	}
	else
	{
		// Serialize properties
	}
}