using System;
using System.Linq;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the action that sets a target pose for the physical entity that is a
	/// rope.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.ActUpon"/> the return value is an indication of success.
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsActionSetRopePose
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.ActUpon"/> to apply this action to
		/// the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsAction Base;
		[UsedImplicitly] private Vector3[] points;
		[UsedImplicitly] private Vector3 posHost;
		[UsedImplicitly] private Quaternion qHost;
		[UsedImplicitly] private IntPtr internal0;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new object that sets the target pose and transforms it.
		/// </summary>
		/// <param name="targetPoints">   
		/// An array of points that represents the target pose in coordinate space that is defined by the
		/// rope flags. Pass <c>null</c> to not be bothered with the pose.
		/// </param>
		/// <param name="hostOrientation">
		/// An orientation of the host the rope is attached to in world space.
		/// </param>
		/// <param name="hostPosition">   
		/// A position of the host the rope is attached to in world space.
		/// </param>
		public PhysicsActionSetRopePose(Vector3[] targetPoints, Quaternion hostOrientation,
										Vector3 hostPosition = new Vector3())
		{
			this.Base = new PhysicsAction(PhysicsActionTypes.SetRopeTargetPose);
			if (targetPoints != null && targetPoints.Length == 0)
			{
				targetPoints = null;
			}
			this.points = targetPoints;
			this.posHost = hostPosition;
			this.qHost = hostOrientation;
			this.internal0 = new IntPtr();
		}
		/// <summary>
		/// Creates a new object that sets the target pose and transforms it.
		/// </summary>
		/// <param name="targetPoints">
		/// An array of points that represents the target pose in coordinate space that is defined by the
		/// rope flags. Pass <c>null</c> to not be bothered with the pose.
		/// </param>
		/// <param name="hostPosition">
		/// A position of the host the rope is attached to in world space.
		/// </param>
		public PhysicsActionSetRopePose(Vector3[] targetPoints, Vector3 hostPosition = new Vector3())
			: this(targetPoints, Quaternion.Identity, hostPosition)
		{
		}
		#endregion
	}
}