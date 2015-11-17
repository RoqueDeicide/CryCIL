using System;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Enumeration of recognizable types of input devices.
	/// </summary>
	public enum InputDeviceType
	{
		/// <summary>
		/// Identifier of the keyboard input device.
		/// </summary>
		Keyboard,
		/// <summary>
		/// Identifier of the mouse input device.
		/// </summary>
		Mouse,
		/// <summary>
		/// Identifier of the joystick input device.
		/// </summary>
		Joystick,
		/// <summary>
		/// Identifier of the gamepad input device.
		/// </summary>
		Gamepad,
		/// <summary>
		/// Identifier of the head-mounted input device.
		/// </summary>
		Headmounted,
		/// <summary>
		/// Identifier of the unknown input device.
		/// </summary>
		Unknown = 0xff
	}
}