#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR struct RayHit
{
	float dist;
	IPhysicalEntity *pCollider;
	int ipart;
	int partid;
	short surface_idx;
	short idmatOrg;
	int foreignIdx;
	int iNode;
	Vec3 pt;
	Vec3 n;
	int bTerrain;
	int iPrim;
	ray_hit *next;

	explicit RayHit(ray_hit &other)
	{
		CHECK_TYPES_SIZE(RayHit, ray_hit);

		ASSIGN_FIELD(dist);
		ASSIGN_FIELD(pCollider);
		ASSIGN_FIELD(ipart);
		ASSIGN_FIELD(partid);
		ASSIGN_FIELD(surface_idx);
		ASSIGN_FIELD(idmatOrg);
		ASSIGN_FIELD(foreignIdx);
		ASSIGN_FIELD(iNode);
		ASSIGN_FIELD(pt);
		ASSIGN_FIELD(n);
		ASSIGN_FIELD(bTerrain);
		ASSIGN_FIELD(iPrim);
		ASSIGN_FIELD(next);

		CHECK_TYPE(dist);
		CHECK_TYPE(pCollider);
		CHECK_TYPE(ipart);
		CHECK_TYPE(partid);
		CHECK_TYPE(surface_idx);
		CHECK_TYPE(idmatOrg);
		CHECK_TYPE(foreignIdx);
		CHECK_TYPE(iNode);
		CHECK_TYPE(pt);
		CHECK_TYPE(n);
		CHECK_TYPE(bTerrain);
		CHECK_TYPE(iPrim);
		CHECK_TYPE(next);
	}
};