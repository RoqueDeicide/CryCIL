using CryCil.Graphics;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a vertex that has its position, color and texture coordinates are represented by 3D
	/// vector with half-precision floating point numbers, 4 <see cref="byte"/> size numbers in range [0;
	/// 255] and 2D vector with half-precision floating point numbers respectively.
	/// </summary>
	public struct Position3HalfColor4ByteTexture2Half
	{
		/// <summary>
		/// Coordinates of the vertex.
		/// </summary>
		public Vector3Half Position;
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