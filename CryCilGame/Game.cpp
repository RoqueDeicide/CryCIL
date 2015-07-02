#include "stdafx.h"

#include "Game.h"

bool CryCilGame::Init(IGameFramework *pFramework)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilGame::InitEditor(IGameToEditorInterface* pGameToEditor)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilGame::GetMemoryStatistics(ICrySizer * s)
{
	throw std::logic_error("The method or operation is not implemented.");
}

bool CryCilGame::CompleteInit()
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilGame::Shutdown()
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilGame::PrePhysicsUpdate()
{
	throw std::logic_error("The method or operation is not implemented.");
}

int CryCilGame::Update(bool haveFocus, unsigned int updateFlags)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilGame::EditorResetGame(bool bStart)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilGame::PlayerIdSet(EntityId playerId)
{
	throw std::logic_error("The method or operation is not implemented.");
}

const char * CryCilGame::GetLongName()
{
	throw std::logic_error("The method or operation is not implemented.");
}

const char * CryCilGame::GetName()
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilGame::LoadActionMaps(const char* filename)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilGame::OnClearPlayerIds()
{
	throw std::logic_error("The method or operation is not implemented.");
}

IGame::TSaveGameName CryCilGame::CreateSaveGameName()
{
	throw std::logic_error("The method or operation is not implemented.");
}

IGameFramework * CryCilGame::GetIGameFramework()
{
	throw std::logic_error("The method or operation is not implemented.");
}

const char* CryCilGame::GetMappedLevelName(const char *levelName) const
{
	throw std::logic_error("The method or operation is not implemented.");
}

IAntiCheatManager * CryCilGame::GetAntiCheatManager()
{
	throw std::logic_error("The method or operation is not implemented.");
}

const bool CryCilGame::DoInitialSavegame() const
{
	throw std::logic_error("The method or operation is not implemented.");
}

uint32 CryCilGame::AddGameWarning(const char* stringId, const char* paramMessage, IGameWarningsListener* pListener /*= NULL*/)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilGame::OnRenderScene(const SRenderingPassInfo &passInfo)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilGame::RenderGameWarnings()
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilGame::RemoveGameWarning(const char* stringId)
{
	throw std::logic_error("The method or operation is not implemented.");
}

bool CryCilGame::GameEndLevel(const char* stringId)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilGame::SetUserProfileChanged(bool yesNo)
{
	throw std::logic_error("The method or operation is not implemented.");
}

IGameStateRecorder* CryCilGame::CreateGameStateRecorder(IGameplayListener* pL)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilGame::FullSerialize(TSerialize ser)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilGame::PostSerialize()
{
	throw std::logic_error("The method or operation is not implemented.");
}

IGame::ExportFilesInfo CryCilGame::ExportLevelData(const char* levelName, const char* missionName) const
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilGame::LoadExportedLevelData(const char* levelName, const char* missionName)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilGame::RegisterGameFlowNodes()
{
	throw std::logic_error("The method or operation is not implemented.");
}

IGamePhysicsSettings* CryCilGame::GetIGamePhysicsSettings()
{
	throw std::logic_error("The method or operation is not implemented.");
}

void* CryCilGame::GetGameInterface()
{
	throw std::logic_error("The method or operation is not implemented.");
}
