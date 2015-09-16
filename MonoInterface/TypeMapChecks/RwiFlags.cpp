#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum RwiFlags
{
	rwi_ignore_terrain_holes_check = 0x20,
	rwi_ignore_noncolliding_check = 0x40,
	rwi_ignore_back_faces_check = 0x80,
	rwi_ignore_solid_back_faces_check = 0x100,
	rwi_pierceability_mask_check = 0x0F,
	rwi_pierceability0_check = 0,
	rwi_stop_at_pierceable_check = 0x0F,
	rwi_separate_important_hits_check = sf_important,
	rwi_colltype_bit_check = 16,
	rwi_colltype_any_check = 0x400,
	rwi_queue_check = 0x800,
	rwi_force_pierceable_noncoll_check = 0x1000,
	rwi_update_last_hit_check = 0x4000,
	rwi_any_hit_check = 0x8000
};

#define CHECK_ENUM(x) static_assert (RwiFlags::x ## _check == rwi_flags::x, "rwi_flags enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(rwi_ignore_terrain_holes);
	CHECK_ENUM(rwi_ignore_noncolliding);
	CHECK_ENUM(rwi_ignore_back_faces);
	CHECK_ENUM(rwi_ignore_solid_back_faces);
	CHECK_ENUM(rwi_pierceability_mask);
	CHECK_ENUM(rwi_pierceability0);
	CHECK_ENUM(rwi_stop_at_pierceable);
	CHECK_ENUM(rwi_separate_important_hits);
	CHECK_ENUM(rwi_colltype_bit);
	CHECK_ENUM(rwi_colltype_any);
	CHECK_ENUM(rwi_queue);
	CHECK_ENUM(rwi_force_pierceable_noncoll);
	CHECK_ENUM(rwi_update_last_hit);
	CHECK_ENUM(rwi_any_hit);
}