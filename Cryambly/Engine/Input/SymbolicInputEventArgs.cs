using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Represents information about keyboard input that was translated into symbolic text input.
	/// </summary>
	public class SymbolicInputEventArgs : InputEventArgs
	{
		#region Properties
		/// <summary>
		/// Gets the identifier of the key.
		/// </summary>
		public char Character { get; private set; }
		#endregion
		#region Construction
		/// <summary>
		/// Creates new object that provides information about virtual key input.
		/// </summary>
		/// <param name="character">Identifier of the key.</param>
		/// <param name="modifiers">A set of modifier keys.</param>
		public SymbolicInputEventArgs(char character, ModifierMask modifiers)
			: base(InputId.Unknown, modifiers, InputState.Pressed)
		{
			this.Character = character;
		}
		#endregion
	}
}
