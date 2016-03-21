using System;
using System.Linq;

namespace CryCil
{
	/// <summary>
	/// Encapsulates 3 integer numbers.
	/// </summary>
	public struct Vector3Int32
	{
		#region Fields
		/// <summary>
		/// X-component of this vector.
		/// </summary>
		public int X;
		/// <summary>
		/// Y-component of this vector.
		/// </summary>
		public int Y;
		/// <summary>
		/// Z-component of this vector.
		/// </summary>
		public int Z;
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of type <see cref="Vector3Int32"/>.
		/// </summary>
		/// <param name="x">X-component of the new vector.</param>
		/// <param name="y">Y-component of the new vector.</param>
		/// <param name="z">Z-component of the new vector.</param>
		public Vector3Int32(int x, int y, int z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}
		#endregion
	}
}