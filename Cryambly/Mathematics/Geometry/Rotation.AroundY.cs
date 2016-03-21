using System;
using System.Linq;

namespace CryCil.Geometry
{
	public partial class Rotation
	{
		/// <summary>
		/// Defines functions for working with rotations around Y-axis.
		/// </summary>
		public static class AroundY
		{
			/// <summary>
			/// Rotates given vector around Y-axis by given angle.
			/// </summary>
			/// <param name="vector">Vector to rotate.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Apply(ref Vector3 vector, float angle)
			{
				double s, c;
				MathHelpers.SinCos(angle, out s, out c);

				Vector3 temp = vector;

				vector.X = (float)(c * temp.X + s * temp.Z);
				vector.Z = (float)(-s * temp.X + c * temp.Z);
			}
			/// <summary>
			/// Rotates given vector around Y-axis using specified pivot.
			/// </summary>
			/// <param name="vector">Vector to rotate.</param>
			/// <param name="pivot"> Point to rotate vector around.</param>
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
			/// Overrides given matrix with one that represents rotation around Y-axis.
			/// </summary>
			/// <param name="matrix">Matrix to override.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Override(out Matrix33 matrix, float angle)
			{
				matrix = Matrix33.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);
				matrix.M00 = cosine;
				matrix.M02 = sine;
				matrix.M20 = -sine;
				matrix.M22 = cosine;
			}
			/// <summary>
			/// Overrides given matrix with one that represents rotation around Y-axis.
			/// </summary>
			/// <param name="matrix">Matrix to override.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Override(out Matrix34 matrix, float angle)
			{
				matrix = Matrix34.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);
				matrix.M00 = cosine;
				matrix.M02 = sine;
				matrix.M20 = -sine;
				matrix.M22 = cosine;
			}
			/// <summary>
			/// Overrides given quaternion with one that represents rotation around Y-axis.
			/// </summary>
			/// <param name="matrix">Matrix to override.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Override(out Matrix44 matrix, float angle)
			{
				matrix = Matrix44.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);
				matrix.M00 = cosine;
				matrix.M02 = sine;
				matrix.M20 = -sine;
				matrix.M22 = cosine;
			}
			/// <summary>
			/// Overrides given quaternion with one that represents rotation around Y-axis.
			/// </summary>
			/// <param name="quaternion">Matrix to override.</param>
			/// <param name="angle">     Angle of rotation in radians.</param>
			public static void Override(out Quaternion quaternion, float angle)
			{
				quaternion = Quaternion.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle * 0.5f, out sine, out cosine);
				quaternion.W = cosine;
				quaternion.Y = sine;
			}
			/// <summary>
			/// Overrides given matrix with one that represents rotation around Y-axis.
			/// </summary>
			/// <param name="matrix">Matrix to override.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Set(ref Matrix33 matrix, float angle)
			{
				float s, c;
				MathHelpers.SinCos(angle, out s, out c);
				matrix.M00 = c;
				matrix.M01 = 0;
				matrix.M02 = s;
				matrix.M10 = 0;
				matrix.M11 = 1;
				matrix.M12 = 0;
				matrix.M20 = -s;
				matrix.M21 = 0;
				matrix.M22 = c;
			}
			/// <summary>
			/// Changes first 3 columns of given matrix so they represent a rotation around Y-axis.
			/// </summary>
			/// <param name="matrix">Matrix to change.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Set(ref Matrix34 matrix, float angle)
			{
				float s, c;
				MathHelpers.SinCos(angle, out s, out c);
				matrix.M00 = c;
				matrix.M01 = 0;
				matrix.M02 = s;
				matrix.M10 = 0;
				matrix.M11 = 1;
				matrix.M12 = 0;
				matrix.M20 = -s;
				matrix.M21 = 0;
				matrix.M22 = c;
			}
			/// <summary>
			/// Changes top-left 3x3 submatrix of given matrix so it represent a rotation around Y-axis.
			/// </summary>
			/// <param name="matrix">Matrix to change.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Set(ref Matrix44 matrix, float angle)
			{
				float s, c;
				MathHelpers.SinCos(angle, out s, out c);
				matrix.M00 = c;
				matrix.M01 = 0;
				matrix.M02 = s;
				matrix.M10 = 0;
				matrix.M11 = 1;
				matrix.M12 = 0;
				matrix.M20 = -s;
				matrix.M21 = 0;
				matrix.M22 = c;
			}
			/// <summary>
			/// Overrides given quaternion with one that represents rotation around Y-axis.
			/// </summary>
			/// <param name="quaternion">Matrix to override.</param>
			/// <param name="angle">     Angle of rotation in radians.</param>
			public static void Set(ref Quaternion quaternion, float angle)
			{
				float sine, cosine;
				MathHelpers.SinCos(angle * 0.5f, out sine, out cosine);
				quaternion.W = cosine;
				quaternion.Y = sine;
				quaternion.X = 0;
				quaternion.Z = 0;
			}
			/// <summary>
			/// Changes given matrix so it represents transformation represented by its original value
			/// followed up by rotation around Y-axis.
			/// </summary>
			/// <param name="matrix">Matrix to add the rotation to.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void StackUp(ref Matrix33 matrix, float angle)
			{
				Matrix33 rm = Matrix33.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);

				rm.M00 = cosine;
				rm.M02 = sine;
				rm.M20 = -sine;
				rm.M22 = cosine;

				matrix = rm * matrix;
			}
			/// <summary>
			/// Changes given matrix so it represents transformation represented by its original value
			/// followed up by rotation around Y-axis.
			/// </summary>
			/// <param name="matrix">Matrix to add the rotation to.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void StackUp(ref Matrix34 matrix, float angle)
			{
				Matrix34 rm = Matrix34.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);

				rm.M00 = cosine;
				rm.M02 = sine;
				rm.M20 = -sine;
				rm.M22 = cosine;

				matrix = rm * matrix;
			}
			/// <summary>
			/// Changes given matrix so it represents transformation represented by its original value
			/// followed up by rotation around Y-axis.
			/// </summary>
			/// <param name="matrix">Matrix to add the rotation to.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void StackUp(ref Matrix44 matrix, float angle)
			{
				Matrix44 rm = Matrix44.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);

				rm.M00 = cosine;
				rm.M02 = sine;
				rm.M20 = -sine;
				rm.M22 = cosine;

				matrix = rm * matrix;
			}
			/// <summary>
			/// Stacks rotation around Y-axis into given quaternion.
			/// </summary>
			/// <remarks>
			/// This function modifies given quaternion in such a way that transforming a vector by
			/// resultant quaternion is equavalent of applying original transformation and follow it up with
			/// rotation around Y-axis.
			/// </remarks>
			/// <param name="quaternion">Quaternion to add rotation to.</param>
			/// <param name="angle">     Angle of rotation in radians.</param>
			public static void StackUp(ref Quaternion quaternion, float angle)
			{
				float s, c;
				MathHelpers.SinCos(angle * 0.5f, out s, out c);
				Quaternion secondQuaternion = Quaternion.Identity;

				secondQuaternion.W = c;
				secondQuaternion.Y = s;

				quaternion = secondQuaternion * quaternion;
			}
			/// <summary>
			/// Creates new 3x3 matrix that represents a rotation around Y-axis.
			/// </summary>
			/// <param name="angle">Angle of rotation in radians.</param>
			/// <returns>3x3 matrix that represents a rotation around Y-axis.</returns>
			public static Matrix33 Create33(float angle)
			{
				Matrix33 matrix = Matrix33.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);
				matrix.M00 = cosine;
				matrix.M02 = sine;
				matrix.M20 = -sine;
				matrix.M22 = cosine;
				return matrix;
			}
			/// <summary>
			/// Creates new 3x4 matrix that represents a rotation around Y-axis.
			/// </summary>
			/// <param name="angle">Angle of rotation in radians.</param>
			/// <returns>3x4 matrix that represents a rotation around Y-axis.</returns>
			public static Matrix34 Create34(float angle)
			{
				Matrix34 matrix = Matrix34.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);
				matrix.M00 = cosine;
				matrix.M02 = sine;
				matrix.M20 = -sine;
				matrix.M22 = cosine;
				return matrix;
			}
			/// <summary>
			/// Creates new 4x4 matrix that represents a rotation around Y-axis.
			/// </summary>
			/// <param name="angle">Angle of rotation in radians.</param>
			/// <returns>4x4 matrix that represents a rotation around Y-axis.</returns>
			public static Matrix44 Create44(float angle)
			{
				Matrix44 matrix = Matrix44.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);
				matrix.M00 = cosine;
				matrix.M02 = sine;
				matrix.M20 = -sine;
				matrix.M22 = cosine;
				return matrix;
			}
			/// <summary>
			/// Creates a quaternion that represents rotation around Y-axis.
			/// </summary>
			/// <param name="angle">Angle of rotation in radians.</param>
			/// <returns><see cref="Quaternion"/> that represents rotation around Y-axis.</returns>
			public static Quaternion CreateQuaternion(float angle)
			{
				Quaternion quaternion = Quaternion.Identity;
				float sine, cosine;
				MathHelpers.SinCos(angle * 0.5f, out sine, out cosine);
				quaternion.W = cosine;
				quaternion.Y = sine;
				return quaternion;
			}
		}
	}
}