using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CryEngine.Actors;
using CryEngine.Async;
using CryEngine.Entities;
using CryEngine.Extensions;
using CryEngine.Initialization;
using CryEngine.Logic.Flowgraph;
using CryEngine.RunTime.Async;
using CryEngine.RunTime.Compatibility;
using CryEngine.RunTime.Compilation;
using CryEngine.RunTime.Registration;
using CryEngine.Utilities;

namespace CryEngine.RunTime
{
	/// <summary>
	/// Represents an object that handles initialization of CryBrary subsystems.
	/// </summary>
	public class MonoInterface
	{
		#region Properties
		/// <summary>
		/// Gets the array of assemblies that were compiled from Code folder.
		/// </summary>
		public Assembly[] CompiledAssemblies { get; private set; }
		#endregion
		#region Construction
		/// <summary>
		/// Invoked from unmanaged code to initialize CryBrary subsystems.
		/// </summary>
		/// <param name="configurationPath">
		/// Path to configuration files and utilities.
		/// </param>
		internal MonoInterface(string configurationPath)
		{
			ProjectSettings.ConfigFolder = configurationPath;
			// Load and build the solution.
			CodeSolution.Load(Path.Combine(CryPak.CodeFolder, "Solutions", "CryMonoCode.sln"));
			this.CompiledAssemblies = CodeSolution.Build();
			// Find types that require registration for interoperability with CryEngine.
			this.RegisterCryEngineTypes();
#if DEBUG
			// Check the types that are used in marshaling for blittability.
			AppDomain.CurrentDomain.GetAssemblies()
					 .SelectMany
					 (
						 assembly =>
							 assembly.GetTypes()
									 .Where(x => x.ContainsAttribute<BlittableAttribute>())
					 )
					 .ForEach(BlitChecker.Check);
#endif
		}
		#endregion
		#region Interface
		internal void RegisterFlowNodeTypes()
		{
			FlowNodeRegister.RegisterTypes();
		}
		internal void Think
		(
			float frameDeltaTime,
			float frameStartTime,
			float asyncTime,
			float frameRate,
			float timeScale
		)
		{
			Time.Set(frameDeltaTime, frameStartTime, asyncTime, frameRate, timeScale);

			Awaiter.Instance.OnUpdate(frameDeltaTime);
		}
		#endregion
		#region Utilities
		private void RegisterCryEngineTypes()
		{
			// Get the list of types that have either EntityAttribute or ActorAttribute.
			List<Assembly> assemblies = new List<Assembly>(this.CompiledAssemblies)
			{
				Assembly.GetAssembly(typeof(MonoInterface))
			};
			List<Type> types = new List<Type>(assemblies.Count * 20);
			assemblies.ForEach
			(
				assembly =>
					types.AddRange
					(
						assembly.GetTypes()
								.Where
								(
									type => type.ContainsAttribute<EntityAttribute>() ||
											type.ContainsAttribute<ActorAttribute>() ||
											type.ContainsAttribute<GameRulesAttribute>() ||
											type.ContainsAttribute<FlowNodeAttribute>()
								)
					)
			);
			this.RegisterEntities(types);
			this.RegisterActors(types);
			this.RegisterGameRules(types);
			this.PrepareFlowNodesRegistration(types);
		}
		private void RegisterEntities(IEnumerable<Type> types)
		{
			// Register entities.
			types.Where(type => type.ContainsAttribute<EntityAttribute>())
				 .ForEach(EntityRegister.Register);
		}
		private void RegisterActors(IEnumerable<Type> types)
		{
			// Register actors.
			types
				.Where(type => type.ContainsAttribute<ActorAttribute>())
				.ForEach
				(
					type =>
						Native.ActorInterop.RegisterActorClass
						(
							type.Name,
							type.Implements<NativeActor>()
						)
				);
		}
		private void RegisterGameRules(IEnumerable<Type> types)
		{
#if DEBUG
			bool defaultSet = false;
#endif
			// Register game rule sets.
			types
				.Where(type => type.ContainsAttribute<GameRulesAttribute>())
				.ForEach
				(
					type =>
					{
						GameRulesAttribute attribute = type.GetAttribute<GameRulesAttribute>();
						Native.GameRulesInterop.RegisterGameMode(attribute.Name);
						if (attribute.Default)
						{
#if DEBUG
							if (defaultSet)
							{
								throw new RegistrationException("There are multiple game rule set types that are marked as default.");
							}
#endif
							Native.GameRulesInterop.SetDefaultGameMode(attribute.Name);
#if DEBUG
							defaultSet = true;
#endif
						}
					}
				);
		}
		private void PrepareFlowNodesRegistration(IEnumerable<Type> types)
		{
			// Prepare the list of flow node types that will be registered when
			// RegisterFlowNodes is called.
			types.Where(type => type.ContainsAttribute<FlowNodeAttribute>()).ForEach(FlowNodeRegister.Prepare);
		}
		#endregion
	}
}