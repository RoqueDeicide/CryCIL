using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the action that resets the dynamic state of the physical entity.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsActionReset
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.ActUpon"/> to apply this action
		/// to the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsAction Base;
		[UsedImplicitly] private bool clearContacts;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new action that resets the dynamic state of the entity.
		/// </summary>
		/// <param name="clearContacts">
		/// Indicates whether information about contacts with other entities should be cleared. By default
		/// it's <c>true</c>.
		/// </param>
		public PhysicsActionReset(bool clearContacts)
		{
			this.Base = new PhysicsAction(PhysicsActionTypes.Reset);
			this.clearContacts = clearContacts;
		}
		#endregion
	}
}