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
	/// Encapsulates 32 bytes worth of data.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 32)]
	public struct Buffer32 : IBuffer
	{
		#region Fields
		/// <summary>
		/// 32 <see cref="Byte" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public byte[] Bytes;
		/// <summary>
		/// 32 <see cref="SByte" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public sbyte[] Sbytes;
		/// <summary>
		/// 16 <see cref="Int16" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public short[] Shorts;
		/// <summary>
		/// 16 <see cref="UInt16" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public ushort[] Ushorts;
		/// <summary>
		/// 8 <see cref="Int32" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public int[] Ints;
		/// <summary>
		/// 8 <see cref="UInt32" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public uint[] Uints;
		/// <summary>
		/// 4 <see cref="Int64" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public long[] Longs;
		/// <summary>
		/// 4 <see cref="UInt64" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public ulong[] Ulongs;
		/// <summary>
		/// 8 <see cref="Single" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public float[] Floats;
		/// <summary>
		/// 4 <see cref="Double" /> objects.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public double[] Doubles;
		#endregion
		#region Properties
		/// <summary>
		/// Gets length of this buffer.
		/// </summary>
		public ulong Length
		{
			get { return 32; }
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
		/// <param name="count">Number of elements to copy from array.</param>
		public Buffer32(byte[] array, ulong startIndex, ulong count)
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
		/// Initializes new instance of <see cref="Buffer32" /> type.
		/// </summary>
		/// <param name="pointer">Pointer to native memory cluster.</param>
		/// <param name="index">
		/// Zero-based index of the first of 32 bytes within native memory cluster.
		/// </param>
		public Buffer32(IntPtr pointer, ulong index)
			: this()
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer", "Buffer32.Constructor: Unable to initialize new object of type Buffer32: Provided pointer is null.");
			}
			this = NativeMemoryHandlingMethods.Get32Bytes(pointer, index);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets 32 bytes from native memory cluster.
		/// </summary>
		/// <param name="handle">Pointer to the beginning of native memory cluster.</param>
		/// <param name="offset">
		/// Zero-based index of first of 32 bytes within native memory cluster to get.
		/// </param>

		public void Get(IntPtr handle, ulong offset)
		{
			if (handle == IntPtr.Zero)
			{
				return;
			}
			this = NativeMemoryHandlingMethods.Get32Bytes(handle, offset);
		}
		/// <summary>
		/// Writes 32 bytes to native memory cluster.
		/// </summary>
		/// <param name="handle">Pointer to the beginning of native memory cluster.</param>
		/// <param name="offset">
		/// Zero-based index of first of 32 bytes within native memory cluster to set.
		/// </param>
		public void Set(IntPtr handle, ulong offset)
		{
			if (handle == IntPtr.Zero)
			{
				return;
			}
			NativeMemoryHandlingMethods.Set32Bytes(handle, offset, this);
		}
		#endregion
	}
}