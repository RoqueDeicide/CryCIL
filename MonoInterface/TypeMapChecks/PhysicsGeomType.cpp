#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum PhysicsGeomType
{
	PHYS_GEOM_TYPE_NONE_check = -1,
	PHYS_GEOM_TYPE_DEFAULT_check = 0x1000 + 0,
	PHYS_GEOM_TYPE_NO_COLLIDE_check = 0x1000 + 1,
	PHYS_GEOM_TYPE_OBSTRUCT_check = 0x1000 + 2,

	PHYS_GEOM_TYPE_DEFAULT_PROXY_check = 0x1000 + 0x100
};

#define CHECK_ENUM(x) static_assert (PhysicsGeomType::x ## _check == EPhysicsGeomType::x, "EPhysicsGeomType enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(PHYS_GEOM_TYPE_NONE);
	CHECK_ENUM(PHYS_GEOM_TYPE_DEFAULT);
	CHECK_ENUM(PHYS_GEOM_TYPE_NO_COLLIDE);
	CHECK_ENUM(PHYS_GEOM_TYPE_OBSTRUCT);

	CHECK_ENUM(PHYS_GEOM_TYPE_DEFAULT_PROXY);
}