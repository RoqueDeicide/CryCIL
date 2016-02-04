#pragma once

#include "IMonoInterface.h"

struct CryNetChannelInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryNetChannel"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Network"; }

	virtual void OnRunTimeInitialized() override;

	static bool GetIsLocal(INetChannel *handle);
};