using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace CryEngine
{
	/// <summary>
	/// Represents a three dimensional mathematical vector.
	/// </summary>
	public struct Vec3
	{
		public static readonly Vec3 Zero = new Vec3(0, 0, 0);
		public static readonly Vec3 Up = new Vec3(0, 0, 1);
		public static readonly Vec3 Down = new Vec3(0, 0, -1);

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

		public Vec3(float vx, float vy, float vz)
		{
			X = vx;
			Y = vy;
			Z = vz;
		}

		public Vec3(float f)
		{
			X = Y = Z = f;
		}

		public Vec3(Vec2 v2)
		{
			X = v2.X;
			Y = v2.Y;
			Z = 0;
		}

		public Vec3(Quat q)
		{
			Y = (float)Math.Asin(Math.Max(-1.0f, Math.Min(1.0f, -(q.V.X * q.V.Z - q.W * q.V.Y) * 2)));
			if (Math.Abs(Math.Abs(Y) - (Math.PI * 0.5f)) < 0.01f)
			{
				X = 0;
				Z = (float)Math.Atan2(-2 * (q.V.X * q.V.Y - q.W * q.V.Z), 1 - (q.V.X * q.V.X + q.V.Z * q.V.Z) * 2);
			}
			else
			{
				X = (float)Math.Atan2((q.V.Y * q.V.Z + q.W * q.V.X) * 2, 1 - (q.V.X * q.V.X + q.V.Y * q.V.Y) * 2);
				Z = (float)Math.Atan2((q.V.X * q.V.Y + q.W * q.V.Z) * 2, 1 - (q.V.Z * q.V.Z + q.V.Y * q.V.Y) * 2);
			}
		}

		public bool IsZero(float epsilon = 0f)
		{
			return (Math.Abs(X) <= epsilon) && (Math.Abs(Y) <= epsilon) && (Math.Abs(Z) <= epsilon);
		}

		public bool IsEquivalent(Vec3 v1, float epsilon = 0.05f)
		{
			return ((Math.Abs(X - v1.X) <= epsilon) && (Math.Abs(Y - v1.Y) <= epsilon) && (Math.Abs(Z - v1.Z) <= epsilon));
		}

		public bool IsUnit(float epsilon = 0.05f)
		{
			return (Math.Abs(1 - LengthSquared) <= epsilon);
		}

		public void ClampLength(float maxLength)
		{
			float sqrLength = LengthSquared;
			if (sqrLength > (maxLength * maxLength))
			{
				var scale = maxLength * MathHelpers.ISqrt(sqrLength);
				X *= scale;
				Y *= scale;
				Z *= scale;
			}
		}

		public float GetDistance(Vec3 vec1)
		{
			return (float)Math.Sqrt((X - vec1.X) * (X - vec1.X) + (Y - vec1.Y) * (Y - vec1.Y) + (Z - vec1.Z) * (Z - vec1.Z));
		}

		public float GetDistanceSquared(Vec3 vec1)
		{
			return (X - vec1.X) * (X - vec1.X) + (Y - vec1.Y) * (Y - vec1.Y) + (Z - vec1.Z) * (Z - vec1.Z);
		}

		public float GetDistance2D(Vec3 v)
		{
			return (float)Math.Sqrt((X - v.X) * (X - v.X) + (Y - v.Y) * (Y - v.Y));
		}

		public float GetDistance2DSquared(Vec3 v)
		{
			return (X - v.X) * (X - v.X) + (Y - v.Y) * (Y - v.Y);
		}

		public void Normalize()
		{
			float fInvLen = MathHelpers.ISqrt(X * X + Y * Y + Z * Z);
			X *= fInvLen; Y *= fInvLen; Z *= fInvLen;
		}

		/// <summary>
		/// Project a point / vector on a (virtual) plane. Consider we have a plane going through
		/// the origin. Because d = 0 we need just the normal. The vector n is assumed to be a unit vector.
		/// </summary>
		/// <param name="i"></param>
		/// <param name="n"></param>
		public void SetProjection(Vec3 i, Vec3 n)
		{
			this = i - n * (n | i);
		}

		/// <summary>
		/// Calculate a reflection vector. Vec3 n is assumed to be a unit-vector.
		/// </summary>
		public void SetReflection(Vec3 i, Vec3 n)
		{
			this = (n * (i | n) * 2) - i;
		}

		public void SetLerp(Vec3 p, Vec3 q, float t)
		{
			Vec3 diff = q - p;
			this = p + (diff * t);
		}

		public void SetSlerp(Vec3 p, Vec3 q, float t)
		{
			// calculate cosine using the "inner product" between two vectors: p*q=cos(radiant)
			float cosine = MathHelpers.Clamp((p | q), -1f, 1f);
			//we explore the special cases where the both vectors are very close together,
			//in which case we approximate using the more economical LERP and avoid "divisions by zero" since sin(Angle) = 0  as   Angle=0
			if (cosine >= 0.99f)
			{
				SetLerp(p, q, t); //perform LERP:
				Normalize();
			}
			else
			{
				//perform SLERP: because of the LERP-check above, a "division by zero" is not possible
				float rad = (float)Math.Acos(cosine);
				float scale_0 = (float)Math.Sin((1 - t) * rad);
				float scale_1 = (float)Math.Sin(t * rad);
				this = (p * scale_0 + q * scale_1) / (float)Math.Sin(rad);
				Normalize();
			}
		}

		public Vec3 GetRotated(Vec3 axis, float angle)
		{
			return GetRotated(axis, (float)Math.Cos(angle), (float)Math.Sin(angle));
		}

		public Vec3 GetRotated(Vec3 axis, float cosa, float sina)
		{
			Vec3 zax = axis * (this | axis);
			Vec3 xax = this - zax;
			Vec3 yax = axis % xax;
			return xax * cosa + yax * sina + zax;
		}

		public Vec3 GetRotated(Vec3 center, Vec3 axis, float angle)
		{
			return center + (this - center).GetRotated(axis, angle);
		}

		public Vec3 GetRotated(Vec3 center, Vec3 axis, float cosa, float sina)
		{
			return center + (this - center).GetRotated(axis, cosa, sina);
		}

		public Vec3 ComponentMul(Vec3 rhs)
		{
			return new Vec3(X * rhs.X, Y * rhs.Y, Z * rhs.Z);
		}

		public float Dot(Vec3 v)
		{
			return X * v.X + Y * v.Y + Z * v.Z;
		}

		public Vec3 Cross(Vec3 v)
		{
			return new Vec3(Y * v.Z - Z * v.Y, Z * v.X - X * v.Z, X * v.Y - Y * v.X);
		}

		public void Flip()
		{
			X = -X;
			Y = -Y;
			Z = -Z;
		}

		public float Length
		{
			get
			{
				return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
			}
			set
			{
				float lengthSquared = LengthSquared;
				if (lengthSquared < 0.00001f * 0.00001f)
					return;

				lengthSquared = value * MathHelpers.ISqrt(lengthSquared);
				X *= lengthSquared;
				Y *= lengthSquared;
				Z *= lengthSquared;
			}
		}

		public float LengthSquared
		{
			get
			{
				return X * X + Y * Y + Z * Z;
			}
		}

		public float Length2D
		{
			get
			{
				return (float)Math.Sqrt(X * X + Y * Y);
			}
		}

		public float Length2DSquared
		{
			get
			{
				return X * X + Y * Y;
			}
		}

		public Vec3 Normalized
		{
			get
			{
				float fInvLen = MathHelpers.ISqrt(X * X + Y * Y + Z * Z);
				return this * fInvLen;
			}
		}

		public float Volume
		{
			get
			{
				return X * Y * Z;
			}
		}

		public Vec3 Absolute
		{
			get
			{
				return new Vec3(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));
			}
		}

		public Vec3 Flipped
		{
			get
			{
				return new Vec3(-X, -Y, -Z);
			}
		}

		/// <summary>
		/// Checks whether the vector is valid.
		/// </summary>
		/// <returns></returns>
		public bool IsValid
		{
			get
			{
				return MathHelpers.IsNumberValid(X) && MathHelpers.IsNumberValid(Y) && MathHelpers.IsNumberValid(Z);
			}
		}

		#region Operators
		public static Vec3 operator *(Vec3 v, float scale)
		{
			return new Vec3(v.X * scale, v.Y * scale, v.Z * scale);
		}

		public static Vec3 operator *(float scale, Vec3 v)
		{
			return v * scale;
		}

		public static Vec3 operator /(Vec3 v, float scale)
		{
			scale = 1.0f / scale;

			return new Vec3(v.X * scale, v.Y * scale, v.Z * scale);
		}

		public static Vec3 operator +(Vec3 v0, Vec3 v1)
		{
			return new Vec3(v0.X + v1.X, v0.Y + v1.Y, v0.Z + v1.Z);
		}

		public static Vec3 operator +(Vec2 v0, Vec3 v1)
		{
			return new Vec3(v0.X + v1.X, v0.Y + v1.Y, v1.Z);
		}

		public static Vec3 operator +(Vec3 v0, Vec2 v1)
		{
			return new Vec3(v0.X + v1.X, v0.Y + v1.Y, v0.Z);
		}

		public static Vec3 operator -(Vec3 v0, Vec3 v1)
		{
			return new Vec3(v0.X - v1.X, v0.Y - v1.Y, v0.Z - v1.Z);
		}

		public static Vec3 operator -(Vec2 v0, Vec3 v1)
		{
			return new Vec3(v0.X - v1.X, v0.Y - v1.Y, 0 - v1.Z);
		}

		public static Vec3 operator -(Vec3 v0, Vec2 v1)
		{
			return new Vec3(v0.X - v1.X, v0.Y - v1.Y, v0.Z);
		}

		public static float operator *(Vec3 v0, Vec3 v1)
		{
			return v0.Dot(v1);
		}

		public static float operator |(Vec3 v0, Vec3 v1)
		{
			return v0.Dot(v1);
		}

		public static Vec3 operator ^(Vec3 v0, Vec3 v1)
		{
			return v0.Cross(v1);
		}

		public static Vec3 operator %(Vec3 v0, Vec3 v1)
		{
			return v0.Cross(v1);
		}

		public static Vec3 operator -(Vec3 v)
		{
			return v.Flipped;
		}

		public static Vec3 operator !(Vec3 v)
		{
			return v.Flipped;
		}

		public static bool operator ==(Vec3 left, Vec3 right)
		{
			return ((left.X == right.X) && (left.Y == right.Y) && (left.Z == right.Z));
		}

		public static bool operator !=(Vec3 left, Vec3 right)
		{
			return !(left == right);
		}
		#endregion

		#region Overrides
		public override int GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;

				hash = hash * 23 + X.GetHashCode();
				hash = hash * 23 + Y.GetHashCode();
				hash = hash * 23 + Z.GetHashCode();

				return hash;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			if (obj is Vec3)
				return this == (Vec3)obj;

			return false;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{0},{1},{2}", X, Y, Z);
		}
		#endregion

		public static Vec3 Parse(string value)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (value == null)
				throw new ArgumentNullException("value");
			if (value.Length < 1)
				throw new ArgumentException("value string was empty");
