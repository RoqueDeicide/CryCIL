using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CryCil
{
	/// <summary>
	/// Represents a two dimensional mathematical vector.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public partial struct Vector2 : IVector<float, Vector2>, IEquatable<Vector2>, IFormattable, IEnumerable<float>,
									IComparable<Vector2>
	{
		#region Static Fields
		/// <summary>
		/// Vector that represents an origin of coordinates.
		/// </summary>
		public static readonly Vector2 Zero = new Vector2(0, 0);
		/// <summary>
		/// Number of bytes each instance of this structure consists of.
		/// </summary>
		public static readonly int ByteCount = Marshal.SizeOf(typeof(Vector2));
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
			get { return Math.Abs((this.X * this.X) + (this.Y * this.Y) - 1f) < MathHelpers.ZeroTolerance; }
		}
		/// <summary>
		/// Gets or sets the component at the specified index.
		/// </summary>
		/// <value>The value of the X or Y component, depending on the index.</value>
		/// <param name="index">
		/// The index of the component to access. Use 0 for the X component and 1 for the Y component.
		/// </param>
		/// <returns>The value of the component at the specified index.</returns>
		public unsafe float this[int index]
		{
			get
			{
				if ((index | 0x1) != 0x1) //index < 0 || index > 1
				{
					throw new ArgumentOutOfRangeException("index", "Attempt to access vector" +
																   " component other then X or Y.");
				}
				Contract.EndContractBlock();

				fixed (float* ptr = &this.X)
				{
					return ptr[index];
				}
			}
			set
			{
				if ((index | 0x1) != 0x1) //index < 0 || index > 1
				{
					throw new ArgumentOutOfRangeException("index", "Attempt to access vector" +
																   " component other then X or Y.");
				}
				Contract.EndContractBlock();

				fixed (float* ptr = &this.X)
				{
					ptr[index] = value;
				}
			}
		}
		/// <summary>
		/// Calculates the length of the vector.
		/// </summary>
		/// <returns>The length of the vector.</returns>
		/// <remarks>
		/// <see cref="Vector2.LengthSquared"/> may be preferred when only the relative length is needed
		/// and speed is of the essence.
		/// </remarks>
		public float Length
		{
			get { return (float)Math.Sqrt((this.X * this.X) + (this.Y * this.Y)); }
		}
		/// <summary>
		/// Calculates the squared length of the vector.
		/// </summary>
		/// <returns>The squared length of the vector.</returns>
		/// <remarks>
		/// This method may be preferred to <see cref="Vector2.Length"/> when only a relative length is
		/// needed and speed is of the essence.
		/// </remarks>
		public float LengthSquared
		{
			get { return (this.X * this.X) + (this.Y * this.Y); }
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
		/// Initializes a new instance of the <see cref="Vector2"/> struct.
		/// </summary>
		/// <param name="value">The value that will be assigned to all components.</param>
		public Vector2(float value)
		{
			this.X = value;
			this.Y = value;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Vector2"/> struct.
		/// </summary>
		/// <param name="x">Initial value for the X component of the vector.</param>
		/// <param name="y">Initial value for the Y component of the vector.</param>
		public Vector2(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Vector2"/> struct.
		/// </summary>
		/// <param name="values">
		/// An array which contents are used to create new vector. Only first two values can be used. If
		/// array only contains one value, it will be assigned to both components of the vector. If array
		/// doesn't contain any values, vector will be initialized with zeros.
		/// </param>
		public Vector2(IList<float> values)
		{
			if (values.Count >= 2)
			{
				this.X = values[0];
				this.Y = values[1];
			}
			else if (values.Count == 1)
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
		/// Creates new instance of <see cref="Vector2"/> where Y, Z and W components are of this instance
		/// and X component is specified by given value.
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
		/// Creates new instance of <see cref="Vector2"/> where X, Z and W components are of this instance
		/// and Y component is specified by given value.
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
		/// Creates new <see cref="Vector2"/> which represents this vector with modified values.
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
			return new[] {this.X, this.Y};
		}
		/// <summary>
		/// Creates a list that contains components of this vector.
		/// </summary>
		/// <returns>
		/// A list which capacity is set to 2 with first element being an X-component of this vector, and
		/// second element being an Y-component of this vector.
		/// </returns>
		public List<float> ToList()
		{
			List<float> result = new List<float>(2) {this.X, this.Y};
			return result;
		}
		/// <summary>
		/// Creates a dictionary that contains components of this vector.
		/// </summary>
		/// <returns>
		/// A dictionary which capacity is set to 2 where components of the vector can be accessed with
		/// keys of same names.
		/// </returns>
		public Dictionary<string, float> ToDictionary()
		{
			Dictionary<string, float> result = new Dictionary<string, float>(2) {{"X", this.X}, {"Y", this.Y}};
			return result;
		}
		#endregion
		#region Generic Mathematics
		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value"> The value to clamp.</param>
		/// <param name="min">   The minimum value.</param>
		/// <param name="max">   The maximum value.</param>
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
		/// <param name="min">  The minimum value.</param>
		/// <param name="max">  The maximum value.</param>
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
		/// <see cref="Vector2.DistanceSquared(ref Vector2, ref Vector2, out float)"/> may be preferred
		/// when only the relative distance is needed and speed is of the essence.
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
		/// <see cref="Vector2.DistanceSquared(Vector2, Vector2)"/> may be preferred when only the relative
		/// distance is needed and speed is of the essence.
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
		/// Distance squared is the value before taking the square root. Distance squared can often be used
		/// in place of distance if relative comparisons are being made. For example, consider three points
		/// A, B, and C. To determine whether B or C is further from A, compare the distance between A and
		/// B to the distance between A and C. Calculating the two distances involves two square roots,
		/// which are computationally expensive. However, using distance squared provides the same
		/// information and avoids calculating two square roots.
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
		/// Distance squared is the value before taking the square root. Distance squared can often be used
		/// in place of distance if relative comparisons are being made. For example, consider three points
		/// A, B, and C. To determine whether B or C is further from A, compare the distance between A and
		/// B to the distance between A and C. Calculating the two distances involves two square roots,
		/// which are computationally expensive. However, using distance squared provides the same
		/// information and avoids calculating two square roots.
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
		/// <param name="left"> First source vector.</param>
		/// <param name="right">Second source vector.</param>
		/// <returns>The dot product of the two vectors.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Dot(Vector2 left, Vector2 right)
		{
			return left.X * right.X + left.Y * right.Y;
		}
		/// <summary>
		/// Returns a vector containing the smallest components of the specified vectors.
		/// </summary>
		/// <param name="left">  The first source vector.</param>
		/// <param name="right"> The second source vector.</param>
		/// <param name="result">
		/// When the method completes, contains an new vector composed of the largest components of the
		/// source vectors.
		/// </param>
		public static void Max(ref Vector2 left, ref Vector2 right, out Vector2 result)
		{
			result.X = (left.X > right.X) ? left.X : right.X;
			result.Y = (left.Y > right.Y) ? left.Y : right.Y;
		}
		/// <summary>
		/// Returns a vector containing the largest components of the specified vectors.
		/// </summary>
		/// <param name="left"> The first source vector.</param>
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
		/// <param name="left">  The first source vector.</param>
		/// <param name="right"> The second source vector.</param>
		/// <param name="result">
		/// When the method completes, contains an new vector composed of the smallest components of the
		/// source vectors.
		/// </param>
		public static void Min(ref Vector2 left, ref Vector2 right, out Vector2 result)
		{
			result.X = (left.X < right.X) ? left.X : right.X;
			result.Y = (left.Y < right.Y) ? left.Y : right.Y;
		}
		/// <summary>
		/// Returns a vector containing the smallest components of the specified vectors.
		/// </summary>
		/// <param name="left"> The first source vector.</param>
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
			float length = this.Length;
			if (length < MathHelpers.ZeroTolerance) return;
			float inv = 1.0f / length;
			this.X *= inv;
			this.Y *= inv;
		}
		/// <summary>
		/// Converts the vector into a unit vector.
		/// </summary>
		/// <param name="value"> The vector to normalize.</param>
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
		#endregion
	}
}