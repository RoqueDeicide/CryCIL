using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

using CryEngine.Async;
using CryEngine.Extensions;
using CryEngine.Native;
using CryEngine.Sandbox;
using CryEngine.Testing;
using CryEngine.Testing.Internals;

using CryEngine.Serialization;
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
					foreach (var file in Directory.GetFiles(tempDirectory))
                    {
                        if (!Path.HasExtension(".scriptdump"))
                            File.Delete(file);
                    }
                }
                catch (UnauthorizedAccessException) { }
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
            if(exception != null)
            {
                var scriptReloadMessage = new ScriptReloadMessage(exception, !initialLoad);
                scriptReloadMessage.ShowDialog();

                result = scriptReloadMessage.Result;
            }

            return result;
        }

        class ScriptManagerData : CryScriptInstance
        {
            public ScriptManagerData()
            {
                Input = new SerializableInput();
            }

            public SerializableInput Input { get; set; }

			public Dictionary<string, ConsoleCommandDelegate> ConsoleCommands { get; set; }
			public List<CVar> ConsoleVariables { get; set; }

            public EntityId GameRulesId { get; set; }

            public int LastScriptId { get; set; }
        }

        void Serialize()
        {
            var data = new ScriptManagerData();
            data.LastScriptId = LastScriptId;

            data.Input.ActionmapEvents = Input.ActionmapEvents;

            data.Input.KeyEvents = Input.KeyEventsInvocationList;
            data.Input.MouseEvents = Input.MouseEventsInvocationList;

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
						NativeCVarMethods.UnregisterCVar(cvar.Name, true);

						return true;
					}

					return false;
				});

            AddScriptInstance(data, ScriptType.CryScriptInstance);

            using (var stream = File.Create(SerializedScriptsFile))
                Formatter.Serialize(stream, Scripts);
        }

        void Deserialize()
        {
            using (var stream = File.Open(SerializedScriptsFile, FileMode.Open))
                Scripts = Formatter.Deserialize(stream) as List<CryScript>;

            File.Delete(SerializedScriptsFile);

            var data = Find<ScriptManagerData>(ScriptType.CryScriptInstance, x => { return true; });

            LastScriptId = data.LastScriptId;

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

            if (data.GameRulesId != -1)
                GameRules.Current = Entity.Get(data.GameRulesId) as GameRules;

			ConsoleCommand.Commands = data.ConsoleCommands;
			CVar.CVars = data.ConsoleVariables;

            RemoveInstance(data.ScriptId, ScriptType.CryScriptInstance);
        }

        /// <summary>
        /// Called from GameDll
        /// </summary>
        public void RegisterFlownodes()
        {
            // These have to be registered later due to the flow system being initialized late.
            // Note: Flow nodes have to be registered from IGame::RegisterGameFlownodes in order to be usable from within UI graphs. (Use IMonoScriptSystem::RegisterFlownodes)
            ForEachScript(ScriptType.FlowNode, x =>
                {
                    FlowNode.Register(x.ScriptName);
                });
        }

        public void OnRevert()
        {
            // Revert to previous state
        }

        void PopulateAssemblyLookup()
        {
#if !RELEASE
            // Doesn't exist when unit testing
			if (Directory.Exists(ProjectSettings.MonoFolder))
            {
				using (XmlWriter writer = XmlWriter.Create(Path.Combine(ProjectSettings.MonoFolder, "assemblylookup.xml")))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("AssemblyLookupTable");

					var gacFolder = Path.Combine(ProjectSettings.MonoFolder, "lib", "mono", "gac");
                    foreach (var assemblyLocation in Directory.GetFiles(gacFolder, "*.dll", SearchOption.AllDirectories))
                    {
                        var separator = new[] { "__" };
                        var splitParentDir = Directory.GetParent(assemblyLocation).Name.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                        var assembly = Assembly.Load(Path.GetFileName(assemblyLocation) + string.Format(", Version={0}, Culture=neutral, PublicKeyToken={1}", splitParentDir.ElementAt(0), splitParentDir.ElementAt(1)));

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

        void RegisterInternalTypes()
        {
            CryScript script;
            if (CryScript.TryCreate(typeof(NativeActor), out script))
                Scripts.Add(script);

            if (CryScript.TryCreate(typeof(NativeEntity), out script))
            {
                var entityRegistrationParams = new EntityRegistrationParams();

                entityRegistrationParams.name = script.ScriptName;
                entityRegistrationParams.flags = EntityClassFlags.Default | EntityClassFlags.Invisible;

#if !UNIT_TESTING
                NativeEntityMethods.RegisterEntityClass(entityRegistrationParams);
#endif

                Scripts.Add(script);
            }
        }

        void ProcessWaitingScripts(bool initialLoad)
        {
            bool hasDefaultGameRules = false;
            foreach (var pluginPair in PluginTypes)
            {
                ICryMonoPlugin plugin = pluginPair.Key;

                foreach (Type type in pluginPair.Value)
                {
                    var script = FindScript(ScriptType.Any, x => x.Type == type);
                    if (script == null)
                    {
                        if (!CryScript.TryCreate(type, out script))
                            continue;
                    }

                    script.RegistrationParams = plugin.GetRegistrationParams(script.ScriptType, type);

                    if (!script.Registered)
                    {
                        if (script == null || script.RegistrationParams == null)
                            continue;

                        // Contain types that can only be registered at startup here.
                        if (initialLoad)
                        {
                            if (script.RegistrationParams is ActorRegistrationParams)
                            {
                                var registrationParams = (ActorRegistrationParams)script.RegistrationParams;

                                NativeActorMethods.RegisterActorClass(script.ScriptName, script.Type.Implements(typeof(NativeActor)), registrationParams.isAI);
                            }
                            else if (script.RegistrationParams is EntityRegistrationParams)
                            {
                                var registrationParams = (EntityRegistrationParams)script.RegistrationParams;

                                if (registrationParams.name == null)
                                    registrationParams.name = script.ScriptName;
                                if (registrationParams.category == null)
                                    registrationParams.category = "Default";

                                NativeEntityMethods.RegisterEntityClass(registrationParams);

                                script.RegistrationParams = registrationParams;
                            }
                        }

                        if (script.RegistrationParams is GameRulesRegistrationParams)
                        {
                            var registrationParams = (GameRulesRegistrationParams)script.RegistrationParams;

                            if (registrationParams.name == null)
                                registrationParams.name = script.ScriptName;

                            NativeGameRulesMethods.RegisterGameMode(registrationParams.name);

                            if (registrationParams.defaultGamemode || !hasDefaultGameRules)
                            {
                                NativeGameRulesMethods.SetDefaultGameMode(registrationParams.name);

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
                        Scripts.Add(script);
                    }
                }
            }
        }

        Exception LoadPlugins(bool initialLoad)
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

                    var compilerType = assembly.GetTypes().First(x => x.Implements<ICryMonoPlugin>());
                    Debug.LogAlways("        Initializing CryMono plugin: {0}...", compilerType.Name);

                    var compiler = Activator.CreateInstance(compilerType) as ICryMonoPlugin;

                    PluginTypes.Add(compiler, null);

                    var assemblyPaths = Directory.GetFiles(directory, "*.dll", SearchOption.AllDirectories);
                    var assemblies = new List<Assembly>();

                    foreach (var assemblyPath in assemblyPaths)
                    {
                        if (assemblyPath != compilerDll)
                        {
                            var foundAssembly = LoadAssembly(assemblyPath);
                            if(foundAssembly != null)
                                assemblies.Add(foundAssembly);
                        }
                    }

                    try
                    {
                        PluginTypes[compiler] = compiler.GetTypes(assemblies);
                    }
                    catch (Exception ex)
                    {
                        return ex;
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

            var assembly =  Assembly.LoadFrom(newPath);
            if (ProcessedAssemblies.Any(x => x.FullName == assembly.FullName))
                return null;

            ProcessedAssemblies.Add(assembly);
            return assembly;
        }

        void TryCopyFile(string currentPath, ref string newPath, bool overwrite = true)
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
				var assembly = Assembly.LoadFrom(Path.Combine(ProjectSettings.MonoFolder, "bin", "pdb2mdb.dll"));
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

            Scripts.ForEach(x =>
            {
                if ((x.ScriptType & ScriptType.CryScriptInstance) == ScriptType.CryScriptInstance && x.ScriptInstances != null)
                {
                    x.ScriptInstances.ForEach(instance =>
                    {
                        if (instance.ReceiveUpdates)
                            instance.OnUpdate();
                    });
                }
            });
        }

        /// <summary>
        /// Instantiates a script using its name and interface.
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="scriptType"></param>
        /// <param name="constructorParams"></param>
        /// <returns>New instance scriptId or -1 if instantiation failed.</returns>
        public CryScriptInstance CreateScriptInstance(string scriptName, ScriptType scriptType, IntPtr cryScriptInstanceHandle, object[] constructorParams = null, bool throwOnFail = true)
        {
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
            if (scriptName == null)
                throw new ArgumentNullException("scriptName");
            if (scriptName.Length < 1)
                throw new ArgumentException("string cannot be empty!", "scriptName");
            if (!Enum.IsDefined(typeof(ScriptType), scriptType))
                throw new ArgumentException(string.Format("scriptType: value {0} was not defined in the enum", scriptType));
#endif

            var script = Scripts.First(x => (x.ScriptType & scriptType) == scriptType && x.ScriptName.Equals(scriptName));
            if (script == null)
            {
                if (throwOnFail)
                    throw new ScriptNotFoundException(string.Format("Script {0} of ScriptType {1} could not be found.", scriptName, scriptType));
                else
                    return null;
            }

            var instance = CreateScriptInstance(script, constructorParams, throwOnFail);
			instance.InstanceHandle = cryScriptInstanceHandle;

			return instance;
        }

        public CryScriptInstance CreateScriptInstance(CryScript script, object[] constructorParams = null, bool throwOnFail = true)
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
                    throw new ArgumentException("Failed to create instance, make sure type derives from CryScriptInstance", "scriptName");
                else
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

        void AddScriptInstance(CryScript script, CryScriptInstance instance, int scriptId = -1)
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
        /// Locates and removes the script with the assigned scriptId.
        /// </summary>
        public int RemoveInstances<T>(ScriptType scriptType, Predicate<T> match) where T : CryScriptInstance
        {
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
            if (!Enum.IsDefined(typeof(ScriptType), scriptType))
                throw new ArgumentException(string.Format("scriptType: value {0} was not defined in the enum", scriptType));
#endif

            int numRemoved = 0;
            foreach(var script in Scripts)
            {
                if ((script.ScriptType & scriptType) == scriptType)
                {
                    if (script.ScriptInstances != null)
                    {
                        numRemoved += script.ScriptInstances.RemoveAll(x =>
                        {
                            if (match(x as T))
                            {
                                x.OnDestroyedInternal();
                                return true;
                            }

                            return false;
                        });
                    }
                }
            }

            return numRemoved;
        }

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
        public CryScript FindScript(ScriptType scriptType, Func<CryScript, bool> predicate)
        {
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
            if (!Enum.IsDefined(typeof(ScriptType), scriptType))
                throw new ArgumentException(string.Format("scriptType: value {0} was not defined in the enum", scriptType));
#endif

            return Scripts.FirstOrDefault(x => (x.ScriptType & scriptType) == scriptType && predicate(x));
        }

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
                        return;
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
        Dictionary<ICryMonoPlugin, IEnumerable<Type>> PluginTypes { get; set; }

        List<Assembly> ProcessedAssemblies { get; set; }

        AppDomain ScriptDomain { get; set; }
        IFormatter Formatter { get; set; }

        string SerializedScriptsFile { get { return Path.Combine(ProjectSettings.TempDirectory, "CompiledScripts.scriptdump"); } }

        public static ScriptManager Instance;
    }

    /// <summary>
    /// Thrown by <see cref="CryEngine.Initialization.ScriptManager"/> if a CryScript could not be located.
    /// </summary>
    [Serializable]
    public class ScriptNotFoundException : Exception
    {
        public ScriptNotFoundException(string error)
        {
            message = error;
        }

        private string message;
        public override string Message
        {
            get { return message; }
        }
    }
}