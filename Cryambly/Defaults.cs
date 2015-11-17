using System;
using System.Globalization;

namespace CryCil
{
	/// <summary>
	/// Defines various default values, used throughout Cryambly code.
	/// </summary>
	public static class Defaults
	{
		/// <summary>
		/// Default culture that is used by methods that create text representations of objects without
		/// defined parsing methods.
		/// </summary>
		/// <remarks>
		/// If Parse-type methods are not defined by the class, then the text representations mostly serve
		/// presentation purposes making adaptation to current culture desirable and consistence across
		/// machines and configurations unneeded.
		/// </remarks>
		public static readonly CultureInfo CultureToStringOnly = CultureInfo.CurrentCulture;
		/// <summary>
		/// Default culture that is used by methods that create text representations of objects with
		/// defined parsing methods.
		/// </summary>
		/// <remarks>
		/// If Parse-type methods are defined by the class, then the text representations are intended to
		/// be used for long-term storage which necessitates default culture to be the same on all machines
		/// and configurations.
		/// </remarks>
		public static readonly CultureInfo CultureTwoWay = CultureInfo.InvariantCulture;
	}
}