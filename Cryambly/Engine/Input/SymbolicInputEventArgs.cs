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
		#region Fields
		private readonly uint character;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the identifier of the key.
		/// </summary>
		public string Character
		{
			get { return Encoding.UTF32.GetString(BitConverter.GetBytes(this.character)); }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new object that provides information about virtual key input.
		/// </summary>
		/// <param name="character">Character in UTF32 encoding.</param>
		public SymbolicInputEventArgs(uint character)
			: base(InputId.Unknown, ModifierMask.None, InputState.Pressed)
		{
			this.character = character;
		}
		#endregion
	}
}
