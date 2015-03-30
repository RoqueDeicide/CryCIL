using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Represents information about a simple key input event that wasn't translated into symbolic text
	/// input.
	/// </summary>
	public class SimpleKeyEventArgs : InputEventArgs
	{
		#region Properties
		#endregion
		#region Construction
		/// <summary>
		/// Creates new object that provides information about simple key input.
		/// </summary>
		/// <param name="key">      Identifier of the key.</param>
		/// <param name="modifiers">A set of modifier keys.</param>
		/// <param name="pressed">  Indicates whether the key was pressed.</param>
		public SimpleKeyEventArgs(InputId key, ModifierMask modifiers, bool pressed)
			: base(key, modifiers, pressed ? InputState.Pressed : InputState.Released)
		{
		}
		#endregion
	}
}