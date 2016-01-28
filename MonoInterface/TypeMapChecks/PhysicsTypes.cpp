#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR struct EventPhysicsCollision
{
	EventPhys *next;
	int idval;
	IPhysicalEntity *pEntity[2];
	void *pForeignData[2];
	int iForeignData[2];
	int idCollider;	// in addition to pEntity[1]
	Vec3 pt; // contact point in world coordinates
	Vec3 n;	// contact normal
	Vec3 vloc[2];	// velocities at the contact point
	float mass[2];
	int partid[2];
	short idmat[2];
	short iPrim[2];
	float penetration; // contact's penetration depth
	float normImpulse; // impulse applied by the solver to resolve the collision
	float radius;	// some characteristic size of the contact area
	void *pEntContact; // reserved for internal use
	char deferredState; // EventPhysCollisionState
	char deferredResult; // stores the result returned by the deferred event
	float fDecalPlacementTestMaxSize;

	EventPhysicsCollision(EventPhysCollision other)
	{
		static_assert(sizeof(EventPhysicsCollision) == sizeof(EventPhysCollision), "EventPhysCollision has been changed.");

		ASSIGN_FIELD(next);
		ASSIGN_FIELD(idval);
		ASSIGN_FIELD(pEntity[0]);
		ASSIGN_FIELD(pEntity[1]);
		ASSIGN_FIELD(pForeignData[0]);
		ASSIGN_FIELD(pForeignData[1]);
		ASSIGN_FIELD(iForeignData[0]);
		ASSIGN_FIELD(iForeignData[1]);
		ASSIGN_FIELD(idCollider);
		ASSIGN_FIELD(pt);
		ASSIGN_FIELD(n);
		ASSIGN_FIELD(vloc[0]);
		ASSIGN_FIELD(mass[0]);
		ASSIGN_FIELD(partid[0]);
		ASSIGN_FIELD(idmat[0]);
		ASSIGN_FIELD(iPrim[0]);
		ASSIGN_FIELD(vloc[1]);
		ASSIGN_FIELD(mass[1]);
		ASSIGN_FIELD(partid[1]);
		ASSIGN_FIELD(idmat[1]);
		ASSIGN_FIELD(iPrim[1]);
		ASSIGN_FIELD(penetration);
		ASSIGN_FIELD(normImpulse);
		ASSIGN_FIELD(radius);
		ASSIGN_FIELD(pEntContact);
		ASSIGN_FIELD(deferredState);
		ASSIGN_FIELD(deferredResult);
		ASSIGN_FIELD(fDecalPlacementTestMaxSize);

		CHECK_TYPE(next);
		CHECK_TYPE(idval);
		CHECK_TYPE(pEntity);
		CHECK_TYPE(pForeignData);
		CHECK_TYPE(iForeignData);
		CHECK_TYPE(idCollider);
		CHECK_TYPE(pt);
		CHECK_TYPE(n);
		CHECK_TYPE(vloc);
		CHECK_TYPE(mass);
		CHECK_TYPE(partid);
		CHECK_TYPE(idmat);
		CHECK_TYPE(iPrim);
		CHECK_TYPE(penetration);
		CHECK_TYPE(normImpulse);
		CHECK_TYPE(radius);
		CHECK_TYPE(pEntContact);
		CHECK_TYPE(deferredState);
		CHECK_TYPE(deferredResult);
		CHECK_TYPE(fDecalPlacementTestMaxSize);
	}
};

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

inline void CheckGeometryFlags()
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

