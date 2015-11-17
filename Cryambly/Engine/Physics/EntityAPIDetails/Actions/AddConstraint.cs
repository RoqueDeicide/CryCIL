using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the action that adds a constraint to the physical entity.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.ActUpon"/> the return value is an identifier of the
	/// constraint, if 0 returned then the action failed.
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsActionAddConstraint
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.ActUpon"/> to apply this action
		/// to the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsAction Base;
		[UsedImplicitly] private int id;
		[UsedImplicitly] private PhysicalEntity pBuddy;
		[UsedImplicitly] private Vector3 pt0;
		[UsedImplicitly] private Vector3 pt1;
		[UsedImplicitly] private int partid0;
		[UsedImplicitly] private int partid1;
		[UsedImplicitly] private Quaternion qframe0;
		[UsedImplicitly] private Quaternion qframe1;
		[UsedImplicitly] private float xlimits0;
		[UsedImplicitly] private float xlimits1;
		[UsedImplicitly] private float yzlimits0;
		[UsedImplicitly] private float yzlimits1;
		[UsedImplicitly] private ConstraintFlags flags;
		[UsedImplicitly] private float damping;
		[UsedImplicitly] private float sensorRadius;
		[UsedImplicitly] private float maxPullForce, maxBendTorque;
		#endregion
		#region Properties
		/// <summary>
		/// Can be set to force the constraint to have specific identifier. The identifier doesn't have to
		/// be unique.
		/// </summary>
		/// <remarks>
		/// If not set, the identifier will be assigned automatically and returned as a return value of
		/// <see cref="PhysicalEntity.ActUpon"/>.
		/// </remarks>
		public int Identifier
		{
			set { this.id = value; }
		}
		/// <summary>
		/// Gets or sets the second entity this one is to be constrained to. Can be
		/// <see cref="PhysicalEntity.World"/> for static attachments.
		/// </summary>
		public PhysicalEntity Buddy
		{
			get { return this.pBuddy; }
			set { this.pBuddy = value; }
		}
		/// <summary>
		/// Gets or sets the position of the first constrained entity before attachment. See Remarks for
		/// details.
		/// </summary>
		/// <remarks>
		/// Initial positions of entities indicate initial relative positioning. If both
		/// <see cref="InitialPoint0"/> and <see cref="InitialPoint1"/> are specified and are different,
		/// the solver will attempt to bring constrained entities together to make sure that relative
		/// position is <see cref="Vector3.Zero"/>.
		/// </remarks>
		public Vector3 InitialPoint0
		{
			get { return this.pt0; }
		}
		/// <summary>
		/// Gets or sets the position of the second constrained entity before attachment. See Remarks for
		/// details.
		/// </summary>
		/// <remarks>
		/// Initial positions of entities indicate initial relative positioning. If both
		/// <see cref="InitialPoint0"/> and <see cref="InitialPoint1"/> are specified and are different,
		/// the solver will attempt to bring constrained entities together to make sure that relative
		/// position is <see cref="Vector3.Zero"/>.
		/// </remarks>
		public Vector3 InitialPoint1
		{
			get { return this.pt1; }
			set { this.pt1 = value; }
		}
		/// <summary>
		/// Gets or sets the orientation of the first constrained entity before attachment. See Remarks for
		/// details.
		/// </summary>
		public Quaternion InitialOrientation0
		{
			get { return this.qframe0; }
			set { this.qframe0 = value; }
		}
		/// <summary>
		/// Gets or sets the orientation of the second constrained entity before attachment. See Remarks
		/// for details.
		/// </summary>
		/// <remarks>
		/// Initial orientations of entities indicate initial relative orientation. If both
		/// <see cref="InitialOrientation0"/> and <see cref="InitialOrientation1"/> are specified and are
		/// different, the solver will attempt to rotate constrained entities to make sure that relative
		/// position is <see cref="Quaternion.Identity"/>.
		/// </remarks>
		public Quaternion InitialOrientation1
		{
			get { return this.qframe1; }
			set { this.qframe1 = value; }
		}
		/// <summary>
		/// Gets or sets identifier of the part of the first entity to bind with this constraint. If not
		/// specified, the first part is assumed.
		/// </summary>
		public int EntityPartId0
		{
			get { return this.partid0; }
			set { this.partid0 = value; }
		}
		/// <summary>
		/// Gets or sets identifier of the part of the second entity to bind with this constraint. If not
		/// specified, the first part is assumed.
		/// </summary>
		public int EntityPartId1
		{
			get { return this.partid1; }
			set { this.partid1 = value; }
		}
		/// <summary>
		/// Gets or sets the lower limit of rotation around X-axis (twist).
		/// </summary>
		/// <remarks>
		/// <para>
		/// If <see cref="XRotationLowerLimit"/> is greater or equal to <see cref="XRotationUpperLimit"/>
		/// the twisting is not allowed at all.
		/// </para>
		/// <para>
		/// Current assumption is that the X-axis is specified by the <see cref="InitialOrientation0"/>.
		/// </para>
		/// </remarks>
		public float XRotationLowerLimit
		{
			get { return this.xlimits0; }
			set { this.xlimits0 = value; }
		}
		/// <summary>
		/// Gets or sets the upper limit of rotation around X-axis (twist).
		/// </summary>
		/// <remarks>
		/// <para>
		/// If <see cref="XRotationLowerLimit"/> is greater or equal to <see cref="XRotationUpperLimit"/>
		/// the twisting is not allowed at all.
		/// </para>
		/// <para>
		/// Current assumption is that the X-axis is specified by the <see cref="InitialOrientation0"/>.
		/// </para>
		/// </remarks>
		public float XRotationUpperLimit
		{
			get { return this.xlimits1; }
			set { this.xlimits1 = value; }
		}
		/// <summary>
		/// Gets or sets the maximal tilt of X-axis of the first constraint frame.
		/// </summary>
		/// <remarks>First constraint frame is created from <see cref="InitialOrientation0"/>.</remarks>
		public float YZRotationLimit
		{
			get { return this.yzlimits1; }
			set { this.yzlimits1 = value; }
		}
		/// <summary>
		/// Gets or sets the flags that specify the constraint.
		/// </summary>
		public ConstraintFlags Flags
		{
			get { return this.flags; }
			set { this.flags = value; }
		}
		/// <summary>
		/// Gets or sets the internal constraint damping.
		/// </summary>
		public float Damping
		{
			get { return this.damping; }
			set { this.damping = value; }
		}
		/// <summary>
		/// Gets or sets the radius of the sphere that is used to scan environment to reattach the
		/// constraint if it breaks.
		/// </summary>
		public float SensorRadius
		{
			get { return this.sensorRadius; }
			set { this.sensorRadius = value; }
		}
		/// <summary>
		/// Gets or sets the maximal pulling force the new constraint can handle before breaking.
		/// </summary>
		public float MaximalPullForce
		{
			get { return this.maxPullForce; }
			set { this.maxPullForce = value; }
		}
		/// <summary>
		/// Gets or sets the maximal bending force the new constraint can handle before breaking.
		/// </summary>
		public float MaximalBendTorque
		{
			get { return this.maxBendTorque; }
			set { this.maxBendTorque = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new valid object of this type.
		/// </summary>
		/// <param name="pBuddy">      Another physical entity to bind to this one.</param>
		/// <param name="initialPoint">
		/// Optional value that can be used to bring bound entities together.
		/// </param>
		public PhysicsActionAddConstraint(PhysicalEntity pBuddy, Vector3 initialPoint = new Vector3())
		{
			this.Base = new PhysicsAction(PhysicsActionTypes.AddConstraint);
			this.id = UnusedValue.Int32;
			this.pBuddy = pBuddy;
			this.pt0 = initialPoint;
			this.pt1 = UnusedValue.Vector;
			this.partid0 = UnusedValue.Int32;
			this.partid1 = UnusedValue.Int32;
			this.qframe0 = UnusedValue.Quaternion;
			this.qframe1 = UnusedValue.Quaternion;
			this.xlimits0 = UnusedValue.Single;
			this.yzlimits0 = UnusedValue.Single;
			this.xlimits1 = UnusedValue.Single;
			this.yzlimits1 = UnusedValue.Single;
			this.flags = ConstraintFlags.WorldFrames;
			this.damping = UnusedValue.Single;
			this.sensorRadius = UnusedValue.Single;
			this.maxPullForce = UnusedValue.Single;
			this.maxBendTorque = UnusedValue.Single;
		}
		#endregion
	}
}