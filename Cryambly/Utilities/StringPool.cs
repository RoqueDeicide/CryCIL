using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CryCil.Utilities
{
	/// <summary>
	/// Defines functionality that allows C# strings to be mapped to their null-terminated equivalents.
	/// </summary>
	/// <remarks>
	/// Sometimes CryEngine native code expects certain strings to be from some kind of a pool. C++ does
	/// have one for literals, so in C++ that is fine. In C# however all null-terminated strings that are
	/// created by conversion of C# ones are dynamically allocated and therefore their pooling must be done
	/// manually. This class provides means of doing that.
	/// </remarks>
	public static class StringPool
	{
		private static readonly SortedList<string, IntPtr> pool = new SortedList<string, IntPtr>();
		private static readonly IntPtr emptyNullTerminatedString;
		static StringPool()
		{
			emptyNullTerminatedString = Marshal.StringToHGlobalAnsi("");
		}
		/// <summary>
		/// Gets a pointer to a null-terminated equivalent of the given string.
		/// </summary>
		/// <param name="text">
		/// Text to convert to null-terminated representation and cache the result.
		/// </param>
		/// <returns>Cached result of conversion.</returns>
		public static IntPtr Get(string text)
		{
			IntPtr ptr = new IntPtr();

			if (text == null)
			{
				return ptr;
			}

			if (text == "")
			{
				return emptyNullTerminatedString;
			}

			if (!pool.TryGetValue(text, out ptr))
			{
				try
				{
					ptr = Marshal.StringToHGlobalAnsi(text);
				}
				catch (OutOfMemoryException)
				{
					return IntPtr.Zero;
				}
				pool.Add(text, ptr);
			}

			return ptr;
		}
	}
}