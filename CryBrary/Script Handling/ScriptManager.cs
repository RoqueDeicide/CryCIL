﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using CryEngine.Actors;
using CryEngine.Annotations;
using CryEngine.Async;
using CryEngine.Entities;
using CryEngine.Extensions;
using CryEngine.Native;
using CryEngine.RunTime.Async;
using CryEngine.RunTime.Serialization;
using CryEngine.Sandbox;
using CryEngine.Testing;
using CryEngine.Testing.Internals;
using CryEngine.Utilities;
using CryEngine.Flowgraph;

namespace CryEngine.Initialization
{
	public class ScriptManager
	{
		public ScriptManager(bool initialLoad = true, string configPath = "")
		{
			ProjectSettings.ConfigFolder = configPath;

			Instance = this;

			Scripts = new List<CryScript>();

			PluginTypes = new Dictionary<ICryMonoPlugin, IEnumerable<Type>>();

			ProcessedAssemblies = new List<Assembly>();

			var tempDirectory = ProjectSettings.TempDirectory;
			if (!Directory.Exists(tempDirectory))
				Directory.CreateDirectory(tempDirectory);
			else
			{
				try
				{
					Directory.GetFiles(tempDirectory)
							 .Where(filePath => Path.GetExtension(filePath) != ".scriptdump")
							 .ForEach(File.Delete);
				}
				catch (UnauthorizedAccessException)
				{
				}
			}

#if !UNIT_TESTING
			TestManager.Init();
#endif

			if (initialLoad)
				RegisterInternalTypes();

			Formatter = new CrySerializer();
		}

		public ScriptReloadResult Initialize(bool initialLoad)
		{
			var result = ScriptReloadResult.Success;

			var exception = LoadPlugins(initialLoad);
			if (exception != null)
			{
				var scriptReloadMessage = new ScriptReloadMessage(exception, !initialLoad);
				scriptReloadMessage.ShowDialog();

				result = scriptReloadMessage.Result;
			}

			return result;
		}
		// Encapsulates extra data that is saved and loaded.
		private class ScriptManagerData : CryScriptInstance
		{
			public ScriptManagerData()
			{
				Input = new SerializableInput();
			}

			public SerializableInput Input { get; private set; }

			public Dictionary<string, ConsoleCommandDelegate> ConsoleCommands { get; set; }
			public List<CVar> ConsoleVariables { get; set; }

			public EntityId GameRulesId { get; set; }

			public int LastScriptId { get; set; }
		}

		[UsedImplicitly]
		private void Serialize()
		{
			var data = new ScriptManagerData
			{
				LastScriptId = this.LastScriptId,
				Input =
				{
					ActionmapEvents = Input.ActionmapEvents,
					KeyEvents = Input.KeyEventsInvocationList,
					MouseEvents = Input.MouseEventsInvocationList
				}
			};

			if (GameRules.Current != null)
				data.GameRulesId = GameRules.Current.Id;
			else
				data.GameRulesId = -1;

			data.ConsoleCommands = ConsoleCommand.Commands;
			data.ConsoleVariables = CVar.CVars;

			data.ConsoleVariables.RemoveAll(cvar =>
			{
				if (cvar is ByRefCVar)
				{
					Native.ConsoleInterop.UnregisterCVar(cvar.Name, true);

					return true;
				}

				return false;
			});

			AddScriptInstance(data, ScriptType.CryScriptInstance);

			using (var stream = File.Create(SerializedScriptsFile))
				Formatter.Serialize(stream, Scripts);
		}

