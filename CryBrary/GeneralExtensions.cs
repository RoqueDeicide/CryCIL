using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
	/// <summary>
	/// Defines some miscellaneous extension methods.
	/// </summary>
	public static class GeneralExtensions
	{
		/// <summary>
		/// Indicates whether this collection is null or is empty.
		/// </summary>
		/// <typeparam name="T">Type of elements in the collection.</typeparam>
		/// <param name="collection">Collection itself.</param>
		/// <returns>
		/// True, if <paramref name="collection"/> is null or number its elements is equal to zero.
		/// </returns>
		public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
		{
			return collection == null || collection.Count == 0;
		}
		/// <summary>
		/// Indicates whether this collection is null or is too small.
		/// </summary>
		/// <typeparam name="T">Type of elements in the collection.</typeparam>
		/// <param name="collection">  Collection itself.</param>
		/// <param name="minimalCount">Minimal number of elements that must be inside the collection.</param>
		/// <returns>
		/// True, if <paramref name="collection"/> is null, or it contains smaller number elements
		/// then one defined by <paramref name="minimalCount"/> .
		/// </returns>
		public static bool IsNullOrTooSmall<T>(this ICollection<T> collection, int minimalCount)
		{
			return collection == null || collection.Count < minimalCount;
		}
		/// <summary>
		/// Finds zero-based indexes of all occurrences of given substring in the text.
		/// </summary>
		/// <param name="text">Text to look for substrings in.</param>
		/// <param name="substring">Piece of text to look for.</param>
		/// <param name="options">Text comparison options.</param>
		/// <returns>A list of all indexes.</returns>
		public static List<int> AllIndexesOf(this string text, string substring, StringComparison options)
		{
			if (String.IsNullOrEmpty(text))
			{
				throw new ArgumentException("Cannot perform search in the empty string.");
			}
			if (String.IsNullOrEmpty(substring))
			{
				throw new ArgumentException("Cannot perform search for an empty string.");
			}
			List<int> indexes = new List<int>(text.Length / substring.Length);
			for (int i = text.IndexOf(substring, options); i != -1;)
			{
				indexes.Add(i);
				i = text.IndexOf(substring, i + substring.Length, options);
			}
			return indexes;
		}
		/// <summary>
		/// Finds zero-based indexes of all occurrences of given substring in the text using the invariant culture.
		/// </summary>
		/// <param name="text">Text to look for substrings in.</param>
		/// <param name="substring">Piece of text to look for.</param>
		/// <returns>A list of all indexes.</returns>
		public static List<int> AllIndexesOf(this string text, string substring)
		{
			return AllIndexesOf(text, substring, StringComparison.InvariantCulture);
		}
		/// <summary>
		/// Gets file that contains the assembly.
		/// </summary>
		/// <param name="assembly">Assembly.</param>
		/// <returns>Full path to the .dll file.</returns>
		public static string GetLocation(this Assembly assembly)
		{
			return Uri.UnescapeDataString(new UriBuilder(assembly.CodeBase).Path);
		}
	}
}