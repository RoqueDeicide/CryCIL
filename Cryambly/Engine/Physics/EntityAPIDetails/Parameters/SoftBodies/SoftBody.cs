using System;
using System.Linq;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to specify the physical entity that is a soft body.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsParametersSoftBody
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to apply these
		/// onto physical entity or to <see cref="PhysicalEntity.GetParameters"/> to get the currently
		/// applied ones.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private float thickness;
		[UsedImplicitly] private float maxSafeStep;
		[UsedImplicitly] private float ks;
		[UsedImplicitly] private float kdRatio;
		[UsedImplicitly] private float friction;
		[UsedImplicitly] private float waterResistance;
		[UsedImplicitly] private float airResistance;
		[UsedImplicitly] private Vector3 wind;
		[UsedImplicitly] private float windVariance;
		[UsedImplicitly] private int nMaxIters;
		[UsedImplicitly] private float accuracy;
		[UsedImplicitly] private float impulseScale;
		[UsedImplicitly] private float explosionScale;
		[UsedImplicitly] private float collisionImpulseScale;
		[UsedImplicitly] private float maxCollisionImpulse;
		[UsedImplicitly] private int collTypes;
		[UsedImplicitly] private float massDecay;
		[UsedImplicitly] private float shapeStiffnessNorm;
		[UsedImplicitly] private float shapeStiffnessTang;
		[UsedImplicitly] private float stiffnessAnim;
		[UsedImplicitly] private float stiffnessDecayAnim;
		[UsedImplicitly] private float dampingAnim;
		[UsedImplicitly] private float maxDistAnim;
		[UsedImplicitly] private float hostSpaceSim;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the radius of spheres that are created for each vertex with latter in the center
		/// that are used for collisions.
		/// </summary>
		public float Thickness
		{
			get { return this.thickness; }
			set { this.thickness = value; }
		}
		/// <summary>
		/// Gets or sets the maximal effective length of the time step.
		/// </summary>
		public float MaximalTimeStep
		{
			get { return this.maxSafeStep; }
			set { this.maxSafeStep = value; }
		}
		/// <summary>
		/// Gets or sets the stiffness against stretching.
		/// </summary>
		public float Stiffness
		{
			get { return this.ks; }
			set { this.ks = value; }
		}
		/// <summary>
		/// Gets or sets the ratio of damping in stretch direction to some unknown value.
		/// </summary>
		public float DampingRatio
		{
			get { return this.kdRatio; }
			set { this.kdRatio = value; }
		}
		/// <summary>
		/// Gets or sets the value that can be used to override the material friction.
		/// </summary>
		public float Friction
		{
			get { return this.friction; }
			set { this.friction = value; }
		}
		/// <summary>
		/// Gets or sets the value that specifies the medium resistance when this entity is above water.
		/// </summary>
		/// <remarks>Has to be greater then zero in order to be affected by the wind.</remarks>
		public float AirReistance
		{
			get { return this.airResistance; }
			set { this.airResistance = value; }
		}
		/// <summary>
		/// Gets or sets the value that specifies the medium resistance when this entity is underwater.
		/// </summary>
		public float WaterResistance
		{
			get { return this.waterResistance; }
			set { this.waterResistance = value; }
		}
		/// <summary>
		/// Gets or sets the local wind direction that is added to any other wind that can be applied to
		/// this entity.
		/// </summary>
		public Vector3 Wind
		{
			get { return this.wind; }
			set { this.wind = value; }
		}
		/// <summary>
		/// Gets or sets the value that defines the variance range that is used with <see cref="Wind"/>.
		/// </summary>
		public float WindVariance
		{
			get { return this.windVariance; }
			set { this.windVariance = value; }
		}
		/// <summary>
		/// Gets or sets the multiplier that scales all incoming general impulses.
		/// </summary>
		public float GeneralImpulseScale
		{
			get { return this.impulseScale; }
			set { this.impulseScale = value; }
		}
		/// <summary>
		/// Gets or sets the multiplier that scales all incoming explosion impulses.
		/// </summary>
		public float ExplosionImpulseScale
		{
			get { return this.explosionScale; }
			set { this.explosionScale = value; }
		}
		/// <summary>
		/// Gets or sets the types of entities this one can collide with.
		/// </summary>
		public PhysicalEntityTypes CollisionTypes
		{
			get { return (PhysicalEntityTypes)this.collTypes; }
			set { this.collTypes = (int)value; }
		}
		// decreases mass from attached points to free ends; mass_free = mass_attached/(1+decay) (can impove
		// stability)
		/// <summary>
		/// Gets or sets the value that indicates how mass decreases from points that are attached to other
		/// entities to points that are free. Can improve stability.
		/// </summary>
		/// <remarks>
		/// The formula for the mass of points that are free:
		/// <code>
		/// float massFreePoint = massAttachedPoint / (1 + massDecay);
		/// </code>
		/// </remarks>
		public float MassDecay
		{
			get { return this.massDecay; }
			set { this.massDecay = value; }
		}
		/// <summary>
		/// Gets or sets the value that defines resistance against bending.
		/// </summary>
		public float ShapeStiffnessNormal
		{
			get { return this.shapeStiffnessNorm; }
			set { this.shapeStiffnessNorm = value; }
		}
		/// <summary>
		/// Gets or sets the value that defines resistance against shearing.
		/// </summary>
		public float ShapeStiffnessTangent
		{
			get { return this.shapeStiffnessTang; }
			set { this.shapeStiffnessTang = value; }
		}
		/// <summary>
		/// Gets or sets the value that defines shape preserving stiffness.
		/// </summary>
		public float StiffnessAnimation
		{
			get { return this.stiffnessAnim; }
			set { this.stiffnessAnim = value; }
		}
		/// <summary>
		/// Gets or sets the value between 0 and 1 that defines how the shape preserving stiffness decreases
		/// as the soft body approaches its normal shape.
		/// </summary>
		/// <remarks>
		/// The formula for the shape-preserving stiffness:
		/// <code>
		/// float full = params.StiffnessAnimation;
		/// float decay = params.StiffnessAnimationDecay;
		/// float stiffness = full * (1 - decay);
		/// </code>
		/// </remarks>
		public float StiffnessAnimationDecay
		{
			get { return this.stiffnessDecayAnim; }
			set { this.stiffnessDecayAnim = MathHelpers.Clamp(value, 0, 1); }
		}
		/// <summary>
		/// Gets or sets the value that specifies that damping for shape-preserving forces.
		/// </summary>
		public float DampingAnimation
		{
			get { return this.dampingAnim; }
			set { this.dampingAnim = value; }
		}
		/// <summary>
		/// Gets or sets maximal distance any point can reach away from its target position.
		/// </summary>
		public float MaximalDistanceAnimation
		{
			get { return this.maxDistAnim; }
			set { this.maxDistAnim = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether simulation is done used host-space coordinates
		/// instead of world space.
		/// </summary>
		public bool HostSpaceSimulation
		{
			get { return this.hostSpaceSim < MathHelpers.ZeroTolerance; }
			set { this.hostSpaceSim = value ? 1 : 0; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid instance of this type.
		/// </summary>
		/// <param name="notUsed">Pass anything.</param>
		public PhysicsParametersSoftBody([UsedImplicitly] int notUsed)
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.SoftBody);
			this.thickness = UnusedValue.Single;
			this.maxSafeStep = UnusedValue.Single;
			this.ks = UnusedValue.Single;
			this.kdRatio = UnusedValue.Single;
			this.friction = UnusedValue.Single;
			this.waterResistance = UnusedValue.Single;
			this.wind = UnusedValue.Vector;
			this.airResistance = UnusedValue.Single;
			this.windVariance = UnusedValue.Single;
			this.nMaxIters = UnusedValue.Int32;
			this.accuracy = UnusedValue.Single;
			this.impulseScale = UnusedValue.Single;
			this.explosionScale = UnusedValue.Single;
			this.collisionImpulseScale = UnusedValue.Single;
			this.maxCollisionImpulse = UnusedValue.Single;
			this.collTypes = UnusedValue.Int32;
			this.massDecay = UnusedValue.Single;
			this.shapeStiffnessTang = UnusedValue.Single;
			this.shapeStiffnessNorm = UnusedValue.Single;
			this.stiffnessAnim = UnusedValue.Single;
			this.stiffnessDecayAnim = UnusedValue.Single;
			this.dampingAnim = UnusedValue.Single;
			this.maxDistAnim = UnusedValue.Single;
			this.hostSpaceSim = UnusedValue.Single;
		}
		#endregion
	}
}