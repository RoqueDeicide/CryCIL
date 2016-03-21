using System;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates basic information about the state of the physical entity.
	/// </summary>
	public struct PhysicalEntityStateInfo
	{
		#region Fields
		private readonly PhysicsSimulationClass simClass;
		private readonly BoundingBox boundingBox;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the simulation class that is used for the entity.
		/// </summary>
		public PhysicsSimulationClass SimulationClass => this.simClass;
		/// <summary>
		/// Gets the axis-aligned bounding box that encompasses the entity.
		/// </summary>
		public BoundingBox BoundingBox => this.boundingBox;
		#endregion
		#region Construction
		internal PhysicalEntityStateInfo(PhysicsSimulationClass simClass, BoundingBox boundingBox)
		{
			this.simClass = simClass;
			this.boundingBox = boundingBox;
		}
		#endregion
	}
}