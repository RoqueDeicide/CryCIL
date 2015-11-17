using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using CryCil.Annotations;

namespace CryCil
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
		[ContractAnnotation("collection:null => true")]
		public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
		{
			return collection == null || collection.Count == 0;
		}
		/// <summary>
		/// Determines whether given text is <c>null</c> or an empty <c>string</c>.
		/// </summary>
		/// <param name="text">This text.</param>
		/// <returns>True, if <paramref name="text"/> is equal to <c>null</c> or is a <c>string</c> with 0 characters in it.</returns>
		[Pure]
		[ContractAnnotation("text:null => true")]
		public static bool IsNullOrEmpty(this string text)
		{
			return string.IsNullOrEmpty(text);
		}
		/// <summary>
		/// Determines whether given text is <c>null</c> or an empty <c>string</c>.
		/// </summary>
		/// <param name="text">This text.</param>
		/// <returns>True, if <paramref name="text"/> is equal to <c>null</c> or is a <c>string</c> with 0 characters in it.</returns>
		[Pure]
		[ContractAnnotation("text:null => true")]
		public static bool IsNullOrWhiteSpace(this string text)
		{
			return string.IsNullOrWhiteSpace(text);
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
		public static string ContentsToString<T>(this IEnumerable<T> collection, char separator)
		{
			return ContentsToString(collection, new string(separator, 1));
		}
		/// <summary>
		/// Finds zero-based indexes of all occurrences of given substring in the text.
		/// </summary>
		/// <param name="text">     Text to look for substrings in.</param>
		/// <param name="substring">Piece of text to look for.</param>
		/// <param name="options">  Text comparison options.</param>
		/// <returns>A list of all indexes.</returns>
		public static List<int> AllIndexesOf([NotNull] this string text, string substring, StringComparison options)
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
		/// Finds zero-based indexes of all occurrences of given substring in the text using the invariant
		/// culture.
		/// </summary>
		/// <param name="text">     Text to look for substrings in.</param>
		/// <param name="substring">Piece of text to look for.</param>
		/// <returns>A list of all indexes.</returns>
		public static List<int> AllIndexesOf([NotNull] this string text, string substring)
		{
			return AllIndexesOf(text, substring, StringComparison.InvariantCulture);
		}
		/// <summary>
		/// Determines whether given text contains any of strings.
		/// </summary>
		/// <param name="text">   Given text.</param>
		/// <param name="strings">A list of strings to check for.</param>
		/// <returns>
		/// True, if <paramref name="text"/> is valid and <paramref name="strings"/> contains text snippets
		/// and at least one of those is in the <paramref name="text"/>, otherwise false.
		/// </returns>
		public static bool ContainsAny(this string text, string[] strings)
		{
			if (String.IsNullOrEmpty(text) || strings.IsNullOrEmpty())
			{
				return false;
			}
			return strings.Any(text.Contains);
		}
		/// <summary>
		/// Determines whether this string can be used as a name for flow node or a port.
		/// </summary>
		/// <param name="text">String.</param>
		/// <returns>True, if this string can be used as a name for flow node or a port.</returns>
		public static bool IsValidFlowGraphName(this string text)
		{
			return
				!(String.IsNullOrWhiteSpace(text)
				  || Regex.IsMatch(text, VariousConstants.InvalidXmlCharsPattern)
				  || text.Any(Char.IsWhiteSpace));
		}
		/// <summary>
		/// Gets file that contains the assembly.
		/// </summary>
		/// <param name="assembly">Assembly.</param>
		/// <returns>Full path to the .dll file.</returns>
		public static string GetLocation([NotNull] this Assembly assembly)
		{
			return Uri.UnescapeDataString(new UriBuilder(assembly.CodeBase).Path);
		}
		/// <summary>
		/// Tries to key associated with given value, if it exists in the dictionary.
		/// </summary>
		/// <typeparam name="TKey">Type of key.</typeparam>
		/// <typeparam name="TValue">Type of values.</typeparam>
		/// <param name="dictionary">Dictionary object.</param>
		/// <param name="value">     Values which to try to get.</param>
		/// <param name="key">       Resultant key.</param>
		/// <returns>True, if successful.</returns>
		public static bool TryGetKey<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary, TValue value,
												   out TKey key)
		{
			key = dictionary.FirstOrDefault(x => x.Value.Equals(value)).Key;

			return key.Equals(default(TKey));
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
		public static void Shift<T>([NotNull] this IList<T> list, int initialIndex, int desiredIndex, int rangeLength)
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
				throw new ArgumentOutOfRangeException("rangeLength", "Number of elements in the range must be greater then 0.");
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
		/// <summary>
		/// Tries to find one of the descendants with specified name.
		/// </summary>
		/// <param name="element">   This element.</param>
		/// <param name="name">      Name of the descendant to find.</param>
		/// <param name="foundChild">If successful contains a reference to found element.</param>
		/// <returns>True, if search was a success, otherwise false.</returns>
		public static bool TryGetElement([NotNull] this XmlElement element, string name, out XmlElement foundChild)
		{
			if (String.IsNullOrWhiteSpace(name))
			{
				foundChild = null;
				return false;
			}
			foundChild = element.GetElementsByTagName(name).OfType<XmlElement>().FirstOrDefault();
			return foundChild != null;
		}
		/// <summary>
		/// Copies characters from the string to the buffer.
		/// </summary>
		/// <param name="str">   String object.</param>
		/// <param name="buffer">Pointer to the beginning of the buffer.</param>
		/// <param name="start"> Zero-based index of the first symbol in the buffer to copy into.</param>
		/// <param name="count"> Number of characters to copy.</param>
		/// <exception cref="ArgumentNullException">The pointer to the buffer cannot be null.</exception>
		/// <exception cref="IndexOutOfRangeException">
		/// Index of the first element in the buffer cannot be less then 0.
		/// </exception>
		/// <exception cref="ArgumentException">Input string doesn't contain enough characters.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern unsafe void CopyToBuffer(this string str, char* buffer, int start, int count);
		/// <summary>
		/// Creates English ordinal number.
		/// </summary>
		/// <param name="number">A number to convert into ordinal form.</param>
		/// <returns>A text representation of the ordinal number.</returns>
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
	/// <summary>
	/// Contains old value of the property that has been changed.
	/// </summary>
	public class ValueChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Old value.
		/// </summary>
		public object OldValue { get; set; }
	}
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
		{
			return Enum.GetValues(typeof(T)).Cast<T>();
		}
	}
	/// <summary>
	/// Useful extensions when working with reflection.
	/// </summary>
	public static class ReflectionExtensions
	{
		#region Inheritance
		/// <summary>
		/// Determines whether a given type is the child of another.
		/// </summary>
		/// <param name="thisType">The child type.</param>
		/// <param name="baseType">The possible parent type.</param>
		/// <returns>True if thisType is a child of baseType.</returns>
		public static bool Implements([NotNull] this Type thisType, [NotNull] Type baseType)
		{
			return baseType.IsAssignableFrom(thisType) && thisType != baseType;
		}
		/// <summary>
		/// Determines whether a given type is the child of another.
		/// </summary>
		/// <typeparam name="T">The possible parent type.</typeparam>
		/// <param name="thisType">The child type.</param>
		/// <returns>True if thisType implements type T.</returns>
		public static bool Implements<T>([NotNull] this Type thisType)
		{
			return thisType.Implements(typeof(T));
		}
		/// <summary>
		/// Determines whether objects of this type can be assigned to another.
		/// </summary>
		/// <param name="thisType">This type.</param>
		/// <param name="baseType">Another type.</param>
		/// <returns>
		/// True, if <paramref name="thisType"/> is <paramref name="baseType"/> or is derived from it.
		/// </returns>
		public static bool ImplementsOrEquals([NotNull] this Type thisType, [NotNull] Type baseType)
		{
			return thisType == baseType || baseType.IsAssignableFrom(thisType);
		}
		/// <summary>
		/// Determines whether objects of this type can be assigned to another.
		/// </summary>
		/// <typeparam name="T">Another type.</typeparam>
		/// <param name="thisType">This type.</param>
		/// <returns>
		/// True, if <paramref name="thisType"/> is <typeparamref name="T"/> or is derived from it.
		/// </returns>
		public static bool ImplementsOrEquals<T>([NotNull] this Type thisType)
		{
			return thisType.ImplementsOrEquals(typeof(T));
		}
		/// <summary>
		/// Determines whether this type implements given generic type.
		/// </summary>
		/// <param name="thisType">       This type.</param>
		/// <param name="genericBaseType">Generic type.</param>
		/// <returns>True, this type implements given generic type.</returns>
		public static bool ImplementsGeneric([NotNull] this Type thisType, [NotNull] Type genericBaseType)
		{
			var type = thisType;

			while (type != null)
			{
				var interfaceTypes = type.GetInterfaces();

				if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericBaseType))
				{
					return true;
				}

				if (type.IsGenericType && type.GetGenericTypeDefinition() == genericBaseType)
					return true;

				type = type.BaseType;
			}

			return false;
		}
		/// <summary>
		/// Gets a collection of generic type arguments.
		/// </summary>
		/// <param name="thisType">       This type.</param>
		/// <param name="genericBaseType">Generic base type.</param>
		/// <returns>Enumeration of types.</returns>
		public static IEnumerable<Type> GetGenericArguments([NotNull] this Type thisType, [NotNull] Type genericBaseType)
		{
			var type = thisType;

			while (type != null)
			{
				var interfaceTypes = type.GetInterfaces();
				Type args = interfaceTypes.FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericBaseType);
				if (args != null)
				{
					return args.GetGenericArguments();
				}

				if (type.IsGenericType && type.GetGenericTypeDefinition() == genericBaseType)
					return type.GetGenericArguments();

				type = type.BaseType;
			}

			return null;
		}
		/// <summary>
		/// Gets all types given one inherits from.
		/// </summary>
		/// <param name="type">Type which base types to get.</param>
		/// <returns>An array of all types given one inherits from.</returns>
		public static Type[] GetInheritanceChain(this Type type)
		{
			return type.EnumerateInheritancChain().ToArray();
		}
		/// <summary>
		/// Enumerates through types this one inherits from.
		/// </summary>
		/// <param name="type">This type.</param>
		/// <returns>Enumerable collection that represents the inheritance chain.</returns>
		public static IEnumerable<Type> EnumerateInheritancChain(this Type type)
		{
			for (var current = type; current != null; current = current.BaseType)
				yield return current;
		}
		#endregion
		#region Attributes
		/// <summary>
		/// Determines whether this reflection object is decorated with at least one instance of a given
		/// attribute.
		/// </summary>
		/// <typeparam name="T">The attribute to search for.</typeparam>
		/// <param name="reflectionObject">The member on which the search is performed.</param>
		/// <returns>
		/// True if the member is decorated with at least one instance of attribute
		/// <typeparamref name="T"/>.
		/// </returns>
		public static bool ContainsAttribute<T>([NotNull] this ICustomAttributeProvider reflectionObject)
			where T : Attribute
		{
			return reflectionObject.IsDefined(typeof(T), true);
		}
		/// <summary>
		/// Gets all instances of a given attribute on the specified reflection object.
		/// </summary>
		/// <typeparam name="T">The attribute to search for.</typeparam>
		/// <param name="reflectionObject">The object on which the search is performed.</param>
		/// <returns>
		/// An array of <typeparamref name="T"/> that contains all found attributes. Can be empty.
		/// </returns>
		[NotNull]
		public static T[] GetAttributes<T>([NotNull] this ICustomAttributeProvider reflectionObject)
			where T : Attribute
		{
			return (T[])reflectionObject.GetCustomAttributes(typeof(T), true);
		}
		/// <summary>
		/// Gets the first instance of a given attribute on the specified reflection object.
		/// </summary>
		/// <typeparam name="T">The attribute to search for.</typeparam>
		/// <param name="reflectionObject">The object on which the search is performed.</param>
		/// <returns>
		/// The first instance of attribute <typeparamref name="T"/>, or null if none is found.
		/// </returns>
		public static T GetAttribute<T>([NotNull] this ICustomAttributeProvider reflectionObject)
			where T : Attribute
		{
			return reflectionObject.GetAttributes<T>().FirstOrDefault();
		}
		/// <summary>
		/// Tests whether the reflection object is decorated with a given attribute, and if so, assigns it
		/// via the out variable.
		/// </summary>
		/// <typeparam name="T">The attribute to search for.</typeparam>
		/// <param name="reflectionObject">The reflection object on which the search is performed.</param>
		/// <param name="attribute">       
		/// The out parameter to which the attribute will be assigned.
		/// </param>
		/// <returns>True if the attribute exists.</returns>
		public static bool TryGetAttribute<T>([NotNull] this ICustomAttributeProvider reflectionObject, out T attribute)
			where T : Attribute
		{
			var attributes = reflectionObject.GetCustomAttributes(typeof(T), true);

			if (attributes.Length > 0)
			{
				attribute = attributes[0] as T;
				return true;
			}

			attribute = null;
			return false;
		}
		#endregion
		#region Member Types
		/// <summary>
		/// Gets the type that is associated with a given member.
		/// </summary>
		/// <param name="info">Member.</param>
		/// <returns>Type of the field or property, if member is one of those, otherwise null.</returns>
		public static Type GetAssociatedType([NotNull] this MemberInfo info)
		{
			switch (info.MemberType)
			{
				case MemberTypes.Field:
					return ((FieldInfo)info).FieldType;
				case MemberTypes.Property:
					return ((PropertyInfo)info).PropertyType;
				default:
					return null;
			}
		}
		#endregion
		#region Methods
		/// <summary>
		/// Creates a delegate for given method.
		/// </summary>
		/// <typeparam name="T">Type of delegate to create.</typeparam>
		/// <param name="info">Method.</param>
		/// <returns>
		/// Delegate that allows to invoke method represented by <paramref name="info"/>.
		/// </returns>
		public static T CreateDelegate<T>(this MethodInfo info)
		{
			return (T)(object)info.CreateDelegate(typeof(T));
		}
		#endregion
	}
}