using System;
using System.Collections.Generic;
using System.Linq;

using CryEngine.Initialization;

namespace CryEngine.Extensions
{
	/// <summary>
	/// Defines extension methods for enumerations.
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// Gets enumeration of members of the enumeration.
		/// </summary>
		/// <typeparam name="T">Type of enumeration.</typeparam>
		/// <returns>A list of values of the enumeration.</returns>
		public static IEnumerable<T> GetMembers<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<T>();
		}
	}
}