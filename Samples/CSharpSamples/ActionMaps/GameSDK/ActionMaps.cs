// This file contains a set of sample Action maps that match the contents of "defaultProfile.xml" file in
// GameSDK.

using CryCil.Engine.Input;
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

namespace CSharpSamples.ActionMaps.GameSDK
{
	/// <summary>
	/// Represents an action map that defines multiplayer-specific actions.
	/// </summary>
	[ActionMap("multiplayer")]
	public static class MultiplayerActionMap
	{
		/// <summary>
		/// An action that is activated to open "Weapon Customization" menu in multiplayer.
		/// </summary>
		[Action("menu_open_customizeweapon")]
		[InputAction(InputId.X, ActionActivationMode.OnPress)]
		[InputAction(InputId.XboxBack, ActionActivationMode.OnRelease)]
		[InputAction(InputId.OrbisTouch, ActionActivationMode.OnRelease)]
		public static event InputActionHandler MenuOpenCustomizeWeapon;
		/// <summary>
		/// An action that is activated to close "Weapon Customization" menu in multiplayer.
		/// </summary>
		[Action("menu_close_customizeweapon")]
		[InputAction(InputId.X, ActionActivationMode.OnPress)]
		[InputAction(InputId.XboxBack, ActionActivationMode.OnRelease | ActionActivationMode.OnPress)]
		[InputAction(InputId.OrbisTouch, ActionActivationMode.OnRelease | ActionActivationMode.OnPress)]
		public static event InputActionHandler MenuCloseCustomizeWeapon;
	}
	/// <summary>
	/// Represents an action map that defines singleplayer-specific actions.
	/// </summary>
	[ActionMap("singleplayer")]
	public static class SingleplayerActionMap
	{
		/// <summary>
		/// An action that is activated to open "Weapon Customization" menu in singleplayer.
		/// </summary>
		[Action("menu_open_customizeweapon")]
		[InputAction(InputId.X, ActionActivationMode.OnPress)]
		[InputAction(InputId.XboxBack, ActionActivationMode.OnHold, holdTriggerDelay: 0.3f, holdRepeatDelay: -1)]
		[InputAction(InputId.OrbisTouch, ActionActivationMode.OnHold, holdTriggerDelay: 0.3f, holdRepeatDelay: -1)]
		public static event InputActionHandler MenuOpenCustomizeWeapon;
		/// <summary>
		/// An action that is activated to close "Weapon Customization" menu in singleplayer.
		/// </summary>
		[Action("menu_close_customizeweapon")]
		[InputAction(InputId.X, ActionActivationMode.OnPress)]
		[InputAction(InputId.XboxBack, ActionActivationMode.OnRelease)]
		[InputAction(InputId.OrbisTouch, ActionActivationMode.OnRelease)]
		public static event InputActionHandler MenuCloseCustomizeWeapon;
	}
	/// <summary>
	/// Represents an action map that defines actions for debugging purposes.
	/// </summary>
	[ActionMap("debug")]
	public static class DebugActionMap
	{
		/// <summary>
		/// An action that saves the game.
		/// </summary>
		[Action("save", ActionActivationMode.OnPress | ActionActivationMode.ConsoleCmd,
			KeyboardMouseInput = InputId.F5)]
		public static event InputActionHandler Save;
		/// <summary>
		/// An action that loads last save game.
		/// </summary>
		[Action("loadLastSave", ActionActivationMode.OnPress | ActionActivationMode.ConsoleCmd,
			KeyboardMouseInput = InputId.F8)]
		public static event InputActionHandler LoadLastSave;
		/// <summary>
		/// An action that toggles "Fly mode" for the player.
		/// </summary>
		[Action("flymode", ActionActivationMode.OnPress | ActionActivationMode.NoModifiers,
			KeyboardMouseInput = InputId.F3)]
		public static event InputActionHandler FlyMode;
		/// <summary>
		/// An action that toggles "God mode" for the player.
		/// </summary>
		[Action("godmode", ActionActivationMode.OnPress | ActionActivationMode.NoModifiers,
			KeyboardMouseInput = InputId.F4)]
		public static event InputActionHandler GodMode;
		/// <summary>
		/// An action that toggles display of AI debug information.
		/// </summary>
		[Action("toggleaidebugdraw", ActionActivationMode.OnPress | ActionActivationMode.NoModifiers,
			KeyboardMouseInput = InputId.F11)]
		public static event InputActionHandler ToggleAiDebugDraw;
		/// <summary>
		/// An action that centers player view on Ai agent that is currently being debugged(?).
		/// </summary>
		[Action("ai_DebugCenterViewAgent", ActionActivationMode.OnPress | ActionActivationMode.NoModifiers,
			KeyboardMouseInput = InputId.NumpadDivide)]
		public static event InputActionHandler AiDebugCenterViewAgent;
		/// <summary>
		/// An action that toggles display of "helpers" for normally invisible objects, like triggers,
		/// sound spots, etc.
		/// </summary>
		[Action("togglepdrawhelpers", ActionActivationMode.OnPress | ActionActivationMode.NoModifiers,
			KeyboardMouseInput = InputId.F10)]
		public static event InputActionHandler ToggleDrawHelpers;
		/// <summary>
		/// An action that toggles "Death mode" for the player.
		/// </summary>
		[Action("toggledmode", ActionActivationMode.OnPress | ActionActivationMode.NoModifiers,
			KeyboardMouseInput = InputId.Backspace)]
		public static event InputActionHandler ToggleDeathMode;
		/// <summary>
		/// An action that activates debug mode (?).
		/// </summary>
		[Action("debug", ActionActivationMode.OnPress, KeyboardMouseInput = InputId.RowDigit7)]
		public static event InputActionHandler Debug;
		/// <summary>
		/// An action that toggles third-person mode on and off.
		/// </summary>
		[Action("thirdperson", ActionActivationMode.OnPress | ActionActivationMode.NoModifiers,
			KeyboardMouseInput = InputId.F1)]
		public static event InputActionHandler ThirdPerson;
		/// <summary>
		/// Unknown.
		/// </summary>
		[Action("debug_ag_step", ActionActivationMode.OnPress, KeyboardMouseInput = InputId.Backslash)]
		public static event InputActionHandler DebugAgStep;
		/// <summary>
		/// One of the actions used for tweaking(?).
		/// </summary>
		[Action("tweakup", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.Up, XboxInput = InputId.XboxDPadUp,
			OrbisInput = InputId.OrbisUp)]
		public static event InputActionHandler TweakUp;
		/// <summary>
		/// One of the actions used for tweaking(?).
		/// </summary>
		[Action("tweakdown", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.Down, XboxInput = InputId.XboxDPadDown,
			OrbisInput = InputId.OrbisDown)]
		public static event InputActionHandler TweakDown;
		/// <summary>
		/// One of the actions used for tweaking(?).
		/// </summary>
		[Action("tweakleft", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.Left, XboxInput = InputId.XboxDPadLeft,
			OrbisInput = InputId.OrbisLeft)]
		public static event InputActionHandler TweakLeft;
		/// <summary>
		/// One of the actions used for tweaking(?).
		/// </summary>
		[Action("tweakright", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.Right, XboxInput = InputId.XboxDPadRight,
			OrbisInput = InputId.OrbisRight)]
		public static event InputActionHandler TweakRight;
		/// <summary>
		/// One of the actions used for tweaking(?).
		/// </summary>
		[Action("tweakincrement", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.NumpadAdd, XboxInput = InputId.XboxShoulderRight,
			OrbisInput = InputId.OrbisCircle)]
		public static event InputActionHandler TweakIncrement;
		/// <summary>
		/// One of the actions used for tweaking(?).
		/// </summary>
		[Action("tweakdecrement", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.NumpadSubstract, XboxInput = InputId.XboxShoulderLeft,
			OrbisInput = InputId.OrbisCross)]
		public static event InputActionHandler TweakDecrement;
		/// <summary>
		/// An action that records player's location as a bookmark (?).
		/// </summary>
		[Action("record_bookmark", ActionActivationMode.OnPress, KeyboardMouseInput = InputId.Numpad5)]
		public static event InputActionHandler RecordBookmark;
		/// <summary>
		/// An action that is used to debug Mannequin animations on AI objects.
		/// </summary>
		[Action("mannequin_debugai", ActionActivationMode.OnPress | ActionActivationMode.NoModifiers,
			KeyboardMouseInput = InputId.NumpadMultiply)]
		public static event InputActionHandler MannequinDebugAi;
		/// <summary>
		/// An action that is used to debug Mannequin animations on players.
		/// </summary>
		[Action("mannequin_debugplayer", ActionActivationMode.OnPress | ActionActivationMode.NoModifiers,
			KeyboardMouseInput = InputId.Numpad0)]
		public static event InputActionHandler MannequinDebugPlayer;
	}
	/// <summary>
	/// Represents an action map that defines action that are used when player is using a flying camera to
	/// move around the world.
	/// </summary>
	[ActionMap("flycam")]
	public static class FlyCameraActionMap
	{
		/// <summary>
		/// An action that is used to move the camera along camera-space Z-axis.
		/// </summary>
		[Action("flycam_movey", XboxInput = InputId.XboxThumbLeftY, OrbisInput = InputId.OrbisStickLeftY)]
		public static event InputActionHandler FlyCamMoveY;
		/// <summary>
		/// An action that is used to move the camera along camera-space X-axis.
		/// </summary>
		[Action("flycam_movex", XboxInput = InputId.XboxThumbLeftX, OrbisInput = InputId.OrbisStickLeftX)]
		public static event InputActionHandler FlyCamMoveX;
		/// <summary>
		/// An action that is used to rotate the camera around camera-space Z-axis.
		/// </summary>
		[Action("flycam_rotateyaw", XboxInput = InputId.XboxThumbRightX, OrbisInput = InputId.OrbisStickRightX)]
		public static event InputActionHandler FlyCamRotateYaw;
		/// <summary>
		/// An action that is used to rotate the camera around camera-space X-axis.
		/// </summary>
		[Action("flycam_rotatepitch", XboxInput = InputId.XboxThumbRightY, OrbisInput = InputId.OrbisStickRightY)]
		public static event InputActionHandler FlyCamRotatePitch;
		/// <summary>
		/// An action that is used to move the camera in positive direction along world-space Z-axis.
		/// </summary>
		[Action("flycam_moveup", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			XboxInput = InputId.XboxTriggerRight, OrbisInput = InputId.OrbisR2)]
		public static event InputActionHandler FlyCamMoveUp;
		/// <summary>
		/// An action that is used to move the camera in negative direction along world-space Z-axis.
		/// </summary>
		[Action("flycam_movedown", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			XboxInput = InputId.XboxTriggerLeft, OrbisInput = InputId.OrbisL2)]
		public static event InputActionHandler FlyCamMoveDown;
		/// <summary>
		/// An action that is used to increase the speed.
		/// </summary>
		[Action("flycam_speedup", ActionActivationMode.OnPress,
			XboxInput = InputId.XboxDPadUp, OrbisInput = InputId.OrbisUp)]
		public static event InputActionHandler FlyCamSpeedUp;
		/// <summary>
		/// An action that is used to decrease the speed.
		/// </summary>
		[Action("flycam_speeddown", ActionActivationMode.OnPress,
			XboxInput = InputId.XboxDPadDown, OrbisInput = InputId.OrbisDown)]
		public static event InputActionHandler FlyCamSpeedDown;
		/// <summary>
		/// An action that is used to activate turbo mode.
		/// </summary>
		[Action("flycam_turbo", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.Space, XboxInput = InputId.XboxA, OrbisInput = InputId.OrbisCross)]
		public static event InputActionHandler FlyCamTurbo;
		/// <summary>
		/// An action that is used to set the point on the flight path.
		/// </summary>
		[Action("flycam_setpoint", ActionActivationMode.OnPress,
			XboxInput = InputId.XboxShoulderLeft, OrbisInput = InputId.OrbisL1)]
		public static event InputActionHandler FlyCamSetPoint;
		/// <summary>
		/// An action that is used to play the recorded flight path.
		/// </summary>
		[Action("flycam_play", ActionActivationMode.OnPress,
			XboxInput = InputId.XboxB, OrbisInput = InputId.OrbisCircle)]
		public static event InputActionHandler FlyCamPlay;
		/// <summary>
		/// An action that is used to clear the recorded flight path.
		/// </summary>
		[Action("flycam_clear", ActionActivationMode.OnPress,
			XboxInput = InputId.XboxY, OrbisInput = InputId.OrbisTriangle)]
		public static event InputActionHandler FlyCamClear;
	}
	/// <summary>
	/// Represents an action map that define common default actions.
	/// </summary>
	[ActionMap("default")]
	public static class DefaultActionMap
	{
		/// <summary>
		/// An action that skips current cutscene.
		/// </summary>
		[Action("skip_cutscene", ActionActivationMode.OnPress | ActionActivationMode.NoModifiers,
			KeyboardMouseInput = InputId.Enter, XboxInput = InputId.XboxY, OrbisInput = InputId.OrbisTriangle)]
		public static event InputActionHandler SkipCutscene;
		/// <summary>
		/// An action that skips the cutscene that plays during the loading screen.
		/// </summary>
		[Action("skip_loadingscreen", ActionActivationMode.OnPress | ActionActivationMode.NoModifiers,
			KeyboardMouseInput = InputId.Enter, XboxInput = InputId.XboxA, OrbisInput = InputId.OrbisCross)]
		public static event InputActionHandler SkipLoadingScreen;
		/// <summary>
		/// An action that skips the cutscene that plays during the loading screen(?).
		/// </summary>
		[Action("skip_loadingscreen_switched", ActionActivationMode.OnPress | ActionActivationMode.NoModifiers,
			KeyboardMouseInput = InputId.Enter, XboxInput = InputId.XboxB, OrbisInput = InputId.OrbisCircle)]
		public static event InputActionHandler SkipLoadingScreenSwitched;
		/// <summary>
		/// An action that is activated when one of the Xbox gamepads is disconnected.
		/// </summary>
		[Action("xi_disconnect", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			XboxInput = InputId.XboxDisconnect)]
		public static event InputActionHandler XInputDisconnect;
		/// <summary>
		/// An action that displays the Objectives screen.
		/// </summary>
		[Action("objectives", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.Tab)]
		public static event InputActionHandler Objectives;
		/// <summary>
		/// An action that moves the pointer on the HUD along screen-space X-axis.
		/// </summary>
		[Action("hud_mousex", KeyboardMouseInput = InputId.MouseX, XboxInput = InputId.XboxThumbRightX,
			OrbisInput = InputId.OrbisStickRightX)]
		public static event InputActionHandler HudMouseX;
		/// <summary>
		/// An action that moves the pointer on the HUD along screen-space Y-axis.
		/// </summary>
		[Action("hud_mousey", KeyboardMouseInput = InputId.MouseY, XboxInput = InputId.XboxThumbRightY,
			OrbisInput = InputId.OrbisStickRightY)]
		public static event InputActionHandler HudMouseY;
		/// <summary>
		/// An action that processes left mouse button click on the HUD.
		/// </summary>
		[Action("hud_mouseclick", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.Mouse1, XboxInput = InputId.XboxA, OrbisInput = InputId.OrbisCross)]
		public static event InputActionHandler HudMouseClick;
		/// <summary>
		/// An action that is used for "Push-To-Talk" voice communication.
		/// </summary>
		[Action("voice_chat_talk", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.LeftAlt)]
		public static event InputActionHandler VoiceChatTalk;
		/// <summary>
		/// An action that opens "All" chat.
		/// </summary>
		[Action("hud_openchat", ActionActivationMode.OnPress, KeyboardMouseInput = InputId.Y)]
		public static event InputActionHandler HudOpenChat;
		/// <summary>
		/// An action that opens "Team" chat.
		/// </summary>
		[Action("hud_openteamchat", ActionActivationMode.OnPress, KeyboardMouseInput = InputId.U)]
		public static event InputActionHandler HudOpenTeamChat;
		/// <summary>
		/// An action that does something with spectator mode.
		/// </summary>
		[Action("spectate_gen_spawn", ActionActivationMode.OnPress,
			KeyboardMouseInput = InputId.Space, XboxInput = InputId.XboxA, OrbisInput = InputId.OrbisCross)]
		public static event InputActionHandler SpectateGenSpawn;
		/// <summary>
		/// An action that switches to the next spectator mode.
		/// </summary>
		[Action("spectate_gen_nextmode", ActionActivationMode.OnPress,
			KeyboardMouseInput = InputId.Space, XboxInput = InputId.XboxShoulderRight, OrbisInput = InputId.OrbisR1)]
		public static event InputActionHandler SpectateGenNextMode;
		/// <summary>
		/// An action that switches to the previous spectator mode.
		/// </summary>
		[Action("spectate_gen_prevmode", ActionActivationMode.OnPress,
			XboxInput = InputId.XboxShoulderLeft, OrbisInput = InputId.OrbisL1)]
		public static event InputActionHandler SpectateGenPreviousMode;
		///// <summary>
		///// An action that skips the death camera in spectator mode.
		///// </summary>
		//[Action("spectate_gen_skipdeathcam", ActionActivationMode.OnPress,
		//KeyboardMouseInput = InputId.Space, XboxInput = InputId.XboxX, OrbisInput = InputId.OrbisSquare)]
		//public static event InputActionHandler SpectateGenSkipDeathCamera;
		/// <summary>
		/// An action that switches to the next camera in spectator mode.
		/// </summary>
		[Action("spectate_gen_nextcamera", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.F, XboxInput = InputId.XboxX, OrbisInput = InputId.OrbisSquare)]
		public static event InputActionHandler SpectateGenNextCamera;
		/// <summary>
		/// An action that switches to the next team-mate in spectator mode.
		/// </summary>
		[Action("spectate_3rdperson_nextteammate", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.D, XboxInput = InputId.XboxDPadRight, OrbisInput = InputId.OrbisRight)]
		public static event InputActionHandler SpectateGen3rdPersonNextTeamMate;
		/// <summary>
		/// An action that switches to the previous team-mate in spectator mode.
		/// </summary>
		[Action("spectate_3rdperson_prevteammate", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.A, XboxInput = InputId.XboxDPadLeft, OrbisInput = InputId.OrbisLeft)]
		public static event InputActionHandler SpectateGen3rdPersonPreviousTeamMate;
		/// <summary>
		/// An action that switches to the different team-mate in spectator mode.
		/// </summary>
		[Action("spectate_3rdperson_changeteammate_xi", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			XboxInput = InputId.XboxThumbLeftX, OrbisInput = InputId.OrbisStickLeftX)]
		public static event InputActionHandler SpectateGen3rdPersonChangeTeamMate;
		/// <summary>
		/// An action that switches to the next CCTV camera in spectator mode.
		/// </summary>
		[Action("spectate_cctv_nextcam", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.D, XboxInput = InputId.XboxDPadRight, OrbisInput = InputId.OrbisRight)]
		public static event InputActionHandler SpectateGen3rdPersonNextCctvCamera;
		/// <summary>
		/// An action that switches to the previous CCTV camera in spectator mode.
		/// </summary>
		[Action("spectate_cctv_prevcam", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.A, XboxInput = InputId.XboxDPadLeft, OrbisInput = InputId.OrbisLeft)]
		public static event InputActionHandler SpectateGen3rdPersonPreviousCctvCamera;
		/// <summary>
		/// An action that switches to the different CCTV camera in spectator mode.
		/// </summary>
		[Action("spectate_cctv_changecam_xi", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			XboxInput = InputId.XboxThumbLeftX, OrbisInput = InputId.OrbisStickLeftX)]
		public static event InputActionHandler SpectateGen3rdPersonChangeCctvCamera;
		/// <summary>
		/// An action that toggles the "Pause" menu.
		/// </summary>
		[Action("ui_toggle_pause", ActionActivationMode.OnPress,
			XboxInput = InputId.XboxStart, OrbisInput = InputId.OrbisStart)]
		public static event InputActionHandler UiTogglePause;
		/// <summary>
		/// An action that shows the "Pause" menu.
		/// </summary>
		[Action("ui_start_pause", ActionActivationMode.OnPress, KeyboardMouseInput = InputId.Escape)]
		public static event InputActionHandler UiStartPause;
		/// <summary>
		/// An action that processes the click on the selected Ui element.
		/// </summary>
		[Action("ui_click", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			XboxInput = InputId.XboxA, OrbisInput = InputId.OrbisCross)]
		[InputAction(InputId.Enter)]
		[InputAction(InputId.NumpadEnter)]
		public static event InputActionHandler UiClick;
		/// <summary>
		/// An action that moves the Ui selection upwards.
		/// </summary>
		[Action("ui_up", ActionActivationMode.OnPress | ActionActivationMode.OnHold,
			holdTriggerDelay: 0.15f, holdRepeatDelay: 0.15f)]
		[InputAction(InputId.Up)]
		[InputAction(InputId.XboxDPadUp)]
		[InputAction(InputId.XboxThumbLeftY, op: AnalogComparisonOperation.GreaterThan, analogCompareValue: 0.5f)]
		[InputAction(InputId.XboxThumbRightY, op: AnalogComparisonOperation.GreaterThan, analogCompareValue: 0.5f)]
		[InputAction(InputId.OrbisUp)]
		[InputAction(InputId.OrbisStickLeftY, op: AnalogComparisonOperation.GreaterThan, analogCompareValue: 0.5f)]
		[InputAction(InputId.OrbisStickRightY, op: AnalogComparisonOperation.GreaterThan, analogCompareValue: 0.5f)]
		public static event InputActionHandler UiUp;
		/// <summary>
		/// An action that moves the Ui selection downwards.
		/// </summary>
		[Action("ui_down", ActionActivationMode.OnPress | ActionActivationMode.OnHold,
			holdTriggerDelay: 0.15f, holdRepeatDelay: 0.15f)]
		[InputAction(InputId.Down)]
		[InputAction(InputId.XboxDPadDown)]
		[InputAction(InputId.XboxThumbLeftY, op: AnalogComparisonOperation.LessThan, analogCompareValue: -0.5f)]
		[InputAction(InputId.XboxThumbRightY, op: AnalogComparisonOperation.LessThan, analogCompareValue: -0.5f)]
		[InputAction(InputId.OrbisDown)]
		[InputAction(InputId.OrbisStickLeftY, op: AnalogComparisonOperation.LessThan, analogCompareValue: -0.5f)]
		[InputAction(InputId.OrbisStickRightY, op: AnalogComparisonOperation.LessThan, analogCompareValue: -0.5f)]
		public static event InputActionHandler UiDown;
		/// <summary>
		/// An action that moves the Ui selection to the left.
		/// </summary>
		[Action("ui_left", ActionActivationMode.OnPress | ActionActivationMode.OnHold,
			holdTriggerDelay: 0.15f, holdRepeatDelay: 0.15f)]
		[InputAction(InputId.Left)]
		[InputAction(InputId.XboxDPadLeft)]
		[InputAction(InputId.XboxThumbLeftX, op: AnalogComparisonOperation.LessThan, analogCompareValue: -0.5f)]
		[InputAction(InputId.XboxThumbRightX, op: AnalogComparisonOperation.LessThan, analogCompareValue: -0.5f)]
		[InputAction(InputId.OrbisLeft)]
		[InputAction(InputId.OrbisStickLeftX, op: AnalogComparisonOperation.LessThan, analogCompareValue: -0.5f)]
		[InputAction(InputId.OrbisStickRightX, op: AnalogComparisonOperation.LessThan, analogCompareValue: -0.5f)]
		public static event InputActionHandler UiLeft;
		/// <summary>
		/// An action that moves the Ui selection to the right.
		/// </summary>
		[Action("ui_right", ActionActivationMode.OnPress | ActionActivationMode.OnHold,
			holdTriggerDelay: 0.15f, holdRepeatDelay: 0.15f)]
		[InputAction(InputId.Right)]
		[InputAction(InputId.XboxDPadRight)]
		[InputAction(InputId.XboxThumbLeftX, op: AnalogComparisonOperation.GreaterThan, analogCompareValue: 0.5f)]
		[InputAction(InputId.XboxThumbRightX, op: AnalogComparisonOperation.GreaterThan, analogCompareValue: 0.5f)]
		[InputAction(InputId.OrbisRight)]
		[InputAction(InputId.OrbisStickLeftX, op: AnalogComparisonOperation.GreaterThan, analogCompareValue: 0.5f)]
		[InputAction(InputId.OrbisStickRightX, op: AnalogComparisonOperation.GreaterThan, analogCompareValue: 0.5f)]
		public static event InputActionHandler UiRight;
		/// <summary>
		/// An action that switches to the previous UI screen.
		/// </summary>
		[Action("ui_back", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.Escape, XboxInput = InputId.XboxB, OrbisInput = InputId.OrbisCircle)]
		public static event InputActionHandler UiBack;
		/// <summary>
		/// An action that confirms an action in UI.
		/// </summary>
		[Action("ui_confirm", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			XboxInput = InputId.XboxY, OrbisInput = InputId.OrbisTriangle)]
		public static event InputActionHandler UiConfirm;
		/// <summary>
		/// An action that resets position of UI.
		/// </summary>
		[Action("ui_reset", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			XboxInput = InputId.XboxX, OrbisInput = InputId.OrbisSquare)]
		public static event InputActionHandler UiReset;
		/// <summary>
		/// An action that skips the video in UI.
		/// </summary>
		[Action("ui_skip_video", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.Space, XboxInput = InputId.XboxA, OrbisInput = InputId.OrbisCross)]
		public static event InputActionHandler UiSkipVideo;
	}
	/// <summary>
	/// Represents an action map that defines player's actions.
	/// </summary>
	[ActionMap("player")]
	public static class PlayerActionMap
	{
		/// <summary>
		/// An action that uses something in the world.
		/// </summary>
		[Action("use", ActionActivationMode.OnRelease)]
		[InputAction(InputId.F, ActionActivationMode.OnPress)]
		[InputAction(InputId.XboxX, ActionActivationMode.OnHold, holdTriggerDelay: 0.3f, holdRepeatDelay: -1)]
		[InputAction(InputId.OrbisSquare, ActionActivationMode.OnHold, holdTriggerDelay: 0.3f, holdRepeatDelay: -1)]
		public static event InputActionHandler Use;
		/// <summary>
		/// An action that performs a primary attack with current weapon.
		/// </summary>
		[Action("attack1", ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.OnHold,
			KeyboardMouseInput = InputId.Mouse1, XboxInput = InputId.XboxTriggerRightButton, OrbisInput = InputId.OrbisR2)]
		public static event InputActionHandler Attack1;
		/// <summary>
		/// An action that toggles aiming down sights.
		/// </summary>
		[Action("zoom", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.Mouse2)]
		public static event InputActionHandler Zoom;
		/// <summary>
		/// An action that toggles aiming down sights using the controller.
		/// </summary>
		[Action("xi_zoom", ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.OnHold,
			XboxInput = InputId.XboxTriggerLeftButton, OrbisInput = InputId.OrbisL2)]
		public static event InputActionHandler XiZoom;
		/// <summary>
		/// An action that
		/// </summary>
		[Action("stabilize", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.LeftShift, XboxInput = InputId.XboxThumbLeft, OrbisInput = InputId.OrbisL3)]
		public static event InputActionHandler Stabilize;
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
		/// An action that switches between prone and standing postures.
		/// </summary>
		[Action("prone", ActionActivationMode.OnPress | ActionActivationMode.OnHold,
			KeyboardMouseInput = InputId.Z)]
		public static event InputActionHandler Prone;
		/// <summary>
		/// An action that triggers a jump.
		/// </summary>
		[Action("jump", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.Space, XboxInput = InputId.XboxA, OrbisInput = InputId.OrbisCross)]
		public static event InputActionHandler Jump;
		/// <summary>
		/// An action that toggles crouch stance.
		/// </summary>
		[Action("crouch",
			ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.Retriggerable,
			KeyboardMouseInput = InputId.C, XboxInput = InputId.XboxB, OrbisInput = InputId.OrbisCircle)]
		public static event InputActionHandler Crouch;
		/// <summary>
		/// An action that toggles sprinting.
		/// </summary>
		[Action("sprint",
			ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.Retriggerable,
			KeyboardMouseInput = InputId.LeftShift, XboxInput = InputId.XboxThumbLeft, OrbisInput = InputId.OrbisR2)]
		public static event InputActionHandler Sprint;
		/// <summary>
		/// An action that triggers immediate respawn.
		/// </summary>
		[Action("respawn", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.Mouse1, XboxInput = InputId.XboxTriggerRightButton, OrbisInput = InputId.OrbisR2)]
		public static event InputActionHandler Respawn;
		/// <summary>
		/// An action that performs a special operation.
		/// </summary>
		[Action("special", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.V, XboxInput = InputId.XboxThumbRight, OrbisInput = InputId.OrbisR3)]
		public static event InputActionHandler Special;
		/// <summary>
		/// An action that toggles leaning to the left.
		/// </summary>
		[Action("leanleft", ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.OnHold,
			KeyboardMouseInput = InputId.Q)]
		public static event InputActionHandler LeanLeft;
		/// <summary>
		/// An action that toggles leaning to the right.
		/// </summary>
		[Action("leanright", ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.OnHold,
			KeyboardMouseInput = InputId.E)]
		public static event InputActionHandler LeanRight;
		/// <summary>
		/// An action that reloads the weapon.
		/// </summary>
		[Action("reload", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.R, XboxInput = InputId.XboxX, OrbisInput = InputId.OrbisSquare)]
		public static event InputActionHandler Reload;
		/// <summary>
		/// An action that drops the weapon.
		/// </summary>
		[Action("drop", ActionActivationMode.OnPress | ActionActivationMode.OnRelease, KeyboardMouseInput = InputId.J)]
		public static event InputActionHandler Drop;
		/// <summary>
		/// An action that switches to another weapon.
		/// </summary>
		[Action("toggle_weapon", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.RowDigit1, XboxInput = InputId.XboxY, OrbisInput = InputId.OrbisTriangle)]
		public static event InputActionHandler ToggleWeapon;
		/// <summary>
		/// An action that switches to the next item.
		/// </summary>
		[Action("nextitem", ActionActivationMode.OnPress, KeyboardMouseInput = InputId.MouseWheelUp)]
		public static event InputActionHandler NextItem;
		/// <summary>
		/// An action that switches to the previous item.
		/// </summary>
		[Action("previtem", ActionActivationMode.OnPress, KeyboardMouseInput = InputId.MouseWheelDown)]
		public static event InputActionHandler PreviousItem;
		/// <summary>
		/// An action that toggles weapon's fire-mode.
		/// </summary>
		[Action("weapon_change_firemode", ActionActivationMode.OnPress)]
		[InputAction(InputId.RowDigit2)]
		[InputAction(InputId.XboxDPadLeft, pressTriggerDelay: 0.15f, pressDelayPriority: 1,
			blockedInputs: new[] { InputId.XboxDPadUp, InputId.XboxDPadDown }, blockDuration: 0.12f)]
		[InputAction(InputId.OrbisLeft, pressTriggerDelay: 0.15f, pressDelayPriority: 1,
			blockedInputs: new[] { InputId.OrbisUp, InputId.OrbisDown },
			blockDuration: 0.12f)]
		public static event InputActionHandler WeaponChangeFiremode;
		/// <summary>
		/// An action that toggle the flashlight on the weapon.
		/// </summary>
		[Action("toggle_flashlight", ActionActivationMode.OnPress)]
		[InputAction(InputId.L)]
		[InputAction(InputId.XboxDPadDown, pressTriggerDelay: 0.15f, pressDelayPriority: 1,
			blockedInputs: new[] { InputId.XboxDPadRight, InputId.XboxDPadLeft }, blockDuration: 0.12f)]
		[InputAction(InputId.OrbisDown, pressTriggerDelay: 0.15f, pressDelayPriority: 1,
			blockedInputs: new[] { InputId.OrbisLeft },
			blockDuration: 0.12f)]
		public static event InputActionHandler ToggleFlashlight;
		/// <summary>
		/// An action that switches to the explosive weapon.
		/// </summary>
		[Action("toggle_explosive", ActionActivationMode.OnPress)]
		[InputAction(InputId.RowDigit4)]
		[InputAction(InputId.XboxDPadRight, pressTriggerDelay: 0.15f, pressDelayPriority: 1,
			blockedInputs: new[] { InputId.XboxDPadUp, InputId.XboxDPadDown }, blockDuration: 0.12f)]
		[InputAction(InputId.OrbisRight, pressTriggerDelay: 0.15f, pressDelayPriority: 1,
			blockedInputs: new[] { InputId.OrbisUp, InputId.OrbisDown },
			blockDuration: 0.12f)]
		public static event InputActionHandler ToggleExplosive;
		/// <summary>
		/// An action that switches to the special weapon.
		/// </summary>
		[Action("toggle_special", ActionActivationMode.OnPress)]
		[InputAction(InputId.RowDigit3)]
		[InputAction(InputId.XboxDPadUp, pressTriggerDelay: 0.15f, pressDelayPriority: 1,
			blockedInputs: new[] { InputId.XboxDPadRight, InputId.XboxDPadLeft }, blockDuration: 0.12f)]
		[InputAction(InputId.OrbisUp, pressTriggerDelay: 0.15f, pressDelayPriority: 1,
			blockedInputs: new[] { InputId.OrbisRight, InputId.OrbisLeft },
			blockDuration: 0.12f)]
		public static event InputActionHandler ToggleSpecial;
		/// <summary>
		/// An action that equips the grenade.
		/// </summary>
		[Action("handgrenade", ActionActivationMode.OnPress, KeyboardMouseInput = InputId.G)]
		public static event InputActionHandler HandGrenade;
		/// <summary>
		/// An action that equips the grenade.
		/// </summary>
		[Action("xi_grenade", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			XboxInput = InputId.XboxShoulderRight, OrbisInput = InputId.OrbisR2)]
		public static event InputActionHandler XiGrenade;
		/// <summary>
		/// An action that change Yaw orientation of player's camera.
		/// </summary>
		[Action("rotateyaw", KeyboardMouseInput = InputId.MouseX)]
		public static event InputActionHandler RotateYaw;
		/// <summary>
		/// An action that change Pitch orientation of player's camera.
		/// </summary>
		[Action("rotatepitch", KeyboardMouseInput = InputId.MouseY)]
		public static event InputActionHandler RotatePitch;
		/// <summary>
		/// An action that toggles zoom.
		/// </summary>
		[Action("zoom_toggle", ActionActivationMode.OnPress,
			KeyboardMouseInput = InputId.V, XboxInput = InputId.XboxThumbRight, OrbisInput = InputId.OrbisR3)]
		public static event InputActionHandler ZoomToggle;
		/// <summary>
		/// An action that zooms in.
		/// </summary>
		[Action("zoom_in", KeyboardMouseInput = InputId.MouseWheelUp)]
		public static event InputActionHandler ZoomIn;
		/// <summary>
		/// An action that zooms out.
		/// </summary>
		[Action("zoom_out", KeyboardMouseInput = InputId.MouseWheelDown)]
		public static event InputActionHandler ZoomOut;
		/// <summary>
		/// An action that moves the player forwards and backwards using the controller.
		/// </summary>
		[Action("xi_movey", XboxInput = InputId.XboxThumbLeftY, OrbisInput = InputId.OrbisStickLeftY)]
		public static event InputActionHandler XiMoveY;
		/// <summary>
		/// An action that moves the player left and right using the controller.
		/// </summary>
		[Action("xi_movex", XboxInput = InputId.XboxThumbLeftX, OrbisInput = InputId.OrbisStickLeftX)]
		public static event InputActionHandler XiMoveX;
		/// <summary>
		/// An action that change Yaw orientation of player's camera using the controller.
		/// </summary>
		[Action("xi_rotateyaw", XboxInput = InputId.XboxThumbRightX, OrbisInput = InputId.OrbisStickRightX)]
		public static event InputActionHandler XiRotateYaw;
		/// <summary>
		/// An action that change Pitch orientation of player's camera using the controller.
		/// </summary>
		[Action("xi_rotatepitch", XboxInput = InputId.XboxThumbRightY, OrbisInput = InputId.OrbisStickRightY)]
		public static event InputActionHandler XiRotatePitch;
	}
	/// <summary>
	/// Represents an action map that defines player's actions that are specific to multiplayer.
	/// </summary>
	[ActionMap("player_mp")]
	public static class PlayerMultiPlayerActionMap
	{
		/// <summary>
		/// An action that throws the grenade.
		/// </summary>
		[Action("grenade", ActionActivationMode.OnPress | ActionActivationMode.OnRelease, KeyboardMouseInput = InputId.H)]
		public static event InputActionHandler Grenade;
	}
	/// <summary>
	/// Represents an action map that defines player's actions that are specific to singleplayer.
	/// </summary>
	[ActionMap("player_sp")]
	public static class PlayerSinglePlayerActionMap
	{
	}
	/// <summary>
	/// Represents an action map that defines general vehicle-related actions.
	/// </summary>
	[ActionMap("vehicle_general")]
	public static class GeneralVehicleActionMap
	{
		/// <summary>
		/// An action that exits the vehicle.
		/// </summary>
		[Action("v_exit", ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.OnHold,
			KeyboardMouseInput = InputId.F, XboxInput = InputId.XboxX, OrbisInput = InputId.OrbisSquare)]
		public static event InputActionHandler Exit;
		/// <summary>
		/// An action that triggers the horn.
		/// </summary>
		[Action("v_horn", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.H, XboxInput = InputId.XboxThumbRight, OrbisInput = InputId.OrbisR3)]
		public static event InputActionHandler Horn;
		/// <summary>
		/// An action that toggles the head-lights.
		/// </summary>
		[Action("v_lights", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.L, XboxInput = InputId.XboxDPadDown, OrbisInput = InputId.OrbisDown)]
		public static event InputActionHandler Lights;
		/// <summary>
		/// An action that changes the seat.
		/// </summary>
		[Action("v_changeseat", ActionActivationMode.OnPress,
			KeyboardMouseInput = InputId.C, XboxInput = InputId.XboxDPadUp, OrbisInput = InputId.OrbisUp)]
		public static event InputActionHandler ChangeSeat;
		/// <summary>
		/// An action that changes the view mode.
		/// </summary>
		[Action("v_changeview", ActionActivationMode.OnPress,
			KeyboardMouseInput = InputId.F1, XboxInput = InputId.XboxBack, OrbisInput = InputId.OrbisTouch)]
		public static event InputActionHandler ChangeView;
		/// <summary>
		/// An action that zooms in.
		/// </summary>
		[Action("v_zoom_in", ActionActivationMode.OnPress, KeyboardMouseInput = InputId.MouseWheelUp)]
		public static event InputActionHandler ZoomIn;
		/// <summary>
		/// An action that zooms out.
		/// </summary>
		[Action("v_zoom_out", ActionActivationMode.OnPress, KeyboardMouseInput = InputId.MouseWheelDown)]
		public static event InputActionHandler ZoomOut;
		/// <summary>
		/// An action that change Yaw orientation of player's camera.
		/// </summary>
		[Action("v_rotateyaw", KeyboardMouseInput = InputId.MouseX)]
		public static event InputActionHandler RotateYaw;
		/// <summary>
		/// An action that change Pitch orientation of player's camera.
		/// </summary>
		[Action("v_rotatepitch", KeyboardMouseInput = InputId.MouseY)]
		public static event InputActionHandler RotatePitch;
	}
	/// <summary>
	/// Represents an action map that defines driver's actions.
	/// </summary>
	[ActionMap("vehicle_driver")]
	public static class VehicleDriverActionMap
	{
		/// <summary>
		/// An action that turns the vehicle to the left.
		/// </summary>
		[Action("v_turnleft",
			ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.Retriggerable,
			KeyboardMouseInput = InputId.A)]
		public static event InputActionHandler TurnLeft;
		/// <summary>
		/// An action that turns the vehicle to the right.
		/// </summary>
		[Action("v_turnright",
			ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.Retriggerable,
			KeyboardMouseInput = InputId.D)]
		public static event InputActionHandler TurnRight;
		/// <summary>
		/// An action that moves the vehicle forward.
		/// </summary>
		[Action("v_moveforward",
			ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.Retriggerable,
			KeyboardMouseInput = InputId.W)]
		public static event InputActionHandler MoveForward;
		/// <summary>
		/// An action that moves the vehicle backwards.
		/// </summary>
		[Action("v_moveback",
			ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.Retriggerable,
			KeyboardMouseInput = InputId.S)]
		public static event InputActionHandler MoveBack;
		/// <summary>
		/// An action that activates the hand-brake.
		/// </summary>
		[Action("v_brake", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.Space, XboxInput = InputId.XboxB, OrbisInput = InputId.OrbisCircle)]
		public static event InputActionHandler Brake;
		/// <summary>
		/// An action that activates the boost.
		/// </summary>
		[Action("v_boost", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.LeftShift, XboxInput = InputId.XboxA, OrbisInput = InputId.OrbisCross)]
		public static event InputActionHandler Boost;
		/// <summary>
		/// An action that rolls the flying vehicle to the left.
		/// </summary>
		[Action("v_rollleft", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.Q)]
		public static event InputActionHandler RollLeft;
		/// <summary>
		/// An action that rolls the flying vehicle to the right.
		/// </summary>
		[Action("v_rollright", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.T)]
		public static event InputActionHandler RollRight;
		/// <summary>
		/// An action that accelerates the vehicle.
		/// </summary>
		[Action("xi_v_accelerate", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			XboxInput = InputId.XboxTriggerRightButton, OrbisInput = InputId.OrbisR2)]
		public static event InputActionHandler XiAccelerate;
		/// <summary>
		/// An action that decelerates the vehicle.
		/// </summary>
		[Action("xi_v_deccelerate", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			XboxInput = InputId.XboxTriggerLeftButton, OrbisInput = InputId.OrbisL2)]
		public static event InputActionHandler XiDeccelerate;
		/// <summary>
		/// An action that performs the attack with vehicle's primary weapon.
		/// </summary>
		[Action("xi_v_attack1", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.Mouse1, XboxInput = InputId.XboxShoulderRight, OrbisInput = InputId.OrbisR1)]
		public static event InputActionHandler XiAttack1;
		/// <summary>
		/// An action that performs the attack with vehicle's secondary weapon.
		/// </summary>
		[Action("xi_v_attack2", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.Mouse2, XboxInput = InputId.XboxShoulderLeft, OrbisInput = InputId.OrbisL1)]
		public static event InputActionHandler XiAttack2;
		/// <summary>
		/// An action that turns the vehicle left and right.
		/// </summary>
		[Action("xi_v_movex", XboxInput = InputId.XboxThumbLeftX, OrbisInput = InputId.OrbisStickLeftX)]
		public static event InputActionHandler XiMoveX;
		/// <summary>
		/// An action that changes the camera orientation when the player is in a vehicle where driver's
		/// and gunner's seats are combined (e.g. a tank).
		/// </summary>
		[Action("xi_rotateyaw", XboxInput = InputId.XboxThumbRightX, OrbisInput = InputId.OrbisStickRightX)]
		public static event InputActionHandler RotateYaw;
		/// <summary>
		/// An action that changes the camera orientation when the player is in a vehicle where driver's
		/// and gunner's seats are combined (e.g. a tank).
		/// </summary>
		[Action("xi_rotatepitch", XboxInput = InputId.XboxThumbRightY, OrbisInput = InputId.OrbisStickRightY)]
		public static event InputActionHandler RotatePitch;
		/// <summary>
		/// An action that changes the camera orientation when the player is in a gunner's seat.
		/// </summary>
		[Action("xi_v_rotateyaw", XboxInput = InputId.XboxThumbRightX, OrbisInput = InputId.OrbisStickRightX)]
		public static event InputActionHandler RotateYawGunner;
		/// <summary>
		/// An action that changes the camera orientation when the player is in a gunner's seat.
		/// </summary>
		[Action("xi_v_rotatepitch", XboxInput = InputId.XboxThumbRightY, OrbisInput = InputId.OrbisStickRightY)]
		public static event InputActionHandler RotatePitchGunner;
	}
	/// <summary>
	/// Represents an action map that defines gunner's actions.
	/// </summary>
	[ActionMap("vehicle_gunner")]
	public static class VehicleGunnerActionMap
	{
		/// <summary>
		/// An action that performs the attack with vehicle's primary weapon.
		/// </summary>
		[Action("xi_v_attack1", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.Mouse1, XboxInput = InputId.XboxShoulderRight, OrbisInput = InputId.OrbisR1)]
		public static event InputActionHandler XiAttack1;
		/// <summary>
		/// An action that performs the attack with vehicle's secondary weapon.
		/// </summary>
		[Action("xi_v_attack2", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.Mouse2, XboxInput = InputId.XboxShoulderLeft, OrbisInput = InputId.OrbisL1)]
		public static event InputActionHandler XiAttack2;
		/// <summary>
		/// An action that rips the weapon off the vehicle (if possible).
		/// </summary>
		[Action("v_ripoff_weapon", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.V, XboxInput = InputId.XboxThumbRight, OrbisInput = InputId.OrbisR3)]
		public static event InputActionHandler RipWeaponOff;
	}
	/// <summary>
	/// Represents an action map that defines helicopter pilot's actions.
	/// </summary>
	[ActionMap("helicopter")]
	public static class HelicopterActionMap
	{
		/// <summary>
		/// An action that moves the helicopter up.
		/// </summary>
		[Action("v_moveup",
			ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.Retriggerable,
			KeyboardMouseInput = InputId.W, XboxInput = InputId.XboxTriggerRight, OrbisInput = InputId.OrbisR2)]
		public static event InputActionHandler MoveUp;
		/// <summary>
		/// An action that moves the helicopter down.
		/// </summary>
		[Action("v_movedown",
			ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.Retriggerable,
			KeyboardMouseInput = InputId.S, XboxInput = InputId.XboxTriggerLeft, OrbisInput = InputId.OrbisL2)]
		public static event InputActionHandler MoveDown;
		/// <summary>
		/// An action that strafes the helicopter to the left.
		/// </summary>
		[Action("v_strafeleft", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.A)]
		public static event InputActionHandler StrafeLeft;
		/// <summary>
		/// An action that strafes the helicopter to the right.
		/// </summary>
		[Action("v_straferight", ActionActivationMode.OnPress | ActionActivationMode.OnRelease,
			KeyboardMouseInput = InputId.D)]
		public static event InputActionHandler StrafeRight;
		/// <summary>
		/// An action that accelerates or decelerates the helicopter.
		/// </summary>
		[Action("xi_v_movey", XboxInput = InputId.XboxThumbLeftY, OrbisInput = InputId.OrbisStickLeftY)]
		public static event InputActionHandler XiMoveY;
		/// <summary>
		/// An action that turns the vehicle left and right.
		/// </summary>
		[Action("xi_v_movex", XboxInput = InputId.XboxThumbLeftX, OrbisInput = InputId.OrbisStickLeftX)]
		public static event InputActionHandler XiMoveX;
		/// <summary>
		/// An action that changes the helicopter's orientation.
		/// </summary>
		[Action("xi_v_rotateyaw", XboxInput = InputId.XboxThumbRightX, OrbisInput = InputId.OrbisStickRightX)]
		public static event InputActionHandler RotateYaw;
	}
	/// <summary>
	/// Represents an action map that defines actions that are used to navigate the menus.
	/// </summary>
	[ActionMap("menu")]
	public static class MenuActionMap
	{
		/// <summary>
		/// An action that is used in "Weapon customization" menu to switch to barrel attachment.
		/// </summary>
		[Action("menu_toggle_barrel", ActionActivationMode.OnPress,
			KeyboardMouseInput = InputId.RowDigit1, XboxInput = InputId.XboxY, OrbisInput = InputId.OrbisTriangle)]
		public static event InputActionHandler MenuToggleBarrel;
		/// <summary>
		/// An action that is used in "Weapon customization" menu to switch to bottom.
		/// </summary>
		[Action("menu_toggle_bottom", ActionActivationMode.OnPress,
			KeyboardMouseInput = InputId.RowDigit2, XboxInput = InputId.XboxX, OrbisInput = InputId.OrbisSquare)]
		public static event InputActionHandler MenuToggleBottom;
		/// <summary>
		/// An action that is used in "Weapon customization" menu to switch to ammo.
		/// </summary>
		[Action("menu_toggle_ammo", ActionActivationMode.OnPress,
			KeyboardMouseInput = InputId.RowDigit3, XboxInput = InputId.XboxA, OrbisInput = InputId.OrbisCross)]
		public static event InputActionHandler MenuToggleAmmo;
		/// <summary>
		/// An action that is used in "Weapon customization" menu to switch to scope.
		/// </summary>
		[Action("menu_toggle_scope", ActionActivationMode.OnPress,
			KeyboardMouseInput = InputId.RowDigit4, XboxInput = InputId.XboxB, OrbisInput = InputId.OrbisCircle)]
		public static event InputActionHandler MenuToggleScope;

