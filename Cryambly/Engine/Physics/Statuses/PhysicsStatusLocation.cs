using CryCil.Annotations;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of values that are used to query location data from the physical entity.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When querying location of the entity using <see cref="PhysicalEntity.GetStatus"/> function the
	/// result will !0 when successful.
	/// </para>
	/// </remarks>
	public struct PhysicsStatusLocation
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to <see cref="PhysicalEntity.GetStatus"/> to query location of the
		/// physical entity.
		/// </summary>
		public PhysicsStatus Base;
		[UsedImplicitly]
		private int partid;
		[UsedImplicitly]
		private int ipart;

		/// <summary>
		/// When query is complete this field will contain coordinates of the center of the physical
		/// entity.
		/// </summary>
		public Vector3 Center;
		/// <summary>
		/// When query is complete this field will contain an AABB that is relative to the
		/// <see cref="Center"/>.
		/// </summary>
		public BoundingBox BoundingBox;
		/// <summary>
		/// When query is complete this field will contain <see cref="Quaternion"/> that represents
		/// orientation of the physical entity.
		/// </summary>
		public Quaternion Orientation;
		/// <summary>
		/// When query is complete this field will contain uniform scale factor of this entity.
		/// </summary>
		public float Scale;
		/// <summary>
		/// When query is complete this field will contain a value that specifies current simulation class
		/// of the entity.
		/// </summary>
		public PhysicsSimulationClass SimulationClass;
		/// <summary>
		/// When query is complete this field will contain 3x4 transformation matrix that represents
		/// position, orientation and scale of the entity.
		/// </summary>
		public Matrix34 Transformation;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new object that can be used to query location of the part of the entity.
		/// </summary>
		/// <param name="part">
		/// An value that identifies the part of the entity that must be queried. If -1 is passed then the
		/// entire entity will be queried.
		/// </param>
		/// <param name="isId">
		/// An optional value that indicates how to treat <paramref name="part"/>. If <c>true</c>, it will
		/// be treated as identifier, otherwise it will be treated as an index.
		/// </param>
		public PhysicsStatusLocation(int part, bool isId = true)
			: this()
		{
			if (part != -1 && !isId)
			{
				this.ipart = part;
			}
			else
			{
				this.partid = part;
			}
		}
		#endregion
	}
}