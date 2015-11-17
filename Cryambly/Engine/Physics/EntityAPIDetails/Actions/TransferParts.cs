using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the action that moves a selection of parts from one physical entity to
	/// another.
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
	public struct PhysicsActionTransferParts
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.ActUpon"/> to apply this action
		/// to the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsAction Base;
		[UsedImplicitly] private int idStart, idEnd;
		[UsedImplicitly] private int idOffset;
		[UsedImplicitly] private PhysicalEntity pTarget;
		[UsedImplicitly] private Matrix34 mtxRel;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new object that represents an action that transfers a number of parts from one
		/// physical entity to another and applies a moving transformation to each part.
		/// </summary>
		/// <param name="pTarget"> A physical entity to move the parts to.</param>
		/// <param name="mtxRel">  
		/// A 3x4 matrix that represents relative transformation to apply to each part (the formula is:
		/// newMatrix = mtxRel * matrixCurrent).
		/// </param>
		/// <param name="idStart"> Identifier of the first part in the range to transfer.</param>
		/// <param name="idEnd">   Identifier of the last part in the range to transfer.</param>
		/// <param name="idOffset">A value to add to the identifier of each transfered part.</param>
		public PhysicsActionTransferParts(PhysicalEntity pTarget, Matrix34 mtxRel, int idStart = 0, int idEnd = 1 << 30,
										  int idOffset = 0)
		{
			this.Base = new PhysicsAction(PhysicsActionTypes.MoveParts);
			this.idStart = idStart;
			this.idEnd = idEnd;
			this.idOffset = idOffset;
			this.pTarget = pTarget;
			this.mtxRel = mtxRel;
		}
		/// <summary>
		/// Creates a new object that represents an action that transfers a number of parts from one
		/// physical entity to another.
		/// </summary>
		/// <param name="pTarget"> A physical entity to move the parts to.</param>
		/// <param name="idStart"> Identifier of the first part in the range to transfer.</param>
		/// <param name="idEnd">   Identifier of the last part in the range to transfer.</param>
		/// <param name="idOffset">A value to add to the identifier of each transfered part.</param>
		public PhysicsActionTransferParts(PhysicalEntity pTarget, int idStart = 0, int idEnd = 1 << 30, int idOffset = 0)
			: this(pTarget, Matrix34.Identity, idStart, idEnd, idOffset)
		{
		}
		#endregion
	}
}