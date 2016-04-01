#pragma once

struct RayInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "Ray"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Geometry"; }

	virtual void InitializeInterops() override;

	static int CastRay(Vec3 &origin, Vec3 &direction, int query, uint32 castFlags, ray_hit* hits, int nMaxHits,
					   IPhysicalEntity **entitiesToSkip, int skipEntityCount, SCollisionClass collisionClass);
};