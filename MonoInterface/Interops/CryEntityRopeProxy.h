#pragma once

#include "IMonoInterface.h"

struct CryEntityRopeProxyInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryEntityRopeProxy"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Logic.EntityProxies"; }

	virtual void InitializeInterops() override;

	static IRopeRenderNode *GetRopeRenderNode(IEntityRopeProxy *handle);
};