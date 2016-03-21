using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
		private readonly ushort id;
		#endregion
		#region Properties
		/// <summary>
		/// Determines whether this identifier is valid.
		/// </summary>
		public bool IsValid => this.id == 0;
		/// <summary>
		/// Gets the object that represents the channel this object is an identifier of.
		/// </summary>
		public CryNetChannel Channel => GetChannel(this.id);
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
		/// Implicitly converts an object of this type to a 32-bit integer number.
		/// </summary>
		/// <param name="id">Object to convert.</param>
		/// <returns>Underlying channel identifier represented by <see cref="int"/> type.</returns>
		public static implicit operator int(ChannelId id)
		{
			return id.id;
		}
		/// <summary>
		/// Implicitly converts an object of this type to a 16-bit integer number.
		/// </summary>
		/// <param name="id">Object to convert.</param>
		/// <returns>Underlying channel identifier represented by <see cref="ushort"/> type.</returns>
		public static implicit operator ushort(ChannelId id)
		{
			return id.id;
		}
		/// <summary>
		/// Implicitly converts 32-bit integer number to an object of this type.
		/// </summary>
		/// <param name="id">Object to convert.</param>
		/// <returns>An identifier object.</returns>
		public static implicit operator ChannelId(int id)
		{
			return new ChannelId((ushort)id);
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryNetChannel GetChannel(ushort id);
		#endregion
	}
}