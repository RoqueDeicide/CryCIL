using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;
using CryCil.RunTime.Compilation;
using CryCil.RunTime.Logging;

namespace CryCil.RunTime
{
	/// <summary>
	/// Represents an object that is an interface of CryCIL on managed side.
	/// </summary>
	/// <remarks>
	/// An object of this type is always created on startup from C++ code.
	/// </remarks>
	public sealed class MonoInterface
	{
		#region Fields
		/// <summary>
		/// An object of this type that is created from C++ code when CryCIL is initialized.
		/// </summary>
		public static MonoInterface Instance { get; private set; }
		#endregion
		#region Properties
		/// <summary>
		/// Gets the list of assemblies that are loaded and recognized as part of CryCIL.
		/// </summary>
		/// <remarks>
		/// All CryEngine related code, like entity and FlowGraph node definitions, is located in these assemblies, so you don't have to loop through all base libraries to find those definitions.
		/// </remarks>
		public List<Assembly> CryCilAssemblies { get; private set; }
		#endregion
		#region Events

		#endregion
		#region Construction
		/// <summary>
		/// Called from C++ code to initialize CryCIL on C# side.
		/// </summary>
		private MonoInterface()
		{
			// Register default handling of exceptions.
			AppDomain.CurrentDomain.UnhandledException +=
				(sender, args) => MonoInterface.Instance.DisplayException(args.ExceptionObject);
			// Redirect Console output.
			Console.SetOut(new ConsoleLogWriter());
			// Load all extra modules.
			this.CryCilAssemblies = new List<Assembly>
			(
				from file in Directory.GetFiles(Path.Combine(DirectoryStructure.ContentFolder, "Modules"))
				where file.EndsWith("dll") && AssemblyExtras.IsAssembly(file)
				select Assembly.Load(AssemblyName.GetAssemblyName(file))
			);
			// Load and compile the solution.
			CodeSolution.Load
			(
				Path.Combine
				(DirectoryStructure.ContentFolder, "Code", "Solutions", "CryCilCode.sln")
			);
			this.CryCilAssemblies.AddRange(CodeSolution.Build());
			// Add Cryambly to the list.
			this.CryCilAssemblies.Add(Assembly.GetAssembly(typeof(MonoInterface)));
		}
		#endregion
		#region Interface
		[PublicAPI("Called from C++ code to initialize CryCIL on managed side.")]
		private static MonoInterface Initialize()
		{
			MonoInterface.Instance = new MonoInterface();
			return MonoInterface.Instance;
		}
		[PublicAPI("Invoked from C++ code after FlowGraph is initialized" +
				   " to register FlowGraph nodes defined in CryCIL.")]
		private void RegisterFlowGraphNodeTypes()
		{
			
		}
		[PublicAPI("Displays exception that was not handled.")]
		private void DisplayException(object ex)
		{
			ExceptionDisplayForm form = new ExceptionDisplayForm(ex as Exception);
			form.ShowDialog();
		}
		#endregion
		#region Utilities
		#endregion
	}
}
