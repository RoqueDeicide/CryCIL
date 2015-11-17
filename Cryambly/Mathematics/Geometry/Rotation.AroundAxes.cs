using System;
using System.Diagnostics.Contracts;

namespace CryCil.Geometry
{
	public partial class Rotation
	{
		/// <summary>
		/// Defines functions working with rotations that use Euler angles.
		/// </summary>
		public static class AroundAxes
		{
			/// <summary>
			/// Rotates given vector using a set of Euler angles.
			/// </summary>
			/// <param name="vector">Vector to rotate.</param>
			/// <param name="angles">A set of Euler angles to use.</param>
			public static void Apply(ref Vector3 vector, ref EulerAngles angles)
			{
				Contract.Assert(angles.IsUnit());

				double sx, cx;
				MathHelpers.SinCos(angles.Pitch, out sx, out cx);
				double sy, cy;
				MathHelpers.SinCos(angles.Roll, out sy, out cy);
				double sz, cz;
				MathHelpers.SinCos(angles.Yaw, out sz, out cz);
				double sycz = (sy * cz), sysz = (sy * sz);
				vector = new Vector3
					(
					(float)(cy * cz * vector.X + (sycz * sx - cx * sz) * vector.Y + (sycz * cx + sx * sz) * vector.Z),
					(float)(cy * sz * vector.X + (sysz * sx + cx * cz) * vector.Y + (sysz * cx - sx * cz) * vector.Z),
					(float)((-sy) * vector.X + cy * sx * vector.Y + vector.Z)
					);
			}
			/// <summary>
			/// Rotates given vector using a set of Euler angles around a specified pivot.
			/// </summary>
			/// <param name="vector">Vector to rotate.</param>
			/// <param name="pivot"> Point to rotate the vector around.</param>
			/// <param name="angles">A set of Euler angles to use.</param>
			public static void Apply(ref Vector3 vector, ref Vector3 pivot, ref EulerAngles angles)
			{
				// Move point of origin to the pivot.
				Vector3 originToPivot = -pivot;
				Translation.Apply(ref vector, ref originToPivot);
				// Rotate the vector.
				Apply(ref vector, ref angles);
				// Return to original start point for coordinates.
				Translation.Apply(ref vector, ref pivot);
			}
			/// <summary>
			/// Overrides given matrix with one that represents rotation using Euler angles.
			/// </summary>
			/// <param name="matrix">Matrix to override.</param>
			/// <param name="angles">A set of Euler angles.</param>
			public static void Override(out Matrix33 matrix, ref EulerAngles angles)
			{
				matrix = Create33(ref angles);
			}
			/// <summary>
			/// Overrides given matrix with one that represents rotation using Euler angles.
			/// </summary>
			/// <param name="matrix">Matrix to override.</param>
			/// <param name="angles">A set of Euler angles.</param>
			public static void Override(out Matrix34 matrix, ref EulerAngles angles)
			{
				matrix = Create34(ref angles);
			}
			/// <summary>
			/// Overrides given matrix with one that represents rotation using Euler angles.
			/// </summary>
			/// <param name="matrix">Matrix to override.</param>
			/// <param name="angles">A set of Euler angles.</param>
			public static void Override(out Matrix44 matrix, ref EulerAngles angles)
			{
				matrix = Create44(ref angles);
			}
			/// <summary>
			/// Overrides given quaternion with one that represents rotation using Euler angles.
			/// </summary>
			/// <param name="quaternion">Quaternion to override.</param>
			/// <param name="angles">    A set of Euler angles.</param>
			public static void Override(out Quaternion quaternion, ref EulerAngles angles)
			{
				quaternion = CreateQuaternion(ref angles);
			}

			// ReSharper disable RedundantAssignment

			/// <summary>
			/// Overrides given matrix with one that represents rotation using Euler angles.
			/// </summary>
			/// <param name="matrix">Matrix to override.</param>
			/// <param name="angles">A set of Euler angles.</param>
			public static void Set(ref Matrix33 matrix, ref EulerAngles angles)
			{
				matrix = Create33(ref angles);
			}

			// ReSharper restore RedundantAssignment

			/// <summary>
			/// Sets top-left 3x3 portion of the given matrix to one that represents rotation using Euler
			/// angles.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="angles">A set of Euler angles.</param>
			public static void Set(ref Matrix34 matrix, ref EulerAngles angles)
			{
				matrix.Matrix33 = Create33(ref angles);
			}
			/// <summary>
			/// Sets top-left 3x3 portion of the given matrix to one that represents rotation using Euler
			/// angles.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="angles">A set of Euler angles.</param>
			public static void Set(ref Matrix44 matrix, ref EulerAngles angles)
			{
				matrix.Matrix33 = Create33(ref angles);
			}

			// ReSharper disable RedundantAssignment

			/// <summary>
			/// Overrides given quaternion with one that represents rotation using Euler angles.
			/// </summary>
			/// <param name="quaternion">Quaternion to override.</param>
			/// <param name="angles">    A set of Euler angles.</param>
			public static void Set(ref Quaternion quaternion, ref EulerAngles angles)
			{
				quaternion = CreateQuaternion(ref angles);
			}

			// ReSharper restore RedundantAssignment

