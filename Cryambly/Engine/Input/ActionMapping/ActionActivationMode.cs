using System;

namespace CryCil.Engine.Input.ActionMapping
{
	/// <summary>
	/// Enumeration of flags that describe how the action was activated. Can also be used to describe the
	/// input that activate the action.
	/// </summary>
	[Flags]
	public enum ActionActivationMode
	{
		/// <summary>
		/// This value represents an invalid activation mode.
		/// </summary>
		Invalid = 0,
		/// <summary>
		/// When set, indicates that the action was activated by pressing a key or a mouse button.
		/// </summary>
		OnPress = 1 << 0,
		/// <summary>
		/// When set, indicates that the action was activated by releasing a key or a mouse button.
		/// </summary>
		OnRelease = 1 << 1,
		/// <summary>
		/// When set, indicates that the action was activated by holding a key or a mouse button for a
		/// specified amount of time.
		/// </summary>
		OnHold = 1 << 2,
		/// <summary>
		/// When set, indicates that the action was activated via code (?).
		/// </summary>
		Always = 1 << 3,

		// Special modifiers.
		/// <summary>
		/// When set, indicates that the action was retriggered. Retriggering means sending out press
		/// events for keys that are currently pressed.
		/// </summary>
		Retriggerable = 1 << 4,
		/// <summary>
		/// This flag is used when registering actions. When set, indicates that in order for the action to
		/// be activated, no modifier keys must be pressed when pressing activation key.
		/// </summary>
		NoModifiers = 1 << 5,
		/// <summary>
		/// This flag is used when registering actions. When set, indicates that the action must execute a
		/// console command of the same name.
		/// </summary>
		ConsoleCmd = 1 << 6,
		/// <summary>
		/// When set, indicates that the action was activated by successfully (success means: boolean
		/// operation has returned <c>true</c>) comparing a certain analog input with a set value.
		/// </summary>
		AnalogCompare = 1 << 7
	}
}