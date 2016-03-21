using System;
using System.Linq;

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
		public static class CatmullRom
		{
			/// <summary>
			/// Applies Catmull-Rom interpolation to the given vector.
			/// </summary>
			/// <param name="result">   Result of Catmull-Rom interpolation.</param>
			/// <param name="v1">       First vector.</param>
			/// <param name="v2">       Second vector.</param>
			/// <param name="v3">       Third vector.</param>
			/// <param name="v4">       Fourth vector.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant vector on the line that goes through the
			/// first and second vector relative to the first one.
			/// </param>
			public static void Apply(out Vector2 result,
									 Vector2 v1, Vector2 v2,
									 Vector2 v3, Vector2 v4, float parameter)
			{
				float squared = parameter * parameter;
				float cubed = parameter * squared;

				result = new Vector2(0.5f * (2.0f * v2.X + (-v1.X + v3.X) * parameter +
											 (2.0f * v1.X - 5.0f * v2.X + 4.0f * v3.X - v4.X) * squared +
											 (-v1.X + 3.0f * v2.X - 3.0f * v3.X + v4.X) * cubed),
									 0.5f * (2.0f * v2.Y + (-v1.Y + v3.Y) * parameter +
											 (2.0f * v1.Y - 5.0f * v2.Y + 4.0f * v3.Y - v4.Y) * squared +
											 (-v1.Y + 3.0f * v2.Y - 3.0f * v3.Y + v4.Y) * cubed));
			}
			/// <summary>
			/// Creates a vector that is a result of Catmull-Rom interpolation.
			/// </summary>
			/// <param name="v1">       First vector.</param>
			/// <param name="v2">       Second vector.</param>
			/// <param name="v3">       Third vector.</param>
			/// <param name="v4">       Fourth vector.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant vector on the line that goes through the
			/// first and second vector relative to the first one.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static Vector2 Create(Vector2 v1, Vector2 v2,
										 Vector2 v3, Vector2 v4, float parameter)
			{
				float squared = parameter * parameter;
				float cubed = parameter * squared;

				return new Vector2(0.5f * (2.0f * v2.X + (-v1.X + v3.X) * parameter +
										   (2.0f * v1.X - 5.0f * v2.X + 4.0f * v3.X - v4.X) * squared +
										   (-v1.X + 3.0f * v2.X - 3.0f * v3.X + v4.X) * cubed),
								   0.5f * (2.0f * v2.Y + (-v1.Y + v3.Y) * parameter +
										   (2.0f * v1.Y - 5.0f * v2.Y + 4.0f * v3.Y - v4.Y) * squared +
										   (-v1.Y + 3.0f * v2.Y - 3.0f * v3.Y + v4.Y) * cubed));
			}
			/// <summary>
			/// Applies Catmull-Rom interpolation to the given vector.
			/// </summary>
			/// <param name="result">   Result of Catmull-Rom interpolation.</param>
			/// <param name="v1">       First vector.</param>
			/// <param name="v2">       Second vector.</param>
			/// <param name="v3">       Third vector.</param>
			/// <param name="v4">       Fourth vector.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant vector on the line that goes through the
			/// first and second vector relative to the first one.
			/// </param>
			public static void Apply(out Vector3 result,
									 Vector3 v1, Vector3 v2,
									 Vector3 v3, Vector3 v4, float parameter)
			{
				float squared = parameter * parameter;
				float cubed = parameter * squared;

				result = new Vector3(0.5f * (2.0f * v2.X + (-v1.X + v3.X) * parameter +
											 (2.0f * v1.X - 5.0f * v2.X + 4.0f * v3.X - v4.X) * squared +
											 (-v1.X + 3.0f * v2.X - 3.0f * v3.X + v4.X) * cubed),
									 0.5f * (2.0f * v2.Y + (-v1.Y + v3.Y) * parameter +
											 (2.0f * v1.Y - 5.0f * v2.Y + 4.0f * v3.Y - v4.Y) * squared +
											 (-v1.Y + 3.0f * v2.Y - 3.0f * v3.Y + v4.Y) * cubed),
									 0.5f * (2.0f * v2.Z + (-v1.Z + v3.Z) * parameter +
											 (2.0f * v1.Z - 5.0f * v2.Z + 4.0f * v3.Z - v4.Z) * squared +
											 (-v1.Z + 3.0f * v2.Z - 3.0f * v3.Z + v4.Z) * cubed));
			}
			/// <summary>
			/// Creates a vector that is a result of Catmull-Rom interpolation.
			/// </summary>
			/// <param name="v1">       First vector.</param>
			/// <param name="v2">       Second vector.</param>
			/// <param name="v3">       Third vector.</param>
			/// <param name="v4">       Fourth vector.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant vector on the line that goes through the
			/// first and second vector relative to the first one.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static Vector3 Create(Vector3 v1, Vector3 v2,
										 Vector3 v3, Vector3 v4, float parameter)
			{
				float squared = parameter * parameter;
				float cubed = parameter * squared;

				return new Vector3(0.5f * (2.0f * v2.X + (-v1.X + v3.X) * parameter +
										   (2.0f * v1.X - 5.0f * v2.X + 4.0f * v3.X - v4.X) * squared +
										   (-v1.X + 3.0f * v2.X - 3.0f * v3.X + v4.X) * cubed),
								   0.5f * (2.0f * v2.Y + (-v1.Y + v3.Y) * parameter +
										   (2.0f * v1.Y - 5.0f * v2.Y + 4.0f * v3.Y - v4.Y) * squared +
										   (-v1.Y + 3.0f * v2.Y - 3.0f * v3.Y + v4.Y) * cubed),
								   0.5f * (2.0f * v2.Z + (-v1.Z + v3.Z) * parameter +
										   (2.0f * v1.Z - 5.0f * v2.Z + 4.0f * v3.Z - v4.Z) * squared +
										   (-v1.Z + 3.0f * v2.Z - 3.0f * v3.Z + v4.Z) * cubed));
			}
			/// <summary>
			/// Applies Catmull-Rom interpolation to the given vector.
			/// </summary>
			/// <param name="result">   Result of Catmull-Rom interpolation.</param>
			/// <param name="v1">       First vector.</param>
			/// <param name="v2">       Second vector.</param>
			/// <param name="v3">       Third vector.</param>
			/// <param name="v4">       Fourth vector.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant vector on the line that goes through the
			/// first and second vector relative to the first one.
			/// </param>
			public static void Apply(out Vector4 result,
									 Vector4 v1, Vector4 v2,
									 Vector4 v3, Vector4 v4, float parameter)
			{
				float squared = parameter * parameter;
				float cubed = parameter * squared;

				result = new Vector4(0.5f * (2.0f * v2.X + (-v1.X + v3.X) * parameter +
											 (2.0f * v1.X - 5.0f * v2.X + 4.0f * v3.X - v4.X) * squared +
											 (-v1.X + 3.0f * v2.X - 3.0f * v3.X + v4.X) * cubed),
									 0.5f * (2.0f * v2.Y + (-v1.Y + v3.Y) * parameter +
											 (2.0f * v1.Y - 5.0f * v2.Y + 4.0f * v3.Y - v4.Y) * squared +
											 (-v1.Y + 3.0f * v2.Y - 3.0f * v3.Y + v4.Y) * cubed),
									 0.5f * (2.0f * v2.Z + (-v1.Z + v3.Z) * parameter +
											 (2.0f * v1.Z - 5.0f * v2.Z + 4.0f * v3.Z - v4.Z) * squared +
											 (-v1.Z + 3.0f * v2.Z - 3.0f * v3.Z + v4.Z) * cubed),
									 0.5f * (2.0f * v2.W + (-v1.W + v3.W) * parameter +
											 (2.0f * v1.W - 5.0f * v2.W + 4.0f * v3.W - v4.W) * squared +
											 (-v1.W + 3.0f * v2.W - 3.0f * v3.W + v4.W) * cubed));
			}
			/// <summary>
			/// Creates a vector that is a result of Catmull-Rom interpolation.
			/// </summary>
			/// <param name="v1">       First vector.</param>
			/// <param name="v2">       Second vector.</param>
			/// <param name="v3">       Third vector.</param>
			/// <param name="v4">       Fourth vector.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant vector on the line that goes through the
			/// first and second vector relative to the first one.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static Vector4 Create(Vector4 v1, Vector4 v2,
										 Vector4 v3, Vector4 v4, float parameter)
			{
				float squared = parameter * parameter;
				float cubed = parameter * squared;

				return new Vector4(0.5f * (2.0f * v2.X + (-v1.X + v3.X) * parameter +
										   (2.0f * v1.X - 5.0f * v2.X + 4.0f * v3.X - v4.X) * squared +
										   (-v1.X + 3.0f * v2.X - 3.0f * v3.X + v4.X) * cubed),
								   0.5f * (2.0f * v2.Y + (-v1.Y + v3.Y) * parameter +
										   (2.0f * v1.Y - 5.0f * v2.Y + 4.0f * v3.Y - v4.Y) * squared +
										   (-v1.Y + 3.0f * v2.Y - 3.0f * v3.Y + v4.Y) * cubed),
								   0.5f * (2.0f * v2.Z + (-v1.Z + v3.Z) * parameter +
										   (2.0f * v1.Z - 5.0f * v2.Z + 4.0f * v3.Z - v4.Z) * squared +
										   (-v1.Z + 3.0f * v2.Z - 3.0f * v3.Z + v4.Z) * cubed),
								   0.5f * (2.0f * v2.W + (-v1.W + v3.W) * parameter +
										   (2.0f * v1.W - 5.0f * v2.W + 4.0f * v3.W - v4.W) * squared +
										   (-v1.W + 3.0f * v2.W - 3.0f * v3.W + v4.W) * cubed));
			}
		}
	}
}