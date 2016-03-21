using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to specify the structural joint in the physical entity.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsParametersStructuralJoint
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to apply these
		/// onto physical entity or to <see cref="PhysicalEntity.GetParameters"/> to get the currently
		/// applied ones.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private int id;
		[UsedImplicitly] private int idx;
		[UsedImplicitly] private int bReplaceExisting;
		[UsedImplicitly] private int partid1;
		[UsedImplicitly] private int partid2;
		[UsedImplicitly] private Vector3 pt;
		[UsedImplicitly] private Vector3 n;
		[UsedImplicitly] private Vector3 axisx;
		[UsedImplicitly] private float maxForcePush, maxForcePull, maxForceShift;
		[UsedImplicitly] private float maxTorqueBend, maxTorqueTwist;
		[UsedImplicitly] private float damageAccum, damageAccumThresh;
		[UsedImplicitly] private Vector3 limitConstraint;
		[UsedImplicitly] private int bBreakable;
		[UsedImplicitly] private int bConstraintWillIgnoreCollisions;
		[UsedImplicitly] private int bDirectBreaksOnly;
		[UsedImplicitly] private float dampingConstraint;
		[UsedImplicitly] private float szSensor;
		[UsedImplicitly] private int bBroken;
		[UsedImplicitly] private int partidEpicenter;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the identifier of the joint.
		/// </summary>
		public int Id => this.id;
		/// <summary>
		/// Gets zero-based index of the joint.
		/// </summary>
		public int Index => this.idx;
		/// <summary>
		/// Sets the value that indicates if when this object is passed to
		/// <see cref="PhysicalEntity.SetParameters"/> a brand new joint will be created if this object was
		/// created with identifier and the latter already exists.
		/// </summary>
		public bool ReplaceExisting
		{
			set { this.bReplaceExisting = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets identifier of the first part this joint connects.
		/// </summary>
		/// <returns>- 1, if this part is ground.</returns>
		public int FirstPart
		{
			get { return this.partid1; }
			set { this.partid1 = value; }
		}
		/// <summary>
		/// Gets or sets identifier of the second part this joint connects.
		/// </summary>
		/// <returns>- 1, if this part is ground.</returns>
		public int SecondPart
		{
			get { return this.partid2; }
			set { this.partid2 = value; }
		}
		/// <summary>
		/// Gets or sets the point of connection in entity space.
		/// </summary>
		public Vector3 Point
		{
			get { return this.pt; }
			set { this.pt = value; }
		}
		/// <summary>
		/// Gets of sets a vector that defines a line parallel to which push/pull is allowed.
		/// </summary>
		public Vector3 PushPullDirection
		{
			get { return this.n; }
			set
			{
				this.n = value;
				this.n.Normalize();
			}
		}
		/// <summary>
		/// Gets or sets the direction of X-axis in entity space that is used by joints that can become
		/// dynamic constraint.
		/// </summary>
		public Vector3 XAxis
		{
			get { return this.axisx; }
			set
			{
				this.axisx = value;
				this.axisx.Normalize();
			}
		}
		/// <summary>
		/// Gets or sets maximal pushing force that can be handled by the joint before it breaks.
		/// </summary>
		public float MaxForcePush
		{
			get { return this.maxForcePush; }
			set { this.maxForcePush = value; }
		}
		/// <summary>
		/// Gets or sets maximal pulling force that can be handled by the joint before it breaks.
		/// </summary>
		public float MaxForcePull
		{
			get { return this.maxForcePull; }
			set { this.maxForcePull = value; }
		}
		/// <summary>
		/// Gets or sets maximal shifting force that can be handled by the joint before it breaks.
		/// </summary>
		public float MaxForceShift
		{
			get { return this.maxForceShift; }
			set { this.maxForceShift = value; }
		}
		/// <summary>
		/// Gets or sets maximal twisting torque that can be handled by the joint before it breaks.
		/// </summary>
		public float MaxTorqueTwist
		{
			get { return this.maxTorqueTwist; }
			set { this.maxTorqueTwist = value; }
		}
		/// <summary>
		/// Gets or sets maximal bending torque that can be handled by the joint before it breaks.
		/// </summary>
		public float MaxTorqueBend
		{
			get { return this.maxTorqueBend; }
			set { this.maxTorqueBend = value; }
		}
		/// <summary>
		/// Gets or sets the value that specifies how much tension can be accumulated by the joint before
		/// breakage.
		/// </summary>
		/// <remarks>This property represents the 'health' of the joint.</remarks>
		public float DamageAccumulation
		{
			get { return this.damageAccum; }
			set { this.damageAccum = value; }
		}
		/// <summary>
		/// Gets or sets the strength of tension the joint must feel to take damage.
		/// </summary>
		public float DamageAccumulationThreshold
		{
			get { return this.damageAccumThresh; }
			set { this.damageAccumThresh = value; }
		}
		/// <summary>
		/// Gets or sets the minimal angle to constraint the motion of connected part to when this joint
		/// becomes a dynamic constraint.
		/// </summary>
		public float MinimalAngleConstraint
		{
			get { return this.limitConstraint.X; }
			set { this.limitConstraint.X = value; }
		}
		/// <summary>
		/// Gets or sets the maximal angle to constraint the motion of connected part to when this joint
		/// becomes a dynamic constraint.
		/// </summary>
		public float MaximalAngleConstraint
		{
			get { return this.limitConstraint.Y; }
			set { this.limitConstraint.Y = value; }
		}
		/// <summary>
		/// Gets or sets the maximal force the constraint can exercise to bring connected parts together.
		/// </summary>
		public float ForceLimitConstraint
		{
			get { return this.limitConstraint.Z; }
			set { this.limitConstraint.Z = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this joint can be broken.
		/// </summary>
		public bool Breakable
		{
			get { return this.bBreakable != 0; }
			set { this.bBreakable = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this joint is broken.
		/// </summary>
		public bool Broken
		{
			get { return this.bBroken != 0; }
			set { this.bBroken = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether parts that are connected by the joint can collide
		/// with each other.
		/// </summary>
		public bool PartsDontCollide
		{
			get { return this.bConstraintWillIgnoreCollisions != 0; }
			set { this.bConstraintWillIgnoreCollisions = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether the joint can only be broken by applying impulses
		/// to parts it connects.
		/// </summary>
		public bool CanOnlyBeBrokenByImpulses
		{
			get { return this.bDirectBreaksOnly != 0; }
			set { this.bDirectBreaksOnly = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the size of geometry that is used to detect parts coming into range where the joint
		/// can be restored if it was broken.
		/// </summary>
		public float ReattachmentRange
		{
			get { return this.szSensor; }
			set { this.szSensor = value; }
		}
		/// <summary>
		/// Gets or sets velocity damping value that is applied to connected parts.
		/// </summary>
		public float DampingContraint
		{
			get { return this.dampingConstraint; }
			set { this.dampingConstraint = value; }
		}
		/// <summary>
		/// Gets or sets identifier of the part that is used as a starting point to recalculate the tension
		/// in the structure.
		/// </summary>
		/// <remarks>Used in network playback when synchronizing physics.</remarks>
		public int PartEpicenter
		{
			get { return this.partidEpicenter; }
			set { this.partidEpicenter = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates an object of this type.
		/// </summary>
		/// <param name="joint">
		/// An object that specifies for which joint these parameters will be get/set.
		/// </param>
		public PhysicsParametersStructuralJoint(EntityPartSpec joint)
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.StructuralJoint);

			this.id = 0;
			this.bReplaceExisting = 0;

			this.idx = UnusedValue.Int32;
			this.partid1 = UnusedValue.Int32;
			this.partid2 = UnusedValue.Int32;
			this.pt = UnusedValue.Vector;
			this.n = UnusedValue.Vector;
			this.axisx = UnusedValue.Vector;
			this.maxForcePush = UnusedValue.Single;
			this.maxForcePull = UnusedValue.Single;
			this.maxForceShift = UnusedValue.Single;
			this.maxTorqueBend = UnusedValue.Single;
			this.maxTorqueTwist = UnusedValue.Single;
			this.damageAccum = UnusedValue.Single;
			this.damageAccumThresh = UnusedValue.Single;
			this.limitConstraint = UnusedValue.Vector;
			this.bBreakable = UnusedValue.Int32;
			this.bConstraintWillIgnoreCollisions = UnusedValue.Int32;
			this.bDirectBreaksOnly = UnusedValue.Int32;
			this.dampingConstraint = UnusedValue.Single;
			this.szSensor = UnusedValue.Single;
			this.bBroken = UnusedValue.Int32;
			this.partidEpicenter = UnusedValue.Int32;

			if (joint.partIsSpecified)
			{
				this.id = joint.PartId;
				this.idx = joint.PartIndex;
			}
		}
		#endregion
	}
}