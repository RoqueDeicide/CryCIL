using System;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Defines signature of methods that handle analog input events that come from gamepads.
	/// </summary>
	/// <param name="input">      Identifier of the input.</param>
	/// <param name="state">      State of the input at the moment of the event.</param>
	/// <param name="value">      Digitized value of the input.</param>
	/// <param name="deviceIndex">Zero-based index of the device that originated this input event.</param>
	/// <returns>
	/// True, if this method must be the last method to handle the event. If <c>true</c> is returned
	/// propagation of the event will stop.
	/// </returns>
	public delegate bool GamepadAnalogInputHandler(InputId input, InputState state, float value, byte deviceIndex);
}