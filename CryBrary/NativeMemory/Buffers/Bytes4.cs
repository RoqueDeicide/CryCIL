﻿using System;
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
	/// Encapsulates 4 bytes worth of data.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	public struct Bytes4 : IBuffer
	{
		#region Fields
		/// <summary>
		/// <see cref="UInt32" /> value.
		/// </summary>
		[FieldOffset(0)]
		public uint UnsignedInt;
		/// <summary>
		/// <see cref="Int32" /> value.
		/// </summary>
		[FieldOffset(0)]
		public int SignedInt;
		/// <summary>
		/// <see cref="Single" /> value.
		/// </summary>
		[FieldOffset(0)]
		public float SingleFloat;
		#endregion
		#region Properties
		/// <summary>
		/// Returns 4.
		/// </summary>
		public ulong Length
		{
			get { return 4; }
		}
		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public byte this[ulong index]
		{
			get
			{
				int shift = (3 - (int)index) * 8;
				return (byte)(255 << (shift & this.SignedInt) >> shift);
			}
			set
			{
				int shift = (3 - (int)index) * 8;
				int mask = ~(255 << shift);
				this.SignedInt &= mask;					// Zero a byte.
				this.SignedInt |= value << shift;		// Give byte a new value.
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes new instance of <see cref="Bytes4" /> type.
		/// </summary>
		/// <param name="value"><see cref="Int32" /> value to initialize this object with.</param>
		public Bytes4(int value)
			: this()
		{
			this.SignedInt = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes4" /> type.
		/// </summary>
		/// <param name="value"><see cref="UInt32" /> value to initialize this object with.</param>
		public Bytes4(uint value)
			: this()
		{
			this.UnsignedInt = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes4" /> type.
		/// </summary>
		/// <param name="value"><see cref="Single" /> value to initialize this object with.</param>
		public Bytes4(float value)
			: this()
		{
			this.SingleFloat = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes4" /> type.
		/// </summary>
		/// <param name="array">Array to use to initialize new object.</param>
		/// <param name="startIndex">
		/// Index of the first element of the array portion from which to start copying bytes.
		/// </param>
		/// <param name="count">Number of elements to copy from array.</param>
		public Bytes4(byte[] array, ulong startIndex, ulong count)
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
			byte[] bytes = new byte[4];
			for (ulong i = 0, j = startIndex; i < count; i++, j++)
			{
				bytes[i] = array[j];
			}
			this.SignedInt = BitConverter.ToInt32(bytes, 0);
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes4" /> type.
		/// </summary>
		/// <param name="pointer">Pointer to native memory cluster.</param>
		/// <param name="index">
		/// Zero-based index of the first of 4 bytes within native memory cluster.
		/// </param>
		public Bytes4(IntPtr pointer, ulong index)
			: this()
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer", "Bytes4.Constructor: Unable to initialize new object of type Bytes4: Provided pointer is null.");
			}
			this = NativeMemoryHandlingMethods.Get4Bytes(pointer, index);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets 4 bytes from native memory cluster.
		/// </summary>
		/// <param name="handle">Pointer to the beginning of native memory cluster.</param>
		/// <param name="offset">
		/// Zero-based index of first of 4 bytes within native memory cluster to get.
		/// </param>

		public void Get(IntPtr handle, ulong offset)
		{
			if (handle == IntPtr.Zero)
			{
				return;
			}
			this = NativeMemoryHandlingMethods.Get4Bytes(handle, offset);
		}
		/// <summary>
		/// Writes 4 bytes to native memory cluster.
		/// </summary>
		/// <param name="handle">Pointer to the beginning of native memory cluster.</param>
		/// <param name="offset">
		/// Zero-based index of first of 4 bytes within native memory cluster to set.
		/// </param>
		public void Set(IntPtr handle, ulong offset)
		{
			if (handle == IntPtr.Zero)
			{
				return;
			}
			NativeMemoryHandlingMethods.Set4Bytes(handle, offset, this.UnsignedInt);
		}
		#endregion
	}
}