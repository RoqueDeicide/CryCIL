/*************************************************************************
	Crytek Source File.
	Copyright (C), Crytek Studios, 2001-2004.
	-------------------------------------------------------------------------
	$Id$
	$DateTime$

	-------------------------------------------------------------------------
	History:
		- 7:2:2006   15:38 : Created by Marcio Martins

*************************************************************************/
#include "StdAfx.h"
#include "GameRules.h"
#include "Game.h"
#include "GameCVars.h"

#include <IAIObject.h>

#include "IVehicleSystem.h"
#include "IItemSystem.h"

#include "IUIDraw.h"
#include "IMovieSystem.h"

#include "GameActions.h"
#include "SPAnalyst.h"
#include "IWorldQuery.h"

#include <StlUtils.h>
#include <StringUtils.h>

#include <IBreakableManager.h>

#include <ICryScriptInstance.h>

//------------------------------------------------------------------------
CGameRules::CGameRules()
	: m_pScript(nullptr)
{
}

//------------------------------------------------------------------------
CGameRules::~CGameRules()
{
	if (gEnv->pGame->GetIGameFramework())
	{
		if (gEnv->pGame->GetIGameFramework()->GetIGameRulesSystem())
			gEnv->pGame->GetIGameFramework()->GetIGameRulesSystem()->SetCurrentGameRules(0);
	}
}

//------------------------------------------------------------------------
bool CGameRules::Init( IGameObject * pGameObject )
{
	SetGameObject(pGameObject);

	if (!GetGameObject()->BindToNetwork())
		return false;

	gEnv->pGame->GetIGameFramework()->GetIGameRulesSystem()->SetCurrentGameRules(this);

	m_pScript = GetMonoScriptSystem()->InstantiateScript(GetEntity()->GetClass()->GetName(), eScriptFlag_GameRules);

	return true;
}

bool CGameRules::ShouldKeepClient(int channelId, EDisconnectionCause cause, const char *desc) const
{
	return (!strcmp("timeout", desc) || cause==eDC_Timeout);
}

//------------------------------------------------------------------------
void CGameRules::OnConnect(struct INetChannel *pNetChannel)
{
	m_pScript->CallMethod("OnConnect");
}


//------------------------------------------------------------------------
void CGameRules::OnDisconnect(EDisconnectionCause cause, const char *desc)
{
	m_pScript->CallMethod("OnDisconnect", cause, desc);
}

//------------------------------------------------------------------------
bool CGameRules::OnClientConnect(int channelId, bool isReset)
{
	const char *playerName;
	if (gEnv->bServer && gEnv->bMultiplayer)
	{
		if (INetChannel *pNetChannel = gEnv->pGame->GetIGameFramework()->GetNetChannel(channelId))
			playerName = pNetChannel->GetNickname();
	}
	else
		playerName = "Dude";

	return m_pScript->CallMethod("OnClientConnect", channelId, isReset, playerName) != 0;
}

//------------------------------------------------------------------------
void CGameRules::OnClientDisconnect(int channelId, EDisconnectionCause cause, const char *desc, bool keepClient)
{
	m_pScript->CallMethod("OnClientDisconnect", channelId);
}

//------------------------------------------------------------------------
bool CGameRules::OnClientEnteredGame(int channelId, bool isReset)
{ 
	IActor *pActor = gEnv->pGame->GetIGameFramework()->GetIActorSystem()->GetActorByChannelId(channelId);
	if(pActor == nullptr)
		return false;
	
	m_pScript->CallMethod("OnClientEnteredGame", channelId, pActor->GetEntityId(), isReset);

	// Need to update the time of day serialization chunk so that the new client can start at the right point
	// Note: Since we don't generally have a dynamic time of day, this will likely only effect clients
	// rejoining after a host migration since they won't be loading the value from the level
	CHANGED_NETWORK_STATE(this, eEA_GameServerDynamic);
	CHANGED_NETWORK_STATE(this, eEA_GameServerStatic);
	
	return true;
}

//------------------------------------------------------------------------
void CGameRules::OnEditorReset(bool enterGameMode)
{
	if(m_pScript != nullptr)
		m_pScript->CallMethod("OnEditorReset", enterGameMode);
}

