using System;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace CryEngine
{
	/// <summary>
	/// Represents a four dimensional mathematical vector.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct Vector4 : IEquatable<Vector4>, IFormattable, IEnumerable<float>
	{
		/// <summary>
		/// Zero vector.
		/// </summary>
		public readonly static Vector4 Zero = new Vector4();
		#region Fields
		/// <summary>
		/// The X component of the vector.
		/// </summary>
		[FieldOffset(0)]
		public float X;
		/// <summary>
		/// The Y component of the vector.
		/// </summary>
		[FieldOffset(4)]
		public float Y;
		/// <summary>
		/// The Z component of the vector.
		/// </summary>
		[FieldOffset(8)]
		public float Z;
		/// <summary>
		/// The W component of the vector.
		/// </summary>
		[FieldOffset(12)]
		public float W;
		/// <summary>
		/// Array of all components of this vector.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public float[] Components;
		#endregion
		#region Properties
		/// <summary>
		/// Gets a value indicting whether this instance is normalized.
		/// </summary>
		public bool IsNormalized
		{
			get { return Math.Abs((X * X) + (Y * Y) + (Z * Z) + (W * W) - 1f) < MathHelpers.ZeroTolerance; }
		}
		/// <summary>
		/// Gets or sets the component at the specified index.
		/// </summary>
		/// <value>The value of the X, Y, Z, or W component, depending on the index.</value>
		/// <param name="index">
		/// The index of the component to access. Use 0 for the X component, 1 for the Y component,
		/// 2 for the Z component, and 3 for the W component.
		/// </param>
		/// <returns>The value of the component at the specified index.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// Thrown when the <paramref name="index" /> is out of the range [0, 3].
		/// </exception>
		public float this[int index]
		{
			get { return this.Components[index]; }
			set { this.Components[index] = value; }
		}
		/// <summary>
		/// Calculates the length of the vector.
		/// </summary>
		public float Length
		{
			get { return (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W)); }
		}
		/// <summary>
		/// Calculates the squared length of the vector.
		/// </summary>
		public float LengthSquared
		{
			get { return (X * X) + (Y * Y) + (Z * Z) + (W * W); }
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
		/// Initializes a new instance of the <see cref="Vector4" /> struct.
		/// </summary>
		/// <param name="value">The value that will be assigned to all components.</param>
		public Vector4(float value)
			: this()
		{
			X = value;
			Y = value;
			Z = value;
			W = value;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Vector4" /> struct.
		/// </summary>
		/// <param name="x">Initial value for the X component of the vector.</param>
		/// <param name="y">Initial value for the Y component of the vector.</param>
		/// <param name="z">Initial value for the Z component of the vector.</param>
		/// <param name="w">Initial value for the W component of the vector.</param>
		public Vector4(float x, float y, float z, float w)
			: this()
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Vector4" /> struct.
		/// </summary>
		/// <param name="value">
		/// A vector containing the values with which to initialize the X, Y, and Z components.
		/// </param>
		/// <param name="w">Initial value for the W component of the vector.</param>
		public Vector4(Vector3 value, float w)
			: this()
		{
			X = value.X;
			Y = value.Y;
			Z = value.Z;
			W = w;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Vector4" /> struct.
		/// </summary>
		/// <param name="value">
		/// A vector containing the values with which to initialize the X and Y components.
		/// </param>
		/// <param name="z">Initial value for the Z component of the vector.</param>
		/// <param name="w">Initial value for the W component of the vector.</param>
		public Vector4(Vector2 value, float z, float w)
			: this()
		{
			X = value.X;
			Y = value.Y;
			Z = z;
			W = w;
		}
		/// <summary>
		/// Creates new <see cref="Vector4" />.
		/// </summary>
		/// <param name="values">
		/// An array of floating point values which specify new vector using following rules:
		/// <para>If array is null or empty zero vector is created.</para><para>If array contains 1
		/// value it is assigned to all three components.</para><para>If array contains 2 values
		/// they are assigned to first two components while Z and W are zeroed.</para><para>If array
		/// contains 3 values they are assigned to first three components while W is
		/// zeroed.</para><para>If array contains 4 or more values first four values are assigned to
		/// vector components.</para>
		/// </param>
		public Vector4(IList<float> values)
			: this()
		{
			if (values == null) return;
			for (int i = 0; i < this.Components.Length || i < values.Count; i++)
			{
				this.Components[i] = values[i];
			}
		}
		/// <summary>
		/// Creates new <see cref="Vector4" />.
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
		/// <param name="left">Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of addition.</returns>
		public static Vector4 operator +(Vector4 left, Vector4 right)
		{
			return new Vector4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
		}
		/// <summary>
		/// Performs subtraction of two vectors.
		/// </summary>
		/// <param name="left">Left operand.</param>
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
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="left" /> has the same value as <paramref name="right" />;
		/// otherwise, <c>false</c>.
		/// </returns>
		public static bool operator ==(Vector4 left, Vector4 right)
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
		/// Performs an explicit conversion from <see cref="Vector4" /> to <see cref="Vector2" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator Vector2(Vector4 value)
		{
			return new Vector2(value.X, value.Y);
		}
		/// <summary>
		/// Performs an explicit conversion from <see cref="Vector4" /> to <see
		/// cref="CryEngine.Vector3" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator Vector3(Vector4 value)
		{
			return new Vector3(value.X, value.Y, value.Z);
		}
		#endregion
		#endregion
		#region Modification
		/// <summary>
		/// Creates new instance of <see cref="Vector4" /> where Y, Z and W components are of this
		/// instance and X component is specified by given value.
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
		/// Creates new instance of <see cref="Vector4" /> where X, Z and W components are of this
		/// instance and Y component is specified by given value.
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
		/// Creates new instance of <see cref="Vector4" /> where X, Y and W components are of this
		/// instance and Z component is specified by given value.
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
		/// Creates new instance of <see cref="Vector4" /> where X, Y and Z components are of this
		/// instance and W component is specified by given value.
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
		/// Creates new <see cref="Vector4" /> which represents this vector with modified values.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="offset">Index of first component to be modified.</param>
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
		#region Interpolations
		/// <summary>
		/// Performs a linear interpolation between two vectors.
		/// </summary>
		/// <param name="start">Start vector.</param>
		/// <param name="end">End vector.</param>
		/// <param name="amount">
		/// Value between 0 and 1 indicating the weight of <paramref name="end" />.
		/// </param>
		public void SetLinearInterpolation(Vector4 start, Vector4 end, float amount)
		{
			this.X = start.X + ((end.X - start.X) * amount);
			this.Y = start.Y + ((end.Y - start.Y) * amount);
			this.Z = start.Z + ((end.Z - start.Z) * amount);
			this.W = start.W + ((end.W - start.W) * amount);
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
		public static Vector4 CreateLinearInterpolation(Vector4 start, Vector4 end, float amount)
		{
			Vector4 result = new Vector4();
			result.SetLinearInterpolation(start, end, amount);
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
		public void SetCubicInterpolation(Vector4 start, Vector4 end, float amount)
		{
			amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
			amount = (amount * amount) * (3.0f - (2.0f * amount));

			this.X = start.X + ((end.X - start.X) * amount);
			this.Y = start.Y + ((end.Y - start.Y) * amount);
			this.Z = start.Z + ((end.Z - start.Z) * amount);
			this.W = start.W + ((end.W - start.W) * amount);
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
		public static Vector4 CreateCubicInterpolation(Vector4 start, Vector4 end, float amount)
		{
			Vector4 result = new Vector4();
			result.SetCubicInterpolation(start, end, amount);
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
		public void SetHermiteInterpolation
			(Vector4 value1, Vector4 tangent1, Vector4 value2, Vector4 tangent2, float amount)
		{
			float squared = amount * amount;
			float cubed = amount * squared;
			float part1 = ((2.0f * cubed) - (3.0f * squared)) + 1.0f;
			float part2 = (-2.0f * cubed) + (3.0f * squared);
			float part3 = (cubed - (2.0f * squared)) + amount;
			float part4 = cubed - squared;

			this = new Vector4((((value1.X * part1) + (value2.X * part2)) + (tangent1.X * part3)) + (tangent2.X * part4),
				(((value1.Y * part1) + (value2.Y * part2)) + (tangent1.Y * part3)) + (tangent2.Y * part4),
				(((value1.Z * part1) + (value2.Z * part2)) + (tangent1.Z * part3)) + (tangent2.Z * part4),
				(((value1.W * part1) + (value2.W * part2)) + (tangent1.W * part3)) + (tangent2.W * part4));
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
		public static Vector4 CreateHermiteInterpolation
			(Vector4 value1, Vector4 tangent1, Vector4 value2, Vector4 tangent2, float amount)
		{
			Vector4 result = new Vector4();
			result.SetHermiteInterpolation(value1, tangent1, value2, tangent2, amount);
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
		public void SetCatmullRomInterpolation
			(Vector4 value1, Vector4 value2, Vector4 value3, Vector4 value4, float amount)
		{
			float squared = amount * amount;
			float cubed = amount * squared;

			this.X = 0.5f * ((((2.0f * value2.X) + ((-value1.X + value3.X) * amount)) + (((((2.0f * value1.X) - (5.0f * value2.X)) + (4.0f * value3.X)) - value4.X) * squared)) + ((((-value1.X + (3.0f * value2.X)) - (3.0f * value3.X)) + value4.X) * cubed));
			this.Y = 0.5f * ((((2.0f * value2.Y) + ((-value1.Y + value3.Y) * amount)) + (((((2.0f * value1.Y) - (5.0f * value2.Y)) + (4.0f * value3.Y)) - value4.Y) * squared)) + ((((-value1.Y + (3.0f * value2.Y)) - (3.0f * value3.Y)) + value4.Y) * cubed));
			this.Z = 0.5f * ((((2.0f * value2.Z) + ((-value1.Z + value3.Z) * amount)) + (((((2.0f * value1.Z) - (5.0f * value2.Z)) + (4.0f * value3.Z)) - value4.Z) * squared)) + ((((-value1.Z + (3.0f * value2.Z)) - (3.0f * value3.Z)) + value4.Z) * cubed));
			this.W = 0.5f * ((((2.0f * value2.W) + ((-value1.W + value3.W) * amount)) + (((((2.0f * value1.W) - (5.0f * value2.W)) + (4.0f * value3.W)) - value4.W) * squared)) + ((((-value1.W + (3.0f * value2.W)) - (3.0f * value3.W)) + value4.W) * cubed));
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
		public static Vector4 CreateCatmullRomInterpolation
			(Vector4 value1, Vector4 value2, Vector4 value3, Vector4 value4, float amount)
		{
			Vector4 result = new Vector4();
			result.SetCatmullRomInterpolation(value1, value2, value3, value4, amount);
			return result;
		}
		#endregion
		#region Generic Operations
		/// <summary>
		/// Converts the vector into a unit vector.
		/// </summary>
		public void Normalize()
		{
			float length = Length;
			if (length > MathHelpers.ZeroTolerance)
			{
				float inverse = 1.0f / length;
				X *= inverse;
				Y *= inverse;
				Z *= inverse;
				W *= inverse;
			}
		}
		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
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
		/// <param name="left">First source vector.</param>
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
		/// <param name="left">The first source vector.</param>
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
		/// <param name="left">The first source vector.</param>
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
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2} W:{3}", X, Y, Z, W);
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

			return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2} W:{3}", X.ToString(format, CultureInfo.CurrentCulture),
				Y.ToString(format, CultureInfo.CurrentCulture), Z.ToString(format, CultureInfo.CurrentCulture), W.ToString(format, CultureInfo.CurrentCulture));
		}
		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public string ToString(IFormatProvider formatProvider)
		{
			return string.Format(formatProvider, "X:{0} Y:{1} Z:{2} W:{3}", X, Y, Z, W);
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

			return string.Format(formatProvider, "X:{0} Y:{1} Z:{2} W:{3}", X.ToString(format, formatProvider),
				Y.ToString(format, formatProvider), Z.ToString(format, formatProvider), W.ToString(format, formatProvider));
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
				hash = hash * 29 + Z.GetHashCode();
				hash = hash * 29 + W.GetHashCode();
				// ReSharper restore NonReadonlyFieldInGetHashCode

				return hash;
			}
		}
		/// <summary>
		/// Determines whether the specified <see cref="Vector4" /> is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="Vector4" /> to compare with this instance.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Vector4" /> is equal to this instance;
		/// otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(Vector4 other)
		{
			// ReSharper disable CompareOfFloatsByEqualityOperator
			return this.X == other.X && (Y == other.Y) && (Z == other.Z) && (W == other.W);
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}
		/// <summary>
		/// Determines whether the specified <see cref="Vector4" /> is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="Vector4" /> to compare with this instance.</param>
		/// <param name="epsilon">The amount of error allowed.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Vector4" /> is equal to this instance;
		/// otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(Vector4 other, float epsilon)
		{
			return
			(
				Math.Abs(other.X - X) < epsilon &&
				Math.Abs(other.Y - Y) < epsilon &&
				Math.Abs(other.Z - Z) < epsilon &&
				Math.Abs(other.W - W) < epsilon
			);
		}
		/// <summary>
		/// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance;
		/// otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			if (obj.GetType() != GetType())
				return false;

			return Equals((Vector4)obj);
		}
		#endregion
		#region To Collections
		/// <summary>
		/// Creates an array that contains components of this vector.
		/// </summary>
		/// <returns>
		/// An array of four elements which contain corresponding vector components.
		/// </returns>
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
		/// A dictionary which capacity is set to 4 where components of the vector can be accessed
		/// with keys of same names.
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
		#endregion
	}
}