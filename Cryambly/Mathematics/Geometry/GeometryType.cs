namespace CryCil.Geometry
{
	/// <summary>
	/// Enumeration of types of geometry used in the engine.
	/// </summary>
	public enum GeometryType
	{
		/// <summary>
		/// Not specified.
		/// </summary>
		None,
		/// <summary>
		/// Bounding box.
		/// </summary>
		BoundingBox,
		/// <summary>
		/// Physics proxy.
		/// </summary>
		Physics,
		/// <summary>
		/// Render proxy.
		/// </summary>
		Render
	}
}