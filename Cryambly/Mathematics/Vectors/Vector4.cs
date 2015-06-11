using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using CryCil.Geometry;

namespace CryCil
{
	/// <summary>
	/// Represents a four dimensional mathematical vector.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector4 : IVector<float, Vector4>, IEquatable<Vector4>, IFormattable, IEnumerable<float>
	{
		/// <summary>
		/// Zero vector.
		/// </summary>
		public readonly static Vector4 Zero = new Vector4();
		/// <summary>
		/// Number of components of this vector.
		/// </summary>
		public const int ComponentCount = 3;
		#region Fields
		/// <summary>
		/// The X component of the vector.
		/// </summary>
		public float X;
		/// <summary>
		/// The Y component of the vector.
		/// </summary>
		public float Y;
		/// <summary>
		/// The Z component of the vector.
		/// </summary>
		public float Z;
		/// <summary>
		/// The W component of the vector.
		/// </summary>
		public float W;
		#endregion
		#region Properties
		/// <summary>
		/// Gets a value indicting whether this instance is normalized.
		/// </summary>
		public bool IsNormalized
		{
			get { return Math.Abs((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z) + (this.W * this.W) - 1f) < MathHelpers.ZeroTolerance; }
		}
		/// <summary>
		/// Gets or sets the component at the specified index.
		/// </summary>
		/// <value>The value of the X, Y, Z, or W component, depending on the index.</value>
		/// <param name="index">
		/// The index of the component to access. Use 0 for the X component, 1 for the Y component, 2 for
		/// the Z component, and 3 for the W component.
		/// </param>
		/// <returns>The value of the component at the specified index.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// Thrown when the <paramref name="index"/> is out of the range [0, 3].
		/// </exception>
		public float this[int index]
		{
			get
			{
				switch (index)
				{
					case 0:
						return this.X;
					case 1:
						return this.Y;
					case 2:
						return this.Z;
					case 3:
						return this.W;
					default:
						throw new ArgumentOutOfRangeException("index", "Attempt to access vector" +
																	   " component other then X, Y, Z or W.");
				}
			}
			set
			{
				switch (index)
				{
					case 0:
						this.X = value;
						break;
					case 1:
						this.Y = value;
						break;
					case 2:
						this.Z = value;
						break;
					case 3:
						this.W = value;
						break;
					default:
						throw new ArgumentOutOfRangeException("index", "Attempt to access vector component" +
																	   " other then X, Y, Z or W.");
				}
			}
		}
		/// <summary>
		/// Calculates the length of the vector.
		/// </summary>
		public float Length
		{
			get { return (float)Math.Sqrt((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z) + (this.W * this.W)); }
		}
		/// <summary>
		/// Calculates the squared length of the vector.
		/// </summary>
		public float LengthSquared
		{
			get { return (this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z) + (this.W * this.W); }
		}
		/// <summary>
		/// Determines whether this vector is represented by valid numbers.
		/// </summary>
		public bool IsValid
		{
			get
			{
				return MathHelpers.IsNumberValid(this.X) &&
					MathHelpers.IsNumberValid(this.Y) &&
					MathHelpers.IsNumberValid(this.Z) &&
					MathHelpers.IsNumberValid(this.W);
			}
		}
		#endregion
		#region Interface
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Vector4"/> struct.
		/// </summary>
		/// <param name="value">The value that will be assigned to all components.</param>
		public Vector4(float value)
			: this()
		{
			this.X = value;
			this.Y = value;
			this.Z = value;
			this.W = value;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Vector4"/> struct.
		/// </summary>
		/// <param name="x">Initial value for the X component of the vector.</param>
		/// <param name="y">Initial value for the Y component of the vector.</param>
		/// <param name="z">Initial value for the Z component of the vector.</param>
		/// <param name="w">Initial value for the W component of the vector.</param>
		public Vector4(float x, float y, float z, float w)
			: this()
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Vector4"/> struct.
		/// </summary>
		/// <param name="value">
		/// A vector containing the values with which to initialize the X, Y, and Z components.
		/// </param>
		/// <param name="w">    Initial value for the W component of the vector.</param>
		public Vector4(Vector3 value, float w)
			: this()
		{
			this.X = value.X;
			this.Y = value.Y;
			this.Z = value.Z;
			this.W = w;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Vector4"/> struct.
		/// </summary>
		/// <param name="value">
		/// A vector containing the values with which to initialize the X and Y components.
		/// </param>
		/// <param name="z">    Initial value for the Z component of the vector.</param>
		/// <param name="w">    Initial value for the W component of the vector.</param>
		public Vector4(Vector2 value, float z, float w)
			: this()
		{
			this.X = value.X;
			this.Y = value.Y;
			this.Z = z;
			this.W = w;
		}
		/// <summary>
		/// Creates new <see cref="Vector4"/>.
		/// </summary>
		/// <param name="values">
		/// An array of floating point values which specify new vector using following rules:
		/// <para>If array is null or empty zero vector is created.</para>
		/// <para>If array contains 1 value it is assigned to all three components.</para>
		/// <para>
		/// If array contains 2 values they are assigned to first two components while Z and W are zeroed.
		/// </para>
		/// <para>
		/// If array contains 3 values they are assigned to first three components while W is zeroed.
		/// </para>
		/// <para>
		/// If array contains 4 or more values first four values are assigned to vector components.
		/// </para>
		/// </param>
		public Vector4(IList<float> values)
			: this(values, 0, 0, values.Count)
		{
		}
		/// <summary>
		/// Creates new <see cref="Vector4"/>.
		/// </summary>
		/// <param name="values">       
		/// A list of floating point numbers which is assigned to components of the new vector.
		/// </param>
		/// <param name="startingIndex">Index of first element of list to copy.</param>
		/// <param name="count">        Number of components to assign.</param>
		public Vector4(IList<float> values, int startingIndex, int count)
			: this(values, startingIndex, 0, count)
		{
		}
		/// <summary>
		/// Creates new <see cref="Vector4"/>.
		/// </summary>
		/// <param name="values">        
		/// A list of floating point numbers which is assigned to components of the new vector.
		/// </param>
		/// <param name="firstIndex">    Index of first element of list to copy.</param>
		/// <param name="firstComponent">Index of the first component to assign.</param>
		/// <param name="count">         Number of components to assign.</param>
		public Vector4(IList<float> values, int firstIndex, int firstComponent, int count)
			: this()
		{
			if (values == null) return;
			if (firstIndex < 0)
			{
				throw new ArgumentOutOfRangeException
					("firstIndex", "Index of the first element to copy cannot be negative.");
			}
			if (firstComponent < 0)
			{
				throw new ArgumentOutOfRangeException
					("firstComponent", "Index of the first component to assign cannot be negative.");
			}
			if (count > Vector4.ComponentCount - firstComponent)
			{
				throw new ArgumentOutOfRangeException
					("count", "Number of elements to copy is bigger then number of components to assign.");
			}
			for (int i = firstComponent; i < Vector4.ComponentCount || i < count; i++)
			{
				this[i] = values[i];
			}
		}
		/// <summary>
		/// Creates new <see cref="Vector4"/>.
		/// </summary>
		/// <param name="values">Dicionary which contents are used to initialize new vector.</param>
		public Vector4(IDictionary<string, float> values)
			: this()
		{
			if (values == null)
			{
				return;
			}
			if (values.ContainsKey("X"))
			{
				this.X = values["X"];
			}
			if (values.ContainsKey("Y"))
			{
				this.X = values["Y"];
			}
			if (values.ContainsKey("Z"))
			{
				this.X = values["Z"];
			}
			if (values.ContainsKey("W"))
			{
				this.X = values["W"];
			}
		}
		#endregion
		#region Operators
		#region Arithmetic Operators
		/// <summary>
		/// Performs addition of two vectors.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of addition.</returns>
		public static Vector4 operator +(Vector4 left, Vector4 right)
		{
			return new Vector4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
		}
		/// <summary>
		/// Performs subtraction of two vectors.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of subtraction.</returns>
		public static Vector4 operator -(Vector4 left, Vector4 right)
		{
			return new Vector4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
		}
		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="value">The vector to scale.</param>
		/// <param name="scale">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		public static Vector4 operator *(float scale, Vector4 value)
		{
			return new Vector4(value.X * scale, value.Y * scale, value.Z * scale, value.W * scale);
		}
		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="value">The vector to scale.</param>
		/// <param name="scale">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		public static Vector4 operator *(Vector4 value, float scale)
		{
			return new Vector4(value.X * scale, value.Y * scale, value.Z * scale, value.W * scale);
		}
		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="value">The vector to scale.</param>
		/// <param name="scale">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		public static Vector4 operator /(Vector4 value, float scale)
		{
			return new Vector4(value.X / scale, value.Y / scale, value.Z / scale, value.W / scale);
		}
		#endregion
		#region Comparison Operators
		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left"> The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>;
		/// otherwise, <c>false</c>.
		/// </returns>
		public static bool operator ==(Vector4 left, Vector4 right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left"> The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>;
		/// otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=(Vector4 left, Vector4 right)
		{
			return !left.Equals(right);
		}
		#endregion
		#region Unary Operators
		/// <summary>
		/// Reverses the direction of a given vector.
		/// </summary>
		/// <param name="value">The vector to negate.</param>
		/// <returns>A vector facing in the opposite direction.</returns>
		public static Vector4 operator -(Vector4 value)
		{
			return new Vector4(-value.X, -value.Y, -value.Z, -value.W);
		}
		#endregion
		#region Conversion Operators
		/// <summary>
		/// Performs an explicit conversion from <see cref="Vector4"/> to <see cref="Vector2"/>.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator Vector2(Vector4 value)
		{
			return new Vector2(value.X, value.Y);
		}
		/// <summary>
		/// Performs an explicit conversion from <see cref="Vector4"/> to <see cref="Vector3"/>.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator Vector3(Vector4 value)
		{
			return new Vector3(value.X, value.Y, value.Z);
		}
		/// <summary>
		/// Performs an explicit conversion from <see cref="Vector4"/> to <see cref="Quaternion"/>.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator Quaternion(Vector4 value)
		{
			return new Quaternion(value.X, value.Y, value.Z, value.W);
		}
		/// <summary>
		/// Performs an explicit conversion from <see cref="Vector4"/> to <see cref="Quaternion"/>.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator Vector4(Quaternion value)
		{
			return new Vector4(value.X, value.Y, value.Z, value.W);
		}
		#endregion
		#region Product Operators
		/// <summary>
		/// Calculates dot product of two vectors.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Dot product.</returns>
		public static float operator *(Vector4 left, Vector4 right)
		{
			return left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
		}
		#endregion
		#endregion
		#region Modification
		/// <summary>
		/// Creates new instance of <see cref="Vector4"/> where Y, Z and W components are of this instance
		/// and X component is specified by given value.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="value">New value for X component.</param>
		/// <returns>Modified vector.</returns>
		public Vector4 ModifyX(float value)
		{
			return new Vector4(value, this.Y, this.Z, this.W);
		}
		/// <summary>
		/// Creates new instance of <see cref="Vector4"/> where X, Z and W components are of this instance
		/// and Y component is specified by given value.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="value">New value for Y component.</param>
		/// <returns>Modified vector.</returns>
		public Vector4 ModifyY(float value)
		{
			return new Vector4(this.X, value, this.Z, this.W);
		}
		/// <summary>
		/// Creates new instance of <see cref="Vector4"/> where X, Y and W components are of this instance
		/// and Z component is specified by given value.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="value">New value for Z component.</param>
		/// <returns>Modified vector.</returns>
		public Vector4 ModifyZ(float value)
		{
			return new Vector4(this.X, this.Y, value, this.W);
		}
		/// <summary>
		/// Creates new instance of <see cref="Vector4"/> where X, Y and Z components are of this instance
		/// and W component is specified by given value.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="value">New value for W component.</param>
		/// <returns>Modified vector.</returns>
		public Vector4 ModifyW(float value)
		{
			return new Vector4(this.X, this.Y, this.Z, value);
		}
		/// <summary>
		/// Creates new <see cref="Vector4"/> which represents this vector with modified values.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="offset">   Index of first component to be modified.</param>
		/// <param name="newValues">New values for components to modify.</param>
		/// <returns>Modified vector.</returns>
		public Vector4 ModifyVector(int offset, params float[] newValues)
		{
			if (offset < 0 || offset > 2)
			{
				throw new ArgumentOutOfRangeException("offset", "offset must be belong to interval [0; 3]");
			}

			Vector4 result = new Vector4(this.X, this.Y, this.Z, this.W);
			for (int i = offset, j = 0; j < newValues.Length && i < 4; i++, j++)
			{
				result[i] = newValues[j];
			}
			return result;
		}
		#endregion
		#region Generic Operations
		/// <summary>
		/// Converts the vector into a unit vector.
		/// </summary>
		public void Normalize()
		{
			float length = this.Length;
			if (length > MathHelpers.ZeroTolerance)
			{
				float inverse = 1.0f / length;
				this.X *= inverse;
				this.Y *= inverse;
				this.Z *= inverse;
				this.W *= inverse;
			}
		}
		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <param name="min">  The minimum value.</param>
		/// <param name="max">  The maximum value.</param>
		/// <returns>The clamped value.</returns>
		public static Vector4 Clamp(Vector4 value, Vector4 min, Vector4 max)
		{
			float x = value.X;
			x = (x > max.X) ? max.X : x;
			x = (x < min.X) ? min.X : x;

			float y = value.Y;
			y = (y > max.Y) ? max.Y : y;
			y = (y < min.Y) ? min.Y : y;

			float z = value.Z;
			z = (z > max.Z) ? max.Z : z;
			z = (z < min.Z) ? min.Z : z;

			float w = value.W;
			w = (w > max.W) ? max.W : w;
			w = (w < min.W) ? min.W : w;

			return new Vector4(x, y, z, w);
		}
		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		/// <param name="value1">The first vector.</param>
		/// <param name="value2">The second vector.</param>
		/// <returns>The distance between the two vectors.</returns>
		public static float Distance(Vector4 value1, Vector4 value2)
		{
			float x = value1.X - value2.X;
			float y = value1.Y - value2.Y;
			float z = value1.Z - value2.Z;
			float w = value1.W - value2.W;

			return (float)Math.Sqrt((x * x) + (y * y) + (z * z) + (w * w));
		}
		/// <summary>
		/// Calculates the squared distance between two vectors.
		/// </summary>
		/// <param name="value1">The first vector.</param>
		/// <param name="value2">The second vector.</param>
		/// <returns>The squared distance between the two vectors.</returns>
		public static float DistanceSquared(Vector4 value1, Vector4 value2)
		{
			float x = value1.X - value2.X;
			float y = value1.Y - value2.Y;
			float z = value1.Z - value2.Z;
			float w = value1.W - value2.W;

			return (x * x) + (y * y) + (z * z) + (w * w);
		}
		/// <summary>
		/// Calculates the dot product of two vectors.
		/// </summary>
		/// <param name="left"> First source vector.</param>
		/// <param name="right">Second source vector.</param>
		/// <returns>The dot product of the two vectors.</returns>
		public static float Dot(Vector4 left, Vector4 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
		}
		/// <summary>
		/// Converts the vector into a unit vector.
		/// </summary>
		/// <param name="value">The vector to normalize.</param>
		/// <returns>The normalized vector.</returns>
		public static Vector4 Normalize(Vector4 value)
		{
			value.Normalize();
			return value;
		}
		/// <summary>
		/// Returns a vector containing the largest components of the specified vectors.
		/// </summary>
		/// <param name="left"> The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>A vector containing the largest components of the source vectors.</returns>
		public static Vector4 Max(Vector4 left, Vector4 right)
		{
			Vector4 result = new Vector4
			{
				X = (left.X > right.X) ? left.X : right.X,
				Y = (left.Y > right.Y) ? left.Y : right.Y,
				Z = (left.Z > right.Z) ? left.Z : right.Z,
				W = (left.W > right.W) ? left.W : right.W
			};
			return result;
		}
		/// <summary>
		/// Returns a vector containing the smallest components of the specified vectors.
		/// </summary>
		/// <param name="left"> The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>A vector containing the smallest components of the source vectors.</returns>
		public static Vector4 Min(Vector4 left, Vector4 right)
		{
			Vector4 result = new Vector4
			{
				X = (left.X < right.X) ? left.X : right.X,
				Y = (left.Y < right.Y) ? left.Y : right.Y,
				Z = (left.Z < right.Z) ? left.Z : right.Z,
				W = (left.W < right.W) ? left.W : right.W
			};
			return result;
		}
		#endregion
		#region Text Conversions
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2} W:{3}", this.X, this.Y, this.Z, this.W);
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public string ToString(string format)
		{
			if (format == null)
				return this.ToString();

			return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2} W:{3}", this.X.ToString(format, CultureInfo.CurrentCulture),
				this.Y.ToString(format, CultureInfo.CurrentCulture), this.Z.ToString(format, CultureInfo.CurrentCulture), this.W.ToString(format, CultureInfo.CurrentCulture));
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public string ToString(IFormatProvider formatProvider)
		{
			return string.Format(formatProvider, "X:{0} Y:{1} Z:{2} W:{3}", this.X, this.Y, this.Z, this.W);
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="format">        The format.</param>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (format == null)
				this.ToString(formatProvider);

			return string.Format(formatProvider, "X:{0} Y:{1} Z:{2} W:{3}", this.X.ToString(format, formatProvider),
				this.Y.ToString(format, formatProvider), this.Z.ToString(format, formatProvider), this.W.ToString(format, formatProvider));
		}
		#endregion
		#region Equality Checks
		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like
		/// a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = 17;

				hash = hash * 29 + this.X.GetHashCode();
				hash = hash * 29 + this.Y.GetHashCode();
				hash = hash * 29 + this.Z.GetHashCode();
				hash = hash * 29 + this.W.GetHashCode();

				return hash;
			}
		}
		/// <summary>
		/// Determines whether the specified <see cref="Vector4"/> is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="Vector4"/> to compare with this instance.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Vector4"/> is equal to this instance; otherwise,
		/// <c>false</c>.
		/// </returns>
		public bool Equals(Vector4 other)
		{
			// ReSharper disable CompareOfFloatsByEqualityOperator
			return this.X == other.X && (this.Y == other.Y) && (this.Z == other.Z) && (this.W == other.W);
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}
		/// <summary>
		/// Determines whether the specified <see cref="Vector4"/> is equal to this instance.
		/// </summary>
		/// <param name="other">  The <see cref="Vector4"/> to compare with this instance.</param>
		/// <param name="epsilon">The amount of error allowed.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Vector4"/> is equal to this instance; otherwise,
		/// <c>false</c>.
		/// </returns>
		public bool Equals(Vector4 other, float epsilon)
		{
			return
			(
				Math.Abs(other.X - this.X) < epsilon &&
				Math.Abs(other.Y - this.Y) < epsilon &&
				Math.Abs(other.Z - this.Z) < epsilon &&
				Math.Abs(other.W - this.W) < epsilon
			);
		}
		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise,
		/// <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			if (obj.GetType() != this.GetType())
				return false;

