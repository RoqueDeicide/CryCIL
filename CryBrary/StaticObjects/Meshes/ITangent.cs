using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine.StaticObjects.Meshes
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