		/// <summary>
		/// An action that is used in "Nanosuit customization" menu to switch to upgrade displayed near the
		/// index finger.
		/// </summary>
		[Action("menu_toggle_index_finger",
			ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.OnHold,
			holdTriggerDelay: 2.0f, holdRepeatDelay: -1.0f,
			KeyboardMouseInput = InputId.RowDigit1, XboxInput = InputId.XboxY, OrbisInput = InputId.OrbisTriangle)]
		public static event InputActionHandler MenuToggleIndexFinger;
		/// <summary>
		/// An action that is used in "Nanosuit customization" menu to switch to upgrade displayed near the
		/// middle finger.
		/// </summary>
		[Action("menu_toggle_middle_finger",
			ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.OnHold,
			holdTriggerDelay: 2.0f, holdRepeatDelay: -1.0f,
			KeyboardMouseInput = InputId.RowDigit2, XboxInput = InputId.XboxX, OrbisInput = InputId.OrbisSquare)]
		public static event InputActionHandler MenuToggleMiddleFinger;
		/// <summary>
		/// An action that is used in "Nanosuit customization" menu to switch to upgrade displayed near the
		/// ring finger.
		/// </summary>
		[Action("menu_toggle_ring_finger",
			ActionActivationMode.OnPress | ActionActivationMode.OnRelease | ActionActivationMode.OnHold,
			holdTriggerDelay: 2.0f, holdRepeatDelay: -1.0f,
			KeyboardMouseInput = InputId.RowDigit3, XboxInput = InputId.XboxB, OrbisInput = InputId.OrbisCircle)]
		public static event InputActionHandler MenuToggleRingFinger;

