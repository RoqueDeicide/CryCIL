using System;
using System.Runtime.InteropServices;

using CryEngine.Native;

namespace CryEngine
{
	/// <summary>
	/// Represents an entity registered outside of CryMono, e.g. in CryGame.dll.
	/// </summary>
	[ExcludeFromCompilation]
	internal class NativeEntity : Entity
	{
		public NativeEntity() { }

		public NativeEntity(EntityId id, IntPtr ptr)
		{
			Id = id;
			this.SetIEntity(ptr);
		}
	}
}