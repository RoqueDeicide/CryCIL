using System;
using System.Linq;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to specify the physical entity that is particle.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsParametersParticle
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to apply these
		/// onto physical entity or to <see cref="PhysicalEntity.GetParameters"/> to get the currently
		/// applied ones.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private uint flags;
		[UsedImplicitly] private float mass;
		[UsedImplicitly] private float size;
		[UsedImplicitly] private float thickness;
		[UsedImplicitly] private Vector3 heading;
		[UsedImplicitly] private float velocity;
		[UsedImplicitly] private float kAirResistance;
		[UsedImplicitly] private float kWaterResistance;
		[UsedImplicitly] private float accThrust;
		[UsedImplicitly] private float accLift;
		[UsedImplicitly] private int surface_idx;
		[UsedImplicitly] private EulerAngles wspin;
		[UsedImplicitly] private Vector3 gravity;
		[UsedImplicitly] private Vector3 waterGravity;
		[UsedImplicitly] private Vector3 normal;
		[UsedImplicitly] private Vector3 rollAxis;
		[UsedImplicitly] private Quaternion q0;
		[UsedImplicitly] private float minBounceVel;
		[UsedImplicitly] private float minVel;
		[UsedImplicitly] private PhysicalEntity pColliderToIgnore;
		[UsedImplicitly] private int iPierceability;
		[UsedImplicitly] private int collTypes;
		[UsedImplicitly] private int areaCheckPeriod;
		[UsedImplicitly] private int dontPlayHitEffect;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets a set of flags that are specific to this entity.
		/// </summary>
		/// <remarks>This is where PhysicalEntity.Particle* flags are used.</remarks>
		public PhysicalEntityFlags Flags
		{
			get { return (PhysicalEntityFlags)this.flags; }
			set { this.flags = (uint)value; }
		}
		/// <summary>
		/// Gets or sets the mass of this particle in kilograms.
		/// </summary>
		public float Mass
		{
			get { return this.mass; }
			set { this.mass = value; }
		}
		/// <summary>
		/// Gets or sets the radius of the sphere that is used by the particle to trace geometry in front of
		/// it.
		/// </summary>
		public float FlyingRadius
		{
			get { return this.size; }
			set { this.size = value * 2; }
		}
		/// <summary>
		/// Gets or sets the distance from the ground this particle positions itself at when lying.
		/// </summary>
		/// <remarks>This property is optional.</remarks>
		public float LyingRadius
		{
			get { return this.thickness; }
			set { this.thickness = value * 2; }
		}
		/// <summary>
		/// Gets or sets the normalized vector that represents direction of this particle's movement.
		/// </summary>
		public Vector3 Heading
		{
			get { return this.heading; }
			set { this.heading = value; }
		}
		/// <summary>
		/// Gets or sets the speed of particle's movement.
		/// </summary>
		public float Speed
		{
			get { return this.velocity; }
			set { this.velocity = value; }
		}
		/// <summary>
		/// Gets or sets the vector of this particle's velocity.
		/// </summary>
		public Vector3 Velocity
		{
			get { return this.heading * this.velocity; }
			set
			{
				this.heading = value;
				this.heading.Normalize();
				this.velocity = value.Length;
			}
		}
		/// <summary>
		/// Gets or sets the angular velocity of this particle.
		/// </summary>
		public EulerAngles AngularVelocity
		{
			get { return this.wspin; }
			set { this.wspin = value; }
		}
		/// <summary>
		/// Gets or sets the magnitude of acceleration of this particle along the <see cref="Heading"/>.
		/// </summary>
		/// <remarks>
		/// You need to specify <see cref="AirResistance"/> and <see cref="Wateresistance"/> when using this
		/// property otherwise the particle will gain infinite velocity.
		/// </remarks>
		public float Acceleration
		{
			get { return this.accThrust; }
			set { this.accThrust = value; }
		}
		/// <summary>
		/// Gets or sets the magnitude of acceleration of this particle along the - <see cref="Gravity"/>.
		/// </summary>
		/// <remarks>
		/// You need to specify <see cref="AirResistance"/> and <see cref="Wateresistance"/> when using this
		/// property otherwise the particle will gain infinite velocity.
		/// </remarks>
		public float LiftingAcceleration
		{
			get { return this.accLift; }
			set { this.accLift = value; }
		}
		/// <summary>
		/// Gets or sets the value that is a air resistance coefficient.
		/// </summary>
		/// <remarks>The formula for resistive force: <c>Force = airResistance * velocity</c></remarks>
		public float AirResistance
		{
			get { return this.kAirResistance; }
			set { this.kAirResistance = value; }
		}
		/// <summary>
		/// Gets or sets the value that is a water resistance coefficient.
		/// </summary>
		/// <remarks>The formula for resistive force: <c>Force = waterResistance * velocity</c></remarks>
		public float Wateresistance
		{
			get { return this.kWaterResistance; }
			set { this.kWaterResistance = value; }
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
		/// Gets or sets the vector that overrides the global gravity for this particle.
		/// </summary>
		public Vector3 Gravity
		{
			get { return this.gravity; }
			set { this.gravity = value; }
		}
		/// <summary>
		/// Gets or sets the vector that overrides the global gravity for this particle when it goes
		/// underwater.
		/// </summary>
		public Vector3 WaterGravity
		{
			get { return this.waterGravity; }
			set { this.waterGravity = value; }
		}
		/// <summary>
		/// Gets or sets the normalized vector that is aligned with surface normal this particle is sliding
		/// on.
		/// </summary>
		/// <remarks>
		/// Particle will only slide along the surface if <see cref="Flags"/> has flag
		/// <see cref="PhysicalEntityFlags.ParticleNoSpin"/> set.
		/// </remarks>
		/// <example>
		/// If the particle is a shard of glass, then this property would be a normal to its glass surface.
		/// </example>
		public Vector3 Normal
		{
			get { return this.normal; }
			set
			{
				this.normal = value;
				this.normal.Normalize();
			}
		}
		/// <summary>
		/// Gets or sets the normalized vector that is aligned with the axis of rolling, when this particle
		/// rolls along the surface.
		/// </summary>
		/// <remarks>
		/// Alignment can be disabled by passing <see cref="Vector3.Zero"/> (e.g. for spherical objects).
		/// </remarks>
		/// <example>
		/// If the particle is a rocket, then this property would point along the line between the center of
		/// its bottom and its tip.
		/// </example>
		public Vector3 RollingAxis
		{
			get { return this.rollAxis; }
			set
			{
				this.rollAxis = value;
				this.rollAxis.Normalize();
			}
		}
		/// <summary>
		/// Gets or sets the quaternion that represents initial orientation of this particle.
		/// </summary>
		/// <remarks>If not specified, it will be picked from <see cref="Heading"/>.</remarks>
		public Quaternion InitialOrientation
		{
			get { return this.q0; }
			set { this.q0 = value; }
		}
		/// <summary>
		/// Gets or sets the minimal speed of particle at which it will bounce of it rather then slide/roll
		/// along.
		/// </summary>
		public float MinimalBouncingSpeed
		{
			get { return this.minBounceVel; }
			set { this.minBounceVel = value; }
		}
		/// <summary>
		/// Gets or sets the minimal velocity this particle can move at without switching to 'sleep' state.
		/// </summary>
		public float MinimalVelocity
		{
			get { return this.minVel; }
			set { this.minVel = value; }
		}
		/// <summary>
		/// Gets or sets the physical entity that cannot collide with this one.
		/// </summary>
		public PhysicalEntity EntityToIgnore
		{
			get { return this.pColliderToIgnore; }
			set { this.pColliderToIgnore = value; }
		}
		/// <summary>
		/// Gets or sets the value that determines pierceability of this particle effect.
		/// </summary>
		/// <remarks>Pierceable hits slow the particle down rather then stop it.</remarks>
		public int Pierceability
		{
			get { return this.iPierceability; }
			set { this.iPierceability = value; }
		}
		/// <summary>
		/// Gets or sets a set of flags that specifies which entities this one can collide with.
		/// </summary>
		public ColliderTypes CollidesWith
		{
			get { return (ColliderTypes)this.collTypes; }
			set { this.collTypes = (int)value; }
		}
		/// <summary>
		/// Gets or sets the length of the period (in frames) once per which this particle checks for
		/// collisions.
		/// </summary>
		public int AreaCheckPeriod
		{
			get { return this.areaCheckPeriod; }
			set { this.areaCheckPeriod = value; }
		}
		/// <summary>
		/// Can be set to stop the surface of this particle from spawning material effects.
		/// </summary>
		public bool StopPlayingHitEffects
		{
			set { this.dontPlayHitEffect = value ? 1 : 0; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass anything.</param>
		public PhysicsParametersParticle([UsedImplicitly] int notUsed)
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.Particle);
			this.mass = UnusedValue.Single;
			this.flags = UnusedValue.UInt32;
			this.size = UnusedValue.Single;
			this.thickness = UnusedValue.Single;
			this.velocity = UnusedValue.Single;
			this.heading = UnusedValue.Vector;
			this.kAirResistance = UnusedValue.Single;
			this.kWaterResistance = UnusedValue.Single;
			this.accThrust = UnusedValue.Single;
			this.accLift = UnusedValue.Single;
			this.wspin = new EulerAngles(UnusedValue.Single, 0, 0);
			this.surface_idx = UnusedValue.Int32;
			this.gravity = UnusedValue.Vector;
			this.waterGravity = UnusedValue.Vector;
			this.normal = UnusedValue.Vector;
			this.rollAxis = UnusedValue.Vector;
			this.q0 = UnusedValue.Quaternion;
			this.minBounceVel = UnusedValue.Single;
			this.minVel = UnusedValue.Single;
			this.pColliderToIgnore = new PhysicalEntity(UnusedValue.Pointer);
			this.iPierceability = UnusedValue.Int32;
			this.collTypes = UnusedValue.Int32;
			this.areaCheckPeriod = UnusedValue.Int32;
			this.dontPlayHitEffect = UnusedValue.Int32;
		}
		#endregion
	}
}