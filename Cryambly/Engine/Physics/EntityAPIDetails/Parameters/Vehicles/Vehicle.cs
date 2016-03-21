using System;
using System.Linq;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Engine.Memory;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to specify the physical entity that is a vehicle.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct PhysicsParametersVehicle
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to apply these
		/// onto physical entity or to <see cref="PhysicalEntity.GetParameters"/> to get the currently
		/// applied ones.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private float axleFriction;
		[UsedImplicitly] private float enginePower;
		[UsedImplicitly] private float maxSteer;
		[UsedImplicitly] private float engineMaxRPM;
		[UsedImplicitly] private float brakeTorque;
		[UsedImplicitly] private int iIntegrationType; // for suspensions; 0-explicit Euler, 1-implicit Euler
		[UsedImplicitly] private float maxTimeStep;
		[UsedImplicitly] private float minEnergy;
		[UsedImplicitly] private float damping;
		[UsedImplicitly] private float minBrakingFriction;
		[UsedImplicitly] private float maxBrakingFriction;
		[UsedImplicitly] private float kStabilizer;
		[UsedImplicitly] private int nWheels;
		[UsedImplicitly] private float engineMinRPM;
		[UsedImplicitly] private float engineShiftUpRPM;
		[UsedImplicitly] private float engineShiftDownRPM;
		[UsedImplicitly] private float engineIdleRPM;
		[UsedImplicitly] private float engineStartRPM;
		[UsedImplicitly] private float clutchSpeed;
		[UsedImplicitly] private int nGears;
		[UsedImplicitly] private float* gearRatios;
		[UsedImplicitly] private int maxGear, minGear; // additional gear index clamping
		[UsedImplicitly] private float slipThreshold;
		[UsedImplicitly] private float gearDirSwitchRPM; // RPM threshold for switching back and forward gears
		[UsedImplicitly] private float kDynFriction;
		[UsedImplicitly] private float steerTrackNeutralTurn;
		[UsedImplicitly] private float pullTilt; // for tracked vehicles, tilt angle of pulling force towards ground
		[UsedImplicitly] private float maxTilt;
		// maximum wheel contact normal tilt (left or right) after which it acts as a locked part of the
		// hull; it's a cosine of the angle
		[UsedImplicitly] private int bKeepTractionWhenTilted; // keeps wheel traction in tilted mode
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the magnitude of friction torque that is caused by axles and gearbox.
		/// </summary>
		public float AxleFriction
		{
			get { return this.axleFriction; }
			set { this.axleFriction = value; }
		}
		/// <summary>
		/// Gets or sets the power of the engine in kilowatts.
		/// </summary>
		/// <remarks>Usual values are 10 000 to 100 000 kW (13.404 to 134.04 hp).</remarks>
		public float EnginePower
		{
			get { return this.enginePower; }
			set { this.enginePower = value; }
		}
		/// <summary>
		/// Gets or sets the power of the engine in horsepower.
		/// </summary>
		/// <remarks>Usual values are 13.404 to 134.04 hp (10 000 to 100 000 kW).</remarks>
		public float EnginePowerInHorsePower
		{
			get { return this.enginePower / 746.0f; }
			set { this.enginePower = value * 746.0f; }
		}
		/// <summary>
		/// Gets or sets maximal number of revolutions per minute the engine can reach.
		/// </summary>
		/// <remarks>At this rate the engines torque is 0.</remarks>
		public float EngineMaxRotationFrequency
		{
			get { return this.engineMaxRPM; }
			set { this.engineMaxRPM = value; }
		}
		/// <summary>
		/// Gets or sets the minimal number of revolutions per minute the engine can be at.
		/// </summary>
		/// <remarks>
		/// When braking with the engine the clutch is automatically disengaged when at this speed.
		/// </remarks>
		public float EngineMinRotationFrequency
		{
			get { return this.engineMinRPM; }
			set { this.engineMinRPM = value; }
		}
		/// <summary>
		/// Gets or sets the number of revolutions per minute the engine operates at while idling.
		/// </summary>
		public float EngineIdleRotationFrequency
		{
			get { return this.engineIdleRPM; }
			set { this.engineIdleRPM = value; }
		}
		/// <summary>
		/// Gets or sets the number of revolutions per minute the engine goes to when it starts.
		/// </summary>
		public float EngineInitialRotationFrequency
		{
			get { return this.engineStartRPM; }
			set { this.engineStartRPM = value; }
		}
		/// <summary>
		/// Gets or sets the number of revolutions per minute the engine has to reach for automatic gearbox
		/// to shift the gear up.
		/// </summary>
		public float EngineShiftUpRotationFrequency
		{
			get { return this.engineShiftUpRPM; }
			set { this.engineShiftUpRPM = value; }
		}
		/// <summary>
		/// Gets or sets the number of revolutions per minute the engine has to reach for automatic gearbox
		/// to shift the gear down.
		/// </summary>
		public float EngineShiftDownRotationFrequency
		{
			get { return this.engineShiftDownRPM; }
			set { this.engineShiftDownRPM = value; }
		}
		/// <summary>
		/// Gets or sets the magnitude of torque that is applied when braking with the engine.
		/// </summary>
		public float EngineBrakeTorque
		{
			get { return this.brakeTorque; }
			set { this.brakeTorque = value; }
		}
		/// <summary>
		/// Gets or sets the speed of clutch (dis-)engagement.
		/// </summary>
		public float ClutchChangeSpeed
		{
			get { return this.clutchSpeed; }
			set { this.clutchSpeed = value; }
		}
		/// <summary>
		/// Gets or sets the array of gear ratios.
		/// </summary>
		/// <remarks>Indices of gears: 0 - backwards; 1 - neutral; 2 and above - forward.</remarks>
		/// <exception cref="ArgumentException">At least 3 gear ratios need to be provided.</exception>
		/// <exception cref="OutOfMemoryException">Unable to allocate native memory block.</exception>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		[CanBeNull]
		public float[] Gears
		{
			get
			{
				if (this.gearRatios == null || this.nGears == 0)
				{
					return null;
				}

				float[] gears = new float[this.nGears];

				fixed (float* gearsPtr = gears)
				{
					for (int i = 0; i < this.nGears; i++)
					{
						gearsPtr[i] = this.gearRatios[i];
					}
				}

				return gears;
			}
			set
			{
				if (value.IsNullOrTooSmall(3))
				{
					throw new ArgumentException("At least 3 gear ratios need to be provided.");
				}

				float* gearRatios = (float*)CryMarshal.Allocate((ulong)(sizeof(float) * value.Length), false).ToPointer();

				fixed (float* gearsPtr = value)
				{
					for (int i = 0; i < value.Length; i++)
					{
						gearRatios[i] = gearsPtr[i];
					}
				}

				this.gearRatios = gearRatios;
				this.nGears = value.Length;
			}
		}
		/// <summary>
		/// Gets or sets the maximal steering angle.
		/// </summary>
		public float MaximalSteeringAngle
		{
			get { return this.maxSteer; }
			set { this.maxSteer = value; }
		}
		/// <summary>
		/// Gets or sets maximal length of simulation time step when the vehicle is not touching anything
		/// with its body.
		/// </summary>
		public float MaximalTimeStep
		{
			get { return this.maxTimeStep; }
			set { this.maxTimeStep = value; }
		}
		/// <summary>
		/// Gets or sets minimal kinetic energy the vehicle can have without switching to 'sleep' state when
		/// the vehicle is not touching anything with its body.
		/// </summary>
		public float MinimalEnergy
		{
			get { return this.minEnergy; }
			set { this.minEnergy = value; }
		}
		/// <summary>
		/// Gets or sets the magnitude of damping that is applied to vehicle velocity when the it is not
		/// touching anything with its body.
		/// </summary>
		public float Damping
		{
			get { return this.damping; }
			set { this.damping = value; }
		}
		/// <summary>
		/// Gets or sets minimal braking friction limit when using the hand-brake.
		/// </summary>
		public float MinimalBrakingFriction
		{
			get { return this.minBrakingFriction; }
			set { this.minBrakingFriction = value; }
		}
		/// <summary>
		/// Gets or sets maximal braking friction limit when using the hand-brake.
		/// </summary>
		public float MaximalBrakingFriction
		{
			get { return this.maxBrakingFriction; }
			set { this.maxBrakingFriction = value; }
		}
		/// <summary>
		/// Gets or sets the stabilizing force that is applied as a multiplier to respective suspensions.
		/// </summary>
		public float Stabilizer
		{
			get { return this.kStabilizer; }
			set { this.kStabilizer = value; }
		}
		/// <summary>
		/// Gets or sets number of wheels.
		/// </summary>
		public int WheelCount
		{
			get { return this.nWheels; }
			set { this.nWheels = value; }
		}
		/// <summary>
		/// Gets or sets lateral speed threshold that switches wheels into slipping mode when passed.
		/// </summary>
		public float SlipThreshold
		{
			get { return this.slipThreshold; }
			set { this.slipThreshold = value; }
		}
		/// <summary>
		/// Gets or sets the modifier that is applied to the friction when wheels are slipping.
		/// </summary>
		public float SlippingFrictionModifier
		{
			get { return this.kDynFriction; }
			set { this.kDynFriction = value; }
		}
		/// <summary>
		/// Gets or sets the angle of steering for vehicles with tracks (tanks, bulldozers) at which the
		/// vehicle starts turning in place.
		/// </summary>
		public float NeutralTurnSteeringWithTracks
		{
			get { return this.steerTrackNeutralTurn; }
			set { this.steerTrackNeutralTurn = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass anything.</param>
		public PhysicsParametersVehicle([UsedImplicitly] int notUsed)
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.Vehicle);
			this.axleFriction = UnusedValue.Single;
			this.maxSteer = UnusedValue.Single;
			this.enginePower = UnusedValue.Single;
			this.engineMaxRPM = UnusedValue.Single;
			this.brakeTorque = UnusedValue.Single;
			this.iIntegrationType = UnusedValue.Int32;
			this.maxTimeStep = UnusedValue.Single;
			this.minEnergy = UnusedValue.Single;
			this.damping = UnusedValue.Single;
			this.maxBrakingFriction = UnusedValue.Single;
			this.minBrakingFriction = UnusedValue.Single;
			this.kStabilizer = UnusedValue.Single;
			this.nWheels = UnusedValue.Int32;
			this.engineMinRPM = UnusedValue.Single;
			this.engineShiftUpRPM = UnusedValue.Single;
			this.engineShiftDownRPM = UnusedValue.Single;
			this.clutchSpeed = UnusedValue.Single;
			this.engineIdleRPM = UnusedValue.Single;
			this.engineStartRPM = UnusedValue.Single;
			this.nGears = UnusedValue.Int32;
			this.gearRatios = (float*)UnusedValue.Pointer.ToPointer();
			this.maxGear = UnusedValue.Int32;
			this.minGear = UnusedValue.Int32;
			this.gearDirSwitchRPM = UnusedValue.Single;
			this.slipThreshold = UnusedValue.Single;
			this.kDynFriction = UnusedValue.Single;
			this.steerTrackNeutralTurn = UnusedValue.Single;
			this.pullTilt = UnusedValue.Single;
			this.maxTilt = UnusedValue.Single;
			this.bKeepTractionWhenTilted = UnusedValue.Int32;
		}
		#endregion
	}
}