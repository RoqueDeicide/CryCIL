using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Provides index of the gamepad that caused the analog input event to be raised.
	/// </summary>
	public class GamepadAnalogInputEventArgs : AnalogInputEventArgs
	{
		#region Properties
		/// <summary>
		/// Gets the identifier of the device.
		/// </summary>
		public byte DeviceIndex { get; private set; }
		#endregion
		#region Construction
		/// <summary>
		/// Creates new object that provides information about virtual key input.
		/// </summary>
		/// <param name="input">      Identifier of the input.</param>
		/// <param name="deviceIndex">Identifier of the device.</param>
		/// <param name="state">      Indicates what state the current input is in.</param>
		/// <param name="value">      Digitized analog input value.</param>
		public GamepadAnalogInputEventArgs(InputId input, byte deviceIndex, InputState state, float value)
			: base(input, ModifierMask.None, state, value)
		{
			this.DeviceIndex = deviceIndex;
		}
		#endregion
	}
}