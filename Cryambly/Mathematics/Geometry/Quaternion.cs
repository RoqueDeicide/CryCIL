﻿using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using CryCil.MemoryMapping;

namespace CryCil.Geometry
{
	/// <summary>
	/// Quaternion is a combination of 4 numbers (sometimes referred to as Euler
	/// parameters), that can be used to represent rotation and orientation.
	/// </summary>
	/// <remarks>
	/// Quaternions are, basically, 3x3 matrices in a more compact form.
	/// </remarks>
	[StructLayout(LayoutKind.Explicit)]
	public struct Quaternion : IEquatable<Quaternion>
	{
		/// <summary>
		/// The identity <see cref="Quaternion"/> (0, 0, 0, 1).
		/// </summary>
		/// <remarks>
		/// Identity quaternion has W equal to 1, and vector is equal to 0.
		/// </remarks>
		public static readonly Quaternion Identity = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
		#region Fields
		/// <summary>
		/// X-component of the quaternion.
		/// </summary>
		[FieldOffset(0)]
		public float X;
		/// <summary>
		/// Y-component of the quaternion.
		/// </summary>
		[FieldOffset(4)]
		public float Y;
		/// <summary>
		/// Z-component of the quaternion.
		/// </summary>
		[FieldOffset(8)]
		public float Z;
		/// <summary>
		/// W-component of the quaternion.
		/// </summary>
		[FieldOffset(12)]
		public float W;
		/// <summary>
		/// Vector portion of this quaternion.
		/// </summary>
		[FieldOffset(0)]
		public Vector3 Vector;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the component at the specified index.
		/// </summary>
		/// <value>
		/// The value of the X, Y, Z, or W component, depending on the index.
		/// </value>
		/// <param name="index">
		/// The index of the component to access. Use 0 for the X component, 1 for the Y
		/// component, 2 for the Z component, and 3 for the W component.
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
						throw new ArgumentOutOfRangeException("index", "Attempt to access Euler" +
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
						throw new ArgumentOutOfRangeException("index", "Attempt to access Euler component" +
																	   " other then X, Y, Z or W.");
				}
			}
		}
		/// <summary>
		/// Gets norm (magnitude or length) of this quaternion.
		/// </summary>
		public float Norm
		{
			get
			{
				return (float)Math.Sqrt
					(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W);
			}
		}
		/// <summary>
		/// Gets inverted quaternion.
		/// </summary>
		/// <remarks>
		/// Inverted quaternion is the quaternion where vector part is negated.
		/// </remarks>
		public Quaternion Inverted
		{
			get
			{
				return new Quaternion(-this.X, -this.Y, -this.Z, this.W);
			}
		}
		/// <summary>
		/// Gets flipped quaternion.
		/// </summary>
		/// <remarks>
		/// Flipped quaternion is the quaternion where all 4 components are negated.
		/// </remarks>
		public Quaternion Flipped
		{
			get
			{
				return new Quaternion(-this.X, -this.Y, -this.Z, this.W);
			}
		}
		/// <summary>
		/// Treating this quaternion as a compressed rotation matrix, gets first column of
		/// that matrix.
		/// </summary>
		/// <remarks>
		/// First column of rotation matrix represents a vector that points to the right
		/// from the point that is oriented using this quaternion.
		/// </remarks>
		public Vector3 Column0 { get { return new Vector3(2 * (this.X * this.X + this.W * this.W) - 1, 2 * (this.Y * this.X + this.Z * this.W), 2 * (this.Z * this.X - this.Y * this.W)); } }
		/// <summary>
		/// Treating this quaternion as a compressed rotation matrix, gets second column
		/// of that matrix.
		/// </summary>
		/// <remarks>
		/// Second column of rotation matrix represents a vector that points forward from
		/// the point that is oriented using this quaternion.
		/// </remarks>
		public Vector3 Column1 { get { return new Vector3(2 * (this.X * this.Y - this.Z * this.W), 2 * (this.Y * this.Y + this.W * this.W) - 1, 2 * (this.Z * this.Y + this.X * this.W)); } }
		/// <summary>
		/// Treating this quaternion as a compressed rotation matrix, gets third column of
		/// that matrix.
		/// </summary>
		/// <remarks>
		/// Third column of rotation matrix represents a vector that points up from the
		/// point that is oriented using this quaternion.
		/// </remarks>
		public Vector3 Column2 { get { return new Vector3(2 * (this.X * this.Z + this.Y * this.W), 2 * (this.Y * this.Z - this.X * this.W), 2 * (this.Z * this.Z + this.W * this.W) - 1); } }
		/// <summary>
		/// Treating this quaternion as a compressed rotation matrix, gets first row of
		/// that matrix.
		/// </summary>
		/// <remarks>
		/// First row of rotation matrix contains X-coordinates of rotation axes.
		/// </remarks>
		public Vector3 Row0 { get { return new Vector3(2 * (this.X * this.X + this.W * this.W) - 1, 2 * (this.X * this.Y - this.Z * this.W), 2 * (this.X * this.Z + this.Y * this.W)); } }
		/// <summary>
		/// Treating this quaternion as a compressed rotation matrix, gets second row of
		/// that matrix.
		/// </summary>
		/// <remarks>
		/// Second row of rotation matrix contains Y-coordinates of rotation axes.
		/// </remarks>
		public Vector3 Row1 { get { return new Vector3(2 * (this.Y * this.X + this.Z * this.W), 2 * (this.Y * this.Y + this.W * this.W) - 1, 2 * (this.Y * this.Z - this.X * this.W)); } }
		/// <summary>
		/// Treating this quaternion as a compressed rotation matrix, gets third row of
		/// that matrix.
		/// </summary>
		/// <remarks>
		/// Third row of rotation matrix contains Z-coordinates of rotation axes.
		/// </remarks>
		public Vector3 Row2 { get { return new Vector3(2 * (this.Z * this.X - this.Y * this.W), 2 * (this.Z * this.Y + this.X * this.W), 2 * (this.Z * this.Z + this.W * this.W) - 1); } }
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of type <see cref="Quaternion"/>.
		/// </summary>
		/// <param name="x">X-component of the new quaternion.</param>
		/// <param name="y">Y-component of the new quaternion.</param>
		/// <param name="z">Z-component of the new quaternion.</param>
		/// <param name="w">W-component of the new quaternion.</param>
		public Quaternion(float x, float y, float z, float w)
			: this()
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}
		/// <summary>
		/// Creates new instance of type <see cref="Quaternion"/>.
		/// </summary>
		/// <param name="angles">
		/// A set of Euler angles that represent rotation that new quaternion should
		/// represent.
		/// </param>
		public Quaternion(EulerAngles angles)
			: this()
		{
			double sx, cx; MathHelpers.SinCos(angles.Pitch * 0.5, out sx, out cx);
			double sy, cy; MathHelpers.SinCos(angles.Roll * 0.5, out sy, out cy);
			double sz, cz; MathHelpers.SinCos(angles.Yaw * 0.5, out sz, out cz);

			this.W = (float)(cx * cy * cz + sx * sy * sz);
			this.X = (float)(cz * cy * sx - sz * sy * cx);
			this.Y = (float)(cz * sy * cx + sz * cy * sx);
			this.Z = (float)(sz * cy * cx - cz * sy * sx);
		}
		/// <summary>
		/// Creates new instance of type <see cref="Quaternion"/>.
		/// </summary>
		/// <param name="axis"> Axis of rotation.</param>
		/// <param name="angle">Angle of rotation.</param>
		public Quaternion(Vector3 axis, float angle)
			: this()
		{
			Contract.Assert(axis.IsUnit(0.0001f));

			float sine, cosine; MathHelpers.SinCos(angle / 2, out sine, out cosine);

			this.Vector = axis * sine;
			this.W = cosine;
		}
		/// <summary>
		/// Creates new instance of type <see cref="Quaternion"/>.
		/// </summary>
		/// <param name="axis">  Axis of rotation.</param>
		/// <param name="sine">  Sine of half-angle of rotation.</param>
		/// <param name="cosine">Cosine of half-angle of rotation.</param>
		public Quaternion(Vector3 axis, float sine, float cosine)
			: this()
		{
			Contract.Assert(axis.IsUnit(0.0001f));

			this.Vector = axis * sine;
			this.W = cosine;
		}
		/// <summary>
		/// Creates new instance of type <see cref="Quaternion"/>.
		/// </summary>
		/// <param name="originalDirection"> Normalized
		/// <see cref="Vector3"/> that represents direction to start rotation from.
		/// </param>
		/// <param name="desiredDirection">  Normalized
		/// <see cref="Vector3"/> that represents direction to finish rotation at.
		/// </param>
		public Quaternion
		(
			Vector3 originalDirection,
			Vector3 desiredDirection
		)
			: this()
		{
			Vector3 start = originalDirection;
			Vector3 end = desiredDirection;
			// Get dot product of two vectors so we can get the angle between them.
			float cosine = Vector3.Dot(start, end);
			// Axis of rotation, and a sine.
			Vector3 axis;
			float sine;
			// We need different behavior depending on cosine. If the angle is close to
			// 180 degrees, then we need to select fallback axis.
			if (cosine < -0.9999f)
			{
				axis =
					originalDirection.SelectiveOrthogonal
					?? desiredDirection.SelectiveOrthogonal
					?? Vector3.Up;
				cosine = 0;			// Cosine and sine of the half of the angle between vectors.
				sine = 1;			//
			}
			else
			{
				// Use cross product.
				axis = start % end;
				cosine = (1 - cosine) / 2;						// Get cosine of half-angle.
				sine = (float)Math.Sqrt(1 - cosine * cosine);	// Get sine from cosine.
			}
			// Define quaternion from calculated angle-axis combination.
			this.Vector = axis * sine;
			this.W = cosine;
		}
		/// <summary>
		/// Creates new instance of type <see cref="Quaternion"/>.
		/// </summary>
		/// <param name="matrix">
		/// Matrix that represents rotation that needs to be packed into the quaternion.
		/// </param>
		public unsafe Quaternion(Matrix33 matrix)
			: this(0, 0, 0, 1)
		{
			Contract.Assert(matrix.IsOrthonormalRightHanded);

			float trace = matrix.M00 + matrix.M11 + matrix.M22;
			float s;
			float t;

			int* next = stackalloc int[3];
			next[0] = 1;
			next[1] = 2;
			next[2] = 0;

			float* mat = (float*)&matrix;

			if (trace > 0.0f)
			{
				t = trace + 1.0f;
				s = MathHelpers.ReciprocalSquareRoot(t) * 0.5f;

				this.W = s * t;
				this.X = (matrix.M21 - matrix.M12) * s;
				this.Y = (matrix.M02 - matrix.M20) * s;
				this.Z = (matrix.M10 - matrix.M01) * s;
			}
			else
			{
				int i = 0;
				if (matrix.M11 > matrix.M00)
				{
					i = 1;
				}
				if (matrix.M22 > mat[i * 3 + i])
				{
					i = 2;
				}
				int j = next[i];
				int k = next[j];

				t = (mat[i * 3 + i] - (mat[j * 3 + j] + mat[k * 3 + k])) + 1.0f;
				s = MathHelpers.ReciprocalSquareRoot(t) * 0.5f;

				this[i] = s * t;
				this.W = (mat[k * 3 + j] - mat[j * 3 + k]) * s;
				this[j] = (mat[j * 3 + i] + mat[i * 3 + j]) * s;
				this[k] = (mat[k * 3 + i] + mat[i * 3 + k]) * s;
			}
		}
		/// <summary>
		/// Creates new instance of type <see cref="Quaternion"/>.
		/// </summary>
		/// <param name="matrix">Matrix that represents rotation that needs to be packed into the quaternion.</param>
		public Quaternion(Matrix34 matrix)
			: this(new Matrix33(matrix)) { }
		/// <summary>
		/// Creates new instance of type <see cref="Quaternion"/>.
		/// </summary>
		/// <param name="matrix">Matrix that represents rotation that needs to be packed into the quaternion.</param>
		public Quaternion(Matrix44 matrix)
			: this(new Matrix33(matrix)) { }
		#endregion
		#region Interface
		/// <summary>
		/// Inverts or conjugates this quaternion by flipping its vector part.
		/// </summary>
		/// <remarks>
		/// Inverted quaternion represents rotation in reverse direction.
		/// </remarks>
		public void Invert()
		{
			this.Vector = -this.Vector;
		}
		/// <summary>
		/// Flips this quaternion by negating every component.
		/// </summary>
		/// <remarks>Flipping quaternion does not result in change of rotation.</remarks>
		public void Flip()
		{
			this.X = -this.X;
			this.Y = -this.Y;
			this.Z = -this.Z;
			this.W = -this.W;
		}
		/// <summary>
		/// Converts this quaternion into unit quaternion.
		/// </summary>
		/// <remarks>
		/// Since this implementation is about rotations, non-unit quaternions are not
		/// really encountered here, use this function only after a multitude of
		/// operations to clean all accumulated rounding errors.
		/// </remarks>
		public void Normalize()
		{
			float norm = this.Norm;
			// Set it to identity quaternion if it's norm is close to zero.
			if (norm < MathHelpers.ZeroTolerance)
			{
				this.X = 0;
				this.Y = 0;
				this.Z = 0;
				this.W = 1;
			}
			else
			{
				this.X /= norm;
				this.Y /= norm;
				this.Z /= norm;
				this.W /= norm;
			}
		}
		#region Interpolations
		/// <summary>
		/// Sets this quaternion to result of normalized linear interpolation.
		/// </summary>
		/// <param name="start"> Starting quaternion in interpolation.</param>
		/// <param name="end">   Ending quaternion in interpolation.</param>
		/// <param name="amount">
		/// Number between 0 and 1 that sets position of resulting quaternion between
		/// <paramref name="start"/> and <paramref name="end"/>.
		/// </param>
		public void NormalizedLinearInterpolation(Quaternion start, Quaternion end, float amount)
		{
			var q = end;
			if ((start | q) < 0) { q = q.Flipped; }

			var vDiff = q.Vector - start.Vector;

			this.Vector = start.Vector + (vDiff * amount);
			this.W = start.W + ((q.W - start.W) * amount);

			this.Normalize();
		}
		/// <summary>
		/// Sets this quaternion to result of normalized linear interpolation using a
		/// different algorithm.
		/// </summary>
		/// <param name="start"> Starting quaternion in interpolation.</param>
		/// <param name="end">   Ending quaternion in interpolation.</param>
		/// <param name="amount">
		/// Number between 0 and 1 that sets position of resulting quaternion between
		/// <paramref name="start"/> and <paramref name="end"/>.
		/// </param>
		public void NormalizedLinearInterpolation2(Quaternion start, Quaternion end, float amount)
		{
			var q = end;
			var cosine = (start | q);
			if (cosine < 0) q = q.Flipped;
			var k = (1 - Math.Abs(cosine)) * 0.4669269f;
			var s = 2 * k * amount * amount * amount - 3 * k * amount * amount + (1 + k) * amount;
			this.X = start.X * (1.0f - s) + q.X * s;
			this.Y = start.Y * (1.0f - s) + q.Y * s;
			this.Z = start.Z * (1.0f - s) + q.Z * s;
			this.W = start.W * (1.0f - s) + q.W * s;
			this.Normalize();
		}
		/// <summary>
		/// Sets this quaternion to result of spherical linear interpolation.
		/// </summary>
		/// <param name="start"> Starting quaternion in interpolation.</param>
		/// <param name="end">   Ending quaternion in interpolation.</param>
		/// <param name="amount">
		/// Number between 0 and 1 that sets position of resulting quaternion between
		/// <paramref name="start"/> and <paramref name="end"/>.
		/// </param>
		public void SphericalLinearInterpolation(Quaternion start, Quaternion end, float amount)
		{
			var p = start;
			var q = end;
			var q2 = new Quaternion();

			var cosine = (p | q);
			if (cosine < 0.0f) { cosine = -cosine; q = q.Flipped; } // take shortest arc
			if (cosine > 0.9999f)
			{
				this.NormalizedLinearInterpolation(p, q, amount);
				return;
			}
			// from now on, a division by 0 is not possible any more
			q2.W = q.W - p.W * cosine;
			q2.X = q.X - p.X * cosine;
			q2.Y = q.Y - p.Y * cosine;
			q2.Z = q.Z - p.Z * cosine;
			var sine = Math.Sqrt(q2 | q2);
			double s, c;

			MathHelpers.SinCos(Math.Atan2(sine, cosine) * amount, out s, out c);
			this.W = (float)(p.W * c + q2.W * s / sine);
			this.X = (float)(p.X * c + q2.X * s / sine);
			this.Y = (float)(p.Y * c + q2.Y * s / sine);
			this.Z = (float)(p.Z * c + q2.Z * s / sine);
		}
		/// <summary>
		/// </summary>
		/// <param name="start"> </param>
		/// <param name="end">   </param>
		/// <param name="amount"></param>
		public void ExpSlerp(Quaternion start, Quaternion end, float amount)
		{
			var q = end;
			if ((start | q) < 0) { q = q.Flipped; }
			this = start * MathHelpers.Exp(MathHelpers.Log(start.Inverted * q) * amount);
		}
		#endregion
		#region Predicates
		/// <summary>
		/// Determines whether this quaternion is sufficiently close to another one.
		/// </summary>
		/// <param name="q">      Another quaternion.</param>
		/// <param name="epsilon">Precision of comparison.</param>
		/// <returns>
		/// True, if this quaternion represents rotation equivalent to one represented by
		/// another quaternion.
		/// </returns>
		public bool IsEquivalent(Quaternion q, float epsilon = 0.05f)
		{
			var p = q.Flipped;
			bool t0 = (Math.Abs(this.X - q.X) <= epsilon) && (Math.Abs(this.Y - q.Y) <= epsilon) && (Math.Abs(this.Z - q.Z) <= epsilon) && (Math.Abs(this.W - q.W) <= epsilon);
			bool t1 = (Math.Abs(this.X - p.X) <= epsilon) && (Math.Abs(this.Y - p.Y) <= epsilon) && (Math.Abs(this.Z - p.Z) <= epsilon) && (Math.Abs(this.W - p.W) <= epsilon);
			t0 |= t1;
			return t0;
		}
		/// <summary>
		/// Determines whether this quaternion is Identity quaternion.
		/// </summary>
		/// <seealso cref="Quaternion.Identity"/>

		// ReSharper disable CompareOfFloatsByEqualityOperator
		public bool IsIdentity { get { return this.W == 1 && this.X == 0 && this.Y == 0 && this.Z == 0; } }
		// ReSharper restore CompareOfFloatsByEqualityOperator

		/// <summary>
		/// Determines whether modulus of this quaternion is equal 1.
		/// </summary>
		/// <param name="epsilon">Precision of comparison.</param>
		/// <returns>
		/// True, if modulus of this quaternion is approximately equal to 1.
		/// </returns>
		public bool IsUnit(float epsilon = 0.05f)
		{
			return Math.Abs(1 - ((this | this))) < epsilon;
		}
		/// <summary>
		/// Determines whether this quaternion is equal to another.
		/// </summary>
		/// <param name="other">Another quaternion.</param>
		/// <returns>True, if quaternions are equal.</returns>
		public bool Equals(Quaternion other)
		{
			return this.X.Equals(other.X) && this.Y.Equals(other.Y) && this.Z.Equals(other.Z) && this.W.Equals(other.W);
		}
		/// <summary>
		/// Determines whether this quaternion is equal to another object.
		/// </summary>
		/// <param name="obj">Another object.</param>
		/// <returns>True, if another object is a quaternion and they are equal.</returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Quaternion && Equals((Quaternion)obj);
		}
		/// <summary>
		/// Calculates simple hash code.
		/// </summary>
		/// <returns>Hash code.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = this.X.GetHashCode();
				hashCode = (hashCode * 397) ^ this.Y.GetHashCode();
				hashCode = (hashCode * 397) ^ this.Z.GetHashCode();
				hashCode = (hashCode * 397) ^ this.W.GetHashCode();
				return hashCode;
			}
		}
		#endregion
		#region Operators
		/// <summary>
		/// Determines whether two quaternions are equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if two quaternions are equal.</returns>
		public static bool operator ==(Quaternion left, Quaternion right)
		{
			return left.IsEquivalent(right, 0.0000001f);
		}
		/// <summary>
		/// Determines whether two quaternions are not equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if two quaternions are not equal.</returns>
		public static bool operator !=(Quaternion left, Quaternion right)
		{
			return !left.IsEquivalent(right, 0.0000001f);
		}
		/// <summary>
		/// Calculates dot-product of two quaternions.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Dot-product of two quaternions.</returns>
		public static float operator |(Quaternion left, Quaternion right)
		{
			return (left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W);
		}
		/// <summary>
		/// Concatenates rotations represented by two quaternions.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>
		/// Quaternion that represents rotation equivalent to rotation by left quaternion
		/// followed up by rotation by right quaternion.
		/// </returns>
		public static Quaternion operator *(Quaternion left, Quaternion right)
		{
			return new Quaternion(
				left.W * right.W - (left.X * right.X + left.Y * right.Y + left.Z * right.Z),
				left.Y * right.Z - left.Z * right.Y + left.W * right.X + left.X * right.W,
				left.Z * right.X - left.X * right.Z + left.W * right.Y + left.Y * right.W,
				left.X * right.Y - left.Y * right.X + left.W * right.Z + left.Z * right.W);
		}
		#endregion
		#endregion
	}
}