		[UsedImplicitly]
		private void Deserialize()
		{
			// Load up the data.
			using (var stream = File.Open(SerializedScriptsFile, FileMode.Open))
				Scripts = Formatter.Deserialize(stream) as List<CryScript>;
			// Delete the file, since data is loaded.
			File.Delete(SerializedScriptsFile);
			// Get the additional data.
			var data = Find<ScriptManagerData>(ScriptType.CryScriptInstance, x => true);

			LastScriptId = data.LastScriptId;
			// Restore input.
			Input.ActionmapEvents = data.Input.ActionmapEvents;

			if (data.Input.KeyEvents != null)
			{
				foreach (var keyDelegate in data.Input.KeyEvents)
					Input.KeyEvents += keyDelegate as KeyEventDelegate;
			}

			if (data.Input.MouseEvents != null)
			{
				foreach (var mouseDelegate in data.Input.MouseEvents)
					Input.MouseEvents += mouseDelegate as MouseEventDelegate;
			}
			// Restore game rules.
			if (data.GameRulesId != -1)
				GameRules.Current = Entity.Get(data.GameRulesId) as GameRules;
			// Restore console commands and variables.
			ConsoleCommand.Commands = data.ConsoleCommands;
			CVar.CVars = data.ConsoleVariables;
			// Delete extra data object.
			RemoveInstance(data.ScriptId, ScriptType.CryScriptInstance);
		}

		/// <summary>
		/// Called from GameDll to register the flow nodes.
		/// </summary>
		public void RegisterFlownodes()
		{
			// These have to be registered later due to the flow system being initialized late.
			// Note: Flow nodes have to be registered from IGame::RegisterGameFlownodes in order to
			//       be usable from within UI graphs. (Use IMonoScriptSystem::RegisterFlownodes)
			ForEachScript(ScriptType.FlowNode, x => FlowNode.Register(x.ScriptName));
		}

		public void OnRevert()
		{
			// Revert to previous state
		}
		// Creates an xml file that contains a bunch of assembly references.
		[UsedImplicitly]
		private void PopulateAssemblyLookup()
		{
#if !RELEASE
			// Doesn't exist when unit testing
			if (Directory.Exists(ProjectSettings.MonoFolder))
			{
				using
				(
					XmlWriter writer = XmlWriter.Create(Path.Combine(ProjectSettings.MonoFolder, "assemblylookup.xml"))
				)
				{
					writer.WriteStartDocument();
					writer.WriteStartElement("AssemblyLookupTable");

					var gacFolder = Path.Combine(ProjectSettings.MonoFolder, "lib", "mono", "gac");
					// For each assembly in GAC folder.
					foreach (var assembly in from assemblyLocation in Directory.GetFiles(gacFolder, "*.dll", SearchOption.AllDirectories)
											 let separator = new[] { "__" }
											 let splitParentDir = Directory.GetParent(assemblyLocation)
																		   .Name.Split(separator, StringSplitOptions.RemoveEmptyEntries)
											 select Assembly.Load
													(
														Path.GetFileName(assemblyLocation)
														+
														string.Format
														(
															", Version={0}, Culture=neutral, PublicKeyToken={1}",
															splitParentDir.ElementAt(0),
															splitParentDir.ElementAt(1)
														)
													)
							)
					{
						// Write information.
						writer.WriteStartElement("Assembly");
						writer.WriteAttributeString("name", assembly.FullName);

						foreach (var nameSpace in assembly.GetTypes().Select(t => t.Namespace).Distinct())
						{
							writer.WriteStartElement("Namespace");
							writer.WriteAttributeString("name", nameSpace);
							writer.WriteEndElement();
						}

						writer.WriteEndElement();
					}

					writer.WriteEndElement();
					writer.WriteEndDocument();
				}
			}
#endif
		}

		private void RegisterInternalTypes()
		{
			CryScript script;
			if (CryScript.TryCreate(typeof(NativeActor), out script))
				Scripts.Add(script);

			if (CryScript.TryCreate(typeof(NativeEntity), out script))
			{
				var entityRegistrationParams = new EntityRegistrationParams
				{
					name = script.ScriptName,
					flags = EntityClassFlags.Default | EntityClassFlags.Invisible
				};

#if !UNIT_TESTING
				EntityInterop.RegisterEntityClass(entityRegistrationParams);
#endif

				Scripts.Add(script);
			}
		}

