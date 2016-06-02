using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CopyBuildOutput
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			PrintDebugMessage("Command line arguments:");
			foreach (string s in args)
			{
				PrintDebugMessage(s);
			}

			// Path to the directory that contains the built file.
			string targetDirectoryPath = Path.GetFullPath(args[0]);
			// Path to Build folder in CryCIL directory.
			string buildPath = Path.GetFullPath(args[1]); 
			// Path to the CRYENGINE directory.
			string enginePath = Path.GetFullPath(args[2]);

			PrintDebugMessage($"Target path: {targetDirectoryPath}");
			PrintDebugMessage($"Build path: {buildPath}");
			PrintDebugMessage($"Engine path: {enginePath}");

			string outputDirName = "Output" + Path.DirectorySeparatorChar;
			string monoDirName = "Mono" + Path.DirectorySeparatorChar;

			// Copy the output.
			CopyFiles(targetDirectoryPath, buildPath, outputDirName, enginePath, "");

			// Copy the Mono folder.
			string monoPath = Path.Combine(buildPath, monoDirName);
			string monoTarget = Path.Combine(enginePath, Path.Combine(enginePath, targetDirectoryPath.Substring(Path.Combine(buildPath, "Output").Length + 1)));

			PrintDebugMessage($"Mono path: {monoPath}");
			PrintDebugMessage($"Mono target path: {monoTarget}");

			CopyFiles(monoPath, buildPath, monoDirName, monoTarget, Path.Combine("Modules", "CryCIL", monoDirName));
		}
		private static void CopyFiles(string sourceDir, string buildDir, string sourceSubDirName,
			string targetDir, string targetSubDir)
		{
			string subDir = Path.Combine(buildDir, sourceSubDirName);
			var files = Directory.EnumerateFiles(sourceDir, "*.*", SearchOption.AllDirectories);
			foreach (string file in files)
			{
				if (Path.GetExtension(file) == ".tmp")
				{
					// Don't copy temporary files.
					continue;
				}
				string destination = CreateDestinationName(file, subDir, Path.Combine(targetDir, targetSubDir));

				try
				{
					FileInfo sourceFile = new FileInfo(file);
					FileInfo destFile = new FileInfo(destination);

					if (!destFile.Exists || destFile.LastWriteTime < sourceFile.LastWriteTime)
					{
						Console.WriteLine("Copying file {0} to {1}", Path.GetFullPath(file), Path.GetFullPath(destination));

						string destDirName = Path.GetDirectoryName(destination) ?? "";
						if (!Directory.Exists(destDirName))
						{
							Directory.CreateDirectory(destDirName);
						}

						File.Copy(file, destination, true);
					}
				}
				catch (Exception ex)
				{
					PrintDebugMessage($"Unable to copy the file {file} due to an error: {ex.Message}");
				}
			}
		}
		private static string CreateDestinationName(string initialFileName, string sourceDir, string destDir)
		{
			PrintDebugMessage($"Source: {sourceDir}");
			PrintDebugMessage($"Target: {destDir}");
			string truncatedName = initialFileName.Substring(sourceDir.Length);
			PrintDebugMessage($"Truncated: {truncatedName}");
			return Path.Combine(destDir, truncatedName);
		}
		[Conditional("DEBUG")]
		private static void PrintDebugMessage(string message)
		{
			Console.WriteLine(message);
		}
	}
}