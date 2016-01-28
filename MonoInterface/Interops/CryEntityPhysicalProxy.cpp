#include "stdafx.h"

#include "CryEntityPhysicalProxy.h"
#include "PhysicalizationParameters.h"

void CryEntityPhysicalProxyInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(AssignPhysicalEntity);
	REGISTER_METHOD(GetWorldBounds);
	REGISTER_METHOD(GetLocalBounds);
	REGISTER_METHOD(PhysicalizeInternal);
	REGISTER_METHOD(ReattachSoftEntityVtx);
	REGISTER_METHOD(GetPhysicalEntity);
	REGISTER_METHOD(SerializeTyped);
	REGISTER_METHOD(EnablePhysics);
	REGISTER_METHOD(IsPhysicsEnabled);
	REGISTER_METHOD(AddImpulseInternal);
	REGISTER_METHOD(SetTriggerBounds);
	REGISTER_METHOD(GetTriggerBounds);
	REGISTER_METHOD(GetPartId0);
	REGISTER_METHOD(EnableNetworkSerialization);
	REGISTER_METHOD(IgnoreXFormEvent);
}

void CryEntityPhysicalProxyInterop::AssignPhysicalEntity(IEntityPhysicalProxy *handle,
														 IPhysicalEntity *pPhysEntity, int nSlot)
{
	handle->AssignPhysicalEntity(pPhysEntity, nSlot);
}

void CryEntityPhysicalProxyInterop::GetWorldBounds(IEntityPhysicalProxy *handle, AABB &bounds)
{
	handle->GetWorldBounds(bounds);
}

void CryEntityPhysicalProxyInterop::GetLocalBounds(IEntityPhysicalProxy *handle, AABB &bounds)
{
	handle->GetLocalBounds(bounds);
}

void CryEntityPhysicalProxyInterop::PhysicalizeInternal(IEntityPhysicalProxy *handle, EntityPhysicalizationParameters &parameters)
{
	SEntityPhysicalizeParams params;
	parameters.ToParams(params);

	handle->Physicalize(params);

	parameters.Dispose(params);
}

void CryEntityPhysicalProxyInterop::ReattachSoftEntityVtx(IEntityPhysicalProxy *handle, IPhysicalEntity *pAttachToEntity, int nAttachToPart)
{
	handle->ReattachSoftEntityVtx(pAttachToEntity, nAttachToPart);
}

IPhysicalEntity *CryEntityPhysicalProxyInterop::GetPhysicalEntity(IEntityPhysicalProxy *handle)
{
	return handle->GetPhysicalEntity();
}

void CryEntityPhysicalProxyInterop::SerializeTyped(IEntityPhysicalProxy *handle, ISerialize *ser, int type, int flags)
{
	handle->SerializeTyped(ser, type, flags);
}

void CryEntityPhysicalProxyInterop::EnablePhysics(IEntityPhysicalProxy *handle, bool bEnable)
{
	handle->EnablePhysics(bEnable);
}

bool CryEntityPhysicalProxyInterop::IsPhysicsEnabled(IEntityPhysicalProxy *handle)
{
	return handle->IsPhysicsEnabled();
}

void CryEntityPhysicalProxyInterop::AddImpulseInternal(IEntityPhysicalProxy *handle, int ipart, const Vec3 &pos, const Vec3 &impulse, bool bPos, float fAuxScale, float fPushScale /*= 1.0f*/)
{
	handle->AddImpulse(ipart, pos, impulse, bPos, fAuxScale, fPushScale);
}

void CryEntityPhysicalProxyInterop::SetTriggerBounds(IEntityPhysicalProxy *handle, const AABB &bbox)
{
	handle->SetTriggerBounds(bbox);
}

void CryEntityPhysicalProxyInterop::GetTriggerBounds(IEntityPhysicalProxy *handle, AABB &bbox)
{
	handle->GetTriggerBounds(bbox);
}

int CryEntityPhysicalProxyInterop::GetPartId0(IEntityPhysicalProxy *handle)
{
	return handle->GetPartId0();
}

void CryEntityPhysicalProxyInterop::EnableNetworkSerialization(IEntityPhysicalProxy *handle, bool enable)
{
	handle->EnableNetworkSerialization(enable);
}

void CryEntityPhysicalProxyInterop::IgnoreXFormEvent(IEntityPhysicalProxy *handle, bool ignore)
{
	handle->IgnoreXFormEvent(ignore);
}