		[UsedImplicitly]
		private void ProcessWaitingScripts(bool initialLoad)
		{
			bool hasDefaultGameRules = false;
			foreach (var pluginPair in PluginTypes)
			{
				ICryMonoPlugin plugin = pluginPair.Key;

				foreach (Type type in pluginPair.Value)
				{
					Type type1 = type;
					var script = FindScript(ScriptType.Any, x => x.Type == type1);
					if (script == null)
					{
						if (!CryScript.TryCreate(type, out script))
							continue;
					}

					script.RegistrationParams = plugin.GetRegistrationParams(script.ScriptType, type);

					if (script.Registered) continue;

					if (script.RegistrationParams == null)
						continue;

					// Contain types that can only be registered at startup here.
					if (initialLoad)
					{
						if (script.RegistrationParams is ActorRegistrationParams)
						{
							var registrationParams = (ActorRegistrationParams)script.RegistrationParams;

							ActorInterop.RegisterActorClass(script.ScriptName, script.Type.Implements(typeof(NativeActor)),
																  registrationParams.isAI);
						}
						else if (script.RegistrationParams is EntityRegistrationParams)
						{
							var registrationParams = (EntityRegistrationParams)script.RegistrationParams;

							if (registrationParams.name == null)
								registrationParams.name = script.ScriptName;
							if (registrationParams.category == null)
								registrationParams.category = "Default";

							EntityInterop.RegisterEntityClass(registrationParams);

							script.RegistrationParams = registrationParams;
						}
					}

					if (script.RegistrationParams is GameRulesRegistrationParams)
					{
						var registrationParams = (GameRulesRegistrationParams)script.RegistrationParams;

						if (registrationParams.name == null)
							registrationParams.name = script.ScriptName;

						GameRulesInterop.RegisterGameMode(registrationParams.name);

						if (registrationParams.defaultGamemode || !hasDefaultGameRules)
						{
							GameRulesInterop.SetDefaultGameMode(registrationParams.name);

							hasDefaultGameRules = true;
						}

						script.RegistrationParams = registrationParams;
					}
					else if (script.RegistrationParams is FlowNodeRegistrationParams)
					{
						var registrationParams = (FlowNodeRegistrationParams)script.RegistrationParams;

						if (registrationParams.name == null)
							registrationParams.name = script.ScriptName;
						if (registrationParams.category == null)
							registrationParams.category = script.Type.Namespace;
						if (registrationParams.filter == 0)
							registrationParams.filter = FlowNodeFilter.Approved;

						script.RegistrationParams = registrationParams;

						script.ScriptName = registrationParams.category + ":" + registrationParams.name;
					}
					else if (script.RegistrationParams is EntityFlowNodeRegistrationParams)
					{
						var registrationParams = (EntityFlowNodeRegistrationParams)script.RegistrationParams;

						script.ScriptName = "entity" + ":" + registrationParams.entityName;
					}

					script.Registered = true;
					this.Scripts.Add(script);
				}
			}
		}

