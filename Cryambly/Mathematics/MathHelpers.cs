using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Geometry;

namespace CryCil
{
	/// <summary>
	/// Defines some useful and not so much mathematical functions.
	/// </summary>
	public static class MathHelpers
	{
		#region Fields
		/// <summary>
		/// All positive numbers that are smaller than this value are considered equal to zero.
		/// </summary>
		public const float ZeroTolerance = 1e-6f;
		/// <summary>
		/// All negative numbers that are greater than this value are considered equal to zero.
		/// </summary>
		public const float NZeroTolerance = -1e-6f;
		/// <summary>
		/// Doubled <see cref="Math.PI"/> .
		/// </summary>
		public const double PI2 = 2 * Math.PI;
		#endregion
		#region Properties
		#endregion
		#region Events
		#endregion
		#region Construction
		#endregion
		#region Interface
		/// <summary>
		/// Returns reciprocal square root of the value.
		/// </summary>
		/// <param name="value">Value to calculate reciprocal square root from.</param>
		/// <returns>1 / square root of <paramref name="value"/>.</returns>
		public static double ReciprocalSquareRoot(double value)
		{
			return 1.0 / Math.Sqrt(value);
		}
		/// <summary>
		/// Returns reciprocal square root of the value.
		/// </summary>
		/// <param name="value">Value to calculate reciprocal square root from.</param>
		/// <returns>1 / square root of <paramref name="value"/>.</returns>
		public static float ReciprocalSquareRoot(float value)
		{
			return (float)(1.0 / Math.Sqrt(value));
		}
		/// <summary>
		/// Calculates sine and cosine at the same time.
		/// </summary>
		/// <param name="value">     Angle to calculate sine and cosine of.</param>
		/// <param name="sine">Resultant sine.</param>
		/// <param name="cosine">Resultant cosine.</param>
		public static void SinCos(double value, out double sine, out double cosine)
		{
			sine = Math.Sin(value);

			cosine = Math.Sqrt(1.0 - sine * sine);
		}
		/// <summary>
		/// Calculates sine and cosine at the same time.
		/// </summary>
		/// <param name="value">     Angle to calculate sine and cosine of.</param>
		/// <param name="sine">Resultant sine.</param>
		/// <param name="cosine">Resultant cosine.</param>
		public static void SinCos(float value, out float sine, out float cosine)
		{
			sine = (float)Math.Sin(value);

			cosine = (float)Math.Sqrt(1.0f - sine * sine);
		}
		/// <summary>
		/// Calculates logarithm of the quaternion.
		/// </summary>
		/// <param name="value">Quaternion to calculate logarithm from.</param>
		/// <returns>
		/// Vector for which <see cref="Exp(Vector3)"/> returns <paramref name="value"/> .
		/// </returns>
		public static Vector3 Log(Quaternion value)
		{
			var lensqr = value.Vector.LengthSquared;
			if (!(lensqr > 0.0f))
			{
				// logarithm of a quaternion, imaginary part (the real part of the logarithm is always 0).
				return new Vector3(0);
			}
			var len = Math.Sqrt(lensqr);
			var angle = Math.Atan2(len, value.W) / len;
			return value.Vector * (float)angle;
		}
		/// <summary>
		/// Calculates a unit quaternion from a vector.
		/// </summary>
		/// <param name="value">Vector to calculate quaternion from.</param>
		/// <returns>A new unit quaternion.</returns>
		public static Quaternion Exp(Vector3 value)
		{
			var lensqr = value.LengthSquared;
			if (!(lensqr > 0.0f))
			{
				return Quaternion.Identity;
			}
			var len = (float)Math.Sqrt(lensqr);
			float s, c;
			SinCos(len, out s, out c);
			s /= len;
			return new Quaternion(c, value.X * s, value.Y * s, value.Z * s);
		}
		/// <summary>
		/// Determines whether a value is inside the specified range.
		/// </summary>
		/// <typeparam name="T">Type that implements <see cref="IComparable{T}"/> interface.</typeparam>
		/// <param name="value">Value to check.</param>
		/// <param name="min">  Value that defines left bound of the range.</param>
		/// <param name="max">  Value that defines right bound of the range.</param>
		/// <returns>True, <paramref name="value"/> if it is within the range. Otherwise false.</returns>
		public static bool IsInRange<T>(T value, T min, T max) where T : IComparable<T>
		{
			return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
		}
		/// <summary>
		/// Clamps a value into a specified range.
		/// </summary>
		/// <typeparam name="T">Type that implements <see cref="IComparable{T}"/> interface.</typeparam>
		/// <param name="value">Value to clamp.</param>
		/// <param name="min">  Value that defines left bound of the range.</param>
		/// <param name="max">  Value that defines right bound of the range.</param>
		/// <returns>
		/// <para><paramref name="value"/> if it is within the range;</para>
		/// <para>
		/// <paramref name="min"/>, if <paramref name="value"/> is less then <paramref name="min"/>;
		/// </para>
		/// <para>
		/// <paramref name="max"/>, if <paramref name="value"/> is greater then <paramref name="max"/>.
		/// </para>
		/// </returns>
		public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
		{
			if (value.CompareTo(min) < 0)
				return min;
			return value.CompareTo(max) > 0 ? max : value;
		}
		/// <summary>
		/// Determines which of two comparable values is the biggest one.
		/// </summary>
		/// <typeparam name="T">Type that implements <see cref="IComparable{T}"/> interface.</typeparam>
		/// <param name="val1">First value.</param>
		/// <param name="val2">Second value.</param>
		/// <returns>The value that is greater then another.</returns>
		public static T Max<T>(T val1, T val2) where T : IComparable<T>
		{
			return val1.CompareTo(val2) > 0 ? val1 : val2;
		}
		/// <summary>
		/// Returns the biggest of all given arguments.
		/// </summary>
		/// <typeparam name="T">Type that implements <see cref="IComparable{T}"/> interface.</typeparam>
		/// <param name="v0">Argument №0.</param>
		/// <param name="v1">Argument №1.</param>
		/// <param name="v2">Argument №2.</param>
		/// <param name="v3">Argument №3.</param>
		/// <param name="v4">Argument №4.</param>
		/// <param name="v5">Argument №5.</param>
		/// <param name="v6">Argument №6.</param>
		/// <param name="v7">Argument №7.</param>
		/// <returns></returns>
		public static T Max<T>(T v0, T v1, T v2, T v3, T v4, T v5, T v6, T v7) where T : IComparable<T>
		{
			T max = v0;
			if (v1.CompareTo(max) > 0)
			{
				max = v1;
			}
			if (v2.CompareTo(max) > 0)
			{
				max = v2;
			}
			if (v3.CompareTo(max) > 0)
			{
				max = v3;
			}
			if (v4.CompareTo(max) > 0)
			{
				max = v4;
			}
			if (v5.CompareTo(max) > 0)
			{
				max = v5;
			}
			if (v6.CompareTo(max) > 0)
			{
				max = v6;
			}
			if (v7.CompareTo(max) > 0)
			{
				max = v7;
			}
			return max;
		}
		/// <summary>
		/// Returns the biggest of all given arguments.
		/// </summary>
		/// <typeparam name="T">Type that implements <see cref="IComparable{T}"/> interface.</typeparam>
		/// <param name="v0">Argument №0.</param>
		/// <param name="v1">Argument №1.</param>
		/// <param name="v2">Argument №2.</param>
		/// <param name="v3">Argument №3.</param>
		/// <param name="v4">Argument №4.</param>
		/// <param name="v5">Argument №5.</param>
		/// <param name="v6">Argument №6.</param>
		/// <returns></returns>
		public static T Max<T>(T v0, T v1, T v2, T v3, T v4, T v5, T v6) where T : IComparable<T>
		{
			T max = v0;
			if (v1.CompareTo(max) > 0)
			{
				max = v1;
			}
			if (v2.CompareTo(max) > 0)
			{
				max = v2;
			}
			if (v3.CompareTo(max) > 0)
			{
				max = v3;
			}
			if (v4.CompareTo(max) > 0)
			{
				max = v4;
			}
			if (v5.CompareTo(max) > 0)
			{
				max = v5;
			}
			if (v6.CompareTo(max) > 0)
			{
				max = v6;
			}
			return max;
		}
		/// <summary>
		/// Returns the biggest of all given arguments.
		/// </summary>
		/// <typeparam name="T">Type that implements <see cref="IComparable{T}"/> interface.</typeparam>
		/// <param name="v0">Argument №0.</param>
		/// <param name="v1">Argument №1.</param>
		/// <param name="v2">Argument №2.</param>
		/// <param name="v3">Argument №3.</param>
		/// <param name="v4">Argument №4.</param>
		/// <param name="v5">Argument №5.</param>
		/// <returns></returns>
		public static T Max<T>(T v0, T v1, T v2, T v3, T v4, T v5) where T : IComparable<T>
		{
			T max = v0;
			if (v1.CompareTo(max) > 0)
			{
				max = v1;
			}
			if (v2.CompareTo(max) > 0)
			{
				max = v2;
			}
			if (v3.CompareTo(max) > 0)
			{
				max = v3;
			}
			if (v4.CompareTo(max) > 0)
			{
				max = v4;
			}
			if (v5.CompareTo(max) > 0)
			{
				max = v5;
			}
			return max;
		}
		/// <summary>
		/// Returns the biggest of all given arguments.
		/// </summary>
		/// <typeparam name="T">Type that implements <see cref="IComparable{T}"/> interface.</typeparam>
		/// <param name="v0">Argument №0.</param>
		/// <param name="v1">Argument №1.</param>
		/// <param name="v2">Argument №2.</param>
		/// <param name="v3">Argument №3.</param>
		/// <param name="v4">Argument №4.</param>
		/// <returns></returns>
		public static T Max<T>(T v0, T v1, T v2, T v3, T v4) where T : IComparable<T>
		{
			T max = v0;
			if (v1.CompareTo(max) > 0)
			{
				max = v1;
			}
			if (v2.CompareTo(max) > 0)
			{
				max = v2;
			}
			if (v3.CompareTo(max) > 0)
			{
				max = v3;
			}
			if (v4.CompareTo(max) > 0)
			{
				max = v4;
			}
			return max;
		}
		/// <summary>
		/// Returns the biggest of all given arguments.
		/// </summary>
		/// <typeparam name="T">Type that implements <see cref="IComparable{T}"/> interface.</typeparam>
		/// <param name="v0">Argument №0.</param>
		/// <param name="v1">Argument №1.</param>
		/// <param name="v2">Argument №2.</param>
		/// <param name="v3">Argument №3.</param>
		/// <returns></returns>
		public static T Max<T>(T v0, T v1, T v2, T v3) where T : IComparable<T>
		{
			T max = v0;
			if (v1.CompareTo(max) > 0)
			{
				max = v1;
			}
			if (v2.CompareTo(max) > 0)
			{
				max = v2;
			}
			if (v3.CompareTo(max) > 0)
			{
				max = v3;
			}
			return max;
		}
		/// <summary>
		/// Returns the biggest of all given arguments.
		/// </summary>
		/// <typeparam name="T">Type that implements <see cref="IComparable{T}"/> interface.</typeparam>
		/// <param name="v0">Argument №0.</param>
		/// <param name="v1">Argument №1.</param>
		/// <param name="v2">Argument №2.</param>
		/// <returns></returns>
		public static T Max<T>(T v0, T v1, T v2) where T : IComparable<T>
		{
			T max = v0;
			if (v1.CompareTo(max) > 0)
			{
				max = v1;
			}
			if (v2.CompareTo(max) > 0)
			{
				max = v2;
			}
			return max;
		}
		/// <summary>
		/// Determines which of two comparable values is the smallest one.
		/// </summary>
		/// <typeparam name="T">Type that implements <see cref="IComparable{T}"/> interface.</typeparam>
		/// <param name="val1">First value.</param>
		/// <param name="val2">Second value.</param>
		/// <returns>The value that is smaller then another.</returns>
		public static T Min<T>(T val1, T val2) where T : IComparable<T>
		{
			return val1.CompareTo(val2) < 0 ? val1 : val2;
		}
		/// <summary>
		/// Returns the smallest of all given arguments.
		/// </summary>
		/// <typeparam name="T">Type that implements <see cref="IComparable{T}"/> interface.</typeparam>
		/// <param name="v0">Argument №0.</param>
		/// <param name="v1">Argument №1.</param>
		/// <param name="v2">Argument №2.</param>
		/// <param name="v3">Argument №3.</param>
		/// <param name="v4">Argument №4.</param>
		/// <param name="v5">Argument №5.</param>
		/// <param name="v6">Argument №6.</param>
		/// <param name="v7">Argument №7.</param>
		/// <returns></returns>
		public static T Min<T>(T v0, T v1, T v2, T v3, T v4, T v5, T v6, T v7) where T : IComparable<T>
		{
			T min = v0;
			if (v1.CompareTo(min) < 0)
			{
				min = v1;
			}
			if (v2.CompareTo(min) < 0)
			{
				min = v2;
			}
			if (v3.CompareTo(min) < 0)
			{
				min = v3;
			}
			if (v4.CompareTo(min) < 0)
			{
				min = v4;
			}
			if (v5.CompareTo(min) < 0)
			{
				min = v5;
			}
			if (v6.CompareTo(min) < 0)
			{
				min = v6;
			}
			if (v7.CompareTo(min) < 0)
			{
				min = v7;
			}
			return min;
		}
		/// <summary>
		/// Returns the smallest of all given arguments.
		/// </summary>
		/// <typeparam name="T">Type that implements <see cref="IComparable{T}"/> interface.</typeparam>
		/// <param name="v0">Argument №0.</param>
		/// <param name="v1">Argument №1.</param>
		/// <param name="v2">Argument №2.</param>
		/// <param name="v3">Argument №3.</param>
		/// <param name="v4">Argument №4.</param>
		/// <param name="v5">Argument №5.</param>
		/// <param name="v6">Argument №6.</param>
		/// <returns></returns>
		public static T Min<T>(T v0, T v1, T v2, T v3, T v4, T v5, T v6) where T : IComparable<T>
		{
			T min = v0;
			if (v1.CompareTo(min) < 0)
			{
				min = v1;
			}
			if (v2.CompareTo(min) < 0)
			{
				min = v2;
			}
			if (v3.CompareTo(min) < 0)
			{
				min = v3;
			}
			if (v4.CompareTo(min) < 0)
			{
				min = v4;
			}
			if (v5.CompareTo(min) < 0)
			{
				min = v5;
			}
			if (v6.CompareTo(min) < 0)
			{
				min = v6;
			}
			return min;
		}
		/// <summary>
		/// Returns the smallest of all given arguments.
		/// </summary>
		/// <typeparam name="T">Type that implements <see cref="IComparable{T}"/> interface.</typeparam>
		/// <param name="v0">Argument №0.</param>
		/// <param name="v1">Argument №1.</param>
		/// <param name="v2">Argument №2.</param>
		/// <param name="v3">Argument №3.</param>
		/// <param name="v4">Argument №4.</param>
		/// <param name="v5">Argument №5.</param>
		/// <returns></returns>
		public static T Min<T>(T v0, T v1, T v2, T v3, T v4, T v5) where T : IComparable<T>
		{
			T min = v0;
			if (v1.CompareTo(min) < 0)
			{
				min = v1;
			}
			if (v2.CompareTo(min) < 0)
			{
				min = v2;
			}
			if (v3.CompareTo(min) < 0)
			{
				min = v3;
			}
			if (v4.CompareTo(min) < 0)
			{
				min = v4;
			}
			if (v5.CompareTo(min) < 0)
			{
				min = v5;
			}
			return min;
		}
		/// <summary>
		/// Returns the smallest of all given arguments.
		/// </summary>
		/// <typeparam name="T">Type that implements <see cref="IComparable{T}"/> interface.</typeparam>
		/// <param name="v0">Argument №0.</param>
		/// <param name="v1">Argument №1.</param>
		/// <param name="v2">Argument №2.</param>
		/// <param name="v3">Argument №3.</param>
		/// <param name="v4">Argument №4.</param>
		/// <returns></returns>
		public static T Min<T>(T v0, T v1, T v2, T v3, T v4) where T : IComparable<T>
		{
			T min = v0;
			if (v1.CompareTo(min) < 0)
			{
				min = v1;
			}
			if (v2.CompareTo(min) < 0)
			{
				min = v2;
			}
			if (v3.CompareTo(min) < 0)
			{
				min = v3;
			}
			if (v4.CompareTo(min) < 0)
			{
				min = v4;
			}
			return min;
		}
		/// <summary>
		/// Returns the smallest of all given arguments.
		/// </summary>
		/// <typeparam name="T">Type that implements <see cref="IComparable{T}"/> interface.</typeparam>
		/// <param name="v0">Argument №0.</param>
		/// <param name="v1">Argument №1.</param>
		/// <param name="v2">Argument №2.</param>
		/// <param name="v3">Argument №3.</param>
		/// <returns></returns>
		public static T Min<T>(T v0, T v1, T v2, T v3) where T : IComparable<T>
		{
			T min = v0;
			if (v1.CompareTo(min) < 0)
			{
				min = v1;
			}
			if (v2.CompareTo(min) < 0)
			{
				min = v2;
			}
			if (v3.CompareTo(min) < 0)
			{
				min = v3;
			}
			return min;
		}
		/// <summary>
		/// Returns the smallest of all given arguments.
		/// </summary>
		/// <typeparam name="T">Type that implements <see cref="IComparable{T}"/> interface.</typeparam>
		/// <param name="v0">Argument №0.</param>
		/// <param name="v1">Argument №1.</param>
		/// <param name="v2">Argument №2.</param>
		/// <returns></returns>
		public static T Min<T>(T v0, T v1, T v2) where T : IComparable<T>
		{
			T min = v0;
			if (v1.CompareTo(min) < 0)
			{
				min = v1;
			}
			if (v2.CompareTo(min) < 0)
			{
				min = v2;
			}
			return min;
		}
		/// <summary>
		/// Determines whether given value is a power of 2.
		/// </summary>
		/// <param name="value">Number to check.</param>
		/// <returns>True, if <paramref name="value"/> is a power of 2.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsPowerOfTwo(int value)
		{
			return (value & (value - 1)) == 0;
		}
		/// <summary>
		/// Determines whether given number is valid.
		/// </summary>
		/// <param name="value">Number to check.</param>
		/// <returns>True, if <paramref name="value"/> is a valid number.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNumberValid(double value)
		{
			const ulong mask = (ulong)(255 << 55);

			return ((ulong)value & mask) != mask;
		}
		/// <summary>
		/// Determines whether given number is valid.
		/// </summary>
		/// <param name="value">Number to check.</param>
		/// <returns>True, if <paramref name="value"/> is a valid number.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNumberValid(float value)
		{
			const int mask = 0xFF << 23;

			return ((uint)value & mask) != mask;
		}
		/// <summary>
		/// Extracts an exponent from the floating point number.
		/// </summary>
		/// <param name="value">Number to extract the exponent from.</param>
		/// <returns>An actual exponent.</returns>
		/// <exception cref="InvalidOperationException">
		/// Cannot extract exponent from an invalid value.
		/// </exception>
		public static unsafe int Exponent(float value)
		{
			if (float.IsNaN(value) || float.IsNegativeInfinity(value) ||
				float.IsPositiveInfinity(value) || float.IsInfinity(value))
			{
				throw new InvalidOperationException("Cannot extract exponent from an invalid value.");
			}

			int bits = *(int*)&value;

			int exp = (bits >> 23) & 255;

			return exp - 127; // Actual exponent.
		}
		/// <summary>
		/// Extracts an exponent from the floating point number.
		/// </summary>
		/// <param name="value">Number to extract the exponent from.</param>
		/// <returns>An actual exponent.</returns>
		/// <exception cref="InvalidOperationException">
		/// Cannot extract exponent from an invalid value.
		/// </exception>
		public static unsafe int Exponent(double value)
		{
			if (double.IsNaN(value) || double.IsNegativeInfinity(value) ||
				double.IsPositiveInfinity(value) || double.IsInfinity(value))
			{
				throw new InvalidOperationException("Cannot extract exponent from an invalid value.");
			}

			long bits = *(long*)&value;

			int exp = (int)((bits >> 52) & 2047);

			return exp - 1023; // Actual exponent.
		}
		#endregion
		#region Utilities
		#endregion
	}
}