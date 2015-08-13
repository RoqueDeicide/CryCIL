#include "stdafx.h"

#include "CheckingBasics.h"

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

#define CHECK_ENUM(x) static_assert (_phentity_flags::x ## _check == phentity_flags::x, "phentity_flags enumeration has been changed.")

inline void Check()
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