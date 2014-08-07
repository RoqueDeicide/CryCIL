using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CryEngine.Mathematics.MemoryMapping;

namespace CryEngine.StaticObjects.Meshes
{
	/// <summary>
	/// Encapsulates data stored for vertex that is used in tangent-space normal mapping.
	/// </summary>
	/// <remarks>
	/// Tangent-space normal mapping is alternative to object-, world-space mapping, that is
	/// independent from underlying geometry.
	///
	/// This type of tangent uses <see cref="Int16" /> objects to store coordinates.
	/// </remarks>
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct TangentInt16 : ITangent
	{
		/// <summary>
		/// Bytes that comprise this object.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public fixed byte BytesArray[16];
		/// <summary>
		/// Tangent vector.
		/// </summary>
		[FieldOffset(0)]
		public Int16Vector4 Tangent;
		/// <summary>
		/// Binormal vector.
		/// </summary>
		[FieldOffset(8)]
		public Int16Vector4 Binormal;
		/// <summary>
		/// Number of bytes that are occupied by a single instance of type <see cref="TangentInt16" />.
		/// </summary>
		public static readonly int ByteCount = Marshal.SizeOf(typeof(TangentInt16));
		/// <summary>
		/// Gets array of bytes that comprise this object.
		/// </summary>
		public byte[] Bytes
		{
			get { return BufferToArray.ToBytes(this.BytesArray, ByteCount); }
		}
		/// <summary>
		/// Indicates whether this tangent space normal is equal to another.
		/// </summary>
		/// <param name="other">Another tangent.</param>
		/// <returns>
		/// True, if another normal uses <see cref="Int16" /> type for coordinates and is equal to
		/// this one.
		/// </returns>
		public bool Equals(ITangent other)
		{
			return other is TangentInt16 &&
				this.Tangent == ((TangentInt16)other).Tangent &&
				this.Binormal == ((TangentInt16)other).Binormal;
		}
	}
	/// <summary>
	/// Represents a tangent space normal with tangent and binormal represented by a single 4
	/// dimensional vector.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct QTangentInt16 : IQTangent
	{
		/// <summary>
		/// Bytes that comprise this object.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public fixed byte BytesArray[8];
		/// <summary>
		/// 4 dimensional vector that represents both tangent and binormal.
		/// </summary>
		[FieldOffset(0)]
		public Int16Vector4 TangentBinormal;
		/// <summary>
		/// Number of bytes occupied by a single instance of this structure.
		/// </summary>
		public static readonly int ByteCount = Marshal.SizeOf(typeof(QTangentInt16));
		/// <summary>
		/// Indicates whether this tangent space normal is equal to another.
		/// </summary>
		/// <param name="other">Another tangent.</param>
		/// <returns>
		/// True, if another normal uses <see cref="Int16" /> type for coordinates and is equal to
		/// this one.
		/// </returns>
		public bool Equals(IQTangent other)
		{
			return other is QTangentInt16 && this.TangentBinormal == ((QTangentInt16)other).TangentBinormal;
		}
		/// <summary>
		/// Gets bytes that comprise this object.
		/// </summary>
		public byte[] Bytes
		{
			get { return BufferToArray.ToBytes(this.BytesArray, ByteCount); }
		}
	}
	/// <summary>
	/// Defines 4 dimensional vector with coordinates represented by 16-bit integer numbers.
	/// </summary>
	/// <remarks>
	/// Used for mesh tangents only.
	/// </remarks>
	public struct Int16Vector4 : IEquatable<Int16Vector4>
	{
		public bool Equals(Int16Vector4 other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.W == other.W;
		}
		public override int GetHashCode()
		{
			unchecked
			{
				// ReSharper disable NonReadonlyFieldInGetHashCode
				int hashCode = this.X.GetHashCode();
				hashCode = (hashCode * 397) ^ this.Y.GetHashCode();
				hashCode = (hashCode * 397) ^ this.Z.GetHashCode();
				hashCode = (hashCode * 397) ^ this.W.GetHashCode();
				// ReSharper restore NonReadonlyFieldInGetHashCode
				return hashCode;
			}
		}
		public static bool operator ==(Int16Vector4 left, Int16Vector4 right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(Int16Vector4 left, Int16Vector4 right)
		{
			return !left.Equals(right);
		}
		/// <summary>
		/// X-component of the vector.
		/// </summary>
		public short X;
		/// <summary>
		/// Y-component of the vector.
		/// </summary>
		public short Y;
		/// <summary>
		/// Z-component of the vector.
		/// </summary>
		public short Z;
		/// <summary>
		/// W-component of the vector.
		/// </summary>
		public short W;
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Int16Vector4 && Equals((Int16Vector4)obj);
		}
	}
}