		/// <summary>
		/// An action that is used in "Asset showcase" menu to pause/unpause the display animation.
		/// </summary>
		[Action("menu_assetpause", ActionActivationMode.OnPress,
			KeyboardMouseInput = InputId.P, XboxInput = InputId.XboxX, OrbisInput = InputId.OrbisSquare)]
		public static event InputActionHandler MenuAssetPause;
		/// <summary>
		/// An action that is used in "Asset showcase" menu to change the zoom level.
		/// </summary>
		[Action("menu_assetzoom", ActionActivationMode.OnPress,
			KeyboardMouseInput = InputId.Z, XboxInput = InputId.XboxTriggerLeftButton, OrbisInput = InputId.OrbisL2)]
		public static event InputActionHandler MenuAssetZoom;

		/// <summary>
		/// An action that is used in the menu to delete items.
		/// </summary>
		[Action("menu_delete", ActionActivationMode.OnPress, XboxInput = InputId.XboxX, OrbisInput = InputId.OrbisSquare)]
		[InputAction(InputId.Backspace)]
		[InputAction(InputId.Delete)]
		public static event InputActionHandler MenuDelete;

		/// <summary>
		/// An action that is used in the menu to apply new settings.
		/// </summary>
		[Action("menu_apply", ActionActivationMode.OnPress,
			XboxInput = InputId.XboxA, OrbisInput = InputId.OrbisCross)]
		public static event InputActionHandler MenuApply;
		/// <summary>
		/// An action that is used in the menu to choose the default option.
		/// </summary>
		[Action("menu_default", ActionActivationMode.OnPress,
			XboxInput = InputId.XboxY, OrbisInput = InputId.OrbisTriangle)]
		public static event InputActionHandler MenuDefault;
		/// <summary>
		/// An action that is used in the menu to switch to another tab.
		/// </summary>
		[Action("menu_tab", ActionActivationMode.OnPress, KeyboardMouseInput = InputId.Tab)]
		public static event InputActionHandler MenuTab;

