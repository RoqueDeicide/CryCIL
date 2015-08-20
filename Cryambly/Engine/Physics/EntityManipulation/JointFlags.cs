using System;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// This is a quasi-enumeration that is used to specify flags that are set for articulated body joints.
	/// </summary>
	public struct JointFlags
	{
		#region Nested Types
		/// <summary>
		/// Represents a flag that can be assigned to one of 3 flags: 0 - pitch, 1 - roll, 2 - yaw.
		/// </summary>
		public struct JointAngleFlags
		{
			private readonly uint flag;
			/// <summary>
			/// Gets the flag for the specified angle.
			/// </summary>
			/// <param name="index">Zero-based index of the angle to get the flag for.</param>
			/// <returns>The flag for the specified angle.</returns>
			public JointFlags this[int index]
			{
				[Pure]
				get
				{
					if (index < 0 || index > 2)
					{
						throw new IndexOutOfRangeException("Index of the angle to get the flag can either be 0, 1 or 2.");
					}
					return new JointFlags(this.flag << index);
				}
			}
			internal JointAngleFlags(uint flag)
			{
				this.flag = flag;
			}
		}
		#endregion
		#region Available Flags
		/// <summary>
		/// When set specifies that the angle is locked to the specific position.
		/// </summary>
		public static readonly JointAngleFlags Locked = new JointAngleFlags(1);
		/// <summary>
		/// When set, specifies that all Euler angles are locked. This value is used to lock the joint into
		/// specific orientation which prevents connected parts from moving relatively to each other.
		/// </summary>
		public static readonly JointFlags AllLocked = new JointFlags(7);
		/// <summary>
		/// Unknown.
		/// </summary>
		public static readonly JointAngleFlags LimitReached = new JointAngleFlags(10);
		/// <summary>
		/// When set, specifies that the damping coefficient for joint springs needs to be automatically
		/// recalculated.
		/// </summary>
		public static readonly JointAngleFlags AutoCalculateDampingCoefficient = new JointAngleFlags(100);
		/// <summary>
		/// When set, specifies that the joint doesn't have the gravity affect it. Used for optimization.
		/// </summary>
		public static readonly JointFlags NoGravity = new JointFlags(1000);
		/// <summary>
		/// When set, specifies that the joint has to use a special mode where it treats springs not like
		/// physical springs but rather like some guidelines on what the acceleration should be like (this
		/// mode is recommended for simulating effects on a skeleton).
		/// </summary>
		public static readonly JointFlags IsolatedAccelerations = new JointFlags(2000);
		/// <summary>
		/// When set, specifies that this joint is using 2 point-to-point constraints representation of
		/// hinge points.
		/// </summary>
		public static readonly JointFlags ExpandHinge = new JointFlags(4000);
		/// <summary>
		/// When set, specifies that no changes to the specified angle are effective due to gimbal lock.
		/// </summary>
		public static readonly JointAngleFlags GimbalLocked = new JointAngleFlags(10000);
		/// <summary>
		/// When set, specified that the angle is close enough to its limit that the dash-pot is now
		/// dampening movement of parts.
		/// </summary>
		public static readonly JointFlags DashpotReached = new JointFlags(100000);
		/// <summary>
		/// When set, specifies that the joint must ignore external impulses.
		/// </summary>
		public static readonly JointFlags IgnoreImpulses = new JointFlags(200000);
		#endregion
		#region Fields
		private readonly uint flags;
		#endregion
		#region Properties
		internal uint Flags
		{
			get { return this.flags; }
		}
		#endregion
		#region Events
		#endregion
		#region Construction
		internal JointFlags(uint flags)
		{
			this.flags = flags;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Applies bitwise inclusive OR operation.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>
		/// A new object of this type that has all flags that are specified by both operands set.
		/// </returns>
		public static JointFlags operator |(JointFlags left, JointFlags right)
		{
			return new JointFlags(left.flags | right.flags);
		}
		/// <summary>
		/// Applies bitwise exclusive OR operation.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>
		/// A new object of this type that has all flags that are specified by both operands set.
		/// </returns>
		public static JointFlags operator ^(JointFlags left, JointFlags right)
		{
			return new JointFlags(left.flags ^ right.flags);
		}
		/// <summary>
		/// Applies bitwise AND operation.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>
		/// A new object of this type that has all flags that are only set in both operands set.
		/// </returns>
		public static JointFlags operator &(JointFlags left, JointFlags right)
		{
			return new JointFlags(left.flags & right.flags);
		}
		/// <summary>
		/// Applies bitwise NOT operation.
		/// </summary>
		/// <param name="flagsToInvert">Flags to invert.</param>
		/// <returns>
		/// A new object where all flags that were not set in the operand set and vice-versa.
		/// </returns>
		public static JointFlags operator ~(JointFlags flagsToInvert)
		{
			return new JointFlags(~flagsToInvert.flags);
		}
		/// <summary>
		/// Determines whether this instance has all specified flags set.
		/// </summary>
		/// <param name="flags">An object that defines the flags that must be set.</param>
		/// <returns>True, all specified flags are set, otherwise false.</returns>
		public bool HasAll(JointFlags flags)
		{
			return (this.flags & flags.flags) == flags.flags;
		}
		/// <summary>
		/// Determines whether this instance has any of specified flags set.
		/// </summary>
		/// <param name="flags">An object that defines the flags to check.</param>
		/// <returns>True, any of specified flags are set, otherwise false.</returns>
		public bool HasAny(JointFlags flags)
		{
			return (this.flags & flags.flags) != 0;
		}
		#endregion
	}
}