//------------------------------------------------------------------------
bool CGameRules::OnCollision(const SGameCollision& event)
{
	FUNCTION_PROFILER(GetISystem(), PROFILE_GAME);
	// currently this function only calls server functions
	// prevent unnecessary script callbacks on the client
	if (!gEnv->bServer || IsDemoPlayback())
		return true; 

	// filter out self-collisions
	if (event.pSrcEntity == event.pTrgEntity)
		return true;

	// collisions involving partId<-1 are to be ignored by game's damage calculations
	// usually created articially to make stuff break. See CMelee::Impulse
	if (event.pCollision->partid[0]<-1||event.pCollision->partid[1]<-1)
		return true;

	m_pScript->CallMethod("OnCollision", event.pSrcEntity ? event.pSrcEntity->GetId() : 0, event.pTrgEntity ? event.pTrgEntity->GetId() : 0, event.pCollision->pt, event.pCollision->vloc[0].GetNormalizedSafe(), event.pCollision->idmat[0], event.pCollision->n);

	return true;
}

//------------------------------------------------------------------------
void CGameRules::CreateEntityRespawnData(EntityId entityId)
{
	if (!gEnv->bServer || gEnv->pGame->GetIGameFramework()->IsEditing())
		return;

	IEntity *pEntity = gEnv->pEntitySystem->GetEntity(entityId);
	if (!pEntity)
		return;

	SEntityRespawnData respawn;
	respawn.position = pEntity->GetWorldPos();
	respawn.rotation = pEntity->GetWorldRotation();
	respawn.scale = pEntity->GetScale();
	respawn.flags = pEntity->GetFlags() & ~ENTITY_FLAG_UNREMOVABLE;
	respawn.pClass = pEntity->GetClass();
#ifdef _DEBUG
	respawn.name = pEntity->GetName();
#endif
	
	IScriptTable *pScriptTable = pEntity->GetScriptTable();

	if (pScriptTable)
		pScriptTable->GetValue("Properties", respawn.properties);

	m_respawndata[entityId] = respawn;
}

//------------------------------------------------------------------------
bool CGameRules::HasEntityRespawnData(EntityId entityId) const
{
	return m_respawndata.find(entityId)!=m_respawndata.end();
}

//------------------------------------------------------------------------
void CGameRules::ScheduleEntityRespawn(EntityId entityId, bool unique, float timer)
{
	if (!gEnv->bServer || gEnv->pGame->GetIGameFramework()->IsEditing())
		return;

	IEntity *pEntity = gEnv->pEntitySystem->GetEntity(entityId);
	if (!pEntity)
		return;

	SEntityRespawn respawn;
	respawn.timer = timer;
	respawn.unique = unique;

	m_respawns[entityId] = respawn;
}


//------------------------------------------------------------------------
void CGameRules::AbortEntityRespawn(EntityId entityId, bool destroyData)
{
	TEntityRespawnMap::iterator it=m_respawns.find(entityId);
	if (it!=m_respawns.end())
		m_respawns.erase(it);

	if (destroyData)
	{
		TEntityRespawnDataMap::iterator dit=m_respawndata.find(entityId);
		if (dit!=m_respawndata.end())
			m_respawndata.erase(dit);
	}
}

//------------------------------------------------------------------------
void CGameRules::ScheduleEntityRemoval(EntityId entityId, float timer, bool visibility)
{
	if (!gEnv->bServer || gEnv->pGame->GetIGameFramework()->IsEditing())
		return;

	IEntity *pEntity = gEnv->pEntitySystem->GetEntity(entityId);
	if (!pEntity)
		return;

	SEntityRemovalData removal;
	removal.time = timer;
	removal.timer = timer;
	removal.visibility = visibility;

	m_removals.insert(TEntityRemovalMap::value_type(entityId, removal));
}

//------------------------------------------------------------------------
void CGameRules::AbortEntityRemoval(EntityId entityId)
{
	TEntityRemovalMap::iterator it=m_removals.find(entityId);
	if (it!=m_removals.end())
		m_removals.erase(it);
}