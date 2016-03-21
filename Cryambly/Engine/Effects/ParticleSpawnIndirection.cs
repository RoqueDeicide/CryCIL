using System;
using System.Linq;

namespace CryCil.Engine
{
	/// <summary>
	/// Enumeration of values that indicates the spawn location of the particles.
	/// </summary>
	public enum ParticleSpawnIndirection : byte
	{
		/// <summary>
		/// Particles are spawned from the emitter's location.
		/// </summary>
		Emitter,
		/// <summary>
		/// Particles are spawned where parent's particles spawn.
		/// </summary>
		ParentStart,
		/// <summary>
		/// Particles are spawned where parent's particles collide with surfaces (probably).
		/// </summary>
		ParentCollide,
		/// <summary>
		/// Particles are spawned where parent's particles despawn.
		/// </summary>
		ParentDeath
	}
}