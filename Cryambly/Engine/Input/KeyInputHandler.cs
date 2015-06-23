namespace CryCil.Engine.Input
{
	/// <summary>
	/// Defines signature of methods that can handle key input events.
	/// </summary>
	/// <param name="key">      Identifier of the key.</param>
	/// <param name="pressed">  Indicates whether the key has been pressed.</param>
	/// <param name="modifiers">
	/// An object that indicates which modifier keys were pressed at the moment of the event.
	/// </param>
	/// <returns>
	/// True, if this method must be the last method to handle the event. If <c>true</c> is returned
	/// propagation of the event will stop.
	/// </returns>
	public delegate bool KeyInputHandler(InputId key, bool pressed, ModifierKeysStatus modifiers);
}