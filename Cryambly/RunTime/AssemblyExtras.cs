using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.RunTime
{
	/// <summary>
	/// Defines static methods for working with assemblies.
	/// </summary>
	public static class AssemblyExtras
	{
		/// <summary>
		/// Determines whether given file contains valid .NET/Mono assembly.
		/// </summary>
		/// <param name="file">Path to the file.</param>
		/// <returns>True, if the file specified by the path contains valid managed assembly.</returns>
		public static bool IsAssembly(string file)
		{
			try
			{
				AssemblyName.GetAssemblyName(file);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}