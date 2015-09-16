#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum _entity_query_flags
{
	ent_static_check = 1,
	ent_sleeping_rigid_check = 2,
	ent_rigid_check = 4,
	ent_living_check = 8,
	ent_independent_check = 16,
	ent_deleted_check = 128,
	ent_terrain_check = 0x100,
	ent_all_check = ent_static_check | ent_sleeping_rigid_check | ent_rigid_check | ent_living_check | ent_independent_check | ent_terrain_check,
	ent_flagged_only_check = pef_update,
	ent_skip_flagged_check = pef_update * 2, // "flagged" meas has pef_update set
	ent_areas_check = 32,
	ent_triggers_check = 64,
	ent_ignore_noncolliding_check = 0x10000,
	ent_sort_by_mass_check = 0x20000, // sort by mass in ascending order
	ent_allocate_list_check = 0x40000, // if not set, the function will return an internal pointer
	ent_addref_results_check = 0x100000, // will call AddRef on each entity in the list (expecting the caller call Release)
	ent_water_check = 0x200, // can only be used in RayWorldIntersection
	ent_no_ondemand_activation_check = 0x80000, // can only be used in RayWorldIntersection
	ent_delayed_deformations_check = 0x80000 // queues procedural breakage requests; can only be used in SimulateExplosion
};

#define CHECK_ENUM(x) static_assert (_entity_query_flags::x ## _check == entity_query_flags::x, "entity_query_flags enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(ent_static);
	CHECK_ENUM(ent_sleeping_rigid);
	CHECK_ENUM(ent_rigid);
	CHECK_ENUM(ent_living);
	CHECK_ENUM(ent_independent);
	CHECK_ENUM(ent_deleted);
	CHECK_ENUM(ent_terrain);
	CHECK_ENUM(ent_all);
	CHECK_ENUM(ent_flagged_only);
	CHECK_ENUM(ent_skip_flagged);
	CHECK_ENUM(ent_areas);
	CHECK_ENUM(ent_triggers);
	CHECK_ENUM(ent_ignore_noncolliding);
	CHECK_ENUM(ent_sort_by_mass);
	CHECK_ENUM(ent_allocate_list);
	CHECK_ENUM(ent_addref_results);
	CHECK_ENUM(ent_water);
	CHECK_ENUM(ent_no_ondemand_activation);
	CHECK_ENUM(ent_delayed_deformations);
}