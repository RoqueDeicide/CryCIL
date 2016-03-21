using System;
using System.Linq;

namespace MainTestingAssembly
{
	/// <summary>
	/// Used by underlying framework to test IMonoText implementation.
	/// </summary>
	public static class StringTest
	{
		/// <summary>
		/// Gets a simple string literal.
		/// </summary>
		/// <returns>A literal which is an interned string.</returns>
		public static string GetInternedString()
		{
			return "Some interned string.";
		}
	}
}