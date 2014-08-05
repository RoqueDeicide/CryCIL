using System;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using CryEngine.NativeMemory;

namespace CryEngine
{
	/// <summary>
	/// Represents a two dimensional mathematical vector.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Vector2 : IEquatable<Vector2>, IFormattable, IEnumerable<float>
	{
		#region Static Fields
		/// <summary>
		/// Vector that represents an origin of coordinates.
		/// </summary>
		public static readonly Vector2 Zero = new Vector2(0, 0);
		/// <summary>
		/// Number of bytes each instance of this structure consists of.
		/// </summary>
		public static readonly ulong ByteCount = (ulong)Marshal.SizeOf(typeof(Vector2));
		#endregion
		#region Fields
		/// <summary>
		/// The X component of the vector.
		/// </summary>
		public float X;
		/// <summary>
		/// The Y component of the vector.
		/// </summary>
		public float Y;
		#endregion
		#region Properties
		/// <summary>
		/// Gets a value indicting whether this instance is normalized.
		/// </summary>
		public bool IsNormalized
		{
			get { return Math.Abs((X * X) + (Y * Y) - 1f) < MathHelpers.ZeroTolerance; }
		}
		/// <summary>
		/// Gets or sets the component at the specified index.
		/// </summary>
		/// <value>The value of the X or Y component, depending on the index.</value>
		/// <param name="index">
		/// The index of the component to access. Use 0 for the X component and 1 for the Y component.
		/// </param>
		/// <returns>The value of the component at the specified index.</returns>
		public float this[int index]
		{
			get
			{
				if (index == 0 || index == 1)
				{
					throw new ArgumentOutOfRangeException("index", "Vector2.Indexer: parameter index must be either equal to 0 or 1.");
				}
				switch (index)
				{
					case 0: return X;
					case 1: return Y;
				}
				return 0;
			}
			set
			{
				if (index == 0 || index == 1)
				{
					throw new ArgumentOutOfRangeException("index", "Vector2.Indexer: parameter index must be either equal to 0 or 1.");
				}
				switch (index)
				{
					case 0: X = value; break;
					case 1: Y = value; break;
				}
			}
		}
		/// <summary>
		/// Calculates the length of the vector.
		/// </summary>
		/// <returns>The length of the vector.</returns>
		/// <remarks>
		/// <see cref="Vector2.LengthSquared" /> may be preferred when only the relative length is
		/// needed and speed is of the essence.
		/// </remarks>
		public float Length
		{
			get { return (float)Math.Sqrt((X * X) + (Y * Y)); }
		}
		/// <summary>
		/// Calculates the squared length of the vector.
		/// </summary>
		/// <returns>The squared length of the vector.</returns>
		/// <remarks>
		/// This method may be preferred to <see cref="Vector2.Length" /> when only a relative
		/// length is needed and speed is of the essence.
		/// </remarks>
		public float LengthSquared
		{
			get { return (X * X) + (Y * Y); }
		}
		/// <summary>
		/// Determines whether this vector is represented by valid numbers.
		/// </summary>
		public bool IsValid
		{
			get
			{
				return MathHelpers.IsNumberValid(this.X) &&
					MathHelpers.IsNumberValid(this.Y);
			}
		}
		#endregion
		#region Interface
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Vector2" /> struct.
		/// </summary>
		/// <param name="value">The value that will be assigned to all components.</param>
		public Vector2(float value)
		{
			X = value;
			Y = value;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Vector2" /> struct.
		/// </summary>
		/// <param name="x">Initial value for the X component of the vector.</param>
		/// <param name="y">Initial value for the Y component of the vector.</param>
		public Vector2(float x, float y)
		{
			X = x;
			Y = y;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Vector2" /> struct.
		/// </summary>
		/// <param name="values">
		/// An array which contents are used to create new vector. Only first two values can be
		/// used. If array only contains one value, it will be assigned to both components of the
		/// vector. If array doesn't contain any values, vector will be initialized with zeros.
		/// </param>
		public Vector2(float[] values)
		{
			if (values.Length >= 2)
			{
				this.X = values[0];
				this.Y = values[1];
			}
			else if (values.Length == 1)
			{
				this.X = values[0];
				this.Y = values[0];
			}
			else
			{
				this.X = 0;
				this.Y = 0;
			}
		}
		#endregion
		#region Modification
		/// <summary>
		/// Creates new instance of <see cref="Vector2" /> where Y, Z and W components are of this
		/// instance and X component is specified by given value.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="value">New value for X component.</param>
		/// <returns>Modified vector.</returns>
		public Vector2 ModifyX(float value)
		{
			return new Vector2(value, this.Y);
		}
		/// <summary>
		/// Creates new instance of <see cref="Vector2" /> where X, Z and W components are of this
		/// instance and Y component is specified by given value.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="value">New value for Y component.</param>
		/// <returns>Modified vector.</returns>
		public Vector2 ModifyY(float value)
		{
			return new Vector2(this.X, value);
		}
		/// <summary>
		/// Creates new <see cref="Vector2" /> which represents this vector with modified values.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="x">New value for X component.</param>
		/// <param name="y">New value for Y component.</param>
		/// <returns>Modified vector.</returns>
		public Vector2 ModifyVector(float x, float y)
		{
			return new Vector2(x, y);
		}
		#endregion
		#region Conversions To Collections
		/// <summary>
		/// Creates an array containing the elements of the vector.
		/// </summary>
		/// <returns>A two-element array containing the components of the vector.</returns>
		public float[] ToArray()
		{
			return new[] { X, Y };
		}
		/// <summary>
		/// Creates a list that contains components of this vector.
		/// </summary>
		/// <returns>
		/// A list which capacity is set to 2 with first element being an X-component of this
		/// vector, and second element being an Y-component of this vector.
		/// </returns>
		public List<float> ToList()
		{
			List<float> result = new List<float>(2) { this.X, this.Y };
			return result;
		}
		/// <summary>
		/// Creates a dictionary that contains components of this vector.
		/// </summary>
		/// <returns>
		/// A dictionary which capacity is set to 2 where components of the vector can be accessed
		/// with keys of same names.
		/// </returns>
		public Dictionary<string, float> ToDictionary()
		{
			Dictionary<string, float> result = new Dictionary<string, float>(2) { { "X", this.X }, { "Y", this.Y } };
			return result;
		}
		#endregion
		#region Generic Mathematics
		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <param name="result">When the method completes, contains the clamped value.</param>
		public static void Clamp(ref Vector2 value, ref Vector2 min, ref Vector2 max, out Vector2 result)
		{
			float x = value.X;
			x = (x > max.X) ? max.X : x;
			x = (x < min.X) ? min.X : x;

			float y = value.Y;
			y = (y > max.Y) ? max.Y : y;
			y = (y < min.Y) ? min.Y : y;

			result = new Vector2(x, y);
		}
		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <returns>The clamped value.</returns>
		public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max)
		{
			Vector2 result;
			Clamp(ref value, ref min, ref max, out result);
			return result;
		}
		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		/// <param name="value1">The first vector.</param>
		/// <param name="value2">The second vector.</param>
		/// <param name="result">
		/// When the method completes, contains the distance between the two vectors.
		/// </param>
		/// <remarks>
		/// <see cref="Vector2.DistanceSquared(ref Vector2, ref Vector2, out float)" /> may be
		/// preferred when only the relative distance is needed and speed is of the essence.
		/// </remarks>
		public static void Distance(ref Vector2 value1, ref Vector2 value2, out float result)
		{
			float x = value1.X - value2.X;
			float y = value1.Y - value2.Y;

			result = (float)Math.Sqrt((x * x) + (y * y));
		}
		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		/// <param name="value1">The first vector.</param>
		/// <param name="value2">The second vector.</param>
		/// <returns>The distance between the two vectors.</returns>
		/// <remarks>
		/// <see cref="Vector2.DistanceSquared(Vector2, Vector2)" /> may be preferred when only the
		/// relative distance is needed and speed is of the essence.
		/// </remarks>
		public static float Distance(Vector2 value1, Vector2 value2)
		{
			float x = value1.X - value2.X;
			float y = value1.Y - value2.Y;

			return (float)Math.Sqrt((x * x) + (y * y));
		}
		/// <summary>
		/// Calculates the squared distance between two vectors.
		/// </summary>
		/// <param name="value1">The first vector.</param>
		/// <param name="value2">The second vector</param>
		/// <param name="result">
		/// When the method completes, contains the squared distance between the two vectors.
		/// </param>
		/// <remarks>
		/// Distance squared is the value before taking the square root. Distance squared can often
		/// be used in place of distance if relative comparisons are being made. For example,
		/// consider three points A, B, and C. To determine whether B or C is further from A,
		/// compare the distance between A and B to the distance between A and C. Calculating the
		/// two distances involves two square roots, which are computationally expensive. However,
		/// using distance squared provides the same information and avoids calculating two square roots.
		/// </remarks>
		public static void DistanceSquared(ref Vector2 value1, ref Vector2 value2, out float result)
		{
			float x = value1.X - value2.X;
			float y = value1.Y - value2.Y;

			result = (x * x) + (y * y);
		}
		/// <summary>
		/// Calculates the squared distance between two vectors.
		/// </summary>
		/// <param name="value1">The first vector.</param>
		/// <param name="value2">The second vector.</param>
		/// <returns>The squared distance between the two vectors.</returns>
		/// <remarks>
		/// Distance squared is the value before taking the square root. Distance squared can often
		/// be used in place of distance if relative comparisons are being made. For example,
		/// consider three points A, B, and C. To determine whether B or C is further from A,
		/// compare the distance between A and B to the distance between A and C. Calculating the
		/// two distances involves two square roots, which are computationally expensive. However,
		/// using distance squared provides the same information and avoids calculating two square roots.
		/// </remarks>
		public static float DistanceSquared(Vector2 value1, Vector2 value2)
		{
			float x = value1.X - value2.X;
			float y = value1.Y - value2.Y;

			return (x * x) + (y * y);
		}
		/// <summary>
		/// Calculates the dot product of two vectors.
		/// </summary>
		/// <param name="left">First source vector.</param>
		/// <param name="right">Second source vector.</param>
		/// <param name="result">
		/// When the method completes, contains the dot product of the two vectors.
		/// </param>
		public static void Dot(ref Vector2 left, ref Vector2 right, out float result)
		{
			result = (left.X * right.X) + (left.Y * right.Y);
		}
		/// <summary>
		/// Calculates the dot product of two vectors.
		/// </summary>
		/// <param name="left">First source vector.</param>
		/// <param name="right">Second source vector.</param>
		/// <returns>The dot product of the two vectors.</returns>
		public static float Dot(Vector2 left, Vector2 right)
		{
			return (left.X * right.X) + (left.Y * right.Y);
		}
		/// <summary>
		/// Returns a vector containing the smallest components of the specified vectors.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <param name="result">
		/// When the method completes, contains an new vector composed of the largest components of
		/// the source vectors.
		/// </param>
		public static void Max(ref Vector2 left, ref Vector2 right, out Vector2 result)
		{
			result.X = (left.X > right.X) ? left.X : right.X;
			result.Y = (left.Y > right.Y) ? left.Y : right.Y;
		}
		/// <summary>
		/// Returns a vector containing the largest components of the specified vectors.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>A vector containing the largest components of the source vectors.</returns>
		public static Vector2 Max(Vector2 left, Vector2 right)
		{
			Vector2 result;
			Max(ref left, ref right, out result);
			return result;
		}
		/// <summary>
		/// Returns a vector containing the smallest components of the specified vectors.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <param name="result">
		/// When the method completes, contains an new vector composed of the smallest components of
		/// the source vectors.
		/// </param>
		public static void Min(ref Vector2 left, ref Vector2 right, out Vector2 result)
		{
			result.X = (left.X < right.X) ? left.X : right.X;
			result.Y = (left.Y < right.Y) ? left.Y : right.Y;
		}
		/// <summary>
		/// Returns a vector containing the smallest components of the specified vectors.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>A vector containing the smallest components of the source vectors.</returns>
		public static Vector2 Min(Vector2 left, Vector2 right)
		{
			Vector2 result;
			Min(ref left, ref right, out result);
			return result;
		}
		#endregion
		#region Normalizations
		/// <summary>
		/// Converts the vector into a unit vector.
		/// </summary>
		public void Normalize()
		{
			float length = Length;
			if (length > MathHelpers.ZeroTolerance)
			{
				float inv = 1.0f / length;
				X *= inv;
				Y *= inv;
			}
		}
		/// <summary>
		/// Converts the vector into a unit vector.
		/// </summary>
		/// <param name="value">The vector to normalize.</param>
		/// <param name="result">When the method completes, contains the normalized vector.</param>
		public static void Normalize(ref Vector2 value, out Vector2 result)
		{
			result = value;
			result.Normalize();
		}
		/// <summary>
		/// Converts the vector into a unit vector.
		/// </summary>
		/// <param name="value">The vector to normalize.</param>
		/// <returns>The normalized vector.</returns>
		public static Vector2 Normalize(Vector2 value)
		{
			value.Normalize();
			return value;
		}
		#endregion
		#region Arithmetic Operations
		/// <summary>
		/// Modulates a vector with another by performing component-wise multiplication.
		/// </summary>
		/// <param name="left">The first vector to modulate.</param>
		/// <param name="right">The second vector to modulate.</param>
		/// <param name="result">When the method completes, contains the modulated vector.</param>
		public static void Modulate(ref Vector2 left, ref Vector2 right, out Vector2 result)
		{
			result = new Vector2(left.X * right.X, left.Y * right.Y);
		}
		/// <summary>
		/// Modulates a vector with another by performing component-wise multiplication.
		/// </summary>
		/// <param name="left">The first vector to modulate.</param>
		/// <param name="right">The second vector to modulate.</param>
		/// <returns>The modulated vector.</returns>
		public static Vector2 Modulate(Vector2 left, Vector2 right)
		{
			return new Vector2(left.X * right.X, left.Y * right.Y);
		}
		#endregion
		#region Interpolations
		/// <summary>
		/// Performs a linear interpolation between two vectors.
		/// </summary>
		/// <param name="start">Start vector.</param>
		/// <param name="end">End vector.</param>
		/// <param name="amount">
		/// Value between 0 and 1 indicating the weight of <paramref name="end" />.
		/// </param>
		/// <param name="result">
		/// When the method completes, contains the linear interpolation of the two vectors.
		/// </param>
		/// <remarks>
		/// This method performs the linear interpolation based on the following formula.
		/// <code>start + (end - start) * amount</code> Passing <paramref name="amount" /> a value
		/// of 0 will cause <paramref name="start" /> to be returned; a value of 1 will cause
		/// <paramref name="end" /> to be returned.
		/// </remarks>
		public static void CreateLinearInterpolation(ref Vector2 start, ref Vector2 end, float amount, out Vector2 result)
		{
			result.X = start.X + ((end.X - start.X) * amount);
			result.Y = start.Y + ((end.Y - start.Y) * amount);
		}
		/// <summary>
		/// Performs a linear interpolation between two vectors.
		/// </summary>
		/// <param name="start">Start vector.</param>
		/// <param name="end">End vector.</param>
		/// <param name="amount">
		/// Value between 0 and 1 indicating the weight of <paramref name="end" />.
		/// </param>
		/// <returns>The linear interpolation of the two vectors.</returns>
		/// <remarks>
		/// This method performs the linear interpolation based on the following formula.
		/// <code>start + (end - start) * amount</code> Passing <paramref name="amount" /> a value
		/// of 0 will cause <paramref name="start" /> to be returned; a value of 1 will cause
		/// <paramref name="end" /> to be returned.
		/// </remarks>
		public static Vector2 CreateLinearInterpolation(Vector2 start, Vector2 end, float amount)
		{
			Vector2 result;
			CreateLinearInterpolation(ref start, ref end, amount, out result);
			return result;
		}
		/// <summary>
		/// Performs a cubic interpolation between two vectors.
		/// </summary>
		/// <param name="start">Start vector.</param>
		/// <param name="end">End vector.</param>
		/// <param name="amount">
		/// Value between 0 and 1 indicating the weight of <paramref name="end" />.
		/// </param>
		/// <param name="result">
		/// When the method completes, contains the cubic interpolation of the two vectors.
		/// </param>
		public static void CreateCubicInterpolation(ref Vector2 start, ref Vector2 end, float amount, out Vector2 result)
		{
			amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
			amount = (amount * amount) * (3.0f - (2.0f * amount));

			result.X = start.X + ((end.X - start.X) * amount);
			result.Y = start.Y + ((end.Y - start.Y) * amount);
		}
		/// <summary>
		/// Performs a cubic interpolation between two vectors.
		/// </summary>
		/// <param name="start">Start vector.</param>
		/// <param name="end">End vector.</param>
		/// <param name="amount">
		/// Value between 0 and 1 indicating the weight of <paramref name="end" />.
		/// </param>
		/// <returns>The cubic interpolation of the two vectors.</returns>
		public static Vector2 CreateCubicInterpolation(Vector2 start, Vector2 end, float amount)
		{
			Vector2 result;
			CreateCubicInterpolation(ref start, ref end, amount, out result);
			return result;
		}
		/// <summary>
		/// Performs a Hermite spline interpolation.
		/// </summary>
		/// <param name="value1">First source position vector.</param>
		/// <param name="tangent1">First source tangent vector.</param>
		/// <param name="value2">Second source position vector.</param>
		/// <param name="tangent2">Second source tangent vector.</param>
		/// <param name="amount">Weighting factor.</param>
		/// <param name="result">
		/// When the method completes, contains the result of the Hermite spline interpolation.
		/// </param>
		public static void CreateHermiteInterpolation(ref Vector2 value1, ref Vector2 tangent1, ref Vector2 value2, ref Vector2 tangent2, float amount, out Vector2 result)
		{
			float squared = amount * amount;
			float cubed = amount * squared;
			float part1 = ((2.0f * cubed) - (3.0f * squared)) + 1.0f;
			float part2 = (-2.0f * cubed) + (3.0f * squared);
			float part3 = (cubed - (2.0f * squared)) + amount;
			float part4 = cubed - squared;

			result.X = (((value1.X * part1) + (value2.X * part2)) + (tangent1.X * part3)) + (tangent2.X * part4);
			result.Y = (((value1.Y * part1) + (value2.Y * part2)) + (tangent1.Y * part3)) + (tangent2.Y * part4);
		}
		/// <summary>
		/// Performs a Hermite spline interpolation.
		/// </summary>
		/// <param name="value1">First source position vector.</param>
		/// <param name="tangent1">First source tangent vector.</param>
		/// <param name="value2">Second source position vector.</param>
		/// <param name="tangent2">Second source tangent vector.</param>
		/// <param name="amount">Weighting factor.</param>
		/// <returns>The result of the Hermite spline interpolation.</returns>
		public static Vector2 CreateHermiteInterpolation(Vector2 value1, Vector2 tangent1, Vector2 value2, Vector2 tangent2, float amount)
		{
			Vector2 result;
			CreateHermiteInterpolation(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
			return result;
		}
		/// <summary>
		/// Performs a Catmull-Rom interpolation using the specified positions.
		/// </summary>
		/// <param name="value1">The first position in the interpolation.</param>
		/// <param name="value2">The second position in the interpolation.</param>
		/// <param name="value3">The third position in the interpolation.</param>
		/// <param name="value4">The fourth position in the interpolation.</param>
		/// <param name="amount">Weighting factor.</param>
		/// <param name="result">
		/// When the method completes, contains the result of the Catmull-Rom interpolation.
		/// </param>
		public static void CreateCatmullRomInterpolation(ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, ref Vector2 value4, float amount, out Vector2 result)
		{
			float squared = amount * amount;
			float cubed = amount * squared;

			result.X = 0.5f * ((((2.0f * value2.X) + ((-value1.X + value3.X) * amount)) +
			(((((2.0f * value1.X) - (5.0f * value2.X)) + (4.0f * value3.X)) - value4.X) * squared)) +
			((((-value1.X + (3.0f * value2.X)) - (3.0f * value3.X)) + value4.X) * cubed));

			result.Y = 0.5f * ((((2.0f * value2.Y) + ((-value1.Y + value3.Y) * amount)) +
				(((((2.0f * value1.Y) - (5.0f * value2.Y)) + (4.0f * value3.Y)) - value4.Y) * squared)) +
				((((-value1.Y + (3.0f * value2.Y)) - (3.0f * value3.Y)) + value4.Y) * cubed));
		}
		/// <summary>
		/// Performs a Catmull-Rom interpolation using the specified positions.
		/// </summary>
		/// <param name="value1">The first position in the interpolation.</param>
		/// <param name="value2">The second position in the interpolation.</param>
		/// <param name="value3">The third position in the interpolation.</param>
		/// <param name="value4">The fourth position in the interpolation.</param>
		/// <param name="amount">Weighting factor.</param>
		/// <returns>A vector that is the result of the Catmull-Rom interpolation.</returns>
		public static Vector2 CreateCatmullRomInterpolation(Vector2 value1, Vector2 value2, Vector2 value3, Vector2 value4, float amount)
		{
			Vector2 result;
			CreateCatmullRomInterpolation(ref value1, ref value2, ref value3, ref value4, amount, out result);
			return result;
		}
		#endregion
		#region Reflections
		/// <summary>
		/// Returns the reflection of a vector off a surface that has the specified normal.
		/// </summary>
		/// <param name="vector">The source vector.</param>
		/// <param name="normal">Normal of the surface.</param>
		/// <param name="result">When the method completes, contains the reflected vector.</param>
		/// <remarks>
		/// Reflect only gives the direction of a reflection off a surface, it does not determine
		/// whether the original vector was close enough to the surface to hit it.
		/// </remarks>
		public static void Reflect(ref Vector2 vector, ref Vector2 normal, out Vector2 result)
		{
			float dot = (vector.X * normal.X) + (vector.Y * normal.Y);

			result.X = vector.X - ((2.0f * dot) * normal.X);
			result.Y = vector.Y - ((2.0f * dot) * normal.Y);
		}
		/// <summary>
		/// Returns the reflection of a vector off a surface that has the specified normal.
		/// </summary>
		/// <param name="vector">The source vector.</param>
		/// <param name="normal">Normal of the surface.</param>
		/// <returns>The reflected vector.</returns>
		/// <remarks>
		/// Reflect only gives the direction of a reflection off a surface, it does not determine
		/// whether the original vector was close enough to the surface to hit it.
		/// </remarks>
		public static Vector2 Reflect(Vector2 vector, Vector2 normal)
		{
			Vector2 result;
			Reflect(ref vector, ref normal, out result);
			return result;
		}
		#endregion
		#region Operators
		#region Arithmetic Operators
		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="left">The first vector to add.</param>
		/// <param name="right">The second vector to add.</param>
		/// <returns>The sum of the two vectors.</returns>
		public static Vector2 operator +(Vector2 left, Vector2 right)
		{
			return new Vector2(left.X + right.X, left.Y + right.Y);
		}
		/// <summary>
		/// Assert a vector (return it unchanged).
		/// </summary>
		/// <param name="value">The vector to assert (unchange).</param>
		/// <returns>The asserted (unchanged) vector.</returns>
		public static Vector2 operator +(Vector2 value)
		{
			return value;
		}
		/// <summary>
		/// Subtracts two vectors.
		/// </summary>
		/// <param name="left">The first vector to subtract.</param>
		/// <param name="right">The second vector to subtract.</param>
		/// <returns>The difference of the two vectors.</returns>
		public static Vector2 operator -(Vector2 left, Vector2 right)
		{
			return new Vector2(left.X - right.X, left.Y - right.Y);
		}
		/// <summary>
		/// Reverses the direction of a given vector.
		/// </summary>
		/// <param name="value">The vector to negate.</param>
		/// <returns>A vector facing in the opposite direction.</returns>
		public static Vector2 operator -(Vector2 value)
		{
			return new Vector2(-value.X, -value.Y);
		}
		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="value">The vector to scale.</param>
		/// <param name="scale">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		public static Vector2 operator *(float scale, Vector2 value)
		{
			return new Vector2(value.X * scale, value.Y * scale);
		}
		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="value">The vector to scale.</param>
		/// <param name="scale">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		public static Vector2 operator *(Vector2 value, float scale)
		{
			return new Vector2(value.X * scale, value.Y * scale);
		}
		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="value">The vector to scale.</param>
		/// <param name="scale">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		public static Vector2 operator /(Vector2 value, float scale)
		{
			return new Vector2(value.X / scale, value.Y / scale);
		}
		#endregion
		#region Comparison Operators
		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="left" /> has the same value as <paramref name="right" />;
		/// otherwise, <c>false</c>.
		/// </returns>
		public static bool operator ==(Vector2 left, Vector2 right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="left" /> has a different value than <paramref
		/// name="right" />; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=(Vector2 left, Vector2 right)
		{
			return !left.Equals(right);
		}
		#endregion
		#region Conversion Operators
		/// <summary>
		/// Performs an explicit conversion from <see cref="Vector2" /> to <see
		/// cref="CryEngine.Vector3" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator Vector3(Vector2 value)
		{
			return new Vector3(value);
		}
		/// <summary>
		/// Performs an explicit conversion from <see cref="Vector2" /> to <see cref="Vector4" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator Vector4(Vector2 value)
		{
			return new Vector4(value, 0.0f, 0.0f);
		}
		#endregion
		#endregion
		#region Text Conversions
		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1}", X, Y);
		}
		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public string ToString(string format)
		{
			if (format == null)
				return ToString();

			return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1}", X.ToString(format, CultureInfo.CurrentCulture), Y.ToString(format, CultureInfo.CurrentCulture));
		}
		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public string ToString(IFormatProvider formatProvider)
		{
			return string.Format(formatProvider, "X:{0} Y:{1}", X, Y);
		}
		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (format == null)
				ToString(formatProvider);

			return string.Format(formatProvider, "X:{0} Y:{1}", X.ToString(format, formatProvider), Y.ToString(format, formatProvider));
		}
		#endregion
		#region Equality Checks
		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data
		/// structures like a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = 17;

