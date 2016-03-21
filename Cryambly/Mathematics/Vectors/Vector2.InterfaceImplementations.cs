using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CryCil
{
	public partial struct Vector2
	{
		#region IVector
		/// <summary>
		/// Creates deep copy of this vector.
		/// </summary>
		Vector2 IVector<float, Vector2>.DeepCopy => this;
		/// <summary>
		/// Calculates the dot product of this vector and another one.
		/// </summary>
		/// <param name="other">Another vector.</param>
		/// <returns>Dot product of the 2 vectors.</returns>
		float IVector<float, Vector2>.Dot(Vector2 other)
		{
			return this * other;
		}
		/// <summary>
		/// Adds components from another vector to respective components of this one.
		/// </summary>
		/// <param name="other">Another vector.</param>
		void IVector<float, Vector2>.Add(Vector2 other)
		{
			this.X += other.X;
			this.Y += other.Y;
		}
		/// <summary>
		/// Multiplies components of this vector by the given factor.
		/// </summary>
		/// <param name="factor">Scaling factor.</param>
		void IVector<float, Vector2>.Scale(float factor)
		{
			this.X *= factor;
			this.Y *= factor;
		}
		/// <summary>
		/// Creates a new vector that is a sum of this one and another one.
		/// </summary>
		/// <param name="other">Another vector.</param>
		/// <returns>Sum of two vectors.</returns>
		Vector2 IVector<float, Vector2>.Added(Vector2 other)
		{
			return this + other;
		}
		/// <summary>
		/// Creates a new vector that is a scaled version of this one.
		/// </summary>
		/// <param name="factor">Scaling factor.</param>
		/// <returns>Scaled vector.</returns>
		Vector2 IVector<float, Vector2>.Scaled(float factor)
		{
			return this * factor;
		}
		#endregion
		#region IEnumerable<float>
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
		#region IEquatable<Vector2>
		/// <summary>
		/// Determines whether the specified <see cref="Vector2"/> is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="Vector2"/> to compare with this instance.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Vector2"/> is equal to this instance; otherwise,
		/// <c>false</c> .
		/// </returns>
		public bool Equals(Vector2 other)
		{
			// ReSharper disable CompareOfFloatsByEqualityOperator
			return this.X == other.X && this.Y == other.Y;
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}
		/// <summary>
		/// Determines whether the specified <see cref="Vector2"/> is equal to this instance using an
		/// epsilon value.
		/// </summary>
		/// <param name="other">  The <see cref="Vector2"/> to compare with this instance.</param>
		/// <param name="epsilon">The amount of error allowed.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Vector2"/> is equal to this instance; otherwise,
		/// <c>false</c> .
		/// </returns>
		public bool Equals(Vector2 other, float epsilon)
		{
			return Math.Abs(other.X - this.X) < epsilon &&
				   Math.Abs(other.Y - this.Y) < epsilon;
		}
		#endregion
		#region IComparable<Vector2>
		/// <summary>
		/// Determines position of this vector relative to another in a sorted sequence.
		/// </summary>
		/// <param name="other">Another vector.</param>
		/// <returns>
		/// <para><c>1</c> - Means that this vector should succeed another.</para>
		/// <para><c>0</c> - Means that both vectors are equal.</para>
		/// <para><c>-1</c> - Means that this vector should preceed another.</para>
		/// </returns>
		public int CompareTo(Vector2 other)
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
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a
		/// hash table.
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
		/// Determines whether the specified <see cref="object"/> is equal to this instance.
		/// </summary>
		/// <param name="value">The <see cref="object"/> to compare with this instance.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise,
		/// <c>false</c> .
		/// </returns>
		public override bool Equals(object value)
		{
			if (value is Vector2 && this.Equals((Vector2)value)) return true;
			Vector2? nullableVector = value as Vector2?;
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
					: string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1}", this.X.ToString(format, CultureInfo.CurrentCulture),
									this.Y.ToString(format, CultureInfo.CurrentCulture));
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

			return string.Format(formatProvider, "X:{0} Y:{1}", this.X.ToString(format, formatProvider),
								 this.Y.ToString(format, formatProvider));
		}
		#endregion
	}
}