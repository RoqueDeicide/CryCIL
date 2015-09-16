using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the action that bakes the matrix of the part of the physical entity
	/// into the entity's one and clears the former.
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
	public struct PhysicsActionResetPartMatrix
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.ActUpon"/> to apply this action
		/// to the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsAction Base;
		[UsedImplicitly] private int ipart;
		[UsedImplicitly] private int partid;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="spec">An object that specifies which part to affect.</param>
		public PhysicsActionResetPartMatrix(EntityPartSpec spec)
		{
			this.Base = new PhysicsAction(PhysicsActionTypes.ResetPartMatrix);
			this.ipart = UnusedValue.Int32;
			this.partid = UnusedValue.Int32;

			if (spec.partIsSpecified)
			{
				this.ipart = spec.PartIndex;
				this.partid = spec.PartId;
			}
		}
		#endregion
	}
}