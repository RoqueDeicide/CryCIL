#pragma once

#include "IMonoInterface.h"

struct ChannelIdInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "ChannelId"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Network"; }

	virtual void OnRunTimeInitialized() override;

	static INetChannel *GetChannel(ushort id);
};