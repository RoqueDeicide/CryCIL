using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace CryCil.Geometry
{
	/// <summary>
	/// Represents a two dimensional mathematical vector.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Vector2 : IEquatable<Vector2>, IFormattable, IEnumerable<float>, IComparable<Vector2>
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
		/// The index of the component to access. Use 0 for the X component and 1 for the
		/// Y component.
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
					case 0: return this.X;
					case 1: return this.Y;
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
					case 0: this.X = value; break;
					case 1: this.Y = value; break;
				}
			}
		}
		/// <summary>
		/// Calculates the length of the vector.
		/// </summary>
		/// <returns>The length of the vector.</returns>
		/// <remarks>
		/// <see cref="Vector2.LengthSquared"/> may be preferred when only the relative
		/// length is needed and speed is of the essence.
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
		/// This method may be preferred to <see cref="Vector2.Length"/> when only a
		/// relative length is needed and speed is of the essence.
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
		/// An array which contents are used to create new vector. Only first two values
		/// can be used. If array only contains one value, it will be assigned to both
		/// components of the vector. If array doesn't contain any values, vector will be
		/// initialized with zeros.
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
		/// Creates new instance of <see cref="Vector2"/> where Y, Z and W components are
		/// of this instance and X component is specified by given value.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which
		/// you want to modify.
		/// </remarks>
		/// <param name="value">New value for X component.</param>
		/// <returns>Modified vector.</returns>
		public Vector2 ModifyX(float value)
		{
			return new Vector2(value, this.Y);
		}
		/// <summary>
		/// Creates new instance of <see cref="Vector2"/> where X, Z and W components are
		/// of this instance and Y component is specified by given value.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which
		/// you want to modify.
		/// </remarks>
		/// <param name="value">New value for Y component.</param>
		/// <returns>Modified vector.</returns>
		public Vector2 ModifyY(float value)
		{
			return new Vector2(this.X, value);
		}
		/// <summary>
		/// Creates new <see cref="Vector2"/> which represents this vector with modified
		/// values.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which
		/// you want to modify.
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
		/// <returns>
		/// A two-element array containing the components of the vector.
		/// </returns>
		public float[] ToArray()
		{
			return new[] { this.X, this.Y };
		}
		/// <summary>
		/// Creates a list that contains components of this vector.
		/// </summary>
		/// <returns>
		/// A list which capacity is set to 2 with first element being an X-component of
		/// this vector, and second element being an Y-component of this vector.
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
		/// A dictionary which capacity is set to 2 where components of the vector can be
		/// accessed with keys of same names.
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
		/// <param name="value"> The value to clamp.</param>
		/// <param name="min">   The minimum value.</param>
		/// <param name="max">   The maximum value.</param>
		/// <param name="result">
		/// When the method completes, contains the clamped value.
		/// </param>
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
		/// <see cref="Vector2.DistanceSquared(ref Vector2, ref Vector2, out float)"/> may
		/// be preferred when only the relative distance is needed and speed is of the
		/// essence.
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
		/// <see cref="Vector2.DistanceSquared(Vector2, Vector2)"/> may be preferred when
		/// only the relative distance is needed and speed is of the essence.
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
		/// When the method completes, contains the squared distance between the two
		/// vectors.
		/// </param>
		/// <remarks>
		/// Distance squared is the value before taking the square root. Distance squared
		/// can often be used in place of distance if relative comparisons are being made.
		/// For example, consider three points A, B, and C. To determine whether B or C is
		/// further from A, compare the distance between A and B to the distance between A
		/// and C. Calculating the two distances involves two square roots, which are
		/// computationally expensive. However, using distance squared provides the same
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
		/// Distance squared is the value before taking the square root. Distance squared
		/// can often be used in place of distance if relative comparisons are being made.
		/// For example, consider three points A, B, and C. To determine whether B or C is
		/// further from A, compare the distance between A and B to the distance between A
		/// and C. Calculating the two distances involves two square roots, which are
		/// computationally expensive. However, using distance squared provides the same
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
		/// <param name="left">  First source vector.</param>
		/// <param name="right"> Second source vector.</param>
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
		/// <param name="left"> First source vector.</param>
		/// <param name="right">Second source vector.</param>
		/// <returns>The dot product of the two vectors.</returns>
		public static float Dot(Vector2 left, Vector2 right)
		{
			return (left.X * right.X) + (left.Y * right.Y);
		}
		/// <summary>
		/// Returns a vector containing the smallest components of the specified vectors.
		/// </summary>
		/// <param name="left">  The first source vector.</param>
		/// <param name="right"> The second source vector.</param>
		/// <param name="result">
		/// When the method completes, contains an new vector composed of the largest
		/// components of the source vectors.
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
		/// <returns>
		/// A vector containing the largest components of the source vectors.
		/// </returns>
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
		/// When the method completes, contains an new vector composed of the smallest
		/// components of the source vectors.
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
		/// <returns>
		/// A vector containing the smallest components of the source vectors.
		/// </returns>
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
		/// <param name="result">
		/// When the method completes, contains the normalized vector.
		/// </param>
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
		/// <param name="left">  The first vector to modulate.</param>
		/// <param name="right"> The second vector to modulate.</param>
		/// <param name="result">
		/// When the method completes, contains the modulated vector.
		/// </param>
		public static void Modulate(ref Vector2 left, ref Vector2 right, out Vector2 result)
		{
			result = new Vector2(left.X * right.X, left.Y * right.Y);
		}
		/// <summary>
		/// Modulates a vector with another by performing component-wise multiplication.
		/// </summary>
		/// <param name="left"> The first vector to modulate.</param>
		/// <param name="right">The second vector to modulate.</param>
		/// <returns>The modulated vector.</returns>
		public static Vector2 Modulate(Vector2 left, Vector2 right)
		{
			return new Vector2(left.X * right.X, left.Y * right.Y);
		}
		#endregion
		#region Reflections
		/// <summary>
		/// Returns the reflection of a vector off a surface that has the specified
		/// normal.
		/// </summary>
		/// <param name="vector">The source vector.</param>
		/// <param name="normal">Normal of the surface.</param>
		/// <param name="result">
		/// When the method completes, contains the reflected vector.
		/// </param>
		/// <remarks>
		/// Reflect only gives the direction of a reflection off a surface, it does not
		/// determine whether the original vector was close enough to the surface to hit
		/// it.
		/// </remarks>
		public static void Reflect(ref Vector2 vector, ref Vector2 normal, out Vector2 result)
		{
			float dot = (vector.X * normal.X) + (vector.Y * normal.Y);

			result.X = vector.X - ((2.0f * dot) * normal.X);
			result.Y = vector.Y - ((2.0f * dot) * normal.Y);
		}
		/// <summary>
		/// Returns the reflection of a vector off a surface that has the specified
		/// normal.
		/// </summary>
		/// <param name="vector">The source vector.</param>
		/// <param name="normal">Normal of the surface.</param>
		/// <returns>The reflected vector.</returns>
		/// <remarks>
		/// Reflect only gives the direction of a reflection off a surface, it does not
		/// determine whether the original vector was close enough to the surface to hit
		/// it.
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
		/// <param name="left"> The first vector to add.</param>
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
		/// <param name="left"> The first vector to subtract.</param>
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
		/// <param name="left"> The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="left"/> has the same value as
		/// <paramref name="right"/> ; otherwise, <c>false</c> .
		/// </returns>
		public static bool operator ==(Vector2 left, Vector2 right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left"> The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="left"/> has a different value than
		/// <paramref name="right"/> ; otherwise, <c>false</c> .
		/// </returns>
		public static bool operator !=(Vector2 left, Vector2 right)
		{
			return !left.Equals(right);
		}
		#endregion
		#region Conversion Operators
		/// <summary>
		/// Performs an explicit conversion from <see cref="Vector2"/> to
		/// <see cref="Vector3"/> .
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator Vector3(Vector2 value)
		{
			return new Vector3(value);
		}
		/// <summary>
		/// Performs an explicit conversion from <see cref="Vector2"/> to
		/// <see cref="Vector4"/> .
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
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1}", this.X, this.Y);
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public string ToString(string format)
		{
			if (format == null)
				return this.ToString();

			return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1}", this.X.ToString(format, CultureInfo.CurrentCulture), this.Y.ToString(format, CultureInfo.CurrentCulture));
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public string ToString(IFormatProvider formatProvider)
		{
			return string.Format(formatProvider, "X:{0} Y:{1}", this.X, this.Y);
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="format">        The format.</param>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (format == null)
				this.ToString(formatProvider);

			return string.Format(formatProvider, "X:{0} Y:{1}", this.X.ToString(format, formatProvider), this.Y.ToString(format, formatProvider));
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
				hash = hash * 29 + this.X.GetHashCode();
				hash = hash * 29 + this.Y.GetHashCode();
				// ReSharper restore NonReadonlyFieldInGetHashCode

				return hash;
			}
		}
		/// <summary>
		/// Determines whether the specified <see cref="Vector2"/> is equal to this
		/// instance.
		/// </summary>
		/// <param name="other">
		/// The <see cref="Vector2"/> to compare with this instance.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Vector2"/> is equal to this instance;
		/// otherwise, <c>false</c> .
		/// </returns>
		public bool Equals(Vector2 other)
		{
			// ReSharper disable CompareOfFloatsByEqualityOperator
			return this.X == other.X && (this.Y == other.Y);
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}
		/// <summary>
		/// Determines whether the specified <see cref="Vector2"/> is equal to this
		/// instance using an epsilon value.
		/// </summary>
		/// <param name="other">  
		/// The <see cref="Vector2"/> to compare with this instance.
		/// </param>
		/// <param name="epsilon">The amount of error allowed.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Vector2"/> is equal to this instance;
		/// otherwise, <c>false</c> .
		/// </returns>
		public bool Equals(Vector2 other, float epsilon)
		{
			return (Math.Abs(other.X - this.X) < epsilon &&
				Math.Abs(other.Y - this.Y) < epsilon);
		}
		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this
		/// instance.
		/// </summary>
		/// <param name="value">
		/// The <see cref="System.Object"/> to compare with this instance.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to this
		/// instance; otherwise, <c>false</c> .
		/// </returns>
		public override bool Equals(object value)
		{
			return value != null && value.GetType() == this.GetType() && this.Equals((Vector2)value);
		}
		/// <summary>
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public int CompareTo(Vector2 other)
		{
			int pos = this.X.CompareTo(other.X);
			return pos != 0 ? pos : this.Y.CompareTo(other.Y);
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
	}
}