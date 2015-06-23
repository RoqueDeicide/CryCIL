namespace CryCil.Engine.Input
{
	/// <summary>
	/// Defines signature of methods that handle analog input events.
	/// </summary>
	/// <param name="input">    Identifier of the input.</param>
	/// <param name="state">    State of the input at the moment of the event.</param>
	/// <param name="value">    Digitized value of the input.</param>
	/// <param name="modifiers">
	/// An object that specifies which modifier were pressed at the moment of the event.
	/// </param>
	/// <returns>
	/// True, if this method must be the last method to handle the event. If <c>true</c> is returned
	/// propagation of the event will stop.
	/// </returns>
	public delegate bool AnalogInputHandler(InputId input, InputState state, float value, ModifierKeysStatus modifiers);
}