using System;
using System.Linq;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the object that is used to check whether a point is with bounds of the
	/// physical entity.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.GetStatus"/> the return value is an indication of entity
	/// being containing a point (1 - contains, 0 - doesn't contain).
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsStatusContainsPoint
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.GetStatus"/> to query information
		/// about the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsStatus Base;
		[UsedImplicitly] private Vector3 pt;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="point">A point to check</param>
		public PhysicsStatusContainsPoint(Vector3 point)
		{
			this.Base = new PhysicsStatus(PhysicsStatusTypes.ContainsPoint);
			this.pt = point;
		}
		#endregion
	}
}