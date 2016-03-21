using System;
using System.Linq;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of flags that can be assigned to the physical geometry objects.
	/// </summary>
	[Flags]
	public enum PhysicsGeometryFlags : uint
	{
		/// <summary>
		/// When set, indicates that the geometry/part can be detected by ray-tracing.
		/// </summary>
		CollisionTypeRay = 0x8000,
		/// <summary>
		/// When set, indicates that this geometry/part can float in liquids.
		/// </summary>
		/// <remarks>
		/// This flag has to be set to apply the parameters that were specified in
		/// <see cref="PhysicsParametersBuoyancy"/>.
		/// </remarks>
		Floats = 0x10000,
		/// <summary>
		/// When set, indicates that this geometry must be used as a proxy for physical interactions when
		/// added via <see cref="PhysicalEntity.AddBody"/>.
		/// </summary>
		Proxy = 0x20000,
		/// <summary>
		/// When set, indicates that this geometry/part can be broken or deformed.
		/// </summary>
		StructureChanges = 0x40000,
		/// <summary>
		/// When set, indicates that this geometry/part is modifiable (only possible if this geometry is
		/// cloned and is used in this entity only).
		/// </summary>
		CanModify = 0x80000,
		/// <summary>
		/// When set, indicates that this geometry/part supports 'soft' collisions (used for tree foliage
		/// proxy) .
		/// </summary>
		Squashy = 0x100000,
		/// <summary>
		/// When set, indicates that this geometry/part will cause a physics event whenever its bounding box
		/// overlaps with anything.
		/// </summary>
		LogInteractions = 0x200000,
		//monitor_contacts = 0x400000, // part needs collision callback from the solver (used internally)
		/// <summary>
		/// When set, indicates that this geometry/part can be broken via code outside of physics.
		/// </summary>
		ManuallyBreakable = 0x800000,
		/// <summary>
		/// When set, indicates that this geometry/part's collisions are detected and reported, but not
		/// processed.
		/// </summary>
		/// <remarks>
		/// Current assumption is that this flag can be set if you don't want the entity to be bobbed around
		/// by the collisions.
		/// </remarks>
		NoCollisionResponse = 0x1000000,
		/// <summary>
		/// When set, indicates that this geometry/part is used to change other collision's material id if
		/// the collision point is inside it.
		/// </summary>
		MaterialSubstitutor = 0x2000000,
		/// <summary>
		/// When set, indicates that this geometry/part has capsule approximation applied to it after
		/// breaking (used for tree trunks)
		/// </summary>
		BreakApproximation = 0x4000000,
		/// <summary>
		/// When set, indicates that the geometry/part can collide with physical entities or their parts
		/// that are marked as default geometry.
		/// </summary>
		CollisionTypeDefault = GeometryCollisionTypeCodes.collision_type0,
		/// <summary>
		/// When set, indicates that the geometry/part can collide with physical entities or their parts
		/// that are marked as players.
		/// </summary>
		CollisionTypePlayer = GeometryCollisionTypeCodes.collision_type1,
		/// <summary>
		/// When set, indicates that the geometry/part can collide with physical entities or their parts
		/// that are marked as explosions.
		/// </summary>
		/// <remarks>Geometries that don't have this flag set cannot be affected by explosions.</remarks>
		CollisionTypeExplosion = GeometryCollisionTypeCodes.collision_type2,
		/// <summary>
		/// When set, indicates that the geometry/part can collide with physical entities or their parts
		/// that are marked as vehicles.
		/// </summary>
		CollisionTypeVehicle = GeometryCollisionTypeCodes.collision_type3,
		/// <summary>
		/// When set, indicates that the geometry/part can collide with physical entities or their parts
		/// that are marked as foliage.
		/// </summary>
		CollisionTypeFoliage = GeometryCollisionTypeCodes.collision_type4,
		/// <summary>
		/// When set, indicates that the geometry/part can collide with physical entities or their parts
		/// that are marked as debris.
		/// </summary>
		CollisionTypeDebris = GeometryCollisionTypeCodes.collision_type5,
		/// <summary>
		/// When set, indicates that the geometry/part can collide with physical entities or their parts
		/// that are marked as players.
		/// </summary>
		CollisionTypeFoliageProxy = GeometryCollisionTypeCodes.collision_type13,
		/// <summary>
		/// When set, indicates that the geometry/part can collide with physical entities or their parts
		/// that are marked as foliage proxies.
		/// </summary>
		CollisionTypeObstruct = GeometryCollisionTypeCodes.collision_type14,
		/// <summary>
		/// When set, indicates that the geometry/part can collide with physical entities or their parts
		/// that are marked as solid objects.
		/// </summary>
		CollisionTypeSolid = 0x0FFF & ~CollisionTypeExplosion,
		/// <summary>
		/// When set, indicates that the geometry/part can collide with anything.
		/// </summary>
		CollisionTypeAnything = 0xFFFF
	}
}