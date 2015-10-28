using CryCil.Graphics;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a vertex that is defined by <see cref="VertexFormat.P3F_C4B_T4B_N3F2"/>.
	/// </summary>
	public struct P3F_C4B_T4B_N3F2
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
		public ColorByte TexturePosition;
		/// <summary>
		/// X-axis of the vertex.
		/// </summary>
		public Vector3 Normal0;
		/// <summary>
		/// Y-axis of the vertex.
		/// </summary>
		public Vector3 Normal1;
	}
}