#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR struct PhysicalGeometry
{
	IGeometry *pGeom;
	Vec3 Ibody;
	quaternionf q;
	Vec3 origin;
	float V;
	int nRefCount;
	int surface_idx;
	int *pMatMapping;
	int nMats;
	void *pForeignData;

	explicit PhysicalGeometry(phys_geometry &other)
	{
		static_assert(sizeof(PhysicalGeometry) == sizeof(phys_geometry), "phys_geometry structure has been changed.");

		ASSIGN_FIELD(pGeom);
		ASSIGN_FIELD(Ibody);
		ASSIGN_FIELD(q);
		ASSIGN_FIELD(origin);
		ASSIGN_FIELD(V);
		ASSIGN_FIELD(nRefCount);
		ASSIGN_FIELD(surface_idx);
		ASSIGN_FIELD(pMatMapping);
		ASSIGN_FIELD(nMats);
		ASSIGN_FIELD(pForeignData);

		CHECK_TYPE(pGeom);
		CHECK_TYPE(Ibody);
		CHECK_TYPE(q);
		CHECK_TYPE(origin);
		CHECK_TYPE(V);
		CHECK_TYPE(nRefCount);
		CHECK_TYPE(surface_idx);
		CHECK_TYPE(pMatMapping);
		CHECK_TYPE(nMats);
		CHECK_TYPE(pForeignData);
	}
};