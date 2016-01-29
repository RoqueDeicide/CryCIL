using System;
using System.Linq;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Enumeration of types of chat messages.
	/// </summary>
	public enum ChatMessageType
	{
		/// <summary>
		/// Chat message must be sent to the singular target.
		/// </summary>
		Target = 0,
		/// <summary>
		/// Chat message must be sent to sender's team-mates only.
		/// </summary>
		Team,
		/// <summary>
		/// Chat message must be sent to everyone.
		/// </summary>
		All
	}
}