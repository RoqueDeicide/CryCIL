
#include "StdAfx.h"
#include "Actor.h"

#include "MonoScriptSystem.h"

#include "EntityEventHandling.h" 

#include <IGameRulesSystem.h>
#include <IViewSystem.h>
#include <ICryAnimation.h>

#include <IMonoAssembly.h>
#include <IMonoClass.h>

CMonoActor::CMonoActor()
	: m_pAnimatedCharacter(NULL)
	, m_bClient(false)
	, m_bMigrating(false)
	, m_pScript(nullptr)
	, m_pManagedObject(nullptr)
{
	m_currentPhysProfile=GetDefaultProfile(eEA_Physics);
}


CMonoActor::~CMonoActor()
{
	GetGameObject()->EnablePhysicsEvent( false, eEPE_OnPostStepImmediate );

	if (m_pAnimatedCharacter)
	{
		GetGameObject()->ReleaseExtension("AnimatedCharacter");
		GetGameObject()->DeactivateExtension("AnimatedCharacter");
	}

	GetGameObject()->ReleaseView(this);
	GetGameObject()->ReleaseProfileManager(this);

	if(g_pScriptSystem)
	{
		if(IActorSystem *pActorSystem = g_pScriptSystem->GetIGameFramework()->GetIActorSystem())
			pActorSystem->RemoveActor(GetEntityId());

		g_pScriptSystem->RemoveListener(this);
	}
}

bool CMonoActor::Init(IGameObject *pGameObject)
{
	SetGameObject(pGameObject);

	if(!GetGameObject()->CaptureView(this))
		return false;
	if (!GetGameObject()->CaptureProfileManager(this))
		return false;

	g_pScriptSystem->GetIGameFramework()->GetIActorSystem()->AddActor(GetEntityId(), this);

	m_pAnimatedCharacter = static_cast<IAnimatedCharacter*>(GetGameObject()->AcquireExtension("AnimatedCharacter"));
	if (m_pAnimatedCharacter)
		GetGameObject()->EnablePhysicsEvent( true, eEPE_OnPostStepImmediate );

	if(!GetGameObject()->BindToNetwork())
		return false;

	GetEntity()->SetFlags(GetEntity()->GetFlags()|(ENTITY_FLAG_ON_RADAR|ENTITY_FLAG_CUSTOM_VIEWDIST_RATIO));

	GetGameObject()->SetAspectProfile(eEA_Physics, eAP_Alive);

	if(IEntityRenderProxy *pProxy = (IEntityRenderProxy *)GetEntity()->GetProxy(ENTITY_PROXY_RENDER))
	{
		if(IRenderNode *pRenderNode = pProxy->GetRenderNode())
			pRenderNode->SetRndFlags(ERF_REGISTER_BY_POSITION, true);
	}

	GetMonoScriptSystem()->AddListener(this);

	return true; 
}

void CMonoActor::PostInit(IGameObject *pGameObject)
{
	if (m_pAnimatedCharacter)
		m_pAnimatedCharacter->ResetState();

	if(gEnv->bMultiplayer)
		GetGameObject()->SetUpdateSlotEnableCondition( this, 0, eUEC_WithoutAI );
	else if (!gEnv->bServer)
		GetGameObject()->SetUpdateSlotEnableCondition( this, 0, eUEC_VisibleOrInRange );
}

bool CMonoActor::ReloadExtension( IGameObject *pGameObject, const SEntitySpawnParams &params )
{
	CRY_ASSERT(GetGameObject() == pGameObject);

	ResetGameObject();

	if (!GetGameObject()->CaptureView(this))
		return false;
	if (!GetGameObject()->CaptureProfileManager(this))
		return false;

	// Re-enable the physics post step callback and CollisionLogging (were cleared during ResetGameObject()).
	GetGameObject()->EnablePhysicsEvent( true, eEPE_OnPostStepImmediate | eEPE_OnCollisionLogged);

	if (!GetGameObject()->BindToNetwork())
		return false;

	g_pScriptSystem->GetIGameFramework()->GetIActorSystem()->RemoveActor(params.prevId);
	g_pScriptSystem->GetIGameFramework()->GetIActorSystem()->AddActor(GetEntityId(), this);

	SetAspectProfile(eEA_Physics, eAP_NotPhysicalized);

	return true;
}

