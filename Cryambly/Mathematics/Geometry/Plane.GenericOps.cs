using System;
using System.Globalization;

namespace CryCil.Geometry
{
	public partial struct Plane
	{
		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left"> The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="left"/> has the same value as
		/// <paramref name="right"/>; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator ==(Plane left, Plane right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left"> The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="left"/> has a different value than
		/// <paramref name="right"/>; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=(Plane left, Plane right)
		{
			return !left.Equals(right);
		}
		/// <summary>
		/// Creates an array containing the elements of the plane.
		/// </summary>
		/// <returns>
		/// A four-element array containing the components of the plane.
		/// </returns>
		public float[] ToArray()
		{
			return new[] { this.Normal.X, this.Normal.Y, this.Normal.Z, this.D };
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "A:{0} B:{1} C:{2} D:{3}", this.Normal.X, this.Normal.Y, this.Normal.Z, this.D);
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public string ToString(string format)
		{
			return string.Format(CultureInfo.CurrentCulture, "A:{0} B:{1} C:{2} D:{3}", this.Normal.X.ToString(format, CultureInfo.CurrentCulture), this.Normal.Y.ToString(format, CultureInfo.CurrentCulture), this.Normal.Z.ToString(format, CultureInfo.CurrentCulture), this.D.ToString(format, CultureInfo.CurrentCulture));
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public string ToString(IFormatProvider formatProvider)
		{
			return string.Format(formatProvider, "A:{0} B:{1} C:{2} D:{3}", this.Normal.X, this.Normal.Y, this.Normal.Z, this.D);
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="format">        The format.</param>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format(formatProvider, "A:{0} B:{1} C:{2} D:{3}", this.Normal.X.ToString(format, formatProvider), this.Normal.Y.ToString(format, formatProvider), this.Normal.Z.ToString(format, formatProvider), this.D.ToString(format, formatProvider));
		}
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
				hash = hash * 29 + this.Normal.GetHashCode();
				hash = hash * 29 + this.D.GetHashCode();
				// ReSharper restore NonReadonlyFieldInGetHashCode

				return hash;
			}
		}
		/// <summary>
		/// Determines whether the specified <see cref="Vector4"/> is equal to this
		/// instance.
		/// </summary>
		/// <param name="value">
		/// The <see cref="Vector4"/> to compare with this instance.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Vector4"/> is equal to this instance;
		/// otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(Plane value)
		{
			return this.Normal == value.Normal && Math.Abs(this.D - value.D) < MathHelpers.ZeroTolerance;
		}
		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this
		/// instance.
		/// </summary>
		/// <param name="value">
		/// The <see cref="System.Object"/> to compare with this instance.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to this
		/// instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object value)
		{
			return value != null && value.GetType() == this.GetType() && this.Equals((Plane)value);
		}
	}
}