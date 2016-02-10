using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using CryCil.RunTime;

namespace CryCil
{
	/// <summary>
	/// Represents a 3D vector that uses double precision floating-point numbers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3Double
	{
		/// <summary>
		/// X-coordinate.
		/// </summary>
		public double X;
		/// <summary>
		/// Y-coordinate.
		/// </summary>
		public double Y;
		/// <summary>
		/// Z-coordinate.
		/// </summary>
		public double Z;
		/// <summary>
		/// Creates new instance of type <see cref="Vector3Double"/>.
		/// </summary>
		/// <param name="x">Number that initializes X-coordinate of the vector.</param>
		/// <param name="y">Number that initializes Y-coordinate of the vector.</param>
		/// <param name="z">Number that initializes Z-coordinate of the vector.</param>
		public Vector3Double(double x, double y, double z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}
	}
	public partial class BatchOps
	{
		/// <summary>
		/// Defines functions that perform math operations in batches.
		/// </summary>
		public static unsafe class Math
		{
			/// <summary>
			/// Calculates sines of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void Sine(float[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (float* ptr = numbers)
				{
					MathSimpleOpSingle(ptr, numbers.LongLength, MathSimpleOperations.Sine);
				}
			}
			/// <summary>
			/// Calculates sines of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void Sine(float[] source, out float[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new float[source.LongLength];
				CopyToResults(source, results);

