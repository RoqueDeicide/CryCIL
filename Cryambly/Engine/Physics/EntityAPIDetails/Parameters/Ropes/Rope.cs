using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Geometry;
using CryCil.Utilities;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of ways the rope can try preserving its shape.
	/// </summary>
	public enum RopeTargetPoseMode
	{
		/// <summary>
		/// Specifies that the rope should try preserving its shape.
		/// </summary>
		NoTargetPose,
		/// <summary>
		/// Specifies that the rope will pull the vertices towards the target shape positions directly.
		/// </summary>
		Simplified,
		/// <summary>
		/// Specifies that the rope will pull the vertices towards the target shape positions with penalty
		/// torque applied at the joints.
		/// </summary>
		PhysicallyCorrect
	}
	/// <summary>
	/// Encapsulates a set of parameters that allows to specify the physical entity that is a rope.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsParametersRope
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to apply these
		/// onto physical entity or to <see cref="PhysicalEntity.GetParameters"/> to get the currently
		/// applied ones.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private float length;
		[UsedImplicitly] private float mass;
		[UsedImplicitly] private float collDist;
		[UsedImplicitly] private int surface_idx;
		[UsedImplicitly] private float friction;
		[UsedImplicitly] private float frictionPull;
		[UsedImplicitly] private float stiffness;
		[UsedImplicitly] private float stiffnessAnim;
		[UsedImplicitly] private float stiffnessDecayAnim;
		[UsedImplicitly] private float dampingAnim;
		[UsedImplicitly] private int bTargetPoseActive;
		[UsedImplicitly] private Vector3 wind;
		[UsedImplicitly] private float windVariance;
		[UsedImplicitly] private float airResistance;
		[UsedImplicitly] private float waterResistance;
		[UsedImplicitly] private float density;
		[UsedImplicitly] private float jointLimit;
		[UsedImplicitly] private float jointLimitDecay;
		[UsedImplicitly] private float sensorRadius;
		[UsedImplicitly] private float maxForce;
		[UsedImplicitly] private float penaltyScale; // for the solver in strained state with subdivision on
		[UsedImplicitly] private float attachmentZone;
		[UsedImplicitly] private float minSegLen;
		[UsedImplicitly] private float unprojLimit; // rotational unprojection limit per frame (no-subdivision mode)
		[UsedImplicitly] private float noCollDist;
		[UsedImplicitly] private int maxIters; // tweak for the internal vertex solver
		[UsedImplicitly] private int nSegments;
		[UsedImplicitly] private int flagsCollider;
		[UsedImplicitly] private int collTypes;
		[UsedImplicitly] private int nMaxSubVtx;
		[UsedImplicitly] private Vector3 collisionBBox0;
		[UsedImplicitly] private Vector3 collisionBBox1;
		[UsedImplicitly] private Vector3 hingeAxis;
										 // only allow rotation around this axis (in parent's frame if
										 // rope_target_vtx_rel is set)
		[UsedImplicitly] private StridedPointer pPoints;
		[UsedImplicitly] private StridedPointer pVelocities;
		[UsedImplicitly] private PhysicalEntity pEntTiedTo0;
		[UsedImplicitly] private PhysicalEntity pEntTiedTo1;
		[UsedImplicitly] private int bLocalPtTied;
		[UsedImplicitly] private Vector3 ptTiedTo0;
		[UsedImplicitly] private Vector3 ptTiedTo1;
		[UsedImplicitly] private int idPartTiedTo0;
		[UsedImplicitly] private int idPartTiedTo1;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the length this rope will try to keep. Can be 0 if the rope has
		/// <see cref="PhysicalEntityFlags.RopeSubdivideSegments"/> flag set.
		/// </summary>
		public float Length
		{
			get { return this.length; }
			set { this.length = value; }
		}
		/// <summary>
		/// Gets or sets the mass of the rope on kilograms.
		/// </summary>
		public float Mass
		{
			get { return this.mass; }
			set { this.mass = value; }
		}
		/// <summary>
		/// Gets or sets the thickness of this rope. Used for collision detection.
		/// </summary>
		public float Thickness
		{
			get { return this.collDist; }
			set { this.collDist = value; }
		}
		/// <summary>
		/// Gets or sets the object that represents the surface type that is used by this entity for
		/// collisions.
		/// </summary>
		public SurfaceType SurfaceType
		{
			get { return SurfaceType.Get(this.surface_idx); }
			set { this.surface_idx = value.Identifier; }
		}
		/// <summary>
		/// Gets or sets the value that specifies the friction in free state and lateral friction in
		/// strained state.
		/// </summary>
		public float Friction
		{
			get { return this.friction; }
			set { this.friction = value; }
		}
		/// <summary>
		/// Gets or sets the value that specifies the friction force in pulling direction when in the
		/// strained state.
		/// </summary>
		public float FrictionPull
		{
			get { return this.frictionPull; }
			set { this.frictionPull = value; }
		}
		/// <summary>
		/// Gets or sets the value that specifies this rope's stiffness against stretching.
		/// </summary>
		public float Stiffness
		{
			get { return this.stiffness; }
			set { this.stiffness = value; }
		}
		/// <summary>
		/// Gets or sets the value that defines shape preserving stiffness.
		/// </summary>
		/// <remarks>
		/// This value is ignored if <see cref="TargetPoseMode"/> is set to
		/// <see cref="RopeTargetPoseMode.NoTargetPose"/>.
		/// </remarks>
		public float StiffnessAnimation
		{
			get { return this.stiffnessAnim; }
			set { this.stiffnessAnim = value; }
		}
		/// <summary>
		/// Gets or sets the value between 0 and 1 that defines how the shape preserving stiffness
		/// decreases as the rope approaches its normal shape.
		/// </summary>
		/// <remarks>
		/// The formula for the shape-preserving stiffness:
		/// <code>
		/// float full = params.StiffnessAnimation;
		/// float decay = params.StiffnessAnimationDecay;
		/// float stiffness = full * (1 - decay);
		/// </code>
		/// <para>
		/// This value is ignored if <see cref="TargetPoseMode"/> is set to
		/// <see cref="RopeTargetPoseMode.NoTargetPose"/>.
		/// </para>
		/// </remarks>
		public float StiffnessAnimationDecay
		{
			get { return this.stiffnessDecayAnim; }
			set { this.stiffnessDecayAnim = MathHelpers.Clamp(value, 0, 1); }
		}
		/// <summary>
		/// Gets or sets the value that specifies that damping for shape-preserving forces.
		/// </summary>
		/// <remarks>
		/// This value is ignored if <see cref="TargetPoseMode"/> is set to
		/// <see cref="RopeTargetPoseMode.NoTargetPose"/>.
		/// </remarks>
		public float DampingAnimation
		{
			get { return this.dampingAnim; }
			set { this.dampingAnim = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this rope will trying preservation of its shape
		/// and, if it will, then how.
		/// </summary>
		public RopeTargetPoseMode TargetPoseMode
		{
			get { return (RopeTargetPoseMode)this.bTargetPoseActive; }
			set { this.bTargetPoseActive = (int)value; }
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
		/// Gets or sets the value that specifies how buoyant this entity is.
		/// </summary>
		public float Density
		{
			get { return this.density; }
			set { this.density = value; }
		}
		/// <summary>
		/// Gets or sets the maximal angle of deviation of joints when one of the ends is not tied.
		/// </summary>
		public float JointLimit
		{
			get { return this.jointLimit; }
			set { this.jointLimit = value; }
		}
		/// <summary>
		/// Gets or sets the value between -1 and 1 that defines how the angle of deviation limit changes
		/// from tied end of the rope to the free one.
		/// </summary>
		public float JointLimitDecay
		{
			get { return this.jointLimitDecay; }
			set { this.jointLimitDecay = MathHelpers.Clamp(value, -1, 1); }
		}
		/// <summary>
		/// Gets or sets the radius of the sphere that is used to reattach the rope when the host entity
		/// breaks.
		/// </summary>
		public float SensorRadius
		{
			get { return this.sensorRadius; }
			set { this.sensorRadius = value; }
		}
		/// <summary>
		/// Gets or sets the biggest force that can be sustained by the rope before tearing apart.
		/// </summary>
		/// <remarks>
		/// This value is ignored when <see cref="PhysicalEntityFlags.RopeNoTears"/> is set.
		/// </remarks>
		public float MaxForce
		{
			get { return this.maxForce; }
			set { this.maxForce = value; }
		}
		/// <summary>
		/// Gets or sets the value that defines the size of spheres around attachment zones where contacts
		/// are not registered when in subdivision mode.
		/// </summary>
		public float AttachementZoneSize
		{
			get { return this.attachmentZone; }
			set { this.attachmentZone = value; }
		}
		/// <summary>
		/// Gets or sets the length of the shortest segment possible in subdivision mode.
		/// </summary>
		public float MinimalSegmentLength
		{
			get { return this.minSegLen; }
			set { this.minSegLen = value; }
		}
		/// <summary>
		/// Gets or sets the distance from the attachment point to the other end of the segment where
		/// collisions are not registered when not in subdivision mode.
		/// </summary>
		public float NoCollideDistance
		{
			get { return this.noCollDist; }
			set { this.noCollDist = value; }
		}
		/// <summary>
		/// Gets or sets the number of segments this rope consists of.
		/// </summary>
		/// <remarks>
		/// Forcing the count to change on a functioning rope causes vertex positions to be reset.
		/// </remarks>
		public int SegmentCount
		{
			get { return this.nSegments; }
			set { this.nSegments = value; }
		}
		/// <summary>
		/// Gets or sets a set of flags that specifies which kinds of parts of entities this rope can
		/// collide with.
		/// </summary>
		public ColliderTypes ColledeWithParts
		{
			get { return (ColliderTypes)this.flagsCollider; }
			set { this.flagsCollider = (int)value; }
		}
		/// <summary>
		/// Gets or sets
		/// </summary>
		public PhysicalEntityTypes CollidesWith
		{
			get { return (PhysicalEntityTypes)this.collTypes; }
			set { this.collTypes = (int)value; }
		}
		/// <summary>
		/// Gets or sets the maximal number of vertices any segment of rope in subdivision mode can consist
		/// of.
		/// </summary>
		public int MaximalVertexCount
		{
			get { return this.nMaxSubVtx; }
			set { this.nMaxSubVtx = value; }
		}
		/// <summary>
		/// Gets the array of vectors that represent positions of points that connect the segments.
		/// </summary>
		public Vector3[] Points
		{
			get
			{
				Vector3[] points = new Vector3[this.nSegments + 1];

				for (int i = 0; i < points.Length; i++)
				{
					points[i] = this.pPoints.GetVector3(i);
				}

				return points;
			}
		}
		/// <summary>
		/// Gets the array of vectors that represent velocities of points that connect the segments.
		/// </summary>
		public Vector3[] Velocities
		{
			get
			{
				Vector3[] velocities = new Vector3[this.nSegments + 1];

				for (int i = 0; i < velocities.Length; i++)
				{
					velocities[i] = this.pVelocities.GetVector3(i);
				}

				return velocities;
			}
		}
		/// <summary>
		/// Gets or sets the first entity this rope is tied to.
		/// </summary>
		public PhysicalEntity FirstEntity
		{
			get { return this.pEntTiedTo0; }
			set { this.pEntTiedTo0 = value; }
		}
		/// <summary>
		/// Gets or sets the second entity this rope is tied to.
		/// </summary>
		public PhysicalEntity SecondEntity
		{
			get { return this.pEntTiedTo1; }
			set { this.pEntTiedTo1 = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether coordinates of points at which the rope is tied
		/// to entities are in entity's or part's local space.
		/// </summary>
		public bool TiePointsInLocalSpace
		{
			get { return this.bLocalPtTied != 0; }
			set { this.bLocalPtTied = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the point at which this rope is attached to <see cref="FirstEntity"/>.
		/// </summary>
		public Vector3 FirstEntityTiePoint
		{
			get { return this.ptTiedTo0; }
			set { this.ptTiedTo0 = value; }
		}
		/// <summary>
		/// Gets or sets the point at which this rope is attached to <see cref="SecondEntity"/>.
		/// </summary>
		public Vector3 SecondEntityTiePoint
		{
			get { return this.ptTiedTo1; }
			set { this.ptTiedTo1 = value; }
		}
		/// <summary>
		/// Gets or sets the identifier of the part of <see cref="FirstEntity"/> this rope is attached to.
		/// </summary>
		public int FirstEntityPart
		{
			get { return this.idPartTiedTo0; }
			set { this.idPartTiedTo0 = value; }
		}
		/// <summary>
		/// Gets or sets the identifier of the part of <see cref="FirstEntity"/> this rope is attached to.
		/// </summary>
		public int SecondEntityPart
		{
			get { return this.idPartTiedTo1; }
			set { this.idPartTiedTo1 = value; }
		}
		/// <summary>
		/// Gets or sets the bounding box in host's local space that is used for collision queries.
		/// </summary>
		/// <remarks>
		/// An entity with multiple ropes attached to it can set this property to the same value for all
		/// ropes to optimize collision detection.
		/// </remarks>
		public BoundingBox CollisionBoundingBox
		{
			get { return new BoundingBox(this.collisionBBox0, this.collisionBBox1); }
			set
			{
				this.collisionBBox0 = value.Minimum;
				this.collisionBBox1 = value.Maximum;
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass anything.</param>
		public PhysicsParametersRope([UsedImplicitly] int notUsed)
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.Rope);
			this.length = UnusedValue.Single;
			this.mass = UnusedValue.Single;
			this.collDist = UnusedValue.Single;
			this.surface_idx = UnusedValue.Int32;
			this.friction = UnusedValue.Single;
			this.frictionPull = UnusedValue.Single;
			this.stiffness = UnusedValue.Single;
			this.stiffnessAnim = UnusedValue.Single;
			this.stiffnessDecayAnim = UnusedValue.Single;
			this.dampingAnim = UnusedValue.Single;
			this.bTargetPoseActive = UnusedValue.Int32;
			this.wind = UnusedValue.Vector;
			this.windVariance = UnusedValue.Single;
			this.airResistance = UnusedValue.Single;
			this.waterResistance = UnusedValue.Single;
			this.density = UnusedValue.Single;
			this.jointLimit = UnusedValue.Single;
			this.sensorRadius = UnusedValue.Single;
			this.maxForce = UnusedValue.Single;
			this.jointLimitDecay = UnusedValue.Single;
			this.penaltyScale = UnusedValue.Single;
			this.attachmentZone = UnusedValue.Single;
			this.minSegLen = UnusedValue.Single;
			this.unprojLimit = UnusedValue.Single;
			this.nSegments = UnusedValue.Int32;
			this.noCollDist = UnusedValue.Single;
			this.maxIters = UnusedValue.Int32;
			this.flagsCollider = UnusedValue.Int32;
			this.collTypes = UnusedValue.Int32;
			this.nMaxSubVtx = UnusedValue.Int32;
			this.collisionBBox0 = UnusedValue.Vector;
			this.collisionBBox1 = Vector3.Zero;
			this.hingeAxis = UnusedValue.Vector;
			this.pVelocities = StridedPointer.Unused;
			this.pPoints = StridedPointer.Unused;
			this.pEntTiedTo0 = new PhysicalEntity(UnusedValue.Pointer);
			this.pEntTiedTo1 = new PhysicalEntity(UnusedValue.Pointer);
			this.bLocalPtTied = UnusedValue.Int32;
			this.ptTiedTo0 = UnusedValue.Vector;
			this.ptTiedTo1 = UnusedValue.Vector;
			this.idPartTiedTo0 = UnusedValue.Int32;
			this.idPartTiedTo1 = UnusedValue.Int32;
		}
		#endregion
	}
}