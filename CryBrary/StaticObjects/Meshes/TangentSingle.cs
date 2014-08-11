using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CryEngine.Mathematics;
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
	/// This type of tangent uses <see cref="Single" /> objects to store coordinates.
	/// </remarks>
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct TangentSingle : ITangent
	{
		/// <summary>
		/// Bytes that comprise this object.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public fixed byte BytesArray[32];
		/// <summary>
		/// Tangent vector.
		/// </summary>
		[FieldOffset(0)]
		public Vector4 Tangent;
		/// <summary>
		/// Binormal vector.
		/// </summary>
		[FieldOffset(16)]
		public Vector4 Binormal;
		/// <summary>
		/// Number of bytes that are occupied by a single instance of type <see cref="TangentSingle" />.
		/// </summary>
		public static readonly int ByteCount = Marshal.SizeOf(typeof(TangentSingle));
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
		/// <param name="other"> Another tangent. </param>
		/// <returns>
		/// True, if another normal uses <see cref="Single" /> type for coordinates and is equal to
		/// this one.
		/// </returns>
		public bool Equals(ITangent other)
		{
			return other is TangentSingle &&
				this.Tangent == ((TangentSingle)other).Tangent &&
				this.Binormal == ((TangentSingle)other).Binormal;
		}
	}
	/// <summary>
	/// Represents a tangent space normal with tangent and binormal represented by a single 4
	/// dimensional vector.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct QTangentSingle : IQTangent
	{
		/// <summary>
		/// Bytes that comprise this object.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public fixed byte BytesArray[16];
		/// <summary>
		/// 4 dimensional vector that represents both tangent and binormal.
		/// </summary>
		[FieldOffset(0)]
		public Vector4 TangentBinormal;
		/// <summary>
		/// Number of bytes occupied by a single instance of this structure.
		/// </summary>
		public static readonly int ByteCount = Marshal.SizeOf(typeof(QTangentSingle));
		/// <summary>
		/// Indicates whether this tangent space normal is equal to another.
		/// </summary>
		/// <param name="other"> Another tangent. </param>
		/// <returns>
		/// True, if another normal uses <see cref="Single" /> type for coordinates and is equal to
		/// this one.
		/// </returns>
		public bool Equals(IQTangent other)
		{
			return other is QTangentSingle &&
				   this.TangentBinormal == ((QTangentSingle)other).TangentBinormal;
		}
		/// <summary>
		/// Gets bytes that comprise this object.
		/// </summary>
		public byte[] Bytes
		{
			get { return BufferToArray.ToBytes(this.BytesArray, ByteCount); }
		}
	}
}