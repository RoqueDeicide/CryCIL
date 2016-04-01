#pragma once

#include "IMonoInterface.h"

struct CryEntityAreaProxyInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryEntityAreaProxy"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Logic.EntityProxies"; }

	virtual void InitializeInterops() override;

	static void SetFlags(IEntityAreaProxy *handle, int nAreaProxyFlags);
	static int GetFlags(IEntityAreaProxy *handle);
	static EEntityAreaType GetAreaType(IEntityAreaProxy *handle);
	static void SetPoints(IEntityAreaProxy *handle, Vec3 *vPoints, bool *pabSoundObstructionSegments, int nPointsCount, float fHeight);
	static void SetPointsDefault(IEntityAreaProxy *handle, Vec3 *vPoints, int nPointsCount, float fHeight);
	static void SetBox(IEntityAreaProxy *handle, const Vec3 &min, const Vec3 &max, bool *pabSoundObstructionSides, int nSideCount);
	static void SetSphere(IEntityAreaProxy *handle, const Vec3 &vCenter, float fRadius);
	static void BeginSettingSolid(IEntityAreaProxy *handle, const Matrix34 &worldTM);
	static void AddConvexHullToSolid(IEntityAreaProxy *handle, Vec3 *verticesOfConvexHull, bool bObstruction, int numberOfVertices);
	static void EndSettingSolid(IEntityAreaProxy *handle);
	static int GetPointsCount(IEntityAreaProxy *handle);
	static Vec3 *GetPoints(IEntityAreaProxy *handle);
	static float  GetHeight(IEntityAreaProxy *handle);
	static void GetBox(IEntityAreaProxy *handle, Vec3 &min, Vec3 &max);
	static void GetSphere(IEntityAreaProxy *handle, Vec3 &vCenter, float &fRadius);
	static void SetGravityVolume(IEntityAreaProxy *handle, Vec3 *pPoints, int nNumPoints, float fRadius, float fGravity, bool bDontDisableInvisible, float fFalloff, float fDamping);
	static void SetID(IEntityAreaProxy *handle, int id);
	static int GetID(IEntityAreaProxy *handle);
	static void SetGroup(IEntityAreaProxy *handle, int id);
	static int GetGroup(IEntityAreaProxy *handle);
	static void SetPriority(IEntityAreaProxy *handle, int nPriority);
	static int GetPriority(IEntityAreaProxy *handle);
	static void SetSoundObstructionOnAreaFace(IEntityAreaProxy *handle, uint nFaceIndex, bool bObstructs);
	static void AddEntity(IEntityAreaProxy *handle, EntityId id);
	static void AddEntityGuid(IEntityAreaProxy *handle, EntityGUID guid);
	static void ClearEntities(IEntityAreaProxy *handle);
	static void SetProximity(IEntityAreaProxy *handle, float fProximity);
	static float GetProximity(IEntityAreaProxy *handle);
	static float CalcPointNearDistSq(IEntityAreaProxy *handle, EntityId nEntityID, const Vec3 &Point3d, Vec3 &OnHull3d);
	static float ClosestPointOnHullDistSq(IEntityAreaProxy *handle, EntityId nEntityID, const Vec3 &Point3d, Vec3 &OnHull3d);
	static bool CalcPointWithin(IEntityAreaProxy *handle, EntityId nEntityID, const Vec3 &Point3d, bool bIgnoreHeight);
	static int GetNumberOfEntitiesInArea(IEntityAreaProxy *handle);
	static EntityId GetEntityInAreaByIdx(IEntityAreaProxy *handle, int index);
};