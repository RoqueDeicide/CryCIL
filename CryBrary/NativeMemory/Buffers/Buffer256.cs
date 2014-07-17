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
	/// Encapsulates 256 bytes worth of data.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 256)]
	public unsafe struct Buffer256 : IBuffer
	{
		#region Fields
		/// <summary>
		/// 256 <see cref="Byte" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public byte[] Bytes;
		/// <summary>
		/// 256 <see cref="SByte" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public sbyte[] Sbytes;
		/// <summary>
		/// 128 <see cref="Int16" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public short[] Shorts;
		/// <summary>
		/// 128 <see cref="UInt16" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public ushort[] Ushorts;
		/// <summary>
		/// 64 <see cref="Int32" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		public int[] Ints;
		/// <summary>
		/// 64 <see cref="UInt32" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		public uint[] Uints;
		/// <summary>
		/// 32 <see cref="Int64" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public long[] Longs;
		/// <summary>
		/// 32 <see cref="UInt64" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public ulong[] Ulongs;
		/// <summary>
		/// 64 <see cref="Single" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		public float[] Floats;
		/// <summary>
		/// 32 <see cref="Double" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public double[] Doubles;
		#endregion
		#region Properties
		/// <summary>
		/// Gets length of this buffer.
		/// </summary>
		public ulong Length
		{
			get { return 256; }
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
		/// Initializes new instance of type <see cref="Buffer64" /> with a portion of the array.
		/// </summary>
		/// <param name="array">Array that will provide data.</param>
		/// <param name="shift">
		/// Index of the first element of the array portion from which to start copying bytes.
		/// </param>
		public Buffer256(byte[] array, ulong shift)
			: this()
		{
			Contract.Requires((ulong)array.LongLength < shift + this.Length);
			fixed (byte* buffer = this.Bytes)
			{
				for (ulong i = 0; i < this.Length; i++)
				{
					buffer[i] = array[i + shift];
				}
			}
		}
		/// <summary>
		/// Initializes new instance of <see cref="Buffer256" /> type.
		/// </summary>
		/// <param name="pointer">Pointer to native memory cluster.</param>
		/// <param name="index">
		/// Zero-based index of the first of 256 bytes within native memory cluster.
		/// </param>
		public Buffer256(IntPtr pointer, ulong index)
			: this()
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer", "Buffer256.Constructor: Unable to initialize new object of type Buffer256: Provided pointer is null.");
			}
			this = NativeMemoryHandlingMethods.Get256Bytes(pointer, index);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets 256 bytes from native memory cluster.
		/// </summary>
		/// <param name="handle">Pointer to the beginning of native memory cluster.</param>
		/// <param name="offset">
		/// Zero-based index of first of 256 bytes within native memory cluster to get.
		/// </param>

		public void Get(IntPtr handle, ulong offset)
		{
			if (handle == IntPtr.Zero)
			{
				return;
			}
			this = NativeMemoryHandlingMethods.Get256Bytes(handle, offset);
		}
		/// <summary>
		/// Writes 256 bytes to native memory cluster.
		/// </summary>
		/// <param name="handle">Pointer to the beginning of native memory cluster.</param>
		/// <param name="offset">
		/// Zero-based index of first of 256 bytes within native memory cluster to set.
		/// </param>
		public void Set(IntPtr handle, ulong offset)
		{
			if (handle == IntPtr.Zero)
			{
				return;
			}
			NativeMemoryHandlingMethods.Set256Bytes(handle, offset, this);
		}
		#endregion
	}
}