void CMonoActor::PostReloadExtension( IGameObject * pGameObject, const SEntitySpawnParams &params )
{
	CRY_ASSERT(GetGameObject() == pGameObject);

	GetEntity()->SetFlags(GetEntity()->GetFlags() |
		(ENTITY_FLAG_ON_RADAR | ENTITY_FLAG_CUSTOM_VIEWDIST_RATIO | ENTITY_FLAG_TRIGGER_AREAS));
}

void CMonoActor::SetAuthority( bool auth )
{
	// we've been given authority of this entity, mark the physics as changed
	// so that we send a current position, failure to do this can result in server/client
	// disagreeing on where the entity is. most likely to happen on restart
	if(auth)
		CHANGED_NETWORK_STATE(this, eEA_Physics);
}

void CMonoActor::HandleEvent(const SGameObjectEvent &event)
{
	if (event.event == 276 /* Ragdoll, defined in GameDll ._. */)
	{
		GetGameObject()->SetAspectProfile(eEA_Physics, eAP_Ragdoll);
	}
	else if (event.event == eGFE_BecomeLocalPlayer)
	{
		IEntity *pEntity = GetEntity();
		pEntity->SetFlags(GetEntity()->GetFlags() | ENTITY_FLAG_TRIGGER_AREAS);
		// Invalidate the matrix in order to force an update through the area manager
		pEntity->InvalidateTM(ENTITY_XFORM_POS);

		m_bClient = true;
		
		// always update client's character
		if (ICharacterInstance * pCharacter = GetEntity()->GetCharacter(0))
			pCharacter->SetFlags(pCharacter->GetFlags() | CS_FLAG_UPDATE_ALWAYS);
	}
	else if (event.event == 277) //eCGE_EnablePhysicalCollider
	{
		m_pAnimatedCharacter->RequestPhysicalColliderMode(eColliderMode_Undefined, eColliderModeLayer_Game, "Actor::HandleEvent");
	}
	else if (event.event == 278) //eCGE_DisablePhysicalCollider
	{
		m_pAnimatedCharacter->RequestPhysicalColliderMode(eColliderMode_Disabled, eColliderModeLayer_Game, "Actor::HandleEvent");
	}
}

void CMonoActor::ProcessEvent(SEntityEvent& event)
{
	if(m_pManagedObject == nullptr)
		return;

	CEntityEventHandler::HandleEntityEvent(CEntityEventHandler::Actor, event, GetEntity(), m_pManagedObject);

	switch (event.event)
	{
	case ENTITY_EVENT_UNHIDE:
	case ENTITY_EVENT_VISIBLE:
		GetGameObject()->RequestRemoteUpdate(eEA_Physics | eEA_GameClientDynamic | eEA_GameServerDynamic | eEA_GameClientStatic | eEA_GameServerStatic);
		break;
	case ENTITY_EVENT_START_GAME:
		{
			GetGameObject()->RequestRemoteUpdate(eEA_Physics | eEA_GameClientDynamic | eEA_GameServerDynamic | eEA_GameClientStatic | eEA_GameServerStatic);

			if (m_pAnimatedCharacter)
				m_pAnimatedCharacter->ResetState();
		}
		break;
	case ENTITY_EVENT_RESET:
		{
			if (m_pAnimatedCharacter)
				m_pAnimatedCharacter->ResetState();

			GetGameObject()->RequestRemoteUpdate(eEA_Physics | eEA_GameClientDynamic | eEA_GameServerDynamic | eEA_GameClientStatic | eEA_GameServerStatic);
		}
		break;
	case ENTITY_EVENT_INIT:
		{
			if (m_pAnimatedCharacter)
				m_pAnimatedCharacter->ResetState();
		}
		break;
	case ENTITY_EVENT_XFORM:
		{
			CryLogAlways("XForm");
		}
		break;
  }  
}

