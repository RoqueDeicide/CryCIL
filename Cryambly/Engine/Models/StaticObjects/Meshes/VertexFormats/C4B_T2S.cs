using System;
using System.Linq;
using CryCil.Graphics;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a vertex that is defined by <see cref="VertexFormat.C4B_T2S"/>.
	/// </summary>
	public struct C4B_T2S
	{
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