		/// <summary>
		/// An action that is used in minimap menu to zoom out.
		/// </summary>
		[Action("menu_map_zoomout")]
		[InputAction(InputId.MouseWheelDown, ActionActivationMode.OnPress)]
		[InputAction(InputId.XboxTriggerLeftButton, ActionActivationMode.OnPress)]
		[InputAction(InputId.OrbisL1, ActionActivationMode.OnPress)]
		public static event InputActionHandler MenuMapZoomOut;
		/// <summary>
		/// An action that is used in minimap menu to zoom in.
		/// </summary>
		[Action("menu_map_zoomin")]
		[InputAction(InputId.MouseWheelUp, ActionActivationMode.OnPress)]
		[InputAction(InputId.XboxTriggerLeftButton, ActionActivationMode.OnRelease)]
		[InputAction(InputId.OrbisL1, ActionActivationMode.OnRelease)]
		public static event InputActionHandler MenuMapZoomIn;

		/// <summary>
		/// An action that is used in the menu to scroll up.
		/// </summary>
		[Action("menu_scrollup", ActionActivationMode.OnPress, KeyboardMouseInput = InputId.MouseWheelUp)]
		public static event InputActionHandler MenuScrollUp;
		/// <summary>
		/// An action that is used in the menu to scroll down.
		/// </summary>
		[Action("menu_scrolldown", ActionActivationMode.OnPress, KeyboardMouseInput = InputId.MouseWheelDown)]
		public static event InputActionHandler MenuScrollDown;
		/// <summary>
		/// An action that is used in the menu to call first function command.
		/// </summary>
		[Action("menu_fcommand1", ActionActivationMode.OnPress, KeyboardMouseInput = InputId.F1)]
		public static event InputActionHandler MenuFunctionCommand1;
		/// <summary>
		/// An action that is used in the menu to call second function command.
		/// </summary>
		[Action("menu_fcommand2", ActionActivationMode.OnPress, KeyboardMouseInput = InputId.F2)]
		public static event InputActionHandler MenuFunctionCommand2;
		/// <summary>
		/// An action that is used in the menu to switch to the tab to the left.
		/// </summary>
		[Action("menu_switchtab_left", ActionActivationMode.OnPress | ActionActivationMode.OnHold,
			holdTriggerDelay: 0.3f, holdRepeatDelay: 0.15f,
			KeyboardMouseInput = InputId.PageUp, XboxInput = InputId.XboxShoulderLeft, OrbisInput = InputId.OrbisL2)]
		public static event InputActionHandler MenuSwitchTabLeft;
		/// <summary>
		/// An action that is used in the menu to switch to the tab to the right.
		/// </summary>
		[Action("menu_switchtab_right", ActionActivationMode.OnPress | ActionActivationMode.OnHold,
			holdTriggerDelay: 0.3f, holdRepeatDelay: 0.15f,
			KeyboardMouseInput = InputId.PageDown, XboxInput = InputId.XboxShoulderRight, OrbisInput = InputId.OrbisR2)]
		public static event InputActionHandler MenuSwitchTabRight;

