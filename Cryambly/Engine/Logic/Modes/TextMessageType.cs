using System;
using System.Linq;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Enumeration of types of text messages.
	/// </summary>
	public enum TextMessageType
	{
		/// <summary>
		/// This type of message must be displayed in the center of the screen(?)
		/// </summary>
		Center = 0,
		/// <summary>
		/// This is a console message.
		/// </summary>
		Console,
		/// <summary>
		/// Error message.
		/// </summary>
		Error,
		/// <summary>
		/// Informational message.
		/// </summary>
		Info,
		/// <summary>
		/// Message from the server (represents messages about the technical status of the server (about to
		/// go down, about to restart etc)).
		/// </summary>
		Server,
		/// <summary>
		/// Announcement message.
		/// </summary>
		Announcement
	}
}