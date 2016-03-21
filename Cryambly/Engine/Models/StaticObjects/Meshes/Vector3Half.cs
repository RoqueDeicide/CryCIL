using System;
using System.Linq;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// A 3D vector which coordinates are represented by half-precision floating point numbers.
	/// </summary>
	public struct Vector3Half
	{
		/// <summary>
		/// X-coordinate.
		/// </summary>
		public Half X;
		/// <summary>
		/// Y-coordinate.
		/// </summary>
		public Half Y;
		/// <summary>
		/// Z-coordinate.
		/// </summary>
		public Half Z;
	}
}