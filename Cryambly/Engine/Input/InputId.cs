using System;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Enumeration of Id starter numbers for various input devices.
	/// </summary>
	public enum InputIdBase : uint
	{
		/// <summary>
		/// Id of the first keyboard input.
		/// </summary>
		Keyboard = 0,
		/// <summary>
		/// Id of the first mouse input.
		/// </summary>
		Mouse = 256,
		/// <summary>
		/// Id of the first Xbox controller input.
		/// </summary>
		XboxController = 512,
		/// <summary>
		/// Id of the first Orbis controller input.
		/// </summary>
		OrbisController = 1024,
		/// <summary>
		/// Id of the first system input.
		/// </summary>
		SystemInput = 4096
	}
	/// <summary>
	/// Enumeration of input identifiers for all known devices.
	/// </summary>
	public enum InputId : uint
	{
		/// <summary>
		/// Identifier of the Escape key.
		/// </summary>
		Escape = InputIdBase.Keyboard,
		/// <summary>
		/// Identifier of the digit 1 on the default row of number keys on the keyboard.
		/// </summary>
		RowDigit1,
		/// <summary>
		/// Identifier of the digit 2 on the default row of number keys on the keyboard.
		/// </summary>
		RowDigit2,
		/// <summary>
		/// Identifier of the digit 3 on the default row of number keys on the keyboard.
		/// </summary>
		RowDigit3,
		/// <summary>
		/// Identifier of the digit 4 on the default row of number keys on the keyboard.
		/// </summary>
		RowDigit4,
		/// <summary>
		/// Identifier of the digit 5 on the default row of number keys on the keyboard.
		/// </summary>
		RowDigit5,
		/// <summary>
		/// Identifier of the digit 6 on the default row of number keys on the keyboard.
		/// </summary>
		RowDigit6,
		/// <summary>
		/// Identifier of the digit 7 on the default row of number keys on the keyboard.
		/// </summary>
		RowDigit7,
		/// <summary>
		/// Identifier of the digit 8 on the default row of number keys on the keyboard.
		/// </summary>
		RowDigit8,
		/// <summary>
		/// Identifier of the digit 9 on the default row of number keys on the keyboard.
		/// </summary>
		RowDigit9,
		/// <summary>
		/// Identifier of the digit 0 on the default row of number keys on the keyboard.
		/// </summary>
		RowDigit0,
		/// <summary>
		/// Identifier of the minus sign key.
		/// </summary>
		Minus,
		/// <summary>
		/// Identifier of the equals sign key.
		/// </summary>
		Equals,
		/// <summary>
		/// Identifier of the Backspace key.
		/// </summary>
		Backspace,
		/// <summary>
		/// Identifier of the Tab key.
		/// </summary>
		Tab,
		/// <summary>
		/// Identifier of the letter Q key.
		/// </summary>
		Q,
		/// <summary>
		/// Identifier of the letter W key.
		/// </summary>
		W,
		/// <summary>
		/// Identifier of the letter E key.
		/// </summary>
		E,
		/// <summary>
		/// Identifier of the letter R key.
		/// </summary>
		R,
		/// <summary>
		/// Identifier of the letter T key.
		/// </summary>
		T,
		/// <summary>
		/// Identifier of the letter Y key.
		/// </summary>
		Y,
		/// <summary>
		/// Identifier of the letter U key.
		/// </summary>
		U,
		/// <summary>
		/// Identifier of the letter I key.
		/// </summary>
		I,
		/// <summary>
		/// Identifier of the letter O key.
		/// </summary>
		O,
		/// <summary>
		/// Identifier of the letter P key.
		/// </summary>
		P,
		/// <summary>
		/// Identifier of the left bracket '[' key.
		/// </summary>
		LeftBracket,
		/// <summary>
		/// Identifier of the right bracket ']' key.
		/// </summary>
		RightBracket,
		/// <summary>
		/// Identifier of the Enter (Return) key.
		/// </summary>
		Enter,
		/// <summary>
		/// Identifier of the left control modifier key.
		/// </summary>
		LeftCtrl,
		/// <summary>
		/// Identifier of the letter A key.
		/// </summary>
		A,
		/// <summary>
		/// Identifier of the letter S key.
		/// </summary>
		S,
		/// <summary>
		/// Identifier of the letter D key.
		/// </summary>
		D,
		/// <summary>
		/// Identifier of the letter F key.
		/// </summary>
		F,
		/// <summary>
		/// Identifier of the letter G key.
		/// </summary>
		G,
		/// <summary>
		/// Identifier of the letter H key.
		/// </summary>
		H,
		/// <summary>
		/// Identifier of the letter J key.
		/// </summary>
		J,
		/// <summary>
		/// Identifier of the letter K key.
		/// </summary>
		K,
		/// <summary>
		/// Identifier of the letter L key.
		/// </summary>
		L,
		/// <summary>
		/// Identifier of the semicolon ';' key.
		/// </summary>
		Semicolon,
		/// <summary>
		/// Identifier of the apostrophe "'" key.
		/// </summary>
		Apostrophe,
		/// <summary>
		/// Identifier of the tilde '~' key.
		/// </summary>
		Tilde,
		/// <summary>
		/// Identifier of the left shift modifier key.
		/// </summary>
		LeftShift,
		/// <summary>
		/// Identifier of the backlash '\' key.
		/// </summary>
		Backslash,
		/// <summary>
		/// Identifier of the letter Z key.
		/// </summary>
		Z,
		/// <summary>
		/// Identifier of the letter X key.
		/// </summary>
		X,
		/// <summary>
		/// Identifier of the letter C key.
		/// </summary>
		C,
		/// <summary>
		/// Identifier of the letter V key.
		/// </summary>
		V,
		/// <summary>
		/// Identifier of the letter B key.
		/// </summary>
		B,
		/// <summary>
		/// Identifier of the letter N key.
		/// </summary>
		N,
		/// <summary>
		/// Identifier of the letter M key.
		/// </summary>
		M,
		/// <summary>
		/// Identifier of the comma ',' key.
		/// </summary>
		Comma,
		/// <summary>
		/// Identifier of the period '.' key.
		/// </summary>
		Period,
		/// <summary>
		/// Identifier of the slash '/' key.
		/// </summary>
		Slash,
		/// <summary>
		/// Identifier of the right shift modifier key.
		/// </summary>
		RightShift,
		/// <summary>
		/// Identifier of the multiply '*' key on the numpad.
		/// </summary>
		NumpadMultiply,
		/// <summary>
		/// Identifier of the left alt modifier key.
		/// </summary>
		LeftAlt,
		/// <summary>
		/// Identifier of the space ' ' key.
		/// </summary>
		Space,
		/// <summary>
		/// Identifier of the caps lock modifier key.
		/// </summary>
		CapsLock,
		/// <summary>
		/// Identifier of the first function key (F1).
		/// </summary>
		F1,
		/// <summary>
		/// Identifier of the second function key (F2).
		/// </summary>
		F2,
		/// <summary>
		/// Identifier of the third function key (F3).
		/// </summary>
		F3,
		/// <summary>
		/// Identifier of the fourth function key (F4).
		/// </summary>
		F4,
		/// <summary>
		/// Identifier of the fifth function key (F5).
		/// </summary>
		F5,
		/// <summary>
		/// Identifier of the sixth function key (F6).
		/// </summary>
		F6,
		/// <summary>
		/// Identifier of the seventh function key (F7).
		/// </summary>
		F7,
		/// <summary>
		/// Identifier of the eighth function key (F8).
		/// </summary>
		F8,
		/// <summary>
		/// Identifier of the ninth function key (F9).
		/// </summary>
		F9,
		/// <summary>
		/// Identifier of the tenth function key (F10).
		/// </summary>
		F10,
		/// <summary>
		/// Identifier of the num lock modifier key.
		/// </summary>
		NumLock,
		/// <summary>
		/// Identifier of the scroll lock modifier key.
		/// </summary>
		ScrollLock,
		/// <summary>
		/// Identifier of the digit 7 on the numpad.
		/// </summary>
		Numpad7,
		/// <summary>
		/// Identifier of the digit 8 on the numpad.
		/// </summary>
		Numpad8,
		/// <summary>
		/// Identifier of the digit 9 on the numpad.
		/// </summary>
		Numpad9,
		/// <summary>
		/// Identifier of the subtract '-' key on the numpad.
		/// </summary>
		NumpadSubstract,
		/// <summary>
		/// Identifier of the digit 4 on the numpad.
		/// </summary>
		Numpad4,
		/// <summary>
		/// Identifier of the digit 5 on the numpad.
		/// </summary>
		Numpad5,
		/// <summary>
		/// Identifier of the digit 6 on the numpad.
		/// </summary>
		Numpad6,
		/// <summary>
		/// Identifier of the add '+' key on the numpad.
		/// </summary>
		NumpadAdd,
		/// <summary>
		/// Identifier of the digit 1 on the numpad.
		/// </summary>
		Numpad1,
		/// <summary>
		/// Identifier of the digit 2 on the numpad.
		/// </summary>
		Numpad2,
		/// <summary>
		/// Identifier of the digit 3 on the numpad.
		/// </summary>
		Numpad3,
		/// <summary>
		/// Identifier of the digit 0 on the numpad.
		/// </summary>
		Numpad0,
		/// <summary>
		/// Identifier of the eleventh function key (F11).
		/// </summary>
		F11,
		/// <summary>
		/// Identifier of the twelveth function key (F12).
		/// </summary>
		F12,
		/// <summary>
		/// Identifier of the thirteenth function key (F13).
		/// </summary>
		F13,
		/// <summary>
		/// Identifier of the fourteenth function key (F14).
		/// </summary>
		F14,
		/// <summary>
		/// Identifier of the fifteenth function key (F15).
		/// </summary>
		F15,
		/// <summary>
		/// Identifier of the colon ':' key.
		/// </summary>
		Colon,
		/// <summary>
		/// Identifier of the underline '_' key.
		/// </summary>
		Underline,
		/// <summary>
		/// Identifier of the enter key on the numpad.
		/// </summary>
		NumpadEnter,
		/// <summary>
		/// Identifier of the right control modifier key.
		/// </summary>
		RightCtrl,
		/// <summary>
		/// Identifier of the period '.' key on the numpad.
		/// </summary>
		NumpadPeriod,
		/// <summary>
		/// Identifier of the divide '/' key on the numpad.
		/// </summary>
		NumpadDivide,
		/// <summary>
		/// Identifier of the Print key.
		/// </summary>
		Print,
		/// <summary>
		/// Identifier of the right alt modifier key.
		/// </summary>
		RightAlt,
		/// <summary>
		/// Identifier of the Pause key.
		/// </summary>
		Pause,
		/// <summary>
		/// Identifier of the Home key.
		/// </summary>
		Home,
		/// <summary>
		/// Identifier of the Up-arrow key.
		/// </summary>
		Up,
		/// <summary>
		/// Identifier of the Page Up key.
		/// </summary>
		PageUp,
		/// <summary>
		/// Identifier of the Left-arrow key.
		/// </summary>
		Left,
		/// <summary>
		/// Identifier of the Right-arrow key.
		/// </summary>
		Right,
		/// <summary>
		/// Identifier of the End key.
		/// </summary>
		End,
		/// <summary>
		/// Identifier of the Down-arrow key.
		/// </summary>
		Down,
		/// <summary>
		/// Identifier of the Page Down key.
		/// </summary>
		PageDown,
		/// <summary>
		/// Identifier of the Insert key.
		/// </summary>
		Insert,
		/// <summary>
		/// Identifier of the Delete key.
		/// </summary>
		Delete,
		/// <summary>
		/// Identifier of the left windows modifier key.
		/// </summary>
		LeftWindows,
		/// <summary>
		/// Identifier of the right windows modifier key.
		/// </summary>
		RightWindows,
		/// <summary>
		/// Identifier of the right windows modifier key.
		/// </summary>
		Apps,
		/// <summary>
		/// Identifier of some OEM-defined key.
		/// </summary>
		OEM_102,

		/// <summary>
		/// Identifier of the left mouse button.
		/// </summary>
		Mouse1 = InputIdBase.Mouse,
		/// <summary>
		/// Identifier of the right mouse button.
		/// </summary>
		Mouse2,
		/// <summary>
		/// Identifier of the middle mouse button.
		/// </summary>
		Mouse3,
		/// <summary>
		/// Identifier of the additional mouse button #4.
		/// </summary>
		Mouse4,
		/// <summary>
		/// Identifier of the additional mouse button #5.
		/// </summary>
		Mouse5,
		/// <summary>
		/// Identifier of the additional mouse button #6.
		/// </summary>
		Mouse6,
		/// <summary>
		/// Identifier of the additional mouse button #7.
		/// </summary>
		Mouse7,
		/// <summary>
		/// Identifier of the additional mouse button #8.
		/// </summary>
		Mouse8,
		/// <summary>
		/// Identifier of the mouse wheel up-input.
		/// </summary>
		MouseWheelUp,
		/// <summary>
		/// Identifier of the mouse wheel down-input.
		/// </summary>
		MouseWheelDown,
		/// <summary>
		/// Identifier of the event of the mouse changing its X-coordinate(?).
		/// </summary>
		MouseX,
		/// <summary>
		/// Identifier of the event of the mouse changing its Y-coordinate(?).
		/// </summary>
		MouseY,
		/// <summary>
		/// Identifier of the event of the mouse changing its Z-coordinate(?).
		/// </summary>
		MouseZ,
		/// <summary>
		/// Identifier of the event of the mouse changing its absolute X-coordinate(?).
		/// </summary>
		MouseXAbsolute,
		/// <summary>
		/// Identifier of the event of the mouse changing its absolute Y-coordinate(?).
		/// </summary>
		MouseYAbsolute,
		/// <summary>
		/// ?
		/// </summary>
		MouseLast,

		/// <summary>
		/// Identifier of the Up direction on the D-Pad of the Xbox360 controller.
		/// </summary>
		XboxDPadUp = InputIdBase.XboxController,
		/// <summary>
		/// Identifier of the Down direction on the D-Pad of the Xbox360 controller.
		/// </summary>
		XboxDPadDown,
		/// <summary>
		/// Identifier of the Left direction on the D-Pad of the Xbox360 controller.
		/// </summary>
		XboxDPadLeft,
		/// <summary>
		/// Identifier of the Right direction on the D-Pad of the Xbox360 controller.
		/// </summary>
		XboxDPadRight,
		/// <summary>
		/// Identifier of the Start button on the Xbox360 controller.
		/// </summary>
		XboxStart,
		/// <summary>
		/// Identifier of the Back button on the Xbox360 controller.
		/// </summary>
		XboxBack,
		/// <summary>
		/// Identifier of the left analog stick click on the Xbox360 controller.
		/// </summary>
		XboxThumbLeft,
		/// <summary>
		/// Identifier of the right analog stick click on the Xbox360 controller.
		/// </summary>
		XboxThumbRight,
		/// <summary>
		/// Identifier of the click with a left shoulder of the Xbox360 controller.
		/// </summary>
		XboxShoulderLeft,
		/// <summary>
		/// Identifier of the click with a right shoulder of the Xbox360 controller.
		/// </summary>
		XboxShoulderRight,
		/// <summary>
		/// Identifier of the A button on the Xbox360 controller.
		/// </summary>
		XboxA,
		/// <summary>
		/// Identifier of the B button on the Xbox360 controller.
		/// </summary>
		XboxB,
		/// <summary>
		/// Identifier of the X button on the Xbox360 controller.
		/// </summary>
		XboxX,
		/// <summary>
		/// Identifier of the Y button on the Xbox360 controller.
		/// </summary>
		XboxY,
		/// <summary>
		/// Identifier of the input with a left trigger on the Xbox360 controller.
		/// </summary>
		XboxTriggerLeft,
		/// <summary>
		/// Identifier of the input with a right trigger on the Xbox360 controller.
		/// </summary>
		XboxTriggerRight,
		/// <summary>
		/// Identifier of the change in X-coordinate of the left analog stick of the Xbox360 controller.
		/// </summary>
		XboxThumbLeftX,
		/// <summary>
		/// Identifier of the change in Y-coordinate of the left analog stick of the Xbox360 controller.
		/// </summary>
		XboxThumbLeftY,
		/// <summary>
		/// Identifier of the Up direction on the left analog stick of the Xbox360 controller.
		/// </summary>
		XboxThumbLeftUp,
		/// <summary>
		/// Identifier of the Down direction on the left analog stick of the Xbox360 controller.
		/// </summary>
		XboxThumbLeftDown,
		/// <summary>
		/// Identifier of the Left direction on the left analog stick of the Xbox360 controller.
		/// </summary>
		XboxThumbLeftLeft,
		/// <summary>
		/// Identifier of the Right direction on the left analog stick of the Xbox360 controller.
		/// </summary>
		XboxThumbLeftRight,
		/// <summary>
		/// Identifier of the change in X-coordinate of the right analog stick of the Xbox360 controller.
		/// </summary>
		XboxThumbRightX,
		/// <summary>
		/// Identifier of the change in Y-coordinate of the right analog stick of the Xbox360 controller.
		/// </summary>
		XboxThumbRightY,
		/// <summary>
		/// Identifier of the Up direction on the right analog stick of the Xbox360 controller.
		/// </summary>
		XboxThumbRightUp,
		/// <summary>
		/// Identifier of the Down direction on the right analog stick of the Xbox360 controller.
		/// </summary>
		XboxThumbRightDown,
		/// <summary>
		/// Identifier of the Left direction on the right analog stick of the Xbox360 controller.
		/// </summary>
		XboxThumbRightLeft,
		/// <summary>
		/// Identifier of the Right direction on the right analog stick of the Xbox360 controller.
		/// </summary>
		XboxThumbRightRight,
		/// <summary>
		/// Identifier of the event when the left trigger of the Xbox360 controller is pressed and released
		/// in quick succession.
		/// </summary>
		XboxTriggerLeftButton,
		/// <summary>
		/// Identifier of the event when the right trigger of the Xbox360 controller is pressed and
		/// released in quick succession.
		/// </summary>
		XboxTriggerRightButton,
		/// <summary>
		/// Deprecated.
		/// </summary>
		XboxConnect,
		/// <summary>
		/// Deprecated.
		/// </summary>
		XboxDisconnect,

		/// <summary>
		/// Identifier of the Select button on the Orbis controller.
		/// </summary>
		OrbisSelect = InputIdBase.OrbisController,
		/// <summary>
		/// Identifier of the L3 analog stick on the Orbis controller.
		/// </summary>
		OrbisL3,
		/// <summary>
		/// Identifier of the R3 analog stick on the Orbis controller.
		/// </summary>
		OrbisR3,
		/// <summary>
		/// Identifier of the Start button on the Orbis controller.
		/// </summary>
		OrbisStart,
		/// <summary>
		/// Identifier of the Up button on the Orbis controller.
		/// </summary>
		OrbisUp,
		/// <summary>
		/// Identifier of the Right button on the Orbis controller.
		/// </summary>
		OrbisRight,
		/// <summary>
		/// Identifier of the Down button on the Orbis controller.
		/// </summary>
		OrbisDown,
		/// <summary>
		/// Identifier of the Left button on the Orbis controller.
		/// </summary>
		OrbisLeft,
		/// <summary>
		/// Identifier of the L2 button on the Orbis controller.
		/// </summary>
		OrbisL2,
		/// <summary>
		/// Identifier of the R2 button on the Orbis controller.
		/// </summary>
		OrbisR2,
		/// <summary>
		/// Identifier of the L1 button on the Orbis controller.
		/// </summary>
		OrbisL1,
		/// <summary>
		/// Identifier of the R1 button on the Orbis controller.
		/// </summary>
		OrbisR1,
		/// <summary>
		/// Identifier of the Triangle button on the Orbis controller.
		/// </summary>
		OrbisTriangle,
		/// <summary>
		/// Identifier of the Circle button on the Orbis controller.
		/// </summary>
		OrbisCircle,
		/// <summary>
		/// Identifier of the Cross button on the Orbis controller.
		/// </summary>
		OrbisCross,
		/// <summary>
		/// Identifier of the Square button on the Orbis controller.
		/// </summary>
		OrbisSquare,
		/// <summary>
		/// Identifier of the X-axis with left stick on the Orbis controller.
		/// </summary>
		OrbisStickLeftX,
		/// <summary>
		/// Identifier of the Y-axis with left stick on the Orbis controller.
		/// </summary>
		OrbisStickLeftY,
		/// <summary>
		/// Identifier of the X-axis with right stick on the Orbis controller.
		/// </summary>
		OrbisStickRightX,
		/// <summary>
		/// Identifier of the Y-axis with right stick on the Orbis controller.
		/// </summary>
		OrbisStickRightY,
		/// <summary>
		/// Unknown.
		/// </summary>
		OrbisRotX,
		/// <summary>
		/// Unknown.
		/// </summary>
		OrbisRotY,
		/// <summary>
		/// Unknown.
		/// </summary>
		OrbisRotZ,
		/// <summary>
		/// Unknown.
		/// </summary>
		OrbisRotXKeyL,
		/// <summary>
		/// Unknown.
		/// </summary>
		OrbisRotXKeyR,
		/// <summary>
		/// Unknown.
		/// </summary>
		OrbisRotZKeyD,
		/// <summary>
		/// Unknown.
		/// </summary>
		OrbisRotZKeyU,

		/// <summary>
		/// Identifier of the touch pad on the Play Station 4 controller.
		/// </summary>
		OrbisTouch,

		/// <summary>
		/// For internal usage.
		/// </summary>
		SystemCommit = InputIdBase.SystemInput,
		/// <summary>
		/// Identifier of the connection of the new input device to the computer.
		/// </summary>
		SystemConnectDevice,
		/// <summary>
		/// Identifier of the disconnection of one the input devices.
		/// </summary>
		SystemDisconnectDevice,

		/// <summary>
		/// Unknown.
		/// </summary>
		Unknown = 0xffffffff
	}
}