		// ReSharper disable UnusedParameter.Local
		private Exception LoadPlugins(bool initialLoad)
		// ReSharper restore UnusedParameter.Local
		{
			var pluginsDirectory = ProjectSettings.PluginsFolder;
			if (!Directory.Exists(pluginsDirectory))
				return null;

			foreach (var directory in Directory.GetDirectories(pluginsDirectory))
			{
				var compilerDll = Path.Combine(directory, "Compiler.dll");
				if (File.Exists(compilerDll))
				{
					var assembly = LoadAssembly(compilerDll);
					if (assembly == null)
						continue;

					ICryMonoPlugin compiler = null;
					var compilerType = assembly.GetTypes().FirstOrDefault(x => x.Implements<ICryMonoPlugin>());
					if (compilerType != null)
					{
						Debug.LogAlways("        Initializing CryMono plugin: {0}...", compilerType.Name);

						compiler = Activator.CreateInstance(compilerType) as ICryMonoPlugin;
					}

					if (compiler != null)
					{
						this.PluginTypes.Add(compiler, null);

						var assemblyPaths = Directory.GetFiles(directory, "*.dll", SearchOption.AllDirectories);
						var assemblies =
							assemblyPaths.Where(assemblyPath => assemblyPath != compilerDll)
										 .Select(this.LoadAssembly)
										 .Where(foundAssembly => foundAssembly != null)
										 .ToList();

						try
						{
							this.PluginTypes[compiler] = compiler.GetTypes(assemblies);
						}
						catch (Exception ex)
						{
							return ex;
						}
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Loads a C# assembly by location, creates a shadow-copy and generates debug database (mdb).
		/// </summary>
		/// <param name="assemblyPath"></param>
		public Assembly LoadAssembly(string assemblyPath)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (assemblyPath == null)
				throw new ArgumentNullException("assemblyPath");
			if (assemblyPath.Length < 1)
				throw new ArgumentException("string cannot be empty!", "assemblyPath");
#endif

			var tempDirectory = ProjectSettings.TempDirectory;
			var newPath = Path.Combine(tempDirectory, Path.GetFileName(assemblyPath));

			TryCopyFile(assemblyPath, ref newPath);

#if !RELEASE
			if (CVar.Get("mono_generateMdbIfPdbIsPresent").IVal != 0)
			{
				GenerateDebugDatabaseForAssembly(assemblyPath);

				var mdbFile = assemblyPath + ".mdb";
				if (File.Exists(mdbFile)) // success
				{
					var newMdbPath = Path.Combine(tempDirectory, Path.GetFileName(mdbFile));
					TryCopyFile(mdbFile, ref newMdbPath);
				}
			}
#endif

			var assembly = Assembly.LoadFrom(newPath);
			if (ProcessedAssemblies.Any(x => x.FullName == assembly.FullName))
				return null;

			ProcessedAssemblies.Add(assembly);
			return assembly;
		}

		private void TryCopyFile(string currentPath, ref string newPath, bool overwrite = true)
		{
			if (!File.Exists(newPath))
				File.Copy(currentPath, newPath, overwrite);
			else
			{
				try
				{
					File.Copy(currentPath, newPath, overwrite);
				}
				catch (Exception ex)
				{
					if (ex is UnauthorizedAccessException || ex is IOException)
					{
						newPath = Path.ChangeExtension(newPath, "_" + Path.GetExtension(newPath));
						TryCopyFile(currentPath, ref newPath);
					}
					else
						throw;
				}
			}
		}

		public void GenerateDebugDatabaseForAssembly(string assemblyPath)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (assemblyPath == null)
				throw new ArgumentNullException("assemblyPath");
			if (assemblyPath.Length < 1)
				throw new ArgumentException("string cannot be empty!", "assemblyPath");
#endif

			if (File.Exists(Path.ChangeExtension(assemblyPath, "pdb")))
			{
				var pdb2MdbDllPath = Path.Combine(ProjectSettings.MonoFolder, "bin", "pdb2mdb.dll");
				if (!File.Exists(pdb2MdbDllPath))
					return;

				var assembly = Assembly.LoadFrom(pdb2MdbDllPath);
				var driver = assembly.GetType("Driver");
				var convertMethod = driver.GetMethod("Convert", BindingFlags.Static | BindingFlags.Public);

				object[] args = { assemblyPath };
				convertMethod.Invoke(null, args);
			}
		}

		/// <summary>
		/// Called once per frame.
		/// </summary>
		public void OnUpdate(float frameTime, float frameStartTime, float asyncTime, float frameRate, float timeScale)
		{
			Time.Set(frameTime, frameStartTime, asyncTime, frameRate, timeScale);

			Awaiter.Instance.OnUpdate(frameTime);
			// Invoke OnUpdate on every script object that is a script instance and wants to receive updates.
			(
				from script in this.Scripts
				where script.ScriptType.HasFlag(ScriptType.CryScriptInstance) && script.ScriptInstances != null
				select script.ScriptInstances
			)
			.ForEach
			(
				instanceList => instanceList.Where(inst => inst.ReceiveUpdates)
											.ForEach(inst => inst.OnUpdate())
			);
		}
		/// <summary>
		/// Instantiates a script using its name and interface.
		/// </summary>
		/// <param name="scriptName">             Name of the script to create.</param>
		/// <param name="scriptType">             Type of script to create.</param>
		/// <param name="cryScriptInstanceHandle">Pointer to unmanaged object for this instance.</param>
		/// <param name="constructorParams">      Parameters to pass to the constructor.</param>
		/// <param name="throwOnFail">            
		/// Indication whether exception must be thrown, if error occurs.
		/// </param>
		/// <returns>New instance scriptId or -1 if instantiation failed.</returns>
		public CryScriptInstance CreateScriptInstance(string scriptName,
													  ScriptType scriptType,
													  IntPtr cryScriptInstanceHandle,
													  object[] constructorParams = null,
													  bool throwOnFail = true)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (scriptName == null)
				throw new ArgumentNullException("scriptName");
			if (scriptName.Length < 1)
				throw new ArgumentException("string cannot be empty!", "scriptName");
			if (!Enum.IsDefined(typeof(ScriptType), scriptType))
				throw new ArgumentException(string.Format("scriptType: value {0} was not defined in the enum", scriptType));
#endif

			var script = Scripts.FirstOrDefault(x => (x.ScriptType & scriptType) == scriptType && x.ScriptName.Equals(scriptName));
			if (script == null)
			{
				if (throwOnFail)
					throw new ScriptNotFoundException(string.Format("Script {0} of ScriptType {1} could not be found.",
																	scriptName,
																	scriptType));
				return null;
			}

			var instance = CreateScriptInstance(script, constructorParams, throwOnFail);
			instance.InstanceHandle = cryScriptInstanceHandle;

			return instance;
		}

		public CryScriptInstance CreateScriptInstance(CryScript script, object[] constructorParams = null,
													  bool throwOnFail = true)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (script == null)
				throw new ArgumentNullException("script");
#endif

			var scriptInstance = Activator.CreateInstance(script.Type, constructorParams) as CryScriptInstance;
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (scriptInstance == null)
			{
				if (throwOnFail)
					throw new ArgumentException("Failed to create instance, make sure type derives from CryScriptInstance",
												"script");
				return null;
			}
#endif
			AddScriptInstance(script, scriptInstance);

			scriptInstance.Script = script;

			return scriptInstance;
		}

