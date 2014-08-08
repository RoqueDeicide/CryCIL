using System;

namespace CryEngine.Entities
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