using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace CryCil.MemoryMapping
{
	/// <summary>
	/// Encapsulates 8 bytes worth of data.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	public struct Bytes8 : IBuffer
	{
		#region Fields
		/// <summary>
		/// <see cref="ulong"/> value.
		/// </summary>
		[FieldOffset(0)] public ulong UnsignedLong;
		/// <summary>
		/// <see cref="long"/> value.
		/// </summary>
		[FieldOffset(0)] public long SignedLong;
		/// <summary>
		/// <see cref="double"/> value.
		/// </summary>
		[FieldOffset(0)] public double DoubleFloat;
		/// <summary>
		/// First 4-byte number.
		/// </summary>
		[FieldOffset(0)] public float FirstSingleFloat;
		/// <summary>
		/// Second 4-byte number.
		/// </summary>
		[FieldOffset(4)] public float SecondSingleFloat;
		#endregion
		#region Properties
		/// <summary>
		/// Returns 8.
		/// </summary>
		public int Length => 8;
		/// <summary>
		/// Gets or sets a byte.
		/// </summary>
		/// <param name="index">Zero-based index of the byte to get or set.</param>
		public byte this[int index]
		{
			get { return (byte)((255ul << (index * 8)) & this.UnsignedLong); }
			set
			{
				// Clear the byte.
				this.UnsignedLong &= ~(255ul << (index * 8));
				// Set the byte.
				this.UnsignedLong |= (ulong)value << (index * 8);
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes new instance of <see cref="Bytes8"/> type.
		/// </summary>
		/// <param name="value"><see cref="long"/> value to initialize this object with.</param>
		public Bytes8(long value)
			: this()
		{
			this.SignedLong = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes8"/> type.
		/// </summary>
		/// <param name="value"><see cref="ulong"/> value to initialize this object with.</param>
		public Bytes8(ulong value)
			: this()
		{
			this.UnsignedLong = value;
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes8"/> type.
		/// </summary>
		/// <param name="value"><see cref="double"/> value to initialize this object with.</param>
		public Bytes8(double value)
			: this()
		{
			this.DoubleFloat = value;
		}
		/// <summary>
		/// Initializes new 8-byte buffer object.
		/// </summary>
		/// <param name="array">     Array that provides elements to fill new buffer with.</param>
		/// <param name="startIndex">
		/// Index of the first element of the array portion from which to start copying bytes.
		/// </param>
		/// <param name="count">     Number of elements to copy from array.</param>
		/// <exception cref="ArgumentNullException">
		/// Array which elements are supposed to be transferred to buffer is null or empty.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Too many elements are requested to be transferred to buffer.
		/// </exception>
		/// <exception cref="ArgumentException">This buffer is too small.</exception>
		public Bytes8(byte[] array, int startIndex, int count)
			: this()
		{
			if (array == null || array.Length == 0)
			{
				throw new ArgumentNullException(nameof(array),
												"Array which elements are supposed to be transferred to buffer is null or empty.");
			}
			if (array.LongLength - startIndex < count)
			{
				throw new ArgumentOutOfRangeException(nameof(count),
													  "Too many elements are requested to be transferred to buffer.");
			}
			if (this.Length < count)
			{
				throw new ArgumentException("This buffer is too small.");
			}
			for (int i = 0, j = startIndex; i < count; i++, j++)
			{
				this[i] = array[j];
			}
		}
		/// <summary>
		/// Initializes new instance of <see cref="Bytes8"/> type.
		/// </summary>
		/// <param name="pointer">Pointer to native memory cluster.</param>
		/// <param name="index">  
		/// Zero-based index of the first of 8 bytes within native memory cluster.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Unable to initialize new object of type Bytes8: Provided pointer is null.
		/// </exception>
		/// <exception cref="AccessViolationException">
		/// Base address ( <paramref name="pointer"/>) plus offset byte ( <paramref name="index"/>) produces
		/// a null or invalid address.
		/// </exception>
		public Bytes8(IntPtr pointer, int index)
			: this()
		{
			if (pointer == null)
			{
				throw new ArgumentNullException(nameof(pointer),
												"Unable to initialize new object of type Bytes8: Provided pointer is null.");
			}
			this.SignedLong = Marshal.ReadInt64(pointer, index);
		}
		/// <summary>
		/// Initializes this buffer from a range of <see cref="Single"/> values.
		/// </summary>
		/// <param name="values">Array of arguments of type <see cref="Single"/>.</param>
		public Bytes8(params float[] values)
			: this()
		{
			if (values.Length > 0)
			{
				this.FirstSingleFloat = values[0];
			}
			if (values.Length > 1)
			{
				this.SecondSingleFloat = values[1];
			}
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
		/// <exception cref="AccessViolationException">
		/// Base address ( <paramref name="handle"/>) plus offset byte ( <paramref name="offset"/>) produces
		/// a null or invalid address.
		/// </exception>
		public void Get(IntPtr handle, int offset)
		{
			this.SignedLong = Marshal.ReadInt64(handle, offset);
		}
		/// <summary>
		/// Writes 8 bytes to native memory cluster.
		/// </summary>
		/// <param name="handle">Pointer to the beginning of native memory cluster.</param>
		/// <param name="offset">
		/// Zero-based index of first of 8 bytes within native memory cluster to set.
		/// </param>
		/// <exception cref="AccessViolationException">
		/// Base address ( <paramref name="handle"/>) plus offset byte ( <paramref name="offset"/>) produces
		/// a null or invalid address.
		/// </exception>
		public void Set(IntPtr handle, int offset)
		{
			Marshal.WriteInt64(handle, offset, this.SignedLong);
		}
		#endregion
	}
}