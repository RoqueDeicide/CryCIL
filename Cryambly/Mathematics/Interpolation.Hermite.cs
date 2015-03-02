using CryCil.Geometry;

namespace CryCil
{
	public static partial class Interpolation
	{
		/// <summary>
		/// Defines functions that perform cubic interpolation of values.
		/// </summary>
		/// <remarks>
		/// Cubic interpolation is similar to linear in principle but it uses cubic polynomial function
		/// instead of linear one.
		/// </remarks>
		public static class Hermite
		{
			/// <summary>
			/// Applies Hermite interpolation to the given vector.
			/// </summary>
			/// <param name="result">   Result of Hermite interpolation.</param>
			/// <param name="v1">       First vector.</param>
			/// <param name="v2">       Second vector.</param>
			/// <param name="t1">       First control vector.</param>
			/// <param name="t2">       Second control vector.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant vector on the line that goes through the
			/// first and second vector relative to the first one.
			/// </param>
			public static void Apply(out Vector2 result,
										 Vector2 v1, Vector2 v2,
										 Vector2 t1, Vector2 t2, float parameter)
			{
				float part1, part2, part3, part4;
				CalculateHermiteInterpolationFactors
					(parameter, out part1, out part2, out part3, out part4);

				result = new Vector2
				(
					v1.X * part1 + v2.X * part2 + t1.X * part3 + t2.X * part4,
					v1.Y * part1 + v2.Y * part2 + t1.Y * part3 + t2.Y * part4
				);
			}
			/// <summary>
			/// Creates a vector that is a result of Hermite interpolation.
			/// </summary>
			/// <param name="v1">       First vector.</param>
			/// <param name="v2">       Second vector.</param>
			/// <param name="t1">       First control vector.</param>
			/// <param name="t2">       Second control vector.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant vector on the line that goes through the
			/// first and second vector relative to the first one.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static Vector2 Create(Vector2 v1, Vector2 v2,
										 Vector2 t1, Vector2 t2, float parameter)
			{
				float part1, part2, part3, part4;
				CalculateHermiteInterpolationFactors
					(parameter, out part1, out part2, out part3, out part4);

				return new Vector2
				(
					v1.X * part1 + v2.X * part2 + t1.X * part3 + t2.X * part4,
					v1.Y * part1 + v2.Y * part2 + t1.Y * part3 + t2.Y * part4
				);
			}
			/// <summary>
			/// Applies Hermite interpolation to the given vector.
			/// </summary>
			/// <param name="result">   Result of Hermite interpolation.</param>
			/// <param name="v1">       First vector.</param>
			/// <param name="v2">       Second vector.</param>
			/// <param name="t1">       First control vector.</param>
			/// <param name="t2">       Second control vector.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant vector on the line that goes through the
			/// first and second vector relative to the first one.
			/// </param>
			public static void Apply(out Vector3 result,
										 Vector3 v1, Vector3 v2,
										 Vector3 t1, Vector3 t2, float parameter)
			{
				float part1, part2, part3, part4;
				CalculateHermiteInterpolationFactors
					(parameter, out part1, out part2, out part3, out part4);

				result = new Vector3
				(
					v1.X * part1 + v2.X * part2 + t1.X * part3 + t2.X * part4,
					v1.Y * part1 + v2.Y * part2 + t1.Y * part3 + t2.Y * part4,
					v1.Z * part1 + v2.Z * part2 + t1.Z * part3 + t2.Z * part4
				);
			}
			/// <summary>
			/// Creates a vector that is a result of Hermite interpolation.
			/// </summary>
			/// <param name="v1">       First vector.</param>
			/// <param name="v2">       Second vector.</param>
			/// <param name="t1">       First control vector.</param>
			/// <param name="t2">       Second control vector.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant vector on the line that goes through the
			/// first and second vector relative to the first one.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static Vector3 Create(Vector3 v1, Vector3 v2,
										 Vector3 t1, Vector3 t2, float parameter)
			{
				float part1, part2, part3, part4;
				CalculateHermiteInterpolationFactors
					(parameter, out part1, out part2, out part3, out part4);

				return new Vector3
				(
					v1.X * part1 + v2.X * part2 + t1.X * part3 + t2.X * part4,
					v1.Y * part1 + v2.Y * part2 + t1.Y * part3 + t2.Y * part4,
					v1.Z * part1 + v2.Z * part2 + t1.Z * part3 + t2.Z * part4
				);
			}
			/// <summary>
			/// Applies Hermite interpolation to the given vector.
			/// </summary>
			/// <param name="result">   Result of Hermite interpolation.</param>
			/// <param name="v1">       First vector.</param>
			/// <param name="v2">       Second vector.</param>
			/// <param name="t1">       First control vector.</param>
			/// <param name="t2">       Second control vector.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant vector on the line that goes through the
			/// first and second vector relative to the first one.
			/// </param>
			public static void Apply(out Vector4 result,
										 Vector4 v1, Vector4 v2,
										 Vector4 t1, Vector4 t2, float parameter)
			{
				float part1, part2, part3, part4;
				CalculateHermiteInterpolationFactors
					(parameter, out part1, out part2, out part3, out part4);

				result = new Vector4
				(
					v1.X * part1 + v2.X * part2 + t1.X * part3 + t2.X * part4,
					v1.Y * part1 + v2.Y * part2 + t1.Y * part3 + t2.Y * part4,
					v1.Z * part1 + v2.Z * part2 + t1.Z * part3 + t2.Z * part4,
					v1.W * part1 + v2.W * part2 + t1.W * part3 + t2.W * part4
				);
			}
			/// <summary>
			/// Creates a vector that is a result of Hermite interpolation.
			/// </summary>
			/// <param name="v1">       First vector.</param>
			/// <param name="v2">       Second vector.</param>
			/// <param name="t1">       First control vector.</param>
			/// <param name="t2">       Second control vector.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant vector on the line that goes through the
			/// first and second vector relative to the first one.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static Vector4 Create(Vector4 v1, Vector4 v2,
										 Vector4 t1, Vector4 t2, float parameter)
			{
				float part1, part2, part3, part4;
				CalculateHermiteInterpolationFactors
					(parameter, out part1, out part2, out part3, out part4);

				return new Vector4
				(
					v1.X * part1 + v2.X * part2 + t1.X * part3 + t2.X * part4,
					v1.Y * part1 + v2.Y * part2 + t1.Y * part3 + t2.Y * part4,
					v1.Z * part1 + v2.Z * part2 + t1.Z * part3 + t2.Z * part4,
					v1.W * part1 + v2.W * part2 + t1.W * part3 + t2.W * part4
				);
			}
			private static void CalculateHermiteInterpolationFactors(float parameter,
																	 out float part1, out float part2,
																	 out float part3, out float part4)
			{
				float squared = parameter * parameter;
				float cubed = parameter * squared;
				part1 = ((2.0f * cubed) - (3.0f * squared)) + 1.0f;
				part2 = (-2.0f * cubed) + (3.0f * squared);
				part3 = (cubed - (2.0f * squared)) + parameter;
				part4 = cubed - squared;
			}
		}
	}
}