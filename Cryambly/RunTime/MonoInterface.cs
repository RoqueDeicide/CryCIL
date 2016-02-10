using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using CryCil.Engine.DebugServices;
using CryCil.RunTime.Compilation;
using CryCil.RunTime.Logging;
using CryCil.RunTime.Registration;
using CryCil.Utilities;

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
		public static readonly List<Assembly> CryCilAssemblies = new List<Assembly>(50);
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
			Application.EnableVisualStyles();

			// Register default handling of exceptions.
			AppDomain.CurrentDomain.UnhandledException +=
				(sender, args) => DisplayException(args.ExceptionObject);

			// Redirect Console output.
			Console.SetOut(new ConsoleLogWriter());

			// Load all extra modules.
			string gameModulesPath = Path.Combine(DirectoryStructure.ContentFolder, "Modules", "CryCIL");
			string engineModulesPath = Path.Combine(DirectoryStructure.PlatformFolder, "Modules", "CryCIL");

			List<string> gameModules = new List<string>();
			List<string> cryEngineModules = new List<string>();

			if (Directory.Exists(gameModulesPath))
			{
				gameModules.AddRange(Directory.GetFiles(gameModulesPath, "*.dll"));
			}
			if (Directory.Exists(engineModulesPath))
			{
				cryEngineModules.AddRange(Directory.GetFiles(engineModulesPath, "*.dll"));
			}

			CryCilAssemblies.AddRange(from file in gameModules.Concat(cryEngineModules)
									  where AssemblyExtras.IsAssembly(file)
									  select Assembly.Load(AssemblyName.GetAssemblyName(file)));

			// Load and compile the solution.
			OnCompilationStarted();
			try
			{
				string solutionPath = Path.Combine(DirectoryStructure.ContentFolder,
												   "Code", "Solutions", "CryCilCode.sln");

				if (!CodeSolution.Load(solutionPath))
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
			Console.WriteLine("Proceeding to stage-based initialization.");

			ProceedWithInitializationStages();

			Console.WriteLine("{0} CryCIL-specific assemblies are loaded.", CryCilAssemblies.Count);
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
				var initializationTypesEnum =
					from assembly in CryCilAssemblies
					from type in assembly.GetTypes()
					where type.ContainsAttribute<InitializationClassAttribute>()
					select type;
				var initializationTypes = initializationTypesEnum.ToList();

				// Get the functions that represent initialization stages.
				BindingFlags publicNonPublic = BindingFlags.NonPublic | BindingFlags.Public;
				var initFuncs = from initializationType in initializationTypes
								from method in initializationType.GetMethods(publicNonPublic)
								let pars = method.GetParameters()
								where method.ContainsAttribute<InitializationStageAttribute>() &&
									  method.IsStatic &&
									  pars.Length == 1 &&
									  pars[0].ParameterType == typeof(int)
								select new
								{
									FunctionDelegate = method.CreateDelegate<InitializationStageFunction>(),
									StageIndexes = (from a in method.GetAttributes<InitializationStageAttribute>()
													select a.StageIndex).ToArray()
								};

				var initializationFunctions = initFuncs.ToList();

				// Add native stages to the list.
				initializationFunctions.Add(new
				{
					FunctionDelegate = new InitializationStageFunction(OnInitializationStageBind),
					StageIndexes = GetSubscribedStagesBind()
				});

				Console.WriteLine("Compiling data about initialization stages.");
				
				// Switch keys and values in the function map.
				for (int i = 0; i < initializationFunctions.Count; i++)
				{
					var stageFuncObj = initializationFunctions[i];
					for (int j = 0; j < stageFuncObj.StageIndexes.Length; j++)
					{
						int stageIndex = stageFuncObj.StageIndexes[j];
						if (stages.ContainsKey(stageIndex))
						{
							stages[stageIndex] += stageFuncObj.FunctionDelegate;
						}
						else
						{
							stages.Add(stageIndex, stageFuncObj.FunctionDelegate);
						}
					}
				}

				Console.WriteLine("Commencing initialization stages.");

				// Now invoke everything.
				foreach (int key in stages.Keys)
				{
					OnInitializationStageStarted(key); //
					stages[key](key); // Yep, it's that simple.
					OnInitializationStageFinished(key); //
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