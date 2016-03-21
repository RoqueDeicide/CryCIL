using System;
using System.Linq;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Encapsulates information about modifier keys that were pressed at the moment of the input event.
	/// </summary>
	public struct ModifierKeysStatus
	{
		/// <summary>
		/// Indicates whether any of the Shift keys are pressed at the moment of the event.
		/// </summary>
		public bool Shift => (this.ModifiersState & ModifierMask.Shift) != 0;
		/// <summary>
		/// Indicates whether left Shift key is pressed at the moment of the event.
		/// </summary>
		public bool LeftShift => (this.ModifiersState & ModifierMask.LeftShift) != 0;
		/// <summary>
		/// Indicates whether right Shift key is pressed at the moment of the event.
		/// </summary>
		public bool RightShift => (this.ModifiersState & ModifierMask.RightShift) != 0;
		/// <summary>
		/// Indicates whether any of the Control keys are pressed at the moment of the event.
		/// </summary>
		public bool Control => (this.ModifiersState & ModifierMask.Control) != 0;
		/// <summary>
		/// Indicates whether left Control key is pressed at the moment of the event.
		/// </summary>
		public bool LeftControl => (this.ModifiersState & ModifierMask.LeftControl) != 0;
		/// <summary>
		/// Indicates whether right Control key is pressed at the moment of the event.
		/// </summary>
		public bool RightControl => (this.ModifiersState & ModifierMask.RightControl) != 0;
		/// <summary>
		/// Indicates whether any of the Alt keys are pressed at the moment of the event.
		/// </summary>
		public bool Alt => (this.ModifiersState & ModifierMask.Alt) != 0;
		/// <summary>
		/// Indicates whether left Alt key is pressed at the moment of the event.
		/// </summary>
		public bool LeftAlt => (this.ModifiersState & ModifierMask.LeftAlt) != 0;
		/// <summary>
		/// Indicates whether right Alt key is pressed at the moment of the event.
		/// </summary>
		public bool RightAlt => (this.ModifiersState & ModifierMask.RightAlt) != 0;
		/// <summary>
		/// Indicates whether any of the Windows keys are pressed at the moment of the event.
		/// </summary>
		public bool Windows => (this.ModifiersState & ModifierMask.Windows) != 0;
		/// <summary>
		/// Indicates whether left Windows key is pressed at the moment of the event.
		/// </summary>
		public bool LeftWindows => (this.ModifiersState & ModifierMask.LeftWindows) != 0;
		/// <summary>
		/// Indicates whether right Windows key is pressed at the moment of the event.
		/// </summary>
		public bool RightWindows => (this.ModifiersState & ModifierMask.RightWindows) != 0;
		/// <summary>
		/// Indicates whether any of the Lock keys are pressed at the moment of the event.
		/// </summary>
		public bool LockKeys => (this.ModifiersState & ModifierMask.LockKeys) != 0;
		/// <summary>
		/// Indicates whether Num Lock key is pressed at the moment of the event.
		/// </summary>
		public bool NumLock => (this.ModifiersState & ModifierMask.NumLock) != 0;
		/// <summary>
		/// Indicates whether Scroll Lock key is pressed at the moment of the event.
		/// </summary>
		public bool ScrollLock => (this.ModifiersState & ModifierMask.ScrollLock) != 0;
		/// <summary>
		/// Indicates whether Caps Lock key is pressed at the moment of the event.
		/// </summary>
		public bool CapsLock => (this.ModifiersState & ModifierMask.CapsLock) != 0;
		/// <summary>
		/// Indicates whether any of the modifier keys (Shift, Control, Alt or Windows) are pressed at the
		/// moment of the event.
		/// </summary>
		public bool Modifiers => (this.ModifiersState & ModifierMask.Modifiers) != 0;
		/// <summary>
		/// Gets the value that indicates which modifier keys are pressed at the moment.
		/// </summary>
		public ModifierMask ModifiersState { get; }
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="modifiers">
		/// A value that indicates which modifier keys are pressed at the moment.
		/// </param>
		public ModifierKeysStatus(ModifierMask modifiers)
			: this()
		{
			this.ModifiersState = modifiers;
		}
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="modifiers">
		/// A value that indicates which modifier keys are pressed at the moment.
		/// </param>
		public ModifierKeysStatus(int modifiers)
			: this()
		{
			this.ModifiersState = (ModifierMask)modifiers;
		}
	}
}