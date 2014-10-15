using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;

namespace CryCil.RunTime.Compilation
{
	/// <summary>
	/// Factory for types that implement <see cref="IProject"/> interface.
	/// </summary>
	public static class ProjectFactory
	{
		private static List<Type> projectTypes;
		private static List<string> projectExtensions;
		static ProjectFactory()
		{
			// Find types in this assembly that implement IProjectFile interface.
			ProjectFactory.projectTypes =
			(
				from type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
				where type.ContainsAttribute<ProjectFileAttribute>()
				select type
			).ToList();

#if DEBUG
			if (ProjectFactory.projectTypes.Count == 0)
			{
				throw new Exception("Cannot compile the code without any types that implement IProjectFile and marked with ProjectFileAttribute.");
			}
#endif
			// Register extensions of project files.
			Type[] ctorParameters = { typeof(string), typeof(string) };
			ProjectFactory.projectExtensions =
			(
				from projectType in projectTypes
				let attr = projectType.GetAttribute<ProjectFileAttribute>()
				where !String.IsNullOrWhiteSpace(attr.Extension) &&
					projectType.GetConstructor(ctorParameters) != null
				select attr.Extension
			).ToList();
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
		[CanBeNull]
		public static IProject Create([NotNull] string projectFileDescription)
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
			return ProjectFactory.Create(projectName, projectPath);
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
		[CanBeNull]
		public static IProject Create([NotNull] string projectName, [NotNull] string projectFile)
		{
			// Check if file exists. If doesn't, then just ignore it.
			if (!File.Exists(projectFile))
			{
				return null;
			}
			// Try finding type that works with the extension.
			int extensionIndex = ProjectFactory.projectExtensions.IndexOf(Path.GetExtension(projectFile));
			if (extensionIndex == -1)
			{
				return null;
			}
			Type projectType = ProjectFactory.projectTypes[extensionIndex];
			return Activator.CreateInstance(projectType, projectName, projectFile) as IProject;
		}
	}
	/// <summary>
	/// Marks classes that implement <see cref="IProject"/> that should be recognized by
	/// the factory.
	/// </summary>
	/// <remarks>
	/// It is necessary for the project class to have constructor that takes two arguments
	/// of type <see cref="String"/>, first one being a name, and last one being the path
	/// to the file.
	/// </remarks>
	[BaseTypeRequired(typeof(IProject))]
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
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