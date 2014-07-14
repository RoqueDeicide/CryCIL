using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
	public struct TangentInt16
	{
		public Int16Vector4 Tangent;
		public Int16Vector4 Binormal;
	}
	/// <summary></summary>
	public struct QTangentInt16 : IQTangent
	{
		public Int16Vector4 TangentBinormal;
	}
	/// <summary>
	/// Defines 4 dimensional vector with coordinates represented by 16-bit integer numbers.
	/// </summary>
	/// <remarks>
	/// Used for mesh tangents only.
	/// </remarks>
	public struct Int16Vector4
	{
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
	}
}