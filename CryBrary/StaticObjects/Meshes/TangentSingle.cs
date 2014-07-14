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
	/// This type of tangent uses <see cref="Single" /> objects to store coordinates.
	/// </remarks>
	public struct TangentSingle : ITangent
	{
		public Vector4 Tangent;
		public Vector4 Binormal;
	}
	/// <summary></summary>
	public struct QTangentSingle : IQTangent
	{
		public Vector4 TangentBinormal;
	}
}