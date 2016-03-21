using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace CryCil.MemoryMapping
{
	/// <summary>
	/// 1-byte structure that can be interpreted as number of types.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 1)]
	public struct Byte1 : IBuffer
	{
		#region Fields
		/// <summary>
		/// <see cref="byte"/> value.
		/// </summary>
		[FieldOffset(0)] public byte UnsignedByte;
		/// <summary>
		/// <see cref="sbyte"/> value.
		/// </summary>
		[FieldOffset(0)] public sbyte SignedByte;
		#endregion
		#region Properties
		/// <summary>
		/// Returns 1.
		/// </summary>
		public int Length => 1;
		/// <summary>
		/// Gets or sets a byte.
		/// </summary>
		/// <param name="index">Ignored as object of this type is just one byte.</param>
		public byte this[int index]
		{
			get { return this.UnsignedByte; }
			set { this.UnsignedByte = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes new instance of <see cref="Byte1"/> type.
		/// </summary>
		/// <param name="value">Signed byte integer value to initialize this object with.</param>
		public Byte1(sbyte value)
			: this()
		{
			this.SignedByte = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Byte1"/> type.
		/// </summary>
		/// <param name="value">Unsigned byte integer value to initialize this object with.</param>
		public Byte1(byte value)
			: this()
		{
			this.UnsignedByte = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Byte1"/> type.
		/// </summary>
		/// <param name="pointer">Pointer to native memory cluster.</param>
		/// <param name="index">  
		/// Zero-based index of the byte within native memory cluster to use as initial value.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Unable to initialize new object of type Bytes4: Provided pointer is null.
		/// </exception>
		/// <exception cref="AccessViolationException">
		/// Base address ( <paramref name="pointer"/>) plus offset byte ( <paramref name="index"/>) produces
		/// a null or invalid address.
		/// </exception>
		public Byte1(IntPtr pointer, int index)
			: this()
		{
			if (pointer == null)
			{
				throw new ArgumentNullException(nameof(pointer),
												"Byte1.Constructor: Unable to initialize new object of type Byte1: Provided pointer is null.");
			}
			this.UnsignedByte = Marshal.ReadByte(pointer, index);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets one byte from native memory cluster.
		/// </summary>
		/// <param name="handle">Pointer to the beginning of native memory cluster.</param>
		/// <param name="offset">Zero-based index of the byte within native memory cluster to get.</param>
		/// <exception cref="AccessViolationException">
		/// Base address ( <paramref name="handle"/>) plus offset byte ( <paramref name="offset"/>) produces
		/// a null or invalid address.
		/// </exception>
		public void Get(IntPtr handle, int offset)
		{
			if (handle == IntPtr.Zero)
			{
				return;
			}
			this.UnsignedByte = Marshal.ReadByte(handle, offset);
		}
		/// <summary>
		/// Writes one byte to native memory cluster.
		/// </summary>
		/// <param name="handle">Pointer to the beginning of native memory cluster.</param>
		/// <param name="offset">Zero-based index of the byte within native memory cluster to set.</param>
		/// <exception cref="AccessViolationException">
		/// Base address ( <paramref name="handle"/>) plus offset byte ( <paramref name="offset"/>) produces
		/// a null or invalid address.
		/// </exception>
		public void Set(IntPtr handle, int offset)
		{
			if (handle == IntPtr.Zero)
			{
				return;
			}
			Marshal.WriteByte(handle, offset, this.UnsignedByte);
		}
		#endregion
	}
}