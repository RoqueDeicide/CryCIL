using System;
using System.Linq;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of types of physical entities.
	/// </summary>
	public enum PhysicalEntityType
	{
		/// <summary>
		/// Represents an entity that is not physicalized.
		/// </summary>
		None = 0,
		/// <summary>
		/// Represents an entity that is static.
		/// </summary>
		/// <remarks>Static entities cannot be moved via physical interactions.</remarks>
		Static = 1,
		/// <summary>
		/// Represents an entity that is a rigid body.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Rigid bodies have a shape that cannot be changed by physical interactions but they can be moved
		/// via physical interactions.
		/// </para>
		/// <para>
		/// Rigid bodies with mass of zero are similar to static entities with only difference being the
		/// collision detection algorithm being more reliable for cases when the entity changes its position
		/// via code.
		/// </para>
		/// </remarks>
		Rigid = 2,
		/// <summary>
		/// Represents an entity that is a vehicle with wheels.
		/// </summary>
		/// <remarks>Vehicles with wheels are extended versions of rigid bodies.</remarks>
		WheeledVehicle = 3,
		/// <summary>
		/// Represents a living entity.
		/// </summary>
		/// <remarks>
		/// Living entities are rigid bodies that have extended set of simulation parameters and they are
		/// always kept up-right.
		/// </remarks>
		Living = 4,
		/// <summary>
		/// Represents a particle that was spawned by the particle effect emitter.
		/// </summary>
		/// <remarks>
		/// Particles are similar to rigid bodies with a set of particle-specific parameters.
		/// </remarks>
		Particle = 5,
		/// <summary>
		/// Represents an articulated entity.
		/// </summary>
		/// <remarks>
		/// Articulated entities consist of multiple physical entities that are rigid bodies that are linked
		/// to each other into an hierarchy that represents a tree with a single root.
		/// </remarks>
		Articulated = 6,
		/// <summary>
		/// Represents a rope.
		/// </summary>
		/// <remarks>
		/// Ropes are simulated as chains of connected equal-length sticks ("segments") with point masses.
		/// </remarks>
		Rope = 7,
		/// <summary>
		/// Represents a soft body.
		/// </summary>
		/// <remarks>
		/// Soft bodies are similar to rigid bodies, except they can be deformed by physical interactions.
		/// </remarks>
		Soft = 8,
		/// <summary>
		/// Represents an area.
		/// </summary>
		/// <remarks>
		/// Areas are incorporeal static entities: they can collide with other entities, but otherwise they
		/// don't interact with physical world. Areas are used for proximity triggers.
		/// </remarks>
		Area = 9
	}
}