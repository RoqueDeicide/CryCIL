#include "stdafx.h"
#include "Ray.h"

void RayInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(CastRay);
}

int RayInterop::CastRay(Vec3 &origin, Vec3 &direction, int query, uint32 castFlags, ray_hit* hits, int nMaxHits,
						IPhysicalEntity **entitiesToSkip, int skipEntityCount, SCollisionClass collisionClass)
{
	IPhysicalWorld::SRWIParams params;
	params.Init(origin, direction, query, castFlags, collisionClass, hits, nMaxHits, entitiesToSkip, skipEntityCount);
	return gEnv->pPhysicalWorld->RayWorldIntersection(params);
}
