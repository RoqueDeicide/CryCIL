using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CryEngine.Utilities;

namespace CryEngine.RunTime.Compilation
{
	/// <summary>
	/// Represents a Visual Studio solution.
	/// </summary>
	/// <remarks>
	/// Solution file is a standard .sln file created by Visual Studio or SharpDevelop.
	/// </remarks>
	public static class CodeSolution
	{
		/// <summary>
		/// A list of projects in the solution.
		/// </summary>
		public static List<IProject> Projects { get; private set; }
		/// <summary>
		/// Creates new instance of type <see cref="CodeSolution"/>.
		/// </summary>
		/// <param name="file">Path to solution file.</param>
		public static void Load(string file)
		{
			if (!File.Exists(file))
			{
				throw new ArgumentException("Given file does not exist.");
			}
			CodeSolution.Projects = new List<IProject>();
			CodeSolution.ParseSolutionFile(file);
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
					if (projects[i].Dependencies.Length == 0 || projects[i].Dependencies.All(buildList.Contains))
					{
						buildList.Add(projects[i]);
						projects.RemoveAt(i);
					}
				}
			}
			return (buildList.Select(project => project.Build())).ToArray();
		}
		private static void ParseSolutionFile(string file)
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
				throw new Exception("Solution file is not properly written: Number of project section start points is not the same as number of end points.");
			}
			// Load up projects.
			for (int i = 0; i < projectSectionStartIndices.Count; i++)
			{
				CodeSolution.Projects.Add
				(
					Project.Create
					(
						solutionFileText.Substring
						(
							projectSectionStartIndices[i] + "Project".Length,
							projectSectionEndIndices[i] - projectSectionStartIndices[i]
						)
					)
				);
			}
		}
	}
}