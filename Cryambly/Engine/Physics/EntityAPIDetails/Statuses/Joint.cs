using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the object that is used to query the status of one of the joints of the
	/// physical entity that is an articulated body.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.GetStatus"/> the return value is an indication of success.
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsStatusJoint
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.GetStatus"/> to query information
		/// about the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsStatus Base;
		[UsedImplicitly] private int idChildBody;
		[UsedImplicitly] private int partid;
		[UsedImplicitly] private uint flags;
		[UsedImplicitly] private EulerAngles q;
		[UsedImplicitly] private EulerAngles qext;
		[UsedImplicitly] private EulerAngles dq;
		[UsedImplicitly] private Quaternion quat0;
		#endregion
		#region Properties
		/// <summary>
		/// Gets a set of flags that is assigned to this joint.
		/// </summary>
		public JointFlags JointFlags
		{
			get { return new JointFlags(this.flags); }
		}
		/// <summary>
		/// Gets a set of Euler angles that represent rotation of this joint away from its base orientation
		/// that is caused by the physical interactions.
		/// </summary>
		public EulerAngles CurrentPhysicalAngles
		{
			get { return this.q; }
		}
		/// <summary>
		/// Gets a set of Euler angles that represent rotation of this joint away from its base orientation
		/// combined with rotation that is represented by <see cref="CurrentPhysicalAngles"/> that is
		/// caused by the animation.
		/// </summary>
		public EulerAngles CurrentAnimationAngles
		{
			get { return this.qext; }
		}
		/// <summary>
		/// Gets current angular velocity of the joint.
		/// </summary>
		public EulerAngles CurrentAngularVelocity
		{
			get { return this.dq; }
		}
		/// <summary>
		/// Gets the quaternion that represents orientation of the joint in parent space when
		/// <see cref="CurrentPhysicalAngles"/> and <see cref="CurrentAnimationAngles"/> are zeroed.
		/// </summary>
		public Quaternion BaseOrientation
		{
			get { return this.quat0; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="number">     
		/// A number that can an identifier of any part that belongs to the child body of the joint or it
		/// can be an identifier of one of the child bodies. What it is is defined by
		/// <paramref name="getByPartId"/> argument.
		/// </param>
		/// <param name="getByPartId">
		/// Indicates whether <paramref name="number"/> is an identifier of any part that belongs to the
		/// child body of the joint or it's an identifier of one of the child bodies.
		/// </param>
		public PhysicsStatusJoint(int number, bool getByPartId = true)
			: this()
		{
			this.Base = new PhysicsStatus(PhysicsStatusTypes.Joint);
			this.idChildBody = UnusedValue.Int32;
			this.partid = UnusedValue.Int32;

			if (getByPartId)
			{
				this.partid = number;
			}
			else
			{
				this.idChildBody = number;
			}
		}
		#endregion
	}
}