			/// <summary>
			/// Stacks rotation with Euler angles into given matrix.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="angles">A set of Euler angles.</param>
			public static void StackUp(ref Matrix33 matrix, ref EulerAngles angles)
			{
				matrix = Create33(ref angles) * matrix;
			}
			/// <summary>
			/// Stacks rotation with Euler angles into given matrix.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="angles">A set of Euler angles.</param>
			public static void StackUp(ref Matrix34 matrix, ref EulerAngles angles)
			{
				matrix = Create34(ref angles) * matrix;
			}
			/// <summary>
			/// Stacks rotation with Euler angles into given matrix.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="angles">A set of Euler angles.</param>
			public static void StackUp(ref Matrix44 matrix, ref EulerAngles angles)
			{
				matrix = Create44(ref angles) * matrix;
			}
			/// <summary>
			/// Stacks rotation with Euler angles into given quaternion.
			/// </summary>
			/// <param name="quaternion">Quaternion to modify.</param>
			/// <param name="angles">    A set of Euler angles.</param>
			public static void StackUp(ref Quaternion quaternion, ref EulerAngles angles)
			{
				quaternion = CreateQuaternion(ref angles) * quaternion;
			}
			/// <summary>
			/// Creates a 3x3 matrix that represents rotation with Euler angles.
			/// </summary>
			/// <param name="angles">A set of Euler angles.</param>
			/// <returns>3x3 matrix that represents rotation with Euler angles.</returns>
			public static Matrix33 Create33(ref EulerAngles angles)
			{
				double sx, cx;
				MathHelpers.SinCos(angles.Pitch, out sx, out cx);
				double sy, cy;
				MathHelpers.SinCos(angles.Roll, out sy, out cy);
				double sz, cz;
				MathHelpers.SinCos(angles.Yaw, out sz, out cz);
				double sycz = (sy * cz), sysz = (sy * sz);
				Matrix33 mat = Matrix33.Identity;
				mat.M00 = (float)(cy * cz);
				mat.M01 = (float)(sycz * sx - cx * sz);
				mat.M02 = (float)(sycz * cx + sx * sz);
				mat.M10 = (float)(cy * sz);
				mat.M11 = (float)(sysz * sx + cx * cz);
				mat.M12 = (float)(sysz * cx - sx * cz);
				mat.M20 = (float)(-sy);
				mat.M21 = (float)(cy * sx);
				mat.M22 = (float)(cy * cx);
				return mat;
			}
			/// <summary>
			/// Creates a 3x4 matrix that represents rotation with Euler angles.
			/// </summary>
			/// <param name="angles">A set of Euler angles.</param>
			/// <returns>3x4 matrix that represents rotation with Euler angles.</returns>
			public static Matrix34 Create34(ref EulerAngles angles)
			{
				double sx, cx;
				MathHelpers.SinCos(angles.Pitch, out sx, out cx);
				double sy, cy;
				MathHelpers.SinCos(angles.Roll, out sy, out cy);
				double sz, cz;
				MathHelpers.SinCos(angles.Yaw, out sz, out cz);
				double sycz = (sy * cz), sysz = (sy * sz);
				Matrix34 mat = Matrix34.Identity;
				mat.M00 = (float)(cy * cz);
				mat.M01 = (float)(sycz * sx - cx * sz);
				mat.M02 = (float)(sycz * cx + sx * sz);
				mat.M10 = (float)(cy * sz);
				mat.M11 = (float)(sysz * sx + cx * cz);
				mat.M12 = (float)(sysz * cx - sx * cz);
				mat.M20 = (float)(-sy);
				mat.M21 = (float)(cy * sx);
				mat.M22 = (float)(cy * cx);
				return mat;
			}
			/// <summary>
			/// Creates a 4x4 matrix that represents rotation with Euler angles.
			/// </summary>
			/// <param name="angles">A set of Euler angles.</param>
			/// <returns>4x4 matrix that represents rotation with Euler angles.</returns>
			public static Matrix44 Create44(ref EulerAngles angles)
			{
				double sx, cx;
				MathHelpers.SinCos(angles.Pitch, out sx, out cx);
				double sy, cy;
				MathHelpers.SinCos(angles.Roll, out sy, out cy);
				double sz, cz;
				MathHelpers.SinCos(angles.Yaw, out sz, out cz);
				double sycz = (sy * cz), sysz = (sy * sz);
				Matrix44 mat = Matrix44.Identity;
				mat.M00 = (float)(cy * cz);
				mat.M01 = (float)(sycz * sx - cx * sz);
				mat.M02 = (float)(sycz * cx + sx * sz);
				mat.M10 = (float)(cy * sz);
				mat.M11 = (float)(sysz * sx + cx * cz);
				mat.M12 = (float)(sysz * cx - sx * cz);
				mat.M20 = (float)(-sy);
				mat.M21 = (float)(cy * sx);
				mat.M22 = (float)(cy * cx);
				return mat;
			}
			/// <summary>
			/// Creates quaternion that represents rotation defined by a set of Euler angles.
			/// </summary>
			/// <param name="angles">A set of Euler angles.</param>
			/// <returns>Quaternion that represents rotation defined by a set of Euler angles.</returns>
			public static Quaternion CreateQuaternion(ref EulerAngles angles)
			{
				float sx, cx;
				MathHelpers.SinCos(angles.Pitch * 0.5f, out sx, out cx);
				float sy, cy;
				MathHelpers.SinCos(angles.Roll * 0.5f, out sy, out cy);
				float sz, cz;
				MathHelpers.SinCos(angles.Yaw * 0.5f, out sz, out cz);
				return new Quaternion
					(
					cz * cy * sx - sz * sy * cx,
					cz * sy * cx + sz * cy * sx,
					sz * cy * cx - cz * sy * sx,
					cx * cy * cz + sx * sy * sz
					);
			}
		}
	}
}