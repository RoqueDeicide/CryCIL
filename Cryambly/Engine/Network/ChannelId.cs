﻿using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Network
{
	/// <summary>
	/// Represents identifier of a network channel that can be used to communicate via network.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct ChannelId
	{
		#region Fields
		[UsedImplicitly] private ushort id;
		#endregion
		#region Properties
		/// <summary>
		/// Determines whether this identifier is valid.
		/// </summary>
		public bool IsValid
		{
			get { return this.id == 0; }
		}
		#endregion
		#region Events
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="id">Identifier to assign to this object.</param>
		public ChannelId(ushort id)
		{
			this.id = id;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Implicitly converts this object to a 32-bit integer number.
		/// </summary>
		/// <param name="id">Object to convert.</param>
		/// <returns>Underlying channel identifier represented by <see cref="int"/> type.</returns>
		public static implicit operator int(ChannelId id)
		{
			return id.id;
		}
		#endregion
		#region Utilities
		#endregion
	}
}