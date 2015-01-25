using CryCil.Graphics;

namespace CryCil.Geometry
{
	/// <summary>
	/// Encapsulates data about a single vertex.
	/// </summary>
	public struct FullVertex
	{
		/// <summary>
		/// Position of the vertex in the world.
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// Vector that is perpendicular to the "surface" this vertex is on.
		/// </summary>
		public Vector3 Normal;
		/// <summary>
		/// Position of this vertex on the UV map.
		/// </summary>
		public Vector2 UvPosition;
		/// <summary>
		/// Primary color of this vertex.
		/// </summary>
		public ColorByte PrimaryColor;
		/// <summary>
		/// Secondary color of this vertex.
		/// </summary>
		public ColorByte SecondaryColor;
	}
}