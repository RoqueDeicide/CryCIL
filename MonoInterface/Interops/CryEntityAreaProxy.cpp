#include "stdafx.h"

#include "CryEntityAreaProxy.h"

void CryEntityAreaProxyInterop::InitializeInterops()
{
	REGISTER_METHOD(SetFlags);
	REGISTER_METHOD(GetFlags);
	REGISTER_METHOD(GetAreaType);
	REGISTER_METHOD(SetPoints);
	REGISTER_METHOD(SetPointsDefault);
	REGISTER_METHOD(SetBox);
	REGISTER_METHOD(SetSphere);
	REGISTER_METHOD(BeginSettingSolid);
	REGISTER_METHOD(AddConvexHullToSolid);
	REGISTER_METHOD(EndSettingSolid);
	REGISTER_METHOD(GetPointsCount);
	REGISTER_METHOD(GetPoints);
	REGISTER_METHOD(GetHeight);
	REGISTER_METHOD(GetBox);
	REGISTER_METHOD(GetSphere);
	REGISTER_METHOD(SetGravityVolume);
	REGISTER_METHOD(SetID);
	REGISTER_METHOD(GetID);
	REGISTER_METHOD(SetGroup);
	REGISTER_METHOD(GetGroup);
	REGISTER_METHOD(SetPriority);
	REGISTER_METHOD(GetPriority);
	REGISTER_METHOD(SetSoundObstructionOnAreaFace);
	REGISTER_METHOD(AddEntity);
	REGISTER_METHOD(AddEntityGuid);
	REGISTER_METHOD(ClearEntities);
	REGISTER_METHOD(SetProximity);
	REGISTER_METHOD(GetProximity);
	REGISTER_METHOD(CalcPointNearDistSq);
	REGISTER_METHOD(ClosestPointOnHullDistSq);
	REGISTER_METHOD(CalcPointWithin);
	REGISTER_METHOD(GetNumberOfEntitiesInArea);
	REGISTER_METHOD(GetEntityInAreaByIdx);
}

void CryEntityAreaProxyInterop::SetFlags(IEntityAreaProxy *handle, int nAreaProxyFlags)
{
	handle->SetFlags(nAreaProxyFlags);
}

int CryEntityAreaProxyInterop::GetFlags(IEntityAreaProxy *handle)
{
	return handle->GetFlags();
}

EEntityAreaType CryEntityAreaProxyInterop::GetAreaType(IEntityAreaProxy *handle)
{
	return handle->GetAreaType();
}

void CryEntityAreaProxyInterop::SetPoints(IEntityAreaProxy *handle, Vec3 *vPoints, bool *pabSoundObstructionSegments, int nPointsCount, float fHeight)
{
	handle->SetPoints(vPoints, pabSoundObstructionSegments, nPointsCount, fHeight);
}

void CryEntityAreaProxyInterop::SetPointsDefault(IEntityAreaProxy *handle, Vec3 *vPoints, int nPointsCount, float fHeight)
{
	bool *t = new bool[nPointsCount];
	handle->SetPoints(vPoints, t, nPointsCount, fHeight);
	delete t;
}

void CryEntityAreaProxyInterop::SetBox(IEntityAreaProxy *handle, const Vec3 &min, const Vec3 &max, bool *pabSoundObstructionSides, int nSideCount)
{
	handle->SetBox(min, max, pabSoundObstructionSides, nSideCount);
}

void CryEntityAreaProxyInterop::SetSphere(IEntityAreaProxy *handle, const Vec3 &vCenter, float fRadius)
{
	handle->SetSphere(vCenter, fRadius);
}

void CryEntityAreaProxyInterop::BeginSettingSolid(IEntityAreaProxy *handle, const Matrix34 &worldTM)
{
	handle->BeginSettingSolid(worldTM);
}

void CryEntityAreaProxyInterop::AddConvexHullToSolid(IEntityAreaProxy *handle, Vec3 *verticesOfConvexHull, bool bObstruction, int numberOfVertices)
{
	handle->AddConvexHullToSolid(verticesOfConvexHull, bObstruction, numberOfVertices);
}

void CryEntityAreaProxyInterop::EndSettingSolid(IEntityAreaProxy *handle)
{
	handle->EndSettingSolid();
}

