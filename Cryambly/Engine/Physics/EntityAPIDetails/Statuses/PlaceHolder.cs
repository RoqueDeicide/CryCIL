using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the object that is used to the full entity of a place holder.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.GetStatus"/> the return value is an indication of entity
	/// being a place holder (1 - is a place holder, 0 - is not a place holder).
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsStatusPlaceHolder
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.GetStatus"/> to query information
		/// about the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsStatus Base;
		[UsedImplicitly] private PhysicalEntity pFullEntity;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the full entity that corresponds to the place holder this was called on.
		/// </summary>
		public PhysicalEntity FullEntity => this.pFullEntity;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass anything.</param>
		public PhysicsStatusPlaceHolder([UsedImplicitly] int notUsed)
			: this()
		{
			this.Base = new PhysicsStatus(PhysicsStatusTypes.PlaceHolder);
		}
		#endregion
	}
}