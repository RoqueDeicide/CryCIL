using System;

namespace CryCil.Geometry
{
	/// <summary>
	/// Defines functions for working with combined transformations.
	/// </summary>
	public static partial class Transformation
	{
		/// <summary>
		/// Applies transformation that is represented by given quaternion to the vector.
		/// </summary>
		/// <param name="vector">    Vector to transform.</param>
		/// <param name="quaternion">Quaternion that represents the transformation.</param>
		public static void Apply(ref Vector3 vector, ref Quaternion quaternion)
		{
			vector.X = quaternion.W * quaternion.W * vector.X + 2 * quaternion.Y * quaternion.W * vector.Z -
					   2 * quaternion.Z * quaternion.W * vector.Y + quaternion.X * quaternion.X * vector.X +
					   2 * quaternion.Y * quaternion.X * vector.Y + 2 * quaternion.Z * quaternion.X * vector.Z -
					   quaternion.Z * quaternion.Z * vector.X - quaternion.Y * quaternion.Y * vector.X;
			vector.Y = 2 * quaternion.X * quaternion.Y * vector.X + quaternion.Y * quaternion.Y * vector.Y +
					   2 * quaternion.Z * quaternion.Y * vector.Z + 2 * quaternion.W * quaternion.Z * vector.X -
					   quaternion.Z * quaternion.Z * vector.Y + quaternion.W * quaternion.W * vector.Y -
					   2 * quaternion.X * quaternion.W * vector.Z - quaternion.X * quaternion.X * vector.Y;
			vector.Z = 2 * quaternion.X * quaternion.Z * vector.X + 2 * quaternion.Y * quaternion.Z * vector.Y +
					   quaternion.Z * quaternion.Z * vector.Z - 2 * quaternion.W * quaternion.Y * vector.X -
					   quaternion.Y * quaternion.Y * vector.Z + 2 * quaternion.W * quaternion.X * vector.Y -
					   quaternion.X * quaternion.X * vector.Z + quaternion.W * quaternion.W * vector.Z;
		}
		/// <summary>
		/// Applies transformation that is represented by given matrix to the vector.
		/// </summary>
		/// <param name="vector">Vector to transform.</param>
		/// <param name="matrix">Matrix that represents the transformation.</param>
		public static void Apply(ref Vector3 vector, ref Matrix33 matrix)
		{
			vector.X = vector.X * matrix.M00 + vector.Y * matrix.M01 + vector.Z * matrix.M02;
			vector.Y = vector.X * matrix.M10 + vector.Y * matrix.M11 + vector.Z * matrix.M12;
			vector.Z = vector.X * matrix.M20 + vector.Y * matrix.M21 + vector.Z * matrix.M22;
		}
		/// <summary>
		/// Applies transformation that is represented by given matrix to the vector.
		/// </summary>
		/// <param name="vector">Vector to transform.</param>
		/// <param name="matrix">Matrix that represents the transformation.</param>
		public static void Apply(ref Vector3 vector, ref Matrix34 matrix)
		{
			vector.X = vector.X * matrix.M00 + vector.Y * matrix.M01 + vector.Z * matrix.M02 + matrix.M03;
			vector.Y = vector.X * matrix.M10 + vector.Y * matrix.M11 + vector.Z * matrix.M12 + matrix.M13;
			vector.Z = vector.X * matrix.M20 + vector.Y * matrix.M21 + vector.Z * matrix.M22 + matrix.M23;
		}
		/// <summary>
		/// Applies transformation that is represented by given matrix to the vector.
		/// </summary>
		/// <param name="vector">Vector to transform.</param>
		/// <param name="matrix">Matrix that represents the transformation.</param>
		public static void Apply(ref Vector3 vector, ref Matrix44 matrix)
		{
			vector.X = vector.X * matrix.M00 + vector.Y * matrix.M01 + vector.Z * matrix.M02 + matrix.M03;
			vector.Y = vector.X * matrix.M10 + vector.Y * matrix.M11 + vector.Z * matrix.M12 + matrix.M13;
			vector.Z = vector.X * matrix.M20 + vector.Y * matrix.M21 + vector.Z * matrix.M22 + matrix.M23;
		}
		/// <summary>
		/// Applies transformation that is represented by given matrix to the vector.
		/// </summary>
		/// <param name="vector">Vector to transform.</param>
		/// <param name="matrix">Matrix that represents the transformation.</param>
		public static void Apply(ref Vector4 vector, ref Matrix44 matrix)
		{
			vector.X = vector.X * matrix.M00 + vector.Y * matrix.M01 + vector.Z * matrix.M02 + vector.W * matrix.M03;
			vector.Y = vector.X * matrix.M10 + vector.Y * matrix.M11 + vector.Z * matrix.M12 + vector.W * matrix.M13;
			vector.Z = vector.X * matrix.M20 + vector.Y * matrix.M21 + vector.Z * matrix.M22 + vector.W * matrix.M23;
			vector.W = vector.X * matrix.M30 + vector.Y * matrix.M31 + vector.Z * matrix.M32 + vector.W * matrix.M33;
		}
		/// <summary>
		/// Combines two transformations together.
		/// </summary>
		/// <param name="first"> <see cref="Quaternion"/> that represents first transformation.</param>
		/// <param name="second"><see cref="Quaternion"/> that represents second transformation.</param>
		/// <returns>
		/// <see cref="Quaternion"/> that represents transformation that is equivalent of two given
		/// transformations applied in order first-second.
		/// </returns>
		public static Quaternion Combine(ref Quaternion first, ref Quaternion second)
		{
			return second * first;
		}
		/// <summary>
		/// Combines two transformations together.
		/// </summary>
		/// <param name="first"> <see cref="Matrix33"/> that represents first transformation.</param>
		/// <param name="second"><see cref="Matrix33"/> that represents second transformation.</param>
		/// <returns>
		/// <see cref="Matrix33"/> that represents transformation that is equivalent of two given
		/// transformations applied in order first-second.
		/// </returns>
		public static Matrix33 Combine(ref Matrix33 first, ref Matrix33 second)
		{
			return second * first;
		}
		/// <summary>
		/// Combines two transformations together.
		/// </summary>
		/// <param name="first"> <see cref="Matrix34"/> that represents first transformation.</param>
		/// <param name="second"><see cref="Matrix34"/> that represents second transformation.</param>
		/// <returns>
		/// <see cref="Matrix34"/> that represents transformation that is equivalent of two given
		/// transformations applied in order first-second.
		/// </returns>
		public static Matrix34 Combine(ref Matrix34 first, ref Matrix34 second)
		{
			return second * first;
		}
		/// <summary>
		/// Combines two transformations together.
		/// </summary>
		/// <param name="first"> <see cref="Matrix44"/> that represents first transformation.</param>
		/// <param name="second"><see cref="Matrix44"/> that represents second transformation.</param>
		/// <returns>
		/// <see cref="Matrix44"/> that represents transformation that is equivalent of two given
		/// transformations applied in order first-second.
		/// </returns>
		public static Matrix44 Combine(ref Matrix44 first, ref Matrix44 second)
		{
			return second * first;
		}
	}
}