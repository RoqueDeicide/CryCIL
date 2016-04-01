#pragma once

#include "IMonoInterface.h"

struct CryEntitySubstitutionProxyInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryEntitySubstitutionProxy"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Logic.EntityProxies"; }

	virtual void InitializeInterops() override;

	static void SetSubstitute(IEntitySubstitutionProxy *handle, IRenderNode *pSubstitute);
	static IRenderNode *GetSubstitute(IEntitySubstitutionProxy *handle);
};