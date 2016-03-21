using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the object that is used to get the surface identifier from the geometry
	/// of the physical entity.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.GetStatus"/> the return value is an indication of success.
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsStatusSurface
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.GetStatus"/> to query information
		/// about the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsStatus Base;
		[UsedImplicitly] private int ipart;
		[UsedImplicitly] private int partid;
		[UsedImplicitly] private int iPrim;
		[UsedImplicitly] private int iFeature;
		[UsedImplicitly] private int bUseProxy;
		[UsedImplicitly] private int id;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the object that represents the surface of requested geometry.
		/// </summary>
		public PhysicalSurface Surface => new PhysicalSurface(this.id);
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="part">    An object that specifies the part of the entity to query.</param>
		/// <param name="useProxy">
		/// Indicates whether to retrieve the surface identifier from the geometry object that is used
		/// specifically for physical interactions.
		/// </param>
		public PhysicsStatusSurface(EntityPartSpec part, bool useProxy = true)
			: this()
		{
			this.Base = new PhysicsStatus(PhysicsStatusTypes.Surface);
			this.partid = -1;
			this.ipart = -1;
			this.bUseProxy = useProxy ? 1 : 0;

			if (part.partIsSpecified)
			{
				this.partid = part.PartId;
				this.ipart = part.PartIndex;
			}
		}
		#endregion
	}
}