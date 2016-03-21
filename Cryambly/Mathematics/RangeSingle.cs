using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CryCil
{
	/// <summary>
	/// Represents a range of values expressed with objects of type <see cref="float"/>.
	/// </summary>
	public struct RangeSingle : IEquatable<RangeSingle>
	{
		#region Fields
		private float start;
		private float end;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the length of this range.
		/// </summary>
		public float Length => this.end - this.start;
		/// <summary>
		/// Indicates whether this range is empty.
		/// </summary>
		/// <remarks>Length of empty range is zero.</remarks>
		public bool IsEmpty => Math.Abs(this.end - this.start) < MathHelpers.ZeroTolerance;
		/// <summary>
		/// Gets the starting value of the range.
		/// </summary>
		public float Start => this.start;
		/// <summary>
		/// Gets the ending value of the range.
		/// </summary>
		public float End => this.end;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="start">Left side of the range.</param>
		/// <param name="end">  Right side of the range.</param>
		/// <exception cref="ArgumentException">
		/// Starting value of the range must not be greater then the ending value.
		/// </exception>
		public RangeSingle(float start, float end)
		{
			this.start = start;
			this.end = end;
			CheckTheRange(start, end);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Assigns new range.
		/// </summary>
		/// <param name="start">New left-most value of the range.</param>
		/// <param name="end">  New right-most value of the range.</param>
		/// <exception cref="ArgumentException">
		/// Starting value of the range must not be greater then the ending value.
		/// </exception>
		public void Set(float start, float end)
		{
			CheckTheRange(start, end);

			this.start = start;
			this.end = end;
		}
		/// <summary>
		/// Determines whether given value is inside this range.
		/// </summary>
		/// <param name="value">Value to check.</param>
		/// <returns>
		/// True, if given value not less then <see cref="start"/> and not greater then <see cref="end"/>.
		/// </returns>
		public bool HasInside(float value)
		{
			return value >= this.start && value <= this.end;
		}
		/// <summary>
		/// Clamps this value into this range.
		/// </summary>
		/// <param name="value">Value to clamp into this range.</param>
		/// <returns>
		/// A value that is either a given value (if its within this range) or the border of this range the
		/// value is closest to.
		/// </returns>
		public float Clamp(float value)
		{
			return
				value < this.start
					? this.start
					: value > this.end
						? this.end
						: value;
		}
		/// <summary>
		/// Clamps given value.
		/// </summary>
		/// <param name="value">Reference to the value to clamp.</param>
		public void Clamp(ref float value)
		{
			if (value < this.Start)
			{
				value = this.Start;
				return;
			}
			if (value > this.End)
			{
				value = this.End;
			}
		}
		/// <summary>
		/// Determines whether 2 ranges are equal.
		/// </summary>
		/// <param name="other">Another range.</param>
		/// <returns>True, if ranges are equal.</returns>
		public bool Equals(RangeSingle other)
		{
			return this.start.Equals(other.start) && this.end.Equals(other.end);
		}
		/// <summary>
		/// Dtermines whether given object is equal to this one.
		/// </summary>
		/// <param name="obj">Another object.</param>
		/// <returns>True, if given object is of this type and is equal to this one.</returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is RangeSingle && this.Equals((RangeSingle)obj);
		}
		/// <summary>
		/// Gets hash code of this object.
		/// </summary>
		/// <returns>Hash code of this object.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return (this.start.GetHashCode() * 397) ^ this.end.GetHashCode();
			}
		}
		#endregion
		#region Operators
		/// <summary>
		/// Determines whether 2 objects are equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if 2 objects are equal.</returns>
		public static bool operator ==(RangeSingle left, RangeSingle right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Determines whether 2 objects are not equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if 2 objects are not equal.</returns>
		public static bool operator !=(RangeSingle left, RangeSingle right)
		{
			return !left.Equals(right);
		}
		/// <summary>
		/// Creates an intersection of 2 ranges.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>
		/// A range that contains common parts of given ranges if they intersect, otherwise returns an empty
		/// range.
		/// </returns>
		public static RangeSingle operator &(RangeSingle left, RangeSingle right)
		{
			if (left.Start > right.End || right.Start > left.End)
			{
				// Ranges don't intersect.
				return new RangeSingle();
			}
			return new RangeSingle(Math.Max(left.start, right.start), Math.Min(left.end, right.end));
		}
		/// <summary>
		/// Creates a combination of 2 ranges.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>
		/// A new range that includes all values within given ranges and all values between them, if they
		/// don't intersect.
		/// </returns>
		public static RangeSingle operator |(RangeSingle left, RangeSingle right)
		{
			return new RangeSingle(Math.Min(left.start, right.start), Math.Max(left.end, right.end));
		}
		/// <summary>
		/// Creates a range that is an expanded version of given one.
		/// </summary>
		/// <param name="range">Left operand.</param>
		/// <param name="value">Right operand.</param>
		/// <returns>A new range that is a given range that is expanded to fit given value.</returns>
		public static RangeSingle operator +(RangeSingle range, float value)
		{
			float s = range.start, e = range.end;
			if (value < s)
			{
				s = value;
			}
			if (value > e)
			{
				e = value;
			}
			return new RangeSingle(s, e);
		}
		/// <summary>
		/// Creates a range that is a shrunk version of given one.
		/// </summary>
		/// <param name="range">Left operand.</param>
		/// <param name="value">Right operand.</param>
		/// <returns>
		/// A new range that is a given range where border that is closest to the given value has be moved
		/// to that value.
		/// </returns>
		public static RangeSingle operator -(RangeSingle range, float value)
		{
			float s = range.start, e = range.end;
			if (value > s)
			{
				s = value;
			}
			if (value < e)
			{
				e = value;
			}
			return new RangeSingle(s, e);
		}
		#endregion
		#region Utilities
		/// <exception cref="ArgumentException">
		/// Starting value of the range must not be greater then the ending value.
		/// </exception>
		[SuppressMessage("ReSharper", "UnusedParameter.Local", Justification = "Reviewed")]
		private static void CheckTheRange(float start, float end)
		{
			if (start > end)
			{
				throw new ArgumentException("Starting value of the range must not be greater then the ending value.");
			}
		}
		#endregion
	}
}