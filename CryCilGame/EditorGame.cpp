#include "stdafx.h"

#include "EditorGame.h"

bool CryCilEditorGame::Init(ISystem *pSystem, IGameToEditorInterface *pEditorInterface)
{
	throw std::logic_error("The method or operation is not implemented.");
}

int CryCilEditorGame::Update(bool haveFocus, unsigned int updateFlags)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilEditorGame::Shutdown()
{
	throw std::logic_error("The method or operation is not implemented.");
}

bool CryCilEditorGame::SetGameMode(bool bGameMode)
{
	throw std::logic_error("The method or operation is not implemented.");
}

IEntity * CryCilEditorGame::GetPlayer()
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilEditorGame::SetPlayerPosAng(Vec3 pos, Vec3 viewDir)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilEditorGame::HidePlayer(bool bHide)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilEditorGame::OnBeforeLevelLoad()
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilEditorGame::OnAfterLevelInit(const char *levelName, const char *levelFolder)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilEditorGame::OnAfterLevelLoad(const char *levelName, const char *levelFolder)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilEditorGame::OnCloseLevel()
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilEditorGame::OnSaveLevel()
{
	throw std::logic_error("The method or operation is not implemented.");
}

bool CryCilEditorGame::BuildEntitySerializationList(XmlNodeRef output)
{
	throw std::logic_error("The method or operation is not implemented.");
}

bool CryCilEditorGame::GetAdditionalMinimapData(XmlNodeRef output)
{
	throw std::logic_error("The method or operation is not implemented.");
}

IFlowSystem * CryCilEditorGame::GetIFlowSystem()
{
	throw std::logic_error("The method or operation is not implemented.");
}

IGameTokenSystem* CryCilEditorGame::GetIGameTokenSystem()
{
	throw std::logic_error("The method or operation is not implemented.");
}

IEquipmentSystemInterface* CryCilEditorGame::GetIEquipmentSystemInterface()
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilEditorGame::RegisterTelemetryTimelineRenderers(Telemetry::ITelemetryRepository* pRepository)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void CryCilEditorGame::OnDisplayRenderUpdated(bool displayHelpers)
{
	throw std::logic_error("The method or operation is not implemented.");
}