void CMonoActor::PostUpdate(float frameTime)
{
	if(m_pScript)
		m_pScript->CallMethod("OnPostUpdate");
}

void CMonoActor::OnScriptInstanceInitialized(ICryScriptInstance *pScriptInstance)
{
	mono::object pIdResult = pScriptInstance->GetPropertyValue("Id", false);
	if(pIdResult)
	{
		IMonoObject *pObject = *pIdResult;
		EntityId id = pObject->Unbox<EntityId>();
		pObject->Release();

		if(id == GetEntityId())
		{
			m_pScript = pScriptInstance;
			m_pManagedObject = pScriptInstance->GetManagedObject();
		}
	}
}

void CMonoActor::UpdateView(SViewParams &viewParams)
{
	void *args[1];
	args[0] = &viewParams;

	m_pScript->GetClass()->GetMethod("UpdateView", 1)->Invoke(m_pManagedObject, args);
}

void CMonoActor::PostUpdateView(SViewParams &viewParams)
{
	void *args[1];
	args[0] = &viewParams;

	m_pScript->GetClass()->GetMethod("PostUpdateView", 1)->Invoke(m_pManagedObject, args);
}

bool CMonoActor::SetAspectProfile( EEntityAspects aspect, uint8 profile )
{
	bool res(false);

	if (aspect == eEA_Physics)
	{
		if (m_currentPhysProfile==profile && !gEnv->pSystem->IsSerializingFile()) //rephysicalize when loading savegame
			return true;

		bool wasFrozen=(m_currentPhysProfile==eAP_Frozen);

		switch (profile)
		{
		case eAP_NotPhysicalized:
			{
				SEntityPhysicalizeParams params;
				params.type = PE_NONE;
				GetEntity()->Physicalize(params);
			}
			res=true;
			break;
		case eAP_Spectator:
		case eAP_Alive:
			{
				// if we were asleep, we just want to wakeup
				if (profile==eAP_Alive && (m_currentPhysProfile==eAP_Sleep))
				{
					ICharacterInstance *pCharacter=GetEntity()->GetCharacter(0);
					if (pCharacter && pCharacter->GetISkeletonAnim())
					{
						IPhysicalEntity *pPhysicalEntity=0;
						Matrix34 delta(IDENTITY);

						if (pPhysicalEntity)
						{
							IEntityPhysicalProxy *pPhysicsProxy=static_cast<IEntityPhysicalProxy *>(GetEntity()->GetProxy(ENTITY_PROXY_PHYSICS));
							if (pPhysicsProxy)
							{
								GetEntity()->SetWorldTM(delta);
								pPhysicsProxy->AssignPhysicalEntity(pPhysicalEntity);
							}
						}

						if(m_pAnimatedCharacter)
							m_pAnimatedCharacter->ForceRefreshPhysicalColliderMode();
					}
				}
				else
				{
				//	Physicalize(wasFrozen?STANCE_PRONE:STANCE_NULL);

					if (profile==eAP_Spectator)
					{
						if (ICharacterInstance *pCharacter=GetEntity()->GetCharacter(0))
							pCharacter->GetISkeletonPose()->DestroyCharacterPhysics(1);

						if(m_pAnimatedCharacter)
						{
							m_pAnimatedCharacter->ForceRefreshPhysicalColliderMode();
							m_pAnimatedCharacter->RequestPhysicalColliderMode( eColliderMode_Spectator, eColliderModeLayer_Game, "Actor::SetAspectProfile");
						}
					}
					else if (profile==eAP_Alive)
					{
						if (m_currentPhysProfile==eAP_Spectator)
						{
							if(m_pAnimatedCharacter)
								m_pAnimatedCharacter->RequestPhysicalColliderMode( eColliderMode_Undefined, eColliderModeLayer_Game, "Actor::SetAspectProfile");
							
							if (IPhysicalEntity *pPhysics=GetEntity()->GetPhysics())
							{
								if (ICharacterInstance *pCharacter=GetEntity()->GetCharacter(0))
								{
									pCharacter->GetISkeletonPose()->DestroyCharacterPhysics(2);

									if (IPhysicalEntity *pCharPhysics=pCharacter->GetISkeletonPose()->GetCharacterPhysics())
									{
										pe_params_articulated_body body;
										body.pHost=pPhysics;
										pCharPhysics->SetParams(&body);
									}
								}
							}
						}
					}
				}
			}
			res=true;
			break;
		case eAP_Linked:
			// make sure we are alive, for when we transition from ragdoll to linked...
			//if (!GetEntity()->GetPhysics() || GetEntity()->GetPhysics()->GetType()!=PE_LIVING)
				//Physicalize();
			res=true;
			break;
		case eAP_Sleep:
			//RagDollize(true);
			res=true;
			break;
		case eAP_Ragdoll:
			// killed while sleeping?
			/*if (m_currentPhysProfile==eAP_Sleep) 
				GoLimp();
			else
				RagDollize(false);*/
			res=true;
			break;
		case eAP_Frozen:
			/*if (!GetEntity()->GetPhysics() || ((GetEntity()->GetPhysics()->GetType()!=PE_LIVING) && (GetEntity()->GetPhysics()->GetType()!=PE_ARTICULATED)))
				Physicalize();
			Freeze(true);*/
			res=true;
			break;
		}

		/*IPhysicalEntity *pPE=GetEntity()->GetPhysics();
		pe_player_dynamics pdyn;

		if (profile!=eAP_Frozen && wasFrozen)
		{
			Freeze(false);

			if (profile==eAP_Alive)
			{
				EStance stance;
				if (!TrySetStance(stance=STANCE_STAND))
					if (!TrySetStance(stance=STANCE_CROUCH))
					{
						pdyn.bActive=0;
						pPE->SetParams(&pdyn);

						if (!TrySetStance(stance=STANCE_PRONE))
							stance=STANCE_NULL;

						pdyn.bActive=1;
						pPE->SetParams(&pdyn);
					}
					
				if (stance!=STANCE_NULL)
				{
					m_stance=STANCE_NULL;
					m_desiredStance=stance;

					UpdateStance();
				}

				GetGameObject()->ChangedNetworkState(IPlayerInput::INPUT_ASPECT);
			}
		}

		if (res)
			ProfileChanged(profile);*/

		m_currentPhysProfile = profile;
	}

	return res;
}

