using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using CryCil.Annotations;

namespace CryCil.RunTime.Compilation
{
	/// <summary>
	/// Factory for types that implement <see cref="IProject"/> interface.
	/// </summary>
	public static class ProjectFactory
	{
		private static readonly List<Type> projectTypes;
		private static readonly List<string> projectExtensions;
		static ProjectFactory()
		{
			// Find types in this assembly that implement IProjectFile interface.
			projectTypes =
				(from type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
				 where type.ContainsAttribute<ProjectFileAttribute>()
				 select type)
					.ToList();

#if DEBUG
			if (projectTypes.Count == 0)
			{
				throw new Exception(
					"Cannot compile the code without any types that implement IProjectFile and marked with ProjectFileAttribute.");
			}
#endif
			// Register extensions of project files.
			Type[] ctorParameters = {typeof(string), typeof(string)};
			projectExtensions =
				(from projectType in projectTypes
				 let attr = projectType.GetAttribute<ProjectFileAttribute>()
				 where !string.IsNullOrWhiteSpace(attr.Extension) && projectType.GetConstructor(ctorParameters) != null
				 select attr.Extension)
					.ToList();
		}
		/// <summary>
		/// Creates a project file wrapper object of type that corresponds given description.
		/// </summary>
		/// <param name="projectFileDescription">
		/// Text that contains description of the project in the solution file.
		/// </param>
		/// <returns>
		/// Object of type that is derived from <see cref="IProject"/> that fits the description, or null,
		/// if corresponding type could not be found.
		/// </returns>
		[CanBeNull]
		public static IProject Create([NotNull] string projectFileDescription)
		{
			// Find name of the project and path to the file in the description.
			List<int> quoteIndices = projectFileDescription.AllIndexesOf("\"");
			string projectName =
				projectFileDescription.Substring( // Name of the project is between third and fourth quotation marks.
												 quoteIndices[2] + 1, quoteIndices[3] - quoteIndices[2] - 1);
			string projectPath =
				projectFileDescription.Substring( // Path is between fifth and sixth quotation marks.
												 quoteIndices[4] + 1, quoteIndices[5] - quoteIndices[4] - 1);
			return Create(projectName,
						  Path.Combine(CodeSolution.SolutionFolder, projectPath));
		}
		/// <summary>
		/// Creates new wrapper object for a project file.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="projectFile">Path to project file.</param>
		/// <returns>
		/// An instance of type that implements <see cref="IProject"/> that can parse given file format.
		/// </returns>
		[CanBeNull]
		public static IProject Create([NotNull] string projectName, [NotNull] string projectFile)
		{
			// Check if file exists. If doesn't, then just ignore it.
			if (!File.Exists(projectFile))
			{
				return null;
			}
			// Try finding type that works with the extension.
			int extensionIndex = projectExtensions.IndexOf(Path.GetExtension(projectFile));
			if (extensionIndex == -1)
			{
				return null;
			}
			Type projectType = projectTypes[extensionIndex];
			try
			{
				return Activator.CreateInstance(projectType, projectName, projectFile) as IProject;
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
				return null;
			}
		}
	}
	/// <summary>
	/// Marks classes that implement <see cref="IProject"/> that should be recognized by the factory.
	/// </summary>
	/// <remarks>
	/// It is necessary for the project class to have constructor that takes two arguments of type
	/// <see cref="String"/>, first one being a name, and last one being the path to the file.
	/// </remarks>
	[BaseTypeRequired(typeof(IProject))]
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class ProjectFileAttribute : Attribute
	{
		/// <summary>
		/// Extension of the files that define the project.
		/// </summary>
		internal string Extension;
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="ext">Extension.</param>
		public ProjectFileAttribute(string ext)
		{
			this.Extension = ext;
		}
	}
}