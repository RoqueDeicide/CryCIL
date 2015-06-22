using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CryCil.Annotations;
using CryCil.Engine.DebugServices;
using CryCil.RunTime.Compilation;
using CryCil.RunTime.Logging;
using CryCil.RunTime.Registration;

namespace CryCil.RunTime
{
	/// <summary>
	/// Represents an interface of CryCIL on managed side.
	/// </summary>
	public static class MonoInterface
	{
		#region Fields
		#endregion
		#region Properties
		/// <summary>
		/// Gets the list of assemblies that are loaded and recognized as part of CryCIL.
		/// </summary>
		/// <remarks>
		/// All CryEngine related code, like entity and FlowGraph node definitions, is located in these
		/// assemblies, so you don't have to loop through all base libraries to find those definitions.
		/// </remarks>
		public static readonly List<Assembly> CryCilAssemblies;
		#endregion
		#region Events
		/// <summary>
		/// Occurs when compilation starts.
		/// </summary>
		public static event Action CompilationStarted;
		/// <summary>
		/// Occurs when compilation is over, successfully or not.
		/// </summary>
		public static event Action<bool> CompilationComplete;
		/// <summary>
		/// Occurs when initialization stage of specific index starts.
		/// </summary>
		public static event Action<int> InitializationStageStarted;
		/// <summary>
		/// Occurs when initialization stage of specific index ends.
		/// </summary>
		public static event Action<int> InitializationStageFinished;
		/// <summary>
		/// Occurs when CryCIL subsystem is updated.
		/// </summary>
		public static event Action Updated;
		/// <summary>
		/// Occurs when native Mono interface receive notification about system-wide shutdown.
		/// </summary>
		public static event Action ShuttingDown;
		#endregion
		#region Construction
		static MonoInterface()
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

			CryCilAssemblies = new List<Assembly>
			(
				from file in gameModules.Concat(cryEngineModules)
				where AssemblyExtras.IsAssembly(file)
				select Assembly.Load(AssemblyName.GetAssemblyName(file))
			);
			// Load and compile the solution.
			OnCompilationStarted();
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
				CryCilAssemblies.AddRange(CodeSolution.Build());
				OnCompilationComplete(true);
			}
			catch (Exception)
			{
				OnCompilationComplete(false);
			}
			// Add Cryambly to the list.
			CryCilAssemblies.Add(Assembly.GetAssembly(typeof(MonoInterface)));
			// A simple test for redirected console output.
			Console.WriteLine("{0} CryCIL-specific assemblies are loaded.", CryCilAssemblies.Count);
			Console.WriteLine("Proceeding to stage-based initialization.");
			ProceedWithInitializationStages();
		}
		#endregion
		#region Interface
		/// <summary>
		/// Displays an exception to the user with an option to either continue or shutdown.
		/// </summary>
		/// <param name="ex">An object that represents the exception.</param>
		[UnmanagedThunk]
		public static void DisplayException(object ex)
		{
			ExceptionDisplayForm form = new ExceptionDisplayForm(ex as Exception);
			form.ShowDialog();
		}
		[UnmanagedThunk("Called from C++ code to initialize CryCIL on managed side.")]
		private static void Initialize()
		{
			// Accessing static variable to trigger static constructor to start initialization process.

// ReSharper disable UnusedVariable
			var l = CryCilAssemblies;
// ReSharper restore UnusedVariable
		}
		[UnmanagedThunk("Invoked from C++ code after FlowGraph is initialized" +
				   " to register FlowGraph nodes defined in CryCIL.")]
		private static void RegisterFlowGraphNodeTypes()
		{
			FlowNodeTypeRegistry.RegisterAllTypes();
		}
		[UnmanagedThunk("Updates this subsystem.")]
		private static void Update()
		{
			OnUpdated();
		}
		[UnmanagedThunk("Informs this object about system-wide shutdown.")]
		private static void Shutdown()
		{
			OnShuttingDown();
		}
		#endregion
		#region Utilities
		#region Event Raisers
		private static void OnCompilationStarted()
		{
			OnCompilationStartingBind();
			if (CompilationStarted != null) CompilationStarted();
		}
		private static void OnCompilationComplete(bool success)
		{
			OnCompilationCompleteBind(success);
			if (CompilationComplete != null)
				CompilationComplete(success);
		}
		private static void OnInitializationStageStarted(int index)
		{
			if (InitializationStageStarted != null)
				InitializationStageStarted(index);
		}
		private static void OnInitializationStageFinished(int index)
		{
			if (InitializationStageFinished != null)
				InitializationStageFinished(index);
		}
		private static void OnUpdated()
		{
			if (Updated != null) Updated();
		}
		private static void OnShuttingDown()
		{
			if (ShuttingDown != null) ShuttingDown();
		}
		#endregion
		private static void ProceedWithInitializationStages()
		{
			using (new ConsoleOutputLevel(LogPostType.Always))
			{
				// Gather some data about initialization stages.
				SortedList<int, InitializationStageFunction> stages =
					new SortedList<int, InitializationStageFunction>();

				Console.WriteLine("Collecting data about initialization stages.");
				// Get the types that are initialization classes.
				List<Type> initializationTypes =
					CryCilAssemblies
						.SelectMany(x => x.GetTypes())
						.Where(x => x.ContainsAttribute<InitializationClassAttribute>())
						.ToList();
				List<Tuple<InitializationStageFunction, int[]>> functionsMap =
					initializationTypes
						.SelectMany(x => x.GetMethods())
						.Where
						(
							method =>
							{
								// Get the methods that are initialization ones with appropriate signature.
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
						).ToList();
				// Create a map of functions and their indices.
				functionsMap.Add
				(
					// Add the native initialization function to the mix.
					new Tuple<InitializationStageFunction, int[]>
					(
						OnInitializationStageBind,
						GetSubscribedStagesBind()
					)
				);
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
					OnInitializationStageStarted(key);		//
					stages[key](key);						// Yep, it's that simple.
					OnInitializationStageFinished(key);		//
				}
			}
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void OnCompilationStartingBind();
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void OnCompilationCompleteBind(bool success);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int[] GetSubscribedStagesBind();
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void OnInitializationStageBind(int stageIndex);
		#endregion
	}
	/// <summary>
	/// Delegate that represents functions that represent initialization stages.
	/// </summary>
	/// <param name="stageIndex">Index of the stage.</param>
	public delegate void InitializationStageFunction(int stageIndex);
}