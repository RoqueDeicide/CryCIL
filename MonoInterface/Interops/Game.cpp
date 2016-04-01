#include "stdafx.h"

#include "Game.h"

void GameInterop::InitializeInterops()
{
	REGISTER_METHOD(get_IsEditor);
	REGISTER_METHOD(get_IsEditing);
	REGISTER_METHOD(get_IsMultiplayer);
	REGISTER_METHOD(get_IsServer);
	REGISTER_METHOD(get_IsClient);
	REGISTER_METHOD(get_IsDedicatedServer);
	REGISTER_METHOD(get_IsFullMotionVideoPlaying);
	REGISTER_METHOD(get_IsCutscenePlaying);
}

bool GameInterop::get_IsEditor()
{
	return gEnv->IsEditor();
}

bool GameInterop::get_IsEditing()
{
	return gEnv->IsEditing();
}

bool GameInterop::get_IsMultiplayer()
{
	return gEnv->bMultiplayer;
}

bool GameInterop::get_IsServer()
{
	return gEnv->bServer;
}

bool GameInterop::get_IsClient()
{
	return gEnv->IsClient();
}

bool GameInterop::get_IsDedicatedServer()
{
	return gEnv->IsDedicated();
}

bool GameInterop::get_IsFullMotionVideoPlaying()
{
	return gEnv->IsFMVPlaying();
}

bool GameInterop::get_IsCutscenePlaying()
{
	return gEnv->IsCutscenePlaying();
}
