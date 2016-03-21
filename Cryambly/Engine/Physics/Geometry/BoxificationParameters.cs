using System;
using System.Linq;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that specify how to create a set of boxes that covers entirety of
	/// <see cref="GeometryShape"/> that is a triangular mesh.
	/// </summary>
	public struct BoxificationParameters
	{
		#region Fields
		/// <summary>
		/// Represents a set of default boxification parameters.
		/// </summary>
		/// <remarks>
		/// The code that is used to initialize this object:
		/// <code>
		/// new BoxificationParameters
		/// {
		///     MinimalFaceArea           = 0.4f * 0.4f,
		///     DistanceFilter            = 0.2f,
		///     VoxelGridResolution       = 100,
		///     MaxFaceTiltAngle          = Degree.ToRadian(10),
		///     MinLayerFilling           = 0.5f,
		///     MaxLayerReusage           = 0.8f,
		///     MaxVoxelIslandConnections = 0.5f
		/// };
		/// </code>
		/// </remarks>
		public static readonly BoxificationParameters Default = new BoxificationParameters
		{
			MinimalFaceArea = 0.4f * 0.4f,
			DistanceFilter = 0.2f,
			VoxelGridResolution = 100,
			MaxFaceTiltAngle = Degree.ToRadian(10),
			MinLayerFilling = 0.5f,
			MaxLayerReusage = 0.8f,
			MaxVoxelIslandConnections = 0.5f
		};

		private float minFaceArea;
		private float distFilter;
		private int voxResolution;
		private float maxFaceTiltAngle;
		private float minLayerFilling;
		private float maxLayerReusage;
		private float maxVoxIslandConnections;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the minimal area of the face for it to affect the box growing process.
		/// </summary>
		public float MinimalFaceArea
		{
			get { return this.minFaceArea; }
			set { this.minFaceArea = value; }
		}
		/// <summary>
		/// Gets or sets the minimal linear size of details that should be smoothed away.
		/// </summary>
		public float DistanceFilter
		{
			get { return this.distFilter; }
			set { this.distFilter = value; }
		}
		/// <summary>
		/// Gets or sets the resolution of the voxel grid to use to form the boxes.
		/// </summary>
		public int VoxelGridResolution
		{
			get { return this.voxResolution; }
			set { this.voxResolution = value; }
		}
		/// <summary>
		/// Gets or sets the maximal tolerable face tilt angle to use when aligning faces with boxes.
		/// </summary>
		public float MaxFaceTiltAngle
		{
			get { return this.maxFaceTiltAngle; }
			set { this.maxFaceTiltAngle = value; }
		}
		/// <summary>
		/// Gets or sets the value between 0 and 1 that indicates the percentage of the geometry that must
		/// be filled before the process of growing boxes stops.
		/// </summary>
		public float MinLayerFilling
		{
			get { return this.minLayerFilling; }
			set { this.minLayerFilling = value; }
		}
		/// <summary>
		/// Gets or sets the value between 0 and 1 that indicates the percentage of space occupied by a
		/// voxel grid cell that already had a box created for it that must be filled by any intersecting
		/// box before the latter stops growing.
		/// </summary>
		public float MaxLayerReusage
		{
			get { return this.maxLayerReusage; }
			set { this.maxLayerReusage = value; }
		}
		/// <summary>
		/// Gets or sets the maximal number of connections any isolated voxel island can have with being
		/// ignored(?).
		/// </summary>
		public float MaxVoxelIslandConnections
		{
			get { return this.maxVoxIslandConnections; }
			set { this.maxVoxIslandConnections = value; }
		}
		#endregion
	}
}