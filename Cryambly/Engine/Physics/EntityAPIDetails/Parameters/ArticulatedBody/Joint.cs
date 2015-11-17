using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Engine.Memory;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of flags that indicates which frames to use when updating pivots of the joint.
	/// </summary>
	public enum PivotUpdateFlags
	{
		/// <summary>
		/// When set, specifies that neither parent nor child pivot points are updated. Probably is not
		/// used.
		/// </summary>
		None = 0,
		/// <summary>
		/// When set, specifies that parent pivot point is updated.
		/// </summary>
		Parent = 1,
		/// <summary>
		/// When set, specifies that child pivot point is updated.
		/// </summary>
		Child = 2,
		/// <summary>
		/// When set, specifies both pivot points are updated.
		/// </summary>
		Both = 3
	}
	/// <summary>
	/// Encapsulates a set of parameters that allows to specify the joint in the physical entity that is an
	/// articulated body.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct PhysicsParametersJoint
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to apply these
		/// onto physical entity or to <see cref="PhysicalEntity.GetParameters"/> to get the currently
		/// applied ones.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private uint flags;
		[UsedImplicitly] private int flagsPivot;
		[UsedImplicitly] private Vector3 pivot;
		[UsedImplicitly] private Quaternion q0;
		[UsedImplicitly] private EulerAngles limits1;
		[UsedImplicitly] private EulerAngles limits2;
		[UsedImplicitly] private EulerAngles bounciness;
		[UsedImplicitly] private EulerAngles ks, kd;
		[UsedImplicitly] private EulerAngles qdashpot;
		[UsedImplicitly] private EulerAngles kdashpot;
		[UsedImplicitly] private EulerAngles q;
		[UsedImplicitly] private EulerAngles qext;
		[UsedImplicitly] private EulerAngles qtarget;
		[UsedImplicitly] private int op1;
		[UsedImplicitly] private int op2;
		[UsedImplicitly] private int[] selfCollidingParts;
		[UsedImplicitly] private int nSelfCollidingParts;
		[UsedImplicitly] private int* pSelfCollidingParts;
		[UsedImplicitly] private int bNoUpdate;
		[UsedImplicitly] private float animationTimeStep;
		[UsedImplicitly] private float ranimationTimeStep;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the flags that describe the joint.
		/// </summary>
		public JointFlags Flags
		{
			get { return new JointFlags(this.flags); }
			set { this.flags = value.Flags; }
		}
		/// <summary>
		/// Gets or sets the value that indicates which pivot points (parent's and child's) are updated. By
		/// default both are updated.
		/// </summary>
		public PivotUpdateFlags PivotsToUpdate
		{
			get { return (PivotUpdateFlags)this.flagsPivot; }
			set { this.flagsPivot = (int)value; }
		}
		/// <summary>
		/// Gets or sets position of the pivot point of this joint in entity space.
		/// </summary>
		public Vector3 Pivot
		{
			get { return this.pivot; }
			set { this.pivot = value; }
		}
		/// <summary>
		/// Gets or sets the quaternion that represents base orientation of the child body in relation to
		/// the parent body.
		/// </summary>
		public Quaternion Orientation
		{
			get { return this.q0; }
			set { this.q0 = value; }
		}
		/// <summary>
		/// Gets or sets the low border of angle limit that is imposed on the joint.
		/// </summary>
		public EulerAngles LowAngleLimit
		{
			get { return this.limits1; }
			set { this.limits1 = value; }
		}
		/// <summary>
		/// Gets or sets the high border of angle limit that is imposed on the joint.
		/// </summary>
		public EulerAngles HighAngleLimit
		{
			get { return this.limits2; }
			set { this.limits2 = value; }
		}
		/// <summary>
		/// Gets or sets the object that defines the bounciness values that are applied when respective
		/// angle reaches one of its limits.
		/// </summary>
		public EulerAngles Bounciness
		{
			get { return this.bounciness; }
			set { this.bounciness = value; }
		}
		/// <summary>
		/// Gets or sets the object that specifies stiffness coefficients that are applied to springs of
		/// respective angles.
		/// </summary>
		public EulerAngles StiffnessCoefficients
		{
			get { return this.ks; }
			set { this.ks = value; }
		}
		/// <summary>
		/// Gets or sets the object that specifies damping coefficients that are applied to springs of
		/// respective angles.
		/// </summary>
		public EulerAngles DampingCoefficients
		{
			get { return this.kd; }
			set { this.kd = value; }
		}
		/// <summary>
		/// Gets or sets the object that specifies the extents of the vicinity from each of the limits of
		/// the respective angle when joint's rotation becomes dampened in that angle.
		/// </summary>
		public EulerAngles DashpotExtent
		{
			get { return this.kdashpot; }
			set { this.kdashpot = value; }
		}
		/// <summary>
		/// Gets or sets the object that specifies the strength of dampening of the respective angular
		/// velocity of the joint when it reaches the vicinity of one of the limits of the respective
		/// angle.
		/// </summary>
		public EulerAngles DashpotStrength
		{
			get { return this.qdashpot; }
			set { this.qdashpot = value; }
		}
		/// <summary>
		/// Gets or sets current angles of rotation of the joint relative to its base
		/// <see cref="Orientation"/>. Only these angles are taken into account when dampening and
		/// returning the joint to the base orientation.
		/// </summary>
		public EulerAngles Angles
		{
			get { return this.q; }
			set { this.q = value; }
		}
		/// <summary>
		/// Gets or sets current extra angles of rotation of the joint relative to its base
		/// <see cref="Orientation"/>. These angles are not taken into account when dampening and returning
		/// the joint to the base orientation.
		/// </summary>
		public EulerAngles ExtraAngles
		{
			get { return this.qext; }
			set { this.qext = value; }
		}
		/// <summary>
		/// Gets current effective angles of rotation of the joint relative to its base
		/// <see cref="Orientation"/>.
		/// </summary>
		public EulerAngles EffectiveAngles
		{
			get
			{
				return new EulerAngles(this.q.Pitch + this.qext.Pitch, this.q.Roll + this.qext.Roll,
									   this.q.Yaw + this.qext.Yaw);
			}
		}
		/// <summary>
		/// Gets or sets the identifier of the articulated body part that was specified using
		/// <see cref="GeometryParametersArticulatedBody"/> that is a parent part of this joint.
		/// </summary>
		/// <remarks>Parent is not required for the joint.</remarks>
		public int ParentId
		{
			get { return this.op1; }
			set { this.op1 = value; }
		}
		/// <summary>
		/// Gets or sets the identifier of the articulated body part that was specified using
		/// <see cref="GeometryParametersArticulatedBody"/> that is a child part of this joint.
		/// </summary>
		public int ChildId
		{
			get { return this.op2; }
			set { this.op2 = value; }
		}
		/// <summary>
		/// Gets or sets array of parts of the articulated body that can collide with each other.
		/// </summary>
		[SuppressMessage("ReSharper", "ExceptionNotDocumented")]
		public int[] SelfCollidingParts
		{
			get { return this.selfCollidingParts; }
			set
			{
				if (value.IsNullOrEmpty())
				{
					this.pSelfCollidingParts = null;
					this.nSelfCollidingParts = 0;
				}
				else
				{
					ulong size = (ulong)(Marshal.SizeOf(typeof(int)) * value.Length);
					int* parts = (int*)CryMarshal.Allocate(size, false).ToPointer();

					fixed (int* valuePtr = value)
					{
						for (int i = 0; i < value.Length; i++)
						{
							parts[i] = valuePtr[i];
						}
					}
					this.pSelfCollidingParts = parts;
					this.nSelfCollidingParts = value.Length;
				}
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether changes to this joint should not cause body
		/// parameters to be recalculated.
		/// </summary>
		public bool NoUpdate
		{
			get { return this.bNoUpdate != 0; }
			set { this.bNoUpdate = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the time step value that is used to calculate joint velocities for the animation.
		/// </summary>
		public float AnimationTimeStep
		{
			get { return this.animationTimeStep; }
			set
			{
				this.animationTimeStep = value;
				this.ranimationTimeStep = 1 / value;
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass any number to just invoke this constructor.</param>
		public PhysicsParametersJoint([UsedImplicitly] int notUsed)
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.Joint);
			this.flags = UnusedValue.UInt32;
			this.flagsPivot = 3;
			this.pivot = UnusedValue.Vector;
			this.q0 = UnusedValue.Quaternion;
			this.limits1 = new EulerAngles(UnusedValue.Single);
			this.limits2 = new EulerAngles(UnusedValue.Single);
			this.bounciness = new EulerAngles(UnusedValue.Single);
			this.kd = new EulerAngles(UnusedValue.Single);
			this.ks = new EulerAngles(UnusedValue.Single);
			this.qdashpot = new EulerAngles(UnusedValue.Single);
			this.kdashpot = new EulerAngles(UnusedValue.Single);
			this.q = new EulerAngles(UnusedValue.Single);
			this.qext = new EulerAngles(UnusedValue.Single);
			this.qtarget = new EulerAngles(UnusedValue.Single);
			this.op2 = UnusedValue.Int32;
			this.op1 = UnusedValue.Int32;
			this.selfCollidingParts = null;
			this.nSelfCollidingParts = UnusedValue.Int32;
			this.pSelfCollidingParts = null;
			this.bNoUpdate = 0;
			this.animationTimeStep = UnusedValue.Single;
			this.ranimationTimeStep = UnusedValue.Single;
		}
		#endregion
	}
}