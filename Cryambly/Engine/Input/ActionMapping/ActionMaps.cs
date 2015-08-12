using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CryCil.Hashing;
using CryCil.RunTime;

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
					IntPtr actionEventField = GetActionEventField(actionEvent);
					if (actionEventField == IntPtr.Zero)
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
		private static extern IntPtr GetActionEventField(EventInfo eventInfo);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern InputActionHandler acquireActionHandler(IntPtr actionField);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryActionMap CreateActionMap(string name);
		#endregion
	}
}