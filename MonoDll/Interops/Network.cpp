#include "StdAfx.h"
#include "Network.h"

#include "MonoEntity.h"

#include <INetwork.h>

CScriptbind_Network::CScriptbind_Network()
{
	REGISTER_METHOD(IsMultiplayer);
	REGISTER_METHOD(IsServer);
	REGISTER_METHOD(IsClient);
}

CScriptbind_Network::~CScriptbind_Network()
{}

bool CScriptbind_Network::IsMultiplayer()
{
	return gEnv->bMultiplayer;
}

bool CScriptbind_Network::IsServer()
{
	return gEnv->bServer;
}

bool CScriptbind_Network::IsClient()
{
	return gEnv->IsClient();
}