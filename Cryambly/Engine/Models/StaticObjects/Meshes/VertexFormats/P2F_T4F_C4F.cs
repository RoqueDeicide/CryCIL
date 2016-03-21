using System;
using System.Linq;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a vertex that is defined by <see cref="VertexFormat.P2F_T4F_C4F"/>.
	/// </summary>
	public struct P2F_T4F_C4F
	{
		/// <summary>
		/// Coordinates of the vertex.
		/// </summary>
		public Vector2 Position;
		/// <summary>
		/// Coordinates of the vertex on UV map.
		/// </summary>
		public Vector4 TexturePosition;
		/// <summary>
		/// Color of the vertex.
		/// </summary>
		public Vector4 Color;
	}
}