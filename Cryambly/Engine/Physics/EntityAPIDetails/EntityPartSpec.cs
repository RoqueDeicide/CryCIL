using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates parameters that can be used to specify which part of the physical entity needs to be
	/// affected when getting/setting parameters, querying status or executing an action.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct EntityPartSpec
	{
		#region Fields
		internal bool partIsSpecified;
		[UsedImplicitly] private int partId;
		[UsedImplicitly] private int partIndex;

		/// <summary>
		/// An object of type <see cref="EntityPartSpec"/> that can be used to designate entire entity.
		/// </summary>
		public static readonly EntityPartSpec EntireEntity;
		/// <summary>
		/// An object of type <see cref="EntityPartSpec"/> that can be used to designate all parts of the
		/// entity.
		/// </summary>
		public static readonly EntityPartSpec AllParts;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the identifier of the part of the physical entity.
		/// </summary>
		/// <remarks>
		/// This property should only be used when you query the part or you are getting its parameters.
		/// </remarks>
		public int PartId => this.partId;
		/// <summary>
		/// Gets zero-based index of the part of the physical entity.
		/// </summary>
		/// <remarks>
		/// This property should only be used when you query the part or you are getting its parameters.
		/// </remarks>
		public int PartIndex => this.partIndex;
		#endregion
		#region Construction
		private EntityPartSpec(int partId, int partIndex)
		{
			this.partId = partId;
			this.partIndex = partIndex;
			this.partIsSpecified = true;
		}
		/// <summary>
		/// Creates a new instance of type <see cref="EntityPartSpec"/> that can be used to specify the part
		/// of the entity using its identifier.
		/// </summary>
		/// <param name="id">Identifier of the part of the entity.</param>
		/// <returns>A valid object of type <see cref="EntityPartSpec"/>.</returns>
		public static EntityPartSpec FromId(int id)
		{
			return new EntityPartSpec(id, UnusedValue.Int32);
		}
		/// <summary>
		/// Creates a new instance of type <see cref="EntityPartSpec"/> that can be used to specify the part
		/// of the entity using its index.
		/// </summary>
		/// <param name="index">Zero-based index of the part of the entity.</param>
		/// <returns>A valid object of type <see cref="EntityPartSpec"/>.</returns>
		public static EntityPartSpec FromIndex(int index)
		{
			return new EntityPartSpec(UnusedValue.Int32, index);
		}
		#endregion
	}
}