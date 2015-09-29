using System;
using CryCil.Engine.Input.ActionMapping;

// Sample device mappings.
#if WIN32 || Durango || Orbis
[assembly: DeviceMapping(SupportedInputDevices.KeyboardMouse)]
#endif
#if WIN32 || Durango
[assembly: DeviceMapping(SupportedInputDevices.XboxPad)]
#endif
#if WIN32 || Orbis
[assembly: DeviceMapping(SupportedInputDevices.OrbisPad)]
#endif

// You must disable this warning when defining action maps, otherwise the compiler will complain.
#pragma warning disable 67

namespace CryCil.Engine.Input.ActionMapping
{
	/// <summary>
	/// This is an example of an action map.
	/// </summary>
	/// <remarks>
	/// Because we didn't specify the name of the action map in the ActionMap attribute, its name will be
	/// taken from the class itself, so it will be called "actionmapsample1".
	/// </remarks>
	[ActionMap]
	public static class ActionMapSample1
	{
		/// <summary>
		/// This is a simple action that will be named "simpleaction" internally and will be activated when
		/// "A" key is pressed on the keyboard.
		/// </summary>
		[Action(null, ActionActivationMode.OnPress, KeyboardMouseInput = InputId.A)]
		public static event InputActionHandler SimpleAction;
	}
	/// <summary>
	/// This is another example of an action map.
	/// </summary>
	/// <remarks>
	/// When specifying the name of the action map or an action it is recommended to follow the naming
	/// scheme that is used below.
	/// </remarks>
	[ActionMap("action_map_sample2")]
	public static class ActionMapSample2
	{
		// This field must be named like this, otherwise it won't be found by the framework.
		private static InputActionHandler _SimpleAction;
		/// <summary>
		/// This action has a custom event implementation.
		/// </summary>
		[Action("simple_action_with_custom_event", ActionActivationMode.OnPress, KeyboardMouseInput = InputId.A)]
		public static event InputActionHandler SimpleAction
		{
			add
			{
				_SimpleAction += value;

				// Custom logic that made this implementation necessary.
			}
			remove
			{
				_SimpleAction = (InputActionHandler)Delegate.Remove(_SimpleAction, value);

				// Custom logic that made this implementation necessary.
			}
		}
	}
}

// Restore the warning after action maps are defined.
#pragma warning restore 67