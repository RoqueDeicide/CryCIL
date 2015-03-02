﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CryEngine.Annotations;

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
		/// <param name="minimalCount">
		/// Minimal number of elements that must be inside the collection.
		/// </param>
		/// <returns>
		/// True, if <paramref name="collection"/> is null, or it contains smaller number elements then one
		/// defined by <paramref name="minimalCount"/> .
		/// </returns>
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
			for (int i = text.IndexOf(substring, options); i != -1; )
			{
				indexes.Add(i);
				i = text.IndexOf(substring, i + substring.Length, options);
			}
			return indexes;
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
		/// Finds zero-based indexes of all occurrences of given substring in the text using the invariant
		/// culture.
		/// </summary>
		/// <param name="text">     Text to look for substrings in.</param>
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
		/// <summary>
		/// Tries to key associated with given value, if it exists in the dictionary.
		/// </summary>
		/// <typeparam name="TKey">Type of key.</typeparam>
		/// <typeparam name="TValue">Type of values.</typeparam>
		/// <param name="dictionary">Dictionary object.</param>
		/// <param name="value">     Values which to try to get.</param>
		/// <param name="key">       Resultant key.</param>
		/// <returns>True, if successful.</returns>
		public static bool TryGetKey<TKey, TValue>([NotNull]this IDictionary<TKey, TValue> dictionary, TValue value, out TKey key)
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
		public static bool Implements(this Type thisType, Type baseType)
		{
			return baseType.IsAssignableFrom(thisType) && thisType != baseType;
		}
		/// <summary>
		/// Determines whether a given type is the child of another.
		/// </summary>
		/// <typeparam name="T">The possible parent type.</typeparam>
		/// <param name="thisType">The child type.</param>
		/// <returns>True if thisType implements type T.</returns>
		public static bool Implements<T>(this Type thisType)
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
		public static bool ImplementsOrEquals(this Type thisType, Type baseType)
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
		public static bool ImplementsOrEquals<T>(this Type thisType)
		{
			return thisType.ImplementsOrEquals(typeof(T));
		}
		/// <summary>
		/// Determines whether this type implements given generic type.
		/// </summary>
		/// <param name="thisType">       This type.</param>
		/// <param name="genericBaseType">Generic type.</param>
		/// <returns>True, this type implements given generic type.</returns>
		public static bool ImplementsGeneric(this Type thisType, Type genericBaseType)
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
		public static IEnumerable<Type> GetGenericArguments(this Type thisType, Type genericBaseType)
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
		#endregion
		#region Attributes
		/// <summary>
		/// Determines whether this member is decorated with at least one instance of a given attribute.
		/// </summary>
		/// <typeparam name="T">The attribute to search for.</typeparam>
		/// <param name="info">The member on which the search is performed.</param>
		/// <returns>True if the member is decorated with at least one instance of attribute T.</returns>
		public static bool ContainsAttribute<T>(this MemberInfo info) where T : Attribute
		{
			return info.GetCustomAttributes(typeof(T), true).Length > 0;
		}
		/// <summary>
		/// Gets all instances of a given attribute on the selected member.
		/// </summary>
		/// <typeparam name="T">The attribute to search for.</typeparam>
		/// <param name="memberInfo">The member on which the search is performed.</param>
		/// <returns>The first instance of attribute T, or null if none is found.</returns>
		public static IEnumerable<T> GetAttributes<T>(this MemberInfo memberInfo) where T : Attribute
		{
			return (T[])memberInfo.GetCustomAttributes(typeof(T), true);
		}
		/// <summary>
		/// Gets the first instance of a given attribute on the selected member.
		/// </summary>
		/// <typeparam name="T">The attribute to search for.</typeparam>
		/// <param name="memberInfo">The member on which the search is performed.</param>
		/// <returns>The first instance of attribute T, or null if none is found.</returns>
		public static T GetAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			var attributes = memberInfo.GetAttributes<T>().ToList();
			return attributes.Count != 0 ? attributes[0] : null;
		}
		/// <summary>
		/// Tests whether the method is decorated with a given attribute, and if so, assigns it via the out
		/// variable.
		/// </summary>
		/// <typeparam name="T">The attribute to search for.</typeparam>
		/// <param name="memberInfo">The member on which the search is performed.</param>
		/// <param name="attribute"> The out parameter to which the attribute will be assigned.</param>
		/// <returns>True if the attribute exists.</returns>
		public static bool TryGetAttribute<T>(this MemberInfo memberInfo, out T attribute) where T : Attribute
		{
			var attributes = memberInfo.GetCustomAttributes(typeof(T), true);

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
		public static Type GetAssociatedType(this MemberInfo info)
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
	}
}