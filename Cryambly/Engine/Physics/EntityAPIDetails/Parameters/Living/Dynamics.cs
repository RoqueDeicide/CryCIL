using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to change special simulation parameters of the physical
	/// entity that is a living entity.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsParametersDynamics
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to apply these
		/// onto physical entity or to <see cref="PhysicalEntity.GetParameters"/> to get the currently
		/// applied ones.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private float kInertia;
		[UsedImplicitly] private float kInertiaAccel;
		[UsedImplicitly] private float kAirControl;
		[UsedImplicitly] private float kAirResistance;
		[UsedImplicitly] private Vector3 gravity;
		[UsedImplicitly] private float nodSpeed;
		[UsedImplicitly] private int bSwimming;
		[UsedImplicitly] private float mass;
		[UsedImplicitly] private int surface_idx;
		[UsedImplicitly] private float minSlideAngle;
		[UsedImplicitly] private float maxClimbAngle;
		[UsedImplicitly] private float maxJumpAngle;
		[UsedImplicitly] private float minFallAngle;
		[UsedImplicitly] private float maxVelGround;
		[UsedImplicitly] private float timeImpulseRecover;
		[UsedImplicitly] private int collTypes;
		[UsedImplicitly] private PhysicalEntity pLivingEntToIgnore;
		[UsedImplicitly] private int bNetwork; // uses extended history information (obsolete)
		[UsedImplicitly] private int bActive;
		[UsedImplicitly] private int iRequestedTime;
		[UsedImplicitly] private int bReleaseGroundColliderWhenNotActive;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the acceleration coefficient that specifies how quickly the entity will the
		/// requested speed. 0 is a special value that means that the speed is reached instantly.
		/// </summary>
		[DefaultValue(8.0f)]
		public float AccelerationCoefficient
		{
			get { return this.kInertia; }
			set { this.kInertia = value; }
		}
		/// <summary>
		/// Gets or sets the value between 0 and 1 that specifies how strongly the requested velocity
		/// affects the movement of the entity when it's not touching the ground.
		/// </summary>
		/// <value>0 means no control whatsoever; 1 means total control.</value>
		public float AirControl
		{
			get { return this.kAirControl; }
			set { this.kAirControl = MathHelpers.Clamp(value, 0, 1); }
		}
		/// <summary>
		/// Gets or sets the value that specifies how quickly entity's velocity is dampened during flight.
		/// </summary>
		public float AirResistance
		{
			get { return this.kAirResistance; }
			set { this.kAirResistance = value; }
		}
		/// <summary>
		/// Gets or sets the normalized vector that represents direction of gravity vector that is used by
		/// this entity.
		/// </summary>
		/// <example>
		/// <code>
		/// params.Gravity = Vector3.Down;
		/// </code>
		/// </example>
		public Vector3 Gravity
		{
			get { return this.gravity; }
			set { this.gravity = value; }
		}
		/// <summary>
		/// Gets or sets the strength of camera reaction to landings.
		/// </summary>
		[DefaultValue(60.0f)]
		public float NodSpeed
		{
			get { return this.nodSpeed; }
			set { this.nodSpeed = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this entity is not bound to the ground plane.
		/// </summary>
		public bool Swimming
		{
			get { return this.bSwimming != 0; }
			set { this.bSwimming = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the mass of the entity in kilograms.
		/// </summary>
		public float Mass
		{
			get { return this.mass; }
			set { this.mass = value; }
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
		/// Gets or sets the minimal angle in radians of the slope that causes this entity to slide down.
		/// </summary>
		public float MinimalSlideAngle
		{
			get { return this.minSlideAngle; }
			set { this.minSlideAngle = value; }
		}
		/// <summary>
		/// Gets or sets angle of the steepest slope this entity can climb in radians.
		/// </summary>
		public float MaximalClimbAngle
		{
			get { return this.maxClimbAngle; }
			set { this.maxClimbAngle = value; }
		}
		/// <summary>
		/// Gets or sets angle of the steepest slope this entity can jump towards while on it in radians.
		/// </summary>
		public float MaximalJumpAngle
		{
			get { return this.maxJumpAngle; }
			set { this.maxJumpAngle = value; }
		}
		/// <summary>
		/// Gets or sets angle of the least steep slope this entity will start falling from in radians.
		/// </summary>
		public float MinimalFallAngle
		{
			get { return this.minFallAngle; }
			set { this.minFallAngle = value; }
		}
		/// <summary>
		/// Gets or sets maximal velocity magnitude (speed) of the ground this entity can stand on.
		/// </summary>
		public float MaximalGroundVelocity
		{
			get { return this.maxVelGround; }
			set { this.maxVelGround = value; }
		}
		/// <summary>
		/// Gets or sets the time it takes to recover from reception of the impulse.
		/// </summary>
		/// <remarks>
		/// Some sort of inertia value is applied to this entity for the duration after impulse.
		/// </remarks>
		public float ImpulseRecoveryTime
		{
			get { return this.timeImpulseRecover; }
			set { this.timeImpulseRecover = value; }
		}
		/// <summary>
		/// Gets or sets the types of objects this one can collide with.
		/// </summary>
		public ColliderTypes CollidesWith
		{
			get { return (ColliderTypes)this.collTypes; }
			set { this.collTypes = (int)value; }
		}
		/// <summary>
		/// Gets or sets the physical entity collisions with which will be ignored.
		/// </summary>
		/// <remarks>
		/// <para>Only works if the value is a living entity.</para>
		/// <para>Used in cooperative animations.</para>
		/// </remarks>
		public PhysicalEntity LivingEntityToIgnore
		{
			get { return this.pLivingEntToIgnore; }
			set { this.pLivingEntToIgnore = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this entity is active.
		/// </summary>
		/// <remarks>
		/// Inactive entities have most of the simulation disabled. The entity can still move in the same
		/// direction, and can only collide with physical entities of the same or higher simulation classes.
		/// </remarks>
		public bool Active
		{
			get { return this.bActive != 0; }
			set { this.bActive = value ? 1 : 0; }
		}
		/// <summary>
		/// Can be set to request this entity to roll back given number of time steps and re-execute all
		/// pending actions during next time step.
		/// </summary>
		/// <remarks>
		/// Probably used to roll back player's position when it turns out that his actions were not valid
		/// for some time (e.g. because of network desynchronization).
		/// </remarks>
		public int StepBack
		{
			set { this.iRequestedTime = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether collider geometry that is used to check the ground
		/// should be released when this entity becomes inactive.
		/// </summary>
		/// <remarks>Probably stops checks against environment when entity is not active.</remarks>
		public bool ReleaseGroundColliderWhenNotActive
		{
			get { return this.bReleaseGroundColliderWhenNotActive != 0; }
			set { this.bReleaseGroundColliderWhenNotActive = value ? 1 : 0; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass anything to invoke this constructor.</param>
		public PhysicsParametersDynamics([UsedImplicitly] int notUsed)
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.PlayerDynamics);
			this.kInertia = UnusedValue.Single;
			this.kInertiaAccel = UnusedValue.Single;
			this.kAirControl = UnusedValue.Single;
			this.kAirResistance = UnusedValue.Single;
			this.gravity = UnusedValue.Vector;
			this.nodSpeed = UnusedValue.Single;
			this.bSwimming = UnusedValue.Int32;
			this.mass = UnusedValue.Single;
			this.surface_idx = UnusedValue.Int32;
			this.minSlideAngle = UnusedValue.Single;
			this.maxClimbAngle = UnusedValue.Single;
			this.minFallAngle = UnusedValue.Single;
			this.maxJumpAngle = UnusedValue.Single;
			this.maxVelGround = UnusedValue.Single;
			this.timeImpulseRecover = UnusedValue.Single;
			this.collTypes = UnusedValue.Int32;
			this.pLivingEntToIgnore = new PhysicalEntity(UnusedValue.Pointer);
			this.bNetwork = UnusedValue.Int32;
			this.bActive = UnusedValue.Int32;
			this.iRequestedTime = UnusedValue.Int32;
			this.bReleaseGroundColliderWhenNotActive = UnusedValue.Int32;
		}
		#endregion
	}
}