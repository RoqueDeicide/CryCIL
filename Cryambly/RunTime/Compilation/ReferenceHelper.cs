using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Utilities;

namespace CryCil.RunTime.Compilation
{
	/// <summary>
	/// Defines useful methods for locating referenced assemblies.
	/// </summary>
	public static class ReferenceHelper
	{
		/// <summary>
		/// An array of .Net framework versions.
		/// </summary>
		public static string[] Versions = {"v1.1", "v2.0", "v3.0", "v3.5", "v4.0", "v4.5"};
		/// <summary>
		/// An array of .Net framework versions without leading letter v.
		/// </summary>
		public static string[] VersionsShort = {"1.1", "2.0", "3.0", "3.5", "4.0", "4.5"};
		/// <summary>
		/// Maps names of .Net framework versions to constants defined in
		/// <see cref="TargetDotNetFrameworkVersion"/> enumeration.
		/// </summary>
		public static Dictionary<string, TargetDotNetFrameworkVersion> TargetFrameworkVersionMap =
			new Dictionary<string, TargetDotNetFrameworkVersion>
			{
				{"v1.1", TargetDotNetFrameworkVersion.Version11},
				{"v2.0", TargetDotNetFrameworkVersion.Version20},
				{"v3.0", TargetDotNetFrameworkVersion.Version30},
				{"v3.5", TargetDotNetFrameworkVersion.Version35},
				{"v4.0", TargetDotNetFrameworkVersion.Version40},
				{"v4.5", TargetDotNetFrameworkVersion.Version45}
			};
		/// <summary>
		/// Finds location of given version of given assembly.
		/// </summary>
		/// <param name="assemblyName">Name of the assembly to find.</param>
		/// <param name="version">     Version of framework to use.</param>
		/// <returns>Absolute path to referenced assembly.</returns>
		public static string GetLocation(string assemblyName, string version)
		{
			if (!assemblyName.EndsWith(".dll"))
			{
				assemblyName += ".dll";
			}
			// Try to find a different version, if current one does not exist.
			int currentVersionIndex = (Versions as IList<string>).IndexOf(version);
			for (int i = currentVersionIndex; i >= 0; i--)
			{
				TargetDotNetFrameworkVersion versionId = TargetFrameworkVersionMap[Versions[i]];
				string path = Path.Combine(ToolLocationHelper.GetPathToDotNetFramework(versionId),
										   VersionsShort[i], assemblyName);
				if (File.Exists(path))
				{
					return path;
				}
			}
			return assemblyName;
		}
	}
}