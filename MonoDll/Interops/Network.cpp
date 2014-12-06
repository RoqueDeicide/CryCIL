#include "StdAfx.h"
#include "Network.h"

#include "MonoEntity.h"

#include <INetwork.h>

NetworkInterop::NetworkInterop()
{
	REGISTER_METHOD(IsMultiplayer);
	REGISTER_METHOD(IsServer);
	REGISTER_METHOD(IsClient);
}

NetworkInterop::~NetworkInterop()
{}

bool NetworkInterop::IsMultiplayer()
{
	return gEnv->bMultiplayer;
}

bool NetworkInterop::IsServer()
{
	return gEnv->bServer;
}

bool NetworkInterop::IsClient()
{
	return gEnv->IsClient();
}