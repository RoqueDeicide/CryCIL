using System;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of flags that specifies how to cast the ray through the physical world.
	/// </summary>
	[Flags]
	public enum RayCastFlags : uint
	{
		/// <summary>
		/// When set, specifies that the ray must ignore the holes in the terrain.
		/// </summary>
		IgnoreTerrainHoles = 0x20,
		/// <summary>
		/// When set, specifies that the ray must ignore non-colliding geometry(?).
		/// </summary>
		IgnoreNonColliding = 0x40,
		/// <summary>
		/// When set, specifies that the ray must ignore back faces.
		/// </summary>
		IgnoreBackFaces = 0x80,
		/// <summary>
		/// When set, specifies that the ray must ignore physicalized back faces (?).
		/// </summary>
		IgnoreSolidBackFaces = 0x100,
		/// <summary>
		/// A bit mask that allows to isolate the bits that contain the pierceability of the ray.
		/// </summary>
		PierceabilityMask = 0x0F,
		/// <summary>
		/// Number of bits to shift in order to store the ray pierceability in this set of flags.
		/// </summary>
		PierceabilityBitShift = 0,
		/// <summary>
		/// When set, specifies that the ray cast result should only have 1 hit.
		/// </summary>
		StopAtPierceable = 0x0F,
		/// <summary>
		/// When set, specifies that hits that pierce the surfaces that have flag
		/// <see cref="SurfaceFlags.Important"/> set should be listed before other piercing hits.
		/// </summary>
		SeparateImportantHits = SurfaceFlags.Important,
		/// <summary>
		/// Number of bits to shift in order to store the flags from <see cref="ColliderTypes"/> in this
		/// set of flags to specify which bits must be set on the part of the entity to register the hit.
		/// </summary>
		CollisionTypeBitShift = 16,
		/// <summary>
		/// When set, specifies that at least one flag among those that were set using
		/// <see cref="CollisionTypeBitShift"/> must be set on the part of the entity for the hit to be
		/// registered.
		/// </summary>
		CollisionTypeAny = 0x400,
		//queue = 0x800, // queues the RWI request, when done it'll generate EventPhysRWIResult

		/// <summary>
		/// When set, specifies that non-colliding geometries must be treated as pierceable regardless of
		/// the actual material.
		/// </summary>
		ForcePierceableOnNonColliders = 0x1000
		//update_last_hit = 0x4000,// update phitLast with the current hit results (should be set if the last hit should be reused for a "warm" start)
		//any_hit = 0x8000 // returns the first found hit for meshes, not necessarily the closets
	}
}