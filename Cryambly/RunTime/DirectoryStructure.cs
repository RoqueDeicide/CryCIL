using System;
using System.IO;
using System.Linq;

namespace CryCil.RunTime
{
	/// <summary>
	/// Provides names of different directories with CryEngine folder.
	/// </summary>
	public static class DirectoryStructure
	{
		/// <summary>
		/// Gets the path to the folder that contains the project.
		/// </summary>
		/// <remarks>
		/// The project is a set of code, data etc. that doesn't include the CryEngine itself, but uses the
		/// latter to work.
		/// </remarks>
		public static string ProjectFolder { get; private set; }
		/// <summary>
		/// Gets the path to the folder that contains the game-specific data.
		/// </summary>
		/// <remarks>This folder is located in the <see cref="ProjectFolder"/>.</remarks>
		public static string ContentFolder { get; private set; }
		/// <summary>
		/// Gets the path to the folder that contains the CryEngine platform.
		/// </summary>
		public static string PlatformFolder { get; private set; }
		/// <summary>
		/// Gets the path to the folder that contains the .exe file for this game.
		/// </summary>
		public static string ExecutablesFolder { get; private set; }
		[RawThunk("Invoked by underlying framework to let the managed code know the paths to relevant " +
				  "directories.")]
		private static void InitializeFolderPaths(string exeFolder, string projectFolder, string gameFolder)
		{
			try
			{
				ExecutablesFolder = exeFolder;
				PlatformFolder = new DirectoryInfo(exeFolder).Parent?.Parent?.FullName ?? "";
				ProjectFolder = projectFolder;
				ContentFolder = gameFolder;
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
	}
}