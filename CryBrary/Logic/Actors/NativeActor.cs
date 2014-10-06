using CryEngine.Entities;
using CryEngine.Logic.Entities;
using CryEngine.Native;

namespace CryEngine.Logic.Actors
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
			this.EntityHandle = actorInfo.EntityPtr;
			this.ActorHandle = actorInfo.ActorPtr;
		}

		internal NativeActor(EntityId id)
		{
			this.Id = id;
		}
	}
}