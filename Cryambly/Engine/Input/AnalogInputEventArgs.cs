using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Represents information about an "analog" input event.
	/// </summary>
	/// <remarks>
	/// "Analog" input is not technically analog because programs do not recognize anything that is not
	/// digital, it's just something that cannot be simply described as 'Up' or 'Down'.
	/// </remarks>
	public class AnalogInputEventArgs : InputEventArgs
	{
		#region Properties
		/// <summary>
		/// Gets digitized analog input value.
		/// </summary>
		public float InputValue { get; private set; }
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="input">    Identifier of the input.</param>
		/// <param name="modifiers">Modifier keys that are active at the moment of the event.</param>
		/// <param name="state">    Indicates what state the current input is in.</param>
		/// <param name="value">    Digitized analog input value.</param>
		public AnalogInputEventArgs(InputId input, ModifierMask modifiers, InputState state, float value)
			: base(input, modifiers, state)
		{
			this.InputValue = value;
		}
		#endregion
	}
}