#include "stdafx.h"

#include "ChannelId.h"

void ChannelIdInterop::InitializeInterops()
{
	REGISTER_METHOD(GetChannel);
}

INetChannel *ChannelIdInterop::GetChannel(ushort id)
{
	return MonoEnv->CryAction->GetNetChannel(id);
}
