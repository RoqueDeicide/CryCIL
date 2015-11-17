using System;

namespace CryCil.Engine.Input.ActionMapping
{
	/// <summary>
	/// Defines signature of methods that can handle action map events.
	/// </summary>
	/// <param name="actionName">Name of the action.</param>
	/// <param name="mode">      A set of flags that describes how the action was activated.</param>
	/// <param name="value">     
	/// A certain value associated with the input (delta movement of the mouse, change to the position of
	/// the analog stick on the controller).
	/// </param>
	public delegate void InputActionHandler(string actionName, ActionActivationMode mode, float value);
}