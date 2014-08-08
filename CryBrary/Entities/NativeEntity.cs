using System;
using CryEngine.Native;

namespace CryEngine.Entities
{
	/// <summary>
	/// Represents an entity registered outside of CryMono, e.g. in CryGame.dll.
	/// </summary>
	[ExcludeFromCompilation]
	internal class NativeEntity : Entity
	{
		public NativeEntity()
		{
		}

		public NativeEntity(EntityId id, IntPtr ptr)
		{
			this.Id = id;
			this.SetIEntity(ptr);
		}
	}
}