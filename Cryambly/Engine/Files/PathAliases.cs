using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Files
{
	/// <summary>
	/// Provides access to CryEngine path aliasing system.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Aliases are special identifiers that can be put into a path and unwrapped into a dynamically
	/// defined path segment.
	/// </para>
	/// <para>
	/// Using aliases enhances portability since you can register the alias to different values depending
	/// on the situation.
	/// </para>
	/// <para>
	/// There are no explicit references to the format of the alias names but the only example ("%USER%")
	/// uses all caps characters surrounded by percentage symbols.
	/// </para>
	/// <para>
	/// WARNING: All internal path handling code uses only ASCII characters, which means, you should avoid
	///          using aliases that may resolve into paths that have Unicode characters (like
	///          aforementioned %USER% alias), or warn users about potential issues with using national
	///          alphabets for stuff like Windows user names.
	/// </para>
	/// </remarks>
	[SuppressMessage("ReSharper", "ExceptionNotThrown")]
	public static class PathAliases
	{
		/// <summary>
		/// Gets the value that is assigned to the alias.
		/// </summary>
		/// <remarks>Cache returned value to avoid excessive GC pressure.</remarks>
		/// <param name="alias">     Name of the alias.</param>
		/// <param name="returnName">
		/// Indicates whether value provided by <paramref name="alias"/> should be returned if the alias
		/// wasn't found.
		/// </param>
		/// <returns>
		/// The value that is assigned to the provided alias or if alias is not registered
		/// <paramref name="alias"/> value will be returned if <paramref name="returnName"/> is set to
		/// <c>true</c>, otherwise returns null.
		/// </returns>
		/// <exception cref="ArgumentNullException">Name of the alias cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string Get(string alias, bool returnName = false);
		/// <summary>
		/// Sets a new value for an alias.
		/// </summary>
		/// <param name="alias">Name of the alias (e.g. %SOMEFOLDER%).</param>
		/// <param name="value">
		/// Path segment the alias will unwrapped into (e.g. C:\\Users). If null the alias will be removed
		/// from the system.
		/// </param>
		/// <exception cref="ArgumentNullException">Name of the alias cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Set(string alias, string value);
	}
}