void CMonoActor::InitLocalPlayer()
{
	GetGameObject()->SetUpdateSlotEnableCondition( this, 0, eUEC_WithoutAI );
	//g_pScriptSystem->GetIGameFramework()->GetIActorSystem()->SetLocalPlayerId(GetEntityId());
}

float CMonoActor::GetHealth() const
{
	IMonoObject *pResult = *m_pScript->GetPropertyValue("Health");
	float health = pResult->Unbox<float>();
	pResult->Release();

	return health;
}

void CMonoActor::SetHealth(float health)
{
	IMonoDomain *pDomain = m_pScript->GetClass()->GetAssembly()->GetDomain();

	m_pScript->SetPropertyValue("Health", pDomain->BoxAnyValue(MonoAnyValue(health)));
}

float CMonoActor::GetMaxHealth() const
{
	IMonoObject *pResult = *m_pScript->GetPropertyValue("MaxHealth");
	float health = pResult->Unbox<float>();
	pResult->Release();

	return health;
}

void CMonoActor::SetMaxHealth(float health)
{
	IMonoDomain *pDomain = m_pScript->GetClass()->GetAssembly()->GetDomain();

	m_pScript->SetPropertyValue("MaxHealth", pDomain->BoxAnyValue(MonoAnyValue(health)));
}

