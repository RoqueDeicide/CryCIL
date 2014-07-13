/*************************************************************************
  Crytek Source File.
  Copyright (C), Crytek Studios, 2001-2004.
 -------------------------------------------------------------------------
  $Id$
  $DateTime$
  
 -------------------------------------------------------------------------
  History:
  - 3:8:2004   11:26 : Created by Marcio Martins
  - 17:8:2005        : Modified - NickH: Factory registration moved to GameFactory.cpp

*************************************************************************/
#include "StdAfx.h"
#include "Game.h"
#include "GameCVars.h"
#include "GameActions.h"

#include "GameRules.h"

#include <ICryPak.h>
#include <CryPath.h>
#include <IActionMapManager.h>
#include <IViewSystem.h>
#include <ILevelSystem.h>
#include <IItemSystem.h>
#include <IVehicleSystem.h>
#include <IMovieSystem.h>
#include <IPlayerProfiles.h>
#include <IPlatformOS.h>

#include "GameFactory.h"

#include "Nodes/G2FlowBaseNode.h"

#include "SPAnalyst.h"

#include "ISaveGame.h"
#include "ILoadGame.h"
#include "CryPath.h"
#include <IPathfinder.h>

#include <IMonoScriptSystem.h>

#include "HUD/UIManager.h"
// #include "HUD/UIWarnings.h"
#include "IMaterialEffects.h"


#include "GameMechanismManager/GameMechanismManager.h"
#include "ICheckPointSystem.h"

#define GAME_DEBUG_MEM  // debug memory usage
#undef  GAME_DEBUG_MEM

#define SDK_GUID "{CDCB9B7A-7390-45AA-BF2F-3A7C7933DCF3}"

//FIXME: really horrible. Remove ASAP
int OnImpulse( const EventPhys *pEvent ) 
{ 
	//return 1;
	return 0;
}

#include "ICryLobbyUI.h"

#define GAME_DEBUG_MEM  // debug memory usage
#undef  GAME_DEBUG_MEM

// Needed for the Game02 specific flow node
CG2AutoRegFlowNodeBase *CG2AutoRegFlowNodeBase::m_pFirst=0;
CG2AutoRegFlowNodeBase *CG2AutoRegFlowNodeBase::m_pLast=0;

CGame *g_pGame = 0;
SCVars *g_pGameCVars = 0;
CGameActions *g_pGameActions = 0;

CGame::CGame()
: m_pFramework(0),
	m_pConsole(0),
	m_pPlayerProfileManager(0),
	m_clientActorId(-1),
	m_pSPAnalyst(0),
	m_pRayCaster(0),
	m_pIntersectionTester(NULL),
	m_cachedUserRegion(-1),
	m_randomGenerator(gEnv->bNoRandomSeed?0:(uint32)gEnv->pTimer->GetAsyncTime().GetValue())
{
	m_pCVars = new SCVars();
	g_pGameCVars = m_pCVars;
	m_pGameActions = new CGameActions();
	g_pGameActions = m_pGameActions;
	g_pGame = this;
	m_bReload = false;
	m_inDevMode = false;

	m_pGameMechanismManager = new CGameMechanismManager();

	m_pDefaultAM = 0;
	m_pMultiplayerAM = 0;

	GetISystem()->SetIGame( this );
}

CGame::~CGame()
{
	m_pFramework->EndGameContext();
	m_pFramework->UnregisterListener(this);
	gEnv->pSystem->GetISystemEventDispatcher()->RemoveListener(this);
	//SAFE_DELETE(m_pCameraManager);
	SAFE_DELETE(m_pSPAnalyst);
	SAFE_DELETE(m_pGameMechanismManager);
	SAFE_DELETE(m_pCVars);
	ClearGameSessionHandler(); // make sure this is cleared before the gamePointer is gone
	g_pGame = 0;
	g_pGameCVars = 0;
	g_pGameActions = 0;
	SAFE_DELETE(m_pRayCaster);
	SAFE_DELETE(m_pGameActions);
	SAFE_DELETE(m_pIntersectionTester);
	gEnv->pGame = 0;
}

