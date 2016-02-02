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
	// TODO: set the name from CryCIL, since the latter is supposed to be already running at this point.
	return GAME_LONGNAME;
}

const char *CryCilGame::GetName()
{
	// TODO: set the name from CryCIL, since the latter is supposed to be already running at this point.
	return GAME_NAME;
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