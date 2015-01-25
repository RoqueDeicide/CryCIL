using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CryEngine.Extensions;

namespace CryEngine.RunTime.Compilation
{
	/// <summary>
	/// Base class for project files.
	/// </summary>
	public abstract class Project : IProject
	{
		#region Abstracts
		/// <summary>
		/// When implemented in derived class, gets an array of project files this one
		/// depends on.
		/// </summary>
		/// <remarks>
		/// Projects in this array must be properly compiled before compilation of this
		/// project.
		/// </remarks>
		public abstract IProject[] Dependencies { get; }
		/// <summary>
		/// When implemented in derived class, gets the name of the project.
		/// </summary>
		public abstract string Name { get; }
		/// <summary>
		/// When implemented in derived class, gets the name of the project file.
		/// </summary>
		public abstract string FileName { get; }
		/// <summary>
		/// When implemented in derived class, gets the path to the folder where to put
		/// the assembly file.
		/// </summary>
		public abstract string BinaryPath { get; }
		/// <summary>
		/// Gets compiled assembly.
		/// </summary>
		public abstract Assembly CompiledAssembly { get; }
		/// <summary>
		/// When implemented in derived class, creates a Mono assembly from project data.
		/// </summary>
		/// <returns>Compiled assembly.</returns>
		public abstract Assembly Build();
		#endregion
		#region Statics
		private static List<Type> projectTypes;
		private static List<string> projectExtensions;
		static Project()
		{
			// Find types in this assembly that implement IProjectFile interface.
			Project.projectTypes =
			(
				from type in Assembly.GetExecutingAssembly().GetTypes()
				let projectAttributes = type.GetCustomAttributes(typeof(ProjectFileAttribute), false)
				where
					type.Implements(typeof(IProject))
					&&
					projectAttributes.Length == 1
					&&
					!String.IsNullOrEmpty(((ProjectFileAttribute)projectAttributes[0]).Extension)
				select type
			).ToList();

#if DEBUG
			if (projectTypes.Count == 0)
			{
				throw new Exception("Cannot compile the code without any types that implement IProjectFile and marked with ProjectFileAttribute.");
			}
#endif
			// Register extensions of project files.
			Project.projectExtensions = new List<string>(Project.projectTypes.Count);
			for (int i = 0; i < Project.projectTypes.Count; i++)
			{
				Project.projectExtensions.Add
				(
					(
						(ProjectFileAttribute)Project.projectTypes[i]
							.GetCustomAttributes(typeof(ProjectFileAttribute), false)[0]
					).Extension
				);
			}
		}
		/// <summary>
		/// Creates a project file wrapper object of type that corresponds given
		/// description.
		/// </summary>
		/// <param name="projectFileDescription">
		/// Text that contains description of the project in the solution file.
		/// </param>
		/// <returns>
		/// Object of type that is derived from <see cref="IProject"/> that fits the
		/// description, or null, if corresponding type could not be found.
		/// </returns>
		public static IProject Create(string projectFileDescription)
		{
			// Find name of the project and path to the file in the description.
			string[] parts = projectFileDescription.Split(new[] { ',' });
			// Name of the project is right before the first comma, path - second comma.
			string projectName =
				parts[0].Substring
				(
				// Name of the project is in quotation marks, last one right before comma.
					parts[0].LastIndexOf("\"", 0, parts[0].Length - 1, StringComparison.InvariantCulture)
				);
			string projectPath =
				parts[1].Substring
				(
				// Same deal with path as with name above.
					parts[1].LastIndexOf("\"", 0, parts[1].Length - 1, StringComparison.InvariantCulture)
				);
			return Project.Create(projectName, projectPath);
		}
		/// <summary>
		/// Creates new wrapper object for a project file.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="projectFile">Path to project file.</param>
		/// <returns>
		/// An instance of type that implements <see cref="IProject"/> that can parse
		/// given file format.
		/// </returns>
		public static IProject Create(string projectName, string projectFile)
		{
			// Check if file exists. If doesn't, then just ignore it.
			if (!File.Exists(projectFile))
			{
				return null;
			}
			// Try finding type that works with the extension.
			int extensionIndex = Project.projectExtensions.IndexOf(Path.GetExtension(projectFile));
			if (extensionIndex == -1)
			{
				return null;
			}
			Type projectType = Project.projectTypes[extensionIndex];
			return (IProject)Activator.CreateInstance(projectType, projectName, projectFile);
		}
		#endregion
	}
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	internal sealed class ProjectFileAttribute : Attribute
	{
		internal string Extension;
		internal ProjectFileAttribute(string ext)
		{
			this.Extension = ext;
		}
	}
}