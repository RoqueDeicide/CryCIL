#include "stdafx.h"

#include "CryEntityTriggerProxy.h"

void CryEntityTriggerProxyInterop::InitializeInterops()
{
	REGISTER_METHOD(SetTriggerBounds);
	REGISTER_METHOD(GetTriggerBounds);
	REGISTER_METHOD(ForwardEventsTo);
	REGISTER_METHOD(InvalidateTrigger);
}

void CryEntityTriggerProxyInterop::SetTriggerBounds(IEntityTriggerProxy *handle, const AABB &bbox)
{
	handle->SetTriggerBounds(bbox);
}

void CryEntityTriggerProxyInterop::GetTriggerBounds(IEntityTriggerProxy *handle, AABB &bbox)
{
	handle->GetTriggerBounds(bbox);
}

void CryEntityTriggerProxyInterop::ForwardEventsTo(IEntityTriggerProxy *handle, EntityId id)
{
	handle->ForwardEventsTo(id);
}

void CryEntityTriggerProxyInterop::InvalidateTrigger(IEntityTriggerProxy *handle)
{
	handle->InvalidateTrigger();
}
