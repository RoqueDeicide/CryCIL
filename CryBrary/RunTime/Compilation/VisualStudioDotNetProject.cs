using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CryEngine.Initialization;
using CryEngine.Utilities;
using Microsoft.CSharp;

namespace CryEngine.RunTime.Compilation
{
	/// <summary>
	/// Base class for Visual Studio .NET projects. (C#, VisualBasic .NET)
	/// </summary>
	public abstract class VisualStudioDotNetProject : Project
	{
		#region Fields
		private readonly XmlDocument projectDocument;
		private Assembly compiledAssembly;
		private readonly string name;
		private readonly string fileName;
		private readonly string binaryPath;
		#endregion
		#region Properties
		/// <summary>
		/// When implemented in derived class, specifies which <see cref="CodeDomProvider"/> will be used
		/// to compile the code from the project.
		/// </summary>
		public abstract CodeDomProvider Provider { get; }
		/// <summary>
		/// Gets the list of projects that must be compiled before this one.
		/// </summary>
		public override IProject[] Dependencies
		{
			get
			{
				return
				(
					from XmlElement element in projectDocument.GetElementsByTagName("ProjectReference")
					select CodeSolution.Projects.Find
							(
								x =>
									x.FileName == element.GetAttribute("Include")
									&&
									x.Name == element.GetElementsByTagName("Name")[0].FirstChild.Value
							)
				).ToArray();
			}
		}
		/// <summary>
		/// Gets the compiled assembly.
		/// </summary>
		public override Assembly CompiledAssembly
		{
			get { return this.compiledAssembly; }
		}
		/// <summary>
		/// Gets the name of the project.
		/// </summary>
		public override string Name
		{
			get { return this.name; }
		}
		/// <summary>
		/// Gets full name of the project file.
		/// </summary>
		public override string FileName
		{
			get { return this.fileName; }
		}
		/// <summary>
		/// Gets the path to the binary.
		/// </summary>
		public override string BinaryPath
		{
			get { return this.binaryPath; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Loads project file into <see cref="XmlDocument"/>.
		/// </summary>
		/// <param name="fileName">Path to project file.</param>
		protected VisualStudioDotNetProject(string name, string fileName)
		{
			this.projectDocument = new XmlDocument();
			this.projectDocument.Load(fileName);

			this.name = name;
			this.fileName = fileName;
			// Better make sure that the path is the same for all configurations.
			this.binaryPath = this.projectDocument.GetElementsByTagName("OutputPath")[0].FirstChild.Value;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Builds this project.
		/// </summary>
		/// <returns>Reference to assembly wrapper object.</returns>
		public override Assembly Build()
		{
			CompilerParameters compilationParameters = new CompilerParameters
			{
#if DEBUG
				GenerateExecutable = false,
				IncludeDebugInformation = true,
				GenerateInMemory = false
#endif
#if RELEASE
				GenerateExecutable = false,
				IncludeDebugInformation = false,
				GenerateInMemory = false,
				CompilerOptions = "/optimize"
#endif
			};
			// Delete previous assembly.
			string assemblyFile = Path.Combine(this.BinaryPath, this.AssemblyName, ".dll");
			if (File.Exists(assemblyFile))
			{
				try
				{
					File.Delete(assemblyFile);
				}
				catch (Exception ex)
				{
					if (ex is UnauthorizedAccessException || ex is IOException)
					{
						throw new CodeCompilationException
						(
							String.Format
							(
								"Unable to compile the code: Assembly file {0} cannot be overwritten.",
								assemblyFile
							)
						);
					}
					throw;
				}
			}
			compilationParameters.OutputAssembly = assemblyFile;
			// Get list of referenced assemblies from the project file.
			compilationParameters.ReferencedAssemblies.AddRange(this.ReferencedAssemblies);
			// Find all code files.
			string[] codeFiles = this.GetCodeFiles;
			CompilerResults results;
			using (CodeDomProvider provider = this.Provider)
			{
				results = provider.CompileAssemblyFromFile(compilationParameters, codeFiles);
			}
			this.compiledAssembly = ValidateCompilation(results);
			return this.compiledAssembly;
		}
		#endregion
		#region Utilities
		private string[] ReferencedAssemblies
		{
			get
			{
				return
				(
					this.Dependencies.Select(x => x.CompiledAssembly.GetLocation())
									 .Concat(this.ReferencesFromFile)
				).ToArray();
			}
		}
		private IEnumerable<string> ReferencesFromFile
		{
			get
			{
				// Get all elements named "Reference", and group them by them having and not having a child
				// node called "HintPath".
				var references =
				(
					from XmlElement element in this.projectDocument.GetElementsByTagName("Reference")
					group element by element.ChildNodes.OfType<XmlElement>().Any(x => x.Name == "HintPath")
						into elementGroups
						select elementGroups
				).ToList();

				List<string> refs = new List<string>();
				// Add GAC references. These are represented by elements without child nodes.
				refs.AddRange
				(
					references
					.Where(x => !x.Key)
					.Select
					(
						y => y.Select
						(
							x => Path.Combine(ProjectSettings.GacFolder, x.GetAttribute("Include"), ".dll")
						)
					).FirstOrDefault() ?? new string[] { }
				);
				// Add the rest.
				refs.AddRange
				(
					references
					.Where(x => x.Key)
					.Select
					(
						y => y.Select
						(
							x => x.FirstChild.Value		// Hint path is just a path to the assembly.
						)
					).FirstOrDefault() ?? new string[] { }
				);
				return refs;
			}
		}
		private string[] GetCodeFiles
		{
			get
			{
				string projectFolder = Path.GetDirectoryName(this.FileName) ?? "";
				return
				(
					from XmlElement element in this.projectDocument.GetElementsByTagName("Compile")
					select Path.Combine(projectFolder, element.GetAttribute("Include"))
				).ToArray();
			}
		}
		private string AssemblyName
		{
			get { return this.projectDocument.GetElementsByTagName("AssemblyName")[0].FirstChild.Value; }
		}
		private static Assembly ValidateCompilation(CompilerResults results)
		{
			// No errors? Fine, just get the assembly.
			if (!results.Errors.HasErrors && results.CompiledAssembly != null)
				return results.CompiledAssembly;
			// Otherwise, create cool looking message that has a list of all compilation errors and throw
			// the exception.
			string compilationError = string.Format("Compilation failed; {0} errors: ", results.Errors.Count);

			foreach (CompilerError error in results.Errors)
			{
				compilationError += Environment.NewLine;

				if (!error.ErrorText.Contains("(Location of the symbol related to previous error)"))
					compilationError += string.Format("{0}({1},{2}): {3} {4}: {5}", error.FileName, error.Line, error.Column, error.IsWarning ? "warning" : "error", error.ErrorNumber, error.ErrorText);
				else
					compilationError += "    " + error.ErrorText;
			}

			throw new CodeCompilationException(compilationError);
		}
		#endregion
	}
}