			return this.Equals((Vector4)obj);
		}
		#endregion
		#region To Collections
		/// <summary>
		/// Creates an array that contains components of this vector.
		/// </summary>
		/// <returns>An array of four elements which contain corresponding vector components.</returns>
		public float[] ToArray()
		{
			return new[] { this.X, this.Y, this.Z, this.W };
		}
		/// <summary>
		/// Creates a list that contains components of this vector.
		/// </summary>
		/// <returns>A list of four elements which contain corresponding vector components.</returns>
		public List<float> ToList()
		{
			List<float> result = new List<float>(4) { this.X, this.Y, this.Z, this.W };
			return result;
		}
		/// <summary>
		/// Creates a dictionary that contains components of this vector.
		/// </summary>
		/// <returns>
		/// A dictionary which capacity is set to 4 where components of the vector can be accessed with
		/// keys of same names.
		/// </returns>
		public Dictionary<string, float> ToDictionary()
		{
			Dictionary<string, float> result = new Dictionary<string, float>(4)
			{
				{"X", this.X},
				{"Y", this.Y},
				{"Z", this.Z},
				{"W", this.W}
			};
			return result;
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
			yield return this.Z;
			yield return this.W;
		}
		/// <summary>
		/// Enumerates this vector.
		/// </summary>
		/// <returns>Yields components of this vector.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			yield return this.X;
			yield return this.Y;
			yield return this.Z;
			yield return this.W;
		}
		#endregion
		#region IVector
		/// <summary>
		/// Creates deep copy of this vector.
		/// </summary>
		Vector4 IVector<float, Vector4>.DeepCopy
		{
			get { return this; }
		}
		/// <summary>
		/// Calculates the dot product of this vector and another one.
		/// </summary>
		/// <param name="other">Another vector.</param>
		/// <returns>Dot product of the 2 vectors.</returns>
		float IVector<float, Vector4>.Dot(Vector4 other)
		{
			return this * other;
		}
		/// <summary>
		/// Adds components from another vector to respective components of this one.
		/// </summary>
		/// <param name="other">Another vector.</param>
		void IVector<float, Vector4>.Add(Vector4 other)
		{
			this.X += other.X;
			this.Y += other.Y;
			this.Z += other.Z;
			this.W += other.W;
		}
		/// <summary>
		/// Multiplies components of this vector by the given factor.
		/// </summary>
		/// <param name="factor">Scaling factor.</param>
		void IVector<float, Vector4>.Scale(float factor)
		{
			this.X *= factor;
			this.Y *= factor;
			this.Z *= factor;
			this.W *= factor;
		}
		/// <summary>
		/// Creates a new vector that is a sum of this one and another one.
		/// </summary>
		/// <param name="other">Another vector.</param>
		/// <returns>Sum of two vectors.</returns>
		Vector4 IVector<float, Vector4>.Added(Vector4 other)
		{
			return this + other;
		}
		/// <summary>
		/// Creates a new vector that is a scaled version of this one.
		/// </summary>
		/// <param name="factor">Scaling factor.</param>
		/// <returns>Scaled vector.</returns>
		Vector4 IVector<float, Vector4>.Scaled(float factor)
		{
			return this * factor;
		}
		#endregion
		#endregion
	}
}