		/// <summary>
		/// An action that is used in the menu to move up.
		/// </summary>
		[Action("menu_up", ActionActivationMode.OnPress | ActionActivationMode.OnHold,
			holdTriggerDelay: 0.3f, holdRepeatDelay: 0.15f)]
		[InputAction(InputId.Up)]
		[InputAction(InputId.XboxDPadUp)]
		[InputAction(InputId.XboxThumbLeftY, op: AnalogComparisonOperation.GreaterThan, analogCompareValue: 0.5f)]
		[InputAction(InputId.OrbisUp)]
		[InputAction(InputId.OrbisStickLeftY, op: AnalogComparisonOperation.GreaterThan, analogCompareValue: 0.5f)]
		public static event InputActionHandler MenuUp;
		/// <summary>
		/// An action that is used in the menu to move down.
		/// </summary>
		[Action("menu_down", ActionActivationMode.OnPress | ActionActivationMode.OnHold,
			holdTriggerDelay: 0.3f, holdRepeatDelay: 0.15f)]
		[InputAction(InputId.Down)]
		[InputAction(InputId.XboxDPadDown)]
		[InputAction(InputId.XboxThumbLeftY, op: AnalogComparisonOperation.LessThan, analogCompareValue: -0.5f)]
		[InputAction(InputId.XboxThumbRightY, op: AnalogComparisonOperation.LessThan, analogCompareValue: -0.5f)]
		[InputAction(InputId.OrbisDown)]
		[InputAction(InputId.OrbisStickLeftY, op: AnalogComparisonOperation.LessThan, analogCompareValue: -0.5f)]
		[InputAction(InputId.OrbisStickRightY, op: AnalogComparisonOperation.LessThan, analogCompareValue: -0.5f)]
		public static event InputActionHandler MenuDown;
		/// <summary>
		/// An action that is used in the menu to move left.
		/// </summary>
		[Action("menu_left", ActionActivationMode.OnPress | ActionActivationMode.OnHold,
			holdTriggerDelay: 0.3f, holdRepeatDelay: 0.15f)]
		[InputAction(InputId.Left)]
		[InputAction(InputId.XboxDPadLeft)]
		[InputAction(InputId.XboxThumbLeftX, op: AnalogComparisonOperation.LessThan, analogCompareValue: -0.5f)]
		[InputAction(InputId.XboxThumbRightX, op: AnalogComparisonOperation.LessThan, analogCompareValue: -0.5f)]
		[InputAction(InputId.OrbisLeft)]
		[InputAction(InputId.OrbisStickLeftX, op: AnalogComparisonOperation.LessThan, analogCompareValue: -0.5f)]
		[InputAction(InputId.OrbisStickRightX, op: AnalogComparisonOperation.LessThan, analogCompareValue: -0.5f)]
		public static event InputActionHandler MenuLeft;
		/// <summary>
		/// An action that is used in the menu to move right.
		/// </summary>
		[Action("menu_right", ActionActivationMode.OnPress | ActionActivationMode.OnHold,
			holdTriggerDelay: 0.3f, holdRepeatDelay: 0.15f)]
		[InputAction(InputId.Right)]
		[InputAction(InputId.XboxDPadRight)]
		[InputAction(InputId.XboxThumbLeftX, op: AnalogComparisonOperation.GreaterThan, analogCompareValue: 0.5f)]
		[InputAction(InputId.XboxThumbRightX, op: AnalogComparisonOperation.GreaterThan, analogCompareValue: 0.5f)]
		[InputAction(InputId.OrbisRight)]
		[InputAction(InputId.OrbisStickLeftX, op: AnalogComparisonOperation.GreaterThan, analogCompareValue: 0.5f)]
		[InputAction(InputId.OrbisStickRightX, op: AnalogComparisonOperation.GreaterThan, analogCompareValue: 0.5f)]
		public static event InputActionHandler MenuRight;

