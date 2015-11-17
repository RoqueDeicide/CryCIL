using System;
using System.Runtime.InteropServices;

namespace CryCil.MemoryMapping
{
	/// <summary>
	/// 2-byte structure that can be interpreted as number of types.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 2)]
	public unsafe struct Bytes2 : IBuffer
	{
		#region Fields
		/// <summary>
		/// <see cref="ushort"/> value.
		/// </summary>
		[FieldOffset(0)] public ushort UnsignedShort;
		/// <summary>
		/// <see cref="short"/> value.
		/// </summary>
		[FieldOffset(0)] public short SignedShort;
		/// <summary>
		/// <see cref="Half"/> value.
		/// </summary>
		[FieldOffset(0)] public Half HalfFloat;
		/// <summary>
		/// <see cref="Char"/> value.
		/// </summary>
		[FieldOffset(0)] public char Character;
		#endregion
		#region Properties
		/// <summary>
		/// Returns 2.
		/// </summary>
		public int Length
		{
			get { return 2; }
		}
		/// <summary>
		/// Gets or sets a byte.
		/// </summary>
		/// <param name="index">Zero-based index of the byte to get or set.</param>
		public byte this[int index]
		{
			get
			{
				ushort us = this.UnsignedShort;
				byte* bytes = (byte*)&us;
				return bytes[index];
			}
			set
			{
				ushort us = this.UnsignedShort;
				byte* bytes = (byte*)&us;
				bytes[index] = value;
				this.UnsignedShort = us;
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes new instance of <see cref="Bytes2"/> type.
		/// </summary>
		/// <param name="value"><see cref="Int16"/> value to initialize this object with.</param>
		public Bytes2(short value)
			: this()
		{
			this.SignedShort = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes2"/> type.
		/// </summary>
		/// <param name="value"><see cref="UInt16"/> value to initialize this object with.</param>
		public Bytes2(ushort value)
			: this()
		{
			this.UnsignedShort = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes2"/> type.
		/// </summary>
		/// <param name="value"><see cref="Half"/> value to initialize this object with.</param>
		public Bytes2(Half value)
			: this()
		{
			this.HalfFloat = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes2"/> type.
		/// </summary>
		/// <param name="value"><see cref="Char"/> value to initialize this object with.</param>
		public Bytes2(char value)
			: this()
		{
			this.Character = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes2"/> type.
		/// </summary>
		/// <param name="pointer">Pointer to native memory cluster.</param>
		/// <param name="index">  
		/// Zero-based index of the first of 2 bytes within native memory cluster.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Unable to initialize new object of type Bytes4: Provided pointer is null.
		/// </exception>
		/// <exception cref="AccessViolationException">
		/// Base address ( <paramref name="pointer"/>) plus offset byte ( <paramref name="index"/>)
		/// produces a null or invalid address.
		/// </exception>
		public Bytes2(IntPtr pointer, int index)
			: this()
		{
			if (pointer == IntPtr.Zero)
			{
				throw new ArgumentNullException("pointer",
												"Bytes2.Constructor: Unable to initialize new object of type Bytes2: Provided pointer is null.");
			}
			this.SignedShort = Marshal.ReadInt16(pointer, index);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets 2 bytes from native memory cluster.
		/// </summary>
		/// <param name="handle">Pointer to the beginning of native memory cluster.</param>
		/// <param name="offset">
		/// Zero-based index of first of 2 bytes within native memory cluster to get.
		/// </param>
		/// <exception cref="AccessViolationException">
		/// Base address ( <paramref name="handle"/>) plus offset byte ( <paramref name="offset"/>)
		/// produces a null or invalid address.
		/// </exception>
		public void Get(IntPtr handle, int offset)
		{
			if (handle == IntPtr.Zero)
			{
				return;
			}
			this.SignedShort = Marshal.ReadInt16(handle, offset);
		}
		/// <summary>
		/// Writes 2 bytes to native memory cluster.
		/// </summary>
		/// <param name="handle">Pointer to the beginning of native memory cluster.</param>
		/// <param name="offset">
		/// Zero-based index of first of 2 bytes within native memory cluster to set.
		/// </param>
		/// <exception cref="AccessViolationException">
		/// Base address ( <paramref name="handle"/>) plus offset byte ( <paramref name="offset"/>)
		/// produces a null or invalid address.
		/// </exception>
		public void Set(IntPtr handle, int offset)
		{
			if (handle == IntPtr.Zero)
			{
				return;
			}
			Marshal.WriteInt16(handle, offset, this.SignedShort);
		}
		#endregion
	}
}