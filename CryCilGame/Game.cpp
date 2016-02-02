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
	IMonoField *field = MonoEnv->Cryambly->GetClass("CryCil", "Game")->GetField("guidText");
	auto value = NtText(field->Get<mono::string>(nullptr));
	this->gameFramework->SetGameGUID(value);
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

const char *CryCilGame::GetLongName()
{
	if (this->longGameName.Length == 0)
	{
		IMonoField *field = MonoEnv->Cryambly->GetClass("CryCil", "Game")->GetField("longName");
		this->longGameName = NtText(field->Get<mono::string>(nullptr));
	}
	return this->longGameName;
}

const char *CryCilGame::GetName()
{
	if (this->gameName.Length == 0)
	{
		IMonoField *field = MonoEnv->Cryambly->GetClass("CryCil", "Game")->GetField("name");
		this->gameName = NtText(field->Get<mono::string>(nullptr));
	}
	return this->gameName;
}

IGameFramework *CryCilGame::GetIGameFramework()
{
	return this->gameFramework;
}
IAntiCheatManager *CryCilGame::GetAntiCheatManager()
{
	return nullptr;
}

void CryCilGame::RegisterGameFlowNodes()
{
	// Let CryCIL handle this matter.
	MonoEnv->RegisterFlowGraphNodes();
}