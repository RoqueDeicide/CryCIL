using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Geometry;
using CryCil.Graphics;

namespace CryCil
{
	/// <summary>
	/// Represents a three dimensional mathematical vector.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3 : IEquatable<Vector3>, IEnumerable<float>, IComparable<Vector3>
	{
		#region Static Fields
		/// <summary>
		/// A zero vector - a point.
		/// </summary>
		public static readonly Vector3 Zero = new Vector3(0, 0, 0);
		/// <summary>
		/// A vector that points straight up.
		/// </summary>
		public static readonly Vector3 Up = new Vector3(0, 0, 1);
		/// <summary>
		/// A vector that points straight down.
		/// </summary>
		public static readonly Vector3 Down = new Vector3(0, 0, -1);
		/// <summary>
		/// A vector that points forward.
		/// </summary>
		public static readonly Vector3 Forward = new Vector3(0, 1, 0);
		/// <summary>
		/// A vector that points backward.
		/// </summary>
		public static readonly Vector3 Backward = new Vector3(0, -1, 0);
		/// <summary>
		/// A vector that points to the right.
		/// </summary>
		public static readonly Vector3 Right = new Vector3(1, 0, 0);
		/// <summary>
		/// A vector that points to the left.
		/// </summary>
		public static readonly Vector3 Left = new Vector3(-1, 0, 0);
		/// <summary>
		/// Number of bytes each instance of this structure consists of.
		/// </summary>
		public static readonly int ByteCount = Marshal.SizeOf(typeof(Vector3));
		/// <summary>
		/// Number of components of this vector.
		/// </summary>
		public const int ComponentCount = 3;
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
		/// <summary>
		/// The Z component of the vector.
		/// </summary>
		public float Z;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets length of this vector.
		/// </summary>
		public float Length
		{
			get
			{
				return (float)Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
			}
			set
			{
				float lengthSquared = this.LengthSquared;
				if (lengthSquared < 0.00001f * 0.00001f)
					return;

				lengthSquared = value * MathHelpers.ReciprocalSquareRoot(lengthSquared);
				this.X *= lengthSquared;
				this.Y *= lengthSquared;
				this.Z *= lengthSquared;
			}
		}
		/// <summary>
		/// Gets squared length of this vector.
		/// </summary>
		public float LengthSquared
		{
			get
			{
				return this.X * this.X + this.Y * this.Y + this.Z * this.Z;
			}
		}
		/// <summary>
		/// Gets length of this vector projected onto XY plane.
		/// </summary>
		public float Length2D
		{
			get
			{
				return (float)Math.Sqrt(this.X * this.X + this.Y * this.Y);
			}
		}
		/// <summary>
		/// Gets squared length of this vector projected onto XY plane.
		/// </summary>
		public float Length2DSquared
		{
			get
			{
				return this.X * this.X + this.Y * this.Y;
			}
		}
		/// <summary>
		/// Gets normalized vector.
		/// </summary>
		public Vector3 Normalized
		{
			get
			{
				float fInvLen = MathHelpers.ReciprocalSquareRoot(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
				return this * fInvLen;
			}
		}
		/// <summary>
		/// Gets volume of the perpendicular cuboid defined by this vector.
		/// </summary>
		public float Volume
		{
			get
			{
				return this.X * this.Y * this.Z;
			}
		}
		/// <summary>
		/// Gets vector which components are absolute values of components of this vector.
		/// </summary>
		public Vector3 Absolute
		{
			get
			{
				return new Vector3(Math.Abs(this.X), Math.Abs(this.Y), Math.Abs(this.Z));
			}
		}
		/// <summary>
		/// Gets flipped vector.
		/// </summary>
		public Vector3 Flipped
		{
			get
			{
				return new Vector3(-this.X, -this.Y, -this.Z);
			}
		}
		/// <summary>
		/// Gets simplest vector that is perpendicular to this one.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Cannot get vector that is orthogonal to zero.
		/// </exception>
		public Vector3 OrthogonalSafe
		{
			get
			{
				// ReSharper disable CompareOfFloatsByEqualityOperator
				if (this.X == 0 && this.Y == 0 && this.Z == 0)
				// ReSharper restore CompareOfFloatsByEqualityOperator
				{
					throw new InvalidOperationException("Cannot get vector that is orthogonal to zero.");
				}
				return new Vector3(-this.Y, this.X, 0);
			}
		}
		/// <summary>
		/// Gets simplest vector that is perpendicular to this one.
		/// </summary>
		public Vector3 Orthogonal
		{
			get
			{
				// ReSharper disable CompareOfFloatsByEqualityOperator
				if (this.X == 0 && this.Y == 0 && this.Z == 0)
				// ReSharper restore CompareOfFloatsByEqualityOperator
				{
					return Vector3.Up;
				}
				return new Vector3(-this.Y, this.X, 0);
			}
		}
		/// <summary>
		/// Gets simplest vector that is perpendicular to this one if this one is not zero.
		/// </summary>
		public Vector3? SelectiveOrthogonal
		{
			get
			{
				// ReSharper disable CompareOfFloatsByEqualityOperator
				if (this.X == 0 && this.Y == 0 && this.Z == 0)
				// ReSharper restore CompareOfFloatsByEqualityOperator
				{
					return null;
				}
				return new Vector3(-this.Y, this.X, 0);
			}
		}
		/// <summary>
		/// Gets or sets the component at the specified index.
		/// </summary>
		/// <value>The value of the X, Y or Z component, depending on the index.</value>
		/// <param name="index">
		/// The index of the component to access. Use 0 for the X component, 1 for the Y component, 2 for
		/// the Z component.
		/// </param>
		/// <returns>The value of the component at the specified index.</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Attempt to access vector component other then X, Y or Z.
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
					default:
						throw new ArgumentOutOfRangeException("index", "Attempt to access vector" +
																	   " component other then X, Y or Z.");
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
					default:
						throw new ArgumentOutOfRangeException("index", "Attempt to access vector component" +
																	   " other then X, Y or Z.");
				}
			}
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
					MathHelpers.IsNumberValid(this.Z);
			}
		}
		/// <summary>
		/// Gets an array of bytes that forms this object.
		/// </summary>
		public byte[] Bytes
		{
			get
			{
				byte[] bytes = new byte[12];
				float[] components = { this.X, this.Y, this.Z };

				Buffer.BlockCopy(components, 0, bytes, 0, bytes.Length);
				return bytes;
			}
		}
		#endregion
		#region Interface
		#region Constructors
		/// <summary>
		/// Creates new <see cref="Vector3"/> with specified components.
		/// </summary>
		/// <param name="vx">X-component of new vector.</param>
		/// <param name="vy">Y-component of new vector.</param>
		/// <param name="vz">Z-component of new vector.</param>
		public Vector3(float vx, float vy, float vz)
			: this()
		{
			this.X = vx;
			this.Y = vy;
			this.Z = vz;
		}
		/// <summary>
		/// Creates new <see cref="Vector3"/>.
		/// </summary>
		/// <param name="f"><see cref="Single"/> value to assign to all components of new vector.</param>
		public Vector3(float f)
			: this()
		{
			this.X = this.Y = this.Z = f;
		}
		/// <summary>
		/// Creates new <see cref="Vector3"/> from specified <see cref="Vector2"/>.
		/// </summary>
		/// <param name="v2"><see cref="Vector2"/> that defines X and Y components of new vector.</param>
		public Vector3(Vector2 v2)
			: this()
		{
			this.X = v2.X;
			this.Y = v2.Y;
			this.Z = 0;
		}
		/// <summary>
		/// Creates new <see cref="Vector3"/> from specified <see cref="Quaternion"/> instance.
		/// </summary>
		/// <param name="q">Quaternion that defines new vector.</param>
		public Vector3(Quaternion q)
			: this()
		{
			this.Y = (float)Math.Asin(Math.Max(-1.0f, Math.Min(1.0f, -(q.X * q.Z - q.W * q.Y) * 2)));
			if (Math.Abs(Math.Abs(this.Y) - (Math.PI * 0.5f)) < 0.01f)
			{
				this.X = 0;
				this.Z = (float)Math.Atan2(-2 * (q.X * q.Y - q.W * q.Z), 1 - (q.X * q.X + q.Z * q.Z) * 2);
			}
			else
			{
				this.X = (float)Math.Atan2((q.Y * q.Z + q.W * q.X) * 2, 1 - (q.X * q.X + q.Y * q.Y) * 2);
				this.Z = (float)Math.Atan2((q.X * q.Y + q.W * q.Z) * 2, 1 - (q.Z * q.Z + q.Y * q.Y) * 2);
			}
		}
		/// <summary>
		/// Creates new <see cref="Vector3"/>.
		/// </summary>
		/// <param name="values">
		/// A list of floating point values which specify new vector using following rules:
		/// <para>If list is null or empty zero vector is created.</para>
		/// <para>If list contains 1 value it is assigned to all three components.</para>
		/// <para>
		/// If list contains 2 values they are assigned to first two components while Z is zeroed.
		/// </para>
		/// <para>
		/// If list contains 3 or more values first three values are assigned to vector components.
		/// </para>
		/// </param>
		public Vector3(IList<float> values)
			: this()
		{
			if (values == null) return;
			for (int i = 0; i < Vector3.ComponentCount || i < values.Count; i++)
			{
				this[i] = values[i];
			}
		}
		/// <summary>
		/// Creates new <see cref="Vector3"/>.
		/// </summary>
		/// <param name="values">
		/// A <see cref="Dictionary{TKey,TValue}"/> which is used to initialize new vector.
		/// </param>
		public Vector3(IDictionary<string, float> values)
			: this()
		{
			if (values == null || values.Count == 0) return;
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
		}
		#endregion
		#region To Collections
		/// <summary>
		/// Creates an array that contains components of this vector.
		/// </summary>
		/// <returns>An array of 3 elements which contain corresponding vector components.</returns>
		public float[] ToArray()
		{
			return new[] { this.X, this.Y, this.Z };
		}
		/// <summary>
		/// Creates a list that contains components of this vector.
		/// </summary>
		/// <returns>A list of four elements which contain corresponding vector components.</returns>
		public List<float> ToList()
		{
			List<float> result = new List<float>(3)
			{
				this.X,
				this.Y,
				this.Z
			};
			return result;
		}
		/// <summary>
		/// Creates a dictionary that contains components of this vector.
		/// </summary>
		/// <returns>
		/// A dictionary which capacity is set to 3 where components of the vector can be accessed with
		/// keys of same names.
		/// </returns>
		public Dictionary<string, float> ToDictionary()
		{
			Dictionary<string, float> result = new Dictionary<string, float>(3)
			{
				{"X", this.X},
				{"Y", this.Y},
				{"Z", this.Z}
			};
			return result;
		}
		#endregion
		#region Operators
		#region Arithmetic Operators
		/// <summary>
		/// Multiplies vector by given float factor.
		/// </summary>
		/// <param name="v">    Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of multiplication.</returns>
		public static Vector3 operator *(Vector3 v, float scale)
		{
			return new Vector3(v.X * scale, v.Y * scale, v.Z * scale);
		}
		/// <summary>
		/// Multiplies vector by given float factor.
		/// </summary>
		/// <param name="v">    Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of multiplication.</returns>
		public static Vector3 operator *(float scale, Vector3 v)
		{
			return v * scale;
		}
		/// <summary>
		/// Divides vector by given float value.
		/// </summary>
		/// <param name="v">    Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of division.</returns>
		public static Vector3 operator /(Vector3 v, float scale)
		{
			scale = 1.0f / scale;

			return new Vector3(v.X * scale, v.Y * scale, v.Z * scale);
		}
		/// <summary>
		/// Adds one vector to another.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of addition.</returns>
		public static Vector3 operator +(Vector3 left, Vector3 right)
		{
			return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}
		/// <summary>
		/// Adds one vector to another.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of addition.</returns>
		public static Vector3 operator +(Vector2 left, Vector3 right)
		{
			return new Vector3(left.X + right.X, left.Y + right.Y, right.Z);
		}
		/// <summary>
		/// Adds one vector to another.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of addition.</returns>
		public static Vector3 operator +(Vector3 left, Vector2 right)
		{
			return new Vector3(left.X + right.X, left.Y + right.Y, left.Z);
		}
		/// <summary>
		/// Subtracts one vector from another.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of subtraction.</returns>
		public static Vector3 operator -(Vector3 left, Vector3 right)
		{
			return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		}
		/// <summary>
		/// Subtracts one vector from another.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of subtraction.</returns>
		public static Vector3 operator -(Vector2 left, Vector3 right)
		{
			return new Vector3(left.X - right.X, left.Y - right.Y, 0 - right.Z);
		}
		/// <summary>
		/// Subtracts one vector from another.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of subtraction.</returns>
		public static Vector3 operator -(Vector3 left, Vector2 right)
		{
			return new Vector3(left.X - right.X, left.Y - right.Y, left.Z);
		}
		#endregion
		#region Product Operators
		/// <summary>
		/// Calculates dot product of two vectors.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Dot product.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float operator |(Vector3 left, Vector3 right)
		{
			return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
		}
		/// <summary>
		/// Calculates cross product of two vectors.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Cross product.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 operator %(Vector3 left, Vector3 right)
		{
			return
				new Vector3
				(
					left.Y * right.Z - left.Z * right.Y,
					left.Z * right.X - left.X * right.Z,
					left.X * right.Y - left.Y * right.X
				);
		}
		#endregion
		#region Unary Operators
		/// <summary>
		/// Gets flipped version of given vector.
		/// </summary>
		/// <param name="v">Vector to flip.</param>
		/// <returns>Flipped vector.</returns>
		public static Vector3 operator -(Vector3 v)
		{
			return v.Flipped;
		}
		/// <summary>
		/// Gets flipped version of given vector.
		/// </summary>
		/// <param name="v">Vector to flip.</param>
		/// <returns>Flipped vector.</returns>
		public static Vector3 operator !(Vector3 v)
		{
			return v.Flipped;
		}
		#endregion
		#region Comparison Operators
		/// <summary>
		/// Checks equality of two given vectors.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if two vectors are equal.</returns>
		public static bool operator ==(Vector3 left, Vector3 right)
		{
			// ReSharper disable CompareOfFloatsByEqualityOperator
			return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}
		/// <summary>
		/// Checks equality of two given vectors.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if two vectors are not equal.</returns>
		public static bool operator !=(Vector3 left, Vector3 right)
		{
			return !(left == right);
		}
		#endregion
		#region Conversion Operators
		/// <summary>
		/// Implicitly converts given vector to <see cref="ColorSingle"/> instance.
		/// </summary>
		/// <param name="vec">Vector to convert.</param>
		/// <returns>
		/// <see cref="ColorSingle"/> object where R is vector's X-component, G - Y, B - Z.
		/// </returns>
		public static implicit operator ColorSingle(Vector3 vec)
		{
			return new ColorSingle(vec.X, vec.Y, vec.Z);
		}
		#endregion
		#endregion
		#region Modification
		/// <summary>
		/// Creates new instance of <see cref="Vector3"/> where Y, Z and W components are of this instance
		/// and X component is specified by given value.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="value">New value for X component.</param>
		/// <returns>Modified vector.</returns>
		public Vector3 ModifyX(float value)
		{
			return new Vector3(value, this.Y, this.Z);
		}
		/// <summary>
		/// Creates new instance of <see cref="Vector3"/> where X, Z and W components are of this instance
		/// and Y component is specified by given value.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="value">New value for Y component.</param>
		/// <returns>Modified vector.</returns>
		public Vector3 ModifyY(float value)
		{
			return new Vector3(this.X, value, this.Z);
		}
		/// <summary>
		/// Creates new instance of <see cref="Vector3"/> where X, Y and W components are of this instance
		/// and Z component is specified by given value.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="value">New value for Z component.</param>
		/// <returns>Modified vector.</returns>
		public Vector3 ModifyZ(float value)
		{
			return new Vector3(this.X, this.Y, value);
		}
		/// <summary>
		/// Creates new <see cref="Vector3"/> which represents this vector with modified values.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="offset">   Index of first component to be modified.</param>
		/// <param name="newValues">New values for components to modify.</param>
		/// <returns>Modified vector.</returns>
		public Vector3 ModifyVector(int offset, params float[] newValues)
		{
			if (offset < 0 || offset > 2)
			{
				throw new ArgumentOutOfRangeException("offset", "offset must be belong to interval [0; 2]");
			}

			Vector3 result = new Vector3(this.X, this.Y, this.Z);
			for (int i = offset, j = 0; j < newValues.Length && i < 3; i++, j++)
			{
				result[i] = newValues[j];
			}
			return result;
		}
		#endregion
		#region Normalizations
		/// <summary>
		/// Normalizes this vector.
		/// </summary>
		public void Normalize()
		{
			float fInvLen = MathHelpers.ReciprocalSquareRoot(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
			this.X *= fInvLen; this.Y *= fInvLen; this.Z *= fInvLen;
		}
		#endregion
		#region Clamping
		/// <summary>
		/// If length of this vector is greater then specified value, then downscales this vector to make
		/// its length match given value.
		/// </summary>
		/// <param name="maxLength">Maximal length this vector should have.</param>
		public void ClampLength(float maxLength)
		{
			float sqrLength = this.LengthSquared;
			if (sqrLength > (maxLength * maxLength))
			{
				var scale = maxLength * MathHelpers.ReciprocalSquareRoot(sqrLength);
				this.X *= scale;
				this.Y *= scale;
				this.Z *= scale;
			}
		}
		/// <summary>
		/// Restricts a vector to be within a specified rectangular cuboid.
		/// </summary>
		/// <param name="value">The vector to clamp.</param>
		/// <param name="min">  The minimum vertex of cuboid.</param>
		/// <param name="max">  The maximum vertex of cuboid.</param>
		/// <returns>The clamped vector.</returns>
		public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
		{
			Vector3 result;
			Clamp(ref value, ref min, ref max, out result);
			return result;
		}
		/// <summary>
		/// Restricts a vector to be within a specified rectangular cuboid.
		/// </summary>
		/// <param name="value"> The vector to clamp.</param>
		/// <param name="min">   The minimum vertex of cuboid.</param>
		/// <param name="max">   The maximum vertex of cuboid.</param>
		/// <param name="result">When the method completes, contains the clamped vector.</param>
		public static void Clamp(ref Vector3 value, ref Vector3 min, ref Vector3 max, out Vector3 result)
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

			result = new Vector3(x, y, z);
		}
		#endregion
		#region Calculating Distances
		/// <summary>
		/// Gets distance between this vector and another one.
		/// </summary>
		/// <param name="vec1">Another vector.</param>
		/// <returns>
		/// Length of the vector that goes from end of another vector to the end of this one.
		/// </returns>
		public float GetDistance(Vector3 vec1)
		{
			return (float)Math.Sqrt((this.X - vec1.X) * (this.X - vec1.X) + (this.Y - vec1.Y) * (this.Y - vec1.Y) + (this.Z - vec1.Z) * (this.Z - vec1.Z));
		}
		/// <summary>
		/// Gets squared distance between this vector and another one.
		/// </summary>
		/// <param name="vec1">Another vector.</param>
		/// <returns>
		/// Squared length of the vector that goes from end of another vector to the end of this one.
		/// </returns>
		public float GetDistanceSquared(Vector3 vec1)
		{
			return (this.X - vec1.X) * (this.X - vec1.X) + (this.Y - vec1.Y) * (this.Y - vec1.Y) + (this.Z - vec1.Z) * (this.Z - vec1.Z);
		}
		/// <summary>
		/// Gets projected distance between this vector and another one.
		/// </summary>
		/// <param name="v">Another vector.</param>
		/// <returns>
		/// Length of the vector that goes from end of another vector to the end of this one and projected
		/// onto XY plane.
		/// </returns>
		public float GetDistance2D(Vector3 v)
		{
			return (float)Math.Sqrt((this.X - v.X) * (this.X - v.X) + (this.Y - v.Y) * (this.Y - v.Y));
		}
		/// <summary>
		/// Gets squared projected distance between this vector and another one.
		/// </summary>
		/// <param name="v">Another vector.</param>
		/// <returns>
		/// Squared length of the vector that goes from end of another vector to the end of this one and
		/// projected onto XY plane.
		/// </returns>
		public float GetDistance2DSquared(Vector3 v)
		{
			return (this.X - v.X) * (this.X - v.X) + (this.Y - v.Y) * (this.Y - v.Y);
		}
		#endregion
		#region Maximums And Minimums
		/// <summary>
		/// Returns a vector containing the biggest components of the specified vectors.
		/// </summary>
		/// <param name="left">  The first source vector.</param>
		/// <param name="right"> The second source vector.</param>
		/// <param name="result">
		/// When the method completes, contains an new vector composed of the largest components of the
		/// source vectors.
		/// </param>
		public static void Max(ref Vector3 left, ref Vector3 right, out Vector3 result)
		{
			result = new Vector3
			{
				X = (left.X > right.X) ? left.X : right.X,
				Y = (left.Y > right.Y) ? left.Y : right.Y,
				Z = (left.Z > right.Z) ? left.Z : right.Z
			};
		}
		/// <summary>
		/// Returns a vector containing the largest components of the specified vectors.
		/// </summary>
		/// <param name="left"> The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>A vector containing the largest components of the source vectors.</returns>
		public static Vector3 Max(Vector3 left, Vector3 right)
		{
			Vector3 result;
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
		public static void Min(ref Vector3 left, ref Vector3 right, out Vector3 result)
		{
			result = new Vector3
			{
				X = (left.X < right.X) ? left.X : right.X,
				Y = (left.Y < right.Y) ? left.Y : right.Y,
				Z = (left.Z < right.Z) ? left.Z : right.Z
			};
		}
		/// <summary>
		/// Returns a vector containing the smallest components of the specified vectors.
		/// </summary>
		/// <param name="left"> The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>A vector containing the smallest components of the source vectors.</returns>
		public static Vector3 Min(Vector3 left, Vector3 right)
		{
			Vector3 result;
			Min(ref left, ref right, out result);
			return result;
		}
		#endregion
		#region Products
		/// <summary>
		/// Calculates cross product of two vectors.
		/// </summary>
		/// <param name="first">First vector.</param>
		/// <param name="v">    Second vector.</param>
		/// <returns>Cross product.</returns>
		public static Vector3 Cross(Vector3 first, Vector3 v)
		{
			return
				new Vector3
				(
					first.Y * v.Z - first.Z * v.Y,
					first.Z * v.X - first.X * v.Z,
					first.X * v.Y - first.Y * v.X
				);
		}
		/// <summary>
		/// Calculates dot product of two vectors.
		/// </summary>
		/// <param name="v0">First vector.</param>
		/// <param name="v1">Second vector.</param>
		/// <returns>Dot product.</returns>
		public static float Dot(Vector3 v0, Vector3 v1)
		{
			return v0.X * v1.X + v0.Y * v1.Y + v0.Z * v1.Z;
		}
		/// <summary>
		/// Calculates mixed product of three vectors.
		/// </summary>
		/// <remarks>a.Dot(b.Cross(c))</remarks>
		/// <param name="v0">First vector.</param>
		/// <param name="v1">Second vector.</param>
		/// <param name="v2">Third vector.</param>
		/// <returns>Dot product of first vector and cross product of second and third.</returns>
		public static float Mixed(Vector3 v0, Vector3 v1, Vector3 v2)
		{
			Vector3 cross = new Vector3
							(
								v1.Y * v2.Z - v1.Z * v2.Y,
								v1.Z * v2.X - v1.X * v2.Z,
								v1.X * v2.Y - v1.Y * v2.X
							);
			return v0.X * cross.X + v0.Y * cross.Y + v0.Z * cross.Z;
		}
		#endregion
		#region Generic Operations
		/// <summary>
		/// Indicates whether this vector is a zero vector.
		/// </summary>
		/// <param name="epsilon">Precision of comparison.</param>
		/// <returns>
		/// True, if difference between this vector and zero vector is within specified precision bounds.
		/// </returns>
		public bool IsZero(float epsilon = 0f)
		{
			return (Math.Abs(this.X) <= epsilon) && (Math.Abs(this.Y) <= epsilon) && (Math.Abs(this.Z) <= epsilon);
		}
		/// <summary>
		/// Indicates whether this vector is equal to other vector.
		/// </summary>
		/// <param name="v1">     Another vector to compare this one with.</param>
		/// <param name="epsilon">Precision of comparison.</param>
		/// <returns>
		/// True, if difference between this vector and another vector is within specified precision
		/// bounds.
		/// </returns>
		public bool IsEquivalent(Vector3 v1, float epsilon = 0.05f)
		{
			return ((Math.Abs(this.X - v1.X) <= epsilon) && (Math.Abs(this.Y - v1.Y) <= epsilon) && (Math.Abs(this.Z - v1.Z) <= epsilon));
		}
		/// <summary>
		/// Indicates whether this vector is a unit vector.
		/// </summary>
		/// <param name="epsilon">Precision of comparison.</param>
		/// <returns>
		/// True, if difference between this vector and unit vector is within specified precision bounds.
		/// </returns>
		public bool IsUnit(float epsilon = 0.05f)
		{
			return (Math.Abs(1 - this.LengthSquared) <= epsilon);
		}
		/// <summary>
		/// Flips direction of this vector.
		/// </summary>
		public void Flip()
		{
			this.X = -this.X;
			this.Y = -this.Y;
			this.Z = -this.Z;
		}
		/// <summary>
		/// Calculates hash code of given vector.
		/// </summary>
		/// <returns>Hash code of given vector.</returns>
		public override int GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;

				hash = hash * 23 + this.X.GetHashCode();
				hash = hash * 23 + this.Y.GetHashCode();
				hash = hash * 23 + this.Z.GetHashCode();

				return hash;
			}
		}
		/// <summary>
		/// Determines whether this vector is equal to another object.
		/// </summary>
		/// <param name="obj">Another object.</param>
		/// <returns>True if objects are equal.</returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			if (obj is Vector3)
				return this == (Vector3)obj;

			return false;
		}
		/// <summary>
		/// Determines whether this vector is equal to another one.
		/// </summary>
		/// <param name="other">Another vector.</param>
		/// <returns>True, if vectors are equal.</returns>
		public bool Equals(Vector3 other)
		{
			return this.IsEquivalent(other, MathHelpers.ZeroTolerance);
		}
		/// <summary>
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public int CompareTo(Vector3 other)
		{
			int pos = this.X.CompareTo(other.X);
			if (pos != 0) return pos;
			pos = this.Y.CompareTo(other.Y);
			return pos == 0 ? this.Z.CompareTo(other.Z) : pos;
		}
		#endregion
		#region Text Conversions
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{0},{1},{2}", this.X, this.Y, this.Z);
		}
		/// <summary>
		/// Creates vector represented by given text.
		/// </summary>
		/// <param name="value">Text that is supposed to be equivalent of the vector.</param>
		/// <returns>Vector which is an equivalent of the given text.</returns>
		public static Vector3 Parse(string value)
		{
			if (String.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException("value", "Attempt to parse null or empty string as a vector.");
			}
			try
			{
				string[] split = value.Split(',');
				return new Vector3(System.Convert.ToSingle(split[0]),
					System.Convert.ToSingle(split[1]), System.Convert.ToSingle(split[2]));
			}
			catch (Exception ex)
			{
				throw new ArgumentException
				(
					"Vector3.Parse:Given string doesn't contain an equivalent of the vector.", "value", ex
				);
			}
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
		}
		#endregion
		#endregion
	}
}