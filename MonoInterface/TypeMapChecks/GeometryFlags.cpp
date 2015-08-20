#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum GeometryFlags
{ // collisions between parts are checked if (part0->flagsCollider & part1->flags) !=0
	geom_colltype0_check = 0x0001,
	geom_colltype1_check = 0x0002,
	geom_colltype2_check = 0x0004,
	geom_colltype3_check = 0x0008,
	geom_colltype4_check = 0x0010,
	geom_colltype5_check = 0x0020,
	geom_colltype6_check = 0x0040,
	geom_colltype7_check = 0x0080,
	geom_colltype8_check = 0x0100,
	geom_colltype9_check = 0x0200,
	geom_colltype10_check = 0x0400,
	geom_colltype11_check = 0x0800,
	geom_colltype12_check = 0x1000,
	geom_colltype13_check = 0x2000,
	geom_colltype14_check = 0x4000,
	geom_colltype_ray_check = 0x8000, // special colltype used by raytracing by default
	geom_floats_check = 0x10000, // colltype required to apply buoyancy
	geom_proxy_check = 0x20000, // only used in AddGeometry to specify that this geometry should go to pPhysGeomProxy
	geom_structure_changes_check = 0x40000, // part is breaking/deforming
	geom_can_modify_check = 0x80000, // geometry is cloned and is used in this entity only
	geom_squashy_check = 0x100000, // part has 'soft' collisions (used for tree foliage proxy)
	geom_log_interactions_check = 0x200000, // part will post EventPhysBBoxOverlap whenever something happens inside its bbox
	geom_monitor_contacts_check = 0x400000, // part needs collision callback from the solver (used internally)
	geom_manually_breakable_check = 0x800000, // part is breakable outside the physics
	geom_no_coll_response_check = 0x1000000, // collisions are detected and reported, but not processed
	geom_mat_substitutor_check = 0x2000000, // geometry is used to change other collision's material id if the collision point is inside it
	geom_break_approximation_check = 0x4000000, // applies capsule approximation after breaking (used for tree trunks)
	geom_no_particle_impulse_check = 0x8000000, // phys particles don't apply impulses to this part; should be used in flagsCollider
	geom_destroyed_on_break_check = 0x2000000, // should be used in flagsCollider
	// mnemonic group names
	geom_colltype_player_check = geom_colltype1,
	geom_colltype_explosion_check = geom_colltype2,
	geom_colltype_vehicle_check = geom_colltype3,
	geom_colltype_foliage_check = geom_colltype4,
	geom_colltype_debris_check = geom_colltype5,
	geom_colltype_foliage_proxy_check = geom_colltype13,
	geom_colltype_obstruct_check = geom_colltype14,
	geom_colltype_solid_check = 0x0FFF & ~geom_colltype_explosion,
	geom_collides_check = 0xFFFF
};

#define CHECK_ENUM(x) static_assert (GeometryFlags::x ## _check == geom_flags::x, "geom_flags enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(geom_colltype0);
	CHECK_ENUM(geom_colltype1);
	CHECK_ENUM(geom_colltype2);
	CHECK_ENUM(geom_colltype3);
	CHECK_ENUM(geom_colltype4);
	CHECK_ENUM(geom_colltype5);
	CHECK_ENUM(geom_colltype6);
	CHECK_ENUM(geom_colltype7);
	CHECK_ENUM(geom_colltype8);
	CHECK_ENUM(geom_colltype9);
	CHECK_ENUM(geom_colltype10);
	CHECK_ENUM(geom_colltype11);
	CHECK_ENUM(geom_colltype12);
	CHECK_ENUM(geom_colltype13);
	CHECK_ENUM(geom_colltype14);
	CHECK_ENUM(geom_colltype_ray);
	CHECK_ENUM(geom_floats);
	CHECK_ENUM(geom_proxy);
	CHECK_ENUM(geom_structure_changes);
	CHECK_ENUM(geom_can_modify);
	CHECK_ENUM(geom_squashy);
	CHECK_ENUM(geom_log_interactions);
	CHECK_ENUM(geom_monitor_contacts);
	CHECK_ENUM(geom_manually_breakable);
	CHECK_ENUM(geom_no_coll_response);
	CHECK_ENUM(geom_mat_substitutor);
	CHECK_ENUM(geom_break_approximation);
	CHECK_ENUM(geom_no_particle_impulse);
	CHECK_ENUM(geom_destroyed_on_break);
	CHECK_ENUM(geom_colltype_player);
	CHECK_ENUM(geom_colltype_explosion);
	CHECK_ENUM(geom_colltype_vehicle);
	CHECK_ENUM(geom_colltype_foliage);
	CHECK_ENUM(geom_colltype_debris);
	CHECK_ENUM(geom_colltype_foliage_proxy);
	CHECK_ENUM(geom_colltype_obstruct);
	CHECK_ENUM(geom_colltype_solid);
	CHECK_ENUM(geom_collides);
}