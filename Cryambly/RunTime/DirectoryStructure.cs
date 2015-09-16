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
		/// Defines the name of the folder within CryEngine folder that contains platform-specific (x86 or
		/// x64) assemblies.
		/// </summary>
		public const string PlatformFolderName =
#if WIN32
			"Bin32";
#else
			"Bin64";
#endif
		/// <summary>
		/// Gets the path to the CryEngine folder.
		/// </summary>
		/// <remarks>
		/// The CryEngine folder is the one that contains system.cfg that contains "sys_game_folder=" line
		/// within.
		/// </remarks>
		public static string CryEngineFolder { get; private set; }
		/// <summary>
		/// Gets path to the platform-specific (x86 or x64) folder within main installation directory.
		/// </summary>
		public static string PlatformFolder { get; private set; }
		/// <summary>
		/// Gets the path to the directory that contains game content.
		/// </summary>
		/// <remarks>
		/// The path to the game content in clean Eaas SDK installation is &lt;CryEngine Installation
		/// Path&gt;\GameSDK.
		/// </remarks>
		public static string ContentFolder { get; private set; }
		/// <summary>
		/// Gets the path to the folder with referable assemblies.
		/// </summary>
		public static string AssembliesFolder { get; private set; }
		static DirectoryStructure()
		{
			string contents;
			string currentDirectory = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);
			string root = Path.GetPathRoot(currentDirectory);
			// Look for system.cfg
			while (currentDirectory != root)
			{
				if (currentDirectory == null)
				{
					break;
				}
				string systemFile =
					Directory
						.GetFiles(currentDirectory, "system.cfg", SearchOption.TopDirectoryOnly)
						.FirstOrDefault();
				if (systemFile != null)
				{
					// Found one, lets check it for presence of sys_game_folder
					using (StreamReader sr = new StreamReader(systemFile))
					{
						contents = sr.ReadToEnd();
					}
					if (contents.Contains("sys_game_folder=") ||
						contents.Contains("sys_game_folder ="))
					{
						CryEngineFolder = Path.GetDirectoryName(systemFile);
						break;
					}
				}
				// Move to the parent directory.
				currentDirectory = Path.GetDirectoryName(currentDirectory);
			}
			if (CryEngineFolder == null)
			{
				throw new Exception("Unable to locate base CryEngine folder.");
			}

			PlatformFolder = Path.Combine(CryEngineFolder,
										  PlatformFolderName);

			// Get game folder from system.cfg
			using (StreamReader sr = new StreamReader(Path.Combine(CryEngineFolder, "system.cfg")))
			{
				contents = sr.ReadToEnd();
			}
			string[] lines =
				contents.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
			string gameFolderLine = lines.FirstOrDefault(l => l.Contains("sys_game_folder"));
			if (gameFolderLine == null)
			{
				throw new Exception("Unable to find location of game folder.");
			}
			int gameFolderNameStart = gameFolderLine.LastIndexOf('=') + 1;
			ContentFolder =
				Path.Combine
					(
					 CryEngineFolder,
					 gameFolderLine.Substring
						 (
						  gameFolderNameStart,
						  gameFolderLine.Length - gameFolderNameStart
						 )
					);
			// Get Mono lib folder.
			AssembliesFolder =
				Path.Combine
					(
					 CryEngineFolder,
					 "Bin32",
					 "Modules",
					 "CryCIL",
					 "Mono",
					 "lib"
					);
		}
	}
}