		public void AddScriptInstance(CryScriptInstance instance, ScriptType scriptType)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (instance == null)
				throw new ArgumentNullException("instance");
			if (!Enum.IsDefined(typeof(ScriptType), scriptType))
				throw new ArgumentException(string.Format("scriptType: value {0} was not defined in the enum", scriptType));
#endif

			var script = FindScript(scriptType, x => x.Type == instance.GetType());
			if (script == null)
			{
				if (CryScript.TryCreate(instance.GetType(), out script))
					Scripts.Add(script);
				else
					return;
			}

			AddScriptInstance(script, instance);
		}

		private void AddScriptInstance(CryScript script, CryScriptInstance instance, int scriptId = -1)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (script == null)
				throw new ArgumentException("script");
#endif

			instance.ScriptId = (scriptId != -1) ? scriptId : LastScriptId++;

			if (script.ScriptInstances == null)
				script.ScriptInstances = new List<CryScriptInstance>();

			script.ScriptInstances.Add(instance);
		}

		public void ReplaceScriptInstance(CryScriptInstance newInstance, int scriptId, ScriptType scriptType)
		{
			RemoveInstance(scriptId, scriptType);

			var script = FindScript(scriptType, x => x.Type == newInstance.GetType());
			if (script == null)
			{
				if (CryScript.TryCreate(newInstance.GetType(), out script))
					Scripts.Add(script);
				else
					return;
			}

			AddScriptInstance(script, newInstance, scriptId);
		}

		public void RemoveInstance(int instanceId, ScriptType scriptType)
		{
			RemoveInstances<CryScriptInstance>(scriptType, x => x.ScriptId == instanceId);
		}
		/// <summary>
		/// Deletes script instances that have specified flags and satisfy a condition.
		/// </summary>
		/// <typeparam name="T">Type of scripts to remove.</typeparam>
		/// <param name="scriptType">A set of flags that must be set on the insatance to make it valid for deletion.</param>
		/// <param name="match">Predicate that represents a condition every deleted instance must satisfy.</param>
		/// <returns>Number of deleted instances.</returns>
		public int RemoveInstances<T>(ScriptType scriptType, Predicate<T> match) where T : CryScriptInstance
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (!Enum.IsDefined(typeof(ScriptType), scriptType))
				throw new ArgumentException(string.Format("scriptType: value {0} was not defined in the enum", scriptType));
