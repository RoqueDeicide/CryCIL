using System;
using System.Linq;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the object that is used to check whether given ray intersects with the
	/// geometry of the physical entity.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.GetStatus"/> the return value is an indication of
	/// intersection (1 - intersects, 0 - doesn't).
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsStatusSampleContactArea
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.GetStatus"/> to query information
		/// about the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsStatus Base;
		[UsedImplicitly] private Vector3 ptTest;
		[UsedImplicitly] private Vector3 dirTest;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="point">    
		/// Coordinates of the point to project along <paramref name="direction"/>.
		/// </param>
		/// <param name="direction">Normalized vector <paramref name="point"/> to project along.</param>
		public PhysicsStatusSampleContactArea(Vector3 point, Vector3 direction)
		{
			this.Base = new PhysicsStatus(PhysicsStatusTypes.SampleContactArea);
			this.ptTest = point;
			this.dirTest = direction;
		}
		#endregion
	}
}