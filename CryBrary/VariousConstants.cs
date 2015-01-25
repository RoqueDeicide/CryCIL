using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
	/// <summary>
	/// Defines some common constants.
	/// </summary>
	public static class VariousConstants
	{
		/// <summary>
		/// Pattern that allows to find characters that are not valid for XML tag names.
		/// </summary>
		public const string InvalidXmlCharsPattern = @"[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD\u10000-\u10FFFF]";
	}
}