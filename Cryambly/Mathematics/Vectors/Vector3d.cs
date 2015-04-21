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
	public partial struct Vector3d : IEquatable<Vector3d>, IEnumerable<double>, IComparable<Vector3d>
	{
		#region Static Fields
		/// <summary>
		/// A zero vector - a point.
		/// </summary>
		public static readonly Vector3d Zero = new Vector3d(0, 0, 0);
		/// <summary>
		/// A vector that points straight up.
		/// </summary>
		public static readonly Vector3d Up = new Vector3d(0, 0, 1);
		/// <summary>
		/// A vector that points straight down.
		/// </summary>
		public static readonly Vector3d Down = new Vector3d(0, 0, -1);
		/// <summary>
		/// A vector that points forward.
		/// </summary>
		public static readonly Vector3d Forward = new Vector3d(0, 1, 0);
		/// <summary>
		/// A vector that points backward.
		/// </summary>
		public static readonly Vector3d Backward = new Vector3d(0, -1, 0);
		/// <summary>
		/// A vector that points to the right.
		/// </summary>
		public static readonly Vector3d Right = new Vector3d(1, 0, 0);
		/// <summary>
		/// A vector that points to the left.
		/// </summary>
		public static readonly Vector3d Left = new Vector3d(-1, 0, 0);
		/// <summary>
		/// Number of bytes each instance of this structure consists of.
		/// </summary>
		public static readonly int ByteCount = Marshal.SizeOf(typeof(Vector3d));
		/// <summary>
		/// Number of components of this vector.
		/// </summary>
		public const int ComponentCount = 3;
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
		/// <summary>
		/// The Z component of the vector.
		/// </summary>
		public double Z;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets length of this vector.
		/// </summary>
		public double Length
		{
			get
			{
				return Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
			}
			set
			{
				double lengthSquared = this.LengthSquared;
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
		public double LengthSquared
		{
			get
			{
				return this.X * this.X + this.Y * this.Y + this.Z * this.Z;
			}
		}
		/// <summary>
		/// Gets length of this vector projected onto XY plane.
		/// </summary>
		public double Length2D
		{
			get
			{
				return Math.Sqrt(this.X * this.X + this.Y * this.Y);
			}
		}
		/// <summary>
		/// Gets squared length of this vector projected onto XY plane.
		/// </summary>
		public double Length2DSquared
		{
			get
			{
				return this.X * this.X + this.Y * this.Y;
			}
		}
		/// <summary>
		/// Gets normalized vector.
		/// </summary>
		public Vector3d Normalized
		{
			get
			{
				double fInvLen = MathHelpers.ReciprocalSquareRoot(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
				return this * fInvLen;
			}
		}
		/// <summary>
		/// Gets volume of the perpendicular cuboid defined by this vector.
		/// </summary>
		public double Volume
		{
			get
			{
				return this.X * this.Y * this.Z;
			}
		}
		/// <summary>
		/// Gets vector which components are absolute values of components of this vector.
		/// </summary>
		public Vector3d Absolute
		{
			get
			{
				return new Vector3d(Math.Abs(this.X), Math.Abs(this.Y), Math.Abs(this.Z));
			}
		}
		/// <summary>
		/// Gets flipped vector.
		/// </summary>
		public Vector3d Flipped
		{
			get { return -this; }
		}
		/// <summary>
		/// Gets simplest vector that is perpendicular to this one.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Cannot get vector that is orthogonal to zero.
		/// </exception>
		public Vector3d OrthogonalSafe
		{
			get
			{
				// ReSharper disable CompareOfFloatsByEqualityOperator
				if (this.X == 0 && this.Y == 0 && this.Z == 0)
				// ReSharper restore CompareOfFloatsByEqualityOperator
				{
					throw new InvalidOperationException("Cannot get vector that is orthogonal to zero.");
				}
				return new Vector3d(-this.Y, this.X, 0);
			}
		}
		/// <summary>
		/// Gets simplest vector that is perpendicular to this one.
		/// </summary>
		public Vector3d Orthogonal
		{
			get
			{
				// ReSharper disable CompareOfFloatsByEqualityOperator
				if (this.X == 0 && this.Y == 0 && this.Z == 0)
				// ReSharper restore CompareOfFloatsByEqualityOperator
				{
					return Vector3d.Up;
				}
				return new Vector3d(-this.Y, this.X, 0);
			}
		}
		/// <summary>
		/// Gets simplest vector that is perpendicular to this one if this one is not zero.
		/// </summary>
		public Vector3d? SelectiveOrthogonal
		{
			get
			{
				// ReSharper disable CompareOfFloatsByEqualityOperator
				if (this.X == 0 && this.Y == 0 && this.Z == 0)
				// ReSharper restore CompareOfFloatsByEqualityOperator
				{
					return null;
				}
				return new Vector3d(-this.Y, this.X, 0);
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
		public double this[int index]
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
				return
					MathHelpers.IsNumberValid(this.X) &&
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
				double[] components = { this.X, this.Y, this.Z };

				Buffer.BlockCopy(components, 0, bytes, 0, bytes.Length);
				return bytes;
			}
		}
		#endregion
		#region Interface
		#region Constructors
		/// <summary>
		/// Creates new <see cref="Vector3d"/> with specified components.
		/// </summary>
		/// <param name="vx">X-component of new vector.</param>
		/// <param name="vy">Y-component of new vector.</param>
		/// <param name="vz">Z-component of new vector.</param>
		public Vector3d(double vx, double vy, double vz)
			: this()
		{
			this.X = vx;
			this.Y = vy;
			this.Z = vz;
		}
		/// <summary>
		/// Creates new <see cref="Vector3d"/>.
		/// </summary>
		/// <param name="f"><see cref="Single"/> value to assign to all components of new vector.</param>
		public Vector3d(double f)
			: this()
		{
			this.X = this.Y = this.Z = f;
		}
		/// <summary>
		/// Creates new <see cref="Vector3d"/> from specified <see cref="Vector2"/>.
		/// </summary>
		/// <param name="v2"><see cref="Vector2"/> that defines X and Y components of new vector.</param>
		public Vector3d(Vector2d v2)
			: this()
		{
			this.X = v2.X;
			this.Y = v2.Y;
			this.Z = 0;
		}
		/// <summary>
		/// Creates new <see cref="Vector3d"/> from specified <see cref="Quaternion"/> instance.
		/// </summary>
		/// <param name="q">Quaternion that defines new vector.</param>
		public Vector3d(Quaternion q)
			: this()
		{
			this.Y = Math.Asin(Math.Max(-1.0f, Math.Min(1.0f, -(q.X * q.Z - q.W * q.Y) * 2)));
			if (Math.Abs(Math.Abs(this.Y) - (Math.PI * 0.5f)) < 0.01f)
			{
				this.X = 0;
				this.Z = Math.Atan2(-2 * (q.X * q.Y - q.W * q.Z), 1 - (q.X * q.X + q.Z * q.Z) * 2);
			}
			else
			{
				this.X = Math.Atan2((q.Y * q.Z + q.W * q.X) * 2, 1 - (q.X * q.X + q.Y * q.Y) * 2);
				this.Z = Math.Atan2((q.X * q.Y + q.W * q.Z) * 2, 1 - (q.Z * q.Z + q.Y * q.Y) * 2);
			}
		}
		/// <summary>
		/// Creates new <see cref="Vector3d"/>.
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
		public Vector3d(IList<double> values)
			: this()
		{
			if (values == null) return;
			for (int i = 0; i < Vector3d.ComponentCount || i < values.Count; i++)
			{
				this[i] = values[i];
			}
		}
		/// <summary>
		/// Creates new <see cref="Vector3d"/>.
		/// </summary>
		/// <param name="values">
		/// A <see cref="Dictionary{TKey,TValue}"/> which is used to initialize new vector.
		/// </param>
		public Vector3d(IDictionary<string, double> values)
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
		public double[] ToArray()
		{
			return new[] { this.X, this.Y, this.Z };
		}
		/// <summary>
		/// Creates a list that contains components of this vector.
		/// </summary>
		/// <returns>A list of four elements which contain corresponding vector components.</returns>
		public List<double> ToList()
		{
			List<double> result = new List<double>(3)
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
		public Dictionary<string, double> ToDictionary()
		{
			Dictionary<string, double> result = new Dictionary<string, double>(3)
			{
				{"X", this.X},
				{"Y", this.Y},
				{"Z", this.Z}
			};
			return result;
		}
		#endregion
		#region Modification
		/// <summary>
		/// Creates new instance of <see cref="Vector3d"/> where Y, Z and W components are of this instance
		/// and X component is specified by given value.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="value">New value for X component.</param>
		/// <returns>Modified vector.</returns>
		public Vector3d ModifyX(double value)
		{
			return new Vector3d(value, this.Y, this.Z);
		}
		/// <summary>
		/// Creates new instance of <see cref="Vector3d"/> where X, Z and W components are of this instance
		/// and Y component is specified by given value.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="value">New value for Y component.</param>
		/// <returns>Modified vector.</returns>
		public Vector3d ModifyY(double value)
		{
			return new Vector3d(this.X, value, this.Z);
		}
		/// <summary>
		/// Creates new instance of <see cref="Vector3d"/> where X, Y and W components are of this instance
		/// and Z component is specified by given value.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="value">New value for Z component.</param>
		/// <returns>Modified vector.</returns>
		public Vector3d ModifyZ(double value)
		{
			return new Vector3d(this.X, this.Y, value);
		}
		/// <summary>
		/// Creates new <see cref="Vector3d"/> which represents this vector with modified values.
		/// </summary>
		/// <remarks>
		/// This method allows to simplify code where this instance is a property which you want to modify.
		/// </remarks>
		/// <param name="offset">   Index of first component to be modified.</param>
		/// <param name="newValues">New values for components to modify.</param>
		/// <returns>Modified vector.</returns>
		public Vector3d ModifyVector(int offset, params double[] newValues)
		{
			if (offset < 0 || offset > 2)
			{
				throw new ArgumentOutOfRangeException("offset", "offset must be belong to interval [0; 2]");
			}

			Vector3d result = new Vector3d(this.X, this.Y, this.Z);
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
			double fInvLen = MathHelpers.ReciprocalSquareRoot(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
			this.X *= fInvLen; this.Y *= fInvLen; this.Z *= fInvLen;
		}
		#endregion
		#region Clamping
		/// <summary>
		/// If length of this vector is greater then specified value, then downscales this vector to make
		/// its length match given value.
		/// </summary>
		/// <param name="maxLength">Maximal length this vector should have.</param>
		public void ClampLength(double maxLength)
		{
			double sqrLength = this.LengthSquared;
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
		public static Vector3d Clamp(Vector3d value, Vector3d min, Vector3d max)
		{
			Vector3d result;
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
		public static void Clamp(ref Vector3d value, ref Vector3d min, ref Vector3d max, out Vector3d result)
		{
			double x = value.X;
			x = (x > max.X) ? max.X : x;
			x = (x < min.X) ? min.X : x;

			double y = value.Y;
			y = (y > max.Y) ? max.Y : y;
			y = (y < min.Y) ? min.Y : y;

			double z = value.Z;
			z = (z > max.Z) ? max.Z : z;
			z = (z < min.Z) ? min.Z : z;

			result = new Vector3d(x, y, z);
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
		public double GetDistance(Vector3d vec1)
		{
			return Math.Sqrt((this.X - vec1.X) * (this.X - vec1.X) + (this.Y - vec1.Y) * (this.Y - vec1.Y) + (this.Z - vec1.Z) * (this.Z - vec1.Z));
		}
		/// <summary>
		/// Gets squared distance between this vector and another one.
		/// </summary>
		/// <param name="vec1">Another vector.</param>
		/// <returns>
		/// Squared length of the vector that goes from end of another vector to the end of this one.
		/// </returns>
		public double GetDistanceSquared(Vector3d vec1)
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
		public double GetDistance2D(Vector3d v)
		{
			return Math.Sqrt((this.X - v.X) * (this.X - v.X) + (this.Y - v.Y) * (this.Y - v.Y));
		}
		/// <summary>
		/// Gets squared projected distance between this vector and another one.
		/// </summary>
		/// <param name="v">Another vector.</param>
		/// <returns>
		/// Squared length of the vector that goes from end of another vector to the end of this one and
		/// projected onto XY plane.
		/// </returns>
		public double GetDistance2DSquared(Vector3d v)
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
		public static void Max(ref Vector3d left, ref Vector3d right, out Vector3d result)
		{
			result = new Vector3d
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
		public static Vector3d Max(Vector3d left, Vector3d right)
		{
			Vector3d result;
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
		public static void Min(ref Vector3d left, ref Vector3d right, out Vector3d result)
		{
			result = new Vector3d
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
		public static Vector3d Min(Vector3d left, Vector3d right)
		{
			Vector3d result;
			Min(ref left, ref right, out result);
			return result;
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
		public bool IsZero(double epsilon = 0f)
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
		public bool IsEquivalent(Vector3d v1, double epsilon = 0.05f)
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
		public bool IsUnit(double epsilon = 0.05f)
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
			if (obj is Vector3d && this.Equals((Vector3d)obj)) return true;
			Vector3d? vec = obj as Vector3d?;
			return vec.HasValue && this.Equals(vec.Value);
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
		public static Vector3d Parse(string value)
		{
			if (String.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException("value", "Attempt to parse null or empty string as a vector.");
			}
			try
			{
				string[] split = value.Split(',');
				return new Vector3d(System.Convert.ToSingle(split[0]),
					System.Convert.ToSingle(split[1]), System.Convert.ToSingle(split[2]));
			}
			catch (Exception ex)
			{
				throw new ArgumentException
				(
					"Vector3d.Parse:Given string doesn't contain an equivalent of the vector.", "value", ex
				);
			}
		}
		#endregion
		#endregion
	}
}