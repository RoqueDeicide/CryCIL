using System;
using System.Collections.Generic;
using System.Linq;

namespace CryCil
{
	/// <summary>
	/// Represents a rational number in a form of numerator and denominator.
	/// </summary>
	public struct rational : IEquatable<rational>, IComparable<rational>
	{
		#region Static Fields
		private static readonly IEqualityComparer<rational> fractionComparerInstance =
			new FractionEqualityComparer();
		#endregion
		#region Fields
		// Numerator.
		private int num;
		// Denominator.
		private uint den;
		#endregion
		#region Static Properties
		/// <summary>
		/// Gets an object that can be used as an equality comparer for rational numbers.
		/// </summary>
		public static IEqualityComparer<rational> FractionComparer => fractionComparerInstance;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the number above the fraction.
		/// </summary>
		public int Numerator => this.num;
		/// <summary>
		/// Gets the number below the fraction.
		/// </summary>
		public uint Denominator => this.den;
		/// <summary>
		/// Gets the ratio between numerator and denominator.
		/// </summary>
		public double Ratio => this.num / (double)this.den;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new rational number.
		/// </summary>
		/// <param name="numerator">  A number to use as a numerator.</param>
		/// <param name="denominator">A number to use as a denominator.</param>
		/// <exception cref="DivideByZeroException">
		/// Attempted to create a rational number with denominator that is equal to zero.
		/// </exception>
		public rational(int numerator, uint denominator)
		{
			this.num = numerator;
			this.den = denominator;

			if (this.den == 0)
			{
				throw new DivideByZeroException("Attempted to create a rational number with denominator that is equal to zero.");
			}

			this.AbridgeFraction();
		}
		#endregion
		#region Interface
		#region Comparison
		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(rational other)
		{
			return this.num == other.num && this.den == other.den;
		}
		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <returns>
		/// true if <paramref name="obj"/> and this instance are the same type and represent the same value;
		/// otherwise, false.
		/// </returns>
		/// <param name="obj">Another object to compare to.</param>
		public override bool Equals(object obj)
		{
			return obj is rational && this.Equals((rational)obj);
		}
		/// <summary>
		/// Compares the current object with another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// <para>
		/// A value that indicates the relative order of the objects being compared. The return value has
		/// the following meanings:
		/// </para>
		/// <para>-1 - this object is less then another one.</para>
		/// <para>0 - this object is equal to another one.</para>
		/// <para>1 - this object is greater then another one.</para>
		/// </returns>
		public int CompareTo(rational other)
		{
			if (this.num < 0 && other.num >= 0)
			{
				return -1;
			}
			if (other.num < 0 && this.num >= 0)
			{
				return 1;
			}
			return (this.num * other.den).CompareTo(other.num * this.den);
		}
		#endregion
		#region Hashing
		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return (this.num * 397) ^ (int)this.den;
			}
		}
		#endregion
		#endregion
		#region Operators
		#region Comparison
		/// <summary>
		/// Determines whether 2 objects are not equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if 2 operands are not equal.</returns>
		public static bool operator ==(rational left, rational right)
		{
			return left.num == right.num && left.den == right.den;
		}
		/// <summary>
		/// Determines whether 2 objects are equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if 2 operands are equal.</returns>
		public static bool operator !=(rational left, rational right)
		{
			return left.num != right.num || left.den != right.den;
		}
		/// <summary>
		/// Determines whether left operand is less then right operand.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if left operand is less then right operand.</returns>
		public static bool operator <(rational left, rational right)
		{
			return left.CompareTo(right) == -1;
		}
		/// <summary>
		/// Determines whether left operand is greater then right operand.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if left operand is greater then right operand.</returns>
		public static bool operator >(rational left, rational right)
		{
			return left.CompareTo(right) == 1;
		}
		#endregion
		#region Unary
		/// <summary>
		/// Negates the rational number.
		/// </summary>
		/// <param name="value">A rational number to negate.</param>
		/// <returns>A negated number.</returns>
		public static rational operator -(rational value)
		{
			return new rational(-value.num, value.den);
		}
		#endregion
		#region Arithmetics
		/// <summary>
		/// Adds 2 rational numbers together.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Sum of 2 rational numbers.</returns>
		public static rational operator +(rational left, rational right)
		{
			rational result = new rational
			{
				den = LeastCommonFactor(left.den, right.den)
			};

			result.num = (int)(left.num * (result.den / left.den) + right.num * (result.den / right.den));
			result.AbridgeFraction();

			return result;
		}
		/// <summary>
		/// Adds 2 numbers together.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Sum of 2 numbers.</returns>
		public static rational operator +(rational left, int right)
		{
			return new rational((int)(left.num + right * left.den), left.den);
		}
		/// <summary>
		/// Adds 2 numbers together.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Sum of 2 numbers.</returns>
		public static rational operator +(int left, rational right)
		{
			return new rational((int)(right.num + left * right.den), right.den);
		}
		/// <summary>
		/// Subtracts rational number from another.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Difference between left and right rational numbers.</returns>
		public static rational operator -(rational left, rational right)
		{
			return left + -right;
		}
		/// <summary>
		/// Subtracts number from another.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Difference between left and right numbers.</returns>
		public static rational operator -(rational left, int right)
		{
			return left + -right;
		}
		/// <summary>
		/// Subtracts number from another.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Difference between left and right numbers.</returns>
		public static rational operator -(int left, rational right)
		{
			return left + -right;
		}
		/// <summary>
		/// Multiplies 2 numbers.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of multiplication</returns>
		public static rational operator *(rational left, rational right)
		{
			return new rational(left.num * right.num, left.den * right.den);
		}
		/// <summary>
		/// Multiplies 2 numbers.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of multiplication</returns>
		public static rational operator *(rational left, int right)
		{
			return new rational(left.num * right, left.den);
		}
		/// <summary>
		/// Multiplies 2 numbers.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of multiplication</returns>
		public static rational operator *(int left, rational right)
		{
			return new rational(left * right.num, right.den);
		}
		/// <summary>
		/// Multiplies 2 numbers.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of multiplication</returns>
		public static rational operator /(rational left, rational right)
		{
			return new rational((int)(left.num * right.den), (uint)(left.den * right.num));
		}
		/// <summary>
		/// Multiplies 2 numbers.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of multiplication</returns>
		public static rational operator /(rational left, uint right)
		{
			return new rational(left.num, left.den * right);
		}
		/// <summary>
		/// Multiplies 2 numbers.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of multiplication</returns>
		public static rational operator /(uint left, rational right)
		{
			return new rational(right.num, left * right.den);
		}
		#endregion
		#region Conversion
		/// <summary>
		/// Creates a rational number that is equal to given number.
		/// </summary>
		/// <param name="value">Given number.</param>
		/// <returns>A rational number is equal to given number.</returns>
		public static implicit operator rational(sbyte value)
		{
			return new rational(value, 1);
		}
		/// <summary>
		/// Creates a rational number that is equal to given number.
		/// </summary>
		/// <param name="value">Given number.</param>
		/// <returns>A rational number is equal to given number.</returns>
		public static implicit operator rational(byte value)
		{
			return new rational(value, 1);
		}
		/// <summary>
		/// Creates a rational number that is equal to given number.
		/// </summary>
		/// <param name="value">Given number.</param>
		/// <returns>A rational number is equal to given number.</returns>
		public static implicit operator rational(short value)
		{
			return new rational(value, 1);
		}
		/// <summary>
		/// Creates a rational number that is equal to given number.
		/// </summary>
		/// <param name="value">Given number.</param>
		/// <returns>A rational number is equal to given number.</returns>
		public static implicit operator rational(ushort value)
		{
			return new rational(value, 1);
		}
		/// <summary>
		/// Creates a rational number that is equal to given number.
		/// </summary>
		/// <param name="value">Given number.</param>
		/// <returns>A rational number is equal to given number.</returns>
		public static implicit operator rational(int value)
		{
			return new rational(value, 1);
		}
		/// <summary>
		/// Creates a rational number that is equal to given number.
		/// </summary>
		/// <param name="value">Given number.</param>
		/// <returns>A rational number is equal to given number.</returns>
		public static implicit operator rational(uint value)
		{
			return new rational((int)value, 1);
		}
		/// <summary>
		/// Converts decimal fraction to normal fraction.
		/// </summary>
		/// <param name="value">Decimal fraction.</param>
		/// <returns>Normal fraction.</returns>
		public static implicit operator rational(float value)
		{
			int n = 1;
			uint d = 1;
			float fraction = 1;

			while (Math.Abs(value - fraction) > MathHelpers.ZeroTolerance)
			{
				if (fraction < value)
				{
					n++;
				}
				else
				{
					d++;
					n = (int)(value * d + 0.5f); // Simple rounding: add 0.5 and then truncate.
				}

				fraction = n / (float)d;
			}

			return new rational(n, d);
		}
		/// <summary>
		/// Creates an integer number from the fraction.
		/// </summary>
		/// <param name="value">Fraction to get the integer number from.</param>
		/// <returns>An integer number.</returns>
		public static explicit operator sbyte(rational value)
		{
			if (Math.Abs(value.num) < value.den)
			{
				return 0;
			}

			return (sbyte)(value.num / value.den);
		}
		/// <summary>
		/// Creates an integer number from the fraction.
		/// </summary>
		/// <param name="value">Fraction to get the integer number from.</param>
		/// <returns>An integer number.</returns>
		public static explicit operator byte(rational value)
		{
			if (Math.Abs(value.num) < value.den)
			{
				return 0;
			}

			return (byte)(value.num / value.den);
		}
		/// <summary>
		/// Creates an integer number from the fraction.
		/// </summary>
		/// <param name="value">Fraction to get the integer number from.</param>
		/// <returns>An integer number.</returns>
		public static explicit operator short(rational value)
		{
			if (Math.Abs(value.num) < value.den)
			{
				return 0;
			}

			return (short)(value.num / value.den);
		}
		/// <summary>
		/// Creates an integer number from the fraction.
		/// </summary>
		/// <param name="value">Fraction to get the integer number from.</param>
		/// <returns>An integer number.</returns>
		public static explicit operator ushort(rational value)
		{
			if (Math.Abs(value.num) < value.den)
			{
				return 0;
			}

			return (ushort)(value.num / value.den);
		}
		/// <summary>
		/// Creates an integer number from the fraction.
		/// </summary>
		/// <param name="value">Fraction to get the integer number from.</param>
		/// <returns>An integer number.</returns>
		public static explicit operator int(rational value)
		{
			if (Math.Abs(value.num) < value.den)
			{
				return 0;
			}

			return (int)(value.num / value.den);
		}
		/// <summary>
		/// Creates an integer number from the fraction.
		/// </summary>
		/// <param name="value">Fraction to get the integer number from.</param>
		/// <returns>An integer number.</returns>
		public static explicit operator uint(rational value)
		{
			if (Math.Abs(value.num) < value.den)
			{
				return 0;
			}

			return (uint)(value.num / value.den);
		}
		/// <summary>
		/// Creates a decimal fraction from a normal one.
		/// </summary>
		/// <param name="value">A normal fraction.</param>
		/// <returns>A decimal fraction.</returns>
		public static explicit operator float(rational value)
		{
			return value.num / (float)value.den;
		}
		#endregion
		#endregion
		#region Utilities
		private void AbridgeFraction()
		{
			if (this.den == 1 || this.num == 1)
			{
				return;
			}

			uint gcd = GreatesCommonDivisor((uint)Math.Abs(this.num), this.den);

			this.num /= (int)gcd;
			this.den /= gcd;
		}
		private static uint GreatesCommonDivisor(uint d1, uint d2)
		{
			if (d1 == 0)
			{
				return 0;
			}
			if (d2 == 0)
			{
				return 0;
			}

			// shift is a greatest power of 2 that can divide both numbers.
			int shift;
			for (shift = 0; ((d1 | d2) & 1) == 0; shift++)
			{
				d1 >>= 1;
				d2 >>= 1;
			}

			while ((d1 & 1) == 0)
			{
				d1 >>= 1;
			}

			// At this point d1 is odd.
			do
			{
				// Remove all factors of 2 - they are not common.
				while ((d2 & 1) == 0)
				{
					d2 >>= 1;
				}

				// At this point both numbers are odd. We need to make sure that d2 > d1 and subtract d1 out
				// of d2.
				if (d1 > d2)
				{
					uint t = d1;
					d1 = d2;
					d2 = t;
				}

				d2 -= d1;
			} while (d2 != 0);

			// Restore common factors of 2.
			return d1 << shift;
		}
		private static uint LeastCommonFactor(uint d1, uint d2)
		{
			d1 /= GreatesCommonDivisor(d1, d2);

			return d1 * d2;
		}
		#endregion
		#region Nested Types
		private sealed class FractionEqualityComparer : IEqualityComparer<rational>
		{
			public bool Equals(rational x, rational y)
			{
				return x.num == y.num && x.den == y.den;
			}
			public int GetHashCode(rational obj)
			{
				unchecked
				{
					return (obj.num * 397) ^ (int)obj.den;
				}
			}
		}
		#endregion
	}
}