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
		public bool Shift
		{
			get { return (this.ModifiersState & ModifierMask.Shift) != 0; }
		}
		/// <summary>
		/// Indicates whether left Shift key is pressed at the moment of the event.
		/// </summary>
		public bool LeftShift
		{
			get { return (this.ModifiersState & ModifierMask.LeftShift) != 0; }
		}
		/// <summary>
		/// Indicates whether right Shift key is pressed at the moment of the event.
		/// </summary>
		public bool RightShift
		{
			get { return (this.ModifiersState & ModifierMask.RightShift) != 0; }
		}
		/// <summary>
		/// Indicates whether any of the Control keys are pressed at the moment of the event.
		/// </summary>
		public bool Control
		{
			get { return (this.ModifiersState & ModifierMask.Control) != 0; }
		}
		/// <summary>
		/// Indicates whether left Control key is pressed at the moment of the event.
		/// </summary>
		public bool LeftControl
		{
			get { return (this.ModifiersState & ModifierMask.LeftControl) != 0; }
		}
		/// <summary>
		/// Indicates whether right Control key is pressed at the moment of the event.
		/// </summary>
		public bool RightControl
		{
			get { return (this.ModifiersState & ModifierMask.RightControl) != 0; }
		}
		/// <summary>
		/// Indicates whether any of the Alt keys are pressed at the moment of the event.
		/// </summary>
		public bool Alt
		{
			get { return (this.ModifiersState & ModifierMask.Alt) != 0; }
		}
		/// <summary>
		/// Indicates whether left Alt key is pressed at the moment of the event.
		/// </summary>
		public bool LeftAlt
		{
			get { return (this.ModifiersState & ModifierMask.LeftAlt) != 0; }
		}
		/// <summary>
		/// Indicates whether right Alt key is pressed at the moment of the event.
		/// </summary>
		public bool RightAlt
		{
			get { return (this.ModifiersState & ModifierMask.RightAlt) != 0; }
		}
		/// <summary>
		/// Indicates whether any of the Windows keys are pressed at the moment of the event.
		/// </summary>
		public bool Windows
		{
			get { return (this.ModifiersState & ModifierMask.Windows) != 0; }
		}
		/// <summary>
		/// Indicates whether left Windows key is pressed at the moment of the event.
		/// </summary>
		public bool LeftWindows
		{
			get { return (this.ModifiersState & ModifierMask.LeftWindows) != 0; }
		}
		/// <summary>
		/// Indicates whether right Windows key is pressed at the moment of the event.
		/// </summary>
		public bool RightWindows
		{
			get { return (this.ModifiersState & ModifierMask.RightWindows) != 0; }
		}
		/// <summary>
		/// Indicates whether any of the Lock keys are pressed at the moment of the event.
		/// </summary>
		public bool LockKeys
		{
			get { return (this.ModifiersState & ModifierMask.LockKeys) != 0; }
		}
		/// <summary>
		/// Indicates whether Num Lock key is pressed at the moment of the event.
		/// </summary>
		public bool NumLock
		{
			get { return (this.ModifiersState & ModifierMask.NumLock) != 0; }
		}
		/// <summary>
		/// Indicates whether Scroll Lock key is pressed at the moment of the event.
		/// </summary>
		public bool ScrollLock
		{
			get { return (this.ModifiersState & ModifierMask.ScrollLock) != 0; }
		}
		/// <summary>
		/// Indicates whether Caps Lock key is pressed at the moment of the event.
		/// </summary>
		public bool CapsLock
		{
			get { return (this.ModifiersState & ModifierMask.CapsLock) != 0; }
		}
		/// <summary>
		/// Indicates whether any of the modifier keys (Shift, Control, Alt or Windows) are pressed at the
		/// moment of the event.
		/// </summary>
		public bool Modifiers
		{
			get { return (this.ModifiersState & ModifierMask.Modifiers) != 0; }
		}
		/// <summary>
		/// Gets the value that indicates which modifier keys are pressed at the moment.
		/// </summary>
		public ModifierMask ModifiersState { get; private set; }
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