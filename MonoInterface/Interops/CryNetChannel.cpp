#include "stdafx.h"

#include "CryNetChannel.h"

void CryNetChannelInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetIsLocal);
}

bool CryNetChannelInterop::GetIsLocal(INetChannel *handle)
{
	return handle->IsLocal();
}
