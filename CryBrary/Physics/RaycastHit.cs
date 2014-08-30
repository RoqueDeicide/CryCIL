using System;
using System.Runtime.CompilerServices;
using CryEngine.Annotations;
using CryEngine.Entities;
using CryEngine.Mathematics;
using CryEngine.Native;
using CryEngine.Physics;

namespace CryEngine
{
	/// <summary>
	/// Encapsulates information about hit of casted ray.
	/// </summary>
	public struct RaycastHit : IEquatable<RaycastHit>
	{
		#region Fields
		/// <summary>
		/// Distance between origin of the ray and point of collision.
		/// </summary>
		public float Distance;
		/// <summary>
		/// Pointer to PhysicalEntity the ray has collided with.
		/// </summary>
		public IntPtr ColliderHandle;
		/// <summary>
		///
		/// </summary>
		public int Part;
		/// <summary>
		///
		/// </summary>
		public int PartIdentifier;
		/// <summary>
		/// Identifier of the surface type.
		/// </summary>
		public ushort SurfaceIdentifeir;
		/// <summary>
		/// Identifier of original material without mapping.
		/// </summary>
		public short OriginalMaterialIdentifeir;
		/// <summary>
		///
		/// </summary>
		public int ForeignDataIdentifiers;
		/// <summary>
		/// Bounding volume tree node.
		/// </summary>
		public int Node;
		/// <summary>
		/// Point of the hit.
		/// </summary>
		public Vector3 Point;
		/// <summary>
		/// Normal of the surface that has been hit.
		/// </summary>
		public Vector3 Normal;
		/// <summary>
		/// Indicator for terrain hit.
		/// </summary>
		public int TerrainIndication;
		/// <summary>
		///
		/// </summary>
		public int Primitive;
		[UsedImplicitly]
		private IntPtr nextHit;
		#endregion
		#region Properties
		/// <summary>
		/// Gets <see cref="PhysicalEntity"/> the ray collided with.
		/// </summary>
		public PhysicalEntity PhysicalCollider
		{
			get
			{
				return this.ColliderHandle == IntPtr.Zero
					? null
					: PhysicalEntity.TryGet(this.ColliderHandle);
			}
		}
		/// <summary>
		/// Entity that the ray collided with.
		/// </summary>
		public EntityBase Entity
		{
			get
			{
				var collider = PhysicalCollider;
				return collider == null ? null : collider.Owner;
			}
		}
		/// <summary>
		/// The surface type that the ray collided with.
		/// </summary>
		public SurfaceType SurfaceType
		{
			get { return SurfaceType.Get(this.SurfaceIdentifeir); }
		}
		/// <summary>
		/// Indicates whether we have hit the terrain.
		/// </summary>
		public bool HitTerrain
		{
			get { return this.TerrainIndication == 1; }
		}
		#endregion
		#region Interface
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if two object are equal.</returns>
		public bool Equals(RaycastHit other)
		{
			return
				this.TerrainIndication == other.TerrainIndication &&
				Math.Abs(this.Distance - other.Distance) < MathHelpers.ZeroTolerance &&
				this.ForeignDataIdentifiers == other.ForeignDataIdentifiers &&
				this.OriginalMaterialIdentifeir == other.OriginalMaterialIdentifeir &&
				this.Node == other.Node &&
				this.Part == other.Part &&
				this.Primitive == other.Primitive &&
				this.PartIdentifier == other.PartIdentifier &&
				this.SurfaceIdentifeir == other.SurfaceIdentifeir &&
				this.Normal == other.Normal &&
				this.nextHit == other.nextHit &&
				this.ColliderHandle == other.ColliderHandle &&
				this.Point == other.Point;
		}
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="obj">Another object.</param>
		/// <returns>
		/// True, if another object is of type <see cref="RaycastHit"/> which is equal to this one.
		/// </returns>
		public override bool Equals(object obj)
		{
			return (obj is RaycastHit) && this.Equals((RaycastHit)obj);
		}
		/// <summary>
		/// Determines whether two instances of type <see cref="RaycastHit"/> are equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if operands are equal.</returns>
		public static bool operator ==(RaycastHit left, RaycastHit right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Determines whether two instances of type <see cref="RaycastHit"/> are not equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if operands are not equal.</returns>
		public static bool operator !=(RaycastHit left, RaycastHit right)
		{
			return !(left == right);
		}
		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			int hash = 17;

			// ReSharper disable NonReadonlyFieldInGetHashCode
			hash = hash * 29 + Distance.GetHashCode();
			hash = hash * 29 + this.ColliderHandle.GetHashCode();
			hash = hash * 29 + Point.GetHashCode();
			hash = hash * 29 + Normal.GetHashCode();
			// ReSharper restore NonReadonlyFieldInGetHashCode

			return hash;
		}
		#endregion
	}

