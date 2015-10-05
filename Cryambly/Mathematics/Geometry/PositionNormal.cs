namespace CryCil.Geometry
{
	/// <summary>
	/// Encapsulates coordinates of the point on the surface and a normal to surface at the point.
	/// </summary>
	public struct PositionNormal
	{
		/// <summary>
		/// Coordinates of the point.
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// Normal to the surface at the point.
		/// </summary>
		public Vector3 Normal;
	}
}