		/// <summary>
		/// An action that is used in the menu to confirm some action.
		/// </summary>
		[Action("menu_confirm", ActionActivationMode.OnPress,
			KeyboardMouseInput = InputId.Enter, XboxInput = InputId.XboxA, OrbisInput = InputId.OrbisCross)]
		public static event InputActionHandler MenuConfirm;
		/// <summary>
		/// An action that is used in the menu to confirm some action.
		/// </summary>
		[Action("menu_confirm2", ActionActivationMode.OnPress, KeyboardMouseInput = InputId.NumpadEnter)]
		public static event InputActionHandler MenuConfirm2;
		/// <summary>
		/// An action that is used in the menu to move to the previous one.
		/// </summary>
		[Action("menu_back", ActionActivationMode.OnPress,
			XboxInput = InputId.XboxB, OrbisInput = InputId.OrbisCircle)]
		public static event InputActionHandler MenuBack;
		/// <summary>
		/// An action that is used to exit the menu.
		/// </summary>
		[Action("menu_exit", ActionActivationMode.OnPress, KeyboardMouseInput = InputId.Escape)]
		public static event InputActionHandler MenuExit;
		/// <summary>
		/// An action that is used in the menu to something.
		/// </summary>
		[Action("menu_back_select", ActionActivationMode.OnPress,
			XboxInput = InputId.XboxBack, OrbisInput = InputId.OrbisTouch)]
		public static event InputActionHandler MenuBackSelect;
		/// <summary>
		/// An action that is used to open the friends list.
		/// </summary>
		[Action("menu_friends_open", ActionActivationMode.OnPress,
			XboxInput = InputId.XboxBack, OrbisInput = InputId.OrbisTouch)]
		public static event InputActionHandler MenuFriendsOpen;

