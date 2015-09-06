#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum SurfaceFlags
{
	sf_pierceable_mask_check = 0x0F,
	sf_max_pierceable_check = 0x0F,
	sf_important_check = 0x200,
	sf_manually_breakable_check = 0x400,
	sf_matbreakable_bit_check = 16
};

#define CHECK_ENUM(x) static_assert (SurfaceFlags::x ## _check == surface_flags::x, "surface_flags enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(sf_pierceable_mask);
	CHECK_ENUM(sf_max_pierceable);
	CHECK_ENUM(sf_important);
	CHECK_ENUM(sf_manually_breakable);
	CHECK_ENUM(sf_matbreakable_bit);
}