#endif

			string[] split = value.Split(',');

#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (split.Length != 3)
				throw new ArgumentException("value string was invalid");
#endif

			return new Vec3(System.Convert.ToSingle(split[0]), System.Convert.ToSingle(split[1]), System.Convert.ToSingle(split[2]));
		}

		public static Vec3 CreateProjection(Vec3 i, Vec3 n)
		{
			return i - n * (n | i);
		}

		public static Vec3 CreateReflection(Vec3 i, Vec3 n)
		{
			return (n * (i | n) * 2) - i;
		}

		public static Vec3 CreateLerp(Vec3 p, Vec3 q, float t)
		{
			Vec3 diff = q - p;
			return p + (diff * t);
		}

		public static Vec3 CreateSlerp(Vec3 p, Vec3 q, float t)
		{
			var v = new Vec3();
			v.SetSlerp(p, q, t);

			return v;
		}

		public static float Dot(Vec3 v0, Vec3 v1)
		{
			return v0.Dot(v1);
		}

		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <param name="result">When the method completes, contains the clamped value.</param>
		public static void Clamp(ref Vec3 value, ref Vec3 min, ref Vec3 max, out Vec3 result)
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

			result = new Vec3(x, y, z);
		}

		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <returns>The clamped value.</returns>
		public static Vec3 Clamp(Vec3 value, Vec3 min, Vec3 max)
		{
			Vec3 result;
			Clamp(ref value, ref min, ref max, out result);
			return result;
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
		public static void Max(ref Vec3 left, ref Vec3 right, out Vec3 result)
		{
			result.X = (left.X > right.X) ? left.X : right.X;
			result.Y = (left.Y > right.Y) ? left.Y : right.Y;
			result.Z = (left.Z > right.Z) ? left.Z : right.Z;
		}

		/// <summary>
		/// Returns a vector containing the largest components of the specified vectors.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>A vector containing the largest components of the source vectors.</returns>
		public static Vec3 Max(Vec3 left, Vec3 right)
		{
			Vec3 result;
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
		public static void Min(ref Vec3 left, ref Vec3 right, out Vec3 result)
		{
			result.X = (left.X < right.X) ? left.X : right.X;
			result.Y = (left.Y < right.Y) ? left.Y : right.Y;
			result.Z = (left.Z < right.Z) ? left.Z : right.Z;
		}

		/// <summary>
		/// Returns a vector containing the smallest components of the specified vectors.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>A vector containing the smallest components of the source vectors.</returns>
		public static Vec3 Min(Vec3 left, Vec3 right)
		{
			Vec3 result;
			Min(ref left, ref right, out result);
			return result;
		}
	}
}