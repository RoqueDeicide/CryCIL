using System;
using CryEngine.Logic.Entities;
using CryEngine.Native;

namespace CryEngine.Entities
{
	/// <summary>
	/// Represents an entity registered outside of CryMono, e.g. in CryGame.dll.
	/// </summary>
	internal class NativeEntity : Entity
	{
		/// <summary>
		/// Creates a default wrapper.
		/// </summary>
		public NativeEntity()
		{
		}
		/// <summary>
		/// Creates a wrapper for an entity.
		/// </summary>
		/// <param name="id">Identifier of the entity.</param>
		/// <param name="ptr">Entity's handle.</param>
		public NativeEntity(EntityId id, IntPtr ptr)
		{
			this.Id = id;
			this.EntityHandle = ptr;
		}
	}
}