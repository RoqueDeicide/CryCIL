using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to set the outer entity for this physical entity. This
	/// type uses factory methods for construction.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Outer entities are supposed to be used to group entities that have
	/// <see cref="PhysicsSimulationClass.Independent"/> together (e.g. a bunch of ropes that attaching
	/// hanging people to a tree where tree is an outer entity for ropes).
	/// </para>
	/// <para>This structure is never used in any sample code. Use at your own risk.</para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsParametersOuterEntity
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to set the outer
		/// entity for this physical entity or to <see cref="PhysicalEntity.GetParameters"/> to query the
		/// outer entity.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private PhysicalEntity outerEntity;
		[UsedImplicitly] private GeometryShape outerGeometry;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the outer entity.
		/// </summary>
		public PhysicalEntity OuterEntity => this.outerEntity;
		/// <summary>
		/// Gets the encompassing geometry (this geometry is used when querying containment of a certain
		/// point using <see cref="PhysicsStatusContainsPoint"/>).
		/// </summary>
		public GeometryShape OuterGeometry => this.outerGeometry;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new instance of this type that can be used to get the outer entity from the physical
		/// entity.
		/// </summary>
		/// <returns>
		/// A valid object of type <see cref="PhysicsParametersOuterEntity"/> that can be passed to
		/// <see cref="PhysicalEntity.GetParameters"/>.
		/// </returns>
		public static PhysicsParametersOuterEntity Create()
		{
			return new PhysicsParametersOuterEntity()
			{
				Base = new PhysicsParameters(PhysicsParametersTypes.OuterEntity),
				outerEntity = new PhysicalEntity(),
				outerGeometry = new GeometryShape()
			};
		}
		/// <summary>
		/// Creates a new instance of this type that can be used to assign an outer entity to the physical
		/// entity.
		/// </summary>
		/// <param name="outerEntity">A physical entity that will serve as outer entity for another.</param>
		/// <returns>
		/// A valid object of type <see cref="PhysicsParametersOuterEntity"/> that can be passed to
		/// <see cref="PhysicalEntity.SetParameters"/>.
		/// </returns>
		public static PhysicsParametersOuterEntity Create(PhysicalEntity outerEntity)
		{
			return new PhysicsParametersOuterEntity()
			{
				Base = new PhysicsParameters(PhysicsParametersTypes.OuterEntity),
				outerEntity = outerEntity,
				outerGeometry = new GeometryShape()
			};
		}
		/// <summary>
		/// Creates a new instance of this type that can be used to assign an outer entity along with
		/// encompassing geometry to the physical entity.
		/// </summary>
		/// <param name="outerEntity">         
		/// A physical entity that will serve as outer entity for another.
		/// </param>
		/// <param name="encompassingGeometry">
		/// A custom geometry that will be used to check for containment of the point.
		/// </param>
		/// <returns>
		/// A valid object of type <see cref="PhysicsParametersOuterEntity"/> that can be passed to
		/// <see cref="PhysicalEntity.SetParameters"/>.
		/// </returns>
		public static PhysicsParametersOuterEntity Create(PhysicalEntity outerEntity, GeometryShape encompassingGeometry)
		{
			return new PhysicsParametersOuterEntity()
			{
				Base = new PhysicsParameters(PhysicsParametersTypes.OuterEntity),
				outerEntity = outerEntity,
				outerGeometry = encompassingGeometry
			};
		}
		#endregion
	}
}