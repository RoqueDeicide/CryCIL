using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	/// <summary>
	/// Thrown when an entity is attempted to be removed improperly.
	/// </summary>
	public class EntityRemovalException : Exception
	{
		public EntityRemovalException()
		{
		}

		public EntityRemovalException(string message)
			: base(message)
		{
		}

		public EntityRemovalException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}