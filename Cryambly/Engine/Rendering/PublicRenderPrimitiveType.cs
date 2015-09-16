namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Enumeration of types of user-defined primitives.
	/// </summary>
	public enum PublicRenderPrimitiveType
	{
		/// <summary>
		/// List of isolated triangles.
		/// </summary>
		TriangleList,
		/// <summary>
		/// A strip of triangles where triangles are formed in order (1, 2, 3), (2, 4, 3), (3, 4, 5), (4,
		/// 6, 5) and so on.
		/// </summary>
		TriangleStrip,
		/// <summary>
		/// List of isolated lines.
		/// </summary>
		LineList,
		/// <summary>
		/// A list of vertices that represent a polyline.
		/// </summary>
		LineStrip
	}
}