#endif
			return
			(
				from script in this.Scripts
					where script.ScriptType.HasFlag(scriptType) && script.ScriptInstances != null
					select script.ScriptInstances.RemoveAll
					(
						x =>
						{
							if (match(x as T))
							{
								x.OnDestroyedInternal();
								return true;
							}

							return false;
						}
					)
			).Sum();
		}
		/// <summary>
		/// Deletes script instances that have specified flags and satisfy a condition.
		/// </summary>
		/// <param name="scriptType">A set of flags that must be set on the insatance to make it valid for deletion.</param>
		/// <param name="match">Predicate that represents a condition every deleted instance must satisfy.</param>
		/// <returns>Number of deleted instances.</returns>
		public int RemoveInstances(ScriptType scriptType, Predicate<CryScriptInstance> match)
		{
			return RemoveInstances<CryScriptInstance>(scriptType, match);
		}

		public CryScriptInstance GetScriptInstanceById(int id, ScriptType scriptType)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (id == 0)
				throw new ArgumentException("instance id cannot be 0!");
#endif

			return Find<CryScriptInstance>(scriptType, x => x.ScriptId == id);
		}
		#region Linq statements
		/// <summary>
		/// Finds first <see cref="CryScript"/> object that has specified flags set and satisfies a condition.
		/// </summary>
		/// <param name="scriptType">A set of flags that must be set on the insatance to make it valid for search.</param>
		/// <param name="predicate">Predicate that represents a condition seeked instance must satisfy.</param>
		/// <returns>Last element of type <see cref="CryScript"/> that has <paramref name="scriptType"/> flags set, and satisfies condition defined by <paramref name="predicate"/>.</returns>
		public CryScript FindScript(ScriptType scriptType, Func<CryScript, bool> predicate)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (!Enum.IsDefined(typeof(ScriptType), scriptType))
				throw new ArgumentException(string.Format("scriptType: value {0} was not defined in the enum", scriptType));
#endif

			return Scripts.FirstOrDefault(x => x.ScriptType.HasFlag(scriptType) && predicate(x));
		}
		/// <summary>
		/// Invokes a function on each script object that has specified flags set.
		/// </summary>
		/// <param name="scriptType">A set of flags that must be set on the insatance to make it valid for operation.</param>
		/// <param name="action">Function that takes <see cref="CryScript"/> object as parameter that is invoked on every object that has <paramref name="scriptType"/> flags set.</param>
		public void ForEachScript(ScriptType scriptType, Action<CryScript> action)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (!Enum.IsDefined(typeof(ScriptType), scriptType))
				throw new ArgumentException(string.Format("scriptType: value {0} was not defined in the enum", scriptType));
