#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum _constrflags
{
	local_frames_check = 1,
	world_frames_check = 2,
	local_frames_part_check = 4,
	constraint_inactive_check = 0x100,
	constraint_ignore_buddy_check = 0x200,
	constraint_line_check = 0x400,
	constraint_plane_check = 0x800,
	constraint_free_position_check = 0x1000,
	constraint_no_rotation_check = 0x2000,
	constraint_no_enforcement_check = 0x4000,
	constraint_no_tears_check = 0x8000
};

#define CHECK_ENUM(x) static_assert (_constrflags::x ## _check == constrflags::x, "constrflags enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(local_frames);
	CHECK_ENUM(world_frames);
	CHECK_ENUM(local_frames_part);
	CHECK_ENUM(constraint_inactive);
	CHECK_ENUM(constraint_ignore_buddy);
	CHECK_ENUM(constraint_line);
	CHECK_ENUM(constraint_plane);
	CHECK_ENUM(constraint_free_position);
	CHECK_ENUM(constraint_no_rotation);
	CHECK_ENUM(constraint_no_enforcement);
	CHECK_ENUM(constraint_no_tears);
}