using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	/// <summary>
	/// Attribute used for specifying extra functionality for custom game rules classes.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class GameRulesAttribute : Attribute
	{
		/// <summary>
		/// Sets the game mode's name. Uses the class name if not set.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// If set to true, the game mode will be set as default.
		/// </summary>
		public bool Default { get; set; }
	}
}