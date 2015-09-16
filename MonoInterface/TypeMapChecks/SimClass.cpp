#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum _sim_class
{
	SC_STATIC_check = 0,
	SC_SLEEPING_RIGID_check = 1,
	SC_ACTIVE_RIGID_check = 2,
	SC_LIVING_check = 3,
	SC_INDEPENDENT_check = 4,
	SC_TRIGGER_check = 6,
	SC_DELETED_check = 7
};

#define CHECK_ENUM(x) static_assert (_sim_class::x ## _check == sim_class::x, "sim_class enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(SC_STATIC);
	CHECK_ENUM(SC_SLEEPING_RIGID);
	CHECK_ENUM(SC_ACTIVE_RIGID);
	CHECK_ENUM(SC_LIVING);
	CHECK_ENUM(SC_INDEPENDENT);
	CHECK_ENUM(SC_TRIGGER);
	CHECK_ENUM(SC_DELETED);
}