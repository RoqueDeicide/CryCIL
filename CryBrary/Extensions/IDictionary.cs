using System;
using System.Collections.Generic;
using System.Linq;

namespace CryEngine.Extensions
{
	/// <summary>
	/// Defines extension methods for <see cref="IDictionary{TKey, TValue}"/> .
	/// </summary>
	public static class DictionaryExtensions
	{
		/// <summary>
		/// Tries to key associated with given value, if it exists in the dictionary.
		/// </summary>
		/// <typeparam name="TKey">Type of key.</typeparam>
		/// <typeparam name="TValue">Type of values.</typeparam>
		/// <param name="dictionary">Dictionary object.</param>
		/// <param name="value">     Values which to try to get.</param>
		/// <param name="key">       Resultant key.</param>
		/// <returns>True, if successful.</returns>
		public static bool TryGetKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value, out TKey key)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (dictionary == null)
				throw new ArgumentNullException("dictionary");
#endif
			key = dictionary.FirstOrDefault(x => x.Value.Equals(value)).Key;

			return key.Equals(default(TKey));
		}
	}
}