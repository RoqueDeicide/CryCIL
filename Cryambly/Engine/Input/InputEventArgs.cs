using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Base class for objects that provide additional information about input events.
	/// </summary>
	public abstract class InputEventArgs : EventArgs
	{
		#region Fields
		private readonly ModifierMask modifiers;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether any of the Shift keys are pressed at the moment of the event.
		/// </summary>
		public bool Shift
		{
			get { return (this.modifiers & ModifierMask.Shift) != 0; }
		}
		/// <summary>
		/// Indicates whether left Shift key is pressed at the moment of the event.
		/// </summary>
		public bool LeftShift
		{
			get { return (this.modifiers & ModifierMask.LeftShift) != 0; }
		}
		/// <summary>
		/// Indicates whether right Shift key is pressed at the moment of the event.
		/// </summary>
		public bool RightShift
		{
			get { return (this.modifiers & ModifierMask.RightShift) != 0; }
		}
		/// <summary>
		/// Indicates whether any of the Control keys are pressed at the moment of the event.
		/// </summary>
		public bool Control
		{
			get { return (this.modifiers & ModifierMask.Control) != 0; }
		}
		/// <summary>
		/// Indicates whether left Control key is pressed at the moment of the event.
		/// </summary>
		public bool LeftControl
		{
			get { return (this.modifiers & ModifierMask.LeftControl) != 0; }
		}
		/// <summary>
		/// Indicates whether right Control key is pressed at the moment of the event.
		/// </summary>
		public bool RightControl
		{
			get { return (this.modifiers & ModifierMask.RightControl) != 0; }
		}
		/// <summary>
		/// Indicates whether any of the Alt keys are pressed at the moment of the event.
		/// </summary>
		public bool Alt
		{
			get { return (this.modifiers & ModifierMask.Alt) != 0; }
		}
		/// <summary>
		/// Indicates whether left Alt key is pressed at the moment of the event.
		/// </summary>
		public bool LeftAlt
		{
			get { return (this.modifiers & ModifierMask.LeftAlt) != 0; }
		}
		/// <summary>
		/// Indicates whether right Alt key is pressed at the moment of the event.
		/// </summary>
		public bool RightAlt
		{
			get { return (this.modifiers & ModifierMask.RightAlt) != 0; }
		}
		/// <summary>
		/// Indicates whether any of the Windows keys are pressed at the moment of the event.
		/// </summary>
		public bool Windows
		{
			get { return (this.modifiers & ModifierMask.Windows) != 0; }
		}
		/// <summary>
		/// Indicates whether left Windows key is pressed at the moment of the event.
		/// </summary>
		public bool LeftWindows
		{
			get { return (this.modifiers & ModifierMask.LeftWindows) != 0; }
		}
		/// <summary>
		/// Indicates whether right Windows key is pressed at the moment of the event.
		/// </summary>
		public bool RightWindows
		{
			get { return (this.modifiers & ModifierMask.RightWindows) != 0; }
		}
		/// <summary>
		/// Indicates whether any of the Lock keys are pressed at the moment of the event.
		/// </summary>
		public bool LockKeys
		{
			get { return (this.modifiers & ModifierMask.LockKeys) != 0; }
		}
		/// <summary>
		/// Indicates whether Num Lock key is pressed at the moment of the event.
		/// </summary>
		public bool NumLock
		{
			get { return (this.modifiers & ModifierMask.NumLock) != 0; }
		}
		/// <summary>
		/// Indicates whether Scroll Lock key is pressed at the moment of the event.
		/// </summary>
		public bool ScrollLock
		{
			get { return (this.modifiers & ModifierMask.ScrollLock) != 0; }
		}
		/// <summary>
		/// Indicates whether Caps Lock key is pressed at the moment of the event.
		/// </summary>
		public bool CapsLock
		{
			get { return (this.modifiers & ModifierMask.CapsLock) != 0; }
		}
		/// <summary>
		/// Indicates whether any of the modifier keys (Shift, Control, Alt or Windows) are pressed at the
		/// moment of the event.
		/// </summary>
		public bool Modifiers
		{
			get { return (this.modifiers & ModifierMask.Modifiers) != 0; }
		}
		/// <summary>
		/// Indicates whether remaining handlers should receive this event.
		/// </summary>
		public bool BlockFurtherHandling { get; set; }
		/// <summary>
		/// Indicates whether the key was pressed.
		/// </summary>
		public InputState State { get; private set; }
		/// <summary>
		/// Gets the identifier of the input.
		/// </summary>
		public InputId InputIdentifier { get; private set; }
		#endregion
		#region Construction
		/// <summary>
		/// Initializes an object of this class.
		/// </summary>
		/// <param name="input">Identifier of the input.</param>
		/// <param name="keys"> Modifier keys that are active at the moment of the event.</param>
		/// <param name="state">Indicates what state the current input is in.</param>
		protected InputEventArgs(InputId input, ModifierMask keys, InputState state)
		{
			this.InputIdentifier = input;
			this.modifiers = keys;
			this.BlockFurtherHandling = false;
			this.State = state;
		}
		#endregion
	}
}