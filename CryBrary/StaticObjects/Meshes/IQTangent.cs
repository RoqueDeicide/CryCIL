using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine.StaticObjects.Meshes
{
	/// <summary>
	/// Defines common functionality of objects that represent tangent-space normals.
	/// </summary>
	public interface IQTangent : IEquatable<IQTangent>
	{
		byte[] Bytes { get; }
	}
}