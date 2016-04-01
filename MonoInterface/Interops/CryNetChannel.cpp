#include "stdafx.h"

#include "CryNetChannel.h"

void CryNetChannelInterop::InitializeInterops()
{
	REGISTER_METHOD(GetIsLocal);
}

bool CryNetChannelInterop::GetIsLocal(INetChannel *handle)
{
	return handle->IsLocal();
}
