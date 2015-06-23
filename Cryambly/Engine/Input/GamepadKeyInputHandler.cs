namespace CryCil.Engine.Input
{
	/// <summary>
	/// Defines signature of methods that can handle key input events that come from gamepads.
	/// </summary>
	/// <param name="key">        Identifier of the key.</param>
	/// <param name="pressed">    Indicates whether the key has been pressed.</param>
	/// <param name="deviceIndex">Zero-based index of the device that originated this input event.</param>
	/// <returns>
	/// True, if this method must be the last method to handle the event. If <c>true</c> is returned
	/// propagation of the event will stop.
	/// </returns>
	public delegate bool GamepadKeyInputHandler(InputId key, bool pressed, byte deviceIndex);
}