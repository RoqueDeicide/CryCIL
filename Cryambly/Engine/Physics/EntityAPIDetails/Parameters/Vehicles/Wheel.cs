using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to specify the wheel of the vehicle.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsParametersWheel
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to apply these
		/// onto physical entity or to <see cref="PhysicalEntity.GetParameters"/> to get the currently
		/// applied ones.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private int iWheel;
		[UsedImplicitly] private int bDriving;
		[UsedImplicitly] private int iAxle;
		[UsedImplicitly] private int bCanBrake;
		[UsedImplicitly] private int bBlocked;
		[UsedImplicitly] private int bCanSteer;
		[UsedImplicitly] private float suspLenMax;
		[UsedImplicitly] private float suspLenInitial;
		[UsedImplicitly] private float minFriction;
		[UsedImplicitly] private float maxFriction;
		[UsedImplicitly] private int surface_idx;
		[UsedImplicitly] private int bRayCast;
		[UsedImplicitly] private float kStiffness;
		[UsedImplicitly] private float kStiffnessWeight;
		[UsedImplicitly] private float kDamping;
		[UsedImplicitly] private float kLatFriction;
		[UsedImplicitly] private float Tscale;
		[UsedImplicitly] private float w;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the zero-based index of the wheel to get/set parameters from/for.
		/// </summary>
		public int WheelIndex => this.iWheel;
		/// <summary>
		/// Gets or sets the value that indicates whether this wheel is a driving wheel.
		/// </summary>
		public bool Driving
		{
			get { return this.bDriving != 0; }
			set { this.bDriving = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the index of the axle this wheel is on.
		/// </summary>
		/// <remarks>
		/// wheels on the same axle align their coordinates (if only slightly misaligned) and apply
		/// stabilizer force (if set); axle&lt;0 means the wheel does not affect the physics.
		/// </remarks>
		public int AxleIndex
		{
			get { return this.iAxle; }
			set { this.iAxle = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this wheel is affected by the hand brake.
		/// </summary>
		public bool HasHandBrake
		{
			get { return this.bCanBrake != 0; }
			set { this.bCanBrake = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this wheel is blocked.
		/// </summary>
		/// <remarks>This property can be used to force the wheel into hand brake state.</remarks>
		public bool Blocked
		{
			get { return this.bBlocked != 0; }
			set { this.bBlocked = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this wheel can steer.
		/// </summary>
		public bool CanSteer
		{
			get { return this.bCanSteer != 0; }
			set { this.bCanSteer = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the length of the suspension 'spring' in fully relaxed state.
		/// </summary>
		public float RelaxedSuspensionLength
		{
			get { return this.suspLenMax; }
			set { this.suspLenMax = value; }
		}
		/// <summary>
		/// Gets or sets the length of the suspension 'spring' in rest state.
		/// </summary>
		public float RestingSuspensionLength
		{
			get { return this.suspLenInitial; }
			set { this.suspLenInitial = value; }
		}
		/// <summary>
		/// Gets or sets the minimal amount of friction that can be induced by the wheel.
		/// </summary>
		/// <remarks>
		/// <para>Amount of friction depends how deep the wheel is in the ground.</para>
		/// </remarks>
		public float MinimalTireFriction
		{
			get { return this.minFriction; }
			set { this.minFriction = value; }
		}
		/// <summary>
		/// Gets or sets the maximal amount of friction that can be induced by the wheel.
		/// </summary>
		/// <remarks>
		/// <para>Amount of friction depends how deep the wheel is in the ground.</para>
		/// </remarks>
		public float MaximalTireFriction
		{
			get { return this.maxFriction; }
			set { this.maxFriction = value; }
		}
		/// <summary>
		/// Gets or sets the object that represents the surface that is used by this entity for collisions.
		/// </summary>
		public PhysicalSurface Surface
		{
			get { return new PhysicalSurface(this.surface_idx); }
			set { this.surface_idx = value.Index; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this wheel uses more simple ray-casting instead of
		/// cylinder geometry.
		/// </summary>
		public bool RayCasts
		{
			get { return this.bRayCast != 0; }
			set { this.bRayCast = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the stiffness of suspension. If left equal to default value, the stiffness will be
		/// automatically calculated from <see cref="RelaxedSuspensionLength"/> and
		/// <see cref="RestingSuspensionLength"/>.
		/// </summary>
		public float SuspensionStiffness
		{
			get { return this.kStiffness; }
			set { this.kStiffness = value; }
		}
		/// <summary>
		/// Gets or sets the weight of the wheel that will only be used when auto-calculating
		/// <see cref="SuspensionStiffness"/>.
		/// </summary>
		public float SuspensionStiffnessWeight
		{
			get { return this.kStiffnessWeight; }
			set { this.kStiffnessWeight = value; }
		}
		/// <summary>
		/// Gets or sets the damping value that is used calm down oscillations.
		/// </summary>
		public float SuspensionDamping
		{
			get { return this.kDamping; }
			set { this.kDamping = value; }
		}
		/// <summary>
		/// Gets or sets lateral friction coefficient.
		/// </summary>
		public float LateralFrictionCoefficient
		{
			get { return this.kLatFriction; }
			set { this.kLatFriction = value; }
		}
		/// <summary>
		/// Gets or sets the scale of torque that is applied to this wheel.
		/// </summary>
		public float TorqueScale
		{
			get { return this.Tscale; }
			set { this.Tscale = value; }
		}
		/// <summary>
		/// Gets or sets the angular velocity of this wheel around the axle.
		/// </summary>
		public float AngularVelocity
		{
			get { return this.w; }
			set { this.w = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="wheel">Zero-based index of the wheel to customize.</param>
		public PhysicsParametersWheel(int wheel)
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.Wheel);
			this.iWheel = wheel;
			this.bDriving = UnusedValue.Int32;
			this.iAxle = UnusedValue.Int32;
			this.bBlocked = UnusedValue.Int32;
			this.bCanBrake = UnusedValue.Int32;
			this.bCanSteer = UnusedValue.Int32;
			this.suspLenMax = UnusedValue.Single;
			this.suspLenInitial = UnusedValue.Single;
			this.minFriction = UnusedValue.Single;
			this.maxFriction = UnusedValue.Single;
			this.surface_idx = UnusedValue.Int32;
			this.bRayCast = UnusedValue.Int32;
			this.kStiffness = UnusedValue.Single;
			this.kStiffnessWeight = UnusedValue.Single;
			this.kDamping = UnusedValue.Single;
			this.kLatFriction = UnusedValue.Single;
			this.Tscale = UnusedValue.Single;
			this.w = UnusedValue.Single;
		}
		#endregion
	}
}