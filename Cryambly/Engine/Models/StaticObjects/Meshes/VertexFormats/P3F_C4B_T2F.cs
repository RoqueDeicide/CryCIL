using System;
using System.Linq;
using CryCil.Graphics;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a vertex that is defined by <see cref="VertexFormat.P3S_C4B_T2S"/>.
	/// </summary>
	public struct P3F_C4B_T2F
	{
		/// <summary>
		/// Coordinates of the vertex.
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// Color of the vertex.
		/// </summary>
		public ColorByte Color;
		/// <summary>
		/// Coordinates of the vertex on UV map.
		/// </summary>
		public Vector2 TexturePosition;
	}
}