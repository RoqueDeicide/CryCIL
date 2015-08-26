using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates information about a point that was touched by the entity's sensor.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct SensedPoint
	{
		/// <summary>
		/// Coordinates of the point.
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// Normal vector to the surface at the point.
		/// </summary>
		public Vector3 Normal;
	}
	/// <summary>
	/// Encapsulates description of the object that is used to query information from the sensors that were
	/// assigned to the physical entity.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.GetStatus"/> the return value is an indication of success.
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct PhysicsStatusSensors
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.GetStatus"/> to query information
		/// about the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsStatus Base;
		[UsedImplicitly] private Vector3* points;
		[UsedImplicitly] private Vector3* normals;
		[UsedImplicitly] private int flags;
		private readonly int sensorCount;
		#endregion
		#region Properties
		/// <summary>
		/// Gets an array of points that were touched by respective sensors.
		/// </summary>
		[CanBeNull]
		public SensedPoint[] Points
		{
			get
			{
				if (this.sensorCount <= 0)
				{
					return null;
				}

				SensedPoint[] ps = new SensedPoint[this.sensorCount];

				for (int i = 0; i < this.sensorCount; i++)
				{
					ps[i] = new SensedPoint
					{
						Position = this.points[i],
						Normal = this.normals[i]
					};
				}

				return ps;
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="sensorCount">Number of sensors attached to the entity.</param>
		public PhysicsStatusSensors(int sensorCount)
			: this()
		{
			this.Base = new PhysicsStatus(PhysicsStatusTypes.Sensors);
			this.sensorCount = sensorCount;
		}
		#endregion
	}
}