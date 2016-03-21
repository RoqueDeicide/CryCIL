using System;
using System.Collections.Generic;
using System.Linq;
using CryCil.RunTime;
using CryCil.Utilities;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Provides access to CryEngine Input API that works with keyboards.
	/// </summary>
	public static class Keyboard
	{
		#region Fields
		private static readonly List<KeyInputHandler> keyInputHandlers;
		private static readonly List<SymbolicInputHandler> textInputHandlers;
		#endregion
		#region Properties
		/// <summary>
		/// Determines whether a keyboard is connected to the machine.
		/// </summary>
		public static bool Available => Inputs.DeviceAvailable(InputDeviceType.Keyboard);
		/// <summary>
		/// Gets all modifier keys that are active at the moment.
		/// </summary>
		public static ModifierMask Modifiers => (ModifierMask)Inputs.GetModifiers();
		#endregion
		#region Events
		/// <summary>
		/// Occurs when one of the keys on the keyboard is pressed and system message about it is not
		/// translated.
		/// </summary>
		public static event KeyInputHandler KeyChanged
		{
			add { keyInputHandlers.Add(value); }
			remove { keyInputHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when one of the keys on the keyboard is pressed and system message about it is translated
		/// into text input.
		/// </summary>
		public static event SymbolicInputHandler CharacterInput
		{
			add { textInputHandlers.Add(value); }
			remove { textInputHandlers.Remove(value); }
		}
		#endregion
		#region Construction
		static Keyboard()
		{
			keyInputHandlers = new List<KeyInputHandler>();
			textInputHandlers = new List<SymbolicInputHandler>();
		}
		#endregion
		#region Interface
		#endregion
		#region Utilities
		[RawThunk("Invoked by underlying framework to raise KeyChanged event.")]
		private static void OnKeyChanged(uint input, int modifiers, bool pressed, out bool blocked)
		{
			try
			{
				blocked = InputEventPropagator.Post(keyInputHandlers, (InputId)input,
													new ModifierKeysStatus(modifiers), pressed);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
				blocked = false;
			}
		}
		[RawThunk("Invoked by underlying framework to raise CharacterInput event.")]
		private static void OnCharacterInput(uint input, out bool blocked)
		{
			try
			{
				blocked = InputEventPropagator.Post(textInputHandlers, new Utf32Char(input));
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
				blocked = false;
			}
		}
		#endregion
	}
}