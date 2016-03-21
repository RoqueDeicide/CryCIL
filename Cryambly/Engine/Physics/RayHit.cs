using System;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates information about one hit from ray cast.
	/// </summary>
	public struct RayHit
	{
		#region Fields
		private readonly float dist;
		private readonly PhysicalEntity pCollider;
		private readonly int ipart;
		private readonly int partid;
		private readonly short surface_idx;
		private readonly short idmatOrg;
		[UsedImplicitly] private readonly int foreignIdx;
		[UsedImplicitly] private readonly int iNode;
		private readonly Vector3 pt;
		private readonly Vector3 n;
		private readonly int bTerrain;
		[UsedImplicitly] private readonly int iPrim;
		[UsedImplicitly] private readonly IntPtr next;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the distance between the origin of the ray and point of the hit.
		/// </summary>
		public float Distance => this.dist;
		/// <summary>
		/// Gets the physical entity that was hit.
		/// </summary>
		public PhysicalEntity Collider => this.pCollider;
		/// <summary>
		/// Gets the identifier of the part of <see cref="Collider"/> that was hit by the ray.
		/// </summary>
		public int PartId => this.partid;
		/// <summary>
		/// Gets zero-based index of the part of <see cref="Collider"/> that was hit by the ray.
		/// </summary>
		public int PartIndex => this.ipart;
		/// <summary>
		/// Gets the object that represents the surface of the body that was hit by the ray.
		/// </summary>
		public PhysicalSurface Surface => new PhysicalSurface(this.surface_idx);
		/// <summary>
		/// Gets the normal identifier of the material of the body that was hit by the ray.
		/// </summary>
		public short MaterialId => this.idmatOrg;
		/// <summary>
		/// Gets the coordinates of the point of contact.
		/// </summary>
		public Vector3 Point => this.pt;
		/// <summary>
		/// Gets the direction of the normal to surface of the body that was hit by the ray at the point of
		/// contact.
		/// </summary>
		public Vector3 Normal => this.n;
		/// <summary>
		/// Gets the value that indicates whether the body that was hit is terrain.
		/// </summary>
		public bool IsTerrain => this.bTerrain != 0;
		#endregion
		#region Construction
		internal RayHit(float dist, PhysicalEntity pCollider, int ipart, int partid, short surfaceIdx, short idmatOrg,
						int foreignIdx, int iNode, Vector3 pt, Vector3 n, int bTerrain, int iPrim, IntPtr next)
		{
			this.dist = dist;
			this.pCollider = pCollider;
			this.ipart = ipart;
			this.partid = partid;
			this.surface_idx = surfaceIdx;
			this.idmatOrg = idmatOrg;
			this.foreignIdx = foreignIdx;
			this.iNode = iNode;
			this.pt = pt;
			this.n = n;
			this.bTerrain = bTerrain;
			this.iPrim = iPrim;
			this.next = next;
		}
		#endregion
	}
}