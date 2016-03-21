using System;
using System.Linq;
using CryCil.Engine.Models.Characters;
using CryCil.Engine.Physics;
using CryCil.Engine.Rendering;
using CryCil.RunTime;

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
	/// Defines signature of methods that can handle <see cref="MonoEntity"/> events that are raised when an
	/// entity enters/leaves the area that has <paramref name="sender"/> entity as one of its targets.
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
	/// <summary>
	/// Defines signature of methods that can handle <see cref="MonoEntity.AnimationEvent"/> event.
	/// </summary>
	/// <param name="sender">   The entity that raised the event.</param>
	/// <param name="event">    Reference to the object that contains information about the event.</param>
	/// <param name="character">Object that represents the animated character that caused the event.</param>
	public delegate void EntityAnimationEventHandler(MonoEntity sender, ref AnimationEvent @event, Character character);
	public partial class MonoEntity
	{
		/// <summary>
		/// Occurs when entity's X-Form transformation changes.
		/// </summary>
		public event EntityMovementEventHandler Moved;
		[RawThunk("Invoked from underlying object to raise event Moved.")]
		private void OnMoved(EntityXFormChange why)
		{
			try
			{
				var handler = this.Moved;
				handler?.Invoke(this, why);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when entity's X-Form transformation changes by the editor user outside of game mode.
		/// </summary>
		/// <remarks>This event is sent when the user releases the button.</remarks>
		public event EntitySimpleEventHandler MovedInEditor;
		[RawThunk("Invoked from underlying object to raise event MovedInEditor.")]
		private void OnMovedInEditor()
		{
			try
			{
				var handler = this.MovedInEditor;
				handler?.Invoke(this);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when one of the timers that was started by this entity has finished counting.
		/// </summary>
		public event EntityTimerEventHandler TimedOut;
		[RawThunk("Invoked from underlying object to raise event TimedOut.")]
		private void OnTimedOut(int timerId, int milliseconds)
		{
			try
			{
				var handler = this.TimedOut;
				handler?.Invoke(this, timerId, TimeSpan.FromMilliseconds(milliseconds));
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when entity that is not removable is reset.
		/// </summary>
		public event EntitySimpleEventHandler Ressurected;
		[RawThunk("Invoked from underlying object to raise event Ressurected.")]
		private void OnRessurected()
		{
			try
			{
				var handler = this.Ressurected;
				handler?.Invoke(this);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs before entity is removed.
		/// </summary>
		public event EntitySimpleEventHandler Done;
		[RawThunk("Invoked from underlying object to raise event Done.")]
		private void OnDone()
		{
			try
			{
				var handler = this.Done;
				handler?.Invoke(this);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs before entity is returned to the pool.
		/// </summary>
		public event EntitySimpleEventHandler ReturningToPool;
		[RawThunk("Invoked from underlying object to raise event ReturningToPool.")]
		private void OnReturningToPool()
		{
			try
			{
				var handler = this.ReturningToPool;
				handler?.Invoke(this);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when entity gets reset when user switches to or from the game mode.
		/// </summary>
		public event EntityResetEventHandler ResetInEditor;
		[RawThunk("Invoked from underlying object to raise event ResetInEditor.")]
		private void OnResetInEditor(bool enterGameMode)
		{
			try
			{
				var handler = this.ResetInEditor;
				handler?.Invoke(this, enterGameMode);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when another entity is attached to this one as a child.
		/// </summary>
		public event EntityAttachmentEventHandler Attached;
		[RawThunk("Invoked from underlying object to raise event Attached.")]
		private void OnAttached(EntityId id)
		{
			try
			{
				var handler = this.Attached;
				handler?.Invoke(this, id);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs this entity is attached to another.
		/// </summary>
		public event EntityAttachmentEventHandler AttachedTo;
		[RawThunk("Invoked from underlying object to raise event AttachedTo.")]
		private void OnAttachedTo(EntityId id)
		{
			try
			{
				var handler = this.AttachedTo;
				handler?.Invoke(this, id);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when another entity is detached from this one.
		/// </summary>
		public event EntityAttachmentEventHandler Detached;
		[RawThunk("Invoked from underlying object to raise event Detached.")]
		private void OnDetached(EntityId id)
		{
			try
			{
				var handler = this.Detached;
				handler?.Invoke(this, id);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs this entity is detached from another.
		/// </summary>
		public event EntityAttachmentEventHandler DetachedFrom;
		[RawThunk("Invoked from underlying object to raise event DetachedFrom.")]
		private void OnDetachedFrom(EntityId id)
		{
			try
			{
				var handler = this.DetachedFrom;
				handler?.Invoke(this, id);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when another entity becomes linked to this one.
		/// </summary>
		public event EntityLinkageEventHandler Linked;
		[RawThunk("Invoked from underlying object to raise event Linked.")]
		private void OnLinked(string linkName, EntityId id, EntityGUID guid)
		{
			try
			{
				var handler = this.Linked;
				handler?.Invoke(this, linkName, id, guid);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when another entity is about to stop being linked to this one.
		/// </summary>
		public event EntityLinkageEventHandler Unlinked;
		[RawThunk("Invoked from underlying object to raise event Unlinked.")]
		private void OnUnlinked(string linkName, EntityId id, EntityGUID guid)
		{
			try
			{
				var handler = this.Unlinked;
				handler?.Invoke(this, linkName, id, guid);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when this entity becomes hidden.
		/// </summary>
		public event EntityHidingEventHandler Hidden;
		[RawThunk("Invoked from underlying object to raise event Hidden.")]
		private void OnHidden()
		{
			try
			{
				var handler = this.Hidden;
				handler?.Invoke(this, true);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when this entity is no longer hidden.
		/// </summary>
		public event EntityHidingEventHandler NotHidden;
		[RawThunk("Invoked from underlying object to raise event NotHidden.")]
		private void OnNotHidden()
		{
			try
			{
				var handler = this.NotHidden;
				handler?.Invoke(this, false);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when physics processing for this entity is enabled.
		/// </summary>
		public event EntityPhysicsChangeEventHandler PhysicsEnabled;
		/// <summary>
		/// Occurs when physics processing for this entity is disabled.
		/// </summary>
		public event EntityPhysicsChangeEventHandler PhysicsDisabled;
		[RawThunk("Invoked from underlying object to raise event PhysicsEnabled or PhysicsDisabled.")]
		private void OnPhysicsEnabled(bool enable)
		{
			try
			{
				if (enable)
				{
					var handler = this.PhysicsEnabled;
					handler?.Invoke(this, false);
				}
				else
				{
					var handler = this.PhysicsDisabled;
					handler?.Invoke(this, false);
				}
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
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
		[RawThunk("Invoked from underlying object to raise event Awoken or PutToSleep.")]
		private void OnAwoken(bool awoken)
		{
			try
			{
				if (awoken)
				{
					var handler = this.Awoken;
					handler?.Invoke(this, false);
				}
				else
				{
					var handler = this.PutToSleep;
					handler?.Invoke(this, false);
				}
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when another entity enters the area that targets this entity.
		/// </summary>
		public event EntityAreaBorderEventHandler AreaEntered;
		[RawThunk("Invoked from underlying object to raise event AreaEntered.")]
		private void OnAreaEntered(EntityId entity, int areaId, EntityId areaEntityId, float fadeFactor)
		{
			try
			{
				var handler = this.AreaEntered;
				handler?.Invoke(this, entity, areaId, areaEntityId, fadeFactor);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when another entity leaves the area that targets this entity.
		/// </summary>
		public event EntityAreaBorderEventHandler AreaLeft;
		[RawThunk("Invoked from underlying object to raise event AreaLeft.")]
		private void OnAreaLeft(EntityId entity, int areaId, EntityId areaEntityId, float fadeFactor)
		{
			try
			{
				var handler = this.AreaLeft;
				handler?.Invoke(this, entity, areaId, areaEntityId, fadeFactor);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when a breakable entity is broken.
		/// </summary>
		public event EntitySimpleEventHandler Broken;
		[RawThunk("Invoked from underlying object to raise event Broken.")]
		private void OnBroken()
		{
			try
			{
				var handler = this.Broken;
				handler?.Invoke(this);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when entity has <see cref="EntityFlags.SendNotSeenTimeout"/> flag set for it and has not
		/// been rendered on screen for a while (time is specified via "es_not_seen_timeout" console
		/// variable which is equal to 30 by default).
		/// </summary>
		public event EntitySimpleEventHandler NotSeen;
		[RawThunk("Invoked from underlying object to raise event NotSeen.")]
		private void OnNotSeen()
		{
			try
			{
				var handler = this.NotSeen;
				handler?.Invoke(this);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when entity collides with another.
		/// </summary>
		public event EntityCollisionEventHandler Collided;
		[RawThunk("Invoked from underlying object to raise event Collided.")]
		private void OnCollided(ref CollisionEventInfo info, bool isTarget)
		{
			try
			{
				var handler = this.Collided;
				handler?.Invoke(this, ref info, isTarget);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs before entity is rendered.
		/// </summary>
		public event EntityRenderEventHandler Rendered;
		[RawThunk("Invoked from underlying object to raise event Rendered.")]
		private void OnRendered(ref RenderParameters parameters)
		{
			try
			{
				var handler = this.Rendered;
				handler?.Invoke(this, ref parameters);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
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
		[RawThunk("Invoked from underlying object to raise event BeforePhysicsUpdate.")]
		private void OnBeforePhysicsUpdate(float frameTimeSeconds)
		{
			try
			{
				TimeSpan frameTime = TimeSpan.FromSeconds(frameTimeSeconds);
				this.PrePhysicsUpdate(frameTime);
				for (int i = 0; i < this.Extensions.Count; i++)
				{
					this.Extensions[i].PrePhysicsUpdate(frameTime);
				}
				var handler = this.BeforePhysicsUpdate;
				handler?.Invoke(this, frameTime);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when a level finishes loading.
		/// </summary>
		public event EntitySimpleEventHandler LevelLoaded;
		[RawThunk("Invoked from underlying object to raise event LevelLoaded.")]
		private void OnLevelLoaded()
		{
			try
			{
				var handler = this.LevelLoaded;
				handler?.Invoke(this);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when a game starts on a level for the first time.
		/// </summary>
		public event EntitySimpleEventHandler LevelStarted;
		[RawThunk("Invoked from underlying object to raise event LevelStarted.")]
		private void OnLevelStarted()
		{
			try
			{
				var handler = this.LevelStarted;
				handler?.Invoke(this);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when a game starts on a level. Unlike <see cref="LevelStarted"/> this event can be raised
		/// multiple times within the level (in case of multiplayer matches).
		/// </summary>
		public event EntitySimpleEventHandler GameStarted;
		[RawThunk("Invoked from underlying object to raise event GameStarted.")]
		private void OnGameStarted()
		{
			try
			{
				var handler = this.GameStarted;
				handler?.Invoke(this);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs before this entity is synchronized with it representation in the file (when saving and
		/// loading games).
		/// </summary>
		public event EntitySimpleEventHandler Synchronizing;
		[RawThunk("Invoked from underlying object to raise event Synchronizing.")]
		private void OnSynchronizing()
		{
			try
			{
				var handler = this.Synchronizing;
				handler?.Invoke(this);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs after this entity is synchronized with it representation in the file (when saving and
		/// loading games).
		/// </summary>
		public event EntitySimpleEventHandler Synchronized;
		[RawThunk("Invoked from underlying object to raise event Synchronized.")]
		private void OnSynchronized()
		{
			try
			{
				var handler = this.Synchronized;
				handler?.Invoke(this);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when this entity becomes visible.
		/// </summary>
		public event EntitySimpleEventHandler BecameVisible;
		[RawThunk("Invoked from underlying object to raise event BecameVisible.")]
		private void OnBecameVisible()
		{
			try
			{
				var handler = this.BecameVisible;
				handler?.Invoke(this);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when this entity becomes invisible.
		/// </summary>
		public event EntitySimpleEventHandler BecameInvisible;
		[RawThunk("Invoked from underlying object to raise event BecameInvisible.")]
		private void OnBecameInvisible()
		{
			try
			{
				var handler = this.BecameInvisible;
				handler?.Invoke(this);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when this entity changes its material.
		/// </summary>
		public event EntityMaterialEventHandler MaterialChanged;
		[RawThunk("Invoked from underlying object to raise event MaterialChanged.")]
		private void OnMaterialChanged(Material newMaterial)
		{
			try
			{
				var handler = this.MaterialChanged;
				handler?.Invoke(this, newMaterial);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when a set of active material layers changes for this entity.
		/// </summary>
		public event EntityMaterialLayersEventHandler MaterialLayersChanged;
		[RawThunk("Invoked from underlying object to raise event MaterialLayersChanged.")]
		private void OnMaterialLayersChanged(byte @new, byte old)
		{
			try
			{
				var handler = this.MaterialLayersChanged;
				handler?.Invoke(this, @new, old);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when this entity becomes active.
		/// </summary>
		/// <remarks>Active entities are updated each frame.</remarks>
		public event EntitySimpleEventHandler Activated;
		[RawThunk("Invoked from underlying object to raise event Activated.")]
		private void OnActivated()
		{
			try
			{
				var handler = this.Activated;
				handler?.Invoke(this);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when this entity stops being active.
		/// </summary>
		/// <remarks>Active entities are updated each frame.</remarks>
		public event EntitySimpleEventHandler Deactivated;
		[RawThunk("Invoked from underlying object to raise event Deactivated.")]
		private void OnDeactivated()
		{
			try
			{
				var handler = this.Deactivated;
				handler?.Invoke(this);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		/// <summary>
		/// Occurs when an event happens during animation of the character that is used by this entity.
		/// </summary>
		public event EntityAnimationEventHandler AnimationEvent;
		[RawThunk("Invoked from underlying object to raise event AnimationEvent.")]
		private void OnAnimationEvent(AnimationEvent @event, Character character)
		{
			try
			{
				var handler = this.AnimationEvent;
				handler?.Invoke(this, ref @event, character);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
	}
}