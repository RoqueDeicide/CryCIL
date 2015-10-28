using CryCil.Graphics;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a vertex that is defined by <see cref="VertexFormat.P2S_N4B_C4B_T1F"/>.
	/// </summary>
	public struct P2S_N4B_C4B_T1F
	{
		/// <summary>
		/// Coordinates of the vertex.
		/// </summary>
		public Vector2Half Position;
		/// <summary>
		/// Normal to the vertex.
		/// </summary>
		public ColorByte Normal;
		/// <summary>
		/// Color of the vertex.
		/// </summary>
		public ColorByte Color;
		/// <summary>
		/// Depth position of the vertex.
		/// </summary>
		public float Z;
	}
}