		/// <summary>
		/// An action that is used to open a scoreboard menu.
		/// </summary>
		[Action("menu_scoreboard_open")]
		[InputAction(InputId.Tab, ActionActivationMode.OnPress)]
		[InputAction(InputId.XboxBack, ActionActivationMode.OnHold, holdTriggerDelay: 0.5f, holdRepeatDelay: -1)]
		[InputAction(InputId.OrbisTouch, ActionActivationMode.OnHold, holdTriggerDelay: 0.5f, holdRepeatDelay: -1)]
		public static event InputActionHandler MenuScoreboardOpen;
		/// <summary>
		/// An action that is used to close a scoreboard menu.
		/// </summary>
		[Action("menu_scoreboard_close")]
		[InputAction(InputId.Tab, ActionActivationMode.OnRelease)]
		[InputAction(InputId.XboxBack, ActionActivationMode.OnPress | ActionActivationMode.OnRelease)]
		[InputAction(InputId.OrbisTouch, ActionActivationMode.OnPress | ActionActivationMode.OnRelease)]
		public static event InputActionHandler MenuScoreboardClose;

		/// <summary>
		/// An action that is used in the menu to something.
		/// </summary>
		[Action("menu_input_1", ActionActivationMode.OnPress,
			XboxInput = InputId.XboxY, OrbisInput = InputId.OrbisTriangle)]
		public static event InputActionHandler MenuInput1;
		/// <summary>
		/// An action that is used in the menu to something.
		/// </summary>
		[Action("menu_input_2", ActionActivationMode.OnPress,
			XboxInput = InputId.XboxX, OrbisInput = InputId.OrbisSquare)]
		public static event InputActionHandler MenuInput2;
		/// <summary>
		/// An action that is used in the menu to something.
		/// </summary>
		/// <remarks>This might a be a rudiment from Crysis nanosuit mode selection menu.</remarks>
		[Action("mouse_wheel_infiction_close", ActionActivationMode.OnRelease, KeyboardMouseInput = InputId.Mouse3)]
		public static event InputActionHandler MouseWheelInfictionClose;
	}
}

// Never hurts to restore the disabled warning.

#pragma warning restore 67