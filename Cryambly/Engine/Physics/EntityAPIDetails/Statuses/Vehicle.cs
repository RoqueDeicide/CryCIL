using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the object that is used to query the status of the physical entity that
	/// is a vehicle.
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
	public struct PhysicsStatusVehicle
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.GetStatus"/> to query information
		/// about the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsStatus Base;
		[UsedImplicitly] private float steer;
		[UsedImplicitly] private float pedal;
		[UsedImplicitly] private int bHandBrake;
		[UsedImplicitly] private float footbrake;
		[UsedImplicitly] private Vector3 vel;
		[UsedImplicitly] private int bWheelContact;
		[UsedImplicitly] private int iCurGear;
		[UsedImplicitly] private float engineRPM;
		[UsedImplicitly] private float clutch;
		[UsedImplicitly] private float drivingTorque;
		[UsedImplicitly] private int nActiveColliders;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the current angle of steering in radians.
		/// </summary>
		public float CurrentSteeringAngle
		{
			get { return this.steer; }
		}
		/// <summary>
		/// Gets the current position of the 'pedal' that defines where and how quickly the vehicle wants
		/// to go.
		/// </summary>
		public float CurrentPedalPosition
		{
			get { return this.pedal; }
		}
		/// <summary>
		/// Gets the value that indicates whether hand brake is active.
		/// </summary>
		public bool HandBrakeActive
		{
			get { return this.bHandBrake != 0; }
		}
		/// <summary>
		/// Gets the current velocity.
		/// </summary>
		public Vector3 Velocity
		{
			get { return this.vel; }
		}
		/// <summary>
		/// Gets the value that indicates whether at least one wheel is touching the ground.
		/// </summary>
		public bool ContactsGround
		{
			get { return this.bWheelContact != 0; }
		}
		/// <summary>
		/// Gets the zero-based index of the current gear.
		/// </summary>
		/// <remarks>
		/// Gears:
		/// <para>0 - backward;</para>
		/// <para>1 - neutral;</para>
		/// <para>2 and more - forward.</para>
		/// </remarks>
		public int CurrentGear
		{
			get { return this.iCurGear; }
		}
		/// <summary>
		/// Gets the number of revolutions per minute the engine is currently operating at.
		/// </summary>
		public float EngineRotationFrequency
		{
			get { return this.engineRPM; }
		}
		/// <summary>
		/// Gets the value from 0 to 1 that specifies the degree of clutch engagement.
		/// </summary>
		public float Clutch
		{
			get { return this.clutch; }
		}
		/// <summary>
		/// Gets the current driving axle torque.
		/// </summary>
		public float DrivingTorque
		{
			get { return this.drivingTorque; }
		}
		/// <summary>
		/// Gets the number of non-static entities this vehicle has contacts with.
		/// </summary>
		public int ActiveContactCount
		{
			get { return this.nActiveColliders; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass anything.</param>
		public PhysicsStatusVehicle([UsedImplicitly] int notUsed)
			: this()
		{
			this.Base = new PhysicsStatus(PhysicsStatusTypes.Vehicle);
		}
		#endregion
	}
}