#endif

			Scripts.ForEach(x =>
			{
				if ((x.ScriptType & scriptType) == scriptType)
					action(x);
			});
		}
		/// <summary>
		/// Invokes a function on each script object that has specified flags set.
		/// </summary>
		/// <param name="scriptType">A set of flags that must be set on the insatance to make it valid for operation.</param>
		/// <param name="action">Function that takes <see cref="CryScriptInstance"/> object as parameter that is invoked on every object that has <paramref name="scriptType"/> flags set.</param>
		public void ForEach(ScriptType scriptType, Action<CryScriptInstance> action)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (!Enum.IsDefined(typeof(ScriptType), scriptType))
				throw new ArgumentException(string.Format("scriptType: value {0} was not defined in the enum", scriptType));
#endif

			ForEachScript(scriptType, script =>
			{
				if (script.ScriptInstances != null)
					script.ScriptInstances.ForEach(action);
			});
		}
		/// <summary>
		/// Finds last occurrence of the script instance that matches given flags and satisfies a condition.
		/// </summary>
		/// <typeparam name="T">Type of the script instance to find.</typeparam>
		/// <param name="scriptType">A set of flags that must be set on the insatance to make it valid for search.</param>
		/// <param name="predicate">Predicate that represents a condition seeked instance must satisfy.</param>
		/// <returns>Last element of type <typeparamref name="T"/> that has <paramref name="scriptType"/> flags set, and satisfies condition defined by <paramref name="predicate"/>.</returns>
		public T Find<T>(ScriptType scriptType, Func<T, bool> predicate) where T : CryScriptInstance
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (!Enum.IsDefined(typeof(ScriptType), scriptType))
				throw new ArgumentException(string.Format("scriptType: value {0} was not defined in the enum", scriptType));
#endif

			T scriptInstance = null;

			ForEachScript(scriptType, script =>
			{
				if (script.ScriptInstances != null && script.Type.ImplementsOrEquals<T>())
				{
					var instance = script.ScriptInstances.Find(x => !x.IsDestroyed && predicate(x as T)) as T;
					if (instance != null)
					{
						scriptInstance = instance;
					}
				}
			});

			return scriptInstance;
		}
		#endregion
		/// <summary>
		/// Last assigned ScriptId, next = + 1
		/// </summary>
		public int LastScriptId = 1;

		public bool IgnoreExternalCalls { get; set; }

		internal List<CryScript> Scripts { get; set; }

		/// <summary>
		/// Temporary storage for scripts before they are registered.
		/// </summary>
		private Dictionary<ICryMonoPlugin, IEnumerable<Type>> PluginTypes { get; set; }

		private List<Assembly> ProcessedAssemblies { get; set; }

		[UsedImplicitly]
		private AppDomain ScriptDomain { get; set; }
		private IFormatter Formatter { get; set; }

		private static string SerializedScriptsFile
		{
			get { return Path.Combine(ProjectSettings.TempDirectory, "CompiledScripts.scriptdump"); }
		}

		public static ScriptManager Instance;
	}
	/// <summary>
	/// Represents an exception that is thrown to cancel a certain operation.
	/// </summary>
	[SerializableAttribute]
	public class ScriptNotFoundException : Exception
	{
		/// <summary>
		/// Creates a default instance of <see cref="ScriptNotFoundException"/> class.
		/// </summary>
		public ScriptNotFoundException()
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="ScriptNotFoundException"/> class with specified message.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		public ScriptNotFoundException(string message)
			: base(message)
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="ScriptNotFoundException"/> class with specified
		/// message and exception object that caused new one to be created.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		/// <param name="inner">  Exception that caused a new one to be created.</param>
		public ScriptNotFoundException(string message, Exception inner)
			: base(message, inner)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="ScriptNotFoundException"/> class with
		/// serialized data.
		/// </summary>
		/// <param name="info">   The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected ScriptNotFoundException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}