using System.Reflection;

namespace CryCil.RunTime.Compilation
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
		string OutputPath { get; }
		/// <summary>
		/// Gets reference to assembly all of the code has been compiled to.
		/// </summary>
		Assembly CompiledAssembly { get; }
		/// <summary>
		/// Builds the project.
		/// </summary>
		/// <returns>True if build was a success.</returns>
		bool Build();
	}
}