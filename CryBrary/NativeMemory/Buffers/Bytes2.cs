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
	/// 2-byte structure that can be interpreted as number of types.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 2)]
	public struct Bytes2 : IBuffer
	{
		#region Fields
		/// <summary>
		/// <see cref="UInt16" /> value.
		/// </summary>
		[FieldOffset(0)]
		public ushort UnsignedShort;
		/// <summary>
		/// <see cref="Int16" /> value.
		/// </summary>
		[FieldOffset(0)]
		public short SignedShort;
		/// <summary>
		/// <see cref="Half" /> value.
		/// </summary>
		[FieldOffset(0)]
		public Half HalfFloat;
		/// <summary>
		/// <see cref="Char" /> value.
		/// </summary>
		[FieldOffset(0)]
		public char Character;
		/// <summary>
		/// Individual 2 bytes.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] Bytes;
		#endregion
		#region Properties
		/// <summary>
		/// Returns 2.
		/// </summary>
		public ulong Length
		{
			get { return 2; }
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
		/// Initializes new instance of <see cref="Bytes2" /> type.
		/// </summary>
		/// <param name="value">
		/// <see cref="Int16" /> value to initialize this object with.
		/// </param>
		public Bytes2(short value)
			: this()
		{
			this.SignedShort = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes2" /> type.
		/// </summary>
		/// <param name="value">
		/// <see cref="UInt16" /> value to initialize this object with.
		/// </param>
		public Bytes2(ushort value)
			: this()
		{
			this.UnsignedShort = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes2" /> type.
		/// </summary>
		/// <param name="value">
		/// <see cref="Half" /> value to initialize this object with.
		/// </param>
		public Bytes2(Half value)
			: this()
		{
			this.HalfFloat = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes2" /> type.
		/// </summary>
		/// <param name="value">
		/// <see cref="Char" /> value to initialize this object with.
		/// </param>
		public Bytes2(char value)
			: this()
		{
			this.Character = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes2" /> type.
		/// </summary>
		/// <param name="pointer">
		/// Pointer to native memory cluster.
		/// </param>
		/// <param name="index">
		/// Zero-based index of the first of 2 bytes within native
		/// memory cluster.
		/// </param>
		public Bytes2(IntPtr pointer, ulong index)
			: this()
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer", "Bytes2.Constructor: Unable to initialize new object of type Bytes2: Provided pointer is null.");
			}
			this = NativeMemoryHandlingMethods.Get2Bytes(pointer, index);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets 2 bytes from native memory cluster.
		/// </summary>
		/// <param name="handle">
		/// Pointer to the beginning of native memory cluster.
		/// </param>
		/// <param name="offset">
		/// Zero-based index of first of 2 bytes within native memory
		/// cluster to get.
		/// </param>
		public void Get(IntPtr handle, ulong offset)
		{
			if (handle == null)
			{
				return;
			}
			this = NativeMemoryHandlingMethods.Get2Bytes(handle, offset);
		}
		/// <summary>
		/// Writes 2 bytes to native memory cluster.
		/// </summary>
		/// <param name="handle">
		/// Pointer to the beginning of native memory cluster.
		/// </param>
		/// <param name="offset">
		/// Zero-based index of first of 2 bytes within native memory
		/// cluster to set.
		/// </param>
		public void Set(IntPtr handle, ulong offset)
		{
			if (handle == null)
			{
				return;
			}
			NativeMemoryHandlingMethods.Set2Bytes(handle, offset, this.UnsignedShort);
		}
		#endregion
	}
}