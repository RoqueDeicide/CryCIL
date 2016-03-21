using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CryCil
{
	/// <summary>
	/// Represents a two dimensional mathematical vector that uses double precision floating point numbers
	/// for coordinates.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public partial struct Vector2d : IVector<double, Vector2d>, IEquatable<Vector2d>, IFormattable, IEnumerable<double>,
									 IComparable<Vector2d>
	{
		#region Static Fields
		/// <summary>
		/// Vector that represents an origin of coordinates.
		/// </summary>
		public static readonly Vector2d Zero = new Vector2d(0, 0);
		/// <summary>
		/// Number of bytes each instance of this structure consists of.
		/// </summary>
		public static readonly int ByteCount = Marshal.SizeOf(typeof(Vector2d));
		#endregion
		#region Fields
		/// <summary>
		/// The X component of the vector.
		/// </summary>
		public double X;
		/// <summary>
		/// The Y component of the vector.
		/// </summary>
		public double Y;
		#endregion
		#region Properties
		/// <summary>
		/// Gets a value indicting whether this instance is normalized.
		/// </summary>
		public bool IsNormalized => Math.Abs(this.X * this.X + this.Y * this.Y - 1f) < MathHelpers.ZeroTolerance;
		/// <summary>
		/// Gets or sets the component at the specified index.
		/// </summary>
		/// <value>The value of the X or Y component, depending on the index.</value>
		/// <param name="index">
		/// The index of the component to access. Use 0 for the X component and 1 for the Y component.
		/// </param>
		/// <returns>The value of the component at the specified index.</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Attempt to access vector component other then X or Y.
		/// </exception>
		public unsafe double this[int index]
		{
			get
			{
				if ((index | 0x1) != 0x1) //index < 0 || index > 1
				{
					throw new ArgumentOutOfRangeException(nameof(index),
														  "Attempt to access vector component other then X or Y.");
				}
				Contract.EndContractBlock();

				fixed (double* ptr = &this.X)
				{
					return ptr[index];
				}
			}
			set
			{
				if ((index | 0x1) != 0x1) //index < 0 || index > 1
				{
					throw new ArgumentOutOfRangeException(nameof(index), "Attempt to access vector component other then X or Y.");
				}
				Contract.EndContractBlock();

				fixed (double* ptr = &this.X)
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
		/// <see cref="Vector2d.LengthSquared"/> may be preferred when only the relative length is needed
		/// and speed is of the essence.
		/// </remarks>
		public double Length => Math.Sqrt(this.X * this.X + this.Y * this.Y);
		/// <summary>
		/// Calculates the squared length of the vector.
		/// </summary>
		/// <returns>The squared length of the vector.</returns>
		/// <remarks>
		/// This method may be preferred to <see cref="Vector2d.Length"/> when only a relative length is
		/// needed and speed is of the essence.
		/// </remarks>
		public double LengthSquared => this.X * this.X + this.Y * this.Y;
		/// <summary>
		/// Determines whether this vector is represented by valid numbers.
		/// </summary>
		public bool IsValid => MathHelpers.IsNumberValid(this.X) && MathHelpers.IsNumberValid(this.Y);
		#endregion
		#region Interface
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Vector2d"/> struct.
		/// </summary>
		/// <param name="value">The value that will be assigned to all components.</param>
		public Vector2d(double value)
		{
			this.X = value;
			this.Y = value;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Vector2d"/> struct.
		/// </summary>
		/// <param name="x">Initial value for the X component of the vector.</param>
		/// <param name="y">Initial value for the Y component of the vector.</param>
		public Vector2d(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Vector2d"/> struct.
		/// </summary>
		/// <param name="values">
		/// An array which contents are used to create new vector. Only first two values can be used. If
		/// array only contains one value, it will be assigned to both components of the vector. If array
		/// doesn't contain any values, vector will be initialized with zeros.
		/// </param>
		public Vector2d(IList<double> values)
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
		/// Creates new instance of <see cref="Vector2d"/> where Y, Z and W components are of this instance
		/// and X component is specified by given value.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="value">New value for X component.</param>
		/// <returns>Modified vector.</returns>
		public Vector2d ModifyX(double value)
		{
			return new Vector2d(value, this.Y);
		}
		/// <summary>
		/// Creates new instance of <see cref="Vector2d"/> where X, Z and W components are of this instance
		/// and Y component is specified by given value.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="value">New value for Y component.</param>
		/// <returns>Modified vector.</returns>
		public Vector2d ModifyY(double value)
		{
			return new Vector2d(this.X, value);
		}
		/// <summary>
		/// Creates new <see cref="Vector2d"/> which represents this vector with modified values.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="x">New value for X component.</param>
		/// <param name="y">New value for Y component.</param>
		/// <returns>Modified vector.</returns>
		public Vector2d ModifyVector(double x, double y)
		{
			return new Vector2d(x, y);
		}
		#endregion
		#region Conversions To Collections
		/// <summary>
		/// Creates an array containing the elements of the vector.
		/// </summary>
		/// <returns>A two-element array containing the components of the vector.</returns>
		public double[] ToArray()
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
		public List<double> ToList()
		{
			List<double> result = new List<double>(2) {this.X, this.Y};
			return result;
		}
		/// <summary>
		/// Creates a dictionary that contains components of this vector.
		/// </summary>
		/// <returns>
		/// A dictionary which capacity is set to 2 where components of the vector can be accessed with keys
		/// of same names.
		/// </returns>
		public Dictionary<string, double> ToDictionary()
		{
			Dictionary<string, double> result = new Dictionary<string, double>(2) {{"X", this.X}, {"Y", this.Y}};
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
		public static void Clamp(ref Vector2d value, ref Vector2d min, ref Vector2d max, out Vector2d result)
		{
			double x = value.X;
			x = x > max.X ? max.X : x;
			x = x < min.X ? min.X : x;

			double y = value.Y;
			y = y > max.Y ? max.Y : y;
			y = y < min.Y ? min.Y : y;

			result = new Vector2d(x, y);
		}
		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <param name="min">  The minimum value.</param>
		/// <param name="max">  The maximum value.</param>
		/// <returns>The clamped value.</returns>
		public static Vector2d Clamp(Vector2d value, Vector2d min, Vector2d max)
		{
			Vector2d result;
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
		/// <see cref="Vector2d.DistanceSquared(ref Vector2d, ref Vector2d, out double)"/> may be preferred
		/// when only the relative distance is needed and speed is of the essence.
		/// </remarks>
		public static void Distance(ref Vector2d value1, ref Vector2d value2, out double result)
		{
			double x = value1.X - value2.X;
			double y = value1.Y - value2.Y;

			result = Math.Sqrt(x * x + y * y);
		}
		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		/// <param name="value1">The first vector.</param>
		/// <param name="value2">The second vector.</param>
		/// <returns>The distance between the two vectors.</returns>
		/// <remarks>
		/// <see cref="Vector2d.DistanceSquared(Vector2d, Vector2d)"/> may be preferred when only the
		/// relative distance is needed and speed is of the essence.
		/// </remarks>
		public static double Distance(Vector2d value1, Vector2d value2)
		{
			double x = value1.X - value2.X;
			double y = value1.Y - value2.Y;

			return Math.Sqrt(x * x + y * y);
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
		/// A, B, and C. To determine whether B or C is further from A, compare the distance between A and B
		/// to the distance between A and C. Calculating the two distances involves two square roots, which
		/// are computationally expensive. However, using distance squared provides the same information and
		/// avoids calculating two square roots.
		/// </remarks>
		public static void DistanceSquared(ref Vector2d value1, ref Vector2d value2, out double result)
		{
			double x = value1.X - value2.X;
			double y = value1.Y - value2.Y;

			result = x * x + y * y;
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
		/// A, B, and C. To determine whether B or C is further from A, compare the distance between A and B
		/// to the distance between A and C. Calculating the two distances involves two square roots, which
		/// are computationally expensive. However, using distance squared provides the same information and
		/// avoids calculating two square roots.
		/// </remarks>
		public static double DistanceSquared(Vector2d value1, Vector2d value2)
		{
			double x = value1.X - value2.X;
			double y = value1.Y - value2.Y;

			return x * x + y * y;
		}
		/// <summary>
		/// Calculates the dot product of two vectors.
		/// </summary>
		/// <param name="left"> First source vector.</param>
		/// <param name="right">Second source vector.</param>
		/// <returns>The dot product of the two vectors.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Dot(Vector2d left, Vector2d right)
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
		public static void Max(ref Vector2d left, ref Vector2d right, out Vector2d result)
		{
			result.X = left.X > right.X ? left.X : right.X;
			result.Y = left.Y > right.Y ? left.Y : right.Y;
		}
		/// <summary>
		/// Returns a vector containing the largest components of the specified vectors.
		/// </summary>
		/// <param name="left"> The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>A vector containing the largest components of the source vectors.</returns>
		public static Vector2d Max(Vector2d left, Vector2d right)
		{
			Vector2d result;
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
		public static void Min(ref Vector2d left, ref Vector2d right, out Vector2d result)
		{
			result.X = left.X < right.X ? left.X : right.X;
			result.Y = left.Y < right.Y ? left.Y : right.Y;
		}
		/// <summary>
		/// Returns a vector containing the smallest components of the specified vectors.
		/// </summary>
		/// <param name="left"> The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>A vector containing the smallest components of the source vectors.</returns>
		public static Vector2d Min(Vector2d left, Vector2d right)
		{
			Vector2d result;
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
			double length = this.Length;
			if (length < MathHelpers.ZeroTolerance) return;
			double inv = 1.0f / length;
			this.X *= inv;
			this.Y *= inv;
		}
		/// <summary>
		/// Converts the vector into a unit vector.
		/// </summary>
		/// <param name="value"> The vector to normalize.</param>
		/// <param name="result">When the method completes, contains the normalized vector.</param>
		public static void Normalize(ref Vector2d value, out Vector2d result)
		{
			result = value;
			result.Normalize();
		}
		/// <summary>
		/// Converts the vector into a unit vector.
		/// </summary>
		/// <param name="value">The vector to normalize.</param>
		/// <returns>The normalized vector.</returns>
		public static Vector2d Normalize(Vector2d value)
		{
			value.Normalize();
			return value;
		}
		#endregion
		#endregion
	}
}