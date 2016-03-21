using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace CryCil.Geometry
{
	/// <summary>
	/// Represents an Oriented Bounding Box.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct OBB
	{
		/// <summary>
		/// Represents orientation of the bounding box.
		/// </summary>
		public Matrix33 Matrix;
		/// <summary>
		/// Half-length-vector.
		/// </summary>
		public Vector3 HalfLength;
		/// <summary>
		/// Position of the center of the bounding box.
		/// </summary>
		public Vector3 Center;
		/// <summary>
		/// Creates new OBB.
		/// </summary>
		/// <param name="matrix">          
		/// <see cref="Matrix33"/> that represents orientation of the bounding box.
		/// </param>
		/// <param name="halfLengthVector">Half-length-vector.</param>
		/// <param name="center">          Position of the center of the bounding box.</param>
		public OBB(Matrix33 matrix, Vector3 halfLengthVector, Vector3 center)
		{
			this.Matrix = matrix;
			this.HalfLength = halfLengthVector;
			this.Center = center;
		}
		/// <summary>
		/// Creates new OBB from AABB.
		/// </summary>
		/// <param name="matrix">
		/// <see cref="Matrix33"/> that represents orientation of the bounding box.
		/// </param>
		/// <param name="aabb">  <see cref="BoundingBox"/> to orient.</param>
		public OBB(Matrix33 matrix, BoundingBox aabb)
		{
			this.Matrix = matrix;
			this.HalfLength = (aabb.Maximum - aabb.Minimum) * 0.5f;
			this.Center = (aabb.Maximum + aabb.Minimum) * 0.5f;
		}
		/// <summary>
		/// Creates new OBB from AABB.
		/// </summary>
		/// <param name="quaternion">
		/// <see cref="Quaternion"/> that represents orientation of the bounding box.
		/// </param>
		/// <param name="aabb">      <see cref="BoundingBox"/> to orient.</param>
		public OBB(Quaternion quaternion, BoundingBox aabb)
		{
			this.Matrix = new Matrix33(quaternion);
			this.HalfLength = (aabb.Maximum - aabb.Minimum) * 0.5f;
			this.Center = (aabb.Maximum + aabb.Minimum) * 0.5f;
		}
	}
}