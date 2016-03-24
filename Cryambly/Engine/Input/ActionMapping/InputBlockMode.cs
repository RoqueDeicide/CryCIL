using System;
using System.Linq;

namespace CryCil.Engine.Input.ActionMapping
{
	/// <summary>
	/// Enumeration of input block modes.
	/// </summary>
	public enum InputBlockMode
	{
		/// <summary>
		/// Nothing is blocked or cleared.
		/// </summary>
		None,
		/// <summary>
		/// Specified inputs will be blocked.
		/// </summary>
		Block,
		/// <summary>
		/// The input will stop all inputs from being blocked.
		/// </summary>
		Clear
	}
}