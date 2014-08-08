////////////////////////////////////////////////////////////////////////////
//
//  Crytek Engine Source File.
//  Copyright (C), Crytek Studios, 2001-2011.
// -------------------------------------------------------------------------
//  File name:   UIMultiPlayer.h
//  Version:     v1.00
//  Created:     26/8/2011 by Paul Reindell.
//  Description: 
// -------------------------------------------------------------------------
//  History:
//
////////////////////////////////////////////////////////////////////////////
#ifndef __UIMultiPlayer_H__
#define __UIMultiPlayer_H__

#include "IUIGameEventSystem.h"
#include <IFlashUI.h>

class CUIMultiPlayer 
	: public IUIGameEventSystem
	, public IUIModule
{
public:
	CUIMultiPlayer();

	// IUIGameEventSystem
	UIEVENTSYSTEM( "UIMultiPlayer" );
	virtual void InitEventSystem();
	virtual void UnloadEventSystem();
	virtual void LoadProfile( IPlayerProfile* pProfile );
	virtual void SaveProfile( IPlayerProfile* pProfile ) const;

	// IUIModule
	virtual void Reset();
	// ~IUIModule

	// UI functions
	void EnteredGame();
	void PlayerJoined(EntityId playerid, const string& name);
	void UpdateScoreBoardItem(EntityId playerid, const string& name, int kills, int deaths);
	void PlayerLeft(EntityId playerid, const string& name);
	void PlayerKilled(EntityId playerid, EntityId shooterid);
	void PlayerRenamed(EntityId playerid, const string& newName);
	void OnChatRecieved(EntityId senderId, int teamFaction, const char* message);

private:
	// UI events
	void RequestPlayers();
	void GetPlayerName();
	void ConnectToServer( const string& server );
	void GetServerName();
	void OnSendChatMessage( const string& message );

	string GetPlayerNameById( EntityId playerid );

private:
	enum EUIEvent
	{
		eUIE_EnteredGame,
		eUIE_PlayerJoined,
		eUIE_UpdateScoreBoardItem,
		eUIE_PlayerLeft,
		eUIE_PlayerKilled,
		eUIE_PlayerRenamed,
		eUIE_SendName,
		eUIE_SendServer,
		eUIE_ChatMsgReceived,
	};

	SUIEventReceiverDispatcher<CUIMultiPlayer> m_eventDispatcher;
	SUIEventSenderDispatcher<EUIEvent> m_eventSender;
	IUIEventSystem* m_pUIEvents;
	IUIEventSystem* m_pUIFunctions;

	struct SPlayerInfo
	{
		SPlayerInfo() : name("<UNDEFINED>"), teamId(-1) {}

		string name;
		int teamId; // todo: set the team id once we support teams in SDK
	};
	typedef std::map<EntityId, SPlayerInfo> TPlayers;
	TPlayers m_Players;

	string m_LocalPlayerName;
	string m_ServerName;
};


#endif // __UIMultiPlayer_H__
