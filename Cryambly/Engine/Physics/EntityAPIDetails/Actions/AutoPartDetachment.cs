using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the action that sets the auto-detachment conditions for parts of the
	/// physical entity that is an articulated body with pre-calculated physics simulation.
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
	public struct PhysicsActionAutoPartDetachment
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.ActUpon"/> to apply this action
		/// to the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsAction Base;
		[UsedImplicitly] private float threshold;
		[UsedImplicitly] private float autoDetachmentDist;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the force limit on each joint. If not set the joints won't have the force limit.
		/// </summary>
		public float Threshold
		{
			get { return this.threshold; }
			set { this.threshold = value; }
		}
		/// <summary>
		/// Gets or sets the distance from the joint's pivot for the former to detach from the main body.
		/// </summary>
		public float AutoDetachmentDistance
		{
			get { return this.autoDetachmentDist; }
			set { this.autoDetachmentDist = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass anything.</param>
		public PhysicsActionAutoPartDetachment([UsedImplicitly] int notUsed)
		{
			this.Base = new PhysicsAction(PhysicsActionTypes.AutoPartDetachment);
			this.threshold = UnusedValue.Single;
			this.autoDetachmentDist = UnusedValue.Single;
		}
		#endregion
	}
}