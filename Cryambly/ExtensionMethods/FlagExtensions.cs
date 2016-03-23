using System;
using CryCil.Utilities;

namespace CryCil
{
	/// <summary>
	/// Defines extension methods for working with flags.
	/// </summary>
	public static class FlagExtensions
	{
		/// <summary>
		/// Determines whether given value has any of specified bits set.
		/// </summary>
		/// <typeparam name="FlagsType">Type of the object. Has to be convertible to <see cref="ulong"/> for successful check.</typeparam>
		/// <param name="value">Value which bits to check.</param>
		/// <param name="bits">Bits to check for presence in the <paramref name="value"/>.</param>
		/// <returns>True, if any of the bits that are specified by <paramref name="bits"/> is set in the <paramref name="value"/>. If <paramref name="bits"/> is equal to 0 then true is returned if <paramref name="value"/> is also equal to 0.</returns>
		/// <exception cref="NotSupportedException"><typeparamref name="FlagsType"/> cannot be converted to ulong.</exception>
		public static bool HasAnyBit<FlagsType>(this FlagsType value, FlagsType bits)
		{
			try
			{
				ulong valueNumber = Cast<ulong>.From(value);
				ulong bitsNumber = Cast<ulong>.From(bits);

				if (bitsNumber == 0)
				{
					return valueNumber == 0;
				}

				return (valueNumber & bitsNumber) != 0;
			}
			catch (Exception)
			{
				throw new NotSupportedException($"{typeof(FlagsType).FullName} cannot be converted to ulong.");
			}
		}
		/// <summary>
		/// Determines whether specified value has a flag set.
		/// </summary>
		/// <typeparam name="FlagsType">Type of the object. Has to be convertible to <see cref="ulong"/> for successful check.</typeparam>
		/// <param name="value">Value to check.</param>
		/// <param name="flag">Flag to check for presence in the <paramref name="value"/>.</param>
		/// <returns>True, if <paramref name="value"/> has all bits that are specified <paramref name="flag"/> set.</returns>
		/// <exception cref="NotSupportedException"><typeparamref name="FlagsType"/> cannot be converted to ulong.</exception>
		public static bool FlagSet<FlagsType>(this FlagsType value, FlagsType flag)
		{
			try
			{
				ulong valueNumber = Cast<ulong>.From(value);
				ulong flagNumber = Cast<ulong>.From(flag);

				return (valueNumber & flagNumber) == flagNumber;
			}
			catch (Exception)
			{
				throw new NotSupportedException($"{typeof(FlagsType).FullName} cannot be converted to ulong.");
			}
		}
	}
}