int CryEntityAreaProxyInterop::GetPointsCount(IEntityAreaProxy *handle)
{
	return handle->GetPointsCount();
}

Vec3 *CryEntityAreaProxyInterop::GetPoints(IEntityAreaProxy *handle)
{
	return const_cast<Vec3 *>(handle->GetPoints());
}

float CryEntityAreaProxyInterop::GetHeight(IEntityAreaProxy *handle)
{
	return handle->GetHeight();
}

void CryEntityAreaProxyInterop::GetBox(IEntityAreaProxy *handle, Vec3 &min, Vec3 &max)
{
	handle->GetBox(min, max);
}

void CryEntityAreaProxyInterop::GetSphere(IEntityAreaProxy *handle, Vec3 &vCenter, float &fRadius)
{
	handle->GetSphere(vCenter, fRadius);
}

void CryEntityAreaProxyInterop::SetGravityVolume(IEntityAreaProxy *handle, Vec3 *pPoints, int nNumPoints, float fRadius, float fGravity, bool bDontDisableInvisible, float fFalloff, float fDamping)
{
	handle->SetGravityVolume(pPoints, nNumPoints, fRadius, fGravity, bDontDisableInvisible, fFalloff, fDamping);
}

void CryEntityAreaProxyInterop::SetID(IEntityAreaProxy *handle, int id)
{
	handle->SetID(id);
}

int CryEntityAreaProxyInterop::GetID(IEntityAreaProxy *handle)
{
	return handle->GetID();
}

void CryEntityAreaProxyInterop::SetGroup(IEntityAreaProxy *handle, int id)
{
	handle->SetGroup(id);
}

int CryEntityAreaProxyInterop::GetGroup(IEntityAreaProxy *handle)
{
	return handle->GetGroup();
}

void CryEntityAreaProxyInterop::SetPriority(IEntityAreaProxy *handle, int nPriority)
{
	handle->SetPriority(nPriority);
}

int CryEntityAreaProxyInterop::GetPriority(IEntityAreaProxy *handle)
{
	return handle->GetPriority();
}

void CryEntityAreaProxyInterop::SetSoundObstructionOnAreaFace(IEntityAreaProxy *handle, uint nFaceIndex, bool bObstructs)
{
	handle->SetSoundObstructionOnAreaFace(nFaceIndex, bObstructs);
}

void CryEntityAreaProxyInterop::AddEntity(IEntityAreaProxy *handle, EntityId id)
{
	handle->AddEntity(id);
}

void CryEntityAreaProxyInterop::AddEntityGuid(IEntityAreaProxy *handle, EntityGUID guid)
{
	handle->AddEntity(guid);
}

void CryEntityAreaProxyInterop::ClearEntities(IEntityAreaProxy *handle)
{
	handle->ClearEntities();
}

void CryEntityAreaProxyInterop::SetProximity(IEntityAreaProxy *handle, float fProximity)
{
	handle->SetProximity(fProximity);
}

float CryEntityAreaProxyInterop::GetProximity(IEntityAreaProxy *handle)
{
	return handle->GetProximity();
}

float CryEntityAreaProxyInterop::CalcPointNearDistSq(IEntityAreaProxy *handle, EntityId nEntityID, const Vec3 &Point3d, Vec3 &OnHull3d)
{
	return handle->CalcPointNearDistSq(nEntityID, Point3d, OnHull3d);
}

float CryEntityAreaProxyInterop::ClosestPointOnHullDistSq(IEntityAreaProxy *handle, EntityId nEntityID, const Vec3 &Point3d, Vec3 &OnHull3d)
{
	return handle->ClosestPointOnHullDistSq(nEntityID, Point3d, OnHull3d);
}

bool CryEntityAreaProxyInterop::CalcPointWithin(IEntityAreaProxy *handle, EntityId nEntityID, const Vec3 &Point3d, bool bIgnoreHeight)
{
	return handle->CalcPointWithin(nEntityID, Point3d, bIgnoreHeight);
}

int CryEntityAreaProxyInterop::GetNumberOfEntitiesInArea(IEntityAreaProxy *handle)
{
	return handle->GetNumberOfEntitiesInArea();
}

EntityId CryEntityAreaProxyInterop::GetEntityInAreaByIdx(IEntityAreaProxy *handle, int index)
{
	return handle->GetEntityInAreaByIdx(index);
}
