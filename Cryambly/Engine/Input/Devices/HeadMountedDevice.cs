using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Provides access to CryEngine Input API that deals with head-mounted devices.
	/// </summary>
	public static class HeadMountedDevice
	{
		#region Fields
		private static readonly List<EventHandler<AnalogInputEventArgs>> pitchHandlers;
		private static readonly List<EventHandler<AnalogInputEventArgs>> yawHandlers;
		private static readonly List<EventHandler<AnalogInputEventArgs>> rollHandlers;
		#endregion
		#region Properties
		/// <summary>
		/// Determines whether an HMD is connected to the machine.
		/// </summary>
		public static bool Available
		{
			get { return Inputs.DeviceAvailable(InputDeviceType.Headmounted); }
		}
		#endregion
		#region Events
		/// <summary>
		/// Occurs when the HMD changes its pitch orientation.
		/// </summary>
		public static event EventHandler<AnalogInputEventArgs> Pitch
		{
			add { pitchHandlers.Add(value); }
			remove { pitchHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the HMD changes its yaw orientation.
		/// </summary>
		public static event EventHandler<AnalogInputEventArgs> Yaw
		{
			add { yawHandlers.Add(value); }
			remove { yawHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the HMD changes its roll orientation.
		/// </summary>
		public static event EventHandler<AnalogInputEventArgs> Roll
		{
			add { rollHandlers.Add(value); }
			remove { rollHandlers.Remove(value); }
		}
		#endregion
		#region Construction
		static HeadMountedDevice()
		{
			pitchHandlers = new List<EventHandler<AnalogInputEventArgs>>();
			yawHandlers = new List<EventHandler<AnalogInputEventArgs>>();
			rollHandlers = new List<EventHandler<AnalogInputEventArgs>>();
		}
		#endregion
		#region Interface

		#endregion
		#region Utilities
		[UnmanagedThunk("Invoked by underlying framework to raise Pitch event.")]
		private static void OnPitch(int modifiers, float value, out bool blocked)
		{
			var args = new AnalogInputEventArgs(InputId.HeadMountedDevicePitch, (ModifierMask)modifiers, InputState.Changed, value);
			blocked = InputEventPropagator.Post(pitchHandlers, args);
		}
		[UnmanagedThunk("Invoked by underlying framework to raise Yaw event.")]
		private static void OnYaw(int modifiers, float value, out bool blocked)
		{
			var args = new AnalogInputEventArgs(InputId.HeadMountedDeviceYaw, (ModifierMask)modifiers, InputState.Changed, value);
			blocked = InputEventPropagator.Post(yawHandlers, args);
		}
		[UnmanagedThunk("Invoked by underlying framework to raise Roll event.")]
		private static void OnRoll(int modifiers, float value, out bool blocked)
		{
			var args = new AnalogInputEventArgs(InputId.HeadMountedDeviceRoll, (ModifierMask)modifiers, InputState.Changed, value);
			blocked = InputEventPropagator.Post(rollHandlers, args);
		}
		#endregion
	}
}
