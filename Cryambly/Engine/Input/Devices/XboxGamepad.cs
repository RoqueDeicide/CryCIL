using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Provides access to CryEngine Input API that deals with Xbox gamepads.
	/// </summary>
	public static class XboxGamepad
	{
		#region Fields
		private static readonly List<EventHandler<GamepadKeyEventArgs>> buttonHandlers;
		private static readonly List<EventHandler<GamepadAnalogInputEventArgs>> leftTriggerHandlers;
		private static readonly List<EventHandler<GamepadAnalogInputEventArgs>> rightTriggerHandlers;
		private static readonly List<EventHandler<GamepadAnalogInputEventArgs>> leftThumbXHandlers;
		private static readonly List<EventHandler<GamepadAnalogInputEventArgs>> leftThumbYHandlers;
		private static readonly List<EventHandler<GamepadAnalogInputEventArgs>> rightThumbXHandlers;
		private static readonly List<EventHandler<GamepadAnalogInputEventArgs>> rightThumbYHandlers;
		private static readonly List<EventHandler<GamepadKeyEventArgs>> leftThumbUpHandlers;
		private static readonly List<EventHandler<GamepadKeyEventArgs>> leftThumbDownHandlers;
		private static readonly List<EventHandler<GamepadKeyEventArgs>> leftThumbLeftHandlers;
		private static readonly List<EventHandler<GamepadKeyEventArgs>> leftThumbRightHandlers;
		private static readonly List<EventHandler<GamepadKeyEventArgs>> rightThumbUpHandlers;
		private static readonly List<EventHandler<GamepadKeyEventArgs>> rightThumbDownHandlers;
		private static readonly List<EventHandler<GamepadKeyEventArgs>> rightThumbLeftHandlers;
		private static readonly List<EventHandler<GamepadKeyEventArgs>> rightThumbRightHandlers;
		#endregion
		#region Properties

		#endregion
		#region Events
		/// <summary>
		/// Occurs when one of the mouse buttons is pressed or released.
		/// </summary>
		public static event EventHandler<GamepadKeyEventArgs> Button
		{
			add { buttonHandlers.Add(value); }
			remove { buttonHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the left trigger is moved/pressed/released.
		/// </summary>
		/// <remarks>
		/// When the trigger simply moves, the movement event with <see cref="InputEventArgs.State"/> set
		/// to <see cref="InputState.Changed"/>. After posting that event a check is made whether the
		/// trigger crosses the threshold, if it goes above from below the threshold, then the event with
		/// <see cref="InputState.Pressed"/> is posted, otherwise, if it goes below from above the
		/// threshold, then the event with <see cref="InputState.Released"/> is posted.
		/// </remarks>
		public static event EventHandler<GamepadAnalogInputEventArgs> LeftTrigger
		{
			add { leftTriggerHandlers.Add(value); }
			remove { leftTriggerHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the right trigger is moved/pressed/released.
		/// </summary>
		/// <remarks>
		/// When the trigger simply moves, the movement event with <see cref="InputEventArgs.State"/> set
		/// to <see cref="InputState.Changed"/>. After posting that event a check is made whether the
		/// trigger crosses the threshold, if it goes above from below the threshold, then the event with
		/// <see cref="InputState.Pressed"/> is posted, otherwise, if it goes below from above the
		/// threshold, then the event with <see cref="InputState.Released"/> is posted.
		/// </remarks>
		public static event EventHandler<GamepadAnalogInputEventArgs> RightTrigger
		{
			add { rightTriggerHandlers.Add(value); }
			remove { rightTriggerHandlers.Remove(value); }
		}
		#region Thumb Stick Events
		/// <summary>
		/// Occurs when the left analog stick moves along X-axis.
		/// </summary>
		public static event EventHandler<GamepadAnalogInputEventArgs> LeftThumbX
		{
			add { leftThumbXHandlers.Add(value); }
			remove { leftThumbXHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the left analog stick moves along Y-axis.
		/// </summary>
		public static event EventHandler<GamepadAnalogInputEventArgs> LeftThumbY
		{
			add { leftThumbYHandlers.Add(value); }
			remove { leftThumbYHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the right analog stick moves along X-axis.
		/// </summary>
		public static event EventHandler<GamepadAnalogInputEventArgs> RightThumbX
		{
			add { rightThumbXHandlers.Add(value); }
			remove { rightThumbXHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the right analog stick moves along Y-axis.
		/// </summary>
		public static event EventHandler<GamepadAnalogInputEventArgs> RightThumbY
		{
			add { rightThumbYHandlers.Add(value); }
			remove { rightThumbYHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the left analog stick moves in or out of certain threshold in the Up direction.
		/// </summary>
		/// <remarks>
		/// When the stick passes the threshold <see cref="InputState.Pressed"/> type event is posted,
		/// otherwise if the stick moves below the threshold, then <see cref="InputState.Released"/> type
		/// event is posted.
		/// </remarks>
		public static event EventHandler<GamepadKeyEventArgs> LeftThumbUp
		{
			add { leftThumbUpHandlers.Add(value); }
			remove { leftThumbUpHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the left analog stick moves in or out of certain threshold in the Down direction.
		/// </summary>
		/// <remarks>
		/// When the stick passes the threshold <see cref="InputState.Pressed"/> type event is posted,
		/// otherwise if the stick moves below the threshold, then <see cref="InputState.Released"/> type
		/// event is posted.
		/// </remarks>
		public static event EventHandler<GamepadKeyEventArgs> LeftThumbDown
		{
			add { leftThumbDownHandlers.Add(value); }
			remove { leftThumbDownHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the left analog stick moves in or out of certain threshold in the Left direction.
		/// </summary>
		/// <remarks>
		/// When the stick passes the threshold <see cref="InputState.Pressed"/> type event is posted,
		/// otherwise if the stick moves below the threshold, then <see cref="InputState.Released"/> type
		/// event is posted.
		/// </remarks>
		public static event EventHandler<GamepadKeyEventArgs> LeftThumbLeft
		{
			add { leftThumbLeftHandlers.Add(value); }
			remove { leftThumbLeftHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the left analog stick moves in or out of certain threshold in the Right direction.
		/// </summary>
		/// <remarks>
		/// When the stick passes the threshold <see cref="InputState.Pressed"/> type event is posted,
		/// otherwise if the stick moves below the threshold, then <see cref="InputState.Released"/> type
		/// event is posted.
		/// </remarks>
		public static event EventHandler<GamepadKeyEventArgs> LeftThumbRight
		{
			add { leftThumbRightHandlers.Add(value); }
			remove { leftThumbRightHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the right analog stick moves in or out of certain threshold in the Up direction.
		/// </summary>
		/// <remarks>
		/// When the stick passes the threshold <see cref="InputState.Pressed"/> type event is posted,
		/// otherwise if the stick moves below the threshold, then <see cref="InputState.Released"/> type
		/// event is posted.
		/// </remarks>
		public static event EventHandler<GamepadKeyEventArgs> RightThumbUp
		{
			add { rightThumbUpHandlers.Add(value); }
			remove { rightThumbUpHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the right analog stick moves in or out of certain threshold in the Down direction.
		/// </summary>
		/// <remarks>
		/// When the stick passes the threshold <see cref="InputState.Pressed"/> type event is posted,
		/// otherwise if the stick moves below the threshold, then <see cref="InputState.Released"/> type
		/// event is posted.
		/// </remarks>
		public static event EventHandler<GamepadKeyEventArgs> RightThumbDown
		{
			add { rightThumbDownHandlers.Add(value); }
			remove { rightThumbDownHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the right analog stick moves in or out of certain threshold in the Left direction.
		/// </summary>
		/// <remarks>
		/// When the stick passes the threshold <see cref="InputState.Pressed"/> type event is posted,
		/// otherwise if the stick moves below the threshold, then <see cref="InputState.Released"/> type
		/// event is posted.
		/// </remarks>
		public static event EventHandler<GamepadKeyEventArgs> RightThumbLeft
		{
			add { rightThumbLeftHandlers.Add(value); }
			remove { rightThumbLeftHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the right analog stick moves in or out of certain threshold in the Right direction.
		/// </summary>
		/// <remarks>
		/// When the stick passes the threshold <see cref="InputState.Pressed"/> type event is posted,
		/// otherwise if the stick moves below the threshold, then <see cref="InputState.Released"/> type
		/// event is posted.
		/// </remarks>
		public static event EventHandler<GamepadKeyEventArgs> RightThumbRight
		{
			add { rightThumbRightHandlers.Add(value); }
			remove { rightThumbRightHandlers.Remove(value); }
		}
		#endregion
		#endregion
		#region Construction
		static XboxGamepad()
		{
			buttonHandlers = new List<EventHandler<GamepadKeyEventArgs>>();
			leftTriggerHandlers = new List<EventHandler<GamepadAnalogInputEventArgs>>();
			rightTriggerHandlers = new List<EventHandler<GamepadAnalogInputEventArgs>>();
			leftThumbXHandlers = new List<EventHandler<GamepadAnalogInputEventArgs>>();
			leftThumbYHandlers = new List<EventHandler<GamepadAnalogInputEventArgs>>();
			rightThumbXHandlers = new List<EventHandler<GamepadAnalogInputEventArgs>>();
			rightThumbYHandlers = new List<EventHandler<GamepadAnalogInputEventArgs>>();
			leftThumbUpHandlers = new List<EventHandler<GamepadKeyEventArgs>>();
			leftThumbDownHandlers = new List<EventHandler<GamepadKeyEventArgs>>();
			leftThumbLeftHandlers = new List<EventHandler<GamepadKeyEventArgs>>();
			leftThumbRightHandlers = new List<EventHandler<GamepadKeyEventArgs>>();
			rightThumbUpHandlers = new List<EventHandler<GamepadKeyEventArgs>>();
			rightThumbDownHandlers = new List<EventHandler<GamepadKeyEventArgs>>();
			rightThumbLeftHandlers = new List<EventHandler<GamepadKeyEventArgs>>();
			rightThumbRightHandlers = new List<EventHandler<GamepadKeyEventArgs>>();
		}
		#endregion
		#region Interface
		/// <summary>
		/// Triggers vibration in the controller.
		/// </summary>
		/// <param name="time">         
		/// Length of the vibration time span in seconds. Setting it to 0 will disable the vibration on the
		/// controller.
		/// </param>
		/// <param name="strengthLeft"> 
		/// A value in range [0, 1] that denotes a fraction of power of the left controller motor to use.
		/// Left motor produces low-frequency vibration.
		/// </param>
		/// <param name="strengthRight">
		/// A value in range [0, 1] that denotes a fraction of power of the right controller motor to use.
		/// Right motor produces high-frequency vibration.
		/// </param>
		/// <param name="deviceIndex">  
		/// A value between 0 and 3 that is a zero-based index of the controller that needs to rumble.
		/// </param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Rumble(float time, float strengthLeft, float strengthRight, byte deviceIndex = 0);
		/// <summary>
		/// Sets dead-zone threshold for analog sticks.
		/// </summary>
		/// <remarks>
		/// Dead-zone determines when the following events are raised:
		/// <para>1) <see cref="LeftThumbUp"/>.</para>
		/// <para>2) <see cref="LeftThumbDown"/>.</para>
		/// <para>3) <see cref="LeftThumbLeft"/>.</para>
		/// <para>4) <see cref="LeftThumbRight"/>.</para>
		/// <para>5) <see cref="RightThumbUp"/>.</para>
		/// <para>6) <see cref="RightThumbDown"/>.</para>
		/// <para>7) <see cref="RightThumbLeft"/>.</para>
		/// <para>8) <see cref="RightThumbRight"/>.</para>
		/// </remarks>
		/// <param name="threshold">
		/// A value between 0 and 1 that determines how far analog stick must be away from its origin for
		/// input events with input state set to either <see cref="InputState.Pressed"/> or
		/// <see cref="InputState.Released"/> to be raised.
		/// </param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetDeadzone(float threshold);
		/// <summary>
		/// Restores dead-zone to its default value of 0.2 .
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void RestoreDeadzone();
		/// <summary>
		/// Determines whether at least one gamepad is connected to the machine.
		/// </summary>
		/// <returns>
		/// True, if at least one gamepad is connected to the machine. Warning: it can be a PS3 or PS4
		/// controller.
		/// </returns>
		public static bool Available()
		{
			return Inputs.DeviceAvailable(InputDeviceType.Gamepad);
		}
		/// <summary>
		/// Determines whether a gamepad with specified index is connected to the machine.
		/// </summary>
		/// <param name="index">Index of the device to query.</param>
		/// <returns>
		/// True, if a gamepad with specified index is connected to the machine. Warning: it can be a PS3
		/// or PS4 controller.
		/// </returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool Connected(ushort index);
		#endregion
		#region Utilities
		[UnmanagedThunk("Invoked by underlying framework to raise Button event.")]
		private static void OnButton(uint input, byte deviceIndex, bool pressed, out bool blocked)
		{
			var args = new GamepadKeyEventArgs((InputId)input, deviceIndex, pressed);
			blocked = InputEventPropagator.Post(buttonHandlers, args);
		}
		[UnmanagedThunk("Invoked by underlying framework to raise LeftTrigger event.")]
		private static void OnLeftTrigger(uint input, byte deviceIndex, int state, float value, out bool blocked)
		{
			var args = new GamepadAnalogInputEventArgs((InputId)input, deviceIndex, (InputState)state, value);
			blocked = InputEventPropagator.Post(leftTriggerHandlers, args);
		}
		[UnmanagedThunk("Invoked by underlying framework to raise RightTrigger event.")]
		private static void OnRightTrigger(uint input, byte deviceIndex, int state, float value, out bool blocked)
		{
			var args = new GamepadAnalogInputEventArgs((InputId)input, deviceIndex, (InputState)state, value);
			blocked = InputEventPropagator.Post(rightTriggerHandlers, args);
		}
		[UnmanagedThunk("Invoked by underlying framework to raise LeftThumbX event.")]
		private static void OnLeftThumbX(int state, byte deviceIndex, float value, out bool blocked)
		{
			var args = new GamepadAnalogInputEventArgs(InputId.XboxThumbLeftX, deviceIndex, (InputState)state, value);
			blocked = InputEventPropagator.Post(leftThumbXHandlers, args);
		}
		[UnmanagedThunk("Invoked by underlying framework to raise LeftThumbY event.")]
		private static void OnLeftThumbY(int state, byte deviceIndex, float value, out bool blocked)
		{
			var args = new GamepadAnalogInputEventArgs(InputId.XboxThumbLeftY, deviceIndex, (InputState)state, value);
			blocked = InputEventPropagator.Post(leftThumbYHandlers, args);
		}
		[UnmanagedThunk("Invoked by underlying framework to raise RightThumbX event.")]
		private static void OnRightThumbX(int state, byte deviceIndex, float value, out bool blocked)
		{
			var args = new GamepadAnalogInputEventArgs(InputId.XboxThumbRightY, deviceIndex, (InputState)state, value);
			blocked = InputEventPropagator.Post(rightThumbXHandlers, args);
		}
		[UnmanagedThunk("Invoked by underlying framework to raise RightThumbY event.")]
		private static void OnRightThumbY(int state, byte deviceIndex, float value, out bool blocked)
		{
			var args = new GamepadAnalogInputEventArgs(InputId.XboxThumbRightY, deviceIndex, (InputState)state, value);
			blocked = InputEventPropagator.Post(rightThumbYHandlers, args);
		}
		[UnmanagedThunk("Invoked by underlying framework to raise on the directional analog stick events.")]
		private static void OnThumbDirection(uint id, byte deviceIndex, bool pressed, out bool blocked)
		{
			var args = new GamepadKeyEventArgs((InputId)id, deviceIndex, pressed);
			List<EventHandler<GamepadKeyEventArgs>> handlers;

			switch (args.InputIdentifier)
			{
				case InputId.XboxThumbLeftUp:
					handlers = leftThumbUpHandlers;
					break;
				case InputId.XboxThumbLeftDown:
					handlers = leftThumbDownHandlers;
					break;
				case InputId.XboxThumbLeftLeft:
					handlers = leftThumbLeftHandlers;
					break;
				case InputId.XboxThumbLeftRight:
					handlers = leftThumbRightHandlers;
					break;
				case InputId.XboxThumbRightUp:
					handlers = rightThumbUpHandlers;
					break;
				case InputId.XboxThumbRightDown:
					handlers = rightThumbDownHandlers;
					break;
				case InputId.XboxThumbRightLeft:
					handlers = rightThumbLeftHandlers;
					break;
				case InputId.XboxThumbRightRight:
					handlers = rightThumbRightHandlers;
					break;
				default:
					blocked = false;
					return;
			}

			blocked = InputEventPropagator.Post(handlers, args);
		}
		#endregion
	}
}