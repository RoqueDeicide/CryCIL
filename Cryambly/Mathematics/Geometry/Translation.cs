namespace CryCil.Geometry
{
	/// <summary>
	/// Defines functions that are used when there is a need to perform translation affine
	/// transformation.
	/// </summary>
	public static class Translation
	{
		/// <summary>
		/// Translates given vector.
		/// </summary>
		/// <param name="vectorToMove">Vector that needs to be moved.</param>
		/// <param name="delta">       Amounts of movement along respective axes.</param>
		public static void Apply(ref Vector2 vectorToMove, ref Vector2 delta)
		{
			vectorToMove.X += delta.X;
			vectorToMove.Y += delta.Y;
		}
		/// <summary>
		/// Translates given vector.
		/// </summary>
		/// <param name="vectorToMove">Vector that needs to be moved.</param>
		/// <param name="delta">       Amounts of movement along respective axes.</param>
		public static void Apply(ref Vector3 vectorToMove, ref Vector3 delta)
		{
			vectorToMove.X += delta.X;
			vectorToMove.Y += delta.Y;
			vectorToMove.Z += delta.Z;
		}
		/// <summary>
		/// Translates given vector.
		/// </summary>
		/// <param name="vectorToMove">Vector that needs to be moved.</param>
		/// <param name="delta">       Amounts of movement along respective axes.</param>
		public static void Apply(ref Vector4 vectorToMove, ref Vector4 delta)
		{
			vectorToMove.X += delta.X;
			vectorToMove.Y += delta.Y;
			vectorToMove.Z += delta.Z;
			vectorToMove.W += delta.W;
		}
		/// <summary>
		/// Stores translation transformation in the matrix.
		/// </summary>
		/// <param name="matrix">Matrix that will store the transformation.</param>
		/// <param name="delta"> Amounts of movement along respective axes.</param>
		public static void Add(ref Matrix34 matrix, ref Vector3 delta)
		{
			matrix.M03 = matrix.M00 * delta.X + matrix.M01 * delta.Y + matrix.M02 * delta.Z;
			matrix.M13 = matrix.M10 * delta.X + matrix.M11 * delta.Y + matrix.M12 * delta.Z;
			matrix.M23 = matrix.M20 * delta.X + matrix.M21 * delta.Y + matrix.M22 * delta.Z;
		}
		/// <summary>
		/// Stores translation transformation in the matrix.
		/// </summary>
		/// <param name="matrix">Matrix that will store the transformation.</param>
		/// <param name="delta"> Amounts of movement along respective axes.</param>
		public static void Add(ref Matrix44 matrix, ref Vector3 delta)
		{
			matrix.M03 += matrix.M00 * delta.X + matrix.M01 * delta.Y + matrix.M02 * delta.Z;
			matrix.M13 += matrix.M10 * delta.X + matrix.M11 * delta.Y + matrix.M12 * delta.Z;
			matrix.M23 += matrix.M20 * delta.X + matrix.M21 * delta.Y + matrix.M22 * delta.Z;
		}
		/// <summary>
		/// Changes given matrix to one that represents a very specific translation
		/// transformation.
		/// </summary>
		/// <param name="matrix">Matrix that needs to be changed.</param>
		/// <param name="delta"> Vector that describes the translation.</param>
		public static void Set(ref Matrix34 matrix, ref Vector3 delta)
		{
			matrix.M03 = delta.X;
			matrix.M13 = delta.Y;
			matrix.M23 = delta.Z;
		}
		/// <summary>
		/// Changes given matrix to one that represents a very specific translation
		/// transformation.
		/// </summary>
		/// <param name="matrix">Matrix that needs to be changed.</param>
		/// <param name="delta"> Vector that describes the translation.</param>
		public static void Set(ref Matrix44 matrix, ref Vector3 delta)
		{
			matrix.M03 = delta.X;
			matrix.M13 = delta.Y;
			matrix.M23 = delta.Z;
		}

		// ReSharper disable RedundantAssignment

		/// <summary>
		/// Overrides given matrix with one that represents a translation transformation.
		/// </summary>
		/// <param name="matrix">Matrix that needs to be overridden.</param>
		/// <param name="delta"> Vector that describes the translation.</param>
		public static void Override(ref Matrix34 matrix, ref Vector3 delta)
		{
			matrix = Matrix34.Identity;

			matrix.M03 = delta.X;
			matrix.M13 = delta.Y;
			matrix.M23 = delta.Z;
		}
		/// <summary>
		/// Overrides given matrix with one that represents a translation transformation.
		/// </summary>
		/// <param name="matrix">Matrix that needs to be overridden.</param>
		/// <param name="delta"> Vector that describes the translation.</param>
		public static void Override(ref Matrix44 matrix, ref Vector3 delta)
		{
			matrix = Matrix44.Identity;

			matrix.M03 = delta.X;
			matrix.M13 = delta.Y;
			matrix.M23 = delta.Z;
		}

		// ReSharper restore RedundantAssignment

		/// <summary>
		/// Creates a matrix that represents given translation transformation.
		/// </summary>
		/// <param name="delta">Amounts of movement along respective axes.</param>
		/// <returns>
		/// Matrix that represents specified translation transformation.
		/// </returns>
		public static Matrix34 Create34(ref Vector3 delta)
		{
			Matrix34 result = Matrix34.Identity;

			result.M03 = delta.X;
			result.M13 = delta.Y;
			result.M23 = delta.Z;

			return result;
		}
		/// <summary>
		/// Creates a matrix that represents given translation transformation.
		/// </summary>
		/// <param name="delta">Amounts of movement along respective axes.</param>
		/// <returns>
		/// Matrix that represents specified translation transformation.
		/// </returns>
		public static Matrix44 Create44(ref Vector3 delta)
		{
			Matrix44 result = Matrix44.Identity;

			result.M03 = delta.X;
			result.M13 = delta.Y;
			result.M23 = delta.Z;

			return result;
		}
	}
}