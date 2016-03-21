using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Annotations;
using CryCil.Engine.Logic;

namespace CryCil.RunTime.Registration
{
	/// <summary>
	/// Defines functions that register new types of flow graph nodes.
	/// </summary>
	[InitializationClass]
	public static class FlowNodeTypeRegistry
	{
		#region Fields
		// Contains references and identifiers of all successfully registered flow node types.
		[NotNull] private static readonly SortedList<ushort, Type> registeredTypes = new SortedList<ushort, Type>();
		// Contains an array of all types that have [FlowNode] attribute. This data is gathered when CryCIL
		// is initialized (which happens before flow graph is initialized) and types here will be registered
		// later.
		[CanBeNull] private static Type[] compiledTypes;
		#endregion
		#region Interface
		/// <summary>
		/// Gets the type of the flow node.
		/// </summary>
		/// <param name="id">Identifier of the type.</param>
		/// <returns>
		/// Reference to the object that represents the type if found, otherwise returns <c>null</c>.
		/// </returns>
		public static Type GetFlowNodeType(ushort id)
		{
			Type type;
			return registeredTypes.TryGetValue(id, out type)
				? type
				: null;
		}
		#endregion
		#region Utilities
		[InitializationStage((int)DefaultInitializationStages.FlowNodeRecognitionStage)]
		private static void CompileFlowNodeTypeData(int stageIndex)
		{
			var types = from assembly in MonoInterface.CryCilAssemblies
						from type in assembly.GetTypes()
						where type.ContainsAttribute<FlowNodeAttribute>() &&
							  !type.ContainsAttribute<ObsoleteAttribute>() &&
							  type.Implements<FlowNode>() &&
							  type.HasConstructor(typeof(ushort), typeof(IntPtr))
						select type;

			compiledTypes = types.ToArray();
		}
		internal static void RegisterAllTypes()
		{
			try
			{
				if (!compiledTypes.IsNullOrEmpty())
				{
					foreach (Type type in compiledTypes)
					{
						string name = type.GetAttribute<FlowNodeAttribute>().Name;
						if (name.IsNullOrWhiteSpace())
						{
							name = type.FullName.Replace('.', ':');
						}
						registeredTypes.Add(RegisterType(name), type);
					}
				}
			}
			catch (Exception ex)
			{
				// Catch everything, since this will be invoked through raw thunk.
				MonoInterface.DisplayException(ex);
			}
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ushort RegisterType(string name);
		[RawThunk("Invoked from underlying framework to delete the type from registry.")]
		private static void UnregisterType(ushort id)
		{
			try
			{
				registeredTypes.Remove(id);
			}
			catch (Exception ex)
			{
				// Catch everything, since this will be invoked through raw thunk.
				MonoInterface.DisplayException(ex);
			}
		}
		#endregion
	}
}