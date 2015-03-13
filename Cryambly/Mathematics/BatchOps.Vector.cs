using System;

namespace CryCil
{
	public partial class BatchOps
	{
		/// <summary>
		/// Defines functions that perform patch operations on vectors.
		/// </summary>
		public static class Vector
		{
			/// <summary>
			/// Calculates dot products of corresponding pairs of vectors.
			/// </summary>
			/// <param name="vectors1">First array of vectors.</param>
			/// <param name="vectors2">Second array of vectors.</param>
			/// <returns>An array of dot products of given vectors.</returns>
			/// <exception cref="ArgumentException">
			/// Number of arguments in two arrays is not equal.
			/// </exception>
			public static float[] Dot(Vector2[] vectors1, Vector2[] vectors2)
			{
				if (vectors1 == null || vectors2 == null)
				{
					return null;
				}
				if (vectors1.LongLength != vectors2.LongLength)
				{
					throw new ArgumentException("Number of arguments in two arrays is not equal.");
				}

				float[] dotProducts = new float[vectors1.LongLength];
				for (long i = 0; i < vectors1.LongLength; i++)
				{
					dotProducts[i] = vectors1[i] * vectors2[i];
				}
				return dotProducts;
			}
			/// <summary>
			/// Calculates dot products of corresponding pairs of vectors.
			/// </summary>
			/// <param name="vectors1">First array of vectors.</param>
			/// <param name="vectors2">Second array of vectors.</param>
			/// <returns>An array of dot products of given vectors.</returns>
			/// <exception cref="ArgumentException">
			/// Number of arguments in two arrays is not equal.
			/// </exception>
			public static float[] Dot(Vector3[] vectors1, Vector3[] vectors2)
			{
				if (vectors1 == null || vectors2 == null)
				{
					return null;
				}
				if (vectors1.LongLength != vectors2.LongLength)
				{
					throw new ArgumentException("Number of arguments in two arrays is not equal.");
				}

				float[] dotProducts = new float[vectors1.LongLength];
				for (long i = 0; i < vectors1.LongLength; i++)
				{
					dotProducts[i] = vectors1[i] * vectors2[i];
				}
				return dotProducts;
			}
			/// <summary>
			/// Calculates cross products of corresponding pairs of vectors.
			/// </summary>
			/// <param name="vectors1">First array of vectors.</param>
			/// <param name="vectors2">Second array of vectors.</param>
			/// <returns>An array of cross products of given vectors.</returns>
			/// <exception cref="ArgumentException">
			/// Number of arguments in two arrays is not equal.
			/// </exception>
			public static Vector3[] Cross(Vector3[] vectors1, Vector3[] vectors2)
			{
				if (vectors1 == null || vectors2 == null)
				{
					return null;
				}
				if (vectors1.LongLength != vectors2.LongLength)
				{
					throw new ArgumentException("Number of arguments in two arrays is not equal.");
				}

				Vector3[] crossProducts = new Vector3[vectors1.LongLength];
				for (long i = 0; i < vectors1.LongLength; i++)
				{
					crossProducts[i] = vectors1[i] % vectors2[i];
				}
				return crossProducts;
			}
		}
	}
}