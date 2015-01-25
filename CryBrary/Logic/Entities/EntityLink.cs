using System;
using System.Runtime.InteropServices;
using System.Text;
using CryEngine.Annotations;
using CryEngine.Entities;

namespace CryEngine.Logic.Entities
{
	/// <summary>
	/// Encapsulates details about the link between the entities.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct EntityLink
	{
		#region Fields
		/// <summary>
		/// Array of bytes that represents ASCII string that is a name of the link.
		/// </summary>
		[UsedImplicitly]
		[FieldOffset(0)]
		private byte name;
		/// <summary>
		/// Fast identifier of the target.
		/// </summary>
		[FieldOffset(32)]
		public EntityId EntityId;
		/// <summary>
		/// Global identifier of the target.
		/// </summary>
		[FieldOffset(36)]
		public ulong EntityGuid;
		[UsedImplicitly]
		[FieldOffset(44)]
		private IntPtr Next;     // Pointer to the next link, or NULL if last link.
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the name of the link.
		/// </summary>
		public unsafe string Name
		{
			get
			{
				byte[] bytes = new byte[32];
				fixed (EntityLink* ptr = &this)
				{
					byte* bytePtr = (byte*)ptr;
					for (int i = 0; i < 32; i++)
					{
						bytes[i] = bytePtr[i];
					}
				}
				return Encoding.ASCII.GetString(bytes);
			}
			set
			{
				byte[] bytes = Encoding.ASCII.GetBytes(value);
				if (bytes.Length > 32)
				{
					throw new ArgumentException("Name of the entity link is too big.");
				}
				fixed (EntityLink* ptr = &this)
				{
					byte* bytePtr = (byte*)ptr;
					for (int i = 0; i < 32; i++)
					{
						bytePtr[i] = bytes[i];
					}
				}
			}
		}
		#endregion
	}
}