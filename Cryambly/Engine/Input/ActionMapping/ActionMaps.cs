﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CryCil.Hashing;
using CryCil.RunTime;
using CryCil.Utilities;

namespace CryCil.Engine.Input.ActionMapping
{
	/// <summary>
	/// Provides access to CryEngine Action Maps API.
	/// </summary>
	[InitializationClass]
	public static class ActionMaps
	{
		#region Fields
		private static readonly SortedList<uint, string> registeredHashes = new SortedList<uint, string>();
		private static readonly SortedList<uint, IntPtr> actions = new SortedList<uint, IntPtr>();
		#endregion
		#region Properties
		#endregion
		#region Events
		#endregion
		#region Construction
		#endregion
		#region Interface
		/// <summary>
		/// Enables an action map.
		/// </summary>
		/// <remarks>
		/// <para>Action maps need to be enabled in order to have the actions in it activated.</para>
		/// <para>All new action maps are enabled by default.</para>
		/// </remarks>
		/// <param name="actionMapName">Name of the map to enable.</param>
		public static void Enable(string actionMapName)
		{
			EnableActionMap(actionMapName, true);
		}
		/// <summary>
		/// Disables an action map.
		/// </summary>
		/// <remarks>
		/// <para>Action maps need to be enabled in order to have the actions in it activated.</para>
		/// <para>All new action maps are enabled by default.</para>
		/// </remarks>
		/// <param name="actionMapName">Name of the map to disable.</param>
		public static void Disable(string actionMapName)
		{
			EnableActionMap(actionMapName, false);
		}
		/// <summary>
		/// Saves the rebind data into the file.
		/// </summary>
		/// <param name="file">Path to the file.</param>
		/// <returns>True, if operation was successful.</returns>
		public static bool SaveRebindData(string file)
		{
			return SyncRebindDataWithFile(file, true);
		}
		/// <summary>
		/// Saves the rebind data into the Xml node.
		/// </summary>
		/// <param name="node">Object that represents the Xml node.</param>
		/// <returns>True, if operation was successful.</returns>
		public static bool SaveRebindData(CryXmlNode node)
		{
			return SyncRebindDataWithNode(node, true);
		}
		/// <summary>
		/// Loads the rebind data from the file.
		/// </summary>
		/// <param name="file">Path to the file.</param>
		/// <returns>True, if operation was successful.</returns>
		public static bool LoadRebindData(string file)
		{
			return SyncRebindDataWithFile(file, false);
		}
		/// <summary>
		/// Loads the rebind data from the Xml node.
		/// </summary>
		/// <param name="node">Object that represents the Xml node.</param>
		/// <returns>True, if operation was successful.</returns>
		public static bool LoadRebindData(CryXmlNode node)
		{
			return SyncRebindDataWithNode(node, false);
		}
		#endregion
		#region Utilities
		[InitializationStage((int)DefaultInitializationStages.ActionMapsRegistrationStage)]
		private static void RegisterActionMaps(int stageIndex)
		{
			var assemblies = MonoInterface.CryCilAssemblies;
			// Add device mappings.
			var deviceMappingAttributes = from assembly in assemblies
										  from attr in assembly.GetAttributes<DeviceMappingAttribute>()
										  select attr;
			foreach (DeviceMappingAttribute deviceMappingAttribute in deviceMappingAttributes)
			{
				AddDeviceMapping(deviceMappingAttribute.SupportedDevice);
			}
			// Form the list of action maps.
			var actionMapTypes = from assembly in assemblies
								 from type in assembly.GetTypes()
								 where type.ContainsAttribute<ActionMapAttribute>() && type.IsAbstract && type.IsSealed
								 select type;
			var actionMapTypesList = actionMapTypes.ToList();
			foreach (Type type in actionMapTypesList)
			{
				// Create the action map.
				var actionMapAttribute = type.GetAttribute<ActionMapAttribute>();
				string actionMapName = actionMapAttribute.Name ?? type.Name;
				CryActionMap actionMap = CreateActionMap(actionMapName);

				if (!actionMap.IsValid)
				{
					string message = string.Format("Action map named \"{0}\" couldn't have been created: one already exists.",
												   actionMapName);
					MonoInterface.DisplayException(new RegistrationException(message));
					continue;
				}

				const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
				var actionEvents = from eventInfo in type.GetEvents(bindingFlags)
								   where eventInfo.ContainsAttribute<ActionAttribute>()
								   select eventInfo;

				foreach (EventInfo actionEvent in actionEvents)
				{
					var actionAttribute = actionEvent.GetAttribute<ActionAttribute>();

					string actionName = actionAttribute.Name ?? actionEvent.Name;
					uint nameHash = Crc32.ComputeLowercase(actionName);
					// Check the hash.
					if (registeredHashes.ContainsKey(nameHash))
					{
						string message = string.Format
							("An action named \"{0}\" already exists its name has the same lowercase hash as {1}",
							 actionName, registeredHashes[nameHash]);
						MonoInterface.DisplayException(new RegistrationException(message));
						continue;
					}

					// Create new action.
					CryInputAction action = actionMap.CreateAction(actionName);
					if (!action.IsValid)
					{
						string message =
							string.Format("Action named \"{0}\" couldn't have been created: one already exists.",
										  actionMapName);
						MonoInterface.DisplayException(new RegistrationException(message));
						continue;
					}

					registeredHashes.Add(nameHash, actionName);
					Type declaringType = actionEvent.DeclaringType;
					// ReSharper disable PossibleNullReferenceException
					FieldInfo actionHandlerField = declaringType.GetField(actionEvent.Name, bindingFlags) ??
												   declaringType.GetField("_" + actionEvent.Name);
					// ReSharper restore PossibleNullReferenceException
					if (actionHandlerField == null)
					{
						string message =
							string.Format
								(
								 "Action named {0}, must either have default event implementation or its declaring class " +
								 "must define static field named _{1}.",
								 actionName, actionEvent.Name
								);
						MonoInterface.DisplayException(new RegistrationException(message));
						continue;
					}
					IntPtr actionEventField = GetActionEventField(actionHandlerField.FieldHandle);
					actions.Add(nameHash, actionEventField);

					// Add inputs to the action.

					// Get master input specification. All other input specifications will inherit from the
					// master.
					ActionInputSpecification masterSpec = actionAttribute.MasterSpec;
					masterSpec.Complete();

					// Register inputs that specified in the Action attribute.
					if (actionAttribute.KeyboardMouseInput != InputId.Unknown)
					{
						action.AddInput(actionAttribute.KeyboardMouseInput, masterSpec);
					}
					if (actionAttribute.XboxInput != InputId.Unknown)
					{
						action.AddInput(actionAttribute.XboxInput, masterSpec);
					}
					if (actionAttribute.OrbisInput != InputId.Unknown)
					{
						action.AddInput(actionAttribute.OrbisInput, masterSpec);
					}

					// Register inputs that are specified in InputAction attributes.
					var extraInputs = actionEvent.GetAttributes<InputActionAttribute>();
					foreach (var inputActionAttribute in extraInputs)
					{
						var extraSpec = inputActionAttribute.ExtraSpec;
						extraSpec.Complete();
						extraSpec.InheritFrom(masterSpec);
						action.AddInput(inputActionAttribute.Input, extraSpec);
					}
				}
			}
		}
		[UnmanagedThunk("Invoked from underlying framework to activate one of the actions.")]
		private static void ActivateAction(uint actionNameHash, int activationMode, float value)
		{
			IntPtr actionField;
			if (actions.TryGetValue(actionNameHash, out actionField))
			{
				InputActionHandler handler = acquireActionHandler(actionField);
				if (handler != null)
				{
					handler(registeredHashes[actionNameHash], (ActionActivationMode)activationMode, value);
				}
			}
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddDeviceMapping(SupportedInputDevices device);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetActionEventField(RuntimeFieldHandle eventInfo);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern InputActionHandler acquireActionHandler(IntPtr actionField);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryActionMap CreateActionMap(string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnableActionMap(string name, bool enable);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SyncRebindDataWithFile(string file, bool save);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SyncRebindDataWithNode(CryXmlNode node, bool save);
		#endregion
	}
}