/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// CryENGINE Network scriptbind
//////////////////////////////////////////////////////////////////////////
// 10/06/2012 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __SCRIPTBIND_NETWORK_H__
#define __SCRIPTBIND_NETWORK_H__

#include <MonoCommon.h>
#include <IMonoScriptbind.h>

#include <IGameObject.h>

class CScriptbind_Network
	: public IMonoScriptBind
{
public:
	CScriptbind_Network();
	~CScriptbind_Network();

	// IMonoScriptbind
	virtual const char *GetClassName() { return "NetworkInterop"; }
	// ~IMonoScriptbind

	static bool IsMultiplayer();
	static bool IsServer();
	static bool IsClient();
};

#endif __SCRIPTBIND_NETWORK_H__