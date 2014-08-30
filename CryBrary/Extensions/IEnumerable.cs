using System;
using System.Collections.Generic;

namespace CryEngine.Extensions
{
	/// <summary>
	/// Defines extension methods for <see cref="IEnumerable{T}"/> .
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Invokes <paramref name="action"/> function with each element in the enumerable.
		/// </summary>
		/// <typeparam name="T">Type of elements in the collection.</typeparam>
		/// <param name="enumeration">Enumeration.</param>
		/// <param name="action">     Function to invoke with each element.</param>
		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach (T item in enumeration)
				action(item);
		}
	}
}