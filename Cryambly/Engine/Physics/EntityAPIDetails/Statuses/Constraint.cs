using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the object that is used to query status of the constraint of the
	/// physical entity.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.GetStatus"/> the return value is an indication of success
	/// (if failure is returned then the constraint doesn't exist or broken).
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsStatusConstraint
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.GetStatus"/> to query information
		/// about the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsStatus Base;
		[UsedImplicitly] private int id;
		[UsedImplicitly] private int idx;
		[UsedImplicitly] private int flags;
		[UsedImplicitly] private Vector3 pt0;
		[UsedImplicitly] private Vector3 pt1;
		[UsedImplicitly] private Vector3 n;
		[UsedImplicitly] private PhysicalEntity pBuddyEntity;
		[UsedImplicitly] private PhysicalEntity pConstraintEntity;
		#endregion
		#region Properties
		/// <summary>
		/// Gets current flags that are set for the constraint.
		/// </summary>
		public ConstraintFlags Flags
		{
			get { return (ConstraintFlags)this.flags; }
		}
		/// <summary>
		/// Gets the coordinates of the first point of the constraint.
		/// </summary>
		public Vector3 FirstPoint
		{
			get { return this.pt0; }
		}
		/// <summary>
		/// Gets the coordinates of the second point of the constraint.
		/// </summary>
		public Vector3 SecondPoint
		{
			get { return this.pt1; }
		}
		/// <summary>
		/// Some normal.
		/// </summary>
		public Vector3 Normal
		{
			get { return this.n; }
		}
		/// <summary>
		/// Gets the second constrained entity.
		/// </summary>
		public PhysicalEntity Buddy
		{
			get { return this.pBuddyEntity; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="constraintId">Identifier of the constraint to query.</param>
		public PhysicsStatusConstraint(int constraintId)
			: this()
		{
			this.Base = new PhysicsStatus(PhysicsStatusTypes.Constraint);
			this.id = constraintId;
		}
		#endregion
	}
}