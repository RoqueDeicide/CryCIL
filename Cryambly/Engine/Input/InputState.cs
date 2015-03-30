using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Enumeration of flags that describe state an input device key is in at the point of an event.
	/// </summary>
	[Flags]
	public enum InputState
	{
		/// <summary>
		/// Unknown state.
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// A key has just been pressed.
		/// </summary>
		Pressed = 1,
		/// <summary>
		/// A key has just been released.
		/// </summary>
		Released = 2,
		/// <summary>
		/// A key is being held.
		/// </summary>
		Down = 4,
		/// <summary>
		/// A state of the key has been changed.
		/// </summary>
		Changed = 8,
		/// <summary>
		/// A key has been intercepted by UI.
		/// </summary>
		UI = 16
	}
}
