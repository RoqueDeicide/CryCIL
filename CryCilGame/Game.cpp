#include "stdafx.h"

#include "Game.h"


CryCilGame::CryCilGame()
	: gameFramework(nullptr)
{
	GetISystem()->SetIGame(this);
}

CryCilGame::~CryCilGame()
{
	// End the game, if it was running.
	if (this->gameFramework->StartedGameContext())
	{
		this->gameFramework->EndGameContext();
	}
	// Tell the system that this object is no longer usable.
	GetISystem()->SetIGame(nullptr);
}

bool CryCilGame::Init(IGameFramework *pFramework)
{
	// Save the pointer to the framework object.
	this->gameFramework = pFramework;
	// Assign a GUID to the game.
	//
	// TODO: set the game GUID from CryCIL, since the latter is supposed to be already running at this point.
	this->gameFramework->SetGameGUID(GAME_GUID);
	return true;
}

void CryCilGame::InitEditor(IGameToEditorInterface* pGameToEditor)
{
	// There is nothing useful that can be done at this point.
}

void CryCilGame::GetMemoryStatistics(ICrySizer * s)
{
}

bool CryCilGame::CompleteInit()
{
	return true;
}

void CryCilGame::Shutdown()
{
	this->~CryCilGame();
}

int CryCilGame::Update(bool haveFocus, unsigned int updateFlags)
{
	const bool run = this->gameFramework->PreUpdate(haveFocus, updateFlags);
	this->gameFramework->PostUpdate(haveFocus, updateFlags);
	return run ? 1 : 0;
}

void CryCilGame::EditorResetGame(bool bStart)
{
}

void CryCilGame::PlayerIdSet(EntityId playerId)
{
}

const char *CryCilGame::GetLongName()
{
	// TODO: set the name from CryCIL, since the latter is supposed to be already running at this point.
	return GAME_LONGNAME;
}

const char *CryCilGame::GetName()
{
	// TODO: set the name from CryCIL, since the latter is supposed to be already running at this point.
	return GAME_NAME;
}

void CryCilGame::LoadActionMaps(const char* filename)
{
}

void CryCilGame::OnClearPlayerIds()
{
}

IGame::TSaveGameName CryCilGame::CreateSaveGameName()
{
	return TSaveGameName();
}

IGameFramework *CryCilGame::GetIGameFramework()
{
	return this->gameFramework;
}

const char *CryCilGame::GetMappedLevelName(const char *levelName) const
{
	return "";
}

IAntiCheatManager *CryCilGame::GetAntiCheatManager()
{
	return nullptr;
}

const bool CryCilGame::DoInitialSavegame() const
{
	return true;
}

uint32 CryCilGame::AddGameWarning(const char* stringId, const char* paramMessage, IGameWarningsListener* pListener /*= NULL*/)
{
	return 0;
}

void CryCilGame::OnRenderScene(const SRenderingPassInfo &passInfo)
{
}

void CryCilGame::RenderGameWarnings()
{
}

void CryCilGame::RemoveGameWarning(const char* stringId)
{
}

bool CryCilGame::GameEndLevel(const char* stringId)
{
	return false;
}

IGameStateRecorder* CryCilGame::CreateGameStateRecorder(IGameplayListener* pL)
{
	return nullptr;
}

void CryCilGame::FullSerialize(TSerialize ser)
{
}

void CryCilGame::PostSerialize()
{
}

IGame::ExportFilesInfo CryCilGame::ExportLevelData(const char* levelName, const char* missionName) const
{
	return IGame::ExportLevelData(levelName, 0);
}

void CryCilGame::LoadExportedLevelData(const char* levelName, const char* missionName)
{
}

void CryCilGame::RegisterGameFlowNodes()
{
	// Let CryCIL handle this matter.
	MonoEnv->RegisterFlowGraphNodes();
}

IGamePhysicsSettings* CryCilGame::GetIGamePhysicsSettings()
{
	return nullptr;
}

void *CryCilGame::GetGameInterface()
{
	return nullptr;
}