using CryCil.Annotations;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to get/set a bounding box for the physical entity.
	/// This type uses factory methods for construction.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	public struct PhysicsParametersBoundingBox
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParams"/> to force the
		/// bounding box onto physical entity or to <see cref="PhysicalEntity.GetParams"/> to get the
		/// current one.
		/// </summary>
		[UsedImplicitly]
		public PhysicsParameters Base;
		[UsedImplicitly]
		private BoundingBox boundingBox;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets a bounding box.
		/// </summary>
		public BoundingBox BoundingBox
		{
			get { return this.boundingBox; }
			set { this.boundingBox = value; }
		}
		#endregion
		#region Construction
		private PhysicsParametersBoundingBox(PhysicsParametersTypes type, BoundingBox box)
		{
			this.Base = new PhysicsParameters(type);
			this.boundingBox = box;
		}
		/// <summary>
		/// Creates a new instance of this type that can be used to query the bounding box on the physical
		/// entity.
		/// </summary>
		/// <returns>
		/// A valid instance of type <see cref="PhysicsParametersBoundingBox"/> that can be passed to
		/// <see cref="PhysicalEntity.GetParams"/>.
		/// </returns>
		public static PhysicsParametersBoundingBox Create()
		{
			return new PhysicsParametersBoundingBox(PhysicsParametersTypes.BoundingBox, new BoundingBox());
		}
		/// <summary>
		/// Creates a new instance of this type that can be used to force the bounding box onto the
		/// physical entity.
		/// </summary>
		/// <param name="box">
		/// A new bounding box for the entity. It will be overridden next the entity decides to recalculate
		/// it (e.g. when it moves).
		/// </param>
		/// <returns>
		/// A valid instance of type <see cref="PhysicsParametersBoundingBox"/> that can be passed to
		/// <see cref="PhysicalEntity.SetParams"/>.
		/// </returns>
		public static PhysicsParametersBoundingBox Create(BoundingBox box)
		{
			return new PhysicsParametersBoundingBox(PhysicsParametersTypes.BoundingBox, box);
		}
		#endregion
	}
}