				// ReSharper disable NonReadonlyFieldInGetHashCode
				hash = hash * 29 + X.GetHashCode();
				hash = hash * 29 + Y.GetHashCode();
				// ReSharper restore NonReadonlyFieldInGetHashCode

				return hash;
			}
		}
		/// <summary>
		/// Determines whether the specified <see cref="Vector2" /> is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="Vector2" /> to compare with this instance.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Vector2" /> is equal to this instance;
		/// otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(Vector2 other)
		{
			// ReSharper disable CompareOfFloatsByEqualityOperator
			return this.X == other.X && (Y == other.Y);
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}
		/// <summary>
		/// Determines whether the specified <see cref="Vector2" /> is equal to this instance using
		/// an epsilon value.
		/// </summary>
		/// <param name="other">The <see cref="Vector2" /> to compare with this instance.</param>
		/// <param name="epsilon">The amount of error allowed.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Vector2" /> is equal to this instance;
		/// otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(Vector2 other, float epsilon)
		{
			return (Math.Abs(other.X - X) < epsilon &&
				Math.Abs(other.Y - Y) < epsilon);
		}
		/// <summary>
		/// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
		/// </summary>
		/// <param name="value">
		/// The <see cref="System.Object" /> to compare with this instance.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance;
		/// otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object value)
		{
			return value != null && value.GetType() == this.GetType() && this.Equals((Vector2)value);
		}
		#endregion
		#region Enumerations
		/// <summary>
		/// Enumerates this vector.
		/// </summary>
		/// <returns>Yields components of this vector.</returns>
		public IEnumerator<float> GetEnumerator()
		{
			yield return this.X;
			yield return this.Y;
		}
		/// <summary>
		/// Enumerates this vector.
		/// </summary>
		/// <returns>Yields components of this vector.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			yield return this.X;
			yield return this.Y;
		}
		#endregion
		#endregion
		/// <summary>
		/// Handles transfer of arrays of type <see cref="Vector2" /> between managed and native memory.
		/// </summary>
		public class TransferAgent : ITransferAgent<Vector2>
		{
			/// <summary>
			/// Determines size of native memory cluster that can fit a collection of objects.
			/// </summary>
			/// <param name="objects">A list of objects of type <see cref="Vector2" />.</param>
			/// <returns>
			/// Number of bytes that would be occupied by given collection of objects.
			/// </returns>
			public ulong GetBytesNumber(IList<Vector2> objects)
			{
				return this.GetBytesNumber((ulong)objects.Count);
			}
			/// <summary>
			/// Determines size of native memory cluster that can fit a number of objects.
			/// </summary>
			/// <param name="objectsCount">A number of objects.</param>
			/// <returns>Number of bytes that would be occupied by given number of objects.</returns>
			public ulong GetBytesNumber(ulong objectsCount)
			{
				return Vector2.ByteCount * objectsCount;
			}
			/// <summary>
			/// Gets number of vectors stored in native memory cluster.
			/// </summary>
			/// <param name="handle">Address of first byte of native memory cluster.</param>
			/// <param name="offset">
			/// Zero-based index of first byte within native memory cluster from which to start
			/// counting vectors.
			/// </param>
			/// <param name="size">Size of native memory cluster in bytes from first byte.</param>
			/// <returns>Number of vectors in [handle + offset; handle + size].</returns>
			public ulong GetObjectsNumber(IntPtr handle, ulong offset, ulong size)
			{
				return (size - offset) / Vector2.ByteCount;
			}
			/// <summary>
			/// Writes a collection of vectors to native memory.
			/// </summary>
			/// <param name="stream">Stream to which to write vectors.</param>
			/// <param name="objects">A list of vectors to write.</param>
			/// <returns>Number of bytes written.</returns>
			public ulong Write(NativeMemoryStream stream, IList<Vector2> objects)
			{
				ulong bytesWritten = 0;
				foreach (Vector2 vector2 in objects)
				{
					stream.Write(new Bytes8(vector2.X, vector2.Y));
					bytesWritten += Vector2.ByteCount;
				}
				return bytesWritten;
			}
			/// <summary>
			/// Reads vectors from native memory stream and stores it in a collection.
			/// </summary>
			/// <param name="stream">Stream from which to read the vectors.</param>
			/// <param name="objects">A collection of vectors where to put the objects.</param>
			/// <param name="index">
			/// Index of the first position inside a collection to which to put vectors. -1 to write
			/// to the end of the list.
			/// </param>
			/// <param name="count">Number of vectors to read, 0 to read everything.</param>
			/// <returns>Number of read vectors.</returns>
			public int Read(NativeMemoryStream stream, IList<Vector2> objects, int index = -1, int count = 0)
			{
				int objectsToRead;
				if (count == 0)
				{
					objectsToRead = (int)this.GetObjectsNumber(IntPtr.Zero, stream.Position, stream.Length);
				}
				else
				{
					objectsToRead = count;
				}
				int i;
				for (i = index == -1 ? objects.Count : index; i < objectsToRead; i++)
				{
					objects.Insert(i, new Vector2
					{
						X = stream.Read4().SingleFloat,
						Y = stream.Read4().SingleFloat
					});
				}
				return i - index == -1 ? objects.Count : index;
			}
		}
	}
}