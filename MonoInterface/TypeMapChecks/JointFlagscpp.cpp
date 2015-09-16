#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum _joint_flags
{
	angle0_locked_check = 1,
	all_angles_locked_check = 7,
	angle0_limit_reached_check = 010,
	angle0_auto_kd_check = 0100,
	joint_no_gravity_check = 01000,
	joint_isolated_accelerations_check = 02000,
	joint_expand_hinge_check = 04000,
	angle0_gimbal_locked_check = 010000,
	joint_dashpot_reached_check = 0100000,
	joint_ignore_impulses_check = 0200000
};

#define CHECK_ENUM(x) static_assert (_joint_flags::x ## _check == joint_flags::x, "joint_flags enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(angle0_locked);
	CHECK_ENUM(all_angles_locked);
	CHECK_ENUM(angle0_limit_reached);
	CHECK_ENUM(angle0_auto_kd);
	CHECK_ENUM(joint_no_gravity);
	CHECK_ENUM(joint_isolated_accelerations);
	CHECK_ENUM(joint_expand_hinge);
	CHECK_ENUM(angle0_gimbal_locked);
	CHECK_ENUM(joint_dashpot_reached);
	CHECK_ENUM(joint_ignore_impulses);
}