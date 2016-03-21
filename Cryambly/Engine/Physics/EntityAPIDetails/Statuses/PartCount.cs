using System;
using System.Linq;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the object that is used to query the number of parts the physical entity
	/// currently has.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.GetStatus"/> the return value is a number of parts.
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsStatusPartCount
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.GetStatus"/> to query information
		/// about the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsStatus Base;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass anything.</param>
		public PhysicsStatusPartCount([UsedImplicitly] int notUsed)
		{
			this.Base = new PhysicsStatus(PhysicsStatusTypes.PartCount);
		}
		#endregion
	}
}