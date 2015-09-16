using CryCil.Annotations;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates information about location and movement of geometric object in the world.
	/// </summary>
	public struct GeometryWorldData
	{
		#region Fields
		[UsedImplicitly] private Vector3 offset;
		[UsedImplicitly] private Matrix33 R;
		[UsedImplicitly] private float scale;
		[UsedImplicitly] private Vector3 v;
		[UsedImplicitly] private EulerAngles w;
		[UsedImplicitly] private Vector3 centerOfMass;
		[UsedImplicitly] private int iStartNode;
		[UsedImplicitly] private bool initialized;
		#endregion
		#region Properties
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new object of this type.
		/// </summary>
		/// <param name="position">       Coordinates of the geometric object in world space.</param>
		/// <param name="orientation">    
		/// 3x3 matrix that represents the orientation of the geometric object.
		/// </param>
		/// <param name="scale">          Scale of geometric object.</param>
		/// <param name="velocity">       Velocity of geometric object.</param>
		/// <param name="angularVelocity">
		/// Angular velocity of geometric object around <paramref name="centerOfMass"/>.
		/// </param>
		/// <param name="centerOfMass">   Position of the center of mass of geometric object.</param>
		/// <param name="startNode">      Optional zero-based index of BV-tree node to start from.</param>
		public GeometryWorldData(Vector3 position, Matrix33 orientation, float scale = 1, Vector3 velocity = new Vector3(),
								 EulerAngles angularVelocity = new EulerAngles(), Vector3 centerOfMass = new Vector3(),
								 int startNode = 0)
		{
			this.initialized = true;
			this.offset = position;
			this.R = orientation;
			this.scale = scale;
			this.v = velocity;
			this.w = angularVelocity;
			this.centerOfMass = centerOfMass;
			this.iStartNode = startNode;
		}
		/// <summary>
		/// Creates a new object of this type.
		/// </summary>
		/// <param name="position">       Coordinates of the geometric object in world space.</param>
		/// <param name="scale">          Scale of geometric object.</param>
		/// <param name="velocity">       Velocity of geometric object.</param>
		/// <param name="angularVelocity">
		/// Angular velocity of geometric object around <paramref name="centerOfMass"/>.
		/// </param>
		/// <param name="centerOfMass">   Position of the center of mass of geometric object.</param>
		/// <param name="startNode">      Optional zero-based index of BV-tree node to start from.</param>
		public GeometryWorldData(Vector3 position, float scale = 1, Vector3 velocity = new Vector3(),
								 EulerAngles angularVelocity = new EulerAngles(), Vector3 centerOfMass = new Vector3(),
								 int startNode = 0)
			: this(position, Matrix33.Identity, scale, velocity, angularVelocity, centerOfMass, startNode)
		{
		}
		#endregion
		#region Interface
		#endregion
		#region Utilities
		internal void CompleteInitialization()
		{
			if (!this.initialized)
			{
				// This code is only reached when the object was created using default constructor.
				if (this.R == new Matrix33())
				{
					this.R = Matrix33.Identity;
				}
				this.initialized = true;
			}
		}
		#endregion
	}
}