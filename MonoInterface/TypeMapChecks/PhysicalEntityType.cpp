#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum _pe_type
{
	PE_NONE_check = 0,
	PE_STATIC_check = 1,
	PE_RIGID_check = 2,
	PE_WHEELEDVEHICLE_check = 3,
	PE_LIVING_check = 4,
	PE_PARTICLE_check = 5,
	PE_ARTICULATED_check = 6,
	PE_ROPE_check = 7,
	PE_SOFT_check = 8,
	PE_AREA_check = 9
};

#define CHECK_ENUM(x) static_assert (_pe_type::x ## _check == pe_type::x, "pe_type enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(PE_NONE);
	CHECK_ENUM(PE_STATIC);
	CHECK_ENUM(PE_RIGID);
	CHECK_ENUM(PE_WHEELEDVEHICLE);
	CHECK_ENUM(PE_LIVING);
	CHECK_ENUM(PE_PARTICLE);
	CHECK_ENUM(PE_ARTICULATED);
	CHECK_ENUM(PE_ROPE);
	CHECK_ENUM(PE_SOFT);
	CHECK_ENUM(PE_AREA);
}