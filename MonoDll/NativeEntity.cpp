#include "StdAfx.h"
#include "NativeEntity.h"
#include "MonoEntityPropertyHandler.h"

#include "Scriptbinds\Entity.h"

#include <IEntityClass.h>

#include <IMonoScriptSystem.h>
#include <IMonoAssembly.h>
#include <IMonoClass.h>
#include <IMonoConverter.h>

#include <MonoCommon.h>

CNativeEntity::CNativeEntity()
	: m_bInitialized(false)
{
}

bool CNativeEntity::Init(IGameObject *pGameObject)
{
	SetGameObject(pGameObject);

	pGameObject->EnablePhysicsEvent( true, eEPE_OnPostStepImmediate );

	// InternalSpawn

	m_bInitialized = true;

	if (!GetGameObject()->BindToNetwork())
		return false;

	return true;
}

void CNativeEntity::ProcessEvent(SEntityEvent &event)
{
	switch(event.event)
	{
	case ENTITY_EVENT_LEVEL_LOADED:
		// OnInit
		break;
	case ENTITY_EVENT_RESET:
		{
			bool enterGamemode = event.nParam[0]==1;

			if(!enterGamemode && GetEntity()->GetFlags() & ENTITY_FLAG_NO_SAVE)
				gEnv->pEntitySystem->RemoveEntity(GetEntityId());

			// OnReset
			
			Physicalize(); // Testing physicalization in non-managed entities
		}
		break;
	case ENTITY_EVENT_COLLISION:
		{
			EventPhysCollision *pCollision = (EventPhysCollision *)event.nParam[0];

			EntityId targetId = 0;

			IEntity *pTarget = pCollision->iForeignData[0]==PHYS_FOREIGN_ID_ENTITY ? (IEntity*)pCollision->pForeignData[0]:0;
			if(pTarget)
				targetId = pTarget->GetId();

			//m_pScript->CallMethod("OnCollision", targetId, pCollision->pt, pCollision->vloc[0].GetNormalizedSafe(), pCollision->idmat[0], pCollision->n);
		}
		break;
	case ENTITY_EVENT_START_GAME:
		//m_pScript->CallMethod("OnStartGame");
		break;
	case ENTITY_EVENT_START_LEVEL:
		//m_pScript->CallMethod("OnStartLevel");
		break;
	case ENTITY_EVENT_XFORM:
		//m_pScript->CallMethod("OnMove");
		break;
	case ENTITY_EVENT_ATTACH:
		//m_pScript->CallMethod("OnAttach", (EntityId)event.nParam[0]);
		break;
	case ENTITY_EVENT_DETACH:
		//m_pScript->CallMethod("OnDetach", (EntityId)event.nParam[0]);
		break;
	case ENTITY_EVENT_DETACH_THIS:
		//m_pScript->CallMethod("OnDetachThis", (EntityId)event.nParam[0]);
		break;
	}
}

void CNativeEntity::Physicalize()
{
	IEntity *pEntity = GetEntity();

	pEntity->LoadGeometry(0, "objects/tank_env_assets/scifi/yellowcrate.cgf");

	// Unphysicalize
	{
		const Ang3 oldRotation = pEntity->GetWorldAngles();
		const Quat newRotation = Quat::CreateRotationZ( oldRotation.z );
		pEntity->SetRotation( newRotation );

		SEntityPhysicalizeParams pp;
		pp.type = PE_NONE;
		pEntity->Physicalize( pp );
	}
	// ~Unphysicalize

	SEntityPhysicalizeParams pp;

	pp.type = PE_RIGID;
	pp.mass = 20;
	pp.nSlot = 0;

	pEntity->Physicalize(pp);
}