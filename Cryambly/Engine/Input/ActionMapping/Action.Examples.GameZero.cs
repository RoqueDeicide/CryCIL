// This file contains a set of sample Action maps that match the contents of "defaultProfile.xml" file in
// GameZero.

using CryCil.Engine.Input.ActionMapping;

#if WIN32 || Durango || Orbis
[assembly: DeviceMapping(SupportedInputDevices.KeyboardMouse)]
#endif
#if WIN32 || Durango
[assembly: DeviceMapping(SupportedInputDevices.XboxPad)]
#endif
#if WIN32 || Orbis
[assembly: DeviceMapping(SupportedInputDevices.OrbisPad)]
#endif

// Important!
// 
// You have to disable warning CS0067, otherwise the compiler will complain about unused events. Don't
// worry they are getting used.

#pragma warning disable 67

namespace CryCil.Engine.Input.ActionMapping.GameZero
{
	/// <summary>
	/// Represents an action map that defines player's actions.
	/// </summary>
	[ActionMap("player")]
	public static class PlayerActionMap
	{
		/// <summary>
		/// An action that moves the player to the left.
		/// </summary>
		[Action("moveleft",
			ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.Retriggerable,
			KeyboardMouseInput = InputId.A)]
		public static event InputActionHandler MoveLeft;
		/// <summary>
		/// An action that moves the player to the right.
		/// </summary>
		[Action("moveright",
			ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.Retriggerable,
			KeyboardMouseInput = InputId.D)]
		public static event InputActionHandler MoveRight;
		/// <summary>
		/// An action that moves the player forwards.
		/// </summary>
		[Action("moveforward",
			ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.Retriggerable,
			KeyboardMouseInput = InputId.W)]
		public static event InputActionHandler MoveForward;
		/// <summary>
		/// An action that moves the player backwards.
		/// </summary>
		[Action("movebackward",
			ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.Retriggerable,
			KeyboardMouseInput = InputId.S)]
		public static event InputActionHandler MoveBackward;
		/// <summary>
		/// An action that moves the player up.
		/// </summary>
		[Action("moveup",
			ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.Space, XboxInput = InputId.XboxA, OrbisInput = InputId.OrbisCross)]
		public static event InputActionHandler MoveUp;
		/// <summary>
		/// An action that moves the player down.
		/// </summary>
		[Action("movedown",
			ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.LeftCtrl, XboxInput = InputId.XboxX, OrbisInput = InputId.OrbisSquare)]
		public static event InputActionHandler MoveDown;
		/// <summary>
		/// An action that boosts player's speed.
		/// </summary>
		[Action("boost",
			ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.Retriggerable,
			KeyboardMouseInput = InputId.LeftShift, XboxInput = InputId.XboxThumbLeft, OrbisInput = InputId.OrbisL3)]
		public static event InputActionHandler Boost;

		/// <summary>
		/// An action that change Yaw orientation of player's camera.
		/// </summary>
		[Action("mouse_rotateyaw", KeyboardMouseInput = InputId.MouseX)]
		public static event InputActionHandler MouseRotateYaw;
		/// <summary>
		/// An action that change Pitch orientation of player's camera.
		/// </summary>
		[Action("mouse_rotatepitch", KeyboardMouseInput = InputId.MouseY)]
		public static event InputActionHandler MouseRotatePitch;

		/// <summary>
		/// An action that moves the player forwards and backwards using the controller.
		/// </summary>
		[Action("controller_movey", XboxInput = InputId.XboxThumbLeftY, OrbisInput = InputId.OrbisStickLeftY)]
		public static event InputActionHandler ControllerMoveY;
		/// <summary>
		/// An action that moves the player left and right using the controller.
		/// </summary>
		[Action("controller_movex", XboxInput = InputId.XboxThumbLeftX, OrbisInput = InputId.OrbisStickLeftX)]
		public static event InputActionHandler ControllerMoveX;
		/// <summary>
		/// An action that change Yaw orientation of player's camera using the controller.
		/// </summary>
		[Action("controller_rotateyaw", XboxInput = InputId.XboxThumbRightX, OrbisInput = InputId.OrbisStickRightX)]
		public static event InputActionHandler ControllerRotateYaw;
		/// <summary>
		/// An action that change Pitch orientation of player's camera using the controller.
		/// </summary>
		[Action("controller_rotatepitch", XboxInput = InputId.XboxThumbRightY, OrbisInput = InputId.OrbisStickRightY)]
		public static event InputActionHandler ControllerRotatePitch;
	}
}

// Never hurts to restore the disabled warning.

#pragma warning restore 67