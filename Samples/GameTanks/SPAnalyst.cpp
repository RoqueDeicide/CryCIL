////////////////////////////////////////////////////////////////////////////
//
//  Crytek Engine Source File.
//  Copyright (C), Crytek Studios, 2007.
// -------------------------------------------------------------------------
//  File name:   SPAnalyst.cpp
//  Version:     v1.00
//  Created:     07/07/2006 by AlexL
//  Compilers:   Visual Studio.NET
//  Description: SPAnalyst to track/serialize some SP stats
// -------------------------------------------------------------------------
//  History:
//
////////////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "SPAnalyst.h"

#include <ISerialize.h>
#include <Game.h>
#include <ISaveGame.h>

CSPAnalyst::CSPAnalyst() : m_bEnabled(false), m_bChainLoad(false)
{
	IGameFramework* pGF = g_pGame->GetIGameFramework();
	pGF->GetILevelSystem()->AddListener(this);
	pGF->RegisterListener(this, "CSPAnalyst", FRAMEWORKLISTENERPRIORITY_GAME);
}

CSPAnalyst::~CSPAnalyst()
{
	IGameFramework* pGF = g_pGame->GetIGameFramework();
	if (m_bEnabled)
		pGF->GetIGameplayRecorder()->UnregisterListener(this);
	pGF->GetILevelSystem()->RemoveListener(this);
	pGF->UnregisterListener(this);
}

void CSPAnalyst::Enable(bool bEnable)
{
	if (m_bEnabled != bEnable)
	{
		if (bEnable)
		{
			Reset();
			g_pGame->GetIGameFramework()->GetIGameplayRecorder()->RegisterListener(this);
		}
		else
			g_pGame->GetIGameFramework()->GetIGameplayRecorder()->UnregisterListener(this);
		m_bEnabled = bEnable;
	}
}

bool CSPAnalyst::IsPlayer(EntityId entityId) const
{
	// fast path, if already known
	if (m_gameAnalysis.player.entityId == entityId)
		return true;

	// slow path, only first time
	if (IActor *pActor=g_pGame->GetIGameFramework()->GetIActorSystem()->GetActor(entityId))
		return pActor->IsPlayer();
	return false;
}

void CSPAnalyst::NewPlayer(IEntity* pEntity)
{
	m_gameAnalysis.player = PlayerAnalysis(pEntity->GetId());
	m_gameAnalysis.player.name = pEntity->GetName();
}

CSPAnalyst::PlayerAnalysis* CSPAnalyst::GetPlayer(EntityId entityId)
{
	if (m_gameAnalysis.player.entityId == entityId)
		return &m_gameAnalysis.player;
	return 0;
}

void CSPAnalyst::OnGameplayEvent(IEntity *pEntity, const GameplayEvent &event)
{
	if(!gEnv->bServer)
		return;

	EntityId entityId = pEntity ? pEntity->GetId() : 0;

	if (entityId && IsPlayer(entityId))
		ProcessPlayerEvent(pEntity, event);

	switch (event.event)
	{
	case eGE_GameStarted:
		{
			const float t = gEnv->pTimer->GetCurrTime();
			// CryLogAlways("[CSPAnalyst]: Game Started at %f", t);
			// if the levelStartTime was serialized, don't touch it
			if (m_gameAnalysis.levelStartTime == 0.0f)
				m_gameAnalysis.levelStartTime = gEnv->pTimer->GetFrameStartTime();
			// if the gameStartTime was serialized, don't touch it
			if (m_gameAnalysis.gameStartTime == 0.0f)
				m_gameAnalysis.gameStartTime = m_gameAnalysis.levelStartTime;
		}
		// called in SP as well
		break;
	case eGE_GameEnd:
		{
			int a = 0;(void)a;
		}
		// not called in SP yet
		break;
	default:
		break;
	}
}

void CSPAnalyst::ProcessPlayerEvent(IEntity* pEntity, const GameplayEvent& event)
{
	assert (pEntity != 0);
	const EntityId entityId = pEntity->GetId();

	switch (event.event)
	{
	case eGE_Connected:
		{
			NewPlayer(pEntity);
			const float t = gEnv->pTimer->GetCurrTime();
			// CryLogAlways("[CSPAnalyst]: Connected at %f", t);
		}
		break;
	case eGE_Kill:
		if (PlayerAnalysis* pA = GetPlayer(entityId))
			++pA->kills;
		break;
	case eGE_Death:
		if (PlayerAnalysis* pA = GetPlayer(entityId))
			++pA->deaths;
	default:
		break;
	}
}

void CSPAnalyst::PlayerAnalysis::Serialize(TSerialize ser)
{
	ser.BeginGroup("PA");
	ser.Value("kills", kills);
	ser.Value("deaths", deaths);
	ser.EndGroup();
}

void CSPAnalyst::Serialize(TSerialize ser)
{
	if (ser.BeginOptionalGroup("SPAnalyst", true))
	{
		ser.Value("levelStartTime", m_gameAnalysis.levelStartTime);
		ser.Value("gameStartTime", m_gameAnalysis.gameStartTime);
		m_gameAnalysis.player.Serialize(ser);
		ser.EndGroup();
	}
}

void CSPAnalyst::Reset()
{
	m_gameAnalysis = GameAnalysis();
}

void CSPAnalyst::OnLoadingStart(ILevelInfo *pLevelInfo)
{
	if (pLevelInfo == 0)
		return;

	// in any case, reset the level start time, so it will either be restored from SaveGame or 
	// set in eGE_GameStarted event handling
	m_gameAnalysis.levelStartTime = CTimeValue(0.0f);
	m_bChainLoad = false;
}

void CSPAnalyst::OnSaveGame(ISaveGame* pSaveGame)
{
	if (m_bEnabled == false || pSaveGame == 0)
		return;

	CTimeValue now = gEnv->pTimer->GetFrameStartTime();

	pSaveGame->AddMetadata("sp_kills", m_gameAnalysis.player.kills);
	pSaveGame->AddMetadata("sp_deaths", m_gameAnalysis.player.deaths);
	pSaveGame->AddMetadata("sp_levelPlayTime", (int)((now-m_gameAnalysis.levelStartTime).GetSeconds()));
	pSaveGame->AddMetadata("sp_gamePlayTime", (int)((now-m_gameAnalysis.gameStartTime).GetSeconds()));
}

void CSPAnalyst::OnLevelEnd(const char *nextLevel)
{
	m_bChainLoad = true;
}