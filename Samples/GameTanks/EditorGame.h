/*************************************************************************
  Crytek Source File.
  Copyright (C), Crytek Studios, 2001-2004.
 -------------------------------------------------------------------------
  $Id$
  $DateTime$
  Description: Implements the Editor->Game communication interface.
  
 -------------------------------------------------------------------------
  History:
  - 30:8:2004   11:17 : Created by Marcio Martins

*************************************************************************/
#ifndef __EDITORGAME_H__
#define __EDITORGAME_H__

#if _MSC_VER > 1000
# pragma once
#endif


#include <IGameRef.h>
#include <IEditorGame.h>

struct IGameStartup;

class CEditorGame :
	public IEditorGame
{
public:
	CEditorGame();
	virtual ~CEditorGame();

	VIRTUAL bool Init(ISystem *pSystem,IGameToEditorInterface *pGameToEditorInterface);
	VIRTUAL int Update(bool haveFocus, unsigned int updateFlags);
	VIRTUAL void Shutdown();
	VIRTUAL bool SetGameMode(bool bGameMode);
	VIRTUAL IEntity * GetPlayer();
	VIRTUAL void SetPlayerPosAng(Vec3 pos,Vec3 viewDir);
	VIRTUAL void HidePlayer(bool bHide);
	VIRTUAL void OnBeforeLevelLoad();
	VIRTUAL void OnAfterLevelLoad(const char *levelName, const char *levelFolder);
	VIRTUAL void OnCloseLevel() {}
	VIRTUAL bool BuildEntitySerializationList(XmlNodeRef output) { return true; }
	VIRTUAL bool GetAdditionalMinimapData(XmlNodeRef output) { return true; }

	VIRTUAL IFlowSystem * GetIFlowSystem();
	VIRTUAL IGameTokenSystem* GetIGameTokenSystem();
	VIRTUAL IEquipmentSystemInterface* GetIEquipmentSystemInterface() { return nullptr; }

	VIRTUAL bool SupportsMultiplayerGameRules() { return true; }
	VIRTUAL void ToggleMultiplayerGameRules();

	VIRTUAL void RegisterTelemetryTimelineRenderers(Telemetry::ITelemetryRepository* pRepository);

	virtual void OnDisplayRenderUpdated( bool displayHelpers ) {}

private:
	void InitUIEnums(IGameToEditorInterface* pGTE);
	void InitGlobalFileEnums(IGameToEditorInterface* pGTE);
	void InitActionEnums(IGameToEditorInterface* pGTE);
	bool ConfigureNetContext( bool on );
	static void OnChangeEditorMode( ICVar * );
	void EnablePlayer(bool bPlayer);
	static void ResetClient(IConsoleCmdArgs*);
	static void SetGameRules();

	IGameRef			m_pGame;
	IGameStartup	*m_pGameStartup;

	bool          m_bEnabled;
	bool          m_bGameMode;
	bool          m_bPlayer;
	bool          m_bUsingMultiplayerGameRules;

	static ICVar  *s_pEditorGameMode;
	static CEditorGame * s_pEditorGame;
};


#endif //__EDITORGAME_H__
