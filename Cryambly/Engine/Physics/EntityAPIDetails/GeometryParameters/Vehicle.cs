using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of parts of the vehicle.
	/// </summary>
	public enum VehiclePartType
	{
		/// <summary>
		/// Body of the vehicle.
		/// </summary>
		Body = -1,
		/// <summary>
		/// Wheel of the vehicle that is not a driving one.
		/// </summary>
		NormalWheel = 0,
		/// <summary>
		/// A driving wheel.
		/// </summary>
		DrivingWheel = 1
	}
	/// <summary>
	/// Encapsulates a set of parameters that specify the geometry that is used by the physical entity that
	/// is a vehicle.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct GeometryParametersVehicle
	{
		#region Fields
		/// <summary>
		/// Pass a reference to this field to <see cref="PhysicalEntity.AddBody"/> to add the geometry
		/// with these parameters.
		/// </summary>
		public GeometryParameters Base;
		[UsedImplicitly] private VehiclePartType bDriving;
		[UsedImplicitly] private int iAxle; // wheel axle, currently not used
		[UsedImplicitly] private int bCanBrake;
		[UsedImplicitly] private int bRayCast;
		[UsedImplicitly] private int bCanSteer;
		[UsedImplicitly] private Vector3 pivot;
		[UsedImplicitly] private float lenMax;
		[UsedImplicitly] private float lenInitial;
		[UsedImplicitly] private float kStiffness;
		[UsedImplicitly] private float kStiffnessWeight;
		[UsedImplicitly] private float kDamping;
		[UsedImplicitly] private float minFriction, maxFriction;
		[UsedImplicitly] private float kLatFriction;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the type of the part of the vehicle this geometry will be used as.
		/// </summary>
		public VehiclePartType Part
		{
			get { return this.bDriving; }
			set { this.bDriving = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this wheel is locked when the driver uses hand
		/// brake.
		/// </summary>
		/// <remarks>
		/// This property is ignored when <see cref="Part"/> is equal to
		/// <see cref="VehiclePartType.Body"/>.
		/// </remarks>
		public bool LockableWithHandBrake
		{
			get { return this.bCanBrake != 0; }
			set { this.bCanBrake = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this wheel uses more simple but less reliable ray
		/// casting instead of geometry sweep check.
		/// </summary>
		/// <remarks>
		/// This property is ignored when <see cref="Part"/> is equal to
		/// <see cref="VehiclePartType.Body"/>.
		/// </remarks>
		public bool RayCasting
		{
			get { return this.bRayCast != 0; }
			set { this.bRayCast = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this wheel can be used for steering.
		/// </summary>
		/// <remarks>
		/// This property is ignored when <see cref="Part"/> is equal to
		/// <see cref="VehiclePartType.Body"/>.
		/// </remarks>
		public bool CanSteer
		{
			get { return this.bCanSteer != 0; }
			set { this.bCanSteer = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the position of the wheel's upper suspension point.
		/// </summary>
		/// <remarks>
		/// This property is ignored when <see cref="Part"/> is equal to
		/// <see cref="VehiclePartType.Body"/>.
		/// </remarks>
		public Vector3 Pivot
		{
			get { return this.pivot; }
			set { this.pivot = value; }
		}
		/// <summary>
		/// Gets or sets the length of the suspension 'spring' in fully relaxed state.
		/// </summary>
		/// <remarks>
		/// This property is ignored when <see cref="Part"/> is equal to
		/// <see cref="VehiclePartType.Body"/>.
		/// </remarks>
		public float RelaxedSuspensionLength
		{
			get { return this.lenMax; }
			set { this.lenMax = value; }
		}
		/// <summary>
		/// Gets or sets the length of the suspension 'spring' in rest state.
		/// </summary>
		/// <remarks>
		/// This property is ignored when <see cref="Part"/> is equal to
		/// <see cref="VehiclePartType.Body"/>.
		/// </remarks>
		public float RestingSuspensionLength
		{
			get { return this.lenInitial; }
			set { this.lenInitial = value; }
		}
		/// <summary>
		/// Gets or sets the stiffness of suspension. If left equal to default value, the stiffness will be
		/// automatically calculated from <see cref="RelaxedSuspensionLength"/> and
		/// <see cref="RestingSuspensionLength"/>.
		/// </summary>
		/// <remarks>
		/// This property is ignored when <see cref="Part"/> is equal to
		/// <see cref="VehiclePartType.Body"/>.
		/// </remarks>
		public float SuspensionStiffness
		{
			get { return this.kStiffness; }
			set { this.kStiffness = value; }
		}
		/// <summary>
		/// Gets or sets the weight of the wheel that will only be used when auto-calculating
		/// <see cref="SuspensionStiffness"/>.
		/// </summary>
		/// <remarks>
		/// This property is ignored when <see cref="Part"/> is equal to
		/// <see cref="VehiclePartType.Body"/>.
		/// </remarks>
		public float SuspensionStiffnessWeight
		{
			get { return this.kStiffnessWeight; }
			set { this.kStiffnessWeight = value; }
		}
		/// <summary>
		/// Gets or sets the damping value that is used calm down oscillations.
		/// </summary>
		/// <remarks>
		/// This property is ignored when <see cref="Part"/> is equal to
		/// <see cref="VehiclePartType.Body"/>.
		/// </remarks>
		public float SuspensionDamping
		{
			get { return this.kDamping; }
			set { this.kDamping = value; }
		}
		/// <summary>
		/// Gets or sets the minimal amount of friction that can be induced by the wheel.
		/// </summary>
		/// <remarks>
		/// <para>Amount of friction depends how deep the wheel is in the ground.</para>
		/// <para>
		/// This property is ignored when <see cref="Part"/> is equal to
		/// <see cref="VehiclePartType.Body"/>.
		/// </para>
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
		/// <para>
		/// This property is ignored when <see cref="Part"/> is equal to
		/// <see cref="VehiclePartType.Body"/>.
		/// </para>
		/// </remarks>
		public float MaximalTireFriction
		{
			get { return this.maxFriction; }
			set { this.maxFriction = value; }
		}
		/// <summary>
		/// Gets or sets lateral friction coefficient.
		/// </summary>
		/// <remarks>
		/// This property is ignored when <see cref="Part"/> is equal to
		/// <see cref="VehiclePartType.Body"/>.
		/// </remarks>
		public float LateralFrictionCoefficient
		{
			get { return this.kLatFriction; }
			set { this.kLatFriction = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass this parameter to invoked this constructor.</param>
		public GeometryParametersVehicle([UsedImplicitly] int notUsed)
		{
			this.Base = new GeometryParameters(notUsed)
			{
				type = PhysicsGeometryParametersTypes.Vehicle
			};
			this.bDriving = (VehiclePartType)UnusedValue.Int32;
			this.minFriction = UnusedValue.Single;
			this.maxFriction = UnusedValue.Single;
			this.bRayCast = UnusedValue.Int32;
			this.kLatFriction = UnusedValue.Single;
			this.bCanBrake = 1;
			this.bCanSteer = 1;
			this.kStiffnessWeight = 1.0f;
			this.iAxle = 0;
			this.pivot = new Vector3();
			this.lenMax = 0;
			this.lenInitial = 0;
			this.kStiffness = 0;
			this.kDamping = 0;
		}
		#endregion
	}
}