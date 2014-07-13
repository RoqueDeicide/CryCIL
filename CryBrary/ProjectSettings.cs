// List of definitions that can be used: (Apply via project settings) RELEASE_DISABLE_CHECKS -
// disable non-vital checks in release mode, if set, CryMono will ignore most safety checks and
// disable the large majority of exceptions.

using System.IO;

namespace CryEngine.Utilities
{
	public static class ProjectSettings
	{
		/// <summary>
		/// Designates what link will be opened when the user presses 'Report Bug' in the exception form.
		/// </summary>
		public static readonly string BugReportLink = "http://www.crydev.net/posting.php?mode=post&f=375"; /* CryMono sub-forum @ CryDev.net */

		/// <summary>
		/// Message displayed in the exception form when an exception has been thrown.
		/// </summary>
		public static readonly string ExceptionMessage = @"An unhandled exception occurred!

        If you believe this is a CryMono bug, please report it on CryDev, including the stacktrace below.";

		public static string TempDirectory { get { return Path.Combine(Path.GetTempPath(), "CryMono"); } }

		/// <summary>
		/// Gets the folder containing CryMono configuration files.
		/// </summary>
		public static string ConfigFolder { get; internal set; }

		/// <summary>
		/// Directory from which CryMono will load managed plugins.
		/// </summary>
		public static string PluginsFolder { get { return Path.Combine(ConfigFolder, "Plugins"); } }

		/// <summary>
		/// Gets the Mono folder containing essential Mono libraries and configuration files.
		/// </summary>
		public static string MonoFolder { get { return Path.Combine(ConfigFolder, "Mono"); } }
	}
}