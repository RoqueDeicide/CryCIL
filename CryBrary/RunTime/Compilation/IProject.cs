using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine.RunTime.Compilation
{
	/// <summary>
	/// Defines common properties of Visual Stdio project files.
	/// </summary>
	public interface IProject
	{
		/// <summary>
		/// Gets the list of project files that are required to be built to allow building of this one.
		/// </summary>
		IProject[] Dependencies { get; }
		/// <summary>
		/// Gets the name of the project.
		/// </summary>
		string Name { get; }
		/// <summary>
		/// Gets the name of the project file.
		/// </summary>
		string FileName { get; }
		/// <summary>
		/// Gets the path to the directory where to put the compiled assembly.
		/// </summary>
		string BinaryPath { get; }
		/// <summary>
		/// Gets reference to assembly all of the code has been compiled to.
		/// </summary>
		Assembly CompiledAssembly { get; }
		/// <summary>
		/// Compiles the project into the assembly.
		/// </summary>
		/// <returns>Compiled assembly.</returns>
		Assembly Build();
	}
}
