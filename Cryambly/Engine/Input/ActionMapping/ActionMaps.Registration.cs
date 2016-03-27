using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CryCil.Annotations;
using CryCil.Hashing;
using CryCil.RunTime;

namespace CryCil.Engine.Input.ActionMapping
{
	public static unsafe partial class ActionMaps
	{
		[InitializationStage((int)DefaultInitializationStages.ActionMapsRegistrationStage)]
		private static void RegisterActionMaps(int stageIndex)
		{
			AddDeviceMappings();

			// Select types that are decorated with ActionMapAttribute and put them into 2 groups: static
			// types and non-static ones.
			var types = from assembly in MonoInterface.CryCilAssemblies
						from type in assembly.GetTypes()
						where type.ContainsAttribute<ActionMapAttribute>()
						select type;

			foreach (var type in types)
			{
				var actionMapAttribute = type.GetAttribute<ActionMapAttribute>();
				string actionMapName = actionMapAttribute.Name ?? type.Name;
				bool isStatic = type.IsSealed && type.IsAbstract;

				// Create a native action map.
				var actionMap = CreateActionMap(actionMapName);

				if (!actionMap.IsValid)
				{
					string message = $"Unable to create action map \"{actionMapName}\": one already exists.";
					MonoInterface.DisplayException(new RegistrationException(message));
					continue;
				}

				BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic |
											(isStatic
												? BindingFlags.Static
												: BindingFlags.Instance);

				// Get methods and events that represent actions.
				var actionEvents = from _event in type.GetEvents(bindingFlags)
								   let attribute = _event.GetAttribute<ActionAttribute>()
								   where attribute != null &&
										 _event.EventHandlerType == typeof(InputActionHandler)
								   select new {Event = _event, Action = attribute};

				var actionMethods = from method in type.GetMethods(bindingFlags)
									let attribute = method.GetAttribute<ActionAttribute>()
									where attribute != null && !(method.IsVirtual || method.IsAbstract) &&
										  method.MatchesParameters(typeof(ActionActivationMode), typeof(float))
									select new {Method = method, Action = attribute};

				// Compile abstraction layers between events and methods and their respective actions.
				List<CryInputAction> actionsList = new List<CryInputAction>();
				List<IntPtr> eventFieldHandles = new List<IntPtr>();
				List<IntPtr> methodHandles = new List<IntPtr>();

				foreach (var @event in actionEvents)
				{
					string actionName = @event.Action.Name ?? @event.Event.Name;

					// Acquire information about a field that we will use to raise the event when action is
					// activated.
					FieldInfo eventField = type.GetField(@event.Event.Name, bindingFlags) ??
										   type.GetField($"_{@event.Event.Name}", bindingFlags);
					if (eventField == null)
					{
						string message = $"Unable to create action {actionName}: underlying event must either " +
										 $"have default implementation, or a field, named _{@event.Event.Name} " +
										 $"that contains the action handlers.";
						MonoInterface.DisplayException(new RegistrationException(message));
						continue;
					}

					// Create the underlying object for an action.
					CryInputAction cryAction;
					if (!TryCreateAction(actionMap, actionName, out cryAction))
					{
						continue;
					}

					// Register all inputs for the action.
					cryAction.AddInputs(@event.Action, @event.Event);

					IntPtr fieldHandle = eventField.FieldHandle.Value;

					actionsList.Add(cryAction);
					eventFieldHandles.Add(fieldHandle);
				}
				foreach (var method in actionMethods)
				{
					string actionName = method.Action.Name ?? method.Method.Name;

					// Create the underlying object for an action.
					CryInputAction cryAction;
					if (!TryCreateAction(actionMap, actionName, out cryAction))
					{
						continue;
					}

					// Register all inputs for the action.
					cryAction.AddInputs(method.Action, method.Method);

					actionsList.Add(cryAction);
					methodHandles.Add(method.Method.MethodHandle.Value);
				}

				var actionsArray = actionsList.ToArray();
				var fieldHandlesArray = eventFieldHandles.ToArray();
				var methodHandlesArray = methodHandles.ToArray();

				fixed (CryInputAction* actionsPtr = actionsArray)
				fixed (IntPtr* fieldsPtr = fieldHandlesArray)
				fixed (IntPtr* methodsPtr = methodHandlesArray)
				{
					CreateInternalActionMap(actionMap,
											actionsPtr, actionsList.Count,
											fieldsPtr, eventFieldHandles.Count,
											methodsPtr);
				}

				uint hash = Crc32.ComputeLowercase(actionMapName);
				AllRegisteredActionMaps.Add(hash, actionMapName);

				if (isStatic)
				{
					// Create the object that will handle this type now.
					ActionMapHandler handler = new ActionMapHandler(actionMap);

					// Instantiate the generic class that will hold the handler in the static field.
					Type delegateType = typeof(StaticActionMapDelegate<>).MakeGenericType(type);

					// Assign the handler object to the Handler field.
					delegateType.GetField("Handler").SetValue(null, handler);

					handler.Active = actionMapAttribute.AutoActive;

					AllRegisteredStaticActionMaps.Add(hash, actionMapName);
				}
				else
				{
					AllRegisteredInstanceActionMaps.Add(hash, actionMapName);
				}
			}
		}

		private static void AddDeviceMappings()
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
		}

		private static bool TryCreateAction(CryActionMap map, string name, out CryInputAction action)
		{
			action = map.CreateAction(name);
			if (!action.IsValid)
			{
				string message = $"Unable to create action {name}: one already exists.";
				MonoInterface.DisplayException(new RegistrationException(message));
				return false;
			}
			return true;
		}

		// Special class that caches the object that handles the static class.
		private static class StaticActionMapDelegate<[UsedImplicitly] ActionMapType>
		{
			[UsedImplicitly] public static ActionMapHandler Handler;
		}
	}
}