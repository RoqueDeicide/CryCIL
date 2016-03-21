using System;
using System.Linq;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the action that updates the constraint that binds this physical entity
	/// to another.
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
	public struct PhysicsActionUpdateConstraint
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.ActUpon"/> to apply this action to
		/// the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsAction Base;
		[UsedImplicitly] private int idConstraint;
		[UsedImplicitly] private uint flagsOR;
		[UsedImplicitly] private uint flagsAND;
		[UsedImplicitly] private int bRemove;
		[UsedImplicitly] private Vector3 pt0;
		[UsedImplicitly] private Vector3 pt1;
		[UsedImplicitly] private Quaternion qframe0;
		[UsedImplicitly] private Quaternion qframe1;
		[UsedImplicitly] private float maxPullForce, maxBendTorque;
		[UsedImplicitly] private float damping;
		[UsedImplicitly] private int flags;
		#endregion
		#region Properties
		/// <summary>
		/// Can be set to specify the identifier of the current set of constraints. If not specified this
		/// object updates all constraints.
		/// </summary>
		public int Identifier
		{
			set { this.idConstraint = value; }
		}
		/// <summary>
		/// Can be set to specify how to change the flags that are assigned to this set of constraints.
		/// </summary>
		public FlagParameters FlagParameters
		{
			set
			{
				if (value.HasFlagBase)
				{
					this.flags = value.FlagBase.SignedInt;
				}
				if (value.HasModFlags)
				{
					this.flagsOR = value.FlagsOr.UnsignedInt;
					this.flagsAND = value.FlagsAnd.UnsignedInt;
				}
			}
		}
		/// <summary>
		/// Can be set to specify whether this set of constraints must be removed.
		/// </summary>
		public bool Remove
		{
			set { this.bRemove = value ? 1 : 0; }
		}
		/// <summary>
		/// Can be set to specify the new position of the first constrained entity before attachment. See
		/// Remarks for details.
		/// </summary>
		/// <remarks>
		/// Initial positions of entities indicate initial relative positioning. If both
		/// <see cref="InitialPoint0"/> and <see cref="InitialPoint1"/> are specified and are different, the
		/// solver will attempt to bring constrained entities together to make sure that relative position
		/// is <see cref="Vector3.Zero"/>.
		/// </remarks>
		public Vector3 InitialPoint0
		{
			set { this.pt0 = value; }
		}
		/// <summary>
		/// Can be set to specify the new position of the second constrained entity before attachment. See
		/// Remarks for details.
		/// </summary>
		/// <remarks>
		/// Initial positions of entities indicate initial relative positioning. If both
		/// <see cref="InitialPoint0"/> and <see cref="InitialPoint1"/> are specified and are different, the
		/// solver will attempt to bring constrained entities together to make sure that relative position
		/// is <see cref="Vector3.Zero"/>.
		/// </remarks>
		public Vector3 InitialPoint1
		{
			set { this.pt1 = value; }
		}
		/// <summary>
		/// Can be set to specify the new orientation of the first constrained entity before attachment. See
		/// Remarks for details.
		/// </summary>
		public Quaternion InitialOrientation0
		{
			set { this.qframe0 = value; }
		}
		/// <summary>
		/// Can be set to specify the new orientation of the second constrained entity before attachment.
		/// See Remarks for details.
		/// </summary>
		/// <remarks>
		/// Initial orientations of entities indicate initial relative orientation. If both
		/// <see cref="InitialOrientation0"/> and <see cref="InitialOrientation1"/> are specified and are
		/// different, the solver will attempt to rotate constrained entities to make sure that relative
		/// position is <see cref="Quaternion.Identity"/>.
		/// </remarks>
		public Quaternion InitialOrientation1
		{
			set { this.qframe1 = value; }
		}
		/// <summary>
		/// Can be set to specify the new maximal pulling force the new constraint can handle before
		/// breaking.
		/// </summary>
		public float MaximalPullForce
		{
			set { this.maxPullForce = value; }
		}
		/// <summary>
		/// Can be set to specify the new maximal bending force the new constraint can handle before
		/// breaking.
		/// </summary>
		public float MaximalBendTorque
		{
			set { this.maxBendTorque = value; }
		}
		/// <summary>
		/// Can be set to specify the new internal constraint damping.
		/// </summary>
		public float Damping
		{
			set { this.damping = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass anything.</param>
		public PhysicsActionUpdateConstraint([UsedImplicitly] int notUsed)
		{
			this.Base = new PhysicsAction(PhysicsActionTypes.UpdateConstraint);
			this.idConstraint = UnusedValue.Int32;
			this.flagsOR = 0;
			this.flagsAND = uint.MaxValue;
			this.bRemove = UnusedValue.Int32;
			this.pt0 = UnusedValue.Vector;
			this.pt1 = UnusedValue.Vector;
			this.qframe0 = UnusedValue.Quaternion;
			this.qframe1 = UnusedValue.Quaternion;
			this.maxPullForce = UnusedValue.Single;
			this.maxBendTorque = UnusedValue.Single;
			this.damping = UnusedValue.Single;
			this.flags = UnusedValue.Int32;
		}
		#endregion
	}
}