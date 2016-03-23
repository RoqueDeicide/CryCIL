using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using CryCil.Annotations;

namespace CryCil
{
	/// <summary>
	/// Defines extension methods for strings.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Determines whether given text is <c>null</c> or <see cref="string.Empty"/>.
		/// </summary>
		/// <param name="text">This text.</param>
		/// <returns>
		/// True, if <paramref name="text"/> is equal to <c>null</c> or is a <c>string</c> with 0 characters
		/// in it.
		/// </returns>
		[Pure]
		[ContractAnnotation("text:null => true")]
		public static bool IsNullOrEmpty(this string text)
		{
			return string.IsNullOrEmpty(text);
		}
		/// <summary>
		/// Determines whether given text is <c>null</c> or <see cref="string.Empty"/>, or only consists of
		/// whitespace characters.
		/// </summary>
		/// <param name="text">This text.</param>
		/// <returns>
		/// True, if <paramref name="text"/> is equal to <c>null</c> or is a <c>string</c> with 0 characters
		/// in it, or only consists of whitespace characters.
		/// </returns>
		[Pure]
		[ContractAnnotation("text:null => true")]
		public static bool IsNullOrWhiteSpace(this string text)
		{
			return string.IsNullOrWhiteSpace(text);
		}
		/// <summary>
		/// Finds zero-based indexes of all occurrences of given substring in the text.
		/// </summary>
		/// <param name="text">     Text to look for substrings in.</param>
		/// <param name="substring">Piece of text to look for.</param>
		/// <param name="options">  Text comparison options.</param>
		/// <returns>A list of all indexes.</returns>
		[Pure]
		[ContractAnnotation("text:null => null")]
		[ContractAnnotation("substring:null => null")]
		public static List<int> AllIndexesOf([NotNull] this string text, string substring,
											 StringComparison options)
		{
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(substring))
			{
				return null;
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
		[Pure]
		[ContractAnnotation("text:null => null")]
		[ContractAnnotation("substring:null => null")]
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
		[Pure]
		[ContractAnnotation("text:null => false")]
		[ContractAnnotation("strings:null => false")]
		public static bool ContainsAny(this string text, string[] strings)
		{
			if (string.IsNullOrEmpty(text) || strings.IsNullOrEmpty())
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
		[Pure]
		[ContractAnnotation("text:null => false")]
		public static bool IsValidFlowGraphName(this string text)
		{
			try
			{
				return !(string.IsNullOrWhiteSpace(text) ||
						 Regex.IsMatch(text, VariousConstants.InvalidXmlCharsPattern) ||
						 text.Any(char.IsWhiteSpace));
			}
			catch (RegexMatchTimeoutException)
			{
				return false;
			}
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
	}
}