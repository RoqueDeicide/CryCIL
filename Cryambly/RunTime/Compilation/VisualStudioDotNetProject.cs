using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CryCil.Annotations;
using CryEngine.RunTime.Compilation;

namespace CryCil.RunTime.Compilation
{
	/// <summary>
	/// Base class for .Net/Mono projects that use Visual Studio project files.
	/// </summary>
	public abstract class VisualStudioDotNetProject : IProject
	{
		#region Fields
		private const string Config
#if DEBUG
			= "Debug";
#else
			= "Release";
#endif
		private const string Platform
#if WIN32
			= "x86";
#else
			= "x64";
#endif

		private readonly string[] projectReferences;
#endregion
		#region Properties
		/// <summary>
		/// When implemented in derived class, gets an object that will handle compilation of the code files within this project.
		/// </summary>
		public abstract CodeDomProvider Compiler { get; }
		/// <summary>
		/// Gets the list of projects this one depends on.
		/// </summary>
		public IProject[] Dependencies
		{
			get
			{
				return
					CodeSolution.Projects.Where(x=>this.projectReferences.Contains(x.Name))
										 .ToArray();
			}
		}
		/// <summary>
		/// Gets the name of the project.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Gets the path to the project.
		/// </summary>
		public string FileName { get; private set; }
		/// <summary>
		/// Path to the directory that contains compiled assembly after building.
		/// </summary>
		[CanBeNull]
		public string OutputPath { get; private set; }
		/// <summary>
		/// Path to documentation file.
		/// </summary>
		[CanBeNull]
		public string DocumentationFile { get; private set; }
		/// <summary>
		/// Target platform for the assembly.
		/// </summary>
		[CanBeNull]
		public string TargetPlatform { get; private set; }
		/// <summary>
		/// Indicates whether unsafe code is allowed within the project.
		/// </summary>
		public bool AllowUnsafeCode { get; private set; }
		/// <summary>
		/// Indicates how much debug information must be saved during the build.
		/// </summary>
		public DebugInformationLevels DebugInformation { get; private set; }
		/// <summary>
		/// Gets the list of defined constants, like DEBUG.
		/// </summary>
		public string DefinedConstants { get; private set; }
		/// <summary>
		/// Indicates whether compiler should consider compilation a failure, if there are any warnings.
		/// </summary>
		public bool TreatWarningsAsErrors { get; private set; }
		/// <summary>
		/// Indicates if the output should be optimized by the compiler.
		/// </summary>
		public bool OptimizeCode { get; private set; }
		/// <summary>
		/// Gets compiled assembly.
		/// </summary>
		[CanBeNull]
		public Assembly CompiledAssembly { get; private set; }
		/// <summary>
		/// Gets an array of paths to code files.
		/// </summary>
		public string[] CodeFiles { get; private set; }
		/// <summary>
		/// Gets an array of paths to assemblies that this project references.
		/// </summary>
		public string[] References { get; private set; }
		#endregion
#region Construction
		/// <summary>
		/// Initializes common properties of types that inherit this class.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="projectFile">Path to the project file.</param>
		protected VisualStudioDotNetProject
		(
			string projectName,
			[PathReference] string projectFile
		)
		{
			this.Name = projectName;
			this.FileName = projectFile;
			if (!File.Exists(this.FileName))
			{
				throw new ArgumentException
					(String.Format("Could locate a project file: {0}", this.FileName));
			}
			const StringComparison icic =
				StringComparison.InvariantCultureIgnoreCase;
			XmlDocument document = new XmlDocument();
			try
			{
				document.Load(this.FileName);
				// Declare properties to get from the file.
				Settings sets = new Settings();
				XmlElement[] propertyGroups =
					document.GetElementsByTagName("PropertyGroup")
					.OfType<XmlElement>().ToArray();
				for (int i = 0; i < propertyGroups.Length; i++)
				{
					// Get the condition.
					string specifier = ExtractConditionSpecifier(propertyGroups[i]);
					// Check the condition.
					if (specifier == null ||					// No condition == true.
						!specifier.Contains("|") &&				// Condition with just a config or platform.
						SimpleConditionMet(specifier, icic) ||	// ^
						SpecificConditionMet(specifier, icic))	// Condition with both config and platform.
					{
						// Get the settings from the property group.
						sets.FromXml(propertyGroups[i]);
					}
				}
				// Save the properties.
				this.OutputPath = sets.Output;
				this.DocumentationFile = sets.Doc;
				this.DebugInformation =
					sets.Debug.Equals("None", icic)
						? DebugInformationLevels.None
						: sets.Debug.Equals("pdbonly", icic)
							? DebugInformationLevels.PdbOnly
							: DebugInformationLevels.Full;
				this.TargetPlatform = sets.Target;
				this.AllowUnsafeCode = sets.AllowUnsafe.Equals("true", icic);
				this.DefinedConstants = sets.Consts;
				this.TreatWarningsAsErrors = sets.WarningsAsErrors.Equals("true", icic);
				this.OptimizeCode = sets.Optimize.Equals("true", icic);
				// Save project references.
				this.projectReferences =
				(
					from XmlElement element in document.GetElementsByTagName("ProjectReference")
					select element.GetAttribute("Include")
				).ToArray();
				// Save references.
				this.References =
				(
					from XmlElement element in document.GetElementsByTagName("Reference")
					let hintPath =
						element.GetElementsByTagName("HintPath").OfType<XmlElement>().ToArray()
					select
						// Use a hint path if available.
						hintPath.Length == 0
						? element.GetAttribute("Include")
						: hintPath[0].FirstChild.Value
				).ToArray();
				// Save a list of files for compilation.
				this.CodeFiles =
				(
					from XmlElement element in document.GetElementsByTagName("Compile")
					select element.GetAttribute("Include")
				).ToArray();
			}
			catch (Exception ex)
			{
				throw new ArgumentException
				(
					String.Format("File {0} is not a recognizable project file.", this.FileName),
					ex
				);
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static string ExtractConditionSpecifier(XmlElement propertyGroup)
		{
			if (!propertyGroup.HasAttribute("Condition"))
			{
				return null;
			}
			string condition = propertyGroup.GetAttribute("Condition");
			// Parse it.
			int equalsSignIndex = condition.IndexOf("==");
			int penultimateInvertedComma = condition.IndexOf("'", equalsSignIndex);
			int lastInvertedComma = condition.LastIndexOf("'");
			// Get the important part.
			return condition.Substring(penultimateInvertedComma + 1,
									   lastInvertedComma - penultimateInvertedComma - 1);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool SpecificConditionMet(string specifier, StringComparison icic)
		{
			string[] configPlatform = specifier.Split(new[] { '|' });
			return configPlatform[0].Equals(Config, icic)
				   &&
				   (
					   configPlatform[1].Equals(Platform, icic)
					   ||
					   configPlatform[1].Equals("AnyCPU", icic)
				   );
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool SimpleConditionMet(string specifier, StringComparison icic)
		{
			return specifier.Equals(Platform, icic) ||
				   specifier.Equals(Config, icic) ||
				   specifier.Equals("AnyCPU", icic);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Compiles code from this project into assembly.
		/// </summary>
		/// <returns>True, if compilation was a success, otherwise false.</returns>
		public bool Build()
		{
// ReSharper disable RedundantEmptyObjectOrCollectionInitializer
			CompilerParameters parameters = new CompilerParameters { };
// ReSharper restore RedundantEmptyObjectOrCollectionInitializer
			
			//
			// Output.
			//
			parameters.OutputAssembly = Path.Combine(this.OutputPath ?? "", this.Name, ".dll");
			// Delete previous assembly.
			if (File.Exists(parameters.OutputAssembly))
			{
				try
				{
					File.Delete(parameters.OutputAssembly);
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
								parameters.OutputAssembly
							)
						);
					}
					throw;
				}
			}
			//
			// Debug/Release specifics.
			//
			StringBuilder extraOptions = new StringBuilder();
			extraOptions.Append("/checked- ");
			switch (this.DebugInformation)
			{
				case DebugInformationLevels.None:
					break;
				case DebugInformationLevels.PdbOnly:
					extraOptions.Append("/debug- ");
					break;
				case DebugInformationLevels.Full:
					extraOptions.Append("/debug+ ");
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			extraOptions.AppendFormat("/define:{0} ", this.DefinedConstants);
			if (!String.IsNullOrWhiteSpace(this.DocumentationFile))
			{
				extraOptions.AppendFormat("/doc:{0} ", this.DocumentationFile);
			}
			if (this.OptimizeCode)
			{
				extraOptions.Append("/optimize ");
			}
			extraOptions.AppendFormat("/platform:{0} ", Platform);
			parameters.TreatWarningsAsErrors = this.TreatWarningsAsErrors;
			if (this.AllowUnsafeCode)
			{
				extraOptions.Append("/unsafe ");
			}
			parameters.CompilerOptions = extraOptions.ToString();
			//
			// References.
			//
			parameters.ReferencedAssemblies.AddRange(this.References);
			parameters.ReferencedAssemblies.AddRange
				(this.Dependencies.Select(x=>x.CompiledAssembly.GetLocation()).ToArray());
			//
			// Build.
			//
			CompilerResults results =
				this.Compiler.CompileAssemblyFromFile(parameters, this.CodeFiles);
			
			results.TempFiles.KeepFiles = false;

			if (results.Errors.HasErrors)
			{
				// Print the errors.
				Console.WriteLine("Failed to compile {0}", this.Name);
				Console.WriteLine("Errors:");
				for (int i = 0; i < results.Errors.Count; i++)
				{
					Console.WriteLine
					(
						"    {0}) Line: {1}, Column: {2}, {5}: {3} ({4})",
						i, results.Errors[i].Line, results.Errors[i].Column,
						results.Errors[i].ErrorText, results.Errors[i].ErrorNumber,
						results.Errors[i].IsWarning ? "Warning" : "Error"
					);
				}
				return false;
			}

			this.CompiledAssembly = results.CompiledAssembly;

			return true;
		}
#endregion
#region Utilities

#endregion
		private struct Settings
		{
			public string Output;			// Output path.
			public string Doc;				// Documentation file.
			public string Target;			// x86, x64, AnyCPU etc.
			public string AllowUnsafe;
			public string Debug;			// full, pdbonly, nothing
			public string Consts;			// Compilation symbols.
			public string WarningsAsErrors;	// Treat warning as errors.
			public string Optimize;			// Optimize code.
			public void FromXml([NotNull] XmlElement propertyGroup)
			{
				this.Output = GetVal(propertyGroup, "OutputPath") ?? this.Output;
				this.Doc = GetVal(propertyGroup, "DocumentationFile") ?? this.Doc;
				this.Target = GetVal(propertyGroup, "PlatformTarget") ?? this.Target;
				this.AllowUnsafe = GetVal(propertyGroup, "AllowUnsafeBlocks") ?? this.AllowUnsafe;
				this.Debug = GetVal(propertyGroup, "DebugType") ?? this.Debug;
				this.Consts = GetVal(propertyGroup, "DefineConstants") ?? this.Consts;
				this.WarningsAsErrors = GetVal(propertyGroup, "TreatWarningsAsErrors") ?? this.WarningsAsErrors;
				this.Optimize = GetVal(propertyGroup, "Optimize") ?? this.Optimize;
			}
			private static string GetVal([NotNull] XmlElement propertyGroup,
										 [NotNull] string propName)
			{
				XmlElement firstOrDefault =
					propertyGroup.GetElementsByTagName(propName)
								 .OfType<XmlElement>().FirstOrDefault();
				return (firstOrDefault != null) ? firstOrDefault.FirstChild.Value : null;
			}
		}
		/// <summary>
		/// Enumeration of levels of debug information to create during the build.
		/// </summary>
		public enum DebugInformationLevels
		{
			/// <summary>
			/// No debug information will be stored.
			/// </summary>
			None,
			/// <summary>
			/// Pdb file will be created only.
			/// </summary>
			PdbOnly,
			/// <summary>
			/// All debug information about the assembly will be stored.
			/// </summary>
			Full
		}
	}
}
