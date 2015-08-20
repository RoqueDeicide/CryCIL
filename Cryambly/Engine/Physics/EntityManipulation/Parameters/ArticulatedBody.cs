using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to get or set the parameters of the physical entity
	/// that us an articulated body.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsParametersArticulatedBody
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to apply these
		/// onto physical entity or to <see cref="PhysicalEntity.GetParameters"/> to get the currently
		/// applied ones.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private int bGrounded;
		[UsedImplicitly] private int bCheckCollisions;
		[UsedImplicitly] private int bCollisionResp;
		[UsedImplicitly] private Vector3 pivot;
		[UsedImplicitly] private Vector3 a;
		[UsedImplicitly] private Vector3 wa;
		[UsedImplicitly] private Vector3 w;
		[UsedImplicitly] private Vector3 v;
		[UsedImplicitly] private float scaleBounceResponse;
		[UsedImplicitly] private int bApply_dqext;
		[UsedImplicitly] private int bAwake;
		[UsedImplicitly] private PhysicalEntity pHost;
		[UsedImplicitly] private Vector3 posHostPivot; // attachment position inside pHost
		[UsedImplicitly] private Quaternion qHostPivot;
		[UsedImplicitly] private int bInheritVel;
		[UsedImplicitly] private int nCollLyingMode;
		[UsedImplicitly] private Vector3 gravityLyingMode;
		[UsedImplicitly] private float dampingLyingMode;
		[UsedImplicitly] private float minEnergyLyingMode;
		[UsedImplicitly] private int iSimType;
		[UsedImplicitly] private int iSimTypeLyingMode;
		[UsedImplicitly] private int nRoots; // only used in GetParams
		[UsedImplicitly] private int nJointsAlloc;
		[UsedImplicitly] private int bRecalcJoints;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the value that indicates whether this articulated body is bound to the
		/// <see cref="Ground"/> entity.
		/// </summary>
		public bool Grounded
		{
			get { return this.bGrounded != 0; }
			set { this.bGrounded = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this body can collide with anything.
		/// </summary>
		/// <remarks>
		/// This value is ignored when <see cref="ConstraintMode"/> is equal to <c>true</c>.
		/// </remarks>
		public bool CheckCollisions
		{
			get { return this.bCheckCollisions != 0; }
			set { this.bCheckCollisions = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this articulated body is using 'constraint'
		/// simulation mode ( <c>true</c>) or 'Featherstone' simulation mode.
		/// </summary>
		/// <remarks>
		/// <para>Constraint mode is used by rag dolls.</para>
		/// <para>Featherstone mode is used by bodies that simulate live actors.</para>
		/// </remarks>
		public bool ConstraintMode
		{
			get { return this.bCollisionResp != 0; }
			set { this.bCollisionResp = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the entity that acts as a ground for this one.
		/// </summary>
		/// <remarks>
		/// This entity can be: terrain, big vehicle with walkable platforms (Scarab from Halo series) or a
		/// building.
		/// </remarks>
		public PhysicalEntity Ground
		{
			get { return this.pHost; }
			set { this.pHost = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this entity inherits the motion parameters from
		/// the <see cref="Ground"/> entity directly.
		/// </summary>
		public bool InheritGroundMotion
		{
			get { return this.bInheritVel != 0; }
			set { this.bInheritVel = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the position on the <see cref="Ground"/> this entity is attached to when it's
		/// grounded.
		/// </summary>
		public Vector3 Pivot
		{
			get { return this.pivot; }
			set { this.pivot = value; }
		}
		/// <summary>
		/// Gets or sets the acceleration of the <see cref="Ground"/> entity.
		/// </summary>
		/// <remarks>
		/// This property's value is ignored when <see cref="InheritGroundMotion"/> is set to <c>true</c>.
		/// </remarks>
		public Vector3 GroundAcceleration
		{
			get { return this.a; }
			set { this.a = value; }
		}
		/// <summary>
		/// Gets or sets the angular acceleration of the <see cref="Ground"/> entity.
		/// </summary>
		/// <remarks>
		/// This property's value is ignored when <see cref="InheritGroundMotion"/> is set to <c>true</c>.
		/// </remarks>
		public Vector3 GroundAngularAcceleration
		{
			get { return this.wa; }
			set { this.wa = value; }
		}
		/// <summary>
		/// Gets or sets the velocity of the <see cref="Ground"/> entity.
		/// </summary>
		/// <remarks>
		/// This property's value is ignored when <see cref="InheritGroundMotion"/> is set to <c>true</c>.
		/// </remarks>
		public Vector3 GroundVelocity
		{
			get { return this.v; }
			set { this.v = value; }
		}
		/// <summary>
		/// Gets or sets the angular velocity of the <see cref="Ground"/> entity.
		/// </summary>
		/// <remarks>
		/// This property's value is ignored when <see cref="InheritGroundMotion"/> is set to <c>true</c>.
		/// </remarks>
		public Vector3 GroundAngularVelocity
		{
			get { return this.w; }
			set { this.w = value; }
		}
		/// <summary>
		/// Gets or sets the scale of the bounce reaction that occurs when a joint reaches its limit.
		/// </summary>
		public float BounceResponseScale
		{
			get { return this.scaleBounceResponse; }
			set { this.scaleBounceResponse = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates a global animation speed must be applied to each joint.
		/// </summary>
		/// <remarks>
		/// Global animation speed is a value that is specific to each articulated body that is calculated
		/// every time <see cref="PhysicsParametersJoint.ExtraAngles"/> is set for any joint (the value is
		/// a difference between old value and a new one divided by
		/// <see cref="PhysicsParametersJoint.AnimationTimeStep"/>).
		/// </remarks>
		public bool ApplyDeltaExtraAngles
		{
			get { return this.bApply_dqext != 0; }
			set { this.bApply_dqext = value ? 1 : 0; }
		}
		/// <summary>
		/// Indicates whether this body is awake.
		/// </summary>
		public bool Awake
		{
			get { return this.bAwake != 0; }
		}
		/// <summary>
		/// Gets or sets number of contacts that must be registered across all parts of this articulated
		/// body that is needed to switch simulation mode into 'lying' mode that is used to rag dolls that
		/// are on the ground.
		/// </summary>
		public int LyingModeTrigger
		{
			get { return this.nCollLyingMode; }
			set { this.nCollLyingMode = value; }
		}
		/// <summary>
		/// Gets or sets the vector of gravity that is used when this articulated body is in 'lying' mode.
		/// </summary>
		public Vector3 LyingModeGravity
		{
			get { return this.gravityLyingMode; }
			set { this.gravityLyingMode = value; }
		}
		/// <summary>
		/// Gets or sets the velocity damping value that is used when this articulated body is in 'lying'
		/// mode.
		/// </summary>
		public float LyingModeDamping
		{
			get { return this.dampingLyingMode; }
			set { this.dampingLyingMode = value; }
		}
		/// <summary>
		/// Gets or sets the minimal energy amount for entering 'sleep' state that is used when this
		/// articulated body is in 'lying' mode.
		/// </summary>
		public float LyingModeMinimalEnergy
		{
			get { return this.minEnergyLyingMode; }
			set { this.minEnergyLyingMode = value; }
		}
		/// <summary>
		/// Gets or sets the value that is used when creating articulated bodies that specifies how many
		/// joints are expected to created for this body.
		/// </summary>
		public int PreallocatedJointCount
		{
			get { return this.nJointsAlloc; }
			set { this.nJointsAlloc = value; }
		}
		/// <summary>
		/// Indicates whether this body is using body-based simulation instead of joint-based one.
		/// </summary>
		/// <remarks>
		/// <para>
		/// In joint mode joint pivots are always enforced exactly by projecting the movement of child
		/// bodies to a set of constrained directions.
		/// </para>
		/// <para>
		/// In body mode bodies that comprise this one evolve independently and rely on the solver to
		/// enforce the joints.
		/// </para>
		/// <para>
		/// Joint mode is enforced when this entity moves quickly but body mode is more smooth, so it's
		/// recommended to pass <c>true</c> to this property when creating a body.
		/// </para>
		/// </remarks>
		public bool UseBodyBasedSimulation
		{
			get { return this.iSimType != 0; }
			set { this.iSimType = value ? 1 : 0; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass anything.</param>
		public PhysicsParametersArticulatedBody([UsedImplicitly] int notUsed)
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.ArticulatedBody);
			this.bGrounded = UnusedValue.Int32;
			this.bCheckCollisions = UnusedValue.Int32;
			this.bCollisionResp = UnusedValue.Int32;
			this.a = UnusedValue.Vector;
			this.pivot = UnusedValue.Vector;
			this.wa = UnusedValue.Vector;
			this.w = UnusedValue.Vector;
			this.v = UnusedValue.Vector;
			this.scaleBounceResponse = UnusedValue.Single;
			this.bApply_dqext = 0;
			this.bAwake = UnusedValue.Int32;
			this.posHostPivot = UnusedValue.Vector;
			this.pHost = new PhysicalEntity(UnusedValue.Pointer);
			this.qHostPivot = UnusedValue.Quaternion;
			this.bInheritVel = UnusedValue.Int32;
			this.nCollLyingMode = UnusedValue.Int32;
			this.gravityLyingMode = UnusedValue.Vector;
			this.dampingLyingMode = UnusedValue.Single;
			this.minEnergyLyingMode = UnusedValue.Single;
			this.iSimType = UnusedValue.Int32;
			this.iSimTypeLyingMode = 1;
			this.nRoots = UnusedValue.Int32;
			this.nJointsAlloc = UnusedValue.Int32;
			this.bRecalcJoints = 1;
		}
		#endregion
	}
}