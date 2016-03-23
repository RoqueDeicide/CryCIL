using System;
using System.Collections.Generic;
using System.Linq;

namespace CryCil
{
	/// <summary>
	/// Defines extension methods for enumerations.
	/// </summary>
	public static class EnumExtas
	{
		/// <summary>
		/// Gets enumeration of members of the enumeration.
		/// </summary>
		/// <typeparam name="T">Type of enumeration.</typeparam>
		/// <returns>A list of values of the enumeration.</returns>
		public static IEnumerable<T> Values<T>()
			where T : struct, IComparable<T>, IConvertible
		{
			return EnumValuesCache<T>.EnumValues;
		}
		private static class EnumValuesCache<EnumType>
			where EnumType : struct, IComparable<EnumType>, IConvertible
		{
			public static readonly EnumType[] EnumValues;

			static EnumValuesCache()
			{
				var vals = Enum.GetValues(typeof(EnumType));

				EnumValues = new EnumType[vals.Length];

				Array.Copy(vals, EnumValues, vals.Length);
			}
		}
	}
}