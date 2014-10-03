using System;
using CryEngine.Annotations;
using CryEngine.Entities;
using CryEngine.Mathematics;
using CryEngine.Mathematics.Graphics;
using CryEngine.RunTime.Serialization;

namespace CryEngine.Logic.Actors
{
	public abstract partial class Actor
		: ActorBase
	{
		[UsedImplicitly]
		private void InternalFullSerialize(CrySerialize serialize)
		{
			//var serialize = new Serialization.CrySerialize();
			//serialize.Handle = handle;

			this.FullSerialize(serialize);
		}

		[UsedImplicitly]
		private void InternalNetSerialize(CrySerialize serialize, int aspect, byte profile, int flags)
		{
			// var serialize = new Serialization.CrySerialize();

			//serialize.Handle = handle;

			this.NetSerialize(serialize, aspect, profile, flags);
		}

		public PrePhysicsUpdateMode PrePhysicsUpdateMode
		{
			set { this.GameObject.PrePhysicsUpdateMode = value; }
		}

		public bool ReceivePostUpdates
		{
			set { this.GameObject.QueryExtension(this.ClassName).ReceivePostUpdates = value; }
		}
		#region Callbacks
		#region Entity events
		/// <summary>
		/// Called after level has been loaded, is not called on serialization. Note that this is
		/// called prior to GameRules.OnClientConnect and OnClientEnteredGame!
		/// </summary>
		protected virtual void OnLevelLoaded()
		{
		}

		/// <summary>
		/// Called when resetting the state of the entity in Editor.
		/// </summary>
		/// <param name="enteringGame">true if currently entering gamemode, false if exiting.</param>
		protected virtual void OnEditorReset(bool enteringGame)
		{
		}

		/// <summary>
		/// Sent on entity collision.
		/// </summary>
		protected virtual void OnCollision(ColliderInfo source, ColliderInfo target, Vector3 hitPos, Vector3 contactNormal,
										   float penetration, float radius)
		{
		}

		/// <summary>
		/// Called when game is started (games may start multiple times)
		/// </summary>
		protected virtual void OnStartGame()
		{
		}

		/// <summary>
		/// Called when the level is started.
		/// </summary>
		protected virtual void OnStartLevel()
		{
		}

		/// <summary>
		/// Sent when entity enters to the area proximity.
		/// </summary>
		/// <param name="entityId"></param>
		protected virtual void OnEnterArea(EntityId entityId, int areaId, EntityId areaEntityId)
		{
		}

		/// <summary>
		/// Sent when entity moves inside the area proximity.
		/// </summary>
		/// <param name="entityId"></param>
		protected virtual void OnMoveInsideArea(EntityId entityId, int areaId, EntityId areaEntityId)
		{
		}

		/// <summary>
		/// Sent when entity leaves the area proximity.
		/// </summary>
		/// <param name="entityId"></param>
		protected virtual void OnLeaveArea(EntityId entityId, int areaId, EntityId areaEntityId)
		{
		}

		protected virtual void OnEnterNearArea(EntityId entityId, int areaId, EntityId areaEntityId)
		{
		}
		protected virtual void OnLeaveNearArea(EntityId entityId, int areaId, EntityId areaEntityId)
		{
		}
		protected virtual void OnMoveNearArea(EntityId entityId, int areaId, EntityId areaEntityId, float fade)
		{
		}

		/// <summary>
		/// Called when the entities local or world transformation matrix changes. (Position /
		/// Rotation / Scale)
		/// </summary>
		protected virtual void OnMove(EntityMoveFlags moveFlags)
		{
		}

		/// <summary>
		/// Called whenever another entity has been linked to this entity.
		/// </summary>
		/// <param name="child"></param>
		protected virtual void OnAttach(EntityId child)
		{
		}
		/// <summary>
		/// Called whenever another entity has been unlinked from this entity.
		/// </summary>
		/// <param name="child"></param>
		protected virtual void OnDetach(EntityId child)
		{
		}
		/// <summary>
		/// Called whenever this entity is unliked from another entity.
		/// </summary>
		/// <param name="parent"></param>
		protected virtual void OnDetachThis(EntityId parent)
		{
		}

		protected virtual void OnPrePhysicsUpdate()
		{
		}

		/// <summary>
		/// Called when an animation event (placed on animations via the Character Editor) is encountered.
		/// </summary>
		/// <param name="animEvent"></param>
		[CLSCompliant(false)]
		protected virtual void OnAnimationEvent(AnimationEvent animEvent)
		{
		}
		#endregion
		/// <summary>
		/// Called to update the view associated to this actor.
		/// </summary>
		/// <param name="viewParams"></param>
		protected virtual void UpdateView(ref ViewParams viewParams)
		{
		}

		/// <summary>
		/// Called after updating the view associated to this actor.
		/// </summary>
		/// <param name="viewParams"></param>
		protected virtual void PostUpdateView(ref ViewParams viewParams)
		{
		}

		protected virtual void OnPostUpdate(float deltaTime)
		{
		}

		[CLSCompliant(false)]
		protected virtual void FullSerialize(ICrySerialize serialize)
		{
		}

		[CLSCompliant(false)]
		protected virtual void NetSerialize(ICrySerialize serialize, int aspect, byte profile, int flags)
		{
		}

		protected virtual void PostSerialize()
		{
		}
		#endregion
	}
}