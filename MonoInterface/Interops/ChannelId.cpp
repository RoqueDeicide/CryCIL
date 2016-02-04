#include "stdafx.h"

#include "ChannelId.h"

void ChannelIdInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetChannel);
}

INetChannel *ChannelIdInterop::GetChannel(ushort id)
{
	return MonoEnv->CryAction->GetNetChannel(id);
}
