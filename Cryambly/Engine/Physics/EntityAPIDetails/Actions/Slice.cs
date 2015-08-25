using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the action that slices the physical entity or part of it with a plane.
	/// </summary>
	/// <remarks>
	/// <para>When passed to <see cref="PhysicalEntity.ActUpon"/> the return value is an indication of success.</para>
	/// <para>Never use objects of this type that were created using a default constructor (they are not configured properly!).</para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsActionSlice
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.ActUpon"/> to apply this action to the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsAction Base;
		[UsedImplicitly] private Plane slicingPlane;
		[UsedImplicitly] private int ipart;
		[UsedImplicitly] private int partid;
		[UsedImplicitly] private IntPtr internal0;
		#endregion
		#region Construction
		/// <summary>
		/// Creates an object that represents an action that slices the entity of part of it with a plane.
		/// </summary>
		/// <param name="part">An object that specify which part of the entity to slice.</param>
		/// <param name="plane">An object that represents a slicing plane.</param>
		public PhysicsActionSlice(EntityPartSpec part, Plane plane)
		{
			this.Base = new PhysicsAction(PhysicsActionTypes.Slice);
			this.ipart = UnusedValue.Int32;
			this.partid = UnusedValue.Int32;
			this.slicingPlane = plane;
			this.internal0 = new IntPtr();

			if (part.partIsSpecified)
			{
				this.ipart = part.PartIndex;
				this.partid = part.PartId;
			}
		}
		#endregion
	}
}