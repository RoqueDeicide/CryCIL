#pragma once

#include "IMonoInterface.h"

struct CryEntityTriggerProxyInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryEntityTriggerProxy"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Logic.EntityProxies"; }

	virtual void InitializeInterops() override;

	static void SetTriggerBounds(IEntityTriggerProxy *handle, const AABB &bbox);
	static void GetTriggerBounds(IEntityTriggerProxy *handle, AABB &bbox);
	static void ForwardEventsTo(IEntityTriggerProxy *handle, EntityId id);
	static void InvalidateTrigger(IEntityTriggerProxy *handle);
};