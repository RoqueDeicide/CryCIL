using System;
using System.Diagnostics.Contracts;

namespace CryCil.Geometry
{
	/// <summary>
	/// Defines functions that are used when there is a need to perform rotation affine transformation.
	/// </summary>
	public static partial class Rotation
	{
		/// <summary>
		/// Calculates rotation between 2 normalized vectors.
		/// </summary>
		/// <remarks>
		/// If given two vectors are collinear and opposite, the simplest vector that is perpendicular to
		/// first vector will be chosen.
		/// </remarks>
		/// <param name="v1">First vector.</param>
		/// <param name="v2">Second vector.</param>
		/// <returns>
		/// Instance of type <see cref="AngleAxis"/> that represents axis and angle of rotation from first
		/// vector to second along the shortest path.
		/// </returns>
		public static AngleAxis ArcBetween2NormalizedVectors(Vector3 v1, Vector3 v2)
		{
			float cosine = v1 | v2;

			if (cosine + 1 < MathHelpers.ZeroTolerance)
			{
				return new AngleAxis { Vector = (v1.SelectiveOrthogonal ?? v2.SelectiveOrthogonal ?? Vector3.Up) * (float)Math.PI };
			}
			return
				cosine - 1 < MathHelpers.ZeroTolerance
				? new AngleAxis()
				: new AngleAxis { Vector = (v1 % v2) * (float)Math.Acos(cosine) };
		}
		/// <summary>
		/// Calculates rotation between 2 vectors.
		/// </summary>
		/// <remarks>
		/// If given two vectors are collinear and opposite, the simplest vector that is perpendicular to
		/// first vector will be chosen.
		/// </remarks>
		/// <param name="vector1">First vector.</param>
		/// <param name="vector2">Second vector.</param>
		/// <returns>
		/// Instance of type <see cref="AngleAxis"/> that represents axis and angle of rotation from first
		/// vector to second along the shortest path.
		/// </returns>
		public static AngleAxis ArcBetween2Vectors(Vector3 vector1, Vector3 vector2)
		{
			Vector3 v1 = vector1.Normalized;
			Vector3 v2 = vector2.Normalized;

			float cosine = v1 | v2;

			if (cosine + 1 < MathHelpers.ZeroTolerance)
			{
				return new AngleAxis { Vector = (v1.SelectiveOrthogonal ?? v2.SelectiveOrthogonal ?? Vector3.Up) * (float)Math.PI };
			}
			return
				cosine - 1 < MathHelpers.ZeroTolerance
				? new AngleAxis()
				: new AngleAxis { Vector = (v1 % v2) * (float)Math.Acos(cosine) };
		}
		/// <summary>
		/// Calculates rotation between 2 normalized vectors.
		/// </summary>
		/// <remarks>
		/// If given two vectors are collinear and opposite, the simplest vector that is perpendicular to
		/// first vector will be chosen.
		/// </remarks>
		/// <param name="v1">    First vector.</param>
		/// <param name="v2">    Second vector.</param>
		/// <param name="axis">  Resultant axis of rotation.</param>
		/// <param name="cosine">Cosine of angle between two vectors.</param>
		public static void ArcBetween2NormalizedVectors(Vector3 v1, Vector3 v2, out Vector3 axis, out float cosine)
		{
			cosine = v1 | v2;

			if (cosine + 1 < MathHelpers.ZeroTolerance)
			{
				axis = v1.SelectiveOrthogonal ?? v2.SelectiveOrthogonal ?? Vector3.Up;
			}
			else if (cosine - 1 < MathHelpers.ZeroTolerance)
			{
				axis = Vector3.Up;
			}
			else
			{
				axis = v1 % v2;
			}
		}
		/// <summary>
		/// Calculates rotation between 2 vectors.
		/// </summary>
		/// <remarks>
		/// If given two vectors are collinear and opposite, the simplest vector that is perpendicular to
		/// first vector will be chosen.
		/// </remarks>
		/// <param name="vector1">First vector.</param>
		/// <param name="vector2">Second vector.</param>
		/// <param name="axis">   Resultant axis of rotation.</param>
		/// <param name="cosine"> Cosine of angle between two vectors.</param>
		public static void ArcBetween2Vectors(Vector3 vector1, Vector3 vector2, out Vector3 axis, out float cosine)
		{
			Vector3 v1 = vector1.Normalized;
			Vector3 v2 = vector2.Normalized;

			cosine = v1 | v2;

			if (cosine + 1 < MathHelpers.ZeroTolerance)
			{
				axis = v1.SelectiveOrthogonal ?? v2.SelectiveOrthogonal ?? Vector3.Up;
			}
			else if (cosine - 1 < MathHelpers.ZeroTolerance)
			{
				axis = Vector3.Up;
			}
			else
			{
				axis = v1 % v2;
			}
		}
	}
}