using System;
using System.Collections.Generic;
using System.Globalization;

namespace CryCil
{
	public partial struct Vector2d
	{
		#region IEnumerable<double>
		/// <summary>
		/// Enumerates this vector.
		/// </summary>
		/// <returns>Yields components of this vector.</returns>
		public IEnumerator<double> GetEnumerator()
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
		#region IEquatable<Vector2d>
		/// <summary>
		/// Determines whether the specified <see cref="Vector2d"/> is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="Vector2d"/> to compare with this instance.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Vector2d"/> is equal to this instance; otherwise,
		/// <c>false</c> .
		/// </returns>
		public bool Equals(Vector2d other)
		{
			// ReSharper disable CompareOfFloatsByEqualityOperator
			return this.X == other.X && this.Y == other.Y;
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}
		/// <summary>
		/// Determines whether the specified <see cref="Vector2d"/> is equal to this instance using an
		/// epsilon value.
		/// </summary>
		/// <param name="other">  The <see cref="Vector2d"/> to compare with this instance.</param>
		/// <param name="epsilon">The amount of error allowed.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Vector2d"/> is equal to this instance; otherwise,
		/// <c>false</c> .
		/// </returns>
		public bool Equals(Vector2d other, double epsilon)
		{
			return (Math.Abs(other.X - this.X) < epsilon && Math.Abs(other.Y - this.Y) < epsilon);
		}
		#endregion
		#region IComparable<Vector2d>
		/// <summary>
		/// Determines position of this vector relative to another in a sorted sequence.
		/// </summary>
		/// <param name="other">Another vector.</param>
		/// <returns>
		/// <para><c>1</c> - Means that this vector should succeed another.</para>
		/// <para><c>0</c> - Means that both vectors are equal.</para>
		/// <para><c>-1</c> - Means that this vector should preceed another.</para>
		/// </returns>
		public int CompareTo(Vector2d other)
		{
			int pos = this.X.CompareTo(other.X);
			return pos != 0 ? pos : this.Y.CompareTo(other.Y);
		}
		#endregion
		#region System.Object Overrides
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

				return hash;
			}
		}
		/// <summary>
		/// Determines whether the specified <see cref="Object"/> is equal to this instance.
		/// </summary>
		/// <param name="value">The <see cref="Object"/> to compare with this instance.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Object"/> is equal to this instance; otherwise,
		/// <c>false</c> .
		/// </returns>
		public override bool Equals(object value)
		{
			if (value is Vector2d && this.Equals((Vector2d)value)) return true;
			Vector2d? nullableVector = value as Vector2d?;
			return nullableVector.HasValue && this.Equals(nullableVector.Value);
		}
		#endregion
		#region Text Conversions
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1}", this.X, this.Y);
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public string ToString(string format)
		{
			return
				format == null
				? this.ToString()
				: string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1}", this.X.ToString(format, CultureInfo.CurrentCulture), this.Y.ToString(format, CultureInfo.CurrentCulture));
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public string ToString(IFormatProvider formatProvider)
		{
			return string.Format(formatProvider, "X:{0} Y:{1}", this.X, this.Y);
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

			return string.Format(formatProvider, "X:{0} Y:{1}", this.X.ToString(format, formatProvider), this.Y.ToString(format, formatProvider));
		}
		#endregion
	}
}