using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the action that changes the movement status of the physical entity that
	/// is a vehicle.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.ActUpon"/> the return value is an indication of success.
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsActionDrive
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.ActUpon"/> to apply this action
		/// to the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsAction Base;
		[UsedImplicitly] private float pedal;
		[UsedImplicitly] private float dpedal;
		[UsedImplicitly] private float steer;
		[UsedImplicitly] private float ackermanOffset;
		[UsedImplicitly] private float dsteer;
		[UsedImplicitly] private float clutch;
		[UsedImplicitly] private int bHandBrake;
		[UsedImplicitly] private int iGear;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the value that represents an absolute position of the pedal.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Having pedal not equal to 0 causes to the vehicle to never fall asleep as a physical entity.
		/// </para>
		/// <para>Pedal position represents the direction and magnitude of vehicle acceleration.</para>
		/// </remarks>
		public float Pedal
		{
			get { return this.pedal; }
			set { this.pedal = value; }
		}
		/// <summary>
		/// Gets or sets the value that represents a change to the absolute position of the pedal.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Having pedal not equal to 0 causes to the vehicle to never fall asleep as a physical entity.
		/// </para>
		/// <para>Pedal position represents the direction and magnitude of vehicle acceleration.</para>
		/// </remarks>
		public float PedalDelta
		{
			get { return this.dpedal; }
			set { this.dpedal = value; }
		}
		/// <summary>
		/// Gets or sets the absolute angle of the steering wheel.
		/// </summary>
		public float SteeringAngle
		{
			get { return this.steer; }
			set { this.steer = value; }
		}
		/// <summary>
		/// Gets or sets the change to the angle of the steering wheel.
		/// </summary>
		public float SteeringAngleDelta
		{
			get { return this.dsteer; }
			set { this.dsteer = value; }
		}
		/// <summary>
		/// Gets or sets the value from 0 to 1 that defines the ratio between steering of front wheels to
		/// steering of back wheels.
		/// </summary>
		/// <value>
		/// <para>
		/// If 0 is passed (it's a default value), then normal steering will be used: front wheels turn,
		/// back ones are fixed.
		/// </para>
		/// <para>
		/// If 1 is passed, then back wheels will turn, the front one will be fixed (can be used for smth
		/// like a forklift).
		/// </para>
		/// <para>If 0.5 is passed, then both front and back wheels will turn equally.</para>
		/// </value>
		public float FrontToBackSteeringOffset
		{
			get { return this.ackermanOffset; }
			set { this.ackermanOffset = value.GetClamped(0, 1); }
		}
		/// <summary>
		/// Gets or sets the value between 0 and 1 that represents the clutch engagement ratio.
		/// </summary>
		public float Clutch
		{
			get { return this.clutch; }
			set { this.clutch = value.GetClamped(0, 1); }
		}
		/// <summary>
		/// Gets or sets the index of current gear.
		/// </summary>
		/// <remarks>0 means back gear, 1 is neutral and everything above is forward gears.</remarks>
		public int Gear
		{
			get { return this.iGear; }
			set { this.iGear = value.GetClamped(0, int.MaxValue); }
		}
		/// <summary>
		/// Gets or sets
		/// </summary>
		public bool HandBrake
		{
			get { return this.bHandBrake != 0; }
			set { this.bHandBrake = value ? 1 : 0; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass anything.</param>
		public PhysicsActionDrive([UsedImplicitly] int notUsed)
		{
			this.Base = new PhysicsAction(PhysicsActionTypes.Drive);
			this.pedal = UnusedValue.Single;
			this.dpedal = UnusedValue.Single;
			this.steer = UnusedValue.Single;
			this.ackermanOffset = UnusedValue.Single;
			this.dsteer = UnusedValue.Single;
			this.clutch = UnusedValue.Single;
			this.bHandBrake = UnusedValue.Int32;
			this.iGear = UnusedValue.Int32;
		}
		#endregion
	}
}