using System;

namespace CryEngine.Mathematics.Geometry.Meshes
{
	/// <summary>
	/// Interface that can represent on of the possible tangent formats.
	/// </summary>
	public interface ITangent : IEquatable<ITangent>
	{
		/// <summary>
		/// Gets the array of bytes that define this object.
		/// </summary>
		byte[] Bytes { get; }
	}
}