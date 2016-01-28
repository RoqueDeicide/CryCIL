#pragma once

#include "IMonoInterface.h"

struct EntityPhysicalizationParameters;

struct CryEntityPhysicalProxyInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryEntityPhysicalProxy"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Logic.EntityProxies"; }

	virtual void OnRunTimeInitialized() override;

	static void AssignPhysicalEntity(IEntityPhysicalProxy *handle, IPhysicalEntity *pPhysEntity, int nSlot);
	static void GetWorldBounds(IEntityPhysicalProxy *handle, AABB &bounds);
	static void GetLocalBounds(IEntityPhysicalProxy *handle, AABB &bounds);
	static void PhysicalizeInternal(IEntityPhysicalProxy *handle, EntityPhysicalizationParameters &parameters);
	static void ReattachSoftEntityVtx(IEntityPhysicalProxy *handle, IPhysicalEntity *pAttachToEntity, int nAttachToPart);
	static IPhysicalEntity *GetPhysicalEntity(IEntityPhysicalProxy *handle);
	static void SerializeTyped(IEntityPhysicalProxy *handle, ISerialize *ser, int type, int flags);
	static void EnablePhysics(IEntityPhysicalProxy *handle, bool bEnable);
	static bool IsPhysicsEnabled(IEntityPhysicalProxy *handle);
	static void AddImpulseInternal(IEntityPhysicalProxy *handle, int ipart, const Vec3 &pos, const Vec3 &impulse, bool bPos, float fAuxScale, float fPushScale = 1.0f);
	static void SetTriggerBounds(IEntityPhysicalProxy *handle, const AABB &bbox);
	static void GetTriggerBounds(IEntityPhysicalProxy *handle, AABB &bbox);
	static int GetPartId0(IEntityPhysicalProxy *handle);
	static void EnableNetworkSerialization(IEntityPhysicalProxy *handle, bool enable);
	static void IgnoreXFormEvent(IEntityPhysicalProxy *handle, bool ignore);
};