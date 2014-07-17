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
	/// Encapsulates 64 bytes worth of data.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 64)]
	public struct Buffer64 : IBuffer
	{
		#region Fields
		/// <summary>
		/// 64 <see cref="Byte" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		public byte[] Bytes;
		/// <summary>
		/// 64 <see cref="SByte" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		public sbyte[] Sbytes;
		/// <summary>
		/// 32 <see cref="Int16" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public short[] Shorts;
		/// <summary>
		/// 32 <see cref="UInt16" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public ushort[] Ushorts;
		/// <summary>
		/// 16 <see cref="Int32" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public int[] Ints;
		/// <summary>
		/// 16 <see cref="UInt32" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public uint[] Uints;
		/// <summary>
		/// 8 <see cref="Int64" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public long[] Longs;
		/// <summary>
		/// 8 <see cref="UInt64" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public ulong[] Ulongs;
		/// <summary>
		/// 16 <see cref="Single" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public float[] Floats;
		/// <summary>
		/// 8 <see cref="Double" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public double[] Doubles;
		#endregion
		#region Properties
		/// <summary>
		/// Gets length of this buffer.
		/// </summary>
		public ulong Length
		{
			get { return 64; }
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
		/// <param name="startIndex">
		/// Index of the first element of the array portion from which to start copying bytes.
		/// </param>
		public Buffer64(byte[] array, ulong startIndex, ulong count)
			: this()
		{
			if (array == null || array.Length == 0)
			{
				throw new ArgumentNullException
					("array", "Array which elements are supposed to be transferred to buffer is null or empty.");
			}
			if ((ulong)array.LongLength - startIndex < count)
			{
				throw new ArgumentOutOfRangeException
					("count", "Too many elements are requested to be transferred to buffer.");
			}
			if (this.Length < count)
			{
				throw new ArgumentException("This buffer is too small.");
			}
			for (ulong i = 0, j = startIndex; i < count; i++, j++)
			{
				this.Bytes[i] = array[j];
			}
		}
		/// <summary>
		/// Initializes new instance of <see cref="Buffer64" /> type.
		/// </summary>
		/// <param name="pointer">Pointer to native memory cluster.</param>
		/// <param name="index">
		/// Zero-based index of the first of 64 bytes within native memory cluster.
		/// </param>
		public Buffer64(IntPtr pointer, ulong index)
			: this()
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer", "Buffer64.Constructor: Unable to initialize new object of type Buffer64: Provided pointer is null.");
			}
			this = NativeMemoryHandlingMethods.Get64Bytes(pointer, index);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets 64 bytes from native memory cluster.
		/// </summary>
		/// <param name="handle">Pointer to the beginning of native memory cluster.</param>
		/// <param name="offset">
		/// Zero-based index of first of 64 bytes within native memory cluster to get.
		/// </param>

		public void Get(IntPtr handle, ulong offset)
		{
			if (handle == IntPtr.Zero)
			{
				return;
			}
			this = NativeMemoryHandlingMethods.Get64Bytes(handle, offset);
		}
		/// <summary>
		/// Writes 64 bytes to native memory cluster.
		/// </summary>
		/// <param name="handle">Pointer to the beginning of native memory cluster.</param>
		/// <param name="offset">
		/// Zero-based index of first of 64 bytes within native memory cluster to set.
		/// </param>
		public void Set(IntPtr handle, ulong offset)
		{
			if (handle == IntPtr.Zero)
			{
				return;
			}
			NativeMemoryHandlingMethods.Set64Bytes(handle, offset, this);
		}
		#endregion
	}
}