using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the object that is used to query what the vehicle can do when steering.
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
	public struct PhysicsStatusVehicleAbilities
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.GetStatus"/> to query information
		/// about the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsStatus Base;
		[UsedImplicitly] private float steer;
		[UsedImplicitly] private Vector3 rotPivot;
		[UsedImplicitly] private float maxVelocity;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the coordinates of the turning circle center.
		/// </summary>
		public Vector3 TurningPivot
		{
			get { return this.rotPivot; }
		}
		/// <summary>
		/// Gets the maximal speed the vehicle can achieve along horizontal plane (steering is ignored).
		/// </summary>
		public float MaxSpeed
		{
			get { return this.maxVelocity; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="steer">
		/// Steering angle to try. Pass <see cref="UnusedValue.Int32"/> if you are only interested in
		/// maximal speed.
		/// </param>
		public PhysicsStatusVehicleAbilities(float steer)
			: this()
		{
			this.Base = new PhysicsStatus(PhysicsStatusTypes.VehicleAbilities);
			this.steer = steer;
		}
		#endregion
	}
}