using System.Runtime.InteropServices;

using CryEngine.Native;

namespace CryEngine
{
	/// <summary>
	/// Represents an actor with a custom IActor implementation outside of CryMono.dll.
	/// </summary>
	[ExcludeFromCompilation]
	public class NativeActor : ActorBase
	{
		public NativeActor() { }

		internal NativeActor(ActorInitializationParams actorInfo)
		{
			Id = new EntityId(actorInfo.Id);
			this.SetIEntity(actorInfo.EntityPtr);
			this.SetIActor(actorInfo.ActorPtr);
		}

		internal NativeActor(EntityId id)
		{
			Id = id;
		}
	}
}