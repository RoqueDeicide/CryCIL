using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CryCil.Utilities;

namespace CryCil.Engine.Input
{
	internal static class InputEventPropagator
	{
		internal static bool Post(List<KeyInputHandler> handlers, InputId input, ModifierKeysStatus modifiers, bool pressed)
		{
			// I'm using for loop in this way to make sure there are no temporary managed objects.
			for (int i = 0; i < handlers.Count; i++)
			{
				if (handlers[i](input, pressed, modifiers))
				{
					return true;
				}
			}
			return false;
		}
		internal static bool Post(List<SymbolicInputHandler> handlers, Utf32Char symbol)
		{
			// I'm using for loop in this way to make sure there are no temporary managed objects.
			for (int i = 0; i < handlers.Count; i++)
			{
				if (handlers[i](symbol))
				{
					return true;
				}
			}
			return false;
		}
		internal static bool Post(List<AnalogInputHandler> handlers, InputId input, ModifierKeysStatus modifiers,
								  InputState state, float value)
		{
			// I'm using for loop in this way to make sure there are no temporary managed objects.
			for (int i = 0; i < handlers.Count; i++)
			{
				if (handlers[i](input, state, value, modifiers))
				{
					return true;
				}
			}
			return false;
		}
		internal static bool Post(List<GamepadAnalogInputHandler> handlers, InputId input, byte deviceIndex,
								  InputState state, float value)
		{
			// I'm using for loop in this way to make sure there are no temporary managed objects.
			for (int i = 0; i < handlers.Count; i++)
			{
				if (handlers[i](input, state, value, deviceIndex))
				{
					return true;
				}
			}
			return false;
		}
		internal static bool Post(List<GamepadKeyInputHandler> handlers, InputId input, byte deviceIndex,
								  bool pressed)
		{
			// I'm using for loop in this way to make sure there are no temporary managed objects.
			for (int i = 0; i < handlers.Count; i++)
			{
				if (handlers[i](input, pressed, deviceIndex))
				{
					return true;
				}
			}
			return false;
		}
	}
	/// <summary>
	/// Provides access to general CryEngine Input API.
	/// </summary>
	public static class Inputs
	{
		#region Fields
		#endregion
		#region Properties
		#endregion
		#region Events
		#endregion
		#region Construction
		#endregion
		#region Interface
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool DeviceAvailable(InputDeviceType type);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetModifiers();
		/// <summary>
		/// Clears states of all keys.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ClearKeys();
		/// <summary>
		/// Clears states of analog inputs.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ClearAnalogInputs();
		#endregion
		#region Utilities
		#endregion
	}
}