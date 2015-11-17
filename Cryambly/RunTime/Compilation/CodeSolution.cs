using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using CryCil.Annotations;
using CryCil.RunTime.Compilation.Reporting;

namespace CryCil.RunTime.Compilation
{
	/// <summary>
	/// Handles compilation of .NET/Mono code associated with CryCIL.
	/// </summary>
	/// <remarks>Solution file is a standard .sln file created by Visual Studio or SharpDevelop.</remarks>
	public static class CodeSolution
	{
		#region Fields
		internal static readonly List<IProject> Projects = new List<IProject>();
		/// <summary>
		/// Project tag within solution file.
		/// </summary>
		public static readonly string ProjectTag = string.Format("{0}Project(", Environment.NewLine);
		/// <summary>
		/// EndProject tag within solution file.
		/// </summary>
		public static readonly string EndProjectTag = string.Format("{0}EndProject{0}", Environment.NewLine);
		/// <summary>
		/// Global tag within solution file.
		/// </summary>
		public static readonly string GlobalTag = string.Format("{0}Global{0}", Environment.NewLine);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the folder where current solution is located.
		/// </summary>
		public static string SolutionFolder { get; private set; }
		#endregion
		#region Interface
		/// <summary>
		/// Loads and parses given solution file.
		/// </summary>
		/// <param name="solutionFile">
		/// Full name of .sln file that contains details about the solution.
		/// </param>
		/// <returns>Indication of success.</returns>
		public static bool Load([PathReference] string solutionFile)
		{
			if (File.Exists(solutionFile))
			{
				SolutionFolder = Path.GetDirectoryName(solutionFile);
				ParseFile(solutionFile);
				return true;
			}
			return false;
		}
		/// <summary>
		/// Builds the solution.
		/// </summary>
		/// <returns>An array of compiled assemblies.</returns>
		public static Assembly[] Build()
		{
			// This is the list of projects to compile.
			List<IProject> buildList = new List<IProject>(Projects.Count);
			// We gonna organize the list so, projects that depend on other projects are built after them.
			List<IProject> projects = new List<IProject>(Projects);
			while (projects.Count != 0)
			{
				// Go through the list and put into the build list those who already have their
				// dependencies in the build list.
				for (int i = 0; i < projects.Count; i++)
				{
					// If project has no extra dependencies, then just put it into the list.
					if (projects[i].Dependencies.Length == 0 ||
						projects[i].Dependencies.All(buildList.Contains))
					{
						buildList.Add(projects[i]);
						projects.RemoveAt(i);
					}
				}
			}
			// Creates a dictionary where keys are names of projects that were a failure to compile, and
			// values a reasons, why compilation was a failure.
			Dictionary<string, string> failures = new Dictionary<string, string>(buildList.Count);
			List<Assembly> compiledAssemblies = new List<Assembly>(buildList.Count);
			while (buildList.Count != 0)
			{
				IProject currentProject = buildList[0];
				buildList.RemoveAt(0);
				if (currentProject.Build() && currentProject.CompiledAssembly != null)
				{
					compiledAssemblies.Add(currentProject.CompiledAssembly);
				}
				else
				{
					failures.Add(currentProject.Name,
								 "Failed to build, check the log for possible errors.");
					// Consider builds of all projects that depend on this one a failure.
					IProject[] deps = buildList.Where(x => x.Dependencies.Any(y => y.FileName == currentProject.FileName)).ToArray();
					for (int i = 0; i < deps.Length; i++)
					{
						var dependant = deps[0];
						buildList.Remove(dependant);
						failures.Add(dependant.Name,
									 string.Format("Failed to compile this project, because it depends on failed project {0}", currentProject.Name));
					}
				}
			}
			if (failures.Count != 0)
			{
				CompilationProblemsReportForm form = new CompilationProblemsReportForm(failures);
				form.ShowDialog();
			}
			// Building is a process that creates a lot of garbage.
			GC.Collect();

			return compiledAssemblies.ToArray();
		}
		#endregion
		#region Utilities
		[SuppressMessage("ReSharper", "ExceptionNotDocumented")]
		private static void ParseFile([PathReference] string file)
		{
			// Load up the file.
			string solutionFileText;
			FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
			using (StreamReader sr = new StreamReader(fs))
			{
				solutionFileText = sr.ReadToEnd();
			}
			// Cut off unneeded start and end of the file.
			int firstProjectWordIndex =
				solutionFileText.IndexOf(ProjectTag, StringComparison.InvariantCulture);
			int firstGlobalWordIndex =
				solutionFileText.IndexOf(GlobalTag, StringComparison.InvariantCulture);
			solutionFileText = solutionFileText.Substring(firstProjectWordIndex,
														  firstGlobalWordIndex - firstProjectWordIndex + 2);
			// Find starts and ends of each Project section.
			List<int> projectSectionStartIndices = solutionFileText.AllIndexesOf(ProjectTag);
			List<int> projectSectionEndIndices = solutionFileText.AllIndexesOf(EndProjectTag);
			if (projectSectionStartIndices.Count != projectSectionEndIndices.Count)
			{
				throw new Exception("Solution file is not properly written: Number of project section" +
									" start points is not the same as number of end points.");
			}
			// Load up projects.
			for (int i = 0; i < projectSectionStartIndices.Count; i++)
			{
				// Get the text between Project and EndProject tags.
				string projectDescription =
					solutionFileText.Substring(projectSectionStartIndices[i] + "Project".Length,
											   projectSectionEndIndices[i] - projectSectionStartIndices[i]);
				IProject project = ProjectFactory.Create(projectDescription);
				if (project != null)
				{
					Projects.Add(project);
				}
			}
		}
		#endregion
	}
}