using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CryCil.Annotations;

namespace CryCil.RunTime.Compilation
{
	/// <summary>
	/// Handles compilation of .NET/Mono code associated with CryCIL.
	/// </summary>
	/// <remarks>
	/// Solution file is a standard .sln file created by Visual Studio or SharpDevelop.
	/// </remarks>
	public static class CodeSolution
	{
		#region Fields
		internal static readonly List<IProject> Projects = new List<IProject>();
		#endregion
		#region Properties

		#endregion
		#region Interface
		/// <summary>
		/// Loads and parses given solution file.
		/// </summary>
		/// <param name="solutionFile">
		/// Full name of .sln file that contains details about the solution.
		/// </param>
		public static void Load([PathReference] string solutionFile)
		{
			if (File.Exists(solutionFile))
			{
				CodeSolution.ParseFile(solutionFile);
			}
		}
		/// <summary>
		/// Builds the solution.
		/// </summary>
		/// <returns>An array of compiled assemblies.</returns>
		public static Assembly[] Build()
		{
			// This is the list of projects to compile.
			List<IProject> buildList = new List<IProject>(CodeSolution.Projects.Count);
			// We gonna organize the list so, projects that depend on other projects are
			// built after them.
			List<IProject> projects = new List<IProject>(CodeSolution.Projects);
			while (projects.Count != 0)
			{
				// Go through the list and put into the build list those who already have
				// their dependencies in the build list.
				for (int i = 0; i < projects.Count; i++)
				{
					// If project has no extra dependencies, then just put it into the
					// list.
					if (projects[i].Dependencies.Length == 0 ||
						projects[i].Dependencies.All(buildList.Contains))
					{
						buildList.Add(projects[i]);
						projects.RemoveAt(i);
					}
				}
			}
			// Creates a dictionary where keys are names of projects that were a failure to compile, and values a reasons, why compilation was a failure.
			Dictionary<string, string> failures = new Dictionary<string, string>(buildList.Count);
			List<Assembly> compiledAssemblies = new List<Assembly>(buildList.Count);
			while (buildList.Count != 0)
			{
				IProject currentProject = buildList[0];
				if (currentProject.Build() && currentProject.CompiledAssembly != null)
				{
					compiledAssemblies.Add(currentProject.CompiledAssembly);
				}
				else
				{
					buildList.RemoveAt(0);
					failures.Add(currentProject.Name,
						"Failed to build, check the log for possible errors.");
					// Consider builds of all projects that depend on this one a failure.
					foreach
					(
						IProject dependant in
							from project in buildList 
							where project.Dependencies.Any
								(
									x => x.Name == currentProject.Name &&
										x.FileName == currentProject.FileName
								)
							select project
					)
					{
						buildList.Remove(dependant);
						failures.Add(dependant.Name,
									 String.Format("Failed to compile this project, because it depends on failed project {0}", currentProject.Name));
					}
				}
			}
			if (failures.Count != 0)
			{
				// Get the length of the longest name for later formatting.
				int maxNameLength = failures.Keys.Max(x => x.Length);
				StringBuilder message = new StringBuilder();
				message.Append("Project  ".PadLeft(maxNameLength + 4));
				message.Append("Cause");
				message.Append(Environment.NewLine);
				foreach (KeyValuePair<string, string> failure in failures)
				{
					message.Append(failure.Key.PadLeft(maxNameLength + 2)); // Name of failed project.
					message.Append("  ");									// Spacing between the name and the cause.
					message.Append(failure.Value);							// Cause of failure.
					message.Append(Environment.NewLine);
				}
				MessageBox.Show
				(
					message.ToString(),
					"There are projects in the code that failed to build",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
			}
			return compiledAssemblies.ToArray();
		}
		#endregion
		#region Utilities
		private static void ParseFile([PathReference] string file)
		{
			// Load up the file.
			string solutionFileText;
			using
			(
				StreamReader sr =
					new StreamReader
					(
						new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read)
					)
			)
			{
				solutionFileText = sr.ReadToEnd();
			}
			// Cut off unneeded start and end of the file.
			int firstProjectWordIndex = solutionFileText.IndexOf("Project", StringComparison.InvariantCulture);
			int firstGlobalWordIndex = solutionFileText.IndexOf("Global", StringComparison.InvariantCulture);
			solutionFileText =
				solutionFileText.Substring
				(
					firstProjectWordIndex,
					firstGlobalWordIndex - firstProjectWordIndex
				);
			// Find starts and ends of each Project section.
			List<int> projectSectionStartIndices = solutionFileText.AllIndexesOf("Project");
			List<int> projectSectionEndIndices = solutionFileText.AllIndexesOf("EndProject");
			if (projectSectionStartIndices.Count != projectSectionEndIndices.Count)
			{
				throw new Exception
				(
					"Solution file is not properly written: Number of project section" +
					" start points is not the same as number of end points."
				);
			}
			// Load up projects.
			for (int i = 0; i < projectSectionStartIndices.Count; i++)
			{
				// Get the text between Project and EndProject tags.
				string projectDescription =
					solutionFileText.Substring
					(
						projectSectionStartIndices[i] + "Project".Length,
						projectSectionEndIndices[i] - projectSectionStartIndices[i]
					);
				IProject project = ProjectFactory.Create(projectDescription);
				if (project != null)
				{
					CodeSolution.Projects.Add(project);
				}
			}
		}
		#endregion
	}
}