using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	[Flags]
	[CLSCompliant(false)]
	public enum PhysicalizationFlags : uint
	{
		// PE_PARTICLE-specific flags
		Particle_SingleContact = 0x01, // full stop after first contact
		Particle_ConstantOrientation = 0x02, // forces constant orientation
		Particle_NoRoll = 0x04, // 'sliding' mode; entity's 'normal' vector axis will be alinged with the ground normal
		Particle_NoPathAlignment = 0x08, // unless set, entity's y axis will be aligned along the movement trajectory
		Particle_NoSpin = 0x10, // disables spinning while flying
		Particle_NoSelfCollisions = 0x100, // disables collisions with other particles
		Particle_NoImpulse = 0x200, // particle will not add hit impulse (expecting that some other system will)

		// PE_LIVING-specific flags
		// <summary>
		// Push objects during contacts
		// </summary>
		Living_PushObjects = 0x01,
		/// <summary>
		/// Push players during contacts
		/// </summary>
		Living_PushPlayers = 0x02,
		Living_SnapVelocities = 0x04,	// quantizes velocities after each step (was ised in MP for precise deterministic sync)
		Living_LoosenStuckChecks = 0x08, // don't do additional intersection checks after each step (recommended for NPCs to improve performance)
		Living_ReportSlidingContacts = 0x10,	// unless set, 'grazing' contacts are not reported

		// PE_ROPE-specific flags
		Rope_FindiffAttachedVel = 0x01, // approximate velocity of the parent object as v = (pos1-pos0)/time_interval
		Rope_NoSolver = 0x02, // no velocity solver; will rely on stiffness (if set) and positional length enforcement
		Rope_IgnoreAttachments = 0x4, // no collisions with objects the rope is attached to
		Rope_TargetVertexRel0 = 0x08, Rope_TargetVertexRel1 = 0x10, // whether target vertices are set in the parent entity's frame
		Rope_SubdivideSegs = 0x100, // turns on 'dynamic subdivision' mode (only in this mode contacts in a strained state are handled correctly)
		Rope_NoTears = 0x200, // rope will not tear when it reaches its force limit, but stretch
		Rope_Collides = 0x200000, // rope will collide with objects other than the terrain
		Rope_CollidesWithTerrain = 0x400000, // rope will collide with the terrain
		Rope_CollidesWithAttachment = 0x80, // rope will collide with the objects it's attached to even if the other collision flags are not set
		Rope_NoStiffnessWhenColliding = 0x10000000, // rope will use stiffness 0 if it has contacts

		// PE_SOFT-specific flags
		Soft_SkipLongestEdges = 0x01,	// the longest edge in each triangle with not participate in the solver
		Soft_RigidCore = 0x02, // soft body will have an additional rigid body core

		// PE_RIGID-specific flags (note that PE_ARTICULATED and PE_WHEELEDVEHICLE are
		// derived from it)
		Rigid_NoSplashes = 0x04, // will not generate EventPhysCollisions when contacting water
		Rigid_SmallAndFast = 0x100, // entity will trace rays against alive characters; set internally unless overriden

		// PE_ARTICULATED-specific flags
		Articulated_RecordedPhysics = 0x02, // specifies a an entity that contains pre-baked physics simulation

		// PE_WHEELEDVEHICLE-specific flags
		WheeledVehicle_FakeInnerWheels = 0x08, // exclude wheels between the first and the last one from the solver
		// (only wheels with non-0 suspension are considered)

		// general flags
		PartsTraceable = 0x10,	// each entity part will be registered separately in the entity grid
		Disabled = 0x20, // entity will not be simulated
		NeverBreak = 0x40, // entity will not break or deform other objects
		Deforming = 0x80, // entity undergoes a dynamic breaking/deforming
		PushableByPlayers = 0x200, // entity can be pushed by playerd
		Traceable = 0x400, Particle_Traceable = 0x400, Rope_Traceable = 0x400, // entity is registered in the entity grid
		Update = 0x800, // only entities with this flag are updated if ent_flagged_only is used in TimeStep()
		MonitorStateChanges = 0x1000, // generate immediate events for simulation class changed (typically rigid bodies falling asleep)
		MonitorCollisions = 0x2000, // generate immediate events for collisions
		MonitorEnvChanges = 0x4000,	// generate immediate events when something breaks nearby
		NeverAffectTriggers = 0x8000,	// don't generate events when moving through triggers
		Invisible = 0x10000, // will apply certain optimizations for invisible entities
		IgnoreOcean = 0x20000, // entity will ignore global water area
		FixedDamping = 0x40000,	// entity will force its damping onto the entire group
		MonitorPostStep = 0x80000, // entity will generate immediate post step events
		AlwaysNotifyOnDeletion = 0x100000,	// when deleted, entity will awake objects around it even if it's not referenced (has refcount 0)
		OverrideImpulseScale = 0x200000, // entity will ignore breakImpulseScale in PhysVars
		PlayersCanBreak = 0x400000, // playes can break the entiy by bumping into it
		CannotSquashPlayers = 0x10000000,	// entity will never trigger 'squashed' state when colliding with players
		IgnoreAreas = 0x800000, // entity will ignore phys areas (gravity and water)
		LogStateChanges = 0x1000000, // entity will log simulation class change events
		LogCollisions = 0x2000000, // entity will log collision events
		LogEnvChanges = 0x4000000, // entity will log EventPhysEnvChange when something breaks nearby
		LogPoststep = 0x8000000, // entity will log EventPhysPostStep events
	}
}