	[Flags]
	public enum SurfaceFlags
	{
		PierceableMask = 0x0F,
		MaxPierceable = 0x0F,
		Important = 0x200,
		ManuallyBreakable = 0x400,
		MaterialBreakableBit = 16
	}

	[Flags]
	public enum RayWorldIntersectionFlags
	{
		IgnoreTerrainHole = 0x20,
		IgnoreNonColliding = 0x40,
		IgnoreBackfaces = 0x80,
		IgnoreSolidBackfaces = 0x100,
		PierceabilityMask = 0x0F,
		Pierceability = 0,
		StopAtPierceable = 0x0F,
		/// <summary>
		/// among pierceble hits, materials with sf_important will have priority
		/// </summary>
		SeperateImportantHits = SurfaceFlags.Important,
		/// <summary>
		/// used to manually specify collision geometry types (default is geom_colltype_ray)
		/// </summary>
		CollissionTypeBit = 16,
		/// <summary>
		/// if several colltype flag are specified, switches between requiring all or any of them in
		/// a geometry
		/// </summary>
		CollissionTypeAny = 0x400,
		/// <summary>
		/// queues the RWI request, when done it'll generate EventPhysRWIResult
		/// </summary>
		Queue = 0x800,
		/// <summary>
		/// non-colliding geometries will be treated as pierceable regardless of the actual material
		/// </summary>
		ForcePiercableNonCollidable = 0x1000,
		/// <summary>
		/// marks the rwi to be a debug rwi (used for spu debugging, only valid in non-release builds)
		/// </summary>
		DebugTrace = 0x2000,
		/// <summary>
		/// update phitLast with the current hit results (should be set if the last hit should be
		/// reused for a "warm" start)
		/// </summary>
		UpdateLastHit = 0x4000,
		/// <summary>
		/// returns the first found hit for meshes, not necessarily the closest
		/// </summary>
		AnyHit = 0x8000
	}

	/// <summary>
	/// Used for GetEntitiesInBox and RayWorldIntersection
	/// </summary>
	[Flags]
	public enum EntityQueryFlags
	{
		Static = 1,
		SleepingRigid = 2,
		Rigid = 4,
		Living = 8,
		Independent = 16,
		Deleted = 128,
		Terrain = 0x100,
		All = Static | SleepingRigid | Rigid | Living | Independent | Terrain,
		FlaggedOnly = 0x800,
		SkipFlagged = FlaggedOnly * 2, // "flagged" meas has pef_update set
		Areas = 32,
		Triggers = 64,
		IgnoreNonColliding = 0x10000,
		/// <summary>
		/// sort by mass in ascending order
		/// </summary>
		SortByMass = 0x20000,
		/// <summary>
		/// if not set, the function will return an internal pointer
		/// </summary>
		AllocateList = 0x40000,
		/// <summary>
		/// will call AddRef on each entity in the list (expecting the caller call Release)
		/// </summary>
		AddRefResults = 0x100000,
		/// <summary>
		/// can only be used in RayWorldIntersection
		/// </summary>
		Water = 0x200,
		/// <summary>
		/// can only be used in RayWorldIntersection
		/// </summary>
		NoOnDemandActivation = 0x80000,
		/// <summary>
		/// queues procedural breakage requests; can only be used in SimulateExplosion
		/// </summary>
		DelayedDeformations = 0x80000
	}
}