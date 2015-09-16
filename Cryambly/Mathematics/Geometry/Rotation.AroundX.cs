namespace CryCil.Geometry
{
	public partial class Rotation
	{
		/// <summary>
		/// Defines functions for working with rotations around X-axis.
		/// </summary>
		public static class AroundX
		{
			/// <summary>
			/// Rotates given vector around X-axis by given angle.
			/// </summary>
			/// <param name="vector">Vector to rotate.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Apply(ref Vector3 vector, float angle)
			{
				double s, c;
				MathHelpers.SinCos(angle, out s, out c);

				Vector3 temp = vector;

				vector.Y = (float)(c * temp.Y + (-s) * temp.Z);
				vector.Z = (float)(s * temp.Y + c * temp.Z);
			}
			/// <summary>
			/// Rotates given vector around X-axis using specified pivot point.
			/// </summary>
			/// <param name="vector">Vector to rotate.</param>
			/// <param name="pivot"> Pivot point of rotation.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Apply(ref Vector3 vector, ref Vector3 pivot, float angle)
			{
				// Move point of origin to the pivot.
				Vector3 originToPivot = -pivot;
				Translation.Apply(ref vector, ref originToPivot);
				// Rotate the vector.
				Apply(ref vector, angle);
				// Return to original start point for coordinates.
				Translation.Apply(ref vector, ref pivot);
			}
			/// <summary>
			/// Overrides given matrix with one that represents rotation around X-axis.
			/// </summary>
			/// <param name="matrix">Matrix to override.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Override(out Matrix33 matrix, float angle)
			{
				matrix = Matrix33.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);
				matrix.M11 = cosine;
				matrix.M12 = -sine;
				matrix.M21 = sine;
				matrix.M22 = cosine;
			}
			/// <summary>
			/// Overrides given matrix with one that represents rotation around X-axis.
			/// </summary>
			/// <param name="matrix">Matrix to override.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Override(out Matrix34 matrix, float angle)
			{
				matrix = Matrix34.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);
				matrix.M11 = cosine;
				matrix.M12 = -sine;
				matrix.M21 = sine;
				matrix.M22 = cosine;
			}
			/// <summary>
			/// Overrides given quaternion with one that represents rotation around X-axis.
			/// </summary>
			/// <param name="matrix">Matrix to override.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Override(out Matrix44 matrix, float angle)
			{
				matrix = Matrix44.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);
				matrix.M11 = cosine;
				matrix.M12 = -sine;
				matrix.M21 = sine;
				matrix.M22 = cosine;
			}
			/// <summary>
			/// Overrides given quaternion with one that represents rotation around X-axis.
			/// </summary>
			/// <param name="quaternion">Matrix to override.</param>
			/// <param name="angle">     Angle of rotation in radians.</param>
			public static void Override(out Quaternion quaternion, float angle)
			{
				quaternion = Quaternion.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle * 0.5f, out sine, out cosine);
				quaternion.W = cosine;
				quaternion.X = sine;
			}
			/// <summary>
			/// Overrides given matrix with one that represents rotation around X-axis.
			/// </summary>
			/// <param name="matrix">Matrix to override.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Set(ref Matrix33 matrix, float angle)
			{
				float s, c;
				MathHelpers.SinCos(angle, out s, out c);
				matrix.M00 = 1.0f;
				matrix.M01 = 0.0f;
				matrix.M02 = 0.0f;
				matrix.M10 = 0.0f;
				matrix.M11 = c;
				matrix.M12 = -s;
				matrix.M20 = 0.0f;
				matrix.M21 = s;
				matrix.M22 = c;
			}
			/// <summary>
			/// Changes first 3 columns of given matrix so they represent a rotation around X-axis.
			/// </summary>
			/// <param name="matrix">Matrix to change.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Set(ref Matrix34 matrix, float angle)
			{
				float s, c;
				MathHelpers.SinCos(angle, out s, out c);
				matrix.M00 = 1.0f;
				matrix.M01 = 0.0f;
				matrix.M02 = 0.0f;
				matrix.M10 = 0.0f;
				matrix.M11 = c;
				matrix.M12 = -s;
				matrix.M20 = 0.0f;
				matrix.M21 = s;
				matrix.M22 = c;
			}
			/// <summary>
			/// Changes top-left 3x3 submatrix of given matrix so it represent a rotation around X-axis.
			/// </summary>
			/// <param name="matrix">Matrix to change.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Set(ref Matrix44 matrix, float angle)
			{
				float s, c;
				MathHelpers.SinCos(angle, out s, out c);
				matrix.M00 = 1.0f;
				matrix.M01 = 0.0f;
				matrix.M02 = 0.0f;
				matrix.M10 = 0.0f;
				matrix.M11 = c;
				matrix.M12 = -s;
				matrix.M20 = 0.0f;
				matrix.M21 = s;
				matrix.M22 = c;
			}
			/// <summary>
			/// Overrides given quaternion with one that represents rotation around X-axis.
			/// </summary>
			/// <param name="quaternion">Matrix to override.</param>
			/// <param name="angle">     Angle of rotation in radians.</param>
			public static void Set(ref Quaternion quaternion, float angle)
			{
				float sine, cosine;
				MathHelpers.SinCos(angle * 0.5f, out sine, out cosine);
				quaternion.W = cosine;
				quaternion.X = sine;
				quaternion.Y = 0;
				quaternion.Z = 0;
			}
			/// <summary>
			/// Changes given matrix so it represents transformation represented by its original value
			/// followed up by rotation around X-axis.
			/// </summary>
			/// <param name="matrix">Matrix to add the rotation to.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void StackUp(ref Matrix33 matrix, float angle)
			{
				Matrix33 rm = Matrix33.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);

				rm.M11 = cosine;
				rm.M12 = -sine;
				rm.M21 = sine;
				rm.M22 = cosine;

				matrix = rm * matrix;
			}
			/// <summary>
			/// Changes given matrix so it represents transformation represented by its original value
			/// followed up by rotation around X-axis.
			/// </summary>
			/// <param name="matrix">Matrix to add the rotation to.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void StackUp(ref Matrix34 matrix, float angle)
			{
				Matrix34 rm = Matrix34.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);

				rm.M11 = cosine;
				rm.M12 = -sine;
				rm.M21 = sine;
				rm.M22 = cosine;

				matrix = rm * matrix;
			}
			/// <summary>
			/// Changes given matrix so it represents transformation represented by its original value
			/// followed up by rotation around X-axis.
			/// </summary>
			/// <param name="matrix">Matrix to add the rotation to.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void StackUp(ref Matrix44 matrix, float angle)
			{
				Matrix44 rm = Matrix44.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);

				rm.M11 = cosine;
				rm.M12 = -sine;
				rm.M21 = sine;
				rm.M22 = cosine;

				matrix = rm * matrix;
			}
			/// <summary>
			/// Stacks rotation around X-axis into given quaternion.
			/// </summary>
			/// <remarks>
			/// This function modifies given quaternion in such a way that transforming a vector by
			/// resultant quaternion is equavalent of applying original transformation and follow it up
			/// with rotation around X-axis.
			/// </remarks>
			/// <param name="quaternion">Quaternion to add rotation to.</param>
			/// <param name="angle">     Angle of rotation in radians.</param>
			public static void StackUp(ref Quaternion quaternion, float angle)
			{
				float s, c;
				MathHelpers.SinCos(angle * 0.5f, out s, out c);
				Quaternion secondQuaternion = Quaternion.Identity;

				secondQuaternion.W = c;
				secondQuaternion.X = s;

				quaternion = secondQuaternion * quaternion;
			}
			/// <summary>
			/// Creates new 3x3 matrix that represents a rotation around X-axis.
			/// </summary>
			/// <param name="angle">Angle of rotation in radians.</param>
			/// <returns>3x3 matrix that represents a rotation around X-axis.</returns>
			public static Matrix33 Create33(float angle)
			{
				Matrix33 matrix = Matrix33.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);
				matrix.M11 = cosine;
				matrix.M12 = -sine;
				matrix.M21 = sine;
				matrix.M22 = cosine;
				return matrix;
			}
			/// <summary>
			/// Creates new 3x4 matrix that represents a rotation around X-axis.
			/// </summary>
			/// <param name="angle">Angle of rotation in radians.</param>
			/// <returns>3x4 matrix that represents a rotation around X-axis.</returns>
			public static Matrix34 Create34(float angle)
			{
				Matrix34 matrix = Matrix34.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);
				matrix.M11 = cosine;
				matrix.M12 = -sine;
				matrix.M21 = sine;
				matrix.M22 = cosine;
				return matrix;
			}
			/// <summary>
			/// Creates new 4x4 matrix that represents a rotation around X-axis.
			/// </summary>
			/// <param name="angle">Angle of rotation in radians.</param>
			/// <returns>4x4 matrix that represents a rotation around X-axis.</returns>
			public static Matrix44 Create44(float angle)
			{
				Matrix44 matrix = Matrix44.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);
				matrix.M11 = cosine;
				matrix.M12 = -sine;
				matrix.M21 = sine;
				matrix.M22 = cosine;
				return matrix;
			}
			/// <summary>
			/// Creates a quaternion that represents rotation around X-axis.
			/// </summary>
			/// <param name="angle">Angle of rotation in radians.</param>
			/// <returns><see cref="Quaternion"/> that represents rotation around X-axis.</returns>
			public static Quaternion CreateQuaternion(float angle)
			{
				Quaternion quaternion = Quaternion.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle * 0.5f, out sine, out cosine);
				quaternion.W = cosine;
				quaternion.X = sine;
				return quaternion;
			}
		}
	}
}