using System;
using System.Linq;

namespace CryCil
{
	/// <summary>
	/// Represents an object that keeps the state of the LCG (Linear Congruential Generator) randomization
	/// algorithm.
	/// </summary>
	/// <remarks>When using this object, you can advance its state by calling</remarks>
	public class LcgRandom
	{
		#region Fields
		internal ulong State;
		#endregion
		#region Properties
		/// <summary>
		/// Sets the seed for this object.
		/// </summary>
		public uint Seed
		{
			set { this.State = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes a default instance of this type.
		/// </summary>
		public LcgRandom()
		{
			this.State = 5489UL;
		}
		/// <summary>
		/// Initializes a new instance of this type.
		/// </summary>
		/// <param name="seed">Initial seed for this generator.</param>
		public LcgRandom(uint seed)
		{
			this.Seed = seed;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Advances the state of this PRNG.
		/// </summary>
		/// <returns>This object.</returns>
		public LcgRandom Next()
		{
			this.State = this.State * 214013 + 2531011;
			return this;
		}
		/// <summary>
		/// Advances the state of this PRNG and creates an unsigned 32-bit integer.
		/// </summary>
		/// <returns>A pseudo-random unsigned 32-bit integer.</returns>
		public uint NextUInt32()
		{
			this.Next();
			return (uint)(this.State >> 16);
		}
		/// <summary>
		/// Advances the state of this PRNG and creates an unsigned 32-bit integer that is less then
		/// provided value.
		/// </summary>
		/// <param name="max">Resultant value cannot be greater then this number.</param>
		/// <returns>A pseudo-random unsigned 32-bit integer.</returns>
		public uint NextUInt32(uint max)
		{
			uint r = this.NextUInt32();
			// Note that the computation below is biased. An alternative computation (also biased):
			// (uint)((ulong)r * ((ulong)max + 1)) >> 32)
			return (uint)(r % ((ulong)max + 1));
		}
		/// <summary>
		/// Advances the state of this PRNG and creates an unsigned 32-bit integer that is within provided
		/// range.
		/// </summary>
		/// <remarks>
		/// The order of <paramref name="first"/> and <paramref name="second"/> makes no difference.
		/// </remarks>
		/// <param name="first"> 
		/// First number that defines a range to put the pseudo-random number into.
		/// </param>
		/// <param name="second">
		/// Second number that defines a range to put the pseudo-random number into.
		/// </param>
		/// <returns>A pseudo-random unsigned 32-bit integer.</returns>
		public uint NextUInt32(uint first, uint second)
		{
			if (first > second)
			{
				MathHelpers.Swap(ref first, ref second);
			}
			return first + this.NextUInt32(second - first);
		}
		/// <summary>
		/// Advances the state of this PRNG and creates an unsigned 64-bit integer.
		/// </summary>
		/// <returns>A pseudo-random unsigned 64-bit integer.</returns>
		public ulong NextUInt64()
		{
			uint a = this.NextUInt32();
			uint b = this.NextUInt32();
			return ((ulong)b << 32) | a;
		}
		/// <summary>
		/// Advances the state of this PRNG and creates an unsigned 64-bit integer that is less then
		/// provided value.
		/// </summary>
		/// <param name="max">Resultant value cannot be greater then this number.</param>
		/// <returns>A pseudo-random unsigned 64-bit integer.</returns>
		public ulong NextUInt64(ulong max)
		{
			ulong r = this.NextUInt64();

			if (max == ulong.MaxValue)
			{
				return r;
			}

			// Note that the computation below is biased.
			return r % (max + 1);
		}
		/// <summary>
		/// Advances the state of this PRNG and creates an unsigned 64-bit integer that is within provided
		/// range.
		/// </summary>
		/// <remarks>
		/// The order of <paramref name="first"/> and <paramref name="second"/> makes no difference.
		/// </remarks>
		/// <param name="first"> 
		/// First number that defines a range to put the pseudo-random number into.
		/// </param>
		/// <param name="second">
		/// Second number that defines a range to put the pseudo-random number into.
		/// </param>
		/// <returns>A pseudo-random unsigned 64-bit integer.</returns>
		public ulong NextUInt64(ulong first, ulong second)
		{
			if (first > second)
			{
				MathHelpers.Swap(ref first, ref second);
			}
			return first + this.NextUInt64(second - first);
		}
		/// <summary>
		/// Advances the state of this PRNG and creates a single-precision floating point number.
		/// </summary>
		/// <returns>A pseudo-random single-precision floating point number.</returns>
		public float NextSingle()
		{
			return this.NextUInt32() / 4294967295.0f;
		}
		/// <summary>
		/// Advances the state of this PRNG and creates a single-precision floating point number that is
		/// less then provided value.
		/// </summary>
		/// <param name="max">Resultant value cannot be greater then this number.</param>
		/// <returns>A pseudo-random single-precision floating point number.</returns>
		public float NextSingle(float max)
		{
			return max * this.NextSingle();
		}
		/// <summary>
		/// Advances the state of this PRNG and creates a single-precision floating point number that is
		/// within provided range.
		/// </summary>
		/// <remarks>
		/// The order of <paramref name="first"/> and <paramref name="second"/> makes no difference.
		/// </remarks>
		/// <param name="first"> 
		/// First number that defines a range to put the pseudo-random number into.
		/// </param>
		/// <param name="second">
		/// Second number that defines a range to put the pseudo-random number into.
		/// </param>
		/// <returns>A pseudo-random single-precision floating point number.</returns>
		public float NextSingle(float first, float second)
		{
			return first + (second - first) * this.NextSingle();
		}
		/// <summary>
		/// Advances the state of this generator and gets next vector where each component is a
		/// pseudo-random number between 0 and 1.
		/// </summary>
		/// <returns>Pseudo-random vector.</returns>
		public Vector2 NextVector2()
		{
			return new Vector2(this.NextSingle(), this.NextSingle());
		}
		/// <summary>
		/// Advances the state of this generator and gets next vector where each component is a
		/// pseudo-random number between 0 and <paramref name="max"/>.
		/// </summary>
		/// <param name="max">Maximal value of each component of resultant vector.</param>
		/// <returns>Pseudo-random vector.</returns>
		public Vector2 NextVector2(float max)
		{
			return new Vector2(this.NextSingle(max), this.NextSingle(max));
		}
		/// <summary>
		/// Advances the state of this generator and gets next vector where each component is a
		/// pseudo-random number that is within the range that is defined by <paramref name="first"/> and
		/// <paramref name="second"/> numbers.
		/// </summary>
		/// <param name="first"> 
		/// First number that defines a range to put the pseudo-random numbers into.
		/// </param>
		/// <param name="second">
		/// Second number that defines a range to put the pseudo-random numbers into.
		/// </param>
		/// <returns>Pseudo-random vector.</returns>
		public Vector2 NextVector2(float first, float second)
		{
			return new Vector2(this.NextSingle(first, second), this.NextSingle(first, second));
		}
		/// <summary>
		/// Advances the state of this generator and gets next vector where each component is a
		/// pseudo-random number between 0 and 1.
		/// </summary>
		/// <returns>Pseudo-random vector.</returns>
		public Vector3 NextVector3()
		{
			return new Vector3(this.NextSingle(), this.NextSingle(), this.NextSingle());
		}
		/// <summary>
		/// Advances the state of this generator and gets next vector where each component is a
		/// pseudo-random number between 0 and <paramref name="max"/>.
		/// </summary>
		/// <param name="max">Maximal value of each component of resultant vector.</param>
		/// <returns>Pseudo-random vector.</returns>
		public Vector3 NextVector3(float max)
		{
			return new Vector3(this.NextSingle(max), this.NextSingle(max), this.NextSingle(max));
		}
		/// <summary>
		/// Advances the state of this generator and gets next vector where each component is a
		/// pseudo-random number that is within the range that is defined by <paramref name="first"/> and
		/// <paramref name="second"/> numbers.
		/// </summary>
		/// <param name="first"> 
		/// First number that defines a range to put the pseudo-random numbers into.
		/// </param>
		/// <param name="second">
		/// Second number that defines a range to put the pseudo-random numbers into.
		/// </param>
		/// <returns>Pseudo-random vector.</returns>
		public Vector3 NextVector3(float first, float second)
		{
			return new Vector3(this.NextSingle(first, second), this.NextSingle(first, second),
							   this.NextSingle(first, second));
		}
		/// <summary>
		/// Advances the state of this generator and gets next vector where each component is a
		/// pseudo-random number between 0 and 1.
		/// </summary>
		/// <returns>Pseudo-random vector.</returns>
		public Vector4 NextVector4()
		{
			return new Vector4(this.NextSingle(), this.NextSingle(), this.NextSingle(), this.NextSingle());
		}
		/// <summary>
		/// Advances the state of this generator and gets next vector where each component is a
		/// pseudo-random number between 0 and <paramref name="max"/>.
		/// </summary>
		/// <param name="max">Maximal value of each component of resultant vector.</param>
		/// <returns>Pseudo-random vector.</returns>
		public Vector4 NextVector4(float max)
		{
			return new Vector4(this.NextSingle(max), this.NextSingle(max), this.NextSingle(max),
							   this.NextSingle(max));
		}
		/// <summary>
		/// Advances the state of this generator and gets next vector where each component is a
		/// pseudo-random number that is within the range that is defined by <paramref name="first"/> and
		/// <paramref name="second"/> numbers.
		/// </summary>
		/// <param name="first"> 
		/// First number that defines a range to put the pseudo-random numbers into.
		/// </param>
		/// <param name="second">
		/// Second number that defines a range to put the pseudo-random numbers into.
		/// </param>
		/// <returns>Pseudo-random vector.</returns>
		public Vector4 NextVector4(float first, float second)
		{
			return new Vector4(this.NextSingle(first, second), this.NextSingle(first, second),
							   this.NextSingle(first, second), this.NextSingle(first, second));
		}
		/// <summary>
		/// Advances the state of this generator and gets next pseudo-random unit vector.
		/// </summary>
		/// <remarks>
		/// This function generates the vector within a square and normalizes it. This causes distribution
		/// to be more crowded in the "corners" of the circle.
		/// </remarks>
		/// <returns>Pseudo-random unit vector.</returns>
		public Vector2 NextUnitVector2Square()
		{
			Vector2 res;
			do
			{
				res = this.NextVector2(-1, 1);
			} while (res.LengthSquared < MathHelpers.ZeroTolerance); // Avoid the zero vector.

			res.Normalize();
			return res;
		}
		/// <summary>
		/// Advances the state of this generator and gets next pseudo-random unit vector.
		/// </summary>
		/// <remarks>
		/// This function generates the vector within a square, if it's outside of the circle, then it gets
		/// discarded, otherwise it is normalized. This creates the distribution that is slightly more
		/// uniform then with "Square" method.
		/// </remarks>
		/// <returns>Pseudo-random unit vector.</returns>
		public Vector2 NextUnitVector2SquareDiscard()
		{
			Vector2 result;
			float lengthSquared;

			do
			{
				do
				{
					result = this.NextVector2(-1, 1);
					lengthSquared = result.LengthSquared;
				} while (lengthSquared > 1);
			} while (lengthSquared < MathHelpers.ZeroTolerance); // Avoid the zero vector.

			return result;
		}
		/// <summary>
		/// Advances the state of this generator and gets next pseudo-random unit vector.
		/// </summary>
		/// <remarks>
		/// This function generates unit vectors that are uniformly distributed on the circle.
		/// </remarks>
		/// <returns>Pseudo-random unit vector.</returns>
		public Vector2 NextUnitVector2Circle()
		{
			float angle = this.NextSingle(0, (float)MathHelpers.PI2);
			float sine, cosine;
			MathHelpers.SinCos(angle, out sine, out cosine);

			return new Vector2(cosine, sine);
		}
		/// <summary>
		/// Advances the state of this generator and gets next pseudo-random unit vector.
		/// </summary>
		/// <remarks>
		/// This function generates the vector within a cube and normalizes it. This causes distribution to
		/// be more crowded in the "corners" of the sphere.
		/// </remarks>
		/// <returns>Pseudo-random unit vector.</returns>
		public Vector3 NextUnitVector3Cube()
		{
			Vector3 res;
			do
			{
				res = this.NextVector3(-1, 1);
			} while (res.LengthSquared < MathHelpers.ZeroTolerance); // Avoid the zero vector.

			res.Normalize();
			return res;
		}
		/// <summary>
		/// Advances the state of this generator and gets next pseudo-random unit vector.
		/// </summary>
		/// <remarks>
		/// This function generates the vector within a cube, if it's outside of the sphere, then it gets
		/// discarded, otherwise it is normalized. This creates the distribution that is slightly more
		/// uniform then with "Cube" method.
		/// </remarks>
		/// <returns>Pseudo-random unit vector.</returns>
		public Vector3 NextUnitVector3CubeDiscard()
		{
			Vector3 result;
			float lengthSquared;

			do
			{
				do
				{
					result = this.NextVector3(-1, 1);
					lengthSquared = result.LengthSquared;
				} while (lengthSquared > 1);
			} while (lengthSquared < MathHelpers.ZeroTolerance); // Avoid the zero vector.

			return result;
		}
		/// <summary>
		/// Advances the state of this generator and gets next pseudo-random unit vector.
		/// </summary>
		/// <remarks>
		/// This function generates unit vectors that are uniformly distributed on the sphere.
		/// </remarks>
		/// <returns>Pseudo-random unit vector.</returns>
		public Vector3 NextUnitVector3Sphere()
		{
			float phi = this.NextSingle(0, (float)MathHelpers.PI2);
			float theta = this.NextSingle(0, (float)MathHelpers.PI2);

			float phiSine, phiCosine, thetaSine, thetaCosine;
			MathHelpers.SinCos(phi, out phiSine, out phiCosine);
			MathHelpers.SinCos(theta, out thetaSine, out thetaCosine);

			return new Vector3(thetaSine * phiCosine, thetaSine * phiSine, thetaCosine);
		}
		/// <summary>
		/// Advances the state of this generator and gets next pseudo-random unit vector.
		/// </summary>
		/// <remarks>
		/// This function generates the vector within a hyper-cube and normalizes it. This causes
		/// distribution to be more crowded in the "corners" of the hyper-sphere.
		/// </remarks>
		/// <returns>Pseudo-random unit vector.</returns>
		public Vector4 NextUnitVector4HyperCube()
		{
			Vector4 res;
			do
			{
				res = this.NextVector4(-1, 1);
			} while (res.LengthSquared < MathHelpers.ZeroTolerance); // Avoid the zero vector.

			res.Normalize();
			return res;
		}
		/// <summary>
		/// Advances the state of this generator and gets next pseudo-random unit vector.
		/// </summary>
		/// <remarks>
		/// This function generates the vector within a hyper-cube, if it's outside of the hyper-sphere,
		/// then it gets discarded, otherwise it is normalized. This creates the distribution that is
		/// slightly more uniform then with "HyperCube" method.
		/// </remarks>
		/// <returns>Pseudo-random unit vector.</returns>
		public Vector4 NextUnitVector4HyperCubeDiscard()
		{
			Vector4 result;
			float lengthSquared;

			do
			{
				do
				{
					result = this.NextVector4(-1, 1);
					lengthSquared = result.LengthSquared;
				} while (lengthSquared > 1);
			} while (lengthSquared < MathHelpers.ZeroTolerance); // Avoid the zero vector.

			return result;
		}
		#endregion
	}
}