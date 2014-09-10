using System;
using CryEngine.Entities;
using CryEngine.Native;

namespace CryEngine.Actors
{
	/// <summary>
	/// Base class which all actors must derive from. Includes basic callbacks.
	/// </summary>
	public abstract class ActorBase : EntityBase
	{
		/// <summary>
		/// Gets a value indicating whether this actor is controlled by the local client. See <see
		/// cref="Actor.LocalClient" />.
		/// </summary>
		public bool IsLocalClient
		{
			get { return Equals(Actor.LocalClient, this); }
		}
		/// <summary>
		/// Gets or sets the channel id, index to the net channel in use by this actor.
		/// </summary>
		public int ChannelId { get; set; }

		internal IntPtr ActorHandle { get; set; }
		#region Callbacks
		/// <summary>
		/// Called after successful actor creation via Actor.Create.
		/// </summary>
		public virtual void OnSpawn()
		{
		}
		#endregion
		#region Overrides
		/// <summary>
		/// Removes this actor from the world.
		/// </summary>
		/// <param name="forceRemoveNow"></param>
		public override void Remove(bool forceRemoveNow = false)
		{
			if (forceRemoveNow)
				throw new NotSupportedException("forceRemoveNow is not supported for actor types.");

			Actor.Remove(this.Id);
		}

		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = 17;

				hash = hash * 29 + this.ScriptId.GetHashCode();
				hash = hash * 29 + this.Id.GetHashCode();
				hash = hash * 29 + this.ChannelId.GetHashCode();
				hash = hash * 29 + this.ActorHandle.GetHashCode();
				hash = hash * 29 + this.EntityHandle.GetHashCode();

				return hash;
			}
		}
		#endregion
		internal override bool InternalInitialize(IScriptInitializationParams initParams)
		{
			var actorInitParams = (ActorInitializationParams)initParams;

			//System.Diagnostics.Contracts.Contract.Requires(actorInitParams.ChannelId > 0);
			if (actorInitParams.ChannelId <= 0)
			{
				throw new ArgumentException("Invalid channel identifier used for actor initialization.");
			}
			this.Id = new EntityId(actorInitParams.Id);
			this.SetIActor(actorInitParams.ActorPtr);
			this.SetIEntity(actorInitParams.EntityPtr);

			this.ChannelId = actorInitParams.ChannelId;

			// actor *has* to have physics.
			this.Physicalize(new PhysicalizationParams(PhysicalizationType.Rigid));

			var result = base.InternalInitialize(initParams);

			this.OnSpawn();

			return result;
		}
	}
}