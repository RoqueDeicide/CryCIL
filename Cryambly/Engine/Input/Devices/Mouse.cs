using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Provides access to CryEngine Input API that works with mice.
	/// </summary>
	public static class Mouse
	{
		#region Fields
		private static readonly List<EventHandler<SimpleKeyEventArgs>> buttonHandlers;
		private static readonly List<EventHandler<AnalogInputEventArgs>> wheelUpHandlers;
		private static readonly List<EventHandler<AnalogInputEventArgs>> wheelDownHandlers;
		private static readonly List<EventHandler<AnalogInputEventArgs>> xHandlers;
		private static readonly List<EventHandler<AnalogInputEventArgs>> yHandlers;
		private static readonly List<EventHandler<AnalogInputEventArgs>> zHandlers;
		#endregion
		#region Properties
		/// <summary>
		/// Determines whether a mouse is connected to the machine.
		/// </summary>
		public static bool Available
		{
			get { return Inputs.DeviceAvailable(InputDeviceType.Mouse); }
		}
		#endregion
		#region Events
		/// <summary>
		/// Occurs when one of the mouse buttons is pressed or released.
		/// </summary>
		public static event EventHandler<SimpleKeyEventArgs> Button
		{
			add { buttonHandlers.Add(value); }
			remove { buttonHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the mouse wheel starts/stops upwards rotation.
		/// </summary>
		public static event EventHandler<AnalogInputEventArgs> WheelUp
		{
			add { wheelUpHandlers.Add(value); }
			remove { wheelUpHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the mouse wheel starts/stops downwards rotation.
		/// </summary>
		public static event EventHandler<AnalogInputEventArgs> WheelDown
		{
			add { wheelDownHandlers.Add(value); }
			remove { wheelDownHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the mouse moves along X-axis.
		/// </summary>
		public static event EventHandler<AnalogInputEventArgs> X
		{
			add { xHandlers.Add(value); }
			remove { xHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the mouse moves along Y-axis.
		/// </summary>
		public static event EventHandler<AnalogInputEventArgs> Y
		{
			add { yHandlers.Add(value); }
			remove { yHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the mouse moves along Z-axis.
		/// </summary>
		public static event EventHandler<AnalogInputEventArgs> Z
		{
			add { zHandlers.Add(value); }
			remove { zHandlers.Remove(value); }
		}
		#endregion
		#region Construction
		static Mouse()
		{
			buttonHandlers = new List<EventHandler<SimpleKeyEventArgs>>();
			wheelUpHandlers = new List<EventHandler<AnalogInputEventArgs>>();
			wheelDownHandlers = new List<EventHandler<AnalogInputEventArgs>>();
			xHandlers = new List<EventHandler<AnalogInputEventArgs>>();
			yHandlers = new List<EventHandler<AnalogInputEventArgs>>();
			zHandlers = new List<EventHandler<AnalogInputEventArgs>>();
		}
		#endregion
		#region Interface

		#endregion
		#region Utilities
		[PublicAPI("Invoked by underlying framework to raise Button event.")]
		private static void OnButton(uint input, int modifiers, bool pressed, out bool blocked)
		{
			var args = new SimpleKeyEventArgs((InputId)input, (ModifierMask)modifiers, pressed);
			blocked = InputEventPropagator.Post(buttonHandlers, args);
		}
		[PublicAPI("Invoked by underlying framework to raise WheelUp event.")]
		private static void OnWheelUp(int modifiers, int state, float value, out bool blocked)
		{
			var args = new AnalogInputEventArgs(InputId.MouseWheelUp, (ModifierMask)modifiers, (InputState)state, value);
			blocked = InputEventPropagator.Post(wheelUpHandlers, args);
		}
		[PublicAPI("Invoked by underlying framework to raise WheelDown event.")]
		private static void OnWheelDown(int modifiers, int state, float value, out bool blocked)
		{
			var args = new AnalogInputEventArgs(InputId.MouseWheelDown, (ModifierMask)modifiers, (InputState)state, value);
			blocked = InputEventPropagator.Post(wheelDownHandlers, args);
		}
		[PublicAPI("Invoked by underlying framework to raise X event.")]
		private static void OnX(int modifiers, float value, out bool blocked)
		{
			var args = new AnalogInputEventArgs(InputId.MouseX, (ModifierMask)modifiers, InputState.Changed, value);
			blocked = InputEventPropagator.Post(xHandlers, args);
		}
		[PublicAPI("Invoked by underlying framework to raise Y event.")]
		private static void OnY(int modifiers, float value, out bool blocked)
		{
			var args = new AnalogInputEventArgs(InputId.MouseY, (ModifierMask)modifiers, InputState.Changed, value);
			blocked = InputEventPropagator.Post(yHandlers, args);
		}
		[PublicAPI("Invoked by underlying framework to raise Z event.")]
		private static void OnZ(int modifiers, float value, out bool blocked)
		{
			var args = new AnalogInputEventArgs(InputId.MouseZ, (ModifierMask)modifiers, InputState.Changed, value);
			blocked = InputEventPropagator.Post(zHandlers, args);
		}
		#endregion
	}
}