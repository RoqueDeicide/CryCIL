using System;
using CryCil.Geometry;

namespace CryCil
{
	public static partial class Interpolation
	{
		/// <summary>
		/// Defines functions for working with spherical interpolations.
		/// </summary>
		/// <remarks>
		/// Spherical interpolations are primarily used when smooth rotation is required. It is costly, but
		/// its only less resource-hungry alternative called normalized linear interpolation (consists of
		/// linear interpolation followed up by normalization of the result) does not maintain constant
		/// speed when rotating.
		/// </remarks>
		public static class SphericalLinear
		{
			/// <summary>
			/// Applies spherical interpolation to the vector.
			/// </summary>
			/// <param name="result">   Result of interpolation.</param>
			/// <param name="first">    First vector.</param>
			/// <param name="second">   Second vector.</param>
			/// <param name="parameter">
			/// Scalar that defines orientation of resultant vector relative to orientation of the first
			/// vector relative to the second one.
			/// </param>
			public static void Apply(out Vector2 result, Vector2 first, Vector2 second, float parameter)
			{
				// calculate cosine using the "inner product" between two
				// vectors: p*q=cos(radiant)
				float cosine = MathHelpers.Clamp(Vector2.Dot(first, second), -1f, 1f);
				// Perform normalized linear interpolation if two vectors if they are very close to each
				// other to avoid division by zero.
				if (cosine >= 0.99f)
				{
					result = Interpolation.Linear.Create(first, second, parameter); //perform LERP:
					result.Normalize();
				}
				else
				{
					float rad = (float)Math.Acos(cosine);
					float scale0 = (float)Math.Sin((1 - parameter) * rad);
					float scale1 = (float)Math.Sin(parameter * rad);
					result = (first * scale0 + second * scale1) / (float)Math.Sin(rad);
					result.Normalize();
				}
			}
			/// <summary>
			/// Applies spherical interpolation to the vector.
			/// </summary>
			/// <param name="first">    First vector.</param>
			/// <param name="second">   Second vector.</param>
			/// <param name="parameter">
			/// Scalar that defines orientation of resultant vector relative to orientation of the first
			/// vector relative to the second one.
			/// </param>
			public static Vector2 Create(Vector2 first, Vector2 second, float parameter)
			{
				Vector2 result;
				// calculate cosine using the "inner product" between two
				// vectors: p*q=cos(radiant)
				float cosine = MathHelpers.Clamp(Vector2.Dot(first, second), -1f, 1f);
				// Perform normalized linear interpolation if two vectors if they are very close to each
				// other to avoid division by zero.
				if (cosine >= 0.99f)
				{
					result = Interpolation.Linear.Create(first, second, parameter); //perform LERP:
					result.Normalize();
				}
				else
				{
					float rad = (float)Math.Acos(cosine);
					float scale0 = (float)Math.Sin((1 - parameter) * rad);
					float scale1 = (float)Math.Sin(parameter * rad);
					result = (first * scale0 + second * scale1) / (float)Math.Sin(rad);
					result.Normalize();
				}
				return result;
			}
			/// <summary>
			/// Applies spherical interpolation to the vector.
			/// </summary>
			/// <param name="result">   Result of interpolation.</param>
			/// <param name="first">    First vector.</param>
			/// <param name="second">   Second vector.</param>
			/// <param name="parameter">
			/// Scalar that defines orientation of resultant vector relative to orientation of the first
			/// vector relative to the second one.
			/// </param>
			public static void Apply(out Vector3 result, Vector3 first, Vector3 second, float parameter)
			{
				// calculate cosine using the "inner product" between two
				// vectors: p*q=cos(radiant)
				float cosine = MathHelpers.Clamp(Vector3.Dot(first, second), -1f, 1f);
				// Perform normalized linear interpolation if two vectors if they are very close to each
				// other to avoid division by zero.
				if (cosine >= 0.99f)
				{
					result = Interpolation.Linear.Create(first, second, parameter); //perform LERP:
					result.Normalize();
				}
				else
				{
					float rad = (float)Math.Acos(cosine);
					float scale0 = (float)Math.Sin((1 - parameter) * rad);
					float scale1 = (float)Math.Sin(parameter * rad);
					result = (first * scale0 + second * scale1) / (float)Math.Sin(rad);
					result.Normalize();
				}
			}
			/// <summary>
			/// Applies spherical interpolation to the vector.
			/// </summary>
			/// <param name="first">    First vector.</param>
			/// <param name="second">   Second vector.</param>
			/// <param name="parameter">
			/// Scalar that defines orientation of resultant vector relative to orientation of the first
			/// vector relative to the second one.
			/// </param>
			public static Vector3 Create(Vector3 first, Vector3 second, float parameter)
			{
				Vector3 result;
				// calculate cosine using the "inner product" between two
				// vectors: p*q=cos(radiant)
				float cosine = MathHelpers.Clamp(Vector3.Dot(first, second), -1f, 1f);
				// Perform normalized linear interpolation if two vectors if they are very close to each
				// other to avoid division by zero.
				if (cosine >= 0.99f)
				{
					result = Interpolation.Linear.Create(first, second, parameter); //perform LERP:
					result.Normalize();
				}
				else
				{
					float rad = (float)Math.Acos(cosine);
					float scale0 = (float)Math.Sin((1 - parameter) * rad);
					float scale1 = (float)Math.Sin(parameter * rad);
					result = (first * scale0 + second * scale1) / (float)Math.Sin(rad);
					result.Normalize();
				}
				return result;
			}
			/// <summary>
			/// Applies spherical interpolation to the vector.
			/// </summary>
			/// <param name="result">   Result of interpolation.</param>
			/// <param name="first">    First quaternion.</param>
			/// <param name="second">   Second quaternion.</param>
			/// <param name="parameter">
			/// Scalar that defines orientation of resultant vector relative to orientation of the first
			/// vector relative to the second one.
			/// </param>
			public static void Apply(out Quaternion result, Quaternion first, Quaternion second, float parameter)
			{
				var q2 = new Quaternion();

				var cosine = (first | second);
				if (cosine < 0.0f) // take shortest arc
				{
					cosine = -cosine;
					second = second.Flipped;
				}
				if (cosine > 0.9999f)
				{
					result = Interpolation.Linear.Create(first, second, parameter);
					result.Normalize();
					return;
				}
				// from now on, a division by 0 is not possible any more
				q2.W = second.W - first.W * cosine;
				q2.X = second.X - first.X * cosine;
				q2.Y = second.Y - first.Y * cosine;
				q2.Z = second.Z - first.Z * cosine;
				var sine = Math.Sqrt(q2 | q2);
				double s, c;

				MathHelpers.SinCos(Math.Atan2(sine, cosine) * parameter, out s, out c);
				result = new Quaternion
				{
					W = (float)(first.W * c + q2.W * s / sine),
					X = (float)(first.X * c + q2.X * s / sine),
					Y = (float)(first.Y * c + q2.Y * s / sine),
					Z = (float)(first.Z * c + q2.Z * s / sine)
				};
			}
			/// <summary>
			/// Applies spherical interpolation to the vector.
			/// </summary>
			/// <param name="first">    First vector.</param>
			/// <param name="second">   Second vector.</param>
			/// <param name="parameter">
			/// Scalar that defines orientation of resultant vector relative to orientation of the first
			/// vector relative to the second one.
			/// </param>
			public static Quaternion Create(Quaternion first, Quaternion second, float parameter)
			{
				Quaternion result;
				var q2 = new Quaternion();

				var cosine = (first | second);
				if (cosine < 0.0f) // take shortest arc
				{
					cosine = -cosine;
					second = second.Flipped;
				}
				if (cosine > 0.9999f)
				{
					result = Interpolation.Linear.Create(first, second, parameter);
					result.Normalize();
				}
				else
				{
					// from now on, a division by 0 is not possible any more
					q2.W = second.W - first.W * cosine;
					q2.X = second.X - first.X * cosine;
					q2.Y = second.Y - first.Y * cosine;
					q2.Z = second.Z - first.Z * cosine;
					var sine = Math.Sqrt(q2 | q2);
					double s, c;

					MathHelpers.SinCos(Math.Atan2(sine, cosine) * parameter, out s, out c);
					result = new Quaternion
					{
						W = (float)(first.W * c + q2.W * s / sine),
						X = (float)(first.X * c + q2.X * s / sine),
						Y = (float)(first.Y * c + q2.Y * s / sine),
						Z = (float)(first.Z * c + q2.Z * s / sine)
					};
				}
				return result;
			}
			/// <summary>
			/// Creates a matrix that represents a spherical linear interpolation from one matrix to
			/// another.
			/// </summary>
			/// <remarks>
			/// <para>This is an implementation of interpolation without quaternions.</para>
			/// <para>
			/// Given two orthonormal 3x3 matrices this function calculates the shortest possible
			/// interpolation-path between the two rotations. The interpolation curve forms the shortest
			/// great arc on the rotation sphere (geodesic).
			/// </para>
			/// <para>Angular velocity of the interpolation is constant.</para>
			/// <para>Possible stability problems:</para>
			/// <para>
			/// There are two singularities at angle = 0 and angle = <see cref="Math.PI"/> . At 0 the
			/// interpolation-axis is arbitrary, which means any axis will produce the same result because
			/// we have no rotation. (1,0,0) axis is used in this case. At <see cref="Math.PI"/> the
			/// rotations point away from each other and the interpolation-axis is unpredictable. In this
			/// case axis (1,0,0) is used as well. If the angle is ~0 or ~PI, then a very small vector has
			/// to be normalized and this can cause numerical instability.
			/// </para>
			/// </remarks>
			/// <param name="matrix">Result of interpolation.</param>
			/// <param name="first"> First matrix.</param>
			/// <param name="second">Second matrix.</param>
			/// <param name="t">     Interpolation parameter.</param>
			public static void Apply(ref Matrix34 matrix, Matrix34 first, Matrix34 second, float t)
			{
				// calculate delta-rotation between m and n (=39 flops)
				Matrix33 d = new Matrix33(), i = new Matrix33();
				d.M00 = first.M00 * second.M00 + first.M10 * second.M10 + first.M20 * second.M20;
				d.M01 = first.M00 * second.M01 + first.M10 * second.M11 + first.M20 * second.M21;
				d.M02 = first.M00 * second.M02 + first.M10 * second.M12 + first.M20 * second.M22;
				d.M10 = first.M01 * second.M00 + first.M11 * second.M10 + first.M21 * second.M20;
				d.M11 = first.M01 * second.M01 + first.M11 * second.M11 + first.M21 * second.M21;
				d.M12 = first.M01 * second.M02 + first.M11 * second.M12 + first.M21 * second.M22;
				d.M20 = d.M01 * d.M12 - d.M02 * d.M11;
				d.M21 = d.M02 * d.M10 - d.M00 * d.M12;
				d.M22 = d.M00 * d.M11 - d.M01 * d.M10;

				// extract angle and axis
				double cosine = MathHelpers.Clamp((d.M00 + d.M11 + d.M22 - 1.0) * 0.5, -1.0, +1.0);
				double angle = Math.Atan2(Math.Sqrt(1.0 - cosine * cosine), cosine);
				var axis = new Vector3(d.M21 - d.M12, d.M02 - d.M20, d.M10 - d.M01);
				double l = Math.Sqrt(axis * axis);
				if (l > 0.00001) axis /= (float)l;
				else axis = new Vector3(1, 0, 0);
				Rotation.AroundAxis.Set(ref i, ref axis, (float)angle * t);
					// angle interpolation and calculation of new delta-matrix (=26 flops)

				// final concatenation (=39 flops)
				matrix.M00 = first.M00 * i.M00 + first.M01 * i.M10 + first.M02 * i.M20;
				matrix.M01 = first.M00 * i.M01 + first.M01 * i.M11 + first.M02 * i.M21;
				matrix.M02 = first.M00 * i.M02 + first.M01 * i.M12 + first.M02 * i.M22;
				matrix.M10 = first.M10 * i.M00 + first.M11 * i.M10 + first.M12 * i.M20;
				matrix.M11 = first.M10 * i.M01 + first.M11 * i.M11 + first.M12 * i.M21;
				matrix.M12 = first.M10 * i.M02 + first.M11 * i.M12 + first.M12 * i.M22;
				matrix.M20 = matrix.M01 * matrix.M12 - matrix.M02 * matrix.M11;
				matrix.M21 = matrix.M02 * matrix.M10 - matrix.M00 * matrix.M12;
				matrix.M22 = matrix.M00 * matrix.M11 - matrix.M01 * matrix.M10;

				matrix.M03 = first.M03 * (1 - t) + second.M03 * t;
				matrix.M13 = first.M13 * (1 - t) + second.M13 * t;
				matrix.M23 = first.M23 * (1 - t) + second.M23 * t;
			}
			/// <summary>
			/// Creates a matrix that represents a spherical linear interpolation from one matrix to
			/// another.
			/// </summary>
			/// <remarks>
			/// <para>This is an implementation of interpolation without quaternions.</para>
			/// <para>
			/// Given two orthonormal 3x3 matrices this function calculates the shortest possible
			/// interpolation-path between the two rotations. The interpolation curve forms the shortest
			/// great arc on the rotation sphere (geodesic).
			/// </para>
			/// <para>Angular velocity of the interpolation is constant.</para>
			/// <para>Possible stability problems:</para>
			/// <para>
			/// There are two singularities at angle = 0 and angle = <see cref="Math.PI"/> . At 0 the
			/// interpolation-axis is arbitrary, which means any axis will produce the same result because
			/// we have no rotation. (1,0,0) axis is used in this case. At <see cref="Math.PI"/> the
			/// rotations point away from each other and the interpolation-axis is unpredictable. In this
			/// case axis (1,0,0) is used as well. If the angle is ~0 or ~PI, then a very small vector has
			/// to be normalized and this can cause numerical instability.
			/// </para>
			/// </remarks>
			/// <param name="first"> First matrix.</param>
			/// <param name="second">Second matrix.</param>
			/// <param name="t">     Interpolation parameter.</param>
			/// <returns>Matrix which transformations are interpolated between given matrices.</returns>
			public static Matrix34 Create(Matrix34 first, Matrix34 second, float t)
			{
				var matrix = new Matrix34();
				Apply(ref matrix, first, second, t);

				return matrix;
			}
			/// <summary>
			/// Creates a matrix that represents a spherical linear interpolation from one matrix to
			/// another.
			/// </summary>
			/// <remarks>
			/// <para>This is an implementation of interpolation without quaternions.</para>
			/// <para>
			/// Given two orthonormal 3x3 matrices this function calculates the shortest possible
			/// interpolation-path between the two rotations. The interpolation curve forms the shortest
			/// great arc on the rotation sphere (geodesic).
			/// </para>
			/// <para>Angular velocity of the interpolation is constant.</para>
			/// <para>Possible stability problems:</para>
			/// <para>
			/// There are two singularities at angle = 0 and angle = <see cref="Math.PI"/> . At 0 the
			/// interpolation-axis is arbitrary, which means any axis will produce the same result because
			/// we have no rotation. (1,0,0) axis is used in this case. At <see cref="Math.PI"/> the
			/// rotations point away from each other and the interpolation-axis is unpredictable. In this
			/// case axis (1,0,0) is used as well. If the angle is ~0 or ~PI, then a very small vector has
			/// to be normalized and this can cause numerical instability.
			/// </para>
			/// </remarks>
			/// <param name="matrix">Result of interpolation.</param>
			/// <param name="first"> First matrix.</param>
			/// <param name="second">Second matrix.</param>
			/// <param name="t">     Interpolation parameter.</param>
			public static void Apply(ref Matrix33 matrix, Matrix33 first, Matrix33 second, float t)
			{
				// calculate delta-rotation between m and n (=39 flops)
				Matrix33 d = new Matrix33(), i = new Matrix33();
				d.M00 = first.M00 * second.M00 + first.M10 * second.M10 + first.M20 * second.M20;
				d.M01 = first.M00 * second.M01 + first.M10 * second.M11 + first.M20 * second.M21;
				d.M02 = first.M00 * second.M02 + first.M10 * second.M12 + first.M20 * second.M22;
				d.M10 = first.M01 * second.M00 + first.M11 * second.M10 + first.M21 * second.M20;
				d.M11 = first.M01 * second.M01 + first.M11 * second.M11 + first.M21 * second.M21;
				d.M12 = first.M01 * second.M02 + first.M11 * second.M12 + first.M21 * second.M22;
				d.M20 = d.M01 * d.M12 - d.M02 * d.M11;
				d.M21 = d.M02 * d.M10 - d.M00 * d.M12;
				d.M22 = d.M00 * d.M11 - d.M01 * d.M10;

				// extract angle and axis
				double cosine = MathHelpers.Clamp((d.M00 + d.M11 + d.M22 - 1.0) * 0.5, -1.0, +1.0);
				double angle = Math.Atan2(Math.Sqrt(1.0 - cosine * cosine), cosine);
				var axis = new Vector3(d.M21 - d.M12, d.M02 - d.M20, d.M10 - d.M01);
				double l = Math.Sqrt(axis * axis);
				if (l > 0.00001) axis /= (float)l;
				else axis = new Vector3(1, 0, 0);
				Rotation.AroundAxis.Set(ref i, ref axis, (float)angle * t);
					// angle interpolation and calculation of new delta-matrix (=26 flops)

				// final concatenation (=39 flops)
				matrix.M00 = first.M00 * i.M00 + first.M01 * i.M10 + first.M02 * i.M20;
				matrix.M01 = first.M00 * i.M01 + first.M01 * i.M11 + first.M02 * i.M21;
				matrix.M02 = first.M00 * i.M02 + first.M01 * i.M12 + first.M02 * i.M22;
				matrix.M10 = first.M10 * i.M00 + first.M11 * i.M10 + first.M12 * i.M20;
				matrix.M11 = first.M10 * i.M01 + first.M11 * i.M11 + first.M12 * i.M21;
				matrix.M12 = first.M10 * i.M02 + first.M11 * i.M12 + first.M12 * i.M22;
				matrix.M20 = matrix.M01 * matrix.M12 - matrix.M02 * matrix.M11;
				matrix.M21 = matrix.M02 * matrix.M10 - matrix.M00 * matrix.M12;
				matrix.M22 = matrix.M00 * matrix.M11 - matrix.M01 * matrix.M10;
			}
			/// <summary>
			/// Creates a matrix that represents a spherical linear interpolation from one matrix to
			/// another.
			/// </summary>
			/// <remarks>
			/// <para>This is an implementation of interpolation without quaternions.</para>
			/// <para>
			/// Given two orthonormal 3x3 matrices this function calculates the shortest possible
			/// interpolation-path between the two rotations. The interpolation curve forms the shortest
			/// great arc on the rotation sphere (geodesic).
			/// </para>
			/// <para>Angular velocity of the interpolation is constant.</para>
			/// <para>Possible stability problems:</para>
			/// <para>
			/// There are two singularities at angle = 0 and angle = <see cref="Math.PI"/> . At 0 the
			/// interpolation-axis is arbitrary, which means any axis will produce the same result because
			/// we have no rotation. (1,0,0) axis is used in this case. At <see cref="Math.PI"/> the
			/// rotations point away from each other and the interpolation-axis is unpredictable. In this
			/// case axis (1,0,0) is used as well. If the angle is ~0 or ~PI, then a very small vector has
			/// to be normalized and this can cause numerical instability.
			/// </para>
			/// </remarks>
			/// <param name="first"> First matrix.</param>
			/// <param name="second">Second matrix.</param>
			/// <param name="t">     Interpolation parameter.</param>
			/// <returns>Matrix which transformations are interpolated between given matrices.</returns>
			public static Matrix33 Create(Matrix33 first, Matrix33 second, float t)
			{
				var matrix = new Matrix33();
				Apply(ref matrix, first, second, t);

				return matrix;
			}
		}
	}
}