				fixed (float* ptr = results)
				{
					MathSimpleOpSingle(ptr, source.LongLength, MathSimpleOperations.Sine);
				}
			}
			/// <summary>
			/// Calculates sines of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void Sine(double[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (double* ptr = numbers)
				{
					MathSimpleOpDouble(ptr, numbers.LongLength, MathSimpleOperations.Sine);
				}
			}
			/// <summary>
			/// Calculates sines of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void Sine(double[] source, out double[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new double[source.LongLength];
				CopyToResults(source, results);

				fixed (double* ptr = results)
				{
					MathSimpleOpDouble(ptr, source.LongLength, MathSimpleOperations.Sine);
				}
			}
			/// <summary>
			/// Calculates cosines of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void Cosine(float[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (float* ptr = numbers)
				{
					MathSimpleOpSingle(ptr, numbers.LongLength, MathSimpleOperations.Cosine);
				}
			}
			/// <summary>
			/// Calculates cosines of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void Cosine(float[] source, out float[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new float[source.LongLength];
				CopyToResults(source, results);

				fixed (float* ptr = results)
				{
					MathSimpleOpSingle(ptr, source.LongLength, MathSimpleOperations.Cosine);
				}
			}
			/// <summary>
			/// Calculates cosines of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void Cosine(double[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (double* ptr = numbers)
				{
					MathSimpleOpDouble(ptr, numbers.LongLength, MathSimpleOperations.Cosine);
				}
			}
			/// <summary>
			/// Calculates cosines of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void Cosine(double[] source, out double[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new double[source.LongLength];
				CopyToResults(source, results);

				fixed (double* ptr = results)
				{
					MathSimpleOpDouble(ptr, source.LongLength, MathSimpleOperations.Cosine);
				}
			}
			/// <summary>
			/// Calculates tangents of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void Tangent(float[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (float* ptr = numbers)
				{
					MathSimpleOpSingle(ptr, numbers.LongLength, MathSimpleOperations.Tangent);
				}
			}
			/// <summary>
			/// Calculates tangents of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void Tangent(float[] source, out float[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new float[source.LongLength];
				CopyToResults(source, results);

				fixed (float* ptr = results)
				{
					MathSimpleOpSingle(ptr, source.LongLength, MathSimpleOperations.Tangent);
				}
			}
			/// <summary>
			/// Calculates tangents of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void Tangent(double[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (double* ptr = numbers)
				{
					MathSimpleOpDouble(ptr, numbers.LongLength, MathSimpleOperations.Tangent);
				}
			}
			/// <summary>
			/// Calculates tangents of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void Tangent(double[] source, out double[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new double[source.LongLength];
				CopyToResults(source, results);

				fixed (double* ptr = results)
				{
					MathSimpleOpDouble(ptr, source.LongLength, MathSimpleOperations.Tangent);
				}
			}
			/// <summary>
			/// Calculates cotangents of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void Cotangent(float[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (float* ptr = numbers)
				{
					MathSimpleOpSingle(ptr, numbers.LongLength, MathSimpleOperations.Cotangent);
				}
			}
			/// <summary>
			/// Calculates cotangents of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void Cotangent(float[] source, out float[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new float[source.LongLength];
				CopyToResults(source, results);

				fixed (float* ptr = results)
				{
					MathSimpleOpSingle(ptr, source.LongLength, MathSimpleOperations.Cotangent);
				}
			}
			/// <summary>
			/// Calculates cotangents of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void Cotangent(double[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (double* ptr = numbers)
				{
					MathSimpleOpDouble(ptr, numbers.LongLength, MathSimpleOperations.Cotangent);
				}
			}
			/// <summary>
			/// Calculates cotangents of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void Cotangent(double[] source, out double[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new double[source.LongLength];
				CopyToResults(source, results);

				fixed (double* ptr = results)
				{
					MathSimpleOpDouble(ptr, source.LongLength, MathSimpleOperations.Cotangent);
				}
			}
			/// <summary>
			/// Calculates arcsines of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void Arcsine(float[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (float* ptr = numbers)
				{
					MathSimpleOpSingle(ptr, numbers.LongLength, MathSimpleOperations.Arcsine);
				}
			}
			/// <summary>
			/// Calculates arcsines of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void Arcsine(float[] source, out float[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new float[source.LongLength];
				CopyToResults(source, results);

				fixed (float* ptr = results)
				{
					MathSimpleOpSingle(ptr, source.LongLength, MathSimpleOperations.Arcsine);
				}
			}
			/// <summary>
			/// Calculates arcsines of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void Arcsine(double[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (double* ptr = numbers)
				{
					MathSimpleOpDouble(ptr, numbers.LongLength, MathSimpleOperations.Arcsine);
				}
			}
			/// <summary>
			/// Calculates arcsines of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void Arcsine(double[] source, out double[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new double[source.LongLength];
				CopyToResults(source, results);

				fixed (double* ptr = results)
				{
					MathSimpleOpDouble(ptr, source.LongLength, MathSimpleOperations.Arcsine);
				}
			}
			/// <summary>
			/// Calculates arccosines of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void Arccosine(float[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (float* ptr = numbers)
				{
					MathSimpleOpSingle(ptr, numbers.LongLength, MathSimpleOperations.Arccosine);
				}
			}
			/// <summary>
			/// Calculates arccosines of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void Arccosine(float[] source, out float[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new float[source.LongLength];
				CopyToResults(source, results);

				fixed (float* ptr = results)
				{
					MathSimpleOpSingle(ptr, source.LongLength, MathSimpleOperations.Arccosine);
				}
			}
			/// <summary>
			/// Calculates arccosines of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void Arccosine(double[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (double* ptr = numbers)
				{
					MathSimpleOpDouble(ptr, numbers.LongLength, MathSimpleOperations.Arccosine);
				}
			}
			/// <summary>
			/// Calculates arccosines of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void Arccosine(double[] source, out double[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new double[source.LongLength];
				CopyToResults(source, results);

				fixed (double* ptr = results)
				{
					MathSimpleOpDouble(ptr, source.LongLength, MathSimpleOperations.Arccosine);
				}
			}
			/// <summary>
			/// Calculates arctangents of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void Arctangent(float[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (float* ptr = numbers)
				{
					MathSimpleOpSingle(ptr, numbers.LongLength, MathSimpleOperations.Arctangent);
				}
			}
			/// <summary>
			/// Calculates arctangents of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void Arctangent(float[] source, out float[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new float[source.LongLength];
				CopyToResults(source, results);

				fixed (float* ptr = results)
				{
					MathSimpleOpSingle(ptr, source.LongLength, MathSimpleOperations.Arctangent);
				}
			}
			/// <summary>
			/// Calculates arctangents of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void Arctangent(double[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (double* ptr = numbers)
				{
					MathSimpleOpDouble(ptr, numbers.LongLength, MathSimpleOperations.Arctangent);
				}
			}
			/// <summary>
			/// Calculates arctangents of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void Arctangent(double[] source, out double[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new double[source.LongLength];
				CopyToResults(source, results);

				fixed (double* ptr = results)
				{
					MathSimpleOpDouble(ptr, source.LongLength, MathSimpleOperations.Arctangent);
				}
			}
			/// <summary>
			/// Calculates arccotangents of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void Arccotangent(float[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (float* ptr = numbers)
				{
					MathSimpleOpSingle(ptr, numbers.LongLength, MathSimpleOperations.Arccotangent);
				}
			}
			/// <summary>
			/// Calculates arccotangents of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void Arccotangent(float[] source, out float[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new float[source.LongLength];
				CopyToResults(source, results);

				fixed (float* ptr = results)
				{
					MathSimpleOpSingle(ptr, source.LongLength, MathSimpleOperations.Arccotangent);
				}
			}
			/// <summary>
			/// Calculates arccotangents of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void Arccotangent(double[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (double* ptr = numbers)
				{
					MathSimpleOpDouble(ptr, numbers.LongLength, MathSimpleOperations.Arccotangent);
				}
			}
			/// <summary>
			/// Calculates arccotangents of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void Arccotangent(double[] source, out double[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new double[source.LongLength];
				CopyToResults(source, results);

				fixed (double* ptr = results)
				{
					MathSimpleOpDouble(ptr, source.LongLength, MathSimpleOperations.Arccotangent);
				}
			}
			/// <summary>
			/// Calculates hyperbolic sines of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void SineHyperbolic(float[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (float* ptr = numbers)
				{
					MathSimpleOpSingle(ptr, numbers.LongLength, MathSimpleOperations.SineHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic sines of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void SineHyperbolic(float[] source, out float[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new float[source.LongLength];
				CopyToResults(source, results);

				fixed (float* ptr = results)
				{
					MathSimpleOpSingle(ptr, source.LongLength, MathSimpleOperations.SineHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic sines of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void SineHyperbolic(double[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (double* ptr = numbers)
				{
					MathSimpleOpDouble(ptr, numbers.LongLength, MathSimpleOperations.SineHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic sines of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void SineHyperbolic(double[] source, out double[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new double[source.LongLength];
				CopyToResults(source, results);

				fixed (double* ptr = results)
				{
					MathSimpleOpDouble(ptr, source.LongLength, MathSimpleOperations.SineHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic cosines of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void CosineHyperbolic(float[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (float* ptr = numbers)
				{
					MathSimpleOpSingle(ptr, numbers.LongLength, MathSimpleOperations.CosineHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic cosines of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void CosineHyperbolic(float[] source, out float[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new float[source.LongLength];
				CopyToResults(source, results);

				fixed (float* ptr = results)
				{
					MathSimpleOpSingle(ptr, source.LongLength, MathSimpleOperations.CosineHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic cosines of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void CosineHyperbolic(double[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (double* ptr = numbers)
				{
					MathSimpleOpDouble(ptr, numbers.LongLength, MathSimpleOperations.CosineHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic cosines of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void CosineHyperbolic(double[] source, out double[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new double[source.LongLength];
				CopyToResults(source, results);

				fixed (double* ptr = results)
				{
					MathSimpleOpDouble(ptr, source.LongLength, MathSimpleOperations.CosineHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic tangents of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void TangentHyperbolic(float[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (float* ptr = numbers)
				{
					MathSimpleOpSingle(ptr, numbers.LongLength, MathSimpleOperations.TangentHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic tangents of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void TangentHyperbolic(float[] source, out float[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new float[source.LongLength];
				CopyToResults(source, results);

				fixed (float* ptr = results)
				{
					MathSimpleOpSingle(ptr, source.LongLength, MathSimpleOperations.TangentHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic tangents of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void TangentHyperbolic(double[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (double* ptr = numbers)
				{
					MathSimpleOpDouble(ptr, numbers.LongLength, MathSimpleOperations.TangentHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic tangents of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void TangentHyperbolic(double[] source, out double[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new double[source.LongLength];
				CopyToResults(source, results);

				fixed (double* ptr = results)
				{
					MathSimpleOpDouble(ptr, source.LongLength, MathSimpleOperations.TangentHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic cotangents of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void CotangentHyperbolic(float[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (float* ptr = numbers)
				{
					MathSimpleOpSingle(ptr, numbers.LongLength, MathSimpleOperations.CotangentHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic cotangents of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void CotangentHyperbolic(float[] source, out float[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new float[source.LongLength];
				CopyToResults(source, results);

				fixed (float* ptr = results)
				{
					MathSimpleOpSingle(ptr, source.LongLength, MathSimpleOperations.CotangentHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic cotangents of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void CotangentHyperbolic(double[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (double* ptr = numbers)
				{
					MathSimpleOpDouble(ptr, numbers.LongLength, MathSimpleOperations.CotangentHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic cotangents of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void CotangentHyperbolic(double[] source, out double[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new double[source.LongLength];
				CopyToResults(source, results);

				fixed (double* ptr = results)
				{
					MathSimpleOpDouble(ptr, source.LongLength, MathSimpleOperations.CotangentHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic arcsines of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void ArcsineHyperbolic(float[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (float* ptr = numbers)
				{
					MathSimpleOpSingle(ptr, numbers.LongLength, MathSimpleOperations.ArcsineHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic arcsines of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void ArcsineHyperbolic(float[] source, out float[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new float[source.LongLength];
				CopyToResults(source, results);

				fixed (float* ptr = results)
				{
					MathSimpleOpSingle(ptr, source.LongLength, MathSimpleOperations.ArcsineHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic arcsines of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void ArcsineHyperbolic(double[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (double* ptr = numbers)
				{
					MathSimpleOpDouble(ptr, numbers.LongLength, MathSimpleOperations.ArcsineHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic arcsines of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void ArcsineHyperbolic(double[] source, out double[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new double[source.LongLength];
				CopyToResults(source, results);

				fixed (double* ptr = results)
				{
					MathSimpleOpDouble(ptr, source.LongLength, MathSimpleOperations.ArcsineHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic arccosines of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void ArccosineHyperbolic(float[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (float* ptr = numbers)
				{
					MathSimpleOpSingle(ptr, numbers.LongLength, MathSimpleOperations.ArccosineHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic arccosines of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void ArccosineHyperbolic(float[] source, out float[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new float[source.LongLength];
				CopyToResults(source, results);

				fixed (float* ptr = results)
				{
					MathSimpleOpSingle(ptr, source.LongLength, MathSimpleOperations.ArccosineHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic arccosines of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void ArccosineHyperbolic(double[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (double* ptr = numbers)
				{
					MathSimpleOpDouble(ptr, numbers.LongLength, MathSimpleOperations.ArccosineHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic arccosines of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void ArccosineHyperbolic(double[] source, out double[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new double[source.LongLength];
				CopyToResults(source, results);

				fixed (double* ptr = results)
				{
					MathSimpleOpDouble(ptr, source.LongLength, MathSimpleOperations.ArccosineHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic arctangents of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void ArctangentHyperbolic(float[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (float* ptr = numbers)
				{
					MathSimpleOpSingle(ptr, numbers.LongLength, MathSimpleOperations.ArctangentHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic arctangents of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void ArctangentHyperbolic(float[] source, out float[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new float[source.LongLength];
				CopyToResults(source, results);

				fixed (float* ptr = results)
				{
					MathSimpleOpSingle(ptr, source.LongLength, MathSimpleOperations.ArctangentHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic arctangents of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void ArctangentHyperbolic(double[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (double* ptr = numbers)
				{
					MathSimpleOpDouble(ptr, numbers.LongLength, MathSimpleOperations.ArctangentHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic arctangents of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void ArctangentHyperbolic(double[] source, out double[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new double[source.LongLength];
				CopyToResults(source, results);

				fixed (double* ptr = results)
				{
					MathSimpleOpDouble(ptr, source.LongLength, MathSimpleOperations.ArctangentHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic arccotangents of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void ArccotangentHyperbolic(float[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (float* ptr = numbers)
				{
					MathSimpleOpSingle(ptr, numbers.LongLength, MathSimpleOperations.ArccotangentHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic arccotangents of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void ArccotangentHyperbolic(float[] source, out float[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new float[source.LongLength];
				CopyToResults(source, results);

				fixed (float* ptr = results)
				{
					MathSimpleOpSingle(ptr, source.LongLength, MathSimpleOperations.ArccotangentHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic arccotangents of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void ArccotangentHyperbolic(double[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (double* ptr = numbers)
				{
					MathSimpleOpDouble(ptr, numbers.LongLength, MathSimpleOperations.ArccotangentHyperbolic);
				}
			}
			/// <summary>
			/// Calculates hyperbolic arccotangents of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void ArccotangentHyperbolic(double[] source, out double[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new double[source.LongLength];
				CopyToResults(source, results);

				fixed (double* ptr = results)
				{
					MathSimpleOpDouble(ptr, source.LongLength, MathSimpleOperations.ArccotangentHyperbolic);
				}
			}
			/// <summary>
			/// Calculates natural logarithms of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void LogarithmNatural(float[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (float* ptr = numbers)
				{
					MathSimpleOpSingle(ptr, numbers.LongLength, MathSimpleOperations.LogarithmNatural);
				}
			}
			/// <summary>
			/// Calculates natural logarithms of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void LogarithmNatural(float[] source, out float[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new float[source.LongLength];
				CopyToResults(source, results);

				fixed (float* ptr = results)
				{
					MathSimpleOpSingle(ptr, source.LongLength, MathSimpleOperations.LogarithmNatural);
				}
			}
			/// <summary>
			/// Calculates natural logarithms of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void LogarithmNatural(double[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (double* ptr = numbers)
				{
					MathSimpleOpDouble(ptr, numbers.LongLength, MathSimpleOperations.LogarithmNatural);
				}
			}
			/// <summary>
			/// Calculates natural logarithms of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void LogarithmNatural(double[] source, out double[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new double[source.LongLength];
				CopyToResults(source, results);

				fixed (double* ptr = results)
				{
					MathSimpleOpDouble(ptr, source.LongLength, MathSimpleOperations.LogarithmNatural);
				}
			}
			/// <summary>
			/// Calculates decimal (common) logarithms of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void LogarithmDecimal(float[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (float* ptr = numbers)
				{
					MathSimpleOpSingle(ptr, numbers.LongLength, MathSimpleOperations.LogarithmDecimal);
				}
			}
			/// <summary>
			/// Calculates decimal (common) logarithms of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void LogarithmDecimal(float[] source, out float[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new float[source.LongLength];
				CopyToResults(source, results);

				fixed (float* ptr = results)
				{
					MathSimpleOpSingle(ptr, source.LongLength, MathSimpleOperations.LogarithmDecimal);
				}
			}
			/// <summary>
			/// Calculates decimal (common) logarithms of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void LogarithmDecimal(double[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (double* ptr = numbers)
				{
					MathSimpleOpDouble(ptr, numbers.LongLength, MathSimpleOperations.LogarithmDecimal);
				}
			}
			/// <summary>
			/// Calculates decimal (common) logarithms of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void LogarithmDecimal(double[] source, out double[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new double[source.LongLength];
				CopyToResults(source, results);

				fixed (double* ptr = results)
				{
					MathSimpleOpDouble(ptr, source.LongLength, MathSimpleOperations.LogarithmDecimal);
				}
			}
			/// <summary>
			/// Calculates exponents of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void Exponent(float[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (float* ptr = numbers)
				{
					MathSimpleOpSingle(ptr, numbers.LongLength, MathSimpleOperations.Exponent);
				}
			}
			/// <summary>
			/// Calculates exponents of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void Exponent(float[] source, out float[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new float[source.LongLength];
				CopyToResults(source, results);

				fixed (float* ptr = results)
				{
					MathSimpleOpSingle(ptr, source.LongLength, MathSimpleOperations.Exponent);
				}
			}
			/// <summary>
			/// Calculates exponents of all numbers in the given array.
			/// </summary>
			/// <param name="numbers">
			/// An array of numbers which values get replaced by results of calculation.
			/// </param>
			public static void Exponent(double[] numbers)
			{
				if (numbers == null || numbers.LongLength == 0)
				{
					return;
				}

				fixed (double* ptr = numbers)
				{
					MathSimpleOpDouble(ptr, numbers.LongLength, MathSimpleOperations.Exponent);
				}
			}
			/// <summary>
			/// Calculates exponents of all numbers in the given array.
			/// </summary>
			/// <param name="source"> An array of initial numbers.</param>
			/// <param name="results">An array that will contain the results of calculations.</param>
			public static void Exponent(double[] source, out double[] results)
			{
				if (source == null || source.LongLength == 0)
				{
					results = null;
					return;
				}

				results = new double[source.LongLength];
				CopyToResults(source, results);

				fixed (double* ptr = results)
				{
					MathSimpleOpDouble(ptr, source.LongLength, MathSimpleOperations.Exponent);
				}
			}
			/// <summary>
			/// Calculates power of a sequence of numbers.
			/// </summary>
			/// <param name="data">
			/// An array of 3D vectors where X-coordinate is a number that needs to be powered,
			/// Y-coordinate is a powering factor and Z-coordinate becomes a result of calculation after
			/// this function concludes.
			/// </param>
			public static void Power(Vector3[] data)
			{
				if (data == null || data.LongLength == 0)
				{
					return;
				}

				fixed (Vector3* ptr = data)
				{
					Math3NumberOpSingle(ptr, data.LongLength, Math3NumberOperations.Power);
				}
			}
			/// <summary>
			/// Calculates power of a sequence of numbers.
			/// </summary>
			/// <param name="data">
			/// An array of 3D vectors where X-coordinate is a number that needs to be powered,
			/// Y-coordinate is a powering factor and Z-coordinate becomes a result of calculation after
			/// this function concludes.
			/// </param>
			public static void Power(Vector3Double[] data)
			{
				if (data == null || data.LongLength == 0)
				{
					return;
				}

				fixed (Vector3Double* ptr = data)
				{
					Math3NumberOpDouble(ptr, data.LongLength, Math3NumberOperations.Power);
				}
			}
			/// <summary>
			/// Calculates logarithms of a sequence of numbers.
			/// </summary>
			/// <param name="data">
			/// An array of 3D vectors where X-coordinate is a logarithm base, Y-coordinate is a number for
			/// which the logarithm has to be calculated and Z-coordinate becomes a result of calculation
			/// after this function concludes.
			/// </param>
			public static void Logarithm(Vector3[] data)
			{
				if (data == null || data.LongLength == 0)
				{
					return;
				}

				fixed (Vector3* ptr = data)
				{
					Math3NumberOpSingle(ptr, data.LongLength, Math3NumberOperations.Logarithm);
				}
			}
			/// <summary>
			/// Calculates logarithms of a sequence of numbers.
			/// </summary>
			/// <param name="data">
			/// An array of 3D vectors where X-coordinate is a logarithm base, Y-coordinate is a number for
			/// which the logarithm has to be calculated and Z-coordinate becomes a result of calculation
			/// after this function concludes.
			/// </param>
			public static void Logarithm(Vector3Double[] data)
			{
				if (data == null || data.LongLength == 0)
				{
					return;
				}

				fixed (Vector3Double* ptr = data)
				{
					Math3NumberOpDouble(ptr, data.LongLength, Math3NumberOperations.Logarithm);
				}
			}
			/// <summary>
			/// Calculates both sine and cosine of a sequence of numbers.
			/// </summary>
			/// <param name="data">
			/// An array of 3D vectors where X-coordinate is a number which sine and cosine need to be
			/// calculated, Y-coordinate is a resultant sine and Z-coordinate is a resultant cosine.
			/// </param>
			public static void SineCosine(Vector3[] data)
			{
				if (data == null || data.LongLength == 0)
				{
					return;
				}

				fixed (Vector3* ptr = data)
				{
					Math3NumberOpSingle(ptr, data.LongLength, Math3NumberOperations.SineCosine);
				}
			}
			/// <summary>
			/// Calculates both sine and cosine of a sequence of numbers.
			/// </summary>
			/// <param name="data">
			/// An array of 3D vectors where X-coordinate is a number which sine and cosine need to be
			/// calculated, Y-coordinate is a resultant sine and Z-coordinate is a resultant cosine.
			/// </param>
			public static void SineCosine(Vector3Double[] data)
			{
				if (data == null || data.LongLength == 0)
				{
					return;
				}

				fixed (Vector3Double* ptr = data)
				{
					Math3NumberOpDouble(ptr, data.LongLength, Math3NumberOperations.SineCosine);
				}
			}
			/// <summary>
			/// Calculates angle defined by two tangents.
			/// </summary>
			/// <param name="data">
			/// An array of 3D vectors where X-coordinate is a first tangent, Y-coordinate is a second
			/// tangent and Z-coordinate is a resultant angle.
			/// </param>
			public static void Arctangent2(Vector3[] data)
			{
				if (data == null || data.LongLength == 0)
				{
					return;
				}

				fixed (Vector3* ptr = data)
				{
					Math3NumberOpSingle(ptr, data.LongLength, Math3NumberOperations.Arctangent2);
				}
			}
			/// <summary>
			/// Calculates angle defined by two tangents.
			/// </summary>
			/// <param name="data">
			/// An array of 3D vectors where X-coordinate is a first tangent, Y-coordinate is a second
			/// tangent and Z-coordinate is a resultant angle.
			/// </param>
			public static void Arctangent2(Vector3Double[] data)
			{
				if (data == null || data.LongLength == 0)
				{
					return;
				}

				fixed (Vector3Double* ptr = data)
				{
					Math3NumberOpDouble(ptr, data.LongLength, Math3NumberOperations.Arctangent2);
				}
			}
			private static void CopyToResults<T>(T[] sourceArray, T[] resultsArray)
			{
				try
				{
					sourceArray.CopyTo(resultsArray, 0);
				}
				catch (Exception ex)
				{
					MonoInterface.DisplayException(ex);
				}
			}
		}
	}
}