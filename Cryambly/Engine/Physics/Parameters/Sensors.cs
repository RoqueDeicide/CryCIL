using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Engine.Memory;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to the physical entity.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct PhysicsParametersSensors
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParams"/> to apply these onto
		/// physical entity or to <see cref="PhysicalEntity.GetParams"/> to get the currently applied ones.
		/// </summary>
		[UsedImplicitly]
		public PhysicsParameters Base;
		private readonly Ray[] sensorsAsRays;	// Used when getting sensors.

		[UsedImplicitly]
		private readonly Vector3* origins;		//
		[UsedImplicitly]						//
		private readonly Vector3* dirs;			// Used when setting sensors.
		[UsedImplicitly]						//
		private readonly int count;				//
		#endregion
		#region Properties
		/// <summary>
		/// Gets an array of sensors (All coordinates are in local entity coordinate space).
		/// </summary>
		public Ray[] Sensors
		{
			get { return this.sensorsAsRays; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes new object of this type.
		/// </summary>
		/// <param name="sensors">
		/// An array of rays in local entity space that are used as sensors. Pass <c>null</c> when querying
		/// which sensors the entity has.
		/// </param>
		public PhysicsParametersSensors([CanBeNull] Ray[] sensors)
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.Sensors);
			this.origins = null;
			this.dirs = null;
			this.count = 0;
			this.sensorsAsRays = null;
			if (sensors == null)
			{
				return;
			}
			fixed (Ray* rays = sensors)
			{
				ulong memorySize = (ulong)(Vector3.ByteCount * sensors.Length);
				this.origins = (Vector3*)CryMarshal.Allocate(memorySize, false).ToPointer();
				this.dirs = (Vector3*)CryMarshal.Allocate(memorySize, false).ToPointer();
				this.count = sensors.Length;

				for (int i = 0; i < this.count; i++)
				{
					this.origins[i] = rays[i].Position;
					this.dirs[i] = rays[i].Direction;
				}
			}
		}
		#endregion
	}
}