TYPE_MIRROR enum GeometryTypes
{
	GEOM_TRIMESH_check = primitives::triangle::type,
	GEOM_HEIGHTFIELD_check = primitives::heightfield::type,
	GEOM_CYLINDER_check = primitives::cylinder::type,
	GEOM_CAPSULE_check = primitives::capsule::type,
	GEOM_RAY_check = primitives::ray::type,
	GEOM_SPHERE_check = primitives::sphere::type,
	GEOM_BOX_check = primitives::box::type,
	GEOM_VOXELGRID_check = primitives::voxelgrid::type
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (GeometryTypes::x ## _check == geomtypes::x, "geomtypes enumeration has been changed.")

inline void CheckGeometryTypes()
{
	CHECK_ENUM(GEOM_TRIMESH);
	CHECK_ENUM(GEOM_HEIGHTFIELD);
	CHECK_ENUM(GEOM_CYLINDER);
	CHECK_ENUM(GEOM_CAPSULE);
	CHECK_ENUM(GEOM_RAY);
	CHECK_ENUM(GEOM_SPHERE);
	CHECK_ENUM(GEOM_BOX);
	CHECK_ENUM(GEOM_VOXELGRID);
}

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

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (_joint_flags::x ## _check == joint_flags::x, "joint_flags enumeration has been changed.")

inline void Check_joint_flags()
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

TYPE_MIRROR struct _bop_newvtx
{
	int idx;
	int iBvtx;
	int idxTri[2];

	explicit _bop_newvtx(bop_newvtx &other)
	{
		CHECK_TYPES_SIZE(_bop_newvtx, bop_newvtx);

		ASSIGN_FIELD(idx);
		ASSIGN_FIELD(iBvtx);
		ASSIGN_FIELD(idxTri[0]);
		ASSIGN_FIELD(idxTri[1]);

		CHECK_TYPE(idx);
		CHECK_TYPE(iBvtx);
		CHECK_TYPE(idxTri);
	}
};

TYPE_MIRROR struct _bop_newtri
{
	int idxNew;
	int iop;
	int idxOrg;
	int iVtx[3];
	float areaOrg;
	Vec3 area[3];

	explicit _bop_newtri(bop_newtri &other)
	{
		CHECK_TYPES_SIZE(_bop_newtri, bop_newtri);

		ASSIGN_FIELD(idxNew);
		ASSIGN_FIELD(iop);
		ASSIGN_FIELD(idxOrg);
		ASSIGN_FIELD(iVtx[0]);
		ASSIGN_FIELD(iVtx[1]);
		ASSIGN_FIELD(iVtx[1]);
		ASSIGN_FIELD(areaOrg);
		ASSIGN_FIELD(area[0]);
		ASSIGN_FIELD(area[1]);
		ASSIGN_FIELD(area[1]);

		CHECK_TYPE(idxNew);
		CHECK_TYPE(iop);
		CHECK_TYPE(idxOrg);
		CHECK_TYPE(iVtx);
		CHECK_TYPE(areaOrg);
		CHECK_TYPE(area);
	}
};

TYPE_MIRROR struct _bop_vtxweld
{
	int ivtxDst : 16;
	int ivtxWelded : 16;

	explicit _bop_vtxweld(bop_vtxweld &other)
	{
		CHECK_TYPES_SIZE(_bop_vtxweld, bop_vtxweld);

		ASSIGN_FIELD(ivtxDst);
		ASSIGN_FIELD(ivtxWelded);

		CHECK_TYPE(ivtxDst);
		CHECK_TYPE(ivtxWelded);
	}
};

TYPE_MIRROR struct _bop_TJfix
{
	int iABC;
	int iACJ;
	int iCA;
	int iAC;
	int iTJvtx;

	explicit _bop_TJfix(bop_TJfix &other)
	{
		CHECK_TYPES_SIZE(_bop_TJfix, bop_TJfix);

		ASSIGN_FIELD(iABC);
		ASSIGN_FIELD(iACJ);
		ASSIGN_FIELD(iCA);
		ASSIGN_FIELD(iAC);
		ASSIGN_FIELD(iTJvtx);

		CHECK_TYPE(iABC);
		CHECK_TYPE(iACJ);
		CHECK_TYPE(iCA);
		CHECK_TYPE(iAC);
		CHECK_TYPE(iTJvtx);
	}
};

TYPE_MIRROR enum _phentity_flags
{
	// PE_PARTICLE-specific flags
	particle_single_contact_check = 0x01, // full stop after first contact
	particle_constant_orientation_check = 0x02, // forces constant orientation
	particle_no_roll_check = 0x04, // 'sliding' mode; entity's 'normal' vector axis will be alinged with the ground normal
	particle_no_path_alignment_check = 0x08, // unless set, entity's y axis will be aligned along the movement trajectory
	particle_no_spin_check = 0x10, // disables spinning while flying
	particle_no_self_collisions_check = 0x100, // disables collisions with other particles
	particle_no_impulse_check = 0x200, // particle will not add hit impulse (expecting that some other system will) 

	// PE_LIVING-specific flags
	lef_push_objects_check = 0x01,
	lef_push_players_check = 0x02,	// push objects and players during contacts
	lef_snap_velocities_check = 0x04,	// quantizes velocities after each step (was ised in MP for precise deterministic sync)
	lef_loosen_stuck_checks_check = 0x08, // don't do additional intersection checks after each step (recommended for NPCs to improve performance)
	lef_report_sliding_contacts_check = 0x10,	// unless set, 'grazing' contacts are not reported 

	// PE_ROPE-specific flags
	rope_findiff_attached_vel_check = 0x01, // approximate velocity of the parent object as v = (pos1-pos0)/time_interval
	rope_no_solver_check = 0x02, // no velocity solver; will rely on stiffness (if set) and positional length enforcement
	rope_ignore_attachments_check = 0x4, // no collisions with objects the rope is attached to
	rope_target_vtx_rel0_check = 0x08,
	rope_target_vtx_rel1_check = 0x10, // whether target vertices are set in the parent entity's frame
	rope_subdivide_segs_check = 0x100, // turns on 'dynamic subdivision' mode (only in this mode contacts in a strained state are handled correctly)
	rope_no_tears_check = 0x200, // rope will not tear when it reaches its force limit, but stretch
	rope_collides_check = 0x200000, // rope will collide with objects other than the terrain
	rope_collides_with_terrain_check = 0x400000, // rope will collide with the terrain
	rope_collides_with_attachment_check = 0x80, // rope will collide with the objects it's attached to even if the other collision flags are not set
	rope_no_stiffness_when_colliding_check = 0x10000000, // rope will use stiffness 0 if it has contacts

	// PE_SOFT-specific flags
	se_skip_longest_edges_check = 0x01,	// the longest edge in each triangle with not participate in the solver
	se_rigid_core_check = 0x02, // soft body will have an additional rigid body core

	// PE_RIGID-specific flags (note that PE_ARTICULATED and PE_WHEELEDVEHICLE are derived from it)
	ref_use_simple_solver_check = 0x01,	// use penalty-based solver (obsolete)
	ref_no_splashes_check = 0x04, // will not generate EventPhysCollisions when contacting water
	ref_checksum_received_check = 0x04,
	ref_checksum_outofsync_check = 0x08, // obsolete
	ref_small_and_fast_check = 0x100, // entity will trace rays against alive characters; set internally unless overriden

	// PE_ARTICULATED-specific flags
	aef_recorded_physics_check = 0x02, // specifies a an entity that contains pre-baked physics simulation

	// PE_WHEELEDVEHICLE-specific flags
	wwef_fake_inner_wheels_check = 0x08, // exclude wheels between the first and the last one from the solver
	// (only wheels with non-0 suspension are considered)

	// general flags
	pef_parts_traceable_check = 0x10,	// each entity part will be registered separately in the entity grid
	pef_disabled_check = 0x20, // entity will not be simulated
	pef_never_break_check = 0x40, // entity will not break or deform other objects
	pef_deforming_check = 0x80, // entity undergoes a dynamic breaking/deforming
	pef_pushable_by_players_check = 0x200, // entity can be pushed by playerd	
	pef_traceable_check = 0x400,
	particle_traceable_check = 0x400,
	rope_traceable_check = 0x400, // entity is registered in the entity grid
	pef_update_check = 0x800, // only entities with this flag are updated if ent_flagged_only is used in TimeStep()
	pef_monitor_state_changes_check = 0x1000, // generate immediate events for simulation class changed (typically rigid bodies falling asleep)
	pef_monitor_collisions_check = 0x2000, // generate immediate events for collisions
	pef_monitor_env_changes_check = 0x4000,	// generate immediate events when something breaks nearby
	pef_never_affect_triggers_check = 0x8000,	// don't generate events when moving through triggers
	pef_invisible_check = 0x10000, // will apply certain optimizations for invisible entities
	pef_ignore_ocean_check = 0x20000, // entity will ignore global water area
	pef_fixed_damping_check = 0x40000,	// entity will force its damping onto the entire group
	pef_monitor_poststep_check = 0x80000, // entity will generate immediate post step events
	pef_always_notify_on_deletion_check = 0x100000,	// when deleted, entity will awake objects around it even if it's not referenced (has refcount 0)
	pef_override_impulse_scale_check = 0x200000, // entity will ignore breakImpulseScale in PhysVars
	pef_players_can_break_check = 0x400000, // playes can break the entiy by bumping into it
	pef_cannot_squash_players_check = 0x10000000,	// entity will never trigger 'squashed' state when colliding with players
	pef_ignore_areas_check = 0x800000, // entity will ignore phys areas (gravity and water)
	pef_log_state_changes_check = 0x1000000, // entity will log simulation class change events
	pef_log_collisions_check = 0x2000000, // entity will log collision events
	pef_log_env_changes_check = 0x4000000, // entity will log EventPhysEnvChange when something breaks nearby
	pef_log_poststep_check = 0x8000000, // entity will log EventPhysPostStep events
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (_phentity_flags::x ## _check == phentity_flags::x, "phentity_flags enumeration has been changed.")

inline void Check_phentity_flags()
{
	CHECK_ENUM(particle_single_contact);
	CHECK_ENUM(particle_constant_orientation);
	CHECK_ENUM(particle_no_roll);
	CHECK_ENUM(particle_no_path_alignment);
	CHECK_ENUM(particle_no_spin);
	CHECK_ENUM(particle_no_self_collisions);
	CHECK_ENUM(particle_no_impulse);

	CHECK_ENUM(lef_push_objects);
	CHECK_ENUM(lef_push_players);
	CHECK_ENUM(lef_snap_velocities);
	CHECK_ENUM(lef_loosen_stuck_checks);
	CHECK_ENUM(lef_report_sliding_contacts);

	CHECK_ENUM(rope_findiff_attached_vel);
	CHECK_ENUM(rope_no_solver);
	CHECK_ENUM(rope_ignore_attachments);
	CHECK_ENUM(rope_target_vtx_rel0);
	CHECK_ENUM(rope_target_vtx_rel1);
	CHECK_ENUM(rope_subdivide_segs);
	CHECK_ENUM(rope_no_tears);
	CHECK_ENUM(rope_collides);
	CHECK_ENUM(rope_collides_with_terrain);
	CHECK_ENUM(rope_collides_with_attachment);
	CHECK_ENUM(rope_no_stiffness_when_colliding);

	CHECK_ENUM(se_skip_longest_edges);
	CHECK_ENUM(se_rigid_core);

	CHECK_ENUM(ref_use_simple_solver);
	CHECK_ENUM(ref_no_splashes);
	CHECK_ENUM(ref_checksum_received);
	CHECK_ENUM(ref_checksum_outofsync);
	CHECK_ENUM(ref_small_and_fast);

	CHECK_ENUM(aef_recorded_physics);

	CHECK_ENUM(wwef_fake_inner_wheels);

	CHECK_ENUM(pef_parts_traceable);
	CHECK_ENUM(pef_disabled);
	CHECK_ENUM(pef_never_break);
	CHECK_ENUM(pef_deforming);
	CHECK_ENUM(pef_pushable_by_players);
	CHECK_ENUM(pef_traceable);
	CHECK_ENUM(particle_traceable);
	CHECK_ENUM(rope_traceable);
	CHECK_ENUM(pef_update);
	CHECK_ENUM(pef_monitor_state_changes);
	CHECK_ENUM(pef_monitor_collisions);
	CHECK_ENUM(pef_monitor_env_changes);
	CHECK_ENUM(pef_never_affect_triggers);
	CHECK_ENUM(pef_invisible);
	CHECK_ENUM(pef_ignore_ocean);
	CHECK_ENUM(pef_fixed_damping);
	CHECK_ENUM(pef_monitor_poststep);
	CHECK_ENUM(pef_always_notify_on_deletion);
	CHECK_ENUM(pef_override_impulse_scale);
	CHECK_ENUM(pef_players_can_break);
	CHECK_ENUM(pef_cannot_squash_players);
	CHECK_ENUM(pef_ignore_areas);
	CHECK_ENUM(pef_log_state_changes);
	CHECK_ENUM(pef_log_collisions);
	CHECK_ENUM(pef_log_env_changes);
	CHECK_ENUM(pef_log_poststep);
}

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

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (_pe_type::x ## _check == pe_type::x, "pe_type enumeration has been changed.")

inline void Check_pe_type()
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

TYPE_MIRROR struct PhysicalGeometry
{
	IGeometry *pGeom;
	Vec3 Ibody;
	quaternionf q;
	Vec3 origin;
	float V;
	int nRefCount;
	int surface_idx;
	int *pMatMapping;
	int nMats;
	void *pForeignData;

	explicit PhysicalGeometry(phys_geometry &other)
	{
		static_assert(sizeof(PhysicalGeometry) == sizeof(phys_geometry), "phys_geometry structure has been changed.");

		ASSIGN_FIELD(pGeom);
		ASSIGN_FIELD(Ibody);
		ASSIGN_FIELD(q);
		ASSIGN_FIELD(origin);
		ASSIGN_FIELD(V);
		ASSIGN_FIELD(nRefCount);
		ASSIGN_FIELD(surface_idx);
		ASSIGN_FIELD(pMatMapping);
		ASSIGN_FIELD(nMats);
		ASSIGN_FIELD(pForeignData);

		CHECK_TYPE(pGeom);
		CHECK_TYPE(Ibody);
		CHECK_TYPE(q);
		CHECK_TYPE(origin);
		CHECK_TYPE(V);
		CHECK_TYPE(nRefCount);
		CHECK_TYPE(surface_idx);
		CHECK_TYPE(pMatMapping);
		CHECK_TYPE(nMats);
		CHECK_TYPE(pForeignData);
	}
};

TYPE_MIRROR enum PhysicsForeignIds
{
	PHYS_FOREIGN_ID_TERRAIN_check = 0,
	PHYS_FOREIGN_ID_STATIC_check = 1,
	PHYS_FOREIGN_ID_ENTITY_check = 2,
	PHYS_FOREIGN_ID_FOLIAGE_check = 3,
	PHYS_FOREIGN_ID_ROPE_check = 4,
	PHYS_FOREIGN_ID_SOUND_OBSTRUCTION_check = 5,
	PHYS_FOREIGN_ID_SOUND_PROXY_OBSTRUCTION_check = 6,
	PHYS_FOREIGN_ID_SOUND_REVERB_OBSTRUCTION_check = 7,
	PHYS_FOREIGN_ID_WATERVOLUME_check = 8,
	PHYS_FOREIGN_ID_BREAKABLE_GLASS_check = 9,
	PHYS_FOREIGN_ID_BREAKABLE_GLASS_FRAGMENT_check = 10,
	PHYS_FOREIGN_ID_RIGID_PARTICLE_check = 11,
	PHYS_FOREIGN_ID_RESERVED1_check = 12,
	PHYS_FOREIGN_ID_RAGDOLL_check = 13,

	PHYS_FOREIGN_ID_USER_check = 100, // All user defined foreign ids should start from this enum.
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (PhysicsForeignIds::x ## _check == EPhysicsForeignIds::x, "EPhysicsForeignIds enumeration has been changed.")

inline void CheckPhysicsForeignIds()
{
	CHECK_ENUM(PHYS_FOREIGN_ID_TERRAIN);
	CHECK_ENUM(PHYS_FOREIGN_ID_STATIC);
	CHECK_ENUM(PHYS_FOREIGN_ID_ENTITY);
	CHECK_ENUM(PHYS_FOREIGN_ID_FOLIAGE);
	CHECK_ENUM(PHYS_FOREIGN_ID_ROPE);
	CHECK_ENUM(PHYS_FOREIGN_ID_SOUND_OBSTRUCTION);
	CHECK_ENUM(PHYS_FOREIGN_ID_SOUND_PROXY_OBSTRUCTION);
	CHECK_ENUM(PHYS_FOREIGN_ID_SOUND_REVERB_OBSTRUCTION);
	CHECK_ENUM(PHYS_FOREIGN_ID_WATERVOLUME);
	CHECK_ENUM(PHYS_FOREIGN_ID_BREAKABLE_GLASS);
	CHECK_ENUM(PHYS_FOREIGN_ID_BREAKABLE_GLASS_FRAGMENT);
	CHECK_ENUM(PHYS_FOREIGN_ID_RIGID_PARTICLE);
	CHECK_ENUM(PHYS_FOREIGN_ID_RESERVED1);
	CHECK_ENUM(PHYS_FOREIGN_ID_RAGDOLL);

	CHECK_ENUM(PHYS_FOREIGN_ID_USER);
}

TYPE_MIRROR enum PhysicsGeomType
{
	PHYS_GEOM_TYPE_NONE_check = -1,
	PHYS_GEOM_TYPE_DEFAULT_check = 0x1000 + 0,
	PHYS_GEOM_TYPE_NO_COLLIDE_check = 0x1000 + 1,
	PHYS_GEOM_TYPE_OBSTRUCT_check = 0x1000 + 2,

	PHYS_GEOM_TYPE_DEFAULT_PROXY_check = 0x1000 + 0x100
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (PhysicsGeomType::x ## _check == EPhysicsGeomType::x, "EPhysicsGeomType enumeration has been changed.")

inline void CheckPhysicsGeomType()
{
	CHECK_ENUM(PHYS_GEOM_TYPE_NONE);
	CHECK_ENUM(PHYS_GEOM_TYPE_DEFAULT);
	CHECK_ENUM(PHYS_GEOM_TYPE_NO_COLLIDE);
	CHECK_ENUM(PHYS_GEOM_TYPE_OBSTRUCT);

	CHECK_ENUM(PHYS_GEOM_TYPE_DEFAULT_PROXY);
}

TYPE_MIRROR enum PhysicsMeshFlags
{
	mesh_shared_vtx_check = 1,
	mesh_shared_idx_check = 2,
	mesh_shared_mats_check = 4,
	mesh_shared_foreign_idx_check = 8,
	mesh_shared_normals_check = 0x10,
	mesh_OBB_check = 0x20,
	mesh_AABB_check = 0x40,
	mesh_SingleBB_check = 0x80,
	mesh_AABB_rotated_check = 0x40000,
	mesh_VoxelGrid_check = 0x80000,
	mesh_multicontact0_check = 0x100,
	mesh_multicontact1_check = 0x200,
	mesh_multicontact2_check = 0x400,
	mesh_approx_cylinder_check = 0x800,
	mesh_approx_box_check = 0x1000,
	mesh_approx_sphere_check = 0x2000,
	mesh_approx_capsule_check = 0x200000,
	mesh_keep_vtxmap_check = 0x8000,
	mesh_keep_vtxmap_for_saving_check = 0x10000,
	mesh_no_vtx_merge_check = 0x20000,
	mesh_always_static_check = 0x100000,
	mesh_full_serialization_check = 0x400000,
	mesh_transient_check = 0x800000,
	mesh_no_booleans_check = 0x1000000,
	mesh_AABB_plane_optimise_check = 0x4000,
	mesh_no_filter_check = 0x2000000
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (PhysicsMeshFlags::x ## _check == meshflags::x, "meshflags enumeration has been changed.")

inline void CheckPhysicsMeshFlags()
{
	CHECK_ENUM(mesh_shared_vtx);
	CHECK_ENUM(mesh_shared_idx);
	CHECK_ENUM(mesh_shared_mats);
	CHECK_ENUM(mesh_shared_foreign_idx);
	CHECK_ENUM(mesh_shared_normals);
	CHECK_ENUM(mesh_OBB);
	CHECK_ENUM(mesh_AABB);
	CHECK_ENUM(mesh_SingleBB);
	CHECK_ENUM(mesh_AABB_rotated);
	CHECK_ENUM(mesh_VoxelGrid);
	CHECK_ENUM(mesh_multicontact0);
	CHECK_ENUM(mesh_multicontact1);
	CHECK_ENUM(mesh_multicontact2);
	CHECK_ENUM(mesh_approx_cylinder);
	CHECK_ENUM(mesh_approx_box);
	CHECK_ENUM(mesh_approx_sphere);
	CHECK_ENUM(mesh_approx_capsule);
	CHECK_ENUM(mesh_keep_vtxmap);
	CHECK_ENUM(mesh_keep_vtxmap_for_saving);
	CHECK_ENUM(mesh_no_vtx_merge);
	CHECK_ENUM(mesh_always_static);
	CHECK_ENUM(mesh_full_serialization);
	CHECK_ENUM(mesh_transient);
	CHECK_ENUM(mesh_no_booleans);
	CHECK_ENUM(mesh_AABB_plane_optimise);
	CHECK_ENUM(mesh_no_filter);
}

TYPE_MIRROR enum PE_Params
{
	ePE_params_pos_check = 0,
	ePE_player_dimensions_check = 1,
	ePE_params_car_check = 2,
	ePE_params_particle_check = 3,
	ePE_player_dynamics_check = 4,
	ePE_params_joint_check = 5,
	ePE_params_part_check = 6,
	ePE_params_sensors_check = 7,
	ePE_params_articulated_body_check = 8,
	ePE_params_outer_entity_check = 9,
	ePE_simulation_params_check = 10,
	ePE_params_foreign_data_check = 11,
	ePE_params_buoyancy_check = 12,
	ePE_params_rope_check = 13,
	ePE_params_bbox_check = 14,
	ePE_params_flags_check = 15,
	ePE_params_wheel_check = 16,
	ePE_params_softbody_check = 17,
	ePE_params_area_check = 18,
	ePE_tetrlattice_params_check = 19,
	ePE_params_ground_plane_check = 20,
	ePE_params_structural_joint_check = 21,
	ePE_params_waterman_check = 22,
	ePE_params_timeout_check = 23,
	ePE_params_skeleton_check = 24,
	ePE_params_structural_initial_velocity_check = 25,
	ePE_params_collision_class_check = 26,

	ePE_Params_Count_check
};

TYPE_MIRROR enum PE_Action
{
	ePE_action_move_check = 1,
	ePE_action_impulse_check = 2,
	ePE_action_drive_check = 3,
	ePE_action_reset_check = 4,
	ePE_action_add_constraint_check = 5,
	ePE_action_update_constraint_check = 6,
	ePE_action_register_coll_event_check = 7,
	ePE_action_awake_check = 8,
	ePE_action_remove_all_parts_check = 9,
	ePE_action_set_velocity_check = 10,
	ePE_action_attach_points_check = 11,
	ePE_action_target_vtx_check = 12,
	ePE_action_reset_part_mtx_check = 13,
	ePE_action_notify_check = 14,
	ePE_action_auto_part_detachment_check = 15,
	ePE_action_move_parts_check = 16,
	ePE_action_batch_parts_update_check = 17,
	ePE_action_slice_check = 18,

	ePE_Action_Count_check
};

TYPE_MIRROR enum PE_GeomParams
{
	ePE_geomparams_check = 0,
	ePE_cargeomparams_check = 1,
	ePE_articgeomparams_check = 2,

	ePE_GeomParams_Count_check
};

TYPE_MIRROR enum PE_Status
{
	ePE_status_pos_check = 1,
	ePE_status_living_check = 2,
	ePE_status_vehicle_check = 4,
	ePE_status_wheel_check = 5,
	ePE_status_joint_check = 6,
	ePE_status_awake_check = 7,
	ePE_status_dynamics_check = 8,
	ePE_status_collisions_check = 9,
	ePE_status_id_check = 10,
	ePE_status_timeslices_check = 11,
	ePE_status_nparts_check = 12,
	ePE_status_contains_point_check = 13,
	ePE_status_rope_check = 14,
	ePE_status_vehicle_abilities_check = 15,
	ePE_status_placeholder_check = 16,
	ePE_status_softvtx_check = 17,
	ePE_status_sensors_check = 18,
	ePE_status_sample_contact_area_check = 19,
	ePE_status_caps_check = 20,
	ePE_status_check_stance_check = 21,
	ePE_status_waterman_check = 22,
	ePE_status_area_check = 23,
	ePE_status_extent_check = 24,
	ePE_status_random_check = 25,
	ePE_status_constraint_check = 26,
	ePE_status_netpos_check = 27,

	ePE_Status_Count_check
};

#define CHECK_ENUM_TYPED(t, x) static_assert (t::x ## _check == E##t::x, "E"#t" enumeration has been changed.")

inline void CheckPhysicsTypeEnumerations()
{
#undef CHECK_ENUM
#define CHECK_ENUM(x) CHECK_ENUM_TYPED(PE_Params, x)

	CHECK_ENUM(ePE_params_pos);
	CHECK_ENUM(ePE_player_dimensions);
	CHECK_ENUM(ePE_params_car);
	CHECK_ENUM(ePE_params_particle);
	CHECK_ENUM(ePE_player_dynamics);
	CHECK_ENUM(ePE_params_joint);
	CHECK_ENUM(ePE_params_part);
	CHECK_ENUM(ePE_params_sensors);
	CHECK_ENUM(ePE_params_articulated_body);
	CHECK_ENUM(ePE_params_outer_entity);
	CHECK_ENUM(ePE_simulation_params);
	CHECK_ENUM(ePE_params_foreign_data);
	CHECK_ENUM(ePE_params_buoyancy);
	CHECK_ENUM(ePE_params_rope);
	CHECK_ENUM(ePE_params_bbox);
	CHECK_ENUM(ePE_params_flags);
	CHECK_ENUM(ePE_params_wheel);
	CHECK_ENUM(ePE_params_softbody);
	CHECK_ENUM(ePE_params_area);
	CHECK_ENUM(ePE_tetrlattice_params);
	CHECK_ENUM(ePE_params_ground_plane);
	CHECK_ENUM(ePE_params_structural_joint);
	CHECK_ENUM(ePE_params_waterman);
	CHECK_ENUM(ePE_params_timeout);
	CHECK_ENUM(ePE_params_skeleton);
	CHECK_ENUM(ePE_params_structural_initial_velocity);
	CHECK_ENUM(ePE_params_collision_class);

	CHECK_ENUM(ePE_Params_Count);

#undef CHECK_ENUM
#define CHECK_ENUM(x) CHECK_ENUM_TYPED(PE_Action, x)

	CHECK_ENUM(ePE_action_move);
	CHECK_ENUM(ePE_action_impulse);
	CHECK_ENUM(ePE_action_drive);
	CHECK_ENUM(ePE_action_reset);
	CHECK_ENUM(ePE_action_add_constraint);
	CHECK_ENUM(ePE_action_update_constraint);
	CHECK_ENUM(ePE_action_register_coll_event);
	CHECK_ENUM(ePE_action_awake);
	CHECK_ENUM(ePE_action_remove_all_parts);
	CHECK_ENUM(ePE_action_set_velocity);
	CHECK_ENUM(ePE_action_attach_points);
	CHECK_ENUM(ePE_action_target_vtx);
	CHECK_ENUM(ePE_action_reset_part_mtx);
	CHECK_ENUM(ePE_action_notify);
	CHECK_ENUM(ePE_action_auto_part_detachment);
	CHECK_ENUM(ePE_action_move_parts);
	CHECK_ENUM(ePE_action_batch_parts_update);
	CHECK_ENUM(ePE_action_slice);

	CHECK_ENUM(ePE_Action_Count);

#undef CHECK_ENUM
#define CHECK_ENUM(x) CHECK_ENUM_TYPED(PE_GeomParams, x)

	CHECK_ENUM(ePE_geomparams);
	CHECK_ENUM(ePE_cargeomparams);
	CHECK_ENUM(ePE_articgeomparams);

	CHECK_ENUM(ePE_GeomParams_Count);

#undef CHECK_ENUM
#define CHECK_ENUM(x) CHECK_ENUM_TYPED(PE_Status, x)

	CHECK_ENUM(ePE_status_pos);
	CHECK_ENUM(ePE_status_living);
	CHECK_ENUM(ePE_status_vehicle);
	CHECK_ENUM(ePE_status_wheel);
	CHECK_ENUM(ePE_status_joint);
	CHECK_ENUM(ePE_status_awake);
	CHECK_ENUM(ePE_status_dynamics);
	CHECK_ENUM(ePE_status_collisions);
	CHECK_ENUM(ePE_status_id);
	CHECK_ENUM(ePE_status_timeslices);
	CHECK_ENUM(ePE_status_nparts);
	CHECK_ENUM(ePE_status_contains_point);
	CHECK_ENUM(ePE_status_rope);
	CHECK_ENUM(ePE_status_vehicle_abilities);
	CHECK_ENUM(ePE_status_placeholder);
	CHECK_ENUM(ePE_status_softvtx);
	CHECK_ENUM(ePE_status_sensors);
	CHECK_ENUM(ePE_status_sample_contact_area);
	CHECK_ENUM(ePE_status_caps);
	CHECK_ENUM(ePE_status_check_stance);
	CHECK_ENUM(ePE_status_waterman);
	CHECK_ENUM(ePE_status_area);
	CHECK_ENUM(ePE_status_extent);
	CHECK_ENUM(ePE_status_random);
	CHECK_ENUM(ePE_status_constraint);
	CHECK_ENUM(ePE_status_netpos);

	CHECK_ENUM(ePE_Status_Count);
}

#include <primitives.h>

TYPE_MIRROR struct primitive
{};

TYPE_MIRROR struct box : primitive
{
	enum entype { type = 0 };
	Matrix33 Basis;	// v_box = Basis*v_world; Basis = Rotation.T()
	int bOriented;
	Vec3 center;
	Vec3 size;

	explicit box(primitives::box &other)
	{
		static_assert(sizeof(box) == sizeof(primitives::box), "primitives::box structure has been changed.");

		ASSIGN_FIELD(Basis);
		ASSIGN_FIELD(bOriented);
		ASSIGN_FIELD(center);
		ASSIGN_FIELD(size);

		CHECK_TYPE(Basis);
		CHECK_TYPE(bOriented);
		CHECK_TYPE(center);
		CHECK_TYPE(size);
	}
};

TYPE_MIRROR struct triangle : primitive
{
	enum entype { type = 1 };
	Vec3 pt[3];
	Vec3 n;

	triangle()
	{}

	explicit triangle(primitives::triangle &other)
	{
		static_assert(sizeof(triangle) == sizeof(primitives::triangle),
					  "primitives::triangle structure has been changed.");

		ASSIGN_FIELD(pt[0]);
		ASSIGN_FIELD(pt[1]);
		ASSIGN_FIELD(pt[2]);
		ASSIGN_FIELD(n);

		CHECK_TYPE(pt);
		CHECK_TYPE(n);
	}
};

TYPE_MIRROR struct indexed_triangle : triangle
{
	int idx;

	explicit indexed_triangle(primitives::indexed_triangle &other)
	{
		static_assert(sizeof(indexed_triangle) == sizeof(primitives::indexed_triangle),
					  "primitives::indexed_triangle structure has been changed.");

		ASSIGN_FIELD(idx);

		CHECK_TYPE(idx);
	}
};

typedef float(*getHeightCallback)(int ix, int iy);
typedef unsigned char(*getSurfTypeCallback)(int ix, int iy);

TYPE_MIRROR struct grid : primitive
{
	Matrix33 Basis;
	int bOriented;
	Vec3 origin;
	vector2df step, stepr;
	vector2di size;
	vector2di stride;
	int bCyclic;

	grid()
		: Basis(IDENTITY)
		, bOriented(0)
		, origin(ZERO)
		, step(ZERO)
		, stepr(ZERO)
		, size(ZERO)
		, stride(ZERO)
		, bCyclic(0)
	{
	}

	explicit grid(primitives::grid &other)
	{
		static_assert(sizeof(grid) == sizeof(primitives::grid), "primitives::grid structure has been changed.");

		ASSIGN_FIELD(Basis);
		ASSIGN_FIELD(bOriented);
		ASSIGN_FIELD(origin);
		ASSIGN_FIELD(step);
		ASSIGN_FIELD(stepr);
		ASSIGN_FIELD(size);
		ASSIGN_FIELD(stride);
		ASSIGN_FIELD(bCyclic);

		CHECK_TYPE(Basis);
		CHECK_TYPE(bOriented);
		CHECK_TYPE(origin);
		CHECK_TYPE(step);
		CHECK_TYPE(stepr);
		CHECK_TYPE(size);
		CHECK_TYPE(stride);
		CHECK_TYPE(bCyclic);
	}
};

TYPE_MIRROR struct heightfield : grid
{
	enum entype { type = 2 };
	float heightscale;
	unsigned short typemask, heightmask;
	int typehole;
	int typepower;
	getHeightCallback fpGetHeightCallback;
	getSurfTypeCallback fpGetSurfTypeCallback;

	explicit heightfield(primitives::heightfield &other)
	{
		static_assert(sizeof(heightfield) == sizeof(primitives::heightfield),
					  "primitives::heightfield structure has been changed.");

		ASSIGN_FIELD(heightscale);
		ASSIGN_FIELD(typemask);
		ASSIGN_FIELD(heightmask);
		ASSIGN_FIELD(typehole);
		ASSIGN_FIELD(typepower);
		ASSIGN_FIELD(fpGetHeightCallback);
		ASSIGN_FIELD(fpGetSurfTypeCallback);

		CHECK_TYPE(heightscale);
		CHECK_TYPE(typemask);
		CHECK_TYPE(heightmask);
		CHECK_TYPE(typehole);
		CHECK_TYPE(typepower);
		CHECK_TYPE(fpGetHeightCallback);
		CHECK_TYPE(fpGetSurfTypeCallback);
	}
};

TYPE_MIRROR struct ray : primitive
{
	enum entype { type = 3 };
	Vec3 origin;
	Vec3 dir;


	explicit ray(primitives::ray &other)
	{
		static_assert(sizeof(ray) == sizeof(primitives::ray), "primitives::ray structure has been changed.");

		ASSIGN_FIELD(origin);
		ASSIGN_FIELD(dir);

		CHECK_TYPE(origin);
		CHECK_TYPE(dir);
	}
};

TYPE_MIRROR struct sphere : primitive
{
	enum entype { type = 4 };
	Vec3 center;
	float r;


	explicit sphere(primitives::sphere &other)
	{
		static_assert(sizeof(sphere) == sizeof(primitives::sphere), "primitives::sphere structure has been changed.");

		ASSIGN_FIELD(center);
		ASSIGN_FIELD(r);

		CHECK_TYPE(center);
		CHECK_TYPE(r);
	}
};

TYPE_MIRROR struct cylinder : primitive
{
	enum entype { type = 5 };
	Vec3 center;
	Vec3 axis;
	float r, hh;

	cylinder()
		: center(ZERO)
		, axis(ZERO)
		, r(0)
		, hh(0)
	{
	}

	explicit cylinder(primitives::cylinder &other)
	{
		static_assert(sizeof(cylinder) == sizeof(primitives::cylinder),
					  "primitives::cylinder structure has been changed.");

		ASSIGN_FIELD(center);
		ASSIGN_FIELD(axis);
		ASSIGN_FIELD(r);
		ASSIGN_FIELD(hh);

		CHECK_TYPE(center);
		CHECK_TYPE(axis);
		CHECK_TYPE(r);
		CHECK_TYPE(hh);
	}
};

TYPE_MIRROR struct capsule : cylinder
{
	enum entype { type = 6 };

	explicit capsule(primitives::capsule &)
	{
		static_assert(sizeof(capsule) == sizeof(primitives::capsule), "primitives::capsule structure has been changed.");
	}
};

TYPE_MIRROR struct grid3d : primitive
{
	Matrix33 Basis;
	int bOriented;
	Vec3 origin;
	Vec3 step, stepr;
	Vec3i size;
	Vec3i stride;

	grid3d()
		: Basis(IDENTITY)
		, bOriented(0)
		, origin(ZERO)
		, step(ZERO)
		, stepr(ZERO)
		, size(ZERO)
		, stride(ZERO)
	{}

	explicit grid3d(primitives::grid3d &other)
	{
		static_assert(sizeof(grid3d) == sizeof(primitives::grid3d), "primitives::grid3d structure has been changed.");

		ASSIGN_FIELD(Basis);
		ASSIGN_FIELD(bOriented);
		ASSIGN_FIELD(origin);
		ASSIGN_FIELD(step);
		ASSIGN_FIELD(stepr);
		ASSIGN_FIELD(size);
		ASSIGN_FIELD(stride);

		CHECK_TYPE(Basis);
		CHECK_TYPE(bOriented);
		CHECK_TYPE(origin);
		CHECK_TYPE(step);
		CHECK_TYPE(stepr);
		CHECK_TYPE(size);
		CHECK_TYPE(stride);
	}
};

TYPE_MIRROR struct voxelgrid : grid3d
{
	enum entype { type = 7 };
	Matrix33 R;
	Vec3 offset;
	float scale, rscale;
	strided_pointer<Vec3> pVtx;
	index_t *pIndices;
	Vec3 *pNormals;
	char *pIds;
	int *pCellTris;
	int *pTriBuf;

	explicit voxelgrid(primitives::voxelgrid &other)
	{
		static_assert(sizeof(voxelgrid) == sizeof(primitives::voxelgrid),
					  "primitives::voxelgrid structure has been changed.");

		ASSIGN_FIELD(R);
		ASSIGN_FIELD(offset);
		ASSIGN_FIELD(scale);
		ASSIGN_FIELD(rscale);
		ASSIGN_FIELD(pVtx);
		ASSIGN_FIELD(pIndices);
		ASSIGN_FIELD(pNormals);
		ASSIGN_FIELD(pIds);
		ASSIGN_FIELD(pCellTris);
		ASSIGN_FIELD(pTriBuf);

		CHECK_TYPE(R);
		CHECK_TYPE(offset);
		CHECK_TYPE(scale);
		CHECK_TYPE(rscale);
		CHECK_TYPE(pVtx);
		CHECK_TYPE(pIndices);
		CHECK_TYPE(pNormals);
		CHECK_TYPE(pIds);
		CHECK_TYPE(pCellTris);
		CHECK_TYPE(pTriBuf);
	}
};

TYPE_MIRROR struct plane : primitive
{
	enum entype { type = 8 };
	Vec3 n;
	Vec3 origin;

	plane()
	{}

	explicit plane(primitives::plane &other)
	{
		static_assert(sizeof(plane) == sizeof(primitives::plane), "primitives::plane structure has been changed.");

		ASSIGN_FIELD(n);
		ASSIGN_FIELD(origin);

		CHECK_TYPE(n);
		CHECK_TYPE(origin);
	}
};

TYPE_MIRROR struct coord_plane : plane
{
	Vec3 axes[2];

	explicit coord_plane(primitives::coord_plane &other)
	{
		static_assert(sizeof(coord_plane) == sizeof(primitives::coord_plane),
					  "primitives::coord_plane structure has been changed.");

		ASSIGN_FIELD(axes[0]);
		ASSIGN_FIELD(axes[1]);

		CHECK_TYPE(axes);
	}
};

TYPE_MIRROR struct _mesh_data : primitives::primitive
{
	index_t *pIndices;
	char *pMats;
	int *pForeignIdx;
	strided_pointer<Vec3> pVertices;
	Vec3 *pNormals;
	int *pVtxMap;
	trinfo *pTopology;
	int nTris, nVertices;
	mesh_island *pIslands;
	int nIslands;
	tri2isle *pTri2Island;
	int flags;

	explicit _mesh_data(mesh_data &other)
	{
		static_assert(sizeof(_mesh_data) == sizeof(mesh_data), "mesh_data structure has been changed.");

		ASSIGN_FIELD(pIndices);
		ASSIGN_FIELD(pMats);
		ASSIGN_FIELD(pForeignIdx);
		ASSIGN_FIELD(pVertices);
		ASSIGN_FIELD(pNormals);
		ASSIGN_FIELD(pVtxMap);
		ASSIGN_FIELD(pTopology);
		ASSIGN_FIELD(nTris);
		ASSIGN_FIELD(nVertices);
		ASSIGN_FIELD(pIslands);
		ASSIGN_FIELD(nIslands);
		ASSIGN_FIELD(pTri2Island);
		ASSIGN_FIELD(flags);

		CHECK_TYPE(pIndices);
		CHECK_TYPE(pMats);
		CHECK_TYPE(pForeignIdx);
		CHECK_TYPE(pVertices);
		CHECK_TYPE(pNormals);
		CHECK_TYPE(pVtxMap);
		CHECK_TYPE(pTopology);
		CHECK_TYPE(nTris);
		CHECK_TYPE(nVertices);
		CHECK_TYPE(pIslands);
		CHECK_TYPE(nIslands);
		CHECK_TYPE(pTri2Island);
		CHECK_TYPE(flags);
	}
};

TYPE_MIRROR struct RayHit
{
	float dist;
	IPhysicalEntity *pCollider;
	int ipart;
	int partid;
	short surface_idx;
	short idmatOrg;
	int foreignIdx;
	int iNode;
	Vec3 pt;
	Vec3 n;
	int bTerrain;
	int iPrim;
	ray_hit *next;

	explicit RayHit(ray_hit &other)
	{
		CHECK_TYPES_SIZE(RayHit, ray_hit);

		ASSIGN_FIELD(dist);
		ASSIGN_FIELD(pCollider);
		ASSIGN_FIELD(ipart);
		ASSIGN_FIELD(partid);
		ASSIGN_FIELD(surface_idx);
		ASSIGN_FIELD(idmatOrg);
		ASSIGN_FIELD(foreignIdx);
		ASSIGN_FIELD(iNode);
		ASSIGN_FIELD(pt);
		ASSIGN_FIELD(n);
		ASSIGN_FIELD(bTerrain);
		ASSIGN_FIELD(iPrim);
		ASSIGN_FIELD(next);

		CHECK_TYPE(dist);
		CHECK_TYPE(pCollider);
		CHECK_TYPE(ipart);
		CHECK_TYPE(partid);
		CHECK_TYPE(surface_idx);
		CHECK_TYPE(idmatOrg);
		CHECK_TYPE(foreignIdx);
		CHECK_TYPE(iNode);
		CHECK_TYPE(pt);
		CHECK_TYPE(n);
		CHECK_TYPE(bTerrain);
		CHECK_TYPE(iPrim);
		CHECK_TYPE(next);
	}
};

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

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (RwiFlags::x ## _check == rwi_flags::x, "rwi_flags enumeration has been changed.")

inline void CheckRwiFlags()
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

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (_sim_class::x ## _check == sim_class::x, "sim_class enumeration has been changed.")

inline void Check_sim_class()
{
	CHECK_ENUM(SC_STATIC);
	CHECK_ENUM(SC_SLEEPING_RIGID);
	CHECK_ENUM(SC_ACTIVE_RIGID);
	CHECK_ENUM(SC_LIVING);
	CHECK_ENUM(SC_INDEPENDENT);
	CHECK_ENUM(SC_TRIGGER);
	CHECK_ENUM(SC_DELETED);
}

TYPE_MIRROR enum SurfaceFlags
{
	sf_pierceable_mask_check = 0x0F,
	sf_max_pierceable_check = 0x0F,
	sf_important_check = 0x200,
	sf_manually_breakable_check = 0x400,
	sf_matbreakable_bit_check = 16
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (SurfaceFlags::x ## _check == surface_flags::x, "surface_flags enumeration has been changed.")

inline void CheckSurfaceFlags()
{
	CHECK_ENUM(sf_pierceable_mask);
	CHECK_ENUM(sf_max_pierceable);
	CHECK_ENUM(sf_important);
	CHECK_ENUM(sf_manually_breakable);
	CHECK_ENUM(sf_matbreakable_bit);
}

TYPE_MIRROR enum UpdateMeshReason
{
	ReasonExplosion_check,
	ReasonFracture_check,
	ReasonRequest_check,
	ReasonDeform_check
};

TYPE_MIRROR enum CreatePartReason
{
	ReasonMeshSplit_check,
	ReasonJointsBroken_check
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (UpdateMeshReason::x ## _check == EventPhysUpdateMesh::x, "EventPhysUpdateMesh::reason enumeration has been changed.")
#define CHECK_ENUM2(x) static_assert (CreatePartReason::x ## _check == EventPhysCreateEntityPart::x, "EventPhysCreateEntityPart::reason enumeration has been changed.")

inline void CheckUpdateMeshCreatePartReason()
{
	CHECK_ENUM(ReasonExplosion);
	CHECK_ENUM(ReasonFracture);
	CHECK_ENUM(ReasonRequest);
	CHECK_ENUM(ReasonDeform);

	CHECK_ENUM2(ReasonMeshSplit);
	CHECK_ENUM2(ReasonJointsBroken);
}

TYPE_MIRROR struct wave_sim
{
	float timeStep;
	float waveSpeed;
	float simDepth;
	float heightLimit;
	float resistance;
	float dampingCenter;
	float dampingRim;
	float minhSpread;
	float minVel;

	explicit wave_sim(params_wavesim &other)
	{
		static_assert(sizeof(wave_sim) == sizeof(params_wavesim), "params_wavesim structure has been changed.");

		ASSIGN_FIELD(timeStep);
		ASSIGN_FIELD(waveSpeed);
		ASSIGN_FIELD(simDepth);
		ASSIGN_FIELD(heightLimit);
		ASSIGN_FIELD(resistance);
		ASSIGN_FIELD(dampingCenter);
		ASSIGN_FIELD(dampingRim);
		ASSIGN_FIELD(minhSpread);
		ASSIGN_FIELD(minVel);

		CHECK_TYPE(timeStep);
		CHECK_TYPE(waveSpeed);
		CHECK_TYPE(simDepth);
		CHECK_TYPE(heightLimit);
		CHECK_TYPE(resistance);
		CHECK_TYPE(dampingCenter);
		CHECK_TYPE(dampingRim);
		CHECK_TYPE(minhSpread);
		CHECK_TYPE(minVel);
	}
};

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

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (_constrflags::x ## _check == constrflags::x, "constrflags enumeration has been changed.")

inline void Check_constrflags()
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

TYPE_MIRROR enum _snapshot_flags
{
	ssf_compensate_time_diff_check=1,
	ssf_checksum_only_check=2,
	ssf_no_update_check=4
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (_snapshot_flags::x ## _check == snapshot_flags::x, "snapshot_flags enumeration has been changed.")

inline void Check_snapshot_flags()
{
	CHECK_ENUM(ssf_compensate_time_diff);
	CHECK_ENUM(ssf_checksum_only);
	CHECK_ENUM(ssf_no_update);
}