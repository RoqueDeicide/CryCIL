using System.Runtime.InteropServices;

namespace CryCil
{
	/// <summary>
	/// Encapsulates 2 integer numbers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2Int32
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
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of type <see cref="Vector2Int32"/>.
		/// </summary>
		/// <param name="x">X-component of the new vector.</param>
		/// <param name="y">Y-component of the new vector.</param>
		public Vector2Int32(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
		#endregion
	}
}