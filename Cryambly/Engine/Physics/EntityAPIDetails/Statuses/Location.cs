using System;
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
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
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
		[UsedImplicitly] private int partid;
		[UsedImplicitly] private int ipart;
		[UsedImplicitly] private bool localSpace;

		/// <summary>
		/// An object that encapsulates coordinates of the center of the part/entity, orientation of it and
		/// its scale.
		/// </summary>
		public Quatvecale Location;
		/// <summary>
		/// When query is complete this field will contain an AABB that is relative to the center of the
		/// part/entity.
		/// </summary>
		public BoundingBox BoundingBox;
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
		/// <summary>
		/// A set of user-defined flags that are assigned to the part of the entity.
		/// </summary>
		public uint Flags;
		/// <summary>
		/// A wrapper for a pointer to the geometry that is used by the part/entity for ray-tracing.
		/// </summary>
		public GeometryShape Geometry;
		/// <summary>
		/// A wrapper for a pointer to the geometry that is used by the part/entity for physical
		/// interactions.
		/// </summary>
		public GeometryShape GeometryProxy;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new object that can be used to query location of the part of the entity.
		/// </summary>
		/// <param name="part">      
		/// Specifies which part of the entity to query. If <see cref="EntityPartSpec.EntireEntity"/> is
		/// passed, then the part won't be specified.
		/// </param>
		/// <param name="localSpace">
		/// Indicates whether location of the part/entity must be specified in local entity space, rather
		/// then world space.
		/// </param>
		public PhysicsStatusLocation(EntityPartSpec part, bool localSpace = false)
			: this()
		{
			this.Base = new PhysicsStatus(PhysicsStatusTypes.Location);
			this.localSpace = localSpace;
			if (part.partIsSpecified)
			{
				this.partid = part.PartId;
				this.ipart = part.PartIndex;
			}
			else
			{
				this.partid = -1;
				this.ipart = -1;
			}
		}
		/// <summary>
		/// Creates a new object that can be used to query location of the entity.
		/// </summary>
		/// <param name="localSpace">
		/// Indicates whether location of the entity must be specified in local entity space, rather then
		/// world space.
		/// </param>
		public PhysicsStatusLocation(bool localSpace)
			: this()
		{
			this.Base = new PhysicsStatus(PhysicsStatusTypes.Location);
			this.localSpace = localSpace;
			this.partid = -1;
			this.ipart = -1;
		}
		#endregion
	}
}