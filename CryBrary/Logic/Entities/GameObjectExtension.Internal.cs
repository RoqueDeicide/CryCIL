using System;
using System.Collections.Generic;
using CryEngine.Annotations;
using CryEngine.Engine.Renderer;
using CryEngine.Entities;
using CryEngine.Mathematics.MemoryMapping;
using CryEngine.Physics;
using CryEngine.RunTime.Serialization;

namespace CryEngine.Logic.Entities
{
	public partial class GameObjectExtension
	{
		#region Registry
		internal static SortedList<string, Type> EntityTypeRegistry = new SortedList<string, Type>();

		#endregion
		#region Internal Interface
		[CanBeNull]
		[UsedImplicitly]
		private static GameObjectExtension CreateWrapper(string entityTypeName, IntPtr handle, EntityId id)
		{
			// Get the type that will manage the entity.
			Type entityType;
			// Create a new managed wrapper.
			if (GameObjectExtension.EntityTypeRegistry.TryGetValue(entityTypeName, out entityType))
			{
				return (GameObjectExtension)Activator.CreateInstance(entityType, handle, id);
			}
			return null;
		}
		[UsedImplicitly]
		private void OnPreInitializedInternal()
		{
			this.OnInitializing();
		}
		[UsedImplicitly]
		private void OnPostInitializedInternal()
		{
			this.OnInitialized();
		}
		[UsedImplicitly]
		private void OnClientPreInitializedInternal(int channelId)
		{
			this.OnInitializingClient(channelId);
		}
		[UsedImplicitly]
		private void OnClientPostInitializedInternal(int channelId)
		{
			this.OnInitializedClient(channelId);
		}
		[UsedImplicitly]
		private bool ReloadInternal(EntitySpawnParameters parameters)
		{
			EntityReloadEventArgs args = new EntityReloadEventArgs
			{
				Parameters = parameters,
				DeleteOnReload = false
			};
			this.OnReloading(args);
			return !args.DeleteOnReload;
		}
		[UsedImplicitly]
		private void PostReloadInternal(EntitySpawnParameters parameters)
		{
			this.OnReloaded(parameters);
		}
		[UsedImplicitly]
		private void FullSerializeInternal(IntPtr ser)
		{
			this.FullSynchronize(new CrySerialize { Handle = ser });
		}
		[UsedImplicitly]
		private void NetSerializeInternal(IntPtr ser, int aspect, byte profile, int flags)
		{
			this.NetworkSynchronize
			(
				new CrySerialize
				{
					Handle = ser
				},
				(EntityAspects)aspect,
				profile,
				flags
			);
		}
		[UsedImplicitly]
		private void UpdateInternal(EntityUpdateContext context, int updateSlot)
		{
			EntityThinkingEventArgs args = new EntityThinkingEventArgs(context, updateSlot);

			this.OnThinking(args);
			this.Think(context, updateSlot);
			this.OnDecided(args);
		}
		[UsedImplicitly]
		private unsafe void ProcessEntityEventInternal(EntityEvent @event,
													   IntPtr parameter0, IntPtr parameter1,
													   IntPtr parameter2, IntPtr parameter3,
													   float singleParameter0,
													   float singleParameter1)
		{
			switch (@event)
			{
				case EntityEvent.XForm:
					this.OnMoved((EntityXFormFlags)parameter0.ToInt32());
					break;
				case EntityEvent.XformFinishedEditor:
					break;
				case EntityEvent.Timer:
					this.OnTimedOut(parameter0, parameter1.ToInt32());
					break;
				case EntityEvent.Initialize:
					break;
				case EntityEvent.Done:
					this.DisposeInternal();
					break;
				case EntityEvent.ReturningToPool:
					break;
				case EntityEvent.Visiblity:
					if (parameter0.ToInt32() == 0)
					{
						this.OnDisappeared();
					}
					else
					{
						this.OnAppeared();
					}
					this.OnVisibilityStateChanged(parameter0.ToInt32() != 0);
					break;
				case EntityEvent.Reset:
					this.OnResetByEditor(parameter0.ToInt32() != 0);
					break;
				case EntityEvent.Attach:
					this.OnAttached(new EntityId(new Bytes4(parameter0.ToInt32()).UnsignedInt));
					break;
				case EntityEvent.AttachThis:
					this.OnAttachedTo(new EntityId(new Bytes4(parameter0.ToInt32()).UnsignedInt));
					break;
				case EntityEvent.Detach:
					this.OnDetached(new EntityId(new Bytes4(parameter0.ToInt32()).UnsignedInt));
					break;
				case EntityEvent.DetachThis:
					this.OnDetachedFrom(new EntityId(new Bytes4(parameter0.ToInt32()).UnsignedInt));
					break;
				case EntityEvent.Link:
					this.OnLinked(parameter0);
					break;
				case EntityEvent.Delink:
					this.OnUnlinked(parameter0);
					break;
				case EntityEvent.Hide:
					this.OnHidden(true);
					break;
				case EntityEvent.Unhide:
					this.OnHidden(false);
					break;
				case EntityEvent.EnablePhysics:
					break;
				case EntityEvent.PhysicsChangeState:
					break;
				case EntityEvent.ScriptEvent:
					this.OnScriptEventBroadcasted
					(
						new EntityScriptEventEventArgs
						(
							parameter0,
							(EntityEventValueType)parameter1.ToInt32(),
							parameter2
						)
					);
					break;
				case EntityEvent.EnterArea:
					this.OnEnteredArea
					(
						new EntityAreaPositionEventArgs
						(
							new EntityId(new Bytes4(parameter0.ToInt32()).UnsignedInt),
							parameter1.ToInt32(),
							new EntityId(new Bytes4(parameter2.ToInt32()).UnsignedInt)
						)
					);
					break;
				case EntityEvent.LeaveArea:
					this.OnLeftArea
					(
						new EntityAreaPositionEventArgs
						(
							new EntityId(new Bytes4(parameter0.ToInt32()).UnsignedInt),
							parameter1.ToInt32(),
							new EntityId(new Bytes4(parameter2.ToInt32()).UnsignedInt)
						)
					);
					break;
				case EntityEvent.EnterNearArea:
					this.OnEnteredProximityArea
					(
						new EntityAreaPositionEventArgs
						(
							new EntityId(new Bytes4(parameter0.ToInt32()).UnsignedInt),
							parameter1.ToInt32(),
							new EntityId(new Bytes4(parameter2.ToInt32()).UnsignedInt)
						)
					);
					break;
				case EntityEvent.LeaveNearArea:
					this.OnLeftProximityArea
					(
						new EntityAreaPositionEventArgs
						(
							new EntityId(new Bytes4(parameter0.ToInt32()).UnsignedInt),
							parameter1.ToInt32(),
							new EntityId(new Bytes4(parameter2.ToInt32()).UnsignedInt)
						)
					);
					break;
				case EntityEvent.MoveInsideArea:
					this.OnMovedInsideArea
					(
						new EntityAreaPositionEventArgs
						(
							new EntityId(new Bytes4(parameter0.ToInt32()).UnsignedInt),
							parameter1.ToInt32(),
							new EntityId(new Bytes4(parameter2.ToInt32()).UnsignedInt)
						)
					);
					break;
				case EntityEvent.MoveNearArea:
					this.OnMovedNearArea
					(
						new EntityAreaPositionEventArgs
						(
							new EntityId(new Bytes4(parameter0.ToInt32()).UnsignedInt),
							parameter1.ToInt32(),
							new EntityId(new Bytes4(parameter2.ToInt32()).UnsignedInt),
							singleParameter0
						)
					);
					break;
				case EntityEvent.CrossArea:
					break;
				case EntityEvent.PhysicsPostStep:
					break;
				case EntityEvent.PhysicsBreak:
					break;
				case EntityEvent.AiDone:
					break;
				case EntityEvent.SoundDone:
					break;
				case EntityEvent.NotSeenTimeout:
					this.OnUnseenTimeOut();
					break;
				case EntityEvent.Collision:
					this.OnCollided(*((CryCollision*)parameter0.ToPointer()));
					break;
				case EntityEvent.Render:
					// Get rendering parameters and create a reference object that will allow handlers
					// modify the structure.
					RenderParams* renderingParameters = (RenderParams*)parameter0.ToPointer();
					RenderParams? renderParamsReference = *renderingParameters;
					this.OnRendered
					(
						new EventArgs<RenderParams?>
						(
							renderParamsReference
						)
					);
					// Pass modifications to native code, if allowed.
					if (Platform.AllowRenderParametersModification)
					{
						*renderingParameters = renderParamsReference.Value;
					}
					break;
				case EntityEvent.PrePhysicsUpdate:
					this.OnBeforePhysicsUpdate(singleParameter0);
					break;
				case EntityEvent.LevelLoaded:
					this.OnLevelLoaded();
					break;
				case EntityEvent.StartLevel:
					this.OnLevelStarted();
					break;
				case EntityEvent.StartGame:
					this.OnGameStarted();
					break;
				case EntityEvent.EnterScriptState:
					break;
				case EntityEvent.LeaveScriptState:
					break;
				case EntityEvent.PreSerialize:
					break;
				case EntityEvent.PostSerialize:
					break;
				case EntityEvent.Invisible:
					this.OnDisappeared();
					this.OnVisibilityStateChanged(false);
					break;
				case EntityEvent.Visible:
					this.OnAppeared();
					this.OnVisibilityStateChanged(true);
					break;
				case EntityEvent.Material:
					this.OnMaterialChanged(parameter0);
					break;
				case EntityEvent.MaterialLayer:
					break;
				case EntityEvent.OnHit:
					break;
				case EntityEvent.AnimationEvent:
					break;
				case EntityEvent.ScriptRequestColliderMode:
					break;
				case EntityEvent.ActiveFlowNodeOutput:
					break;
				case EntityEvent.EditorPropertyChanged:
					break;
				case EntityEvent.ReloadScript:
					break;
				case EntityEvent.Activated:
					this.OnActivated();
					this.OnActivationStateChanged(true);
					break;
				case EntityEvent.Deactivated:
					this.OnDeactivated();
					this.OnActivationStateChanged(false);
					break;
				case EntityEvent.Last:
					break;
				default:
					return;
			}
		}
		[UsedImplicitly]
		private void PostUpdateInternal(float deltaTime)
		{
			FrameTimeEventArgs args = new FrameTimeEventArgs(deltaTime);

			this.OnSecondThought(args);
			this.AfterThought(deltaTime);
			this.OnSecondDecision(args);
		}
		[UsedImplicitly]
		private void DisposeInternal()
		{
			// This method is invoked from unmanaged code, so we should not bother telling CryEngine to
			// remove this entity.
			this.Dispose(true);
		}
		[UsedImplicitly]
		private void PostRemoteSpawnInternal()
		{
			this.OnSpawnedRemotely();
		}
		#endregion
	}
}