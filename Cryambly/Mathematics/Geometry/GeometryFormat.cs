using System;
using System.Linq;

namespace CryCil.Geometry
{
	/// <summary>
	/// Enumeration of ways the geometry can be formed.
	/// </summary>
	public enum GeometryFormat
	{
		/// <summary>
		/// Geometry is formed from vertices.
		/// </summary>
		Vertices,
		/// <summary>
		/// Geometry is formed from edges.
		/// </summary>
		Edges,
		/// <summary>
		/// Geometry is formed from surfaces.
		/// </summary>
		Surface,
		/// <summary>
		/// Geometry is formed from volumes.
		/// </summary>
		Volume
	}
}