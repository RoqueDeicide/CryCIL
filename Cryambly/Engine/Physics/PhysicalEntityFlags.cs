using System;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of flags that can be set for physical entities.
	/// </summary>
	[Flags]
	public enum PhysicalEntityFlags
	{
		/// <summary>
		/// When set, specifies that this physical entity will lose all kinetic energy when makes first
		/// contact with another one.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Particle"/>.</remarks>
		ParticleSingleContact = 0x01,
		/// <summary>
		/// When set, specifies that this physical entity doesn't change its orientation via physical
		/// interactions.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Particle"/>.</remarks>
		ParticleConstantOrientation = 0x02,
		/// <summary>
		/// When set, specifies that this physical entity is moving in 'sliding' mode: it's normal vector
		/// is aligned with a ground normal.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Particle"/>.</remarks>
		ParticleNoRoll = 0x04,
		/// <summary>
		/// When set, specifies that this physical entity's forward axis doesn't need to be aligned with
		/// the movement trajectory.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Particle"/>.</remarks>
		ParticleNoPathAlignment = 0x08,
		/// <summary>
		/// When set, specifies that this physical entity won't change its orientation during flight.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Particle"/>.</remarks>
		ParticleNoSpin = 0x10,
		/// <summary>
		/// When set, specifies that this physical entity doesn't collide with other particles.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Particle"/>.</remarks>
		ParticleNoSelfCollisions = 0x100,
		/// <summary>
		/// When set, specifies that this physical entity will not add hit impulse to other entities when
		/// it collides with them.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Particle"/>.</remarks>
		ParticleNoImpulse = 0x200,

		/// <summary>
		/// When set, specifies that this physical entity will push other objects away when it collides
		/// with them.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Living"/>.</remarks>
		LivingEntityPushObjects = 0x01,
		/// <summary>
		/// When set, specifies that this physical entity will push players away when it collides with
		/// them.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Living"/>.</remarks>
		LivingEntityPushPlayers = 0x02,
		/// <summary>
		/// When set, specifies that this physical entity has its velocity quantized after each step in
		/// physics simulation (was used in MultiPlayer before for precise deterministic synchronization).
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Living"/>.</remarks>
		LivingEntitySnapVelocities = 0x04,
		/// <summary>
		/// When set, specifies that this physical entity doesn't have additional intersection checks done
		/// for it after each step in physics simulation (recommended for NPCs to improve performance, but
		/// increases likelihood of them getting stuck).
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Living"/>.</remarks>
		LivingEntityLoosenStuckChecks = 0x08,
		/// <summary>
		/// When set, specifies that this physical entity will react to 'grazing' contacts with other
		/// entities.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Living"/>.</remarks>
		LivingEntityReportSlidingContacts = 0x10,

		/// <summary>
		/// When set, specifies that this physical entity approximates velocity of the parent object using
		/// formula: <c>v = (pos1 - pos0) / time_span;</c>
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Rope"/>.</remarks>
		RopeApproximateAttachedVelocity = 0x01,
		/// <summary>
		/// When set, specifies that this physical entity doesn't use velocity solver and instead relies on
		/// stiffness (if set) and positional length enforcement.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Rope"/>.</remarks>
		RopeNoSolver = 0x02,
		/// <summary>
		/// When set, specifies that this physical entity doesn't collide with objects it's attached to.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Rope"/>.</remarks>
		RopeIgnoreAttachments = 0x4,
		/// <summary>
		/// When set, specifies that this physical entity targets a vertex in the first attached entity.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Rope"/>.</remarks>
		RopeTargetVertexRelative0 = 0x08,
		/// <summary>
		/// When set, specifies that this physical entity targets a vertex in the second attached entity.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Rope"/>.</remarks>
		RopeTargetVertexRelative1 = 0x10,
		/// <summary>
		/// When set, specifies that this physical entity has its segments dynamically subdivided (allows
		/// contacts in strained state to be handled correctly).
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Rope"/>.</remarks>
		RopeSubdivideSegments = 0x100,
		/// <summary>
		/// When set, specifies that this physical entity won't tear when it reaches its force limit and
		/// will stretch instead.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Rope"/>.</remarks>
		RopeNoTears = 0x200,
		/// <summary>
		/// When set, specifies that this physical entity will collide with objects other then terrain.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Rope"/>.</remarks>
		RopeCollides = 0x200000,
		/// <summary>
		/// When set, specifies that this physical entity will collide with terrain.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Rope"/>.</remarks>
		RopeCollidesWithTerrain = 0x400000,
		/// <summary>
		/// When set, specifies that this physical entity will collide with the objects it's attached to
		/// even if the other collision flags are not set.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Rope"/>.</remarks>
		RopeCollidesWithAttachment = 0x80,
		/// <summary>
		/// When set, specifies that this physical entity uses stiffness value of 0 when processing
		/// contacts.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Rope"/>.</remarks>
		RopeNoStiffnessWhenColliding = 0x10000000,

		/// <summary>
		/// When set, specifies that this physical entity will have the longest edge in each triangle
		/// skipped by the solver (increase performance at the cost of precision).
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Soft"/>.</remarks>
		SoftEntitySkipLongestEdges = 0x01,
		/// <summary>
		/// When set, specifies that this physical entity has an additional rigid body core.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Soft"/>.</remarks>
		SoftEntityRigidCore = 0x02,

		/// <summary>
		/// Deprecated.
		/// </summary>
		/// <remarks>
		/// This flag is specific to <see cref="PhysicalEntityType.Rigid"/>,
		/// <see cref="PhysicalEntityType.Articulated"/> and
		/// <see cref="PhysicalEntityType.WheeledVehicle"/>.
		/// </remarks>
		[Obsolete]
		RigidEntityUseSimpleSolver = 0x01,
		/// <summary>
		/// When set, specifies that this physical entity will not report contacts with water.
		/// </summary>
		/// <remarks>
		/// This flag is specific to <see cref="PhysicalEntityType.Rigid"/>,
		/// <see cref="PhysicalEntityType.Articulated"/> and
		/// <see cref="PhysicalEntityType.WheeledVehicle"/>.
		/// </remarks>
		RigidEntityNoSplashes = 0x04,
		/// <summary>
		/// Deprecated.
		/// </summary>
		/// <remarks>
		/// This flag is specific to <see cref="PhysicalEntityType.Rigid"/>,
		/// <see cref="PhysicalEntityType.Articulated"/> and
		/// <see cref="PhysicalEntityType.WheeledVehicle"/>.
		/// </remarks>
		[Obsolete]
		RigidEntityCheckSumReceived = 0x04,
		/// <summary>
		/// Deprecated.
		/// </summary>
		/// <remarks>
		/// This flag is specific to <see cref="PhysicalEntityType.Rigid"/>,
		/// <see cref="PhysicalEntityType.Articulated"/> and
		/// <see cref="PhysicalEntityType.WheeledVehicle"/>.
		/// </remarks>
		[Obsolete]
		RigidEntityCheckSumOutOfSync = 0x08,
		/// <summary>
		/// When set, specifies that this physical entity will trace rays against alive characters (used
		/// for small and fast objects like bullets to make sure they don't go through any players without
		/// touching them). This flag is set internally when the entity is sufficiently small and fast.
		/// </summary>
		/// <remarks>
		/// This flag is specific to <see cref="PhysicalEntityType.Rigid"/>,
		/// <see cref="PhysicalEntityType.Articulated"/> and
		/// <see cref="PhysicalEntityType.WheeledVehicle"/>.
		/// </remarks>
		RigidEntitySmallAndFast = 0x100,

		/// <summary>
		/// When set, specifies that this physical entity contains pre-calculated physics simulation.
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.Articulated"/>.</remarks>
		ArticulatedEntityRecordedPhysics = 0x02,

		/// <summary>
		/// When set, specifies that this physical entity has only its first and last wheel considered by
		/// the solver. (All wheels with non-0 suspension are always considered).
		/// </summary>
		/// <remarks>This flag is specific to <see cref="PhysicalEntityType.WheeledVehicle"/>.</remarks>
		VehicleFakeInnerWheels = 0x08,

		/// <summary>
		/// When set, specifies that this physical entity will have all of its parts registered separately
		/// in the entity grid.
		/// </summary>
		PartsTraceable = 0x10,
		/// <summary>
		/// When set, specifies that this physical entity is not simulated.
		/// </summary>
		Disabled = 0x20,
		/// <summary>
		/// When set, specifies that this physical entity won't break or deform other objects.
		/// </summary>
		NeverBreak = 0x40,
		/// <summary>
		/// When set, specifies that this physical entity can undergo dynamic breakage/deformation.
		/// </summary>
		Deforming = 0x80,
		/// <summary>
		/// When set, specifies that this physical entity can be pushed by the player.
		/// </summary>
		PushableByPlayers = 0x200,
		/// <summary>
		/// When set, specifies that this physical entity is registered in the entity grid.
		/// </summary>
		Traceable = 0x400,
		/// <summary>
		/// When set, specifies that this physical entity is registered in the entity grid as a particle.
		/// </summary>
		ParticleTraceable = 0x400,
		/// <summary>
		/// When set, specifies that this physical entity is registered in the entity grid as a rope.
		/// </summary>
		RopeTraceable = 0x400,
		/// <summary>
		/// When set, specifies that this physical entity will always be updated in each step of the
		/// simulation.
		/// </summary>
		Update = 0x800,
		/// <summary>
		/// When set, specifies that this physical entity generate immediate events for simulation class
		/// changed (typically rigid bodies falling asleep).
		/// </summary>
		MonitorStateChanges = 0x1000,
		/// <summary>
		/// When set, specifies that this physical entity generate immediate events for collisions.
		/// </summary>
		MonitorCollisions = 0x2000,
		/// <summary>
		/// When set, specifies that this physical entity generate immediate events when something breaks
		/// nearby.
		/// </summary>
		MonitorEnvironmentChanges = 0x4000,
		/// <summary>
		/// When set, specifies that this physical entity doesn't report contacts with triggers (
		/// <see cref="PhysicalEntityType.Area"/> type entities?).
		/// </summary>
		NeverAffectTriggers = 0x8000,
		/// <summary>
		/// When set, specifies that this physical entity is invisible (certain optimizations are applied
		/// to these).
		/// </summary>
		Invisible = 0x10000,
		/// <summary>
		/// When set, specifies that this physical entity will ignore the global water volume.
		/// </summary>
		IgnoreOcean = 0x20000,
		/// <summary>
		/// When set, specifies that this physical entity will force its damping onto the entire group.
		/// </summary>
		FixedDamping = 0x40000,
		/// <summary>
		/// When set, specifies that this physical entity will generate immediate post step events.
		/// </summary>
		MonitorPostStep = 0x80000,
		/// <summary>
		/// When set, specifies that this physical entity will awake objects around it when deleted.
		/// </summary>
		AlwaysNotifyOnDeletion = 0x100000,
		/// <summary>
		/// When set, specifies that this physical entity will ignore
		/// <see cref="PhysicsVariables.BreakImpulseScale"/>.
		/// </summary>
		OverrideImpulseScale = 0x200000,
		/// <summary>
		/// When set, specifies that this physical entity can be broken by player that bump into it.
		/// </summary>
		PlayersCanBreak = 0x400000,
		/// <summary>
		/// When set, specifies that this physical entity cannot trigger 'squashed' state when colliding
		/// with players.
		/// </summary>
		CannotSquashPlayers = 0x10000000,
		/// <summary>
		/// When set, specifies that this physical entity will ignore physical areas (gravity and water).
		/// </summary>
		IgnoreAreas = 0x800000,
		/// <summary>
		/// When set, specifies that this physical entity log simulation class change events.
		/// </summary>
		LogStateChanges = 0x1000000,
		/// <summary>
		/// When set, specifies that this physical entity log collision events.
		/// </summary>
		LogCollisions = 0x2000000,
		/// <summary>
		/// When set, specifies that this physical entity log changes when something breaks nearby
		/// </summary>
		LogEnvironmentChanges = 0x4000000,
		/// <summary>
		/// When set, specifies that this physical entity log post-step-update events
		/// </summary>
		LogPostStep = 0x8000000,
	}
}