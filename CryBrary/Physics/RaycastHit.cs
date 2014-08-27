using System;
using System.Runtime.CompilerServices;
using CryEngine.Entities;
using CryEngine.Mathematics;
using CryEngine.Native;
using CryEngine.Physics;

namespace CryEngine
{
	public struct RaycastHit
	{
		public override bool Equals(object obj)
		{
			if (obj is RaycastHit)
				return this == (RaycastHit)obj;

			return false;
		}

		public static bool operator ==(RaycastHit a, RaycastHit b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(RaycastHit a, RaycastHit b)
		{
			return !(a == b);
		}

		public override int GetHashCode()
		{
			int hash = 17;

			// ReSharper disable NonReadonlyFieldInGetHashCode
			hash = hash * 29 + Distance.GetHashCode();
			hash = hash * 29 + physicalCollider.GetHashCode();
			hash = hash * 29 + Point.GetHashCode();
			hash = hash * 29 + Normal.GetHashCode();
			// ReSharper restore NonReadonlyFieldInGetHashCode

			return hash;
		}

		internal float dist;
		public float Distance
		{
			get { return dist; }
		}

		internal IntPtr physicalCollider;
		public PhysicalEntity PhysicalCollider
		{
			get
			{
				return this.physicalCollider == IntPtr.Zero
					? null
					: PhysicalEntity.TryGet(this.physicalCollider);
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

		internal int ipart;
		public int iPart
		{
			get { return ipart; }
		}

		internal int partid;
		public int PartId
		{
			get { return partid; }
		}

		internal ushort surface_idx;
		public int SurfaceId
		{
			get { return surface_idx; }
		}

		/// <summary>
		/// The surface type that the ray collided with.
		/// </summary>
		public SurfaceType SurfaceType
		{
			get { return SurfaceType.Get(SurfaceId); }
		}

		internal short idmatOrg;
		/// <summary>
		/// original material index, not mapped with material mapping
		/// </summary>
		public short OriginalMaterialIndex
		{
			get { return idmatOrg; }
		}

		internal int foreignIdx;
		public int ForeignIDx
		{
			get { return foreignIdx; }
		}

		internal int inode;
		/// <summary>
		/// BV tree node that had the intersection; can be used for "warm start" next time
		/// </summary>
		public int iNode
		{
			get { return inode; }
		}

		internal Vector3 pt;
		public Vector3 Point
		{
			get { return pt; }
		}

		internal Vector3 n; // surface normal
		public Vector3 Normal
		{
			get { return n; }
		}

		internal int bTerrain; // global terrain hit
		public bool HitTerrain
		{
			get { return bTerrain == 1; }
		}

		internal int iprim;
		/// <summary>
		/// hit triangle index
		/// </summary>
		public int iPrim
		{
			get { return iprim; }
		}

		public IntPtr nextHit;
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