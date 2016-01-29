#include "stdafx.h"

#include "CryEntityRopeProxy.h"

void CryEntityRopeProxyInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetRopeRenderNode);
}

IRopeRenderNode *CryEntityRopeProxyInterop::GetRopeRenderNode(IEntityRopeProxy *handle)
{
	return handle->GetRopeRenderNode();
}
