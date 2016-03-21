using System;
using System.Linq;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of types of collidable object types.
	/// </summary>
	[Flags]
	public enum ColliderTypes : uint
	{
		/// <summary>
		/// When set, specifies that this object can be recognized as a player by other objects.
		/// </summary>
		Player = GeometryCollisionTypeCodes.collision_type1,
		/// <summary>
		/// When set, specifies that this object can be recognized as an explosion by other objects.
		/// </summary>
		Explosion = GeometryCollisionTypeCodes.collision_type2,
		/// <summary>
		/// When set, specifies that this object can be recognized as a vehicle by other objects.
		/// </summary>
		Vehicle = GeometryCollisionTypeCodes.collision_type3,
		/// <summary>
		/// When set, specifies that this object can be recognized as a foliage by other objects.
		/// </summary>
		Foliage = GeometryCollisionTypeCodes.collision_type4,
		/// <summary>
		/// When set, specifies that this object can be recognized as a debris by other objects.
		/// </summary>
		Debris = GeometryCollisionTypeCodes.collision_type5,
		/// <summary>
		/// When set, specifies that this object can be recognized as a foliage proxy by other objects.
		/// </summary>
		FoliageProxy = GeometryCollisionTypeCodes.collision_type13,
		/// <summary>
		/// When set, specifies that this object can be recognized as an obstruction by other objects.
		/// </summary>
		Obstruct = GeometryCollisionTypeCodes.collision_type14,
		/// <summary>
		/// When set, specifies that this object can be recognized as a solid object by other objects.
		/// </summary>
		Solid = 0x0FFF & ~Explosion,
		/// <summary>
		/// When set, specifies that this object can be recognized as anything by other objects.
		/// </summary>
		Any = 0xFFFF,
		/// <summary>
		/// When set, specifies that physical particles don't apply impulses to this part/entity.
		/// </summary>
		NoParticleImpulse = 0x8000000,
		/// <summary>
		/// When set, specifies that this part/entity is destroyed when it breaks.
		/// </summary>
		DestroyedOnBreak = 0x2000000,
	}
}