using System;
using CryCil.Annotations;

namespace CryCil
{
	/// <summary>
	/// Defines extension methods for numbers.
	/// </summary>
	public static class NumberExtensions
	{
		/// <summary>
		/// Creates English ordinal number.
		/// </summary>
		/// <param name="number">A number to convert into ordinal form.</param>
		/// <returns>A text representation of the ordinal number.</returns>
		[Pure]
		public static string ToOrdinal(this int number)
		{
			if (number <= 0) return number.ToString();

			switch (number % 100)
			{
				case 11:
				case 12:
				case 13:
					return number + "th";
			}

			switch (number % 10)
			{
				case 1:
					return number + "st";
				case 2:
					return number + "nd";
				case 3:
					return number + "rd";
				default:
					return number + "th";
			}
		}
		/// <summary>
		/// Gets a value that is clamped into specified range.
		/// </summary>
		/// <typeparam name="ValType">Type of the value to clamp.</typeparam>
		/// <param name="instance">A value to clamp.</param>
		/// <param name="min">     Minimal value in the range to clamp the value into.</param>
		/// <param name="max">     Maximal value in the range to clamp the value into.</param>
		/// <returns>
		/// <para><paramref name="instance"/> if it is within the range;</para>
		/// <para>
		/// <paramref name="min"/>, if <paramref name="instance"/> is less then <paramref name="min"/>;
		/// </para>
		/// <para>
		/// <paramref name="max"/>, if <paramref name="instance"/> is greater then <paramref name="max"/>.
		/// </para>
		/// </returns>
		/// <exception cref="ArgumentException">Minimal value must be less then maximal value.</exception>
		[Pure]
		public static ValType GetClamped<ValType>(this ValType instance, ValType min, ValType max)
			where ValType : IComparable<ValType>
		{
			int minToMax = min.CompareTo(max);
			if (minToMax == 0)
			{
				return min;
			}
			if (minToMax > 0)
			{
				throw new ArgumentException("Minimal value must be less then maximal value.");
			}
			if (instance.CompareTo(min) < 0)
			{
				return min;
			}
			return instance.CompareTo(max) > 0 ? max : instance;
		}
	}
}