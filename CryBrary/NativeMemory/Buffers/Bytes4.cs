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
	/// Encapsulates 4 bytes worth of data.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	public unsafe struct Bytes4 : IBuffer
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
		/// <summary>
		/// Individual 4 bytes.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] Bytes;
		#endregion
		#region Properties
		/// <summary>
		/// Returns 4.
		/// </summary>
		public ulong Length
		{
			get { return 4; }
		}
		/// <summary>
		/// Gets or sets a byte.
		/// </summary>
		/// <param name="index">
		/// Zero-based index of the byte to get or set.
		/// </param>
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
		/// Initializes new instance of <see cref="Bytes4" /> type.
		/// </summary>
		/// <param name="value">
		/// <see cref="Int32" /> value to initialize this object with.
		/// </param>
		public Bytes4(int value)
			: this()
		{
			this.SignedInt = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes4" /> type.
		/// </summary>
		/// <param name="value">
		/// <see cref="UInt32" /> value to initialize this object with.
		/// </param>
		public Bytes4(uint value)
			: this()
		{
			this.UnsignedInt = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes4" /> type.
		/// </summary>
		/// <param name="value">
		/// <see cref="Single" /> value to initialize this object with.
		/// </param>
		public Bytes4(float value)
			: this()
		{
			this.SingleFloat = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes4" /> type.
		/// </summary>
		/// <param name="array">
		/// Array to use to initialize new object.
		/// </param>
		/// <param name="shift">
		/// Zero-based index of the first of 4 bytes to use for initialization.
		/// </param>
		public Bytes4(byte[] array, ulong shift)
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
		/// Initializes new instance of <see cref="Bytes4" /> type.
		/// </summary>
		/// <param name="bytes">
		/// Pointer to stack array or fixed-sized buffer.
		/// </param>
		/// <param name="shift">
		/// Zero-based index of the first of 4 bytes to use for initialization.
		/// </param>
		public Bytes4(byte* bytes, ulong shift)
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
		/// Initializes new instance of <see cref="Bytes4" /> type.
		/// </summary>
		/// <param name="pointer">
		/// Pointer to native memory cluster.
		/// </param>
		/// <param name="index">
		/// Zero-based index of the first of 4 bytes within native
		/// memory cluster.
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
		/// <param name="handle">
		/// Pointer to the beginning of native memory cluster.
		/// </param>
		/// <param name="offset">
		/// Zero-based index of first of 4 bytes within native memory
		/// cluster to get.
		/// </param>

		public void Get(IntPtr handle, ulong offset)
		{
			if (handle == null)
			{
				return;
			}
			this = NativeMemoryHandlingMethods.Get4Bytes(handle, offset);
		}
		/// <summary>
		/// Writes 4 bytes to native memory cluster.
		/// </summary>
		/// <param name="handle">
		/// Pointer to the beginning of native memory cluster.
		/// </param>
		/// <param name="offset">
		/// Zero-based index of first of 4 bytes within native memory
		/// cluster to set.
		/// </param>
		public void Set(IntPtr handle, ulong offset)
		{
			if (handle == null)
			{
				return;
			}
			NativeMemoryHandlingMethods.Set4Bytes(handle, offset, this.UnsignedInt);
		}
		#endregion
	}
}