using System;
using CryCil.Graphics;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a vertex that is defined by <see cref="VertexFormat.P3S_N4B_C4B_T2S"/>.
	/// </summary>
	public struct P3S_N4B_C4B_T2S
	{
		/// <summary>
		/// Coordinates of the vertex.
		/// </summary>
		public Vector3Half Position;
		/// <summary>
		/// Normal to the vertex.
		/// </summary>
		public ColorByte Normal;
		/// <summary>
		/// Color of the vertex.
		/// </summary>
		public ColorByte Color;
		/// <summary>
		/// Coordinates of the vertex on UV map.
		/// </summary>
		public Vector2Half TexturePosition;
	}
}