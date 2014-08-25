using System;

namespace CryEngine.Mathematics.Geometry.Meshes
{
	/// <summary>
	/// Defines common functionality of objects that represent tangent-space normals.
	/// </summary>
	public interface IQTangent : IEquatable<IQTangent>
	{
		byte[] Bytes { get; }
	}
}