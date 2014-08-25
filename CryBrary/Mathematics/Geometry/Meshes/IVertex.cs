using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine.Mathematics.Geometry.Meshes
{
	/// <summary>
	/// Defines common functionality of objects that represent vertices of the polygon.
	/// </summary>
	public interface IVertex
	{
		/// <summary>
		/// When implemented, creates a deep copy of this vertex.
		/// </summary>
		IVertex Clone { get; }
		/// <summary>
		/// When implemented, creates a vertex that is located on the line between this one and
		/// another one.
		/// </summary>
		/// <remarks>See http://en.wikipedia.org/wiki/Linear_interpolation for details.</remarks>
		/// <param name="anotherVertex">Another vertex.</param>
		/// <param name="position">     Relative position of the new vertex between 2 vertices.</param>
		/// <returns>A new vertex that is interpolated between two vertices.</returns>
		IVertex CreateLinearInterpolation(IVertex anotherVertex, float position);
		/// <summary>
		/// When implemented, creates a vertex that is spherically interpolated between this vertex
		/// and another one.
		/// </summary>
		/// <remarks>See http://en.wikipedia.org/wiki/Slerp for details.</remarks>
		/// <param name="anotherVertex">Another vertex.</param>
		/// <param name="position">     Interpolation value.</param>
		/// <returns>A new vertex that is interpolated between two vertices.</returns>
		IVertex CreateSphericalInterpolation(IVertex anotherVertex, float position);
		/// <summary>
		/// When implemented, flips any properties of the vertex that have a direction.
		/// </summary>
		void Flip();
		/// <summary>
		/// When implemented, converts all vector properties of this vertex to unit vectors.
		/// </summary>
		/// <remarks>
		/// Can be used to create normalized linear interpolation, which is a simple surrogate of
		/// spherical linear interpolation.
		/// </remarks>
		void Normalize();
	}
}