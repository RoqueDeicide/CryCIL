using System;
using System.Linq;

namespace CryCil.Engine.Input.ActionMapping
{
	/// <summary>
	/// Must be implemented by classes that represent objects that are capable of processing input actions.
	/// </summary>
	public interface IActionMapHandler
	{
		/// <summary>
		/// Gets or sets the value that indicates this object is listening to the action map.
		/// </summary>
		bool ListeningToActions { get; set; }
	}
}