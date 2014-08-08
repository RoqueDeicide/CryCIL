/*************************************************************************
  Crytek Source File.
  Copyright (C), Crytek Studios, 2001-2004.
 -------------------------------------------------------------------------
  $Id$
  $DateTime$
  Description: 
  
 -------------------------------------------------------------------------
  History:
  - 3:8:2004   11:23 : Created by Marcio Martins

*************************************************************************/
#ifndef __GAME_H__
#define __GAME_H__

#if _MSC_VER > 1000
# pragma once
#endif

#include <IGame.h>
#include <IGameFramework.h>
#include <IGameObjectSystem.h>
#include <IGameObject.h>
#include <IActorSystem.h>
#include <StlUtils.h>
#include "RayCastQueue.h"
#include <IntersectionTestQueue.h>
#include "ILevelSystem.h"

#define GAME_NAME				"Tanks"
#define GAME_LONGNAME		"CryMono_Tanks"

#define ACTIONMAP_DEFAULT_PROFILE	"libs/config/defaultProfile.xml"

struct ISystem;
struct IConsole;


class CGameMechanismManager;

struct IActionMap;
struct IActionFilter;
class  CGameActions;
class CGameRules;
struct SCVars;
class CSPAnalyst;

// when you add stuff here, also update in CGame::RegisterGameObjectEvents
enum ECryGameEvent
{
	eCGE_PreFreeze = eGFE_Last,	// this is really bad and must be fixed
	eCGE_PreShatter,
	eCGE_PostFreeze,
	eCGE_PostShatter,
	eCGE_OnShoot,
	eCGE_Recoil, 
	eCGE_BeginReloadLoop,
	eCGE_EndReloadLoop,
	eCGE_ActorRevive,
	eCGE_VehicleDestroyed,
	eCGE_TurnRagdoll,
	eCGE_EnableFallAndPlay,
	eCGE_DisableFallAndPlay,
	eCGE_VehicleTransitionEnter,
	eCGE_VehicleTransitionExit,
	eCGE_TextArea,
	eCGE_InitiateAutoDestruction,
	eCGE_Event_Collapsing,
	eCGE_Event_Collapsed,
	eCGE_MultiplayerChatMessage,
	eCGE_ResetMovementController,
	eCGE_AnimateHands,
	eCGE_Ragdoll,
	eCGE_EnablePhysicalCollider,
	eCGE_DisablePhysicalCollider,
	eCGE_RebindAnimGraphInputs,
	eCGE_OpenParachute,
  eCGE_Turret_LockedTarget,
  eCGE_Turret_LostTarget,
	eCGE_ReactionEnd
};

enum EInviteAcceptedState
{
	eIAS_None = 0,
	eIAS_Init,										// initialisation
	eIAS_StartAcceptInvite,				// begin the process
	eIAS_InitProfile,							// progress to profile loading screen, user might not have created profile yet
	eIAS_WaitForInitProfile,			// wait for profile creation to be finished
	eIAS_WaitForLoadToFinish,			// waiting for loading to finish
	eIAS_DisconnectGame,					// disconnect user from game
	eIAS_DisconnectLobby,					// disconnect user from lobby session
	eIAS_WaitForSessionDelete,		// waiting for game session to be deleted
	eIAS_ConfirmInvite,
	eIAS_WaitForInviteConfirmation,
	eIAS_InitSinglePlayer,
	eIAS_WaitForInitSinglePlayer,
	eIAS_WaitForSplashScreen,			// return user to splash screen
	eIAS_WaitForValidUser,				// PS3 - need user to select controller
	eIAS_InitMultiplayer,					// init the multiplayer gamemode
	eIAS_WaitForInitMultiplayer,	// wait for multiplayer game mode to be initialised
	eIAS_InitOnline,							// init online functionality
	eIAS_WaitForInitOnline,				// wait for online mode to be initialised
	eIAS_WaitForSquadManagerEnabled,
	eIAS_Accept,									// accept the invite
	eIAS_Error,										// we recieved an invite that had an error attached to it
};

static const int GLOBAL_SERVER_IP_KEY						=	1000;
static const int GLOBAL_SERVER_PUBLIC_PORT_KEY	= 1001;
static const int GLOBAL_SERVER_NAME_KEY					=	1002;