bool CGame::Init(IGameFramework *pFramework)
{
  LOADING_TIME_PROFILE_SECTION(GetISystem());

#ifdef GAME_DEBUG_MEM
	DumpMemInfo("CGame::Init start");
#endif

	m_pFramework = pFramework;
	assert(m_pFramework);

	m_pConsole = gEnv->pConsole;

	RegisterConsoleVars();
	RegisterConsoleCommands();
	RegisterGameObjectEvents();

	LoadActionMaps( ACTIONMAP_DEFAULT_PROFILE );

	//load user levelnames for ingame text and savegames
	XmlNodeRef lnames = GetISystem()->LoadXmlFromFile(PathUtil::GetGameFolder() + "/Scripts/GameRules/LevelNames.xml");
	if(lnames)
	{
		int num = lnames->getNumAttributes();
		const char *nameA, *nameB;
		for(int n = 0; n < num; ++n)
		{
			lnames->getAttributeByIndex(n, &nameA, &nameB);
			m_mapNames[string(nameA)] = string(nameB);
		}
	}

	// Register all the games factory classes e.g. maps "Player" to CPlayer
	InitGameFactory(m_pFramework);

	//FIXME: horrible, remove this ASAP
	//gEnv->pPhysicalWorld->AddEventClient( EventPhysImpulse::id,OnImpulse,0 );  

	m_pSPAnalyst = new CSPAnalyst();
 
	gEnv->pConsole->CreateKeyBind("f12", "r_getscreenshot 2");

	//Ivo: initialites the Crysis conversion file.
	//this is a conversion solution for the Crysis game DLL. Other projects don't need it.
	// No need anymore
	//gEnv->pCharacterManager->LoadCharacterConversionFile("Objects/CrysisCharacterConversion.ccc");

	// set game GUID
	m_pFramework->SetGameGUID(SDK_GUID);
	gEnv->pSystem->GetPlatformOS()->UserDoSignIn(0); // sign in the default user

	gEnv->pSystem->GetISystemEventDispatcher()->RegisterListener(this);

	// TEMP
	// Load the action map beforehand (see above)
	// afterwards load the user's profile whose action maps get merged with default's action map
	m_pPlayerProfileManager = m_pFramework->GetIPlayerProfileManager();

	bool bIsFirstTime = false;
	const bool bResetProfile = gEnv->pSystem->GetICmdLine()->FindArg(eCLAT_Pre,"ResetProfile") != 0;
	if (m_pPlayerProfileManager)
	{
		const char* userName = gEnv->pSystem->GetUserName();

		bool ok = m_pPlayerProfileManager->LoginUser(userName, bIsFirstTime);
		if (ok)
		{
			m_pPlayerProfileManager->SetExclusiveControllerDeviceIndex(0);

			// activate the always present profile "default"
			int profileCount = m_pPlayerProfileManager->GetProfileCount(userName);
			if (profileCount > 0)
			{
				bool handled = false;
				if(gEnv->IsDedicated())
				{
					for(int i = 0; i < profileCount; ++i )
					{
						IPlayerProfileManager::SProfileDescription profDesc;
						ok = m_pPlayerProfileManager->GetProfileInfo(userName, i, profDesc);
						if(ok)
						{
							const IPlayerProfile *preview = m_pPlayerProfileManager->PreviewProfile(userName, profDesc.name);
							int iActive = 0;
							if(preview)
							{
								preview->GetAttribute("Activated",iActive);
							}
							if(iActive>0)
							{
								m_pPlayerProfileManager->ActivateProfile(userName,profDesc.name);
								CryLogAlways("[GameProfiles]: Successfully activated profile '%s' for user '%s'", profDesc.name, userName);
								m_pFramework->GetILevelSystem()->LoadRotation();
								handled = true;
								break;
							}
						}
					}
					m_pPlayerProfileManager->PreviewProfile(userName,NULL);
				}

				if(!handled)
				{
					IPlayerProfileManager::SProfileDescription desc;
					ok = m_pPlayerProfileManager->GetProfileInfo(userName, 0, desc);
					if (ok)
					{
						IPlayerProfile* pProfile = m_pPlayerProfileManager->ActivateProfile(userName, desc.name);

						if (pProfile == 0)
						{
							GameWarning("[GameProfiles]: Cannot activate profile '%s' for user '%s'. Trying to re-create.", desc.name, userName);
							IPlayerProfileManager::EProfileOperationResult profileResult;
							m_pPlayerProfileManager->CreateProfile(userName, desc.name, true, profileResult); // override if present!
							pProfile = m_pPlayerProfileManager->ActivateProfile(userName, desc.name);
							if (pProfile == 0)
								GameWarning("[GameProfiles]: Cannot activate profile '%s' for user '%s'.", desc.name, userName);
							else
								GameWarning("[GameProfiles]: Successfully re-created profile '%s' for user '%s'.", desc.name, userName);
						}

						if (pProfile)
						{
							if (bResetProfile)
							{
								bIsFirstTime = true;
								pProfile->Reset();
								gEnv->pCryPak->RemoveFile("%USER%/game.cfg");
								CryLogAlways("[GameProfiles]: Successfully reset and activated profile '%s' for user '%s'", desc.name, userName);
							}
							CryLogAlways("[GameProfiles]: Successfully activated profile '%s' for user '%s'", desc.name, userName);
							m_pFramework->GetILevelSystem()->LoadRotation();
						}
					}
					else
					{
						GameWarning("[GameProfiles]: Cannot get profile info for user '%s'", userName);
					}
				}
			}
			else
			{
				GameWarning("[GameProfiles]: User 'dude' has no profiles");
			}
		}
		else
			GameWarning("[GameProfiles]: Cannot login user '%s'", userName);
	}
	else
		GameWarning("[GameProfiles]: PlayerProfileManager not available. Running without.");

	m_pFramework->RegisterListener(this,"Game", FRAMEWORKLISTENERPRIORITY_GAME);

	CUIManager::Init();

#if ENABLE_FEATURE_TESTER
	new CFeatureTester();
#endif

	m_pRayCaster = new GlobalRayCaster;
	m_pRayCaster->SetQuota(6);

	m_pIntersectionTester = new GlobalIntersectionTester;
	m_pIntersectionTester->SetQuota(6);

#ifdef GAME_DEBUG_MEM
	DumpMemInfo("CGame::Init end");
#endif

	return true;
}

