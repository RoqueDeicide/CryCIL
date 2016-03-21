using System;
using System.Collections.Generic;
using System.Linq;
using CryCil.RunTime;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Provides access to CryEngine Input API that works with mice.
	/// </summary>
	public static class Mouse
	{
		#region Fields
		private static readonly List<KeyInputHandler> buttonHandlers;
		private static readonly List<AnalogInputHandler> wheelUpHandlers;
		private static readonly List<AnalogInputHandler> wheelDownHandlers;
		private static readonly List<AnalogInputHandler> xHandlers;
		private static readonly List<AnalogInputHandler> yHandlers;
		private static readonly List<AnalogInputHandler> zHandlers;
		#endregion
		#region Properties
		/// <summary>
		/// Determines whether a mouse is connected to the machine.
		/// </summary>
		public static bool Available => Inputs.DeviceAvailable(InputDeviceType.Mouse);
		#endregion
		#region Events
		/// <summary>
		/// Occurs when one of the mouse buttons is pressed or released.
		/// </summary>
		public static event KeyInputHandler Button
		{
			add { buttonHandlers.Add(value); }
			remove { buttonHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the mouse wheel starts/stops upwards rotation.
		/// </summary>
		public static event AnalogInputHandler WheelUp
		{
			add { wheelUpHandlers.Add(value); }
			remove { wheelUpHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the mouse wheel starts/stops downwards rotation.
		/// </summary>
		public static event AnalogInputHandler WheelDown
		{
			add { wheelDownHandlers.Add(value); }
			remove { wheelDownHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the mouse moves along X-axis.
		/// </summary>
		public static event AnalogInputHandler X
		{
			add { xHandlers.Add(value); }
			remove { xHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the mouse moves along Y-axis.
		/// </summary>
		public static event AnalogInputHandler Y
		{
			add { yHandlers.Add(value); }
			remove { yHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the mouse moves along Z-axis.
		/// </summary>
		public static event AnalogInputHandler Z
		{
			add { zHandlers.Add(value); }
			remove { zHandlers.Remove(value); }
		}
		#endregion
		#region Construction
		static Mouse()
		{
			buttonHandlers = new List<KeyInputHandler>();
			wheelUpHandlers = new List<AnalogInputHandler>();
			wheelDownHandlers = new List<AnalogInputHandler>();
			xHandlers = new List<AnalogInputHandler>();
			yHandlers = new List<AnalogInputHandler>();
			zHandlers = new List<AnalogInputHandler>();
		}
		#endregion
		#region Interface
		#endregion
		#region Utilities
		[RawThunk("Invoked by underlying framework to raise Button event.")]
		private static void OnButton(uint input, int modifiers, bool pressed, out bool blocked)
		{
			try
			{
				blocked = InputEventPropagator.Post(buttonHandlers, (InputId)input,
													new ModifierKeysStatus(modifiers), pressed);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
				blocked = false;
			}
		}
		[RawThunk("Invoked by underlying framework to raise WheelUp event.")]
		private static void OnWheelUp(int modifiers, int state, float value, out bool blocked)
		{
			try
			{
				blocked = InputEventPropagator.Post(wheelUpHandlers, InputId.MouseWheelUp, new ModifierKeysStatus(modifiers),
													(InputState)state, value);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
				blocked = false;
			}
		}
		[RawThunk("Invoked by underlying framework to raise WheelDown event.")]
		private static void OnWheelDown(int modifiers, int state, float value, out bool blocked)
		{
			try
			{
				blocked = InputEventPropagator.Post(wheelDownHandlers, InputId.MouseWheelDown, new ModifierKeysStatus(modifiers),
													(InputState)state, value);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
				blocked = false;
			}
		}
		[RawThunk("Invoked by underlying framework to raise X event.")]
		private static void OnX(int modifiers, float value, out bool blocked)
		{
			try
			{
				blocked = InputEventPropagator.Post(xHandlers, InputId.MouseX, new ModifierKeysStatus(modifiers),
													InputState.Changed, value);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
				blocked = false;
			}
		}
		[RawThunk("Invoked by underlying framework to raise Y event.")]
		private static void OnY(int modifiers, float value, out bool blocked)
		{
			try
			{
				blocked = InputEventPropagator.Post(yHandlers, InputId.MouseY, new ModifierKeysStatus(modifiers),
													InputState.Changed, value);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
				blocked = false;
			}
		}
		[RawThunk("Invoked by underlying framework to raise Z event.")]
		private static void OnZ(int modifiers, float value, out bool blocked)
		{
			try
			{
				blocked = InputEventPropagator.Post(zHandlers, InputId.MouseZ, new ModifierKeysStatus(modifiers),
													InputState.Changed, value);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
				blocked = false;
			}
		}
		#endregion
	}
}