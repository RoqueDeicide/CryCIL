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

void CryCilGame::InitEditor(IGameToEditorInterface*)
{
	// There is nothing useful that can be done at this point.
}

void CryCilGame::GetMemoryStatistics(ICrySizer *)
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

void CryCilGame::EditorResetGame(bool)
{
}

void CryCilGame::PlayerIdSet(EntityId)
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

void CryCilGame::LoadActionMaps(const char *)
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

const char *CryCilGame::GetMappedLevelName(const char *) const
{
	return "";
}

IAntiCheatManager *CryCilGame::GetAntiCheatManager()
{
	return nullptr;
}

// ReSharper disable once CppConstValueFunctionReturnType
const bool CryCilGame::DoInitialSavegame() const
{
	return true;
}

uint32 CryCilGame::AddGameWarning(const char *, const char *, IGameWarningsListener *)
{
	return 0;
}

void CryCilGame::OnRenderScene(const SRenderingPassInfo &)
{
}

void CryCilGame::RenderGameWarnings()
{
}

void CryCilGame::RemoveGameWarning(const char *)
{
}

bool CryCilGame::GameEndLevel(const char *)
{
	return false;
}

IGameStateRecorder* CryCilGame::CreateGameStateRecorder(IGameplayListener *)
{
	return nullptr;
}

void CryCilGame::FullSerialize(TSerialize)
{
}

void CryCilGame::PostSerialize()
{
}

IGame::ExportFilesInfo CryCilGame::ExportLevelData(const char *levelName, const char *) const
{
	return IGame::ExportLevelData(levelName, nullptr);
}

void CryCilGame::LoadExportedLevelData(const char *, const char *)
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