using System;
using CryCil.Engine.Physics;
using CryCil.Engine.Rendering;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Defines signature of methods that can handle <see cref="MonoEntity"/> events that don't provide any
	/// special data.
	/// </summary>
	/// <param name="sender">The entity that raised the event.</param>
	public delegate void EntitySimpleEventHandler(MonoEntity sender);
	/// <summary>
	/// Defines signature of methods that can handle <see cref="MonoEntity.Moved"/> event.
	/// </summary>
	/// <param name="sender">The entity that has been moved.</param>
	/// <param name="why">   A set of flags that describe why the event was raised.</param>
	public delegate void EntityMovementEventHandler(MonoEntity sender, EntityXFormChange why);
	/// <summary>
	/// Defines signature of methods that can handle <see cref="MonoEntity.TimedOut"/> event.
	/// </summary>
	/// <param name="sender">  The entity that raised the event.</param>
	/// <param name="timerId"> Identifier of the timer that reached the end of its time.</param>
	/// <param name="timeSpan">How long the timer has been active for.</param>
	public delegate void EntityTimerEventHandler(MonoEntity sender, int timerId, TimeSpan timeSpan);
	/// <summary>
	/// Defines signature of methods that can handle <see cref="MonoEntity.ResetInEditor"/> event.
	/// </summary>
	/// <param name="sender">          The entity that raised the event.</param>
	/// <param name="enteringGameMode">
	/// Indicates whether this event was caused by the user entering game mode.
	/// </param>
	public delegate void EntityResetEventHandler(MonoEntity sender, bool enteringGameMode);
	/// <summary>
	/// Defines signature of methods that can handle <see cref="MonoEntity"/> events related to
	/// (de/at)tachment of entities to/from each other.
	/// </summary>
	/// <param name="sender">The entity that raised the event.</param>
	/// <param name="other"> Identifier of another entity that is specific to this event.</param>
	public delegate void EntityAttachmentEventHandler(MonoEntity sender, EntityId other);
	/// <summary>
	/// Defines signature of methods that can handle <see cref="MonoEntity"/> events related to entities
	/// (becoming/stopping being) linked to the <paramref name="sender"/>.
	/// </summary>
	/// <param name="sender">   The entity that raised the event.</param>
	/// <param name="linkName"> Name of the object that describes the link.</param>
	/// <param name="other">    Identifier of another entity that (un-)linked to/from this one.</param>
	/// <param name="otherGuid">Identifier of another entity that (un-)linked to/from this one.</param>
	public delegate void EntityLinkageEventHandler(MonoEntity sender, string linkName, EntityId other, EntityGUID otherGuid
		);
	/// <summary>
	/// Defines signature of methods that can handle <see cref="MonoEntity.Hidden"/> and
	/// <see cref="MonoEntity.NotHidden"/> events.
	/// </summary>
	/// <param name="sender">The entity that raised the event.</param>
	/// <param name="hidden">Indicates whether entity has been hidden.</param>
	public delegate void EntityHidingEventHandler(MonoEntity sender, bool hidden);
	/// <summary>
	/// Defines signature of methods that can handle <see cref="MonoEntity.PhysicsEnabled"/> and
	/// <see cref="MonoEntity.PhysicsDisabled"/> events.
	/// </summary>
	/// <param name="sender">The entity that raised the event.</param>
	/// <param name="enable">Indicates whether physics processing was enabled for this entity.</param>
	public delegate void EntityPhysicsChangeEventHandler(MonoEntity sender, bool enable);
	/// <summary>
	/// Defines signature of methods that can handle <see cref="MonoEntity.Awoken"/> and
	/// <see cref="MonoEntity.PutToSleep"/> events.
	/// </summary>
	/// <param name="sender">The entity that raised the event.</param>
	/// <param name="awoken">
	/// Indicates entity has been awoken ( <c>true</c>) or put to sleep ( <c>false</c>).
	/// </param>
	public delegate void EntityPhysicsAwakeningEventHandler(MonoEntity sender, bool awoken);
	/// <summary>
	/// Defines signature of methods that can handle <see cref="MonoEntity"/> events that are raised when
	/// an entity enters/leaves the area that has <paramref name="sender"/> entity as one of its targets.
	/// </summary>
	/// <param name="sender">      Entity that is targeted by the area.</param>
	/// <param name="entity">      Identifier of the entity that has entered/left the area.</param>
	/// <param name="areaId">      Identifier of the area.</param>
	/// <param name="areaEntityId">Identifier of the entity that represents the area.</param>
	/// <param name="fadeFactor">  
	/// A multiplier that determines how close the <paramref name="entity"/> is to the "inside" region of
	/// the area. If the area doesn't have the "inside" region this parameter will be equal to 1.
	/// </param>
	public delegate void EntityAreaBorderEventHandler(MonoEntity sender, EntityId entity, int areaId,
													  EntityId areaEntityId, float fadeFactor);
	/// <summary>
	/// Defines signature of methods that can handle <see cref="MonoEntity.Collided"/> event.
	/// </summary>
	/// <param name="sender">   The entity that raised the event.</param>
	/// <param name="collision">A reference to the object that describes the collision.</param>
	/// <param name="isTarget"> 
	/// Indicates whether <paramref name="sender"/> is an entity that got collided with.
	/// </param>
	public delegate void EntityCollisionEventHandler(MonoEntity sender, ref CollisionEventInfo collision, bool isTarget);
	/// <summary>
	/// Defines signature of methods that can handle <see cref="MonoEntity.Rendered"/> event.
	/// </summary>
	/// <param name="sender">          The entity that raised the event.</param>
	/// <param name="renderParameters">
	/// A reference to the object that describes the parameters that are used to render this entity.
	/// Modifying this object changes the way all slots of this entity are rendered.
	/// </param>
	public delegate void EntityRenderEventHandler(MonoEntity sender, ref RenderParameters renderParameters);
	/// <summary>
	/// Defines signature of methods that can handle <see cref="MonoEntity.BeforePhysicsUpdate"/> event.
	/// </summary>
	/// <param name="sender">   The entity that raised the event.</param>
	/// <param name="frameTime">Length of the previous frame.</param>
	public delegate void EntityPrePhysicsEventHandler(MonoEntity sender, TimeSpan frameTime);
	/// <summary>
	/// Defines signature of methods that can handle <see cref="MonoEntity.MaterialChanged"/> event.
	/// </summary>
	/// <param name="sender">     The entity that raised the event.</param>
	/// <param name="newMaterial">A new material for the entity.</param>
	public delegate void EntityMaterialEventHandler(MonoEntity sender, Material newMaterial);
	/// <summary>
	/// Defines signature of methods that can handle <see cref="MonoEntity.MaterialLayersChanged"/> event.
	/// </summary>
	/// <param name="sender">The entity that raised the event.</param>
	/// <param name="new">   A new material layers mask.</param>
	/// <param name="old">   An old material layers mask.</param>
	public delegate void EntityMaterialLayersEventHandler(MonoEntity sender, byte @new, byte old);
	public partial class MonoEntity
	{
		/// <summary>
		/// Occurs when entity's X-Form transformation changes.
		/// </summary>
		public event EntityMovementEventHandler Moved;
		[UnmanagedThunk("Invoked from underlying object to raise event Moved.")]
		private void OnMoved(EntityXFormChange why)
		{
			var handler = this.Moved;
			if (handler != null) handler(this, why);
		}
		/// <summary>
		/// Occurs when entity's X-Form transformation changes by the editor user outside of game mode.
		/// </summary>
		/// <remarks>This event is sent when the user releases the button.</remarks>
		public event EntitySimpleEventHandler MovedInEditor;
		[UnmanagedThunk("Invoked from underlying object to raise event MovedInEditor.")]
		private void OnMovedInEditor()
		{
			var handler = this.MovedInEditor;
			if (handler != null) handler(this);
		}
		/// <summary>
		/// Occurs when one of the timers that was started by this entity has finished counting.
		/// </summary>
		public event EntityTimerEventHandler TimedOut;
		[UnmanagedThunk("Invoked from underlying object to raise event TimedOut.")]
		private void OnTimedOut(int timerId, int milliseconds)
		{
			var handler = this.TimedOut;
			if (handler != null) handler(this, timerId, TimeSpan.FromMilliseconds(milliseconds));
		}
		/// <summary>
		/// Occurs when entity that is not removable is reset.
		/// </summary>
		public event EntitySimpleEventHandler Ressurected;
		[UnmanagedThunk("Invoked from underlying object to raise event Ressurected.")]
		private void OnRessurected()
		{
			var handler = this.Ressurected;
			if (handler != null) handler(this);
		}
		/// <summary>
		/// Occurs before entity is removed.
		/// </summary>
		public event EntitySimpleEventHandler Done;
		[UnmanagedThunk("Invoked from underlying object to raise event Done.")]
		private void OnDone()
		{
			var handler = this.Done;
			if (handler != null) handler(this);
		}
		/// <summary>
		/// Occurs before entity is returned to the pool.
		/// </summary>
		public event EntitySimpleEventHandler ReturningToPool;
		[UnmanagedThunk("Invoked from underlying object to raise event ReturningToPool.")]
		private void OnReturningToPool()
		{
			var handler = this.ReturningToPool;
			if (handler != null) handler(this);
		}
		/// <summary>
		/// Occurs when entity gets reset when user switches to or from the game mode.
		/// </summary>
		public event EntityResetEventHandler ResetInEditor;
		[UnmanagedThunk("Invoked from underlying object to raise event ResetInEditor.")]
		private void OnResetInEditor(bool enterGameMode)
		{
			var handler = this.ResetInEditor;
			if (handler != null) handler(this, enterGameMode);
		}
		/// <summary>
		/// Occurs when another entity is attached to this one as a child.
		/// </summary>
		public event EntityAttachmentEventHandler Attached;
		[UnmanagedThunk("Invoked from underlying object to raise event Attached.")]
		private void OnAttached(EntityId id)
		{
			var handler = this.Attached;
			if (handler != null) handler(this, id);
		}
		/// <summary>
		/// Occurs this entity is attached to another.
		/// </summary>
		public event EntityAttachmentEventHandler AttachedTo;
		[UnmanagedThunk("Invoked from underlying object to raise event AttachedTo.")]
		private void OnAttachedTo(EntityId id)
		{
			var handler = this.AttachedTo;
			if (handler != null) handler(this, id);
		}
		/// <summary>
		/// Occurs when another entity is detached from this one.
		/// </summary>
		public event EntityAttachmentEventHandler Detached;
		[UnmanagedThunk("Invoked from underlying object to raise event Detached.")]
		private void OnDetached(EntityId id)
		{
			var handler = this.Detached;
			if (handler != null) handler(this, id);
		}
		/// <summary>
		/// Occurs this entity is detached from another.
		/// </summary>
		public event EntityAttachmentEventHandler DetachedFrom;
		[UnmanagedThunk("Invoked from underlying object to raise event DetachedFrom.")]
		private void OnDetachedFrom(EntityId id)
		{
			var handler = this.DetachedFrom;
			if (handler != null) handler(this, id);
		}
		/// <summary>
		/// Occurs when another entity becomes linked to this one.
		/// </summary>
		public event EntityLinkageEventHandler Linked;
		[UnmanagedThunk("Invoked from underlying object to raise event Linked.")]
		private void OnLinked(string linkName, EntityId id, EntityGUID guid)
		{
			var handler = this.Linked;
			if (handler != null) handler(this, linkName, id, guid);
		}
		/// <summary>
		/// Occurs when another entity is about to stop being linked to this one.
		/// </summary>
		public event EntityLinkageEventHandler Unlinked;
		[UnmanagedThunk("Invoked from underlying object to raise event Unlinked.")]
		private void OnUnlinked(string linkName, EntityId id, EntityGUID guid)
		{
			var handler = this.Unlinked;
			if (handler != null) handler(this, linkName, id, guid);
		}
		/// <summary>
		/// Occurs when this entity becomes hidden.
		/// </summary>
		public event EntityHidingEventHandler Hidden;
		[UnmanagedThunk("Invoked from underlying object to raise event Hidden.")]
		private void OnHidden()
		{
			var handler = this.Hidden;
			if (handler != null) handler(this, true);
		}
		/// <summary>
		/// Occurs when this entity is no longer hidden.
		/// </summary>
		public event EntityHidingEventHandler NotHidden;
		[UnmanagedThunk("Invoked from underlying object to raise event NotHidden.")]
		private void OnNotHidden()
		{
			var handler = this.NotHidden;
			if (handler != null) handler(this, false);
		}
		/// <summary>
		/// Occurs when physics processing for this entity is enabled.
		/// </summary>
		public event EntityPhysicsChangeEventHandler PhysicsEnabled;
		/// <summary>
		/// Occurs when physics processing for this entity is disabled.
		/// </summary>
		public event EntityPhysicsChangeEventHandler PhysicsDisabled;
		[UnmanagedThunk("Invoked from underlying object to raise event PhysicsEnabled or PhysicsDisabled.")]
		private void OnPhysicsEnabled(bool enable)
		{
			if (enable)
			{
				var handler = this.PhysicsEnabled;
				if (handler != null) handler(this, false);
			}
			else
			{
				var handler = this.PhysicsDisabled;
				if (handler != null) handler(this, false);
			}
		}
		/// <summary>
		/// Occurs when physics processing for this entity is enabled.
		/// </summary>
		public event EntityPhysicsAwakeningEventHandler Awoken;
		/// <summary>
		/// Occurs when physics processing for this entity is disabled.
		/// </summary>
		public event EntityPhysicsAwakeningEventHandler PutToSleep;
		[UnmanagedThunk("Invoked from underlying object to raise event Awoken or PutToSleep.")]
		private void OnAwoken(bool awoken)
		{
			if (awoken)
			{
				var handler = this.Awoken;
				if (handler != null) handler(this, false);
			}
			else
			{
				var handler = this.PutToSleep;
				if (handler != null) handler(this, false);
			}
		}
		/// <summary>
		/// Occurs when another entity enters the area that targets this entity.
		/// </summary>
		public event EntityAreaBorderEventHandler AreaEntered;
		[UnmanagedThunk("Invoked from underlying object to raise event AreaEntered.")]
		private void OnAreaEntered(EntityId entity, int areaId, EntityId areaEntityId, float fadeFactor)
		{
			var handler = this.AreaEntered;
			if (handler != null) handler(this, entity, areaId, areaEntityId, fadeFactor);
		}
		/// <summary>
		/// Occurs when another entity leaves the area that targets this entity.
		/// </summary>
		public event EntityAreaBorderEventHandler AreaLeft;
		[UnmanagedThunk("Invoked from underlying object to raise event AreaLeft.")]
		private void OnAreaLeft(EntityId entity, int areaId, EntityId areaEntityId, float fadeFactor)
		{
			var handler = this.AreaLeft;
			if (handler != null) handler(this, entity, areaId, areaEntityId, fadeFactor);
		}
		/// <summary>
		/// Occurs when a breakable entity is broken.
		/// </summary>
		public event EntitySimpleEventHandler Broken;
		[UnmanagedThunk("Invoked from underlying object to raise event Broken.")]
		private void OnBroken()
		{
			var handler = this.Broken;
			if (handler != null) handler(this);
		}
		/// <summary>
		/// Occurs when entity has <see cref="EntityFlags.SendNotSeenTimeout"/> flag set for it and has not
		/// been rendered on screen for a while (time is specified via "es_not_seen_timeout" console
		/// variable which is equal to 30 by default).
		/// </summary>
		public event EntitySimpleEventHandler NotSeen;
		[UnmanagedThunk("Invoked from underlying object to raise event NotSeen.")]
		private void OnNotSeen()
		{
			var handler = this.NotSeen;
			if (handler != null) handler(this);
		}
		/// <summary>
		/// Occurs when entity collides with another.
		/// </summary>
		public event EntityCollisionEventHandler Collided;
		[UnmanagedThunk("Invoked from underlying object to raise event Collided.")]
		private void OnCollided(ref CollisionEventInfo info, bool isTarget)
		{
			var handler = this.Collided;
			if (handler != null) handler(this, ref info, isTarget);
		}
		/// <summary>
		/// Occurs before entity is rendered.
		/// </summary>
		public event EntityRenderEventHandler Rendered;
		[UnmanagedThunk("Invoked from underlying object to raise event Rendered.")]
		private void OnRendered(ref RenderParameters parameters)
		{
			var handler = this.Rendered;
			if (handler != null) handler(this, ref parameters);
		}
		/// <summary>
		/// Occurs before entity is rendered.
		/// </summary>
		public event EntityPrePhysicsEventHandler BeforePhysicsUpdate;
		/// <summary>
		/// Can be overridden in derived class to allow the entity to request changes to the entity's
		/// positioning in physical world, before physics engine updates it.
		/// </summary>
		/// <param name="frameTime">Length of the last frame.</param>
		public virtual void PrePhysicsUpdate(TimeSpan frameTime)
		{
		}
		[UnmanagedThunk("Invoked from underlying object to raise event BeforePhysicsUpdate.")]
		private void OnBeforePhysicsUpdate(float frameTimeSeconds)
		{
			TimeSpan frameTime = TimeSpan.FromSeconds(frameTimeSeconds);
			this.PrePhysicsUpdate(frameTime);
			var handler = this.BeforePhysicsUpdate;
			if (handler != null) handler(this, frameTime);
		}
		/// <summary>
		/// Occurs when a level finishes loading.
		/// </summary>
		public event EntitySimpleEventHandler LevelLoaded;
		[UnmanagedThunk("Invoked from underlying object to raise event LevelLoaded.")]
		private void OnLevelLoaded()
		{
			var handler = this.LevelLoaded;
			if (handler != null) handler(this);
		}
		/// <summary>
		/// Occurs when a game starts on a level for the first time.
		/// </summary>
		public event EntitySimpleEventHandler LevelStarted;
		[UnmanagedThunk("Invoked from underlying object to raise event LevelStarted.")]
		private void OnLevelStarted()
		{
			var handler = this.LevelStarted;
			if (handler != null) handler(this);
		}
		/// <summary>
		/// Occurs when a game starts on a level. Unlike <see cref="LevelStarted"/> this event can be
		/// raised multiple times within the level (in case of multiplayer matches).
		/// </summary>
		public event EntitySimpleEventHandler GameStarted;
		[UnmanagedThunk("Invoked from underlying object to raise event GameStarted.")]
		private void OnGameStarted()
		{
			var handler = this.GameStarted;
			if (handler != null) handler(this);
		}
		/// <summary>
		/// Occurs before this entity is synchronized with it representation in the file (when saving and
		/// loading games).
		/// </summary>
		public event EntitySimpleEventHandler Synchronizing;
		[UnmanagedThunk("Invoked from underlying object to raise event Synchronizing.")]
		private void OnSynchronizing()
		{
			var handler = this.Synchronizing;
			if (handler != null) handler(this);
		}
		/// <summary>
		/// Occurs after this entity is synchronized with it representation in the file (when saving and
		/// loading games).
		/// </summary>
		public event EntitySimpleEventHandler Synchronized;
		[UnmanagedThunk("Invoked from underlying object to raise event Synchronized.")]
		private void OnSynchronized()
		{
			var handler = this.Synchronized;
			if (handler != null) handler(this);
		}
		/// <summary>
		/// Occurs when this entity becomes visible.
		/// </summary>
		public event EntitySimpleEventHandler BecameVisible;
		[UnmanagedThunk("Invoked from underlying object to raise event BecameVisible.")]
		private void OnBecameVisible()
		{
			var handler = this.BecameVisible;
			if (handler != null) handler(this);
		}
		/// <summary>
		/// Occurs when this entity becomes invisible.
		/// </summary>
		public event EntitySimpleEventHandler BecameInvisible;
		[UnmanagedThunk("Invoked from underlying object to raise event BecameInvisible.")]
		private void OnBecameInvisible()
		{
			var handler = this.BecameInvisible;
			if (handler != null) handler(this);
		}
		/// <summary>
		/// Occurs when this entity changes its material.
		/// </summary>
		public event EntityMaterialEventHandler MaterialChanged;
		[UnmanagedThunk("Invoked from underlying object to raise event MaterialChanged.")]
		private void OnMaterialChanged(Material newMaterial)
		{
			var handler = this.MaterialChanged;
			if (handler != null) handler(this, newMaterial);
		}
		/// <summary>
		/// Occurs when a set of active material layers changes for this entity.
		/// </summary>
		public event EntityMaterialLayersEventHandler MaterialLayersChanged;
		[UnmanagedThunk("Invoked from underlying object to raise event MaterialLayersChanged.")]
		private void OnMaterialLayersChanged(byte @new, byte old)
		{
			var handler = this.MaterialLayersChanged;
			if (handler != null) handler(this, @new, old);
		}
		/// <summary>
		/// Occurs when this entity becomes active.
		/// </summary>
		/// <remarks>Active entities are updated each frame.</remarks>
		public event EntitySimpleEventHandler Activated;
		[UnmanagedThunk("Invoked from underlying object to raise event Activated.")]
		private void OnActivated()
		{
			var handler = this.Activated;
			if (handler != null) handler(this);
		}
		/// <summary>
		/// Occurs when this entity stops being active.
		/// </summary>
		/// <remarks>Active entities are updated each frame.</remarks>
		public event EntitySimpleEventHandler Deactivated;
		[UnmanagedThunk("Invoked from underlying object to raise event Deactivated.")]
		private void OnDeactivated()
		{
			var handler = this.Deactivated;
			if (handler != null) handler(this);
		}
	}
}