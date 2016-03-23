using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryCil.Annotations;

namespace CryCil
{
	/// <summary>
	/// Defines extension methods for collections.
	/// </summary>
	public static class CollectionExtensions
	{
		/// <summary>
		/// Indicates whether this collection is null or is empty.
		/// </summary>
		/// <typeparam name="T">Type of elements in the collection.</typeparam>
		/// <param name="collection">Collection itself.</param>
		/// <returns>
		/// True, if <paramref name="collection"/> is null or number its elements is equal to zero.
		/// </returns>
		[Pure]
		[ContractAnnotation("collection:null => true")]
		public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
		{
			return collection == null || collection.Count == 0;
		}
		/// <summary>
		/// Indicates whether this collection is null or is too small.
		/// </summary>
		/// <typeparam name="T">Type of elements in the collection.</typeparam>
		/// <param name="collection">  Collection itself.</param>
		/// <param name="minimalCount">
		/// Minimal number of elements that must be inside the collection.
		/// </param>
		/// <returns>
		/// True, if <paramref name="collection"/> is null, or it contains smaller number elements then one
		/// defined by <paramref name="minimalCount"/> .
		/// </returns>
		[Pure]
		[ContractAnnotation("collection:null => true")]
		public static bool IsNullOrTooSmall<T>(this ICollection<T> collection, int minimalCount)
		{
			return collection == null || collection.Count < minimalCount;
		}
		/// <summary>
		/// Creates a string that is a list of text representation of all elements of the collection
		/// separated by a comma.
		/// </summary>
		/// <typeparam name="T">Type of elements in the collection.</typeparam>
		/// <param name="collection">Collection.</param>
		/// <returns>Text representation of the collection.</returns>
		[Pure]
		public static string ContentsToString<T>(this IEnumerable<T> collection)
		{
			return ContentsToString(collection, ",");
		}
		/// <summary>
		/// Creates a string that is a list of text representation of all elements of the collection
		/// separated by a comma.
		/// </summary>
		/// <typeparam name="T">Type of elements in the collection.</typeparam>
		/// <param name="collection">Collection.</param>
		/// <param name="separator"> Text to insert between elements.</param>
		/// <returns>Text representation of the collection.</returns>
		[Pure]
		public static string ContentsToString<T>(this IEnumerable<T> collection, string separator)
		{
			if (collection == null)
			{
				return "";
			}
			StringBuilder builder = new StringBuilder();
			IEnumerator<T> enumerator = collection.GetEnumerator();
			enumerator.Reset();
			enumerator.MoveNext();
			builder.Append(enumerator.Current);
			while (enumerator.MoveNext())
			{
				builder.Append(separator);
				builder.Append(enumerator.Current);
			}
			return builder.ToString();
		}
		/// <summary>
		/// Creates a string that is a list of text representation of all elements of the collection
		/// separated by a comma.
		/// </summary>
		/// <typeparam name="T">Type of elements in the collection.</typeparam>
		/// <param name="collection">Collection.</param>
		/// <param name="separator"> Character to insert between elements.</param>
		/// <returns>Text representation of the collection.</returns>
		[Pure]
		public static string ContentsToString<T>(this IEnumerable<T> collection, char separator)
		{
			return ContentsToString(collection, new string(separator, 1));
		}
		/// <summary>
		/// Tries to key associated with given value, if it exists in the dictionary.
		/// </summary>
		/// <typeparam name="Key">Type of key.</typeparam>
		/// <typeparam name="Element">Type of values.</typeparam>
		/// <param name="dictionary">Dictionary object.</param>
		/// <param name="value">     Values which to try to get.</param>
		/// <param name="key">       Resultant key.</param>
		/// <returns>
		/// True, if found key is not equal to default object of type <typeparamref name="Key"/>.
		/// </returns>
		[Pure]
		public static bool TryGetKey<Key, Element>([NotNull] this IDictionary<Key, Element> dictionary,
												   Element value, out Key key)
		{
			key = dictionary.FirstOrDefault(x => x.Value.Equals(value)).Key;

			return !key.Equals(default(Key));
		}
		/// <summary>
		/// Invokes <paramref name="action"/> function with each element in the enumerable.
		/// </summary>
		/// <typeparam name="T">Type of elements in the collection.</typeparam>
		/// <param name="enumeration">Enumeration.</param>
		/// <param name="action">     Function to invoke with each element.</param>
		public static void ForEach<T>([NotNull] this IEnumerable<T> enumeration, [NotNull] Action<T> action)
		{
			foreach (T item in enumeration)
				action(item);
		}
		/// <summary>
		/// Shifts a range of items within the list.
		/// </summary>
		/// <typeparam name="T">Type of objects within the list.</typeparam>
		/// <param name="list">        A list to shift elements within.</param>
		/// <param name="initialIndex">Zero-based index of the start of the range before shift.</param>
		/// <param name="desiredIndex">Zero-based index of the start of the range after shift.</param>
		/// <param name="rangeLength"> Number of elements in the range to move.</param>
		/// <exception cref="IndexOutOfRangeException">Initial index cannot be less then 0.</exception>
		/// <exception cref="IndexOutOfRangeException">Desired index cannot be less then 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Number of elements in the range must be greater then 0.
		/// </exception>
		/// <exception cref="Exception">Given list is not big enough.</exception>
		public static void Shift<T>([NotNull] this IList<T> list, int initialIndex, int desiredIndex,
									int rangeLength)
		{
			if (initialIndex < 0)
			{
				throw new IndexOutOfRangeException("Initial index cannot be less then 0.");
			}
			if (desiredIndex < 0)
			{
				throw new IndexOutOfRangeException("Desired index cannot be less then 0.");
			}
			if (rangeLength <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(rangeLength),
													  "Number of elements in the range must be greater " +
													  "then 0.");
			}
			int count = list.Count;
			if (desiredIndex > count - rangeLength)
			{
				throw new Exception("Given list is not big enough.");
			}

			if (initialIndex == desiredIndex)
			{
				return;
			}

			int offset = desiredIndex - initialIndex;
			if (offset > 0)
			{
				// Move right to left.
				for (int i = initialIndex + count - 1; i >= initialIndex; i--)
				{
					list[i + offset] = list[i];
				}
			}
			else if (offset < 0)
			{
				// Move left to right.
				for (int i = 0; i < count; i++)
				{
					list[desiredIndex + i] = list[initialIndex + i];
				}
			}
		}
	}
}