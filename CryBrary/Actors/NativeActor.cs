using CryEngine.Entities;
using CryEngine.Native;

namespace CryEngine.Actors
{
	/// <summary>
	/// Represents an actor with a custom IActor implementation outside of CryMono.dll.
	/// </summary>
	public class NativeActor : ActorBase
	{
		public NativeActor() { }

		internal NativeActor(ActorInitializationParams actorInfo)
		{
			this.Id = new EntityId(actorInfo.Id);
			this.SetIEntity(actorInfo.EntityPtr);
			this.SetIActor(actorInfo.ActorPtr);
		}

		internal NativeActor(EntityId id)
		{
			this.Id = id;
		}
	}
}