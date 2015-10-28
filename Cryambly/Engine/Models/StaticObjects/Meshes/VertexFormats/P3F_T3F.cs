namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a vertex that is defined by <see cref="VertexFormat.P3F_T3F"/>.
	/// </summary>
	public struct P3F_T3F
	{
		/// <summary>
		/// Coordinates of the vertex.
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// Coordinates of the vertex on UV map.
		/// </summary>
		public Vector3 TexturePosition;
	}
}