#define CryInviteID CrySessionID
#define CryInvalidInvite CrySessionInvalidID

class CGame :
  public IGame, public IGameFrameworkListener, public ISystemEventListener
{
public:
  typedef bool (*BlockingConditionFunction)();
  typedef RayCastQueue<41> GlobalRayCaster;
	typedef IntersectionTestQueue<43> GlobalIntersectionTester;

	enum EHostMigrationState
	{
		eHMS_NotMigrating,
		eHMS_WaitingForPlayers,
		eHMS_Resuming,
	};

	enum ERichPresenceType
	{
		eRPT_String = 0,
		eRPT_Param1,
		eRPT_Param2,
		eRPT_Max,
	};

public:
	CGame();
	VIRTUAL ~CGame();

	// IGame
	VIRTUAL bool  Init(IGameFramework *pFramework);
	VIRTUAL bool  CompleteInit();
	VIRTUAL void  Shutdown();
	VIRTUAL int   Update(bool haveFocus, unsigned int updateFlags);
	VIRTUAL void  ConfigureGameChannel(bool isServer, IProtocolBuilder *pBuilder) {}
	VIRTUAL void  EditorResetGame(bool bStart);
	VIRTUAL void  PlayerIdSet(EntityId playerId);
	VIRTUAL string  InitMapReloading();
	VIRTUAL bool IsReloading() { return m_bReload; }
	VIRTUAL IGameFramework *GetIGameFramework() { return m_pFramework; }

	VIRTUAL const char *GetLongName();
	VIRTUAL const char *GetName();

	VIRTUAL void GetMemoryStatistics(ICrySizer * s);

	VIRTUAL void OnClearPlayerIds();
	//auto-generated save game file name
	VIRTUAL IGame::TSaveGameName CreateSaveGameName();
	//level names were renamed without changing the file/directory
	VIRTUAL const char* GetMappedLevelName(const char *levelName) const;
	
	virtual IAntiCheatManager * GetAntiCheatManager() { return nullptr; }

	VIRTUAL const bool DoInitialSavegame() const { return true; }

	VIRTUAL uint32 AddGameWarning(const char* stringId, const char* paramMessage, IGameWarningsListener* pListener = NULL);
	virtual void OnRenderScene(const SRenderingPassInfo &passInfo) {}
	VIRTUAL void RenderGameWarnings() {};
	VIRTUAL void RemoveGameWarning(const char* stringId);

	VIRTUAL bool GameEndLevel(const char* stringId) { return false; }

	VIRTUAL const uint8* GetDRMKey();
	VIRTUAL const char* GetDRMFileList();

	VIRTUAL IGameStateRecorder* CreateGameStateRecorder(IGameplayListener* pL) { return nullptr; }

	virtual void FullSerialize(TSerialize ser) {}
	virtual void PostSerialize();

	virtual IGame::ExportFilesInfo ExportLevelData( const char* levelName, const char* missionName ) const { return ExportFilesInfo("", 0); }

	// Description
	// Interface hook to load all game exported data when the level is loaded
	virtual void   LoadExportedLevelData( const char* levelName, const char* missionName ) {}

	VIRTUAL void RegisterGameFlowNodes();

	virtual IGamePhysicsSettings* GetIGamePhysicsSettings() { return nullptr; }
	// ~IGame

  // IGameFrameworkListener
  VIRTUAL void OnPostUpdate(float fDeltaTime);
  VIRTUAL void OnSaveGame(ISaveGame* pSaveGame) {}
  VIRTUAL void OnLoadGame(ILoadGame* pLoadGame) {}
	VIRTUAL void OnLevelEnd(const char* nextLevel) {};
  VIRTUAL void OnActionEvent(const SActionEvent& event);
  // ~IGameFrameworkListener

  void BlockingProcess(BlockingConditionFunction f);

	void SetExclusiveControllerFromPreviousInput();
	void SetPreviousExclusiveControllerDeviceIndex(unsigned int idx) { m_previousInputControllerDeviceIndex = idx; }
	void RemoveExclusiveController();
	bool HasExclusiveControllerIndex() const { return m_hasExclusiveController; }
	bool IsExclusiveControllerConnected() const { return m_bExclusiveControllerConnected; }

	// ISystemEventListener
	virtual void OnSystemEvent(ESystemEvent event, UINT_PTR wparam, UINT_PTR lparam);
	// ~ISystemEventListener

	CGameActions&	Actions() const {	return *m_pGameActions;	};

	CGameRules *GetGameRules() const;

  ILINE GlobalRayCaster& GetRayCaster() { assert(m_pRayCaster); return *m_pRayCaster; }
	GlobalIntersectionTester& GetIntersectionTester() { assert(m_pIntersectionTester); return *m_pIntersectionTester; }

#if IMPLEMENT_PC_BLADES
	CGameFriendsManager* GetGameFriendsManager() { return m_gameFriendMgr; }
	CGameServerLists*	GetGameServerLists() { return m_pGameServerLists; }
#endif //IMPLEMENT_PC_BLADES

	IPlayerProfileManager* GetPlayerProfileManager() { return m_pPlayerProfileManager; }

	bool IsGameActive() const;
	void ClearGameSessionHandler();

	void SetUserRegion(int region) { m_cachedUserRegion = region; }
	int GetUserRegion(void) const { return m_cachedUserRegion; }

	uint32 GetRandomNumber();
	
	CSPAnalyst* GetSPAnalyst() const { return m_pSPAnalyst; }

	const string& GetLastSaveGame(string &levelName);
	const string& GetLastSaveGame() { string tmp; return GetLastSaveGame(tmp); }
	bool LoadLastSave();

  ILINE SCVars *GetCVars() {return m_pCVars;}
	static void DumpMemInfo(const char* format, ...) PRINTF_PARAMS(1, 2);

	VIRTUAL void LoadActionMaps(const char* filename);

	EntityId GetClientActorId() const { return m_clientActorId; }

protected:
	VIRTUAL void CheckReloadLevel();

	// These funcs live in GameCVars.cpp
	VIRTUAL void RegisterConsoleVars();
	VIRTUAL void RegisterConsoleCommands();
	VIRTUAL void UnregisterConsoleCommands();

	VIRTUAL void RegisterGameObjectEvents();

	// marcok: this is bad and evil ... should be removed soon
	static void CmdRestartGame(IConsoleCmdArgs *pArgs);

	static void CmdSay(IConsoleCmdArgs *pArgs);
	static void CmdLoadActionmap(IConsoleCmdArgs *pArgs);
  static void CmdReloadGameRules(IConsoleCmdArgs *pArgs);
  static void CmdNextLevel(IConsoleCmdArgs* pArgs);

	CRndGen m_randomGenerator;
	IGameFramework			*m_pFramework;
	IConsole						*m_pConsole;

	bool								m_bReload;

	IActionMap					*m_pDebugAM;
	IActionMap					*m_pDefaultAM;
	IActionMap					*m_pMultiplayerAM;
	CGameActions				*m_pGameActions;	
	IPlayerProfileManager* m_pPlayerProfileManager;

	CSPAnalyst          *m_pSPAnalyst;
	bool								m_inDevMode;

	EntityId						m_clientActorId;

	SCVars*	m_pCVars;
	string                 m_lastSaveGame;

  GlobalRayCaster* m_pRayCaster;
	GlobalIntersectionTester* m_pIntersectionTester;

	typedef std::map<string, string, stl::less_stricmp<string> > TLevelMapMap;
	TLevelMapMap m_mapNames;

	CGameMechanismManager* m_pGameMechanismManager;

	int m_cachedUserRegion;

#if IMPLEMENT_PC_BLADES
	// Game side friends manager
	CGameFriendsManager* m_gameFriendMgr;
	CGameServerLists* m_pGameServerLists;
#endif

	bool				 m_hasExclusiveController;
	bool				 m_bExclusiveControllerConnected;
	bool				 m_previousPausedGameState;

	unsigned int m_previousInputControllerDeviceIndex;
};

extern CGame *g_pGame;

#define SAFE_HARDWARE_MOUSE_FUNC(func)\
	if(gEnv->pHardwareMouse)\
		gEnv->pHardwareMouse->func

#endif //__GAME_H__
