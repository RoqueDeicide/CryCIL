using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace CryCil.Geometry
{
	/// <summary>
	/// Encapsulates information that describes a very simple sphere.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Sphere
	{
		/// <summary>
		/// Coordinates of the center of this sphere.
		/// </summary>
		public Vector3 Center;
		/// <summary>
		/// Radius of this sphere.
		/// </summary>
		public float Radius;
	}
}