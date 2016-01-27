#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum AreaProxyFlags
{
	FLAG_NOT_UPDATE_AREA_check = BIT(1),
	FLAG_NOT_SERIALIZE_check = BIT(2)
};

#define CHECK_ENUM(x) static_assert (AreaProxyFlags::x ## _check == IEntityAreaProxy::EAreaProxyFlags::x, "IEntityAreaProxy::EAreaProxyFlags enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(FLAG_NOT_UPDATE_AREA);
	CHECK_ENUM(FLAG_NOT_SERIALIZE);
}

TYPE_MIRROR enum EntityAreaType
{
	ENTITY_AREA_TYPE_SHAPE_check,
	ENTITY_AREA_TYPE_BOX_check,
	ENTITY_AREA_TYPE_SPHERE_check,
	ENTITY_AREA_TYPE_GRAVITYVOLUME_check,
	ENTITY_AREA_TYPE_SOLID_check
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (EntityAreaType::x ## _check == EEntityAreaType::x, "EEntityAreaType enumeration has been changed.")

inline void CheckEntityAreaType()
{
	CHECK_ENUM(ENTITY_AREA_TYPE_SHAPE);
	CHECK_ENUM(ENTITY_AREA_TYPE_BOX);
	CHECK_ENUM(ENTITY_AREA_TYPE_SPHERE);
	CHECK_ENUM(ENTITY_AREA_TYPE_GRAVITYVOLUME);
	CHECK_ENUM(ENTITY_AREA_TYPE_SOLID);
}