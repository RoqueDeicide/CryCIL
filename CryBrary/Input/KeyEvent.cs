using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public enum KeyEvent
	{
		Invalid = 0,

		/// <summary>
		/// Used when the action key is pressed
		/// </summary>
		OnPress,

		/// <summary>
		/// Used when the action key is released
		/// </summary>
		OnRelease,

		/// <summary>
		/// Used when the action key is held
		/// </summary>
		OnHold,

		Always,

		Retriggerable,

		NoModifiers,

		ConsoleCmd,

		/// <summary>
		/// Used when analog compare op succeeds
		/// </summary>
		AnalogCmd
	}
}