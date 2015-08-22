using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to get and simulation parameters of the physical
	/// entity.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsParametersSimulation
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to apply these
		/// onto physical entity or to <see cref="PhysicalEntity.GetParameters"/> to get the currently
		/// applied ones.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private int iSimClass;
		[UsedImplicitly] private float maxTimeStep;
		[UsedImplicitly] private float minEnergy;
		[UsedImplicitly] private float damping;
		[UsedImplicitly] private Vector3 gravity;
		[UsedImplicitly] private float dampingFreefall;
		[UsedImplicitly] private Vector3 gravityFreefall;
		[UsedImplicitly] private float maxRotVel;
		[UsedImplicitly] private float mass;
		[UsedImplicitly] private float density;
		[UsedImplicitly] private int maxLoggedCollisions;
		[UsedImplicitly] private int disablePreCG;
		[UsedImplicitly] private float maxFriction;
		[UsedImplicitly] private int collTypes;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the simulation class that this entity is using.
		/// </summary>
		public PhysicsSimulationClass SimulationClass
		{
			get { return (PhysicsSimulationClass)this.iSimClass; }
			set { this.iSimClass = (int)value; }
		}
		/// <summary>
		/// Gets maximal length of the time step in seconds(?) the entity can receive (larger time-steps
		/// will be split).
		/// </summary>
		public float MaxTimeStep
		{
			get { return this.maxTimeStep; }
			set { this.maxTimeStep = value; }
		}
		/// <summary>
		/// Gets the velocity damping value.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The formula that is used for damping:
		/// <code>
		/// Vector3 velocity = originalVelocity * (1 - damping * timeInterval);
		/// </code>.
		/// </para>
		/// <para>Damping is used to simulate environmental resistance (e.g. air friction).</para>
		/// </remarks>
		public float Damping
		{
			get { return this.damping; }
			set { this.damping = value; }
		}
		/// <summary>
		/// Gets minimal amount of kinetic energy the entity must get down to for it to fall asleep.
		/// </summary>
		/// <remarks>Original documentation mentions that this value is divided by mass.</remarks>
		public float MinEnergy
		{
			get { return this.minEnergy; }
			set { this.minEnergy = value; }
		}
		/// <summary>
		/// Gets the vector of gravity that is used by this entity.
		/// </summary>
		/// <remarks>
		/// This vector gets overridden when the entity enters a gravity volume unless the entity has the
		/// flag <see cref="PhysicalEntityFlags.IgnoreAreas"/> set.
		/// </remarks>
		public Vector3 Gravity
		{
			get { return this.gravity; }
			set { this.gravity = value; }
		}
		/// <summary>
		/// Gets the vector of gravity that is used by this entity when it is not in contact with anything
		/// (In free-fall).
		/// </summary>
		public Vector3 GravityFreefall
		{
			get { return this.gravityFreefall.IsUsed() ? this.gravityFreefall : this.gravity; }
			set { this.gravityFreefall = value; }
		}
		/// <summary>
		/// Gets the damping value that is used when this entity is not in contact with anything (In
		/// free-fall).
		/// </summary>
		public float DampingFreefall
		{
			get { return this.dampingFreefall.IsUsed() ? this.dampingFreefall : this.damping; }
			set { this.dampingFreefall = value; }
		}
		/// <summary>
		/// Gets the value the angular velocity of the entity is clamped to.
		/// </summary>
		public float MaxAngularVelocity
		{
			get { return this.maxRotVel; }
			set { this.maxRotVel = value; }
		}
		/// <summary>
		/// Gets the mass of this entity.
		/// </summary>
		public float Mass
		{
			get { return this.mass; }
			set { this.mass = value; }
		}
		/// <summary>
		/// Gets the density of this entity.
		/// </summary>
		public float Density
		{
			get { return this.density; }
			set { this.density = value; }
		}
		/// <summary>
		/// Gets maximal number of collisions that can be reported by the entity.
		/// </summary>
		public int MaxLoggedCollisions
		{
			get { return this.maxLoggedCollisions; }
			set { this.maxLoggedCollisions = value; }
		}
		/// <summary>
		/// Indicates whether Pre-CG solver is disabled for the entity.
		/// </summary>
		/// <remarks>Disabling this solver is recommended for spherical objects.</remarks>
		public int DisablePreCg
		{
			get { return this.disablePreCG; }
			set { this.disablePreCG = value; }
		}
		/// <summary>
		/// Gets the value that indicates how big the friction value can be when this entity is in contact
		/// with others.
		/// </summary>
		public float MaxFriction
		{
			get { return this.maxFriction; }
			set { this.maxFriction = value; }
		}
		/// <summary>
		/// A set of flags that indicates which types of entities this one can collide with.
		/// </summary>
		public EntityQueryFlags CollidesWith
		{
			get { return (EntityQueryFlags)this.collTypes; }
			set { this.collTypes = (int)value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid default object of this type that can be initialized via object initializer.
		/// </summary>
		/// <param name="s">
		/// Must be passed to ensure this constructor is invoked, other then that this parameter is not
		/// used.
		/// </param>
		public PhysicsParametersSimulation([UsedImplicitly] bool s)
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.Simulation);
			this.iSimClass = UnusedValue.Int32;
			this.maxTimeStep = UnusedValue.Single;
			this.minEnergy = UnusedValue.Single;
			this.damping = UnusedValue.Single;
			this.gravity = UnusedValue.Vector;
			this.dampingFreefall = UnusedValue.Single;
			this.gravityFreefall = UnusedValue.Vector;
			this.maxRotVel = UnusedValue.Single;
			this.mass = UnusedValue.Single;
			this.density = UnusedValue.Single;
			this.maxLoggedCollisions = UnusedValue.Int32;
			this.disablePreCG = UnusedValue.Int32;
			this.maxFriction = UnusedValue.Single;
			this.collTypes = UnusedValue.Int32;
		}
		#endregion
	}
}