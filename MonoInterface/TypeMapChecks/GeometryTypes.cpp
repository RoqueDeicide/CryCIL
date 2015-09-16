#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum GeometryTypes
{
	GEOM_TRIMESH_check = primitives::triangle::type,
	GEOM_HEIGHTFIELD_check = primitives::heightfield::type,
	GEOM_CYLINDER_check = primitives::cylinder::type,
	GEOM_CAPSULE_check = primitives::capsule::type,
	GEOM_RAY_check = primitives::ray::type,
	GEOM_SPHERE_check = primitives::sphere::type,
	GEOM_BOX_check = primitives::box::type,
	GEOM_VOXELGRID_check = primitives::voxelgrid::type
};

#define CHECK_ENUM(x) static_assert (GeometryTypes::x ## _check == geomtypes::x, "geomtypes enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(GEOM_TRIMESH);
	CHECK_ENUM(GEOM_HEIGHTFIELD);
	CHECK_ENUM(GEOM_CYLINDER);
	CHECK_ENUM(GEOM_CAPSULE);
	CHECK_ENUM(GEOM_RAY);
	CHECK_ENUM(GEOM_SPHERE);
	CHECK_ENUM(GEOM_BOX);
	CHECK_ENUM(GEOM_VOXELGRID);
}