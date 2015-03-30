using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Provides access to CryEngine Input API that works with keyboards.
	/// </summary>
	public static class Keyboard
	{
		#region Fields
		private static readonly List<EventHandler<SimpleKeyEventArgs>> keyInputHandlers;
		private static readonly List<EventHandler<SymbolicInputEventArgs>> textInputHandlers;
		#endregion
		#region Properties
		/// <summary>
		/// Determines whether a keyboard is connected to the machine.
		/// </summary>
		public static bool Available
		{
			get { return Inputs.DeviceAvailable(InputDeviceType.Keyboard); }
		}
		/// <summary>
		/// Gets all modifier keys that are active at the moment.
		/// </summary>
		public static ModifierMask Modifiers
		{
			get { return (ModifierMask)Inputs.GetModifiers(); }
		}
		#endregion
		#region Events
		/// <summary>
		/// Occurs when one of the keys on the keyboard is pressed and system message about it is not
		/// translated.
		/// </summary>
		public static event EventHandler<SimpleKeyEventArgs> KeyChanged
		{
			add { keyInputHandlers.Add(value); }
			remove { keyInputHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when one of the keys on the keyboard is pressed and system message about it is
		/// translated into text input.
		/// </summary>
		public static event EventHandler<SymbolicInputEventArgs> CharacterInput
		{
			add { textInputHandlers.Add(value); }
			remove { textInputHandlers.Remove(value); }
		}
		#endregion
		#region Construction
		static Keyboard()
		{
			keyInputHandlers = new List<EventHandler<SimpleKeyEventArgs>>();
			textInputHandlers = new List<EventHandler<SymbolicInputEventArgs>>();
		}
		#endregion
		#region Interface
		#endregion
		#region Utilities
		[PublicAPI("Invoked by underlying framework to raise KeyChanged event.")]
		private static void OnKeyChanged(uint input, int modifiers, bool pressed, out bool blocked)
		{
			var args = new SimpleKeyEventArgs((InputId)input, (ModifierMask)modifiers, pressed);
			blocked = InputEventPropagator.Post(keyInputHandlers, args);
		}
		[PublicAPI("Invoked by underlying framework to raise CharacterInput event.")]
		private static void OnCharacterInput(char input, int modifiers, out bool blocked)
		{
			var args = new SymbolicInputEventArgs(input, (ModifierMask)modifiers);
			blocked = InputEventPropagator.Post(textInputHandlers, args);
		}
		#endregion
	}
}