using System;
using System.Runtime.InteropServices;

namespace CryEngine.Mathematics.Geometry.Meshes
{
	/// <summary>
	/// Encapsulates data that describe a mesh face.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct IndexedTriangleFace : IEquatable<IndexedTriangleFace>
	{
		/// <summary>
		/// Array of indices of vertices that form this face.
		/// </summary>
		[FieldOffset(0)]
		public Int32Vector3 Indices;
		/// <summary>
		/// Index of mesh subset this face is assigned to.
		/// </summary>
		[FieldOffset(12)]
		public byte SubsetIndex;
		/// <summary>
		/// Array of bytes that grants access to each byte of this object.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 13)]
		public fixed byte Bytes[13];
		/// <summary>
		/// Number of bytes.
		/// </summary>
		public static int ByteCount
		{
			get { return 13; }
		}
		/// <summary>
		/// Transfers instance of <see cref="IndexedTriangleFace" /> from native memory.
		/// </summary>
		/// <param name="pointer"> Pointer to native memory block. </param>
		/// <param name="offset">  Byte offset that is added to <paramref name="pointer" />. </param>
		/// <returns>
		/// New instance of structure <see cref="IndexedTriangleFace" /> that is interpreted from 13
		/// bytes from native memory.
		/// </returns>
		public static IndexedTriangleFace FromNativeMemory(IntPtr pointer, int offset)
		{
			return (IndexedTriangleFace)Marshal.PtrToStructure(pointer + offset, typeof(IndexedTriangleFace));
		}
		/// <summary>
		/// Writes this instance to native memory.
		/// </summary>
		/// <param name="pointer"> Pointer to native memory block. </param>
		/// <param name="offset">  Byte offset that is added to <paramref name="pointer" />. </param>
		public void WriteToNativeMemory(IntPtr pointer, int offset)
		{
			Marshal.StructureToPtr(this, pointer + offset, false);
		}
		/// <summary>
		/// Determines whether two instances of type <see cref="IndexedTriangleFace" /> are equal.
		/// </summary>
		/// <param name="left">  Left operand. </param>
		/// <param name="right"> Right operand. </param>
		/// <returns> True, if operands are equal. </returns>
		public static bool operator ==(IndexedTriangleFace left, IndexedTriangleFace right)
		{
			return
				left.Indices.X == right.Indices.X &&
				left.Indices.Y == right.Indices.Y &&
				left.Indices.Z == right.Indices.Z &&
				left.SubsetIndex == right.SubsetIndex;
		}
		/// <summary>
		/// Determines whether two instances of type <see cref="IndexedTriangleFace" /> are not equal.
		/// </summary>
		/// <param name="left">  Left operand. </param>
		/// <param name="right"> Right operand. </param>
		/// <returns> True, if operands are not equal. </returns>
		public static bool operator !=(IndexedTriangleFace left, IndexedTriangleFace right)
		{
			return !(left == right);
		}
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="other"> Another object. </param>
		/// <returns>
		/// True, if another object is of this type and can be considered equal to this one.
		/// </returns>
		public bool Equals(IndexedTriangleFace other)
		{
			return this == other;
		}
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="obj"> Another object. </param>
		/// <returns>
		/// True, if another object is of this type and can be considered equal to this one.
		/// </returns>
		public override bool Equals(object obj)
		{
			return !ReferenceEquals(null, obj) && obj is IndexedTriangleFace && this.Equals((IndexedTriangleFace)obj);
		}
		/// <summary>
		/// Gets hash code of this object.
		/// </summary>
		/// <returns> Hash code of this object. </returns>
		public override int GetHashCode()
		{
			// ReSharper disable NonReadonlyFieldInGetHashCode
			int hash = -1047578147;
			hash = (hash * -1521134295) + this.Indices[0];
			hash = (hash * -1521134295) + this.Indices[1];
			hash = (hash * -1521134295) + this.Indices[2];
			hash = (hash * -1521134295) + this.SubsetIndex;
			return hash;
			// ReSharper restore NonReadonlyFieldInGetHashCode
		}
	}
}