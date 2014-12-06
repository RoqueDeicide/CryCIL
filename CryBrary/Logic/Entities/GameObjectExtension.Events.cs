using System;
using CryEngine.Engine.Renderer;
using CryEngine.Entities;
using CryEngine.Physics;

namespace CryEngine.Logic.Entities
{
	public partial class GameObjectExtension
	{
		#region Events
		/// <summary>
		/// Occurs when first stage of entity initialization is complete.
		/// </summary>
		/// <remarks>
		/// At this point, CryEngine side of the entity is not full initialized, which means that, in the handler, interaction with the CryEngine should be kept at minimum.
		/// </remarks>
		public event EventHandler Initializing;
		/// <summary>
		/// Occurs when entity initialization is complete.
		/// </summary>
		public event EventHandler Initialized;
		/// <summary>
		/// Occurs when entity needs to initialize client side specific properties.
		/// </summary>
		/// <remarks>
		/// At this point, CryEngine side of the entity is not full initialized, which means that, in the handler, interaction with the CryEngine should be kept at minimum.
		/// </remarks>
		public event EventHandler<EntityClientInitializationEventArgs> InitializingClient;
		/// <summary>
		/// Occurs when entity client side specific initialization is complete.
		/// </summary>
		public event EventHandler<EntityClientInitializationEventArgs> InitializedClient;
		/// <summary>
		/// Occurs when CryEngine entity initiates reloading sequence.
		/// </summary>
		public event EventHandler<EntityReloadEventArgs> Reloading;
		/// <summary>
		/// Occurs when CryEngine entity completes reloading sequence.
		/// </summary>
		public event EventHandler<EntitySpawnEventArgs> Reloaded;
		/// <summary>
		/// Occurs when this entity is "thinking" on how to update it's logical state.
		/// </summary>
		/// <remarks>
		/// This event occurs before <see cref="Think"/> method is invoked.
		/// </remarks>
		public event EventHandler<EntityThinkingEventArgs> Thinking;
		/// <summary>
		/// Occurs after this entity has "decided" on its logical state for this frame.
		/// </summary>
		/// <remarks>
		/// This event occurs after <see cref="Think"/> method is invoked.
		/// </remarks>
		public event EventHandler<EntityThinkingEventArgs> Decided;
		/// <summary>
		/// Occurs when this entity is "thinking" on how to update it's logical state after all other entities have made their first one.
		/// </summary>
		/// <remarks>
		/// This event occurs before <see cref="AfterThought"/> method is invoked.
		/// </remarks>
		public event EventHandler<FrameTimeEventArgs> SecondThought;
		/// <summary>
		/// Occurs after this entity has "decided" on its logical state for this frame after all other entities have made their first one.
		/// </summary>
		/// <remarks>
		/// This event occurs after <see cref="AfterThought"/> method is invoked.
		/// </remarks>
		public event EventHandler<FrameTimeEventArgs> SecondDecision;
		/// <summary>
		/// Occurs when this entity moves.
		/// </summary>
		public event EventHandler<EntityMovementEventArgs> Moved;
		/// <summary>
		/// Occurs when timer assigned to the entity reaches a specific point in time.
		/// </summary>
		public event EventHandler<EntityTimerEventArgs> TimedOut;
		/// <summary>
		/// Occurs when this entity appears in the view.
		/// </summary>
		public event EventHandler Appeared;
		/// <summary>
		/// Occurs when this entity disappears from the view.
		/// </summary>
		public event EventHandler Disappeared;
		/// <summary>
		/// Occurs when this entity appears/disappears from the view.
		/// </summary>
		public event EventHandler<EventArgs<bool>> VisibilityStateChanged;
		/// <summary>
		/// Occurs when editor resets this entity, with extra parameter indicating whether reset was done by entering game mode.
		/// </summary>
		public event EventHandler<EventArgs<bool>> ResetByEditor;
		/// <summary>
		/// Occurs when another entity was attached to this one. Parameter is identifier of another entity.
		/// </summary>
		public event EventHandler<EventArgs<EntityId>> Attached;
		/// <summary>
		/// Occurs when this entity was attached to another one. Parameter is identifier of another entity.
		/// </summary>
		public event EventHandler<EventArgs<EntityId>> AttachedTo;
		/// <summary>
		/// Occurs when another entity was detached from this one. Parameter is identifier of another entity.
		/// </summary>
		public event EventHandler<EventArgs<EntityId>> Detached;
		/// <summary>
		/// Occurs when this entity was detached from another one. Parameter is identifier of another entity.
		/// </summary>
		public event EventHandler<EventArgs<EntityId>> DetachedFrom;
		/// <summary>
		/// Occurs when another entity is linked to this one.
		/// </summary>
		public event EventHandler<EventArgs<EntityLink>> Linked;
		/// <summary>
		/// Occurs when another entity is unlinked from this one.
		/// </summary>
		public event EventHandler<EventArgs<EntityLink>> Unlinked;
		/// <summary>
		/// Occurs when this entity is hidden/revealed with parameter being true, if this entity hides.
		/// </summary>
		public event EventHandler<EventArgs<bool>> Hidden;
		/// <summary>
		/// Occurs when Lua entity attached to this one broadcasts the event.
		/// </summary>
		public event EventHandler<EntityScriptEventEventArgs> ScriptEventBroadcasted;
		/// <summary>
		/// Occurs after some entity enters the area that is linked to this one.
		/// </summary>
		public event EventHandler<EntityAreaPositionEventArgs> EnteredArea;
		/// <summary>
		/// Occurs after some entity leaves the area that is linked to this one.
		/// </summary>
		public event EventHandler<EntityAreaPositionEventArgs> LeftArea;
		/// <summary>
		/// Occurs after some entity enters the proximity of the area that is linked to this one.
		/// </summary>
		public event EventHandler<EntityAreaPositionEventArgs> EnteredProximityArea;
		/// <summary>
		/// Occurs after some entity leaves the proximity of the area that is linked to this one.
		/// </summary>
		public event EventHandler<EntityAreaPositionEventArgs> LeftProximityArea;
		/// <summary>
		/// Occurs after some entity moves inside the area that is linked to this one.
		/// </summary>
		public event EventHandler<EntityAreaPositionEventArgs> MovedInsideArea;
		/// <summary>
		/// Occurs after some entity moves near the area that is linked to this one.
		/// </summary>
		public event EventHandler<EntityAreaPositionEventArgs> MovedNearArea;
		/// <summary>
		/// Occurs after some entity crosses a group of areas this one is attached to.
		/// </summary>
		public event EventHandler<EventArgs<EntityId>> AreaCrossed;
		/// <summary>
		/// Occurs when this entity has been outside of vision for a while.
		/// </summary>
		public event EventHandler UnseenTimeOut;
		/// <summary>
		/// Occurs when this entity collides with another one.
		/// </summary>
		public event EventHandler<EventArgs<CryCollision>> Collided;
		/// <summary>
		/// Occurs when this entity receives notification about its rendering.
		/// </summary>
		/// <remarks>
		/// <see cref="RenderParams"/> reference supplied by this event contains parameters that were used to render the entity on the screen. It is modifiable (don't make use of this unless you REALLY know, what you are doing).
		/// </remarks>
		public event EventHandler<EventArgs<RenderParams?>> Rendered;
		/// <summary>
		/// Occurs prior to update of entity's state in physics engine.
		/// </summary>
		/// <remarks>
		/// Handling this event is perfect for submitting action requests to physics engine.
		/// </remarks>
		public event EventHandler<EventArgs<float>> BeforePhysicsUpdate;
		/// <summary>
		/// Occurs when level is loaded.
		/// </summary>
		public event EventHandler LevelLoaded;
		/// <summary>
		/// Occurs when game-play on the level commences.
		/// </summary>
		public event EventHandler LevelStarted;
		/// <summary>
		/// Occurs when the game starts.
		/// </summary>
		/// <remarks>
		/// May happen multiple times on the same level.
		/// </remarks>
		public event EventHandler GameStarted;
		/// <summary>
		/// Occurs when this entity is activated.
		/// </summary>
		public event EventHandler Activated;
		/// <summary>
		/// Occurs when this entity is deactivated.
		/// </summary>
		public event EventHandler Deactivated;
		/// <summary>
		/// Occurs when this entity is activated/deactivated.
		/// </summary>
		public event EventHandler<EventArgs<bool>> ActivationStateChanged;
		/// <summary>
		/// Occurs when material on entity changes.
		/// </summary>
		public event EventHandler<EventArgs<Material>> MaterialChanged;
		/// <summary>
		/// Occurs when this entity is spawned on this game instance from a remote machine.
		/// </summary>
		public event EventHandler SpawnedRemotely;
		#endregion
		#region Event Raisers
		/// <summary>
		/// Raises event <see cref="Initializing"/>.
		/// </summary>
		protected virtual void OnInitializing()
		{
			if (this.Initializing != null) this.Initializing(this, EventArgs.Empty);
		}
		/// <summary>
		/// Raises event <see cref="Initialized"/>.
		/// </summary>
		protected virtual void OnInitialized()
		{
			if (this.Initialized != null) this.Initialized(this, EventArgs.Empty);
		}
		/// <summary>
		/// Raises event <see cref="InitializingClient"/>.
		/// </summary>
		/// <param name="channelId">Client's channel identifier.</param>
		protected virtual void OnInitializingClient(int channelId)
		{
			if (this.InitializingClient != null)
				this.InitializingClient(this,
										new EntityClientInitializationEventArgs
										{
											ChannelId = channelId
										});
		}
		/// <summary>
		/// Raises event <see cref="InitializedClient"/>.
		/// </summary>
		/// <param name="channelId">Client's channel identifier.</param>
		protected virtual void OnInitializedClient(int channelId)
		{
			if (this.InitializedClient != null)
				this.InitializedClient(this,
									   new EntityClientInitializationEventArgs
									   {
										   ChannelId = channelId
									   });
		}
		/// <summary>
		/// Raises event <see cref="Reloading"/>.
		/// </summary>
		/// <param name="eventArgs">Extra data about the event.</param>
		protected virtual void OnReloading(EntityReloadEventArgs eventArgs)
		{
			if (this.Reloading != null)
				this.Reloading
				(
					this,
					eventArgs
				);
		}
		/// <summary>
		/// Raises event <see cref="Reloaded"/>.
		/// </summary>
		/// <param name="parameters">A set of paramters that describe entity after it was reloaded.</param>
		protected virtual void OnReloaded(EntitySpawnParameters parameters)
		{
			if (this.Reloaded != null)
				this.Reloaded
				(
					this,
					new EntitySpawnEventArgs
					{
						Parameters = parameters
					}
				);
		}
		/// <summary>
		/// Raises event <see cref="Thinking"/>.
		/// </summary>
		/// <param name="e">Data specific for this event.</param>
		protected virtual void OnThinking(EntityThinkingEventArgs e)
		{
			if (this.Thinking != null) this.Thinking(this, e);
		}
		/// <summary>
		/// Raises event <see cref="Decided"/>.
		/// </summary>
		/// <param name="e">Data specific for this event.</param>
		protected virtual void OnDecided(EntityThinkingEventArgs e)
		{
			if (this.Decided != null) this.Decided(this, e);
		}
		/// <summary>
		/// Raises event <see cref="SecondThought"/>.
		/// </summary>
		/// <param name="e">Data specific for this event.</param>
		protected virtual void OnSecondThought(FrameTimeEventArgs e)
		{
			if (this.SecondThought != null) this.SecondThought(this, e);
		}
		/// <summary>
		/// Raises event <see cref="SecondDecision"/>.
		/// </summary>
		/// <param name="e">Data specific for this event.</param>
		protected virtual void OnSecondDecision(FrameTimeEventArgs e)
		{
			if (this.SecondDecision != null) this.SecondDecision(this, e);
		}
		/// <summary>
		/// Raises event <see cref="Moved"/>.
		/// </summary>
		/// <param name="flags">Data specific for this event.</param>
		protected virtual void OnMoved(EntityXFormFlags flags)
		{
			if (this.Moved != null) this.Moved(this, new EntityMovementEventArgs(flags));
		}
		/// <summary>
		/// Raises event <see cref="TimedOut"/>.
		/// </summary>
		/// <param name="handle">Data specific for this event.</param>
		/// <param name="time">Data specific for this event.</param>
		protected virtual void OnTimedOut(IntPtr handle, int time)
		{
			if (this.TimedOut != null)
			{
				this.TimedOut(this, new EntityTimerEventArgs(new CryTimer(handle), time));
			}
		}
		/// <summary>
		/// Raises event <see cref="Appeared"/>.
		/// </summary>
		protected virtual void OnAppeared()
		{
			if (this.Appeared != null) this.Appeared(this, EventArgs.Empty);
		}
		/// <summary>
		/// Raises event <see cref="Disappeared"/>.
		/// </summary>
		protected virtual void OnDisappeared()
		{
			if (this.Disappeared != null) this.Disappeared(this, EventArgs.Empty);
		}
		/// <summary>
		/// Raises event <see cref="ResetByEditor"/>.
		/// </summary>
		/// <param name="enteringGame">Data specific for this event.</param>
		protected virtual void OnResetByEditor(bool enteringGame)
		{
			if (this.ResetByEditor != null)
				this.ResetByEditor(this, new EventArgs<bool>(enteringGame));
		}
		/// <summary>
		/// Raises event <see cref="Attached"/>
		/// </summary>
		/// <param name="id">Identifier of another entity.</param>
		protected virtual void OnAttached(EntityId id)
		{
			if (this.Attached != null) this.Attached(this, new EventArgs<EntityId>(id));
		}
		/// <summary>
		/// Raises event <see cref="AttachedTo"/>
		/// </summary>
		/// <param name="id">Identifier of another entity.</param>
		protected virtual void OnAttachedTo(EntityId id)
		{
			if (this.AttachedTo != null) this.AttachedTo(this, new EventArgs<EntityId>(id));
		}
		/// <summary>
		/// Raises event <see cref="Detached"/>
		/// </summary>
		/// <param name="id">Identifier of another entity.</param>
		protected virtual void OnDetached(EntityId id)
		{
			if (this.Detached != null) this.Detached(this, new EventArgs<EntityId>(id));
		}
		/// <summary>
		/// Raises event <see cref="DetachedFrom"/>
		/// </summary>
		/// <param name="id">Identifier of another entity.</param>
		protected virtual void OnDetachedFrom(EntityId id)
		{
			if (this.DetachedFrom != null) this.DetachedFrom(this, new EventArgs<EntityId>(id));
		}
		/// <summary>
		/// Raises event <see cref="Linked"/>.
		/// </summary>
		/// <param name="handle">Pointer to <see cref="EntityLink"/> object in native memory.</param>
		protected virtual unsafe void OnLinked(IntPtr handle)
		{
			if (this.Linked != null)
				this.Linked(this, new EventArgs<EntityLink>(*((EntityLink *)handle.ToPointer())));
		}
		/// <summary>
		/// Raises event <see cref="Unlinked"/>.
		/// </summary>
		/// <param name="handle">Pointer to <see cref="EntityLink"/> object in native memory.</param>
		protected virtual unsafe void OnUnlinked(IntPtr handle)
		{
			if (this.Unlinked != null)
				this.Unlinked(this, new EventArgs<EntityLink>(*((EntityLink*)handle.ToPointer())));
		}
		/// <summary>
		/// Raises event <see cref="Hidden"/>.
		/// </summary>
		/// <param name="hidden">True, if this entity hides.</param>
		protected virtual void OnHidden(bool hidden)
		{
			if (this.Hidden != null) this.Hidden(this, new EventArgs<bool>(hidden));
		}
		/// <summary>
		/// Raises event <see cref="ScriptEventBroadcasted"/>.
		/// </summary>
		/// <param name="e">Data that describes the event.</param>
		protected virtual void OnScriptEventBroadcasted(EntityScriptEventEventArgs e)
		{
			EventHandler<EntityScriptEventEventArgs> handler = this.ScriptEventBroadcasted;
			if (handler != null) handler(this, e);
		}
		/// <summary>
		/// Raises event <see cref="EnteredArea"/>.
		/// </summary>
		/// <param name="e">Data that describes the event.</param>
		protected virtual void OnEnteredArea(EntityAreaPositionEventArgs e)
		{
			if (this.EnteredArea != null) this.EnteredArea(this, e);
		}
		/// <summary>
		/// Raises event <see cref="LeftArea"/>.
		/// </summary>
		/// <param name="e">Data that describes the event.</param>
		protected virtual void OnLeftArea(EntityAreaPositionEventArgs e)
		{
			if (this.LeftArea != null) this.LeftArea(this, e);
		}
		/// <summary>
		/// Raises event <see cref="EnteredProximityArea"/>.
		/// </summary>
		/// <param name="e">Data that describes the event.</param>
		protected virtual void OnEnteredProximityArea(EntityAreaPositionEventArgs e)
		{
			if (this.EnteredProximityArea != null) this.EnteredProximityArea(this, e);
		}
		/// <summary>
		/// Raises event <see cref="LeftProximityArea"/>.
		/// </summary>
		/// <param name="e">Data that describes the event.</param>
		protected virtual void OnLeftProximityArea(EntityAreaPositionEventArgs e)
		{
			if (this.LeftProximityArea != null) this.LeftProximityArea(this, e);
		}
		/// <summary>
		/// Raises event <see cref="MovedInsideArea"/>.
		/// </summary>
		/// <param name="e">Data that describes the event.</param>
		protected virtual void OnMovedInsideArea(EntityAreaPositionEventArgs e)
		{
			if (this.MovedInsideArea != null) this.MovedInsideArea(this, e);
		}
		/// <summary>
		/// Raises event <see cref="MovedNearArea"/>.
		/// </summary>
		/// <param name="e">Data that describes the event.</param>
		protected virtual void OnMovedNearArea(EntityAreaPositionEventArgs e)
		{
			if (this.MovedNearArea != null) this.MovedNearArea(this, e);
		}
		/// <summary>
		/// Raises event <see cref="AreaCrossed"/>.
		/// </summary>
		/// <param name="id">Identifier of the trespasser.</param>
		protected virtual void OnAreaCrossed(EntityId id)
		{
			if (this.AreaCrossed != null) this.AreaCrossed(this, new EventArgs<EntityId>(id));
		}
		/// <summary>
		/// Raises event <see cref="UnseenTimeOut"/>.
		/// </summary>
		protected virtual void OnUnseenTimeOut()
		{
			if (this.UnseenTimeOut != null) this.UnseenTimeOut(this, EventArgs.Empty);
		}
		/// <summary>
		/// Raises event <see cref="UnseenTimeOut"/>.
		/// </summary>
		/// <param name="e">Data that describes the event.</param>
		protected virtual void OnCollided(CryCollision e)
		{
			if (this.Collided != null) this.Collided(this, new EventArgs<CryCollision>(e));
		}
		/// <summary>
		/// Raises event <see cref="Rendered"/>.
		/// </summary>
		/// <param name="e">Data that describes the event.</param>
		protected virtual void OnRendered(EventArgs<RenderParams?> e)
		{
			if (this.Rendered != null) this.Rendered(this, e);
		}
		/// <summary>
		/// Raises event <see cref="Rendered"/>.
		/// </summary>
		/// <param name="frameTime">Time passed since previous frame.</param>
		protected virtual void OnBeforePhysicsUpdate(float frameTime)
		{
			if (this.BeforePhysicsUpdate != null)
				this.BeforePhysicsUpdate(this, new EventArgs<float>(frameTime));
		}
		/// <summary>
		/// Raises event <see cref="LevelLoaded"/>.
		/// </summary>
		protected virtual void OnLevelLoaded()
		{
			if (this.LevelLoaded != null) this.LevelLoaded(this, EventArgs.Empty);
		}
		/// <summary>
		/// Raises event <see cref="LevelStarted"/>.
		/// </summary>
		protected virtual void OnLevelStarted()
		{
			if (this.LevelStarted != null) this.LevelStarted(this, EventArgs.Empty);
		}
		/// <summary>
		/// Raises event <see cref="GameStarted"/>.
		/// </summary>
		protected virtual void OnGameStarted()
		{
			if (this.GameStarted != null) this.GameStarted(this, EventArgs.Empty);
		}
		/// <summary>
		/// Raises event <see cref="Activated"/>.
		/// </summary>
		protected virtual void OnActivated()
		{
			if (this.Activated != null) this.Activated(this, EventArgs.Empty);
		}
		/// <summary>
		/// Raises event <see cref="Deactivated"/>.
		/// </summary>
		protected virtual void OnDeactivated()
		{
			if (this.Deactivated != null) this.Deactivated(this, EventArgs.Empty);
		}
		/// <summary>
		/// Raises event <see cref="VisibilityStateChanged"/>.
		/// </summary>
		/// <param name="visible">Indicates whether this entity has become visible.</param>
		protected virtual void OnVisibilityStateChanged(bool visible)
		{
			if (this.VisibilityStateChanged != null)
				this.VisibilityStateChanged(this, new EventArgs<bool>(visible));
		}
		/// <summary>
		/// Raises event <see cref="ActivationStateChanged"/>.
		/// </summary>
		/// <param name="activated">Indicates whether this entity was activated.</param>
		protected virtual void OnActivationStateChanged(bool activated)
		{
			if (this.ActivationStateChanged != null)
				this.ActivationStateChanged(this, new EventArgs<bool>(activated));
		}
		/// <summary>
		/// Raises event <see cref="MaterialChanged"/>.
		/// </summary>
		/// <param name="handle">Material handle.</param>
		protected virtual void OnMaterialChanged(IntPtr handle)
		{
			if (this.MaterialChanged != null)
				this.MaterialChanged(this, new EventArgs<Material>(new Material(handle)));
		}
		/// <summary>
		/// Raises event <see cref="SpawnedRemotely"/>.
		/// </summary>
		protected virtual void OnSpawnedRemotely()
		{
			if (this.SpawnedRemotely != null) this.SpawnedRemotely(this, EventArgs.Empty);
		}
		#endregion
	}
}