#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum ResRefreshFlags
{
	FRO_SHADERS_check = 1,
	FRO_SHADERTEXTURES_check = 2,
	FRO_TEXTURES_check = 4,
	FRO_GEOMETRY_check = 8,
	FRO_FORCERELOAD_check = 0x10
};

#define CHECK_ENUM(x) static_assert (ResRefreshFlags::x ## _check == x, "Render resource refresh defines were changed.")

inline void Check()
{
	CHECK_ENUM(FRO_SHADERS);
	CHECK_ENUM(FRO_SHADERTEXTURES);
	CHECK_ENUM(FRO_TEXTURES);
	CHECK_ENUM(FRO_GEOMETRY);
	CHECK_ENUM(FRO_FORCERELOAD);
}