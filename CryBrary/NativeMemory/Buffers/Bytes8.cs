using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using CryEngine.Native;

namespace CryEngine.NativeMemory
{
	/// <summary>
	/// Encapsulates 8 bytes worth of data.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	public unsafe struct Bytes8 : IBuffer
	{
		#region Fields
		/// <summary>
		/// <see cref="UInt64" /> value.
		/// </summary>
		[FieldOffset(0)]
		public ulong UnsignedLong;
		/// <summary>
		/// <see cref="Int64" /> value.
		/// </summary>
		[FieldOffset(0)]
		public long SignedLong;
		/// <summary>
		/// <see cref="Double" /> value.
		/// </summary>
		[FieldOffset(0)]
		public double DoubleFloat;
		/// <summary>
		/// Individual 8 bytes.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] Bytes;
		/// <summary>
		/// Two separate 32-bit values.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public Bytes4[] Quads;
		/// <summary>
		/// Four separate 16-bit values.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public Bytes2[] Doubles;
		#endregion
		#region Properties
		/// <summary>
		/// Returns 8.
		/// </summary>
		public ulong Length
		{
			get { return 8; }
		}
		/// <summary>
		/// Gets or sets a byte.
		/// </summary>
		/// <param name="index">Zero-based index of the byte to get or set.</param>
		public byte this[ulong index]
		{
			get
			{
				return this.Bytes[index];
			}
			set
			{
				this.Bytes[index] = value;
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes new instance of <see cref="Bytes8" /> type.
		/// </summary>
		/// <param name="value"><see cref="Int64" /> value to initialize this object with.</param>
		public Bytes8(long value)
			: this()
		{
			this.SignedLong = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes8" /> type.
		/// </summary>
		/// <param name="value"><see cref="UInt64" /> value to initialize this object with.</param>
		public Bytes8(ulong value)
			: this()
		{
			this.UnsignedLong = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes8" /> type.
		/// </summary>
		/// <param name="value"><see cref="Double" /> value to initialize this object with.</param>
		public Bytes8(double value)
			: this()
		{
			this.DoubleFloat = value;
		}
		/// <summary>
		/// Initializes new 8-byte buffer object.
		/// </summary>
		/// <param name="array">Array that provides elements to fill new buffer with.</param>
		/// <param name="shift">Zero-based index of first byte in given array.</param>
		public Bytes8(byte[] array, ulong shift)
			: this()
		{
			Contract.Requires((ulong)array.LongLength <= shift + this.Length);
			fixed (byte* buffer = this.Bytes)
			{
				for (ulong i = 0; i < this.Length; i++)
				{
					buffer[i] = array[shift + i];
				}
			}
		}
		/// <summary>
		/// Initializes new 8-byte buffer object.
		/// </summary>
		/// <param name="bytes">
		/// Fixed size buffer that provides elements to fill new buffer with.
		/// </param>
		/// <param name="shift">Zero-based index of first byte in given buffer.</param>
		public Bytes8(byte* bytes, ulong shift)
			: this()
		{
			fixed (byte* buffer = this.Bytes)
			{
				for (ulong i = 0; i < this.Length; i++)
				{
					buffer[i] = bytes[shift + i];
				}
			}
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes8" /> type.
		/// </summary>
		/// <param name="pointer">Pointer to native memory cluster.</param>
		/// <param name="index">
		/// Zero-based index of the first of 8 bytes within native memory cluster.
		/// </param>
		public Bytes8(IntPtr pointer, ulong index)
			: this()
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer", "Bytes8.Constructor: Unable to initialize new object of type Bytes8: Provided pointer is null.");
			}
			this = NativeMemoryHandlingMethods.Get8Bytes(pointer, index);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets 8 bytes from native memory cluster.
		/// </summary>
		/// <param name="handle">Pointer to the beginning of native memory cluster.</param>
		/// <param name="offset">
		/// Zero-based index of first of 8 bytes within native memory cluster to get.
		/// </param>
		public void Get(IntPtr handle, ulong offset)
		{
			if (handle == IntPtr.Zero)
			{
				return;
			}
			this = NativeMemoryHandlingMethods.Get8Bytes(handle, offset);
		}
		/// <summary>
		/// Writes 8 bytes to native memory cluster.
		/// </summary>
		/// <param name="handle">Pointer to the beginning of native memory cluster.</param>
		/// <param name="offset">
		/// Zero-based index of first of 8 bytes within native memory cluster to set.
		/// </param>
		public void Set(IntPtr handle, ulong offset)
		{
			if (handle == IntPtr.Zero)
			{
				return;
			}
			NativeMemoryHandlingMethods.Set8Bytes(handle, offset, this.UnsignedLong);
		}
		#endregion
	}
}