bool CMonoActor::NetSerialize( TSerialize ser, EEntityAspects aspect, uint8 profile, int pflags )
{
	if (aspect == eEA_Physics)
	{
		pe_type type = PE_NONE;
		switch (profile)
		{
		case eAP_NotPhysicalized:
			type = PE_NONE;
			break;
		case eAP_Spectator:
			type = PE_LIVING;
			break;
		case eAP_Alive:
			type = PE_LIVING;
			break;
		case eAP_Sleep:
			type = PE_ARTICULATED;
			break;
		case eAP_Frozen:
			type = PE_RIGID;
			break;
		case eAP_Ragdoll:
			type = PE_ARTICULATED;
			break;
		case eAP_Linked: 	//if actor is attached to a vehicle - don't serialize actor physics additionally
			return true;
			break;
		default:
			return false;
		}

		// TODO: remove this when craig fixes it in the network system
		if (profile==eAP_Spectator)
		{
			int x=0;	
			ser.Value("unused", x, 'skip');
		}
		else if (profile==eAP_Sleep)
		{
			int x=0;	
			ser.Value("unused1", x, 'skip');
			ser.Value("unused2", x, 'skip');
		}

		if (type == PE_NONE)
			return true;

		IEntityPhysicalProxy * pEPP = (IEntityPhysicalProxy *) GetEntity()->GetProxy(ENTITY_PROXY_PHYSICS);
		if (ser.IsWriting())
		{
			if (!pEPP || !pEPP->GetPhysicalEntity() || pEPP->GetPhysicalEntity()->GetType() != type)
			{
				gEnv->pPhysicalWorld->SerializeGarbageTypedSnapshot( ser, type, 0 );
				return true;
			}
		}
		else if (!pEPP)
		{
			return false;
		}

		// PLAYERPREDICTION
    	if(type!=PE_LIVING)
    	{
      		pEPP->SerializeTyped( ser, type, pflags );
    	}
		// ~PLAYERPREDICTION
	}

	ser.BeginGroup("ManagedActor");

	void *params[4];
	params[0] = &ser;
	params[1] = &aspect;
	params[2] = &profile;
	params[3] = &pflags;

	m_pScript->GetClass()->GetMethod("InternalNetSerialize", 4)->Invoke(m_pManagedObject, params);

	ser.EndGroup();

	return true;
}

void CMonoActor::FullSerialize(TSerialize ser)
{
	ser.BeginGroup("ManagedActor");

	IMonoArray *pArgs = CreateMonoArray(1);
	pArgs->InsertNativePointer(&ser);

	m_pScript->GetClass()->GetMethod("InternalFullSerialize", 1)->InvokeArray(m_pManagedObject, pArgs);
	pArgs->Release();

	ser.EndGroup();
}

void CMonoActor::PostSerialize()
{
	m_pScript->GetClass()->GetMethod("PostSerialize")->Invoke(m_pManagedObject);
}

IMPLEMENT_RMI(CMonoActor, SvScriptRMI)
{
	IMonoClass *pEntityClass = g_pScriptSystem->GetCryBraryAssembly()->GetClass("Entity");

	IMonoArray *pNetworkArgs = CreateMonoArray(3);
	pNetworkArgs->Insert(ToMonoString(params.methodName.c_str()));
	pNetworkArgs->InsertMonoObject(params.args);
	pNetworkArgs->Insert(params.targetId);

	pEntityClass->GetMethod("OnRemoteInvocation", 3)->InvokeArray(nullptr, pNetworkArgs);
	pNetworkArgs->Release();

	return true;
}

IMPLEMENT_RMI(CMonoActor, ClScriptRMI)
{
	IMonoClass *pEntityClass = g_pScriptSystem->GetCryBraryAssembly()->GetClass("Entity");

	IMonoArray *pNetworkArgs = CreateMonoArray(3);
	pNetworkArgs->Insert(ToMonoString(params.methodName.c_str()));
	pNetworkArgs->InsertMonoObject(params.args);
	pNetworkArgs->Insert(params.targetId);

	pEntityClass->GetMethod("OnRemoteInvocation", 3)->InvokeArray(nullptr, pNetworkArgs);
	pNetworkArgs->Release();

	return true;
}