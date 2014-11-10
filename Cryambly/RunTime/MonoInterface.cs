using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CryCil.Annotations;
using CryCil.Engine.DebugServices;
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
		/// <summary>
		/// Occurs when compilation starts.
		/// </summary>
		public event EventHandler CompilationStarted;
		/// <summary>
		/// Occurs when compilation is over, successfully or not.
		/// </summary>
		public event EventHandler<EventArgs<bool>> CompilationComplete;
		/// <summary>
		/// Occurs when initialization stage of specific index starts.
		/// </summary>
		public event EventHandler<EventArgs<int>> InitializationStageStarted;
		/// <summary>
		/// Occurs when initialization stage of specific index ends.
		/// </summary>
		public event EventHandler<EventArgs<int>> InitializationStageFinished;
		/// <summary>
		/// Occurs when CryCIL subsystem is updated.
		/// </summary>
		public event EventHandler Updated;
		/// <summary>
		/// Occurs when native Mono interface receive notification about system-wide shutdown.
		/// </summary>
		public event EventHandler ShuttingDown;
		#endregion
		#region Construction
		/// <summary>
		/// Called from C++ code to initialize CryCIL on C# side.
		/// </summary>
		private MonoInterface()
		{
			Application.EnableVisualStyles();
			// Register default handling of exceptions.
			AppDomain.CurrentDomain.UnhandledException +=
				(sender, args) => MonoInterface.DisplayException(args.ExceptionObject);
			// Redirect Console output.
			Console.SetOut(new ConsoleLogWriter());
			// Load all extra modules.
			string gameModulesFolder = Path.Combine
				(DirectoryStructure.ContentFolder, "Modules", "CryCIL");
			string cryEngineModulesFolder = Path.Combine
				(DirectoryStructure.CryEngineFolder, "Modules", "CryCIL");
			List<string> gameModules = new List<string>();
			List<string> cryEngineModules = new List<string>();
			if (Directory.Exists(gameModulesFolder))
			{
				gameModules.AddRange(Directory.GetFiles(gameModulesFolder, "*.dll"));
			}
			if (Directory.Exists(cryEngineModulesFolder))
			{
				cryEngineModules.AddRange(Directory.GetFiles(cryEngineModulesFolder, "*.dll"));
			}

			this.CryCilAssemblies = new List<Assembly>
			(
				from file in gameModules.Concat(cryEngineModules)
				where AssemblyExtras.IsAssembly(file)
				select Assembly.Load(AssemblyName.GetAssemblyName(file))
			);
			// Load and compile the solution.
			this.OnCompilationStarted();
			try
			{
				bool loadingSuccessful =
					CodeSolution.Load
					(
						Path.Combine
						(
							DirectoryStructure.ContentFolder,
							"Code", "Solutions", "CryCilCode.sln"
						)
					);
				if (!loadingSuccessful)
				{
					throw new Exception("Unable to load the solution.");
				}
				this.CryCilAssemblies.AddRange(CodeSolution.Build());
				this.OnCompilationComplete(true);
			}
			catch (Exception)
			{
				this.OnCompilationComplete(false);
			}
			// Add Cryambly to the list.
			this.CryCilAssemblies.Add(Assembly.GetAssembly(typeof(MonoInterface)));
			// A simple test for redirected console output.
			Console.Write(this.CryCilAssemblies.Count);
			Console.WriteLine(" CryCIL-specific assemblies are loaded.");
			Console.WriteLine("Proceeding to stage-based initialization.");
			this.ProceedWithInitializationStages();
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
		private static void DisplayException(object ex)
		{
			ExceptionDisplayForm form = new ExceptionDisplayForm(ex as Exception);
			form.ShowDialog();
		}
		[PublicAPI("Updates this subsystem.")]
		private void Update()
		{
			this.OnUpdated();
		}
		[PublicAPI("Informs this object about system-wide shutdown.")]
		private void Shutdown()
		{
			this.OnShuttingDown();
		}
		#endregion
		#region Utilities
		#region Event Raisers
		private void OnCompilationStarted()
		{
			Interops.Initialization.OnCompilationStartingBind();
			if (this.CompilationStarted != null) this.CompilationStarted(this, EventArgs.Empty);
		}
		private void OnCompilationComplete(bool success)
		{
			Interops.Initialization.OnCompilationCompleteBind(success);
			if (this.CompilationComplete != null)
				this.CompilationComplete(this, new EventArgs<bool>(success));
		}
		private void OnInitializationStageStarted(int index)
		{
			if (this.InitializationStageStarted != null)
				this.InitializationStageStarted(this, new EventArgs<int>(index));
		}
		private void OnInitializationStageFinished(int index)
		{
			if (this.InitializationStageFinished != null)
				this.InitializationStageFinished(this, new EventArgs<int>(index));
		}
		private void OnUpdated()
		{
			if (this.Updated != null) this.Updated(this, EventArgs.Empty);
		}
		private void OnShuttingDown()
		{
			if (this.ShuttingDown != null) this.ShuttingDown(this, EventArgs.Empty);
		}
		#endregion
		private void ProceedWithInitializationStages()
		{
			using (new ConsoleOutputLevel(LogPostType.Always))
			{
				// Gather some data about initialization stages.
				SortedList<int, InitializationStageFunction> stages =
					new SortedList<int, InitializationStageFunction>();

				Console.WriteLine("Collecting data about initialization stages.");
				// Create a map of functions and their indices.
				List<Tuple<InitializationStageFunction, int[]>> functionsMap =
					new List<Tuple<InitializationStageFunction, int[]>>
					(
						this.CryCilAssemblies
							// Get the types that are initialization classes.
							.SelectMany(assembly => assembly.GetTypes())
							.Where(type => type.ContainsAttribute<InitializationClassAttribute>())
							// Get the methods that are initialization ones with appropriate signature.
							.SelectMany(type => type.GetMethods(BindingFlags.Static))
							.Where
							(
								method =>
								{
									ParameterInfo[] pars = method.GetParameters();
									return
										method.ContainsAttribute<InitializationStageAttribute>() &&
										method.IsStatic &&
										pars.Length == 1 &&
										pars[0].ParameterType == typeof(int);
								}
							)
							// Parse the method info and gather usable data.
							.Select
							(
								method =>
									new Tuple<InitializationStageFunction, int[]>
									(
										method.CreateDelegate<InitializationStageFunction>(),
										method.GetCustomAttributes<InitializationStageAttribute>()
											  .Select(attr => attr.StageIndex)
											  .ToArray()
									)
							)
					)
					{
						// Add the native initialization function to the mix.
						new Tuple<InitializationStageFunction, int[]>
						(
							Interops.Initialization.OnInitializationStageBind,
							Interops.Initialization.GetSubscribedStagesBind()
						)
					};
				Console.WriteLine("Compiling data about initialization stages.");
				// Switch keys and values in the function map.
				for (int i = 0; i < functionsMap.Count; i++)
				{
					for (int j = 0; j < functionsMap[i].Item2.Length; j++)
					{
						int stageIndex = functionsMap[i].Item2[j];
						if (stages.ContainsKey(stageIndex))
						{
							stages[stageIndex] += functionsMap[i].Item1;
						}
						else
						{
							stages.Add(stageIndex, functionsMap[i].Item1);
						}
					}
				}
				Console.WriteLine("Commencing initialization stages.");
				// Now invoke everything.
				foreach (int key in stages.Keys)
				{
					this.OnInitializationStageStarted(key);		//
					stages[key](key);							// Yep, it's that simple.
					this.OnInitializationStageFinished(key);	//
				}
			}
		}
		#endregion
	}
	/// <summary>
	/// Delegate that represents functions that represent initialization stages.
	/// </summary>
	/// <param name="stageIndex">Index of the stage.</param>
	public delegate void InitializationStageFunction(int stageIndex);
}