bool CGame::CompleteInit()
{
#ifdef GAME_DEBUG_MEM
	DumpMemInfo("CGame::CompleteInit");
#endif

	return true;
}

void CGame::RegisterGameFlowNodes()
{
	// Initialize Game02 flow nodes
	if (IFlowSystem *pFlow = m_pFramework->GetIFlowSystem())
	{
		CG2AutoRegFlowNodeBase *pFactory = CG2AutoRegFlowNodeBase::m_pFirst;

		while (pFactory)
		{
			pFlow->RegisterType( pFactory->m_sClassName,pFactory );
			pFactory = pFactory->m_pNext;
		}
	}

	GetMonoScriptSystem()->RegisterFlownodes();
}

// Small test for the IPathfinder.h interfaces
/*
extern INavPath *g_testPath;
extern IPathFollower *g_pathFollower;
extern Vec3 g_pos;
extern Vec3 g_vel;
*/

int CGame::Update(bool haveFocus, unsigned int updateFlags)
{
	bool bRun = m_pFramework->PreUpdate( true, updateFlags );

	float frameTime = gEnv->pTimer->GetFrameTime();

	if(m_pRayCaster)
	{
		m_pRayCaster->Update(frameTime);
	}

	if (m_pIntersectionTester)
	{
			FRAME_PROFILER("GlobalIntersectionTester", gEnv->pSystem, PROFILE_AI);

			m_pIntersectionTester->SetQuota(6);
			m_pIntersectionTester->Update(frameTime);
	}

	m_pGameMechanismManager->Update(frameTime);

	m_pFramework->PostUpdate( true, updateFlags );

	if(m_inDevMode != gEnv->pSystem->IsDevMode())
	{
		m_inDevMode = gEnv->pSystem->IsDevMode();
		m_pFramework->GetIActionMapManager()->EnableActionMap("debug", m_inDevMode);
	}
	
	CheckReloadLevel();

	return bRun ? 1 : 0;
}

void CGame::EditorResetGame(bool bStart)
{
	CRY_ASSERT(gEnv->IsEditor());

	if(bStart)
	{
		IActionMapManager* pAM = m_pFramework->GetIActionMapManager();
		if (pAM)
		{
			pAM->EnableActionMap(0, true); // enable all action maps
			pAM->EnableFilter(0, false); // disable all filters
		}		
	}	
}

void CGame::PlayerIdSet(EntityId playerId)
{
	m_clientActorId = playerId;	
}

string CGame::InitMapReloading()
{
	string levelFileName = GetIGameFramework()->GetLevelName();
	levelFileName = PathUtil::GetFileName(levelFileName);
	if(const char* visibleName = GetMappedLevelName(levelFileName))
		levelFileName = visibleName;

	levelFileName.append("_cryengine.cryenginejmsf");
	if (m_pPlayerProfileManager)
	{
		const char* userName = GetISystem()->GetUserName();
		IPlayerProfile* pProfile = m_pPlayerProfileManager->GetCurrentProfile(userName);
		if (pProfile)
		{
			const char* sharedSaveGameFolder = m_pPlayerProfileManager->GetSharedSaveGameFolder();
			if (sharedSaveGameFolder && *sharedSaveGameFolder)
			{
				string prefix = pProfile->GetName();
				prefix+="_";
				levelFileName = prefix + levelFileName;
			}
			ISaveGameEnumeratorPtr pSGE = pProfile->CreateSaveGameEnumerator();
			ISaveGameEnumerator::SGameDescription desc;	
			const int nSaveGames = pSGE->GetCount();
			for (int i=0; i<nSaveGames; ++i)
			{
				if (pSGE->GetDescription(i, desc))
				{
					if(!stricmp(desc.name,levelFileName.c_str()))
					{
						m_bReload = true;
						return levelFileName;
					}
				}
			}
		}
	}
#ifndef WIN32
	m_bReload = true; //using map command
#else
	m_bReload = false;
	levelFileName.clear();
#endif
	return levelFileName;
}

void CGame::Shutdown()
{
	if (m_pPlayerProfileManager)
		m_pPlayerProfileManager->LogoutUser(m_pPlayerProfileManager->GetCurrentUser());

	CUIManager::Destroy();

	this->~CGame();
}

const char *CGame::GetLongName()
{
	return GAME_LONGNAME;
}

const char *CGame::GetName()
{
	return GAME_NAME;
}

void CGame::OnPostUpdate(float fDeltaTime)
{
	//update camera system
	//m_pCameraManager->Update();
}

void CGame::OnActionEvent(const SActionEvent& event)
{ 
	switch(event.m_event)
	{
	case eAE_unloadLevel:
		m_clientActorId = 0;
		break;
	}
}

void CGame::BlockingProcess(BlockingConditionFunction f)
{
  INetwork* pNetwork = gEnv->pNetwork;

  if (!pNetwork)
	  return;

  bool ok = false;

  ITimer * pTimer = gEnv->pTimer;
  CTimeValue startTime = pTimer->GetAsyncTime();

  while (!ok)
  {
    pNetwork->SyncWithGame(eNGS_FrameStart);
    pNetwork->SyncWithGame(eNGS_FrameEnd);
    gEnv->pTimer->UpdateOnFrameStart();
    ok = (*f)();
  }
}

uint32 CGame::AddGameWarning(const char* stringId, const char* paramMessage, IGameWarningsListener* pListener)
{
	if(CUIManager::GetInstance() && !gEnv->IsDedicated())
	{
		return CUIManager::GetInstance()->GetWarningManager()->AddGameWarning(stringId, paramMessage, pListener);
	}
	else
	{
		CryLogAlways("GameWarning trying to display: %s", stringId);
		return 0;
	}
}

void CGame::RemoveGameWarning(const char* stringId)
{
	if(CUIManager::GetInstance() && !gEnv->IsDedicated())
	{
		CUIManager::GetInstance()->GetWarningManager()->RemoveGameWarning(stringId);
	}
}







const static uint8 drmKeyData[16] = {0};
const static char* drmFiles = NULL;


const uint8* CGame::GetDRMKey()
{
	return drmKeyData;
}

const char* CGame::GetDRMFileList()
{
	return drmFiles;
}

CGameRules *CGame::GetGameRules() const
{
	return static_cast<CGameRules *>(m_pFramework->GetIGameRulesSystem()->GetCurrentGameRules());
}

void CGame::LoadActionMaps(const char* filename)
{
	CRY_ASSERT_MESSAGE((filename || *filename != 0), "filename is empty!");
	if(g_pGame->GetIGameFramework()->IsGameStarted())
	{
		CryLogAlways("Can't change configuration while game is running (yet)");
		return;
	}

	IActionMapManager *pActionMapMan = m_pFramework->GetIActionMapManager();
	pActionMapMan->AddInputDeviceMapping(eAID_KeyboardMouse, "keyboard");
	pActionMapMan->AddInputDeviceMapping(eAID_XboxPad, "xboxpad");
	pActionMapMan->AddInputDeviceMapping(eAID_PS3Pad, "ps3pad");

	// make sure that they are also added to the GameActions.actions file!
	XmlNodeRef rootNode = m_pFramework->GetISystem()->LoadXmlFromFile(filename);
	if(rootNode)
	{
		pActionMapMan->Clear();
		pActionMapMan->LoadFromXML(rootNode);
		m_pDefaultAM = pActionMapMan->GetActionMap("default");
		m_pDebugAM = pActionMapMan->GetActionMap("debug");
		m_pMultiplayerAM = pActionMapMan->GetActionMap("multiplayer");

		// enable defaults
		pActionMapMan->EnableActionMap("default",true);

		// enable debug
		pActionMapMan->EnableActionMap("debug",gEnv->pSystem->IsDevMode());

		// enable player action map
		pActionMapMan->EnableActionMap("player",true);
	}
	else
	{
		CryLogAlways("[game] error: Could not open configuration file %s", filename);
		CryLogAlways("[game] error: this will probably cause an infinite loop later while loading a map");
	}

	m_pGameActions->Init();
}

void CGame::CheckReloadLevel()
{
	if(!m_bReload)
		return;

	if(gEnv->IsEditor() || gEnv->bMultiplayer)
	{
		if(m_bReload)
			m_bReload = false;
		return;
	}

#ifdef WIN32
	// Restart interrupts cutscenes
	gEnv->pMovieSystem->StopAllCutScenes();

	GetISystem()->SerializingFile(1);

	//load levelstart
	ILevelSystem* pLevelSystem = m_pFramework->GetILevelSystem();
	ILevel*			pLevel = pLevelSystem->GetCurrentLevel();
	ILevelInfo* pLevelInfo = pLevelSystem->GetLevelInfo(m_pFramework->GetLevelName());
	//**********
	EntityId playerID = GetIGameFramework()->GetClientActorId();
	pLevelSystem->OnLoadingStart(pLevelInfo);
	PlayerIdSet(playerID);
	string levelstart(GetIGameFramework()->GetLevelName());
	if(const char* visibleName = GetMappedLevelName(levelstart))
		levelstart = visibleName;

	levelstart.append("_cryengine.cryenginejmsf");
	GetIGameFramework()->LoadGame(levelstart.c_str(), true, true);
	//**********
	pLevelSystem->OnLoadingComplete(pLevel);
	m_bReload = false;	//if m_bReload is true - load at levelstart

	//if paused - start game
	m_pFramework->PauseGame(false, true);

	GetISystem()->SerializingFile(0);
#else
	string command("map ");
	command.append(m_pFramework->GetLevelName());
	gEnv->pConsole->ExecuteString(command);
#endif
}

void CGame::RegisterGameObjectEvents()
{
	IGameObjectSystem* pGOS = m_pFramework->GetIGameObjectSystem();

	pGOS->RegisterEvent(eCGE_PostFreeze, "PostFreeze");
	pGOS->RegisterEvent(eCGE_PostShatter,"PostShatter");
	pGOS->RegisterEvent(eCGE_OnShoot,"OnShoot");
	pGOS->RegisterEvent(eCGE_Recoil,"Recoil");
	pGOS->RegisterEvent(eCGE_BeginReloadLoop,"BeginReloadLoop");
	pGOS->RegisterEvent(eCGE_EndReloadLoop,"EndReloadLoop");
	pGOS->RegisterEvent(eCGE_ActorRevive,"ActorRevive");
	pGOS->RegisterEvent(eCGE_VehicleDestroyed,"VehicleDestroyed");
	pGOS->RegisterEvent(eCGE_TurnRagdoll,"TurnRagdoll");
	pGOS->RegisterEvent(eCGE_EnableFallAndPlay,"EnableFallAndPlay");
	pGOS->RegisterEvent(eCGE_DisableFallAndPlay,"DisableFallAndPlay1");
	pGOS->RegisterEvent(eCGE_VehicleTransitionEnter,"VehicleTransitionEnter");
	pGOS->RegisterEvent(eCGE_VehicleTransitionExit,"VehicleTransitionExit");
	pGOS->RegisterEvent(eCGE_TextArea,"TextArea");
	pGOS->RegisterEvent(eCGE_InitiateAutoDestruction,"InitiateAutoDestruction");
	pGOS->RegisterEvent(eCGE_Event_Collapsing,"Event_Collapsing");
	pGOS->RegisterEvent(eCGE_Event_Collapsed,"Event_Collapsed");
	pGOS->RegisterEvent(eCGE_MultiplayerChatMessage,"MultiplayerChatMessage");
	pGOS->RegisterEvent(eCGE_ResetMovementController,"ResetMovementController");
	pGOS->RegisterEvent(eCGE_AnimateHands,"AnimateHands");
	pGOS->RegisterEvent(eCGE_Ragdoll,"Ragdoll");
	pGOS->RegisterEvent(eCGE_EnablePhysicalCollider,"EnablePhysicalCollider");
	pGOS->RegisterEvent(eCGE_DisablePhysicalCollider,"DisablePhysicalCollider");
	pGOS->RegisterEvent(eCGE_RebindAnimGraphInputs,"RebindAnimGraphInputs");
	pGOS->RegisterEvent(eCGE_OpenParachute, "OpenParachute");
	pGOS->RegisterEvent(eCGE_ReactionEnd, "ReactionEnd");
}

void CGame::GetMemoryStatistics(ICrySizer * s)
{
	s->Add(*this);

	s->Add(*m_pGameActions);
}

void CGame::OnClearPlayerIds()
{
}

void CGame::DumpMemInfo(const char* format, ...)
{
	CryModuleMemoryInfo memInfo;
	CryGetMemoryInfoForModule(&memInfo);

	va_list args;
	va_start(args,format);
	gEnv->pLog->LogV( ILog::eAlways,format,args );
	va_end(args);

	gEnv->pLog->LogWithType( ILog::eAlways, "Alloc=%I64d kb  String=%I64d kb  STL-alloc=%I64d kb  STL-wasted=%I64d kb", (memInfo.allocated - memInfo.freed) >> 10 , memInfo.CryString_allocated >> 10, memInfo.STL_allocated >> 10 , memInfo.STL_wasted >> 10);
	// gEnv->pLog->LogV( ILog::eAlways, "%s alloc=%llu kb  instring=%llu kb  stl-alloc=%llu kb  stl-wasted=%llu kb", text, memInfo.allocated >> 10 , memInfo.CryString_allocated >> 10, memInfo.STL_allocated >> 10 , memInfo.STL_wasted >> 10);
}


const string& CGame::GetLastSaveGame(string &levelName)
{
	if (m_pPlayerProfileManager)
	{
		const char* userName = GetISystem()->GetUserName();
		IPlayerProfile* pProfile = m_pPlayerProfileManager->GetCurrentProfile(userName);
		if (pProfile)
		{
			ISaveGameEnumeratorPtr pSGE = pProfile->CreateSaveGameEnumerator();
			ISaveGameEnumerator::SGameDescription desc;	
			time_t curLatestTime = (time_t) 0;
			const char* lastSaveGame = "";
			const int nSaveGames = pSGE->GetCount();
			for (int i=0; i<nSaveGames; ++i)
			{
				if (pSGE->GetDescription(i, desc))
				{
					if (desc.metaData.saveTime > curLatestTime)
					{
						lastSaveGame = desc.name;
						curLatestTime = desc.metaData.saveTime;
						levelName = desc.metaData.levelName;
					}
				}
			}
			m_lastSaveGame = lastSaveGame;
		}
	}

	return m_lastSaveGame;
}

void CGame::PostSerialize()
{

}


ILINE void expandSeconds(int secs, int& days, int& hours, int& minutes, int& seconds)
{
	days  = secs / 86400;
	secs -= days * 86400;
	hours = secs / 3600;
	secs -= hours * 3600;
	minutes = secs / 60;
	seconds = secs - minutes * 60;
	hours += days*24;
	days = 0;
}

void secondsToString(int secs, string& outString)
{
	int d,h,m,s;
	expandSeconds(secs, d, h, m, s);
	if (h > 0)
		outString.Format("%02dh_%02dm_%02ds", h, m, s);
	else
		outString.Format("%02dm_%02ds", m, s);
}

IGame::TSaveGameName CGame::CreateSaveGameName()
{
	//design wants to have different, more readable names for the savegames generated
	int id = 0;

	TSaveGameName saveGameName;



	//saves a running savegame id which is displayed with the savegame name
	if(IPlayerProfileManager *m_pPlayerProfileManager = gEnv->pGame->GetIGameFramework()->GetIPlayerProfileManager())
	{
		const char *user = m_pPlayerProfileManager->GetCurrentUser();
		if(IPlayerProfile *pProfile = m_pPlayerProfileManager->GetCurrentProfile(user))
		{
			pProfile->GetAttribute("Singleplayer.SaveRunningID", id);
			pProfile->SetAttribute("Singleplayer.SaveRunningID", id+1);
		}
	}

	saveGameName = CRY_SAVEGAME_FILENAME;
	char buffer[16];
	itoa(id, buffer, 10);
	saveGameName.clear();
	if(id < 10)
		saveGameName += "0";
	saveGameName += buffer;
	saveGameName += "_";

	const char* levelName = GetIGameFramework()->GetLevelName();
	const char* mappedName = GetMappedLevelName(levelName);
	saveGameName += mappedName;
	saveGameName += "_cryengine";

	saveGameName += CRY_SAVEGAME_FILE_EXT;

	return saveGameName;

}

const char* CGame::GetMappedLevelName(const char *levelName) const
{ 
	TLevelMapMap::const_iterator iter = m_mapNames.find(CONST_TEMP_STRING(levelName));
	return (iter == m_mapNames.end()) ? levelName : iter->second.c_str();
}


void CGame::OnSystemEvent( ESystemEvent event, UINT_PTR wparam, UINT_PTR lparam )
{
		switch (event)
		{
		case ESYSTEM_EVENT_LEVEL_LOAD_START:
				{
						if (!m_pRayCaster)
						{
								m_pRayCaster = new GlobalRayCaster;
								m_pRayCaster->SetQuota(6);
						}
						if (!m_pIntersectionTester)
						{
								m_pIntersectionTester = new GlobalIntersectionTester;
								m_pIntersectionTester->SetQuota(6);
						}
				}
				break;
		case ESYSTEM_EVENT_LEVEL_UNLOAD:
				{
						SAFE_DELETE(m_pRayCaster);
						SAFE_DELETE(m_pIntersectionTester);
				}
				break;
		}
}

bool CGame::IsGameActive() const
{
	assert(g_pGame);
	IGameFramework* pGameFramework = g_pGame->GetIGameFramework();
	assert(pGameFramework);
	return (pGameFramework->StartingGameContext() || pGameFramework->StartedGameContext()) && pGameFramework->GetClientChannel();
}

void CGame::ClearGameSessionHandler()
{
	GetIGameFramework()->SetGameSessionHandler(NULL);
}

uint32 CGame::GetRandomNumber()
{
	return m_randomGenerator.GenerateUint32();
}

#include UNIQUE_VIRTUAL_WRAPPER(IGame)

