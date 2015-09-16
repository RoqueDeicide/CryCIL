using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Provides access to CryEngine Input API that deals with Xbox gamepads.
	/// </summary>
	public static class XboxGamepad
	{
		#region Fields
		private static readonly List<GamepadKeyInputHandler> buttonHandlers;
		private static readonly List<GamepadAnalogInputHandler> leftTriggerHandlers;
		private static readonly List<GamepadAnalogInputHandler> rightTriggerHandlers;
		private static readonly List<GamepadAnalogInputHandler> leftThumbXHandlers;
		private static readonly List<GamepadAnalogInputHandler> leftThumbYHandlers;
		private static readonly List<GamepadAnalogInputHandler> rightThumbXHandlers;
		private static readonly List<GamepadAnalogInputHandler> rightThumbYHandlers;
		private static readonly List<GamepadKeyInputHandler> leftThumbUpHandlers;
		private static readonly List<GamepadKeyInputHandler> leftThumbDownHandlers;
		private static readonly List<GamepadKeyInputHandler> leftThumbLeftHandlers;
		private static readonly List<GamepadKeyInputHandler> leftThumbRightHandlers;
		private static readonly List<GamepadKeyInputHandler> rightThumbUpHandlers;
		private static readonly List<GamepadKeyInputHandler> rightThumbDownHandlers;
		private static readonly List<GamepadKeyInputHandler> rightThumbLeftHandlers;
		private static readonly List<GamepadKeyInputHandler> rightThumbRightHandlers;
		#endregion
		#region Properties
		#endregion
		#region Events
		/// <summary>
		/// Occurs when one of the mouse buttons is pressed or released.
		/// </summary>
		public static event GamepadKeyInputHandler Button
		{
			add { buttonHandlers.Add(value); }
			remove { buttonHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the left trigger is moved/pressed/released.
		/// </summary>
		/// <remarks>
		/// When the trigger simply moves, the movement event will be raised with state parameter equal to
		/// <see cref="InputState.Changed"/>. After posting that event a check is made whether the trigger
		/// crosses the threshold, if it goes above from below the threshold, then the event with
		/// <see cref="InputState.Pressed"/> is posted, otherwise, if it goes below from above the
		/// threshold, then the event with <see cref="InputState.Released"/> is posted.
		/// </remarks>
		public static event GamepadAnalogInputHandler LeftTrigger
		{
			add { leftTriggerHandlers.Add(value); }
			remove { leftTriggerHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the right trigger is moved/pressed/released.
		/// </summary>
		/// <remarks>
		/// When the trigger simply moves, the movement event will be raised with state parameter equal to
		/// <see cref="InputState.Changed"/>. After posting that event a check is made whether the trigger
		/// crosses the threshold, if it goes above from below the threshold, then the event with
		/// <see cref="InputState.Pressed"/> is posted, otherwise, if it goes below from above the
		/// threshold, then the event with <see cref="InputState.Released"/> is posted.
		/// </remarks>
		public static event GamepadAnalogInputHandler RightTrigger
		{
			add { rightTriggerHandlers.Add(value); }
			remove { rightTriggerHandlers.Remove(value); }
		}
		#region Thumb Stick Events
		/// <summary>
		/// Occurs when the left analog stick moves along X-axis.
		/// </summary>
		public static event GamepadAnalogInputHandler LeftThumbX
		{
			add { leftThumbXHandlers.Add(value); }
			remove { leftThumbXHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the left analog stick moves along Y-axis.
		/// </summary>
		public static event GamepadAnalogInputHandler LeftThumbY
		{
			add { leftThumbYHandlers.Add(value); }
			remove { leftThumbYHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the right analog stick moves along X-axis.
		/// </summary>
		public static event GamepadAnalogInputHandler RightThumbX
		{
			add { rightThumbXHandlers.Add(value); }
			remove { rightThumbXHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the right analog stick moves along Y-axis.
		/// </summary>
		public static event GamepadAnalogInputHandler RightThumbY
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
		public static event GamepadKeyInputHandler LeftThumbUp
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
		public static event GamepadKeyInputHandler LeftThumbDown
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
		public static event GamepadKeyInputHandler LeftThumbLeft
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
		public static event GamepadKeyInputHandler LeftThumbRight
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
		public static event GamepadKeyInputHandler RightThumbUp
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
		public static event GamepadKeyInputHandler RightThumbDown
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
		public static event GamepadKeyInputHandler RightThumbLeft
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
		public static event GamepadKeyInputHandler RightThumbRight
		{
			add { rightThumbRightHandlers.Add(value); }
			remove { rightThumbRightHandlers.Remove(value); }
		}
		#endregion
		#endregion
		#region Construction
		static XboxGamepad()
		{
			buttonHandlers = new List<GamepadKeyInputHandler>();
			leftTriggerHandlers = new List<GamepadAnalogInputHandler>();
			rightTriggerHandlers = new List<GamepadAnalogInputHandler>();
			leftThumbXHandlers = new List<GamepadAnalogInputHandler>();
			leftThumbYHandlers = new List<GamepadAnalogInputHandler>();
			rightThumbXHandlers = new List<GamepadAnalogInputHandler>();
			rightThumbYHandlers = new List<GamepadAnalogInputHandler>();
			leftThumbUpHandlers = new List<GamepadKeyInputHandler>();
			leftThumbDownHandlers = new List<GamepadKeyInputHandler>();
			leftThumbLeftHandlers = new List<GamepadKeyInputHandler>();
			leftThumbRightHandlers = new List<GamepadKeyInputHandler>();
			rightThumbUpHandlers = new List<GamepadKeyInputHandler>();
			rightThumbDownHandlers = new List<GamepadKeyInputHandler>();
			rightThumbLeftHandlers = new List<GamepadKeyInputHandler>();
			rightThumbRightHandlers = new List<GamepadKeyInputHandler>();
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
			blocked = InputEventPropagator.Post(buttonHandlers, (InputId)input, deviceIndex, pressed);
		}
		[UnmanagedThunk("Invoked by underlying framework to raise LeftTrigger event.")]
		private static void OnLeftTrigger(uint input, byte deviceIndex, int state, float value, out bool blocked)
		{
			blocked = InputEventPropagator.Post(leftTriggerHandlers, (InputId)input, deviceIndex, (InputState)state, value);
		}
		[UnmanagedThunk("Invoked by underlying framework to raise RightTrigger event.")]
		private static void OnRightTrigger(uint input, byte deviceIndex, int state, float value, out bool blocked)
		{
			blocked = InputEventPropagator.Post(rightTriggerHandlers, (InputId)input, deviceIndex, (InputState)state, value);
		}
		[UnmanagedThunk("Invoked by underlying framework to raise LeftThumbX event.")]
		private static void OnLeftThumbX(int state, byte deviceIndex, float value, out bool blocked)
		{
			blocked = InputEventPropagator.Post(leftThumbXHandlers, InputId.XboxThumbLeftX, deviceIndex, (InputState)state, value);
		}
		[UnmanagedThunk("Invoked by underlying framework to raise LeftThumbY event.")]
		private static void OnLeftThumbY(int state, byte deviceIndex, float value, out bool blocked)
		{
			blocked = InputEventPropagator.Post(leftThumbYHandlers, InputId.XboxThumbLeftY, deviceIndex, (InputState)state, value);
		}
		[UnmanagedThunk("Invoked by underlying framework to raise RightThumbX event.")]
		private static void OnRightThumbX(int state, byte deviceIndex, float value, out bool blocked)
		{
			blocked = InputEventPropagator.Post(rightThumbXHandlers, InputId.XboxThumbRightY, deviceIndex, (InputState)state,
												value);
		}
		[UnmanagedThunk("Invoked by underlying framework to raise RightThumbY event.")]
		private static void OnRightThumbY(int state, byte deviceIndex, float value, out bool blocked)
		{
			blocked = InputEventPropagator.Post(rightThumbYHandlers, InputId.XboxThumbRightY, deviceIndex, (InputState)state,
												value);
		}
		[UnmanagedThunk("Invoked by underlying framework to raise on the directional analog stick events.")]
		private static void OnThumbDirection(uint id, byte deviceIndex, bool pressed, out bool blocked)
		{
			List<GamepadKeyInputHandler> handlers;

			InputId input = (InputId)id;
			switch (input)
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

			blocked = InputEventPropagator.Post(handlers, input, deviceIndex, pressed);
		}
		#endregion
	}
}