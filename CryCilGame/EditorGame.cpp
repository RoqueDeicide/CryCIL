#include "stdafx.h"

#include "EditorGame.h"
#include <IEntitySystem.h>
#include <IGameRulesSystem.h>
#include <ILevelSystem.h>

extern "C"
{
	GAME_API IGameStartup *CreateGameStartup();
};

CryCilEditorGame::CryCilEditorGame()
	: game(nullptr)
	, gameStartup(nullptr)
	, enabled(false)
	, gameMode(false)
	, player(false)
	, saturateScreen(false)
{
}

CryCilEditorGame::~CryCilEditorGame()
{
}

bool CryCilEditorGame::Init(ISystem *system, IGameToEditorInterface *editorInterface)
{
	SSystemInitParams initParams;
	initParams.bEditor = true;
	initParams.pSystem = system;
	initParams.bExecuteCommandLine = false;

	this->gameStartup = CreateGameStartup();
	this->game = this->gameStartup->Init(initParams);
	this->game->InitEditor(editorInterface);

	gEnv->bServer = true;
	gEnv->bMultiplayer = false;

#if !defined(CONSOLE)
	gEnv->SetIsClient(true);
#endif

	this->SetGameMode(false);
	this->ConfigureNetContext(true);

	return true;
}

int CryCilEditorGame::Update(bool haveFocus, unsigned int updateFlags)
{
	if (this->saturateScreen)
	{
		this->SaturateScreen();
	}

	return this->gameStartup->Update(haveFocus, updateFlags);
}

void CryCilEditorGame::Shutdown()
{
	this->EnablePlayer(false);
	this->SetGameMode(false);
	this->gameStartup->Shutdown();
}

bool CryCilEditorGame::SetGameMode(bool gameMode)
{
	this->gameMode = gameMode;

	if (this->ConfigureNetContext(this->player))
	{
		if (gEnv->IsEditor())
		{
			this->game->EditorResetGame(gameMode);
		}

		this->saturateScreen = gameMode;
		this->game->GetIGameFramework()->OnEditorSetGameMode(gameMode);

		return true;
	}

	gEnv->pLog->LogWarning("Failed configuring net context");
	return false;
}

IEntity *CryCilEditorGame::GetPlayer()
{
	return gEnv->pEntitySystem->GetEntity(LOCAL_PLAYER_ENTITY_ID);
}

void CryCilEditorGame::SetPlayerPosAng(Vec3 pos, Vec3 viewDir)
{
	IEntity *player = this->GetPlayer();
	if (player)
	{
		player->SetPosRotScale(pos, Quat::CreateRotationVDir(viewDir), Vec3(1), ENTITY_XFORM_EDITOR);
	}
}

void CryCilEditorGame::HidePlayer(bool hide)
{
	IEntity *player = this->GetPlayer();
	if (player)
	{
		player->Hide(hide);
	}
}

void CryCilEditorGame::OnBeforeLevelLoad()
{
	this->EnablePlayer(false);
	this->ConfigureNetContext(true);
	auto gameFramework = this->game->GetIGameFramework();
	const char *defaultGameRules = gEnv->pConsole->GetCVar("sv_gamerulesdefault")->GetString();
	gameFramework->GetIGameRulesSystem()->CreateGameRules(defaultGameRules);
	gameFramework->GetILevelSystem()->OnLoadingStart(nullptr);
}

void CryCilEditorGame::OnAfterLevelLoad(const char *levelName, const char *)
{
	auto levelSystem = this->game->GetIGameFramework()->GetILevelSystem();
	auto level = levelSystem->SetEditorLoadedLevel(levelName);
	levelSystem->OnLoadingComplete(level);
	this->EnablePlayer(true);
}

IFlowSystem *CryCilEditorGame::GetIFlowSystem()
{
	return this->game->GetIGameFramework()->GetIFlowSystem();
}

IGameTokenSystem *CryCilEditorGame::GetIGameTokenSystem()
{
	return this->game->GetIGameFramework()->GetIGameTokenSystem();
}

bool CryCilEditorGame::ConfigureNetContext(bool on)
{
	bool ok = false;

	IGameFramework* gameFramework = this->game->GetIGameFramework();

	if (on == this->enabled)
	{
		ok = true;
	}
	else if (on)
	{
		SGameContextParams context;

		SGameStartParams gameParams;
		gameParams.flags = eGSF_Server
			| eGSF_NoSpawnPlayer
			| eGSF_Client
			| eGSF_NoLevelLoading
			| eGSF_BlockingClientConnect
			| eGSF_NoGameRules
			| eGSF_NoQueries
			| eGSF_LocalOnly;
		gameParams.connectionString = "";
		gameParams.hostname = "localhost";
		gameParams.port = 60695;
		gameParams.pContextParams = &context;
		gameParams.maxPlayers = 1;

		if (gameFramework->StartGameContext(&gameParams))
		{
			ok = true;
		}
	}
	else
	{
		gameFramework->EndGameContext();
		gEnv->pNetwork->SyncWithGame(eNGS_Shutdown);
		ok = true;
	}

	this->enabled = on && ok;
	return ok;
}

void CryCilEditorGame::EnablePlayer(bool player)
{
	bool spawnPlayer = false;

	if (this->player != player)
	{
		spawnPlayer = this->player = player;
	}
	if (!this->SetGameMode(this->gameMode))
	{
		gEnv->pLog->LogWarning("Failed setting game mode");
	}
	else if (this->enabled && spawnPlayer)
	{
		if (!this->game->GetIGameFramework()->BlockingSpawnPlayer())
		{
			gEnv->pLog->LogWarning("Failed spawning player");
		}
	}
}

void CryCilEditorGame::SaturateScreen()
{
	static float currentSaturation;
	currentSaturation = gEnv->pTimer->GetFrameTime() * 10;

	if (currentSaturation > g_PI2)
	{
		currentSaturation = 0;
		this->saturateScreen = false;
	}

	gEnv->p3DEngine->SetPostEffectParam("Global_User_Saturation", (cosf